using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Interfaces.Services.Relationships;
using MundoDev.Mvc.ViewModels.Entities;
using MundoDev.Mvc.ViewModels.Parameters;
using MundoDev.Mvc.ViewModels.Student;
using System.Security.Claims;

namespace MundoDev.Mvc.Controllers
{
    [Authorize]
    public class StudentController : BaseController
    {
        private readonly IUserService               _userService;
        private readonly ICourseService             _courseService;
        private readonly ITopicService              _topicService;
        private readonly ILessonService             _lessonService;
        private readonly ICertificateService        _certificateService;
        private readonly IOrderService              _orderService;
        private readonly IUserCourseLessonService   _userCourseLessonService;
        private readonly ICategoryService           _categoryService;
        private readonly IMapper                    _mapper;

        public StudentController(
            IUserService userService,
            ICourseService courseService,
            ITopicService topicService,
            ILessonService lessonService,
            ICertificateService certificateService,
            IOrderService orderService,
            IUserCourseLessonService userCourseLessonService,
            ICategoryService categoryService,
            IMapper mapper,
            INotificator notificator) : base(notificator)
        {
            _userService             = userService;
            _courseService           = courseService;
            _topicService            = topicService;
            _lessonService           = lessonService;
            _certificateService      = certificateService;
            _orderService            = orderService;
            _userCourseLessonService = userCourseLessonService;
            _categoryService         = categoryService;
            _mapper                  = mapper;
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private Guid GetCurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(idStr, out var id) ? id : Guid.Empty;
        }

        /// <summary>
        /// Extracts a YouTube video ID from a full URL or returns the raw value
        /// if it's already just an ID. Supports:
        ///   https://www.youtube.com/watch?v=VIDEO_ID
        ///   https://youtu.be/VIDEO_ID
        ///   https://www.youtube.com/embed/VIDEO_ID
        /// </summary>
        private static string? ExtractYouTubeEmbedUrl(string? videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl)) return null;

            string? videoId = null;

            if (videoUrl.Contains("youtube.com/watch", StringComparison.OrdinalIgnoreCase))
            {
                var uri     = new Uri(videoUrl);
                var queries = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
                videoId     = queries.TryGetValue("v", out var vVal) ? vVal.ToString() : null;
            }
            else if (videoUrl.Contains("youtu.be/", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(videoUrl);
                videoId = uri.AbsolutePath.TrimStart('/');
            }
            else if (videoUrl.Contains("youtube.com/embed/", StringComparison.OrdinalIgnoreCase))
            {
                var uri = new Uri(videoUrl);
                videoId = uri.AbsolutePath.Replace("/embed/", "");
            }
            else if (!videoUrl.Contains('/') && !videoUrl.Contains('.'))
            {
                // Assume it's already a raw video ID
                videoId = videoUrl;
            }

            return string.IsNullOrEmpty(videoId)
                ? null
                : $"https://www.youtube.com/embed/{videoId}";
        }

        // ─── Dashboard ───────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            ViewData["ActiveMenu"] = "dashboard";

            var userId       = GetCurrentUserId();
            var user         = await _userService.GetByIdAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            var userProgress = await _userCourseLessonService.GetByUserAsync(userId);
            var allLessons   = await _lessonService.GetAllAsync();
            var allCourses   = await _courseService.GetAllAsync();
            var certificates = await _certificateService.GetAllAsync();
            var userCerts    = certificates.Where(c => c.UserId == userId).ToList();
            var userOrders   = (await _orderService.GetAllAsync())
                                   .Where(o => o.UserId == userId)
                                   .OrderByDescending(o => o.CreatedDate)
                                   .ToList();

            // Build per-course progress
            var completedLessonIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();

            var courseProgressList = new List<CourseProgressViewModel>();

