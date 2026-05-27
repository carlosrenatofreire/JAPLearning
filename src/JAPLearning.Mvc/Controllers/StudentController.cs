using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Domains.Relationships;
using JAPLearning.Mvc.ViewModels.Entities;
using JAPLearning.Mvc.ViewModels.Parameters;
using JAPLearning.Mvc.ViewModels.Student;
using System.Security.Claims;

namespace JAPLearning.Mvc.Controllers
{
    [Authorize]
    public class StudentController : BaseController
    {
        private readonly IUserService               _userService;
        private readonly ICourseService             _courseService;
        private readonly ITopicService              _topicService;
        private readonly ILessonService             _lessonService;
        private readonly IQuestionService           _questionService;
        private readonly ICertificateService        _certificateService;
        private readonly IUserCourseLessonService   _userCourseLessonService;
        private readonly IUserLessonTestService     _userLessonTestService;
        private readonly ICategoryService           _categoryService;
        private readonly ICloudinaryService         _cloudinary;
        private readonly IMapper                    _mapper;

        public StudentController(
            IUserService userService,
            ICourseService courseService,
            ITopicService topicService,
            ILessonService lessonService,
            IQuestionService questionService,
            ICertificateService certificateService,
            IUserCourseLessonService userCourseLessonService,
            IUserLessonTestService userLessonTestService,
            ICategoryService categoryService,
            ICloudinaryService cloudinary,
            IMapper mapper,
            INotificator notificator) : base(notificator)
        {
            _userService             = userService;
            _courseService           = courseService;
            _topicService            = topicService;
            _lessonService           = lessonService;
            _questionService         = questionService;
            _certificateService      = certificateService;
            _userCourseLessonService = userCourseLessonService;
            _userLessonTestService   = userLessonTestService;
            _categoryService         = categoryService;
            _cloudinary              = cloudinary;
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

            var vm = new StudentDashboardViewModel
            {
                FirstName           = user.FirstName,
                CoursesInProgress   = courseProgressList.Count(c => !c.IsCompleted),
                CoursesCompleted    = courseProgressList.Count(c => c.IsCompleted),
                CertificatesCount   = userCerts.Count,
                TotalWatchedSeconds = userProgress.Sum(x => x.WatchedSeconds ?? 0),
                InProgressCourses   = courseProgressList.Where(c => !c.IsCompleted).Take(5).ToList()
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
            if (course.IsBrief) return RedirectToAction(nameof(Courses));

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

            // Load questions + options for current lesson
            var questions   = await _questionService.GetByLessonAsync(currentLessonEntity.Id);
            var questionVms = _mapper.Map<List<QuestionViewModel>>(questions);

            // Load quiz attempt history for current lesson
            var allTests    = await _userLessonTestService.GetByUserAsync(userId);
            var lessonTests = allTests
                .Where(t => t.LessonId == currentLessonEntity.Id && t.Score.HasValue)
                .ToList();
            int? bestScore = lessonTests.Any() ? lessonTests.Max(t => t.Score!.Value) : null;

            var vm = new PlayerViewModel
            {
                Course              = _mapper.Map<CourseViewModel>(course),
                CurrentLesson       = currentLessonVm,
                EmbedUrl            = embedUrl ?? string.Empty,
                Topics              = topicGroups,
                CompletedLessonIds  = completedLessonIds,
                ProgressPercent     = progressPct,
                PassingScore        = course.PassingScore,
                Questions           = questionVms,
                HasAttemptedQuiz    = lessonTests.Any(),
                BestQuizScore       = bestScore
            };

            ViewData["Title"]      = currentLessonEntity.Name;
            ViewData["CourseName"] = course.Title;
            ViewData["Progress"]   = progressPct.ToString();

            return View(vm);
        }

        // ─── Save Quiz Result (AJAX POST) ────────────────────────────────────

        public class SaveQuizResultRequest
        {
            public Guid LessonId { get; set; }
            public int  CorrectAnswers { get; set; }
            public int  TotalQuestions { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuizResult([FromBody] SaveQuizResultRequest req)
        {
            if (req == null || req.TotalQuestions == 0)
                return Json(new { success = false });

            var userId = GetCurrentUserId();

            // Compute score
            int score  = (int)Math.Round((double)req.CorrectAnswers / req.TotalQuestions * 100);

            // Determine attempt number
            var latest = await _userLessonTestService.GetLatestByUserAndLessonAsync(userId, req.LessonId);
            int attempt = latest == null ? 1 : latest.AttemptNumber + 1;

            var test = new UserLessonTest
            {
                UserId        = userId,
                LessonId      = req.LessonId,
                Score         = score,
                Passed        = false,           // will be determined against course PassingScore at completion
                AttemptNumber = attempt,
                CompletedDate = DateTime.UtcNow
            };

            await _userLessonTestService.AddAsync(test);
            return Json(new { success = true, score });
        }

        // ─── Complete Lesson ─────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteLesson(Guid lessonId, Guid courseId)
        {
            var userId = GetCurrentUserId();
            await _userCourseLessonService.MarkCompletedAsync(userId, lessonId);

            var course = await _courseService.GetByIdAsync(courseId);
            var allLessons = await _lessonService.GetAllAsync();
            var courseLessons = allLessons
                .Where(l => l.CourseId == courseId && l.IsActived)
                .OrderBy(l => l.Order)
                .ToList();

            // ── Check course completion + certificate eligibility ──────────
            var userProgress = await _userCourseLessonService.GetByUserAsync(userId);
            var completedIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();
            completedIds.Add(lessonId); // include the one just marked

            bool allDone = courseLessons.All(l => completedIds.Contains(l.Id));

            if (allDone && course != null && !await _certificateService.HasCertificateAsync(userId, courseId))
            {
                // Best score per lesson that has at least one quiz attempt
                var allTests    = await _userLessonTestService.GetByUserAsync(userId);
                var lessonIds   = courseLessons.Select(l => l.Id).ToHashSet();
                var courseTests = allTests
                    .Where(t => lessonIds.Contains(t.LessonId) && t.Score.HasValue)
                    .GroupBy(t => t.LessonId)
                    .Select(g => g.Max(t => t.Score!.Value))
                    .ToList();

                // If no quiz at all → 100 %; otherwise average of best scores per lesson
                int avgScore = courseTests.Any()
                    ? (int)Math.Round(courseTests.Average())
                    : 100;

                if (avgScore >= (course.PassingScore > 0 ? course.PassingScore : 60))
                {
                    await _certificateService.IssueCertificateAsync(userId, courseId, avgScore);
                    TempData["CertificateIssued"] = "true";
                    TempData["Success"] = $"🏆 Parabéns! Concluiu a formação com {avgScore}% de aproveitamento. O seu certificado está disponível em Meus Certificados!";
                }
                else
                {
                    TempData["Warning"] = $"Concluiu todas as aulas, mas a sua média ({avgScore}%) é inferior ao mínimo exigido ({course.PassingScore}%). Repita os testes para melhorar a nota.";
                }
            }

            // ── Navigate to next lesson (unless certificate was just issued) ─
            var currentIdx = courseLessons.FindIndex(l => l.Id == lessonId);
            var nextLesson = currentIdx >= 0 && currentIdx + 1 < courseLessons.Count
                ? courseLessons[currentIdx + 1]
                : null;

            if (nextLesson != null && TempData.Peek("CertificateIssued") == null)
                return RedirectToAction(nameof(Player), new { courseId, lessonId = nextLesson.Id });

            return RedirectToAction(nameof(Player), new { courseId, lessonId });
        }

        // ─── Certificate View (printable) ────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> CertificateView(Guid id)
        {
            var cert = await _certificateService.GetByIdAsync(id);
            if (cert == null) return NotFound();

            var userId = GetCurrentUserId();
            if (cert.UserId != userId && !User.IsInRole("Administrador"))
                return Forbid();

            var vm = _mapper.Map<CertificateViewModel>(cert);
            return View(vm);
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
                FirstName       = user.FirstName,
                LastName        = user.LastName,
                Email           = user.Email,
                Phone           = user.PhoneNumber,
                CurrentPhotoUrl = user.PhotoUrl
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalData(PersonalDataViewModel model, IFormFile? photo)
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

            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(user.PhotoUrl))
                {
                    var oldId = _cloudinary.ExtractPublicId(user.PhotoUrl);
                    if (oldId != null) await _cloudinary.DeleteImageAsync(oldId);
                }
                user.PhotoUrl = await _cloudinary.UploadImageAsync(photo, "users");
            }

            await _userService.UpdateAsync(user);

            if (!IsOperationValid())
            {
                AddErrors();
                return View(model);
            }

            // Refresh claims in cookie (including updated PhotoUrl)
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, User.FindFirstValue(ClaimTypes.Role) ?? ""),
                new("PhotoUrl", user.PhotoUrl ?? string.Empty)
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

            var userId        = GetCurrentUserId();
            var user          = await _userService.GetByIdAsync(userId);
            var allCourses    = await _courseService.GetAllAsync();
            var allTopics     = await _topicService.GetAllAsync();
            var allLessons    = await _lessonService.GetAllAsync();
            var userProgress  = await _userCourseLessonService.GetByUserAsync(userId);
            var allCategories = await _categoryService.GetAllAsync();

            // Filtrar categorias pela equipa do aluno.
            // Administradores vêem tudo.
            var isAdmin = User.IsInRole("Administrador");
            var teamCategories = allCategories
                .Where(c => c.IsActived && (isAdmin || c.TeamId == user?.TeamId))
                .ToList();

            var teamCategoryIds = teamCategories.Select(c => c.Id).ToHashSet();

            var completedLessonIds = userProgress
                .Where(x => x.CompletedDate.HasValue)
                .Select(x => x.LessonId)
                .ToHashSet();

            var items = allCourses
                .Where(c => c.IsActived && teamCategoryIds.Contains(c.CategoryId))
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
                Categories = teamCategories
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