            foreach (var course in allCourses.Where(c => c.IsActived))
            {
                var courseLessons = allLessons.Where(l => l.CourseId == course.Id).ToList();
                if (!courseLessons.Any()) continue;

                var completed = courseLessons.Count(l => completedLessonIds.Contains(l.Id));
                if (completed == 0) continue; // not started

                // Last lesson in this course the user interacted with
                var lastRecord = userProgress
                    .Where(x => courseLessons.Any(l => l.Id == x.LessonId))
                    .OrderByDescending(x => x.CompletedDate)
                    .FirstOrDefault();

                var lastLesson = lastRecord != null
                    ? courseLessons.FirstOrDefault(l => l.Id == lastRecord.LessonId)
                    : null;

                courseProgressList.Add(new CourseProgressViewModel
                {
                    Course           = _mapper.Map<CourseViewModel>(course),
                    CompletedLessons = completed,
                    TotalLessons     = courseLessons.Count,
                    LastLessonId     = lastLesson?.Id,
                    LastTopicName    = lastLesson?.Topic?.Name
                });
            }

            var activeOrder = userOrders.FirstOrDefault(o => o.IsActived);

            var vm = new StudentDashboardViewModel
            {
                FirstName           = user.FirstName,
                CoursesInProgress   = courseProgressList.Count(c => !c.IsCompleted),
                CoursesCompleted    = courseProgressList.Count(c => c.IsCompleted),
                CertificatesCount   = userCerts.Count,
                TotalWatchedSeconds = userProgress.Sum(x => x.WatchedSeconds ?? 0),
                InProgressCourses   = courseProgressList.Where(c => !c.IsCompleted).Take(5).ToList(),
                ActiveOrder         = activeOrder != null ? _mapper.Map<OrderViewModel>(activeOrder) : null
            };

            return View(vm);
        }

        // ─── My Courses ──────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            ViewData["ActiveMenu"] = "my-courses";
            ViewData["Title"]      = "Meus Cursos";

            var userId       = GetCurrentUserId();
            var userProgress = await _userCourseLessonService.GetByUserAsync(userId);
            var allLessons   = await _lessonService.GetAllAsync();
            var allCourses   = await _courseService.GetAllAsync();

            var completedLessonIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();

            var progressList = new List<CourseProgressViewModel>();

            foreach (var course in allCourses.Where(c => c.IsActived))
            {
                var courseLessons = allLessons.Where(l => l.CourseId == course.Id).ToList();
                if (!courseLessons.Any()) continue;

                var completed = courseLessons.Count(l => completedLessonIds.Contains(l.Id));
                if (completed == 0) continue;

                var lastRecord = userProgress
                    .Where(x => courseLessons.Any(l => l.Id == x.LessonId))
                    .OrderByDescending(x => x.CompletedDate)
                    .FirstOrDefault();

                var lastLesson = lastRecord != null
                    ? courseLessons.FirstOrDefault(l => l.Id == lastRecord.LessonId)
                    : null;

                progressList.Add(new CourseProgressViewModel
                {
                    Course           = _mapper.Map<CourseViewModel>(course),
                    CompletedLessons = completed,
                    TotalLessons     = courseLessons.Count,
                    LastLessonId     = lastLesson?.Id,
                    LastTopicName    = lastLesson?.Topic?.Name
                });
            }

            return View(progressList);
        }

        // ─── Player ──────────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Player(Guid courseId, Guid? lessonId)
        {
            var userId  = GetCurrentUserId();
            var course  = await _courseService.GetByIdAsync(courseId);
            if (course == null) return NotFound();

            var allTopics  = await _topicService.GetAllAsync();
            var allLessons = await _lessonService.GetAllAsync();

            var courseTopics  = allTopics
                .Where(t => t.CourseId == courseId && t.IsActived)
                .OrderBy(t => t.Order)
                .ToList();
            var courseLessons = allLessons
                .Where(l => l.CourseId == courseId && l.IsActived)
                .OrderBy(l => l.Order)
                .ToList();

            if (!courseLessons.Any()) return NotFound();

            // Determine current lesson
            var currentLessonEntity = lessonId.HasValue
                ? courseLessons.FirstOrDefault(l => l.Id == lessonId.Value)
                : null;
            currentLessonEntity ??= courseLessons.First();

            // User progress
            var userProgress       = await _userCourseLessonService.GetByUserAsync(userId);
            var completedLessonIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();

            int completedCount = courseLessons.Count(l => completedLessonIds.Contains(l.Id));
            int progressPct    = courseLessons.Count == 0 ? 0
                : (int)Math.Round((double)completedCount / courseLessons.Count * 100);

            // Build topics with lessons
            var topicGroups = courseTopics.Select(t => new TopicWithLessonsViewModel
            {
                Topic   = _mapper.Map<TopicViewModel>(t),
                Lessons = courseLessons
                    .Where(l => l.TopicId == t.Id)
                    .Select(l => _mapper.Map<LessonViewModel>(l))
                    .ToList()
            }).ToList();

            // Lessons without a topic
            var topicIds           = courseTopics.Select(t => t.Id).ToHashSet();
            var lessonsWithoutTopic = courseLessons.Where(l => !topicIds.Contains(l.TopicId)).ToList();
            if (lessonsWithoutTopic.Any())
            {
                topicGroups.Insert(0, new TopicWithLessonsViewModel
                {
                    Topic   = new TopicViewModel { Name = "Lições" },
                    Lessons = lessonsWithoutTopic.Select(l => _mapper.Map<LessonViewModel>(l)).ToList()
                });
            }

            var currentLessonVm = _mapper.Map<LessonViewModel>(currentLessonEntity);
            var embedUrl        = ExtractYouTubeEmbedUrl(currentLessonEntity.Video);

            var vm = new PlayerViewModel
            {
                Course              = _mapper.Map<CourseViewModel>(course),
                CurrentLesson       = currentLessonVm,
                EmbedUrl            = embedUrl ?? string.Empty,
                Topics              = topicGroups,
                CompletedLessonIds  = completedLessonIds,
                ProgressPercent     = progressPct
            };

            ViewData["Title"]      = currentLessonEntity.Name;
            ViewData["CourseName"] = course.Title;
            ViewData["Progress"]   = progressPct.ToString();

            return View(vm);
        }

        // ─── Complete Lesson (AJAX POST) ─────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteLesson(Guid lessonId, Guid courseId)
        {
            var userId = GetCurrentUserId();
            await _userCourseLessonService.MarkCompletedAsync(userId, lessonId);

            // Find next lesson in course
            var allLessons = await _lessonService.GetAllAsync();
            var courseLessons = allLessons
                .Where(l => l.CourseId == courseId && l.IsActived)
                .OrderBy(l => l.Order)
                .ToList();

            var currentIdx = courseLessons.FindIndex(l => l.Id == lessonId);
            var nextLesson = currentIdx >= 0 && currentIdx + 1 < courseLessons.Count
                ? courseLessons[currentIdx + 1]
                : null;

            if (nextLesson != null)
                return RedirectToAction(nameof(Player), new { courseId, lessonId = nextLesson.Id });

            TempData["Success"] = "Parabéns! Você concluiu todas as aulas deste curso.";
            return RedirectToAction(nameof(Player), new { courseId, lessonId });
        }

        // ─── My Certificates ─────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> MyCertificates()
        {
            ViewData["ActiveMenu"] = "certificates";
            ViewData["Title"]      = "Meus Certificados";

            var userId       = GetCurrentUserId();
            var certificates = await _certificateService.GetAllAsync();
            var userCerts    = certificates
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CompletedDate)
                .ToList();

            var vm = userCerts.Select(c => _mapper.Map<CertificateViewModel>(c)).ToList();
            return View(vm);
        }

        // ─── My Orders ───────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            ViewData["ActiveMenu"] = "my-orders";
            ViewData["Title"]      = "Meus Pedidos";

            var userId = GetCurrentUserId();
            var orders = (await _orderService.GetAllAsync())
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedDate)
                .ToList();

            var vm = orders.Select(o => _mapper.Map<OrderViewModel>(o)).ToList();
            return View(vm);
        }

        // ─── Personal Data ───────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            ViewData["ActiveMenu"] = "profile";
            ViewData["Title"]      = "Dados Pessoais";

            var userId = GetCurrentUserId();
            var user   = await _userService.GetByIdAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            var vm = new PersonalDataViewModel
            {
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Email     = user.Email,
                Phone     = user.PhoneNumber
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalData(PersonalDataViewModel model)
        {
            ViewData["ActiveMenu"] = "profile";
            ViewData["Title"]      = "Dados Pessoais";

            if (!ModelState.IsValid) return View(model);

            var userId = GetCurrentUserId();
            var user   = await _userService.GetByIdAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            user.FirstName   = model.FirstName;
            user.LastName    = model.LastName;
            user.Email       = model.Email;
            user.PhoneNumber = model.Phone;
            user.ChangedDate = DateTime.UtcNow;

            await _userService.UpdateAsync(user);

            if (!IsOperationValid())
            {
                AddErrors();
                return View(model);
            }

            // Refresh name claim in cookie
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, User.FindFirstValue(ClaimTypes.Role) ?? "")
            };
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            TempData["Success"] = "Dados actualizados com sucesso.";
            return RedirectToAction(nameof(PersonalData));
        }

        // ─── Course Catalog ──────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Courses(string? category = null)
        {
            ViewData["ActiveMenu"] = "courses";
            ViewData["Title"]      = "Cursos";

            var userId       = GetCurrentUserId();
            var allCourses   = await _courseService.GetAllAsync();
            var allTopics    = await _topicService.GetAllAsync();
            var allLessons   = await _lessonService.GetAllAsync();
            var userProgress = await _userCourseLessonService.GetByUserAsync(userId);
            var allCategories = await _categoryService.GetAllAsync();

            var completedLessonIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();

            var items = allCourses
                .Where(c => c.IsActived)
                .OrderBy(c => c.Title)
                .Select(c =>
                {
                    var courseLessons = allLessons.Where(l => l.CourseId == c.Id && l.IsActived).ToList();
                    var courseTopics  = allTopics.Where(t => t.CourseId == c.Id && t.IsActived).ToList();
                    var completed     = courseLessons.Count(l => completedLessonIds.Contains(l.Id));

                    var lastRecord = userProgress
                        .Where(x => courseLessons.Any(l => l.Id == x.LessonId))
                        .OrderByDescending(x => x.CompletedDate)
                        .FirstOrDefault();

                    var totalDuration = courseLessons
                        .Where(l => l.TimeLesson.HasValue)
                        .Aggregate(TimeSpan.Zero, (sum, l) => sum + l.TimeLesson!.Value);

                    return new CourseCatalogItemViewModel
                    {
                        Course           = _mapper.Map<CourseViewModel>(c),
                        TotalLessons     = courseLessons.Count,
                        TotalTopics      = courseTopics.Count,
                        CompletedLessons = completed,
                        LastLessonId     = lastRecord?.LessonId,
                        TotalDuration    = totalDuration
                    };
                }).ToList();

            var vm = new CourseCatalogViewModel
            {
                Courses    = items,
                Categories = allCategories.Where(c => c.IsActived)
                                          .Select(c => _mapper.Map<CategoryViewModel>(c))
                                          .OrderBy(c => c.Name)
                                          .ToList()
            };

            ViewBag.ActiveCategory = category;
            return View(vm);
        }

        // ─── Categories ───────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            ViewData["ActiveMenu"] = "categories";
            ViewData["Title"]      = "Categorias";

            var categories = await _categoryService.GetAllAsync();
            var allCourses = await _courseService.GetAllAsync();

            var vm = categories
                .Where(c => c.IsActived)
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    Category    = _mapper.Map<CategoryViewModel>(c),
                    CourseCount = allCourses.Count(x => x.CategoryId == c.Id && x.IsActived)
                })
                .ToList();

            ViewBag.CategoryItems = vm;
            return View();
        }

        // ─── Change Password ─────────────────────────────────────────────────

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ViewData["ActiveMenu"] = "profile";
            ViewData["Title"]      = "Alterar Senha";
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ViewData["ActiveMenu"] = "profile";
            ViewData["Title"]      = "Alterar Senha";

            if (!ModelState.IsValid) return View(model);

            var userId = GetCurrentUserId();
            var user   = await _userService.GetByIdAsync(userId);
            if (user == null) return RedirectToAction("Login", "Account");

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Senha actual incorrecta.");
                return View(model);
            }

            user.Password    = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            user.ChangedDate = DateTime.UtcNow;
            await _userService.UpdateAsync(user);

            if (!IsOperationValid())
            {
                AddErrors();
                return View(model);
            }

            TempData["Success"] = "Senha alterada com sucesso.";
            return RedirectToAction(nameof(PersonalData));
        }
    }
}
