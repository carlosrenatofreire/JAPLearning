using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Business.Models.Domains.Entities;
using JAPLearning.Business.Models.Domains.Parameters;
using JAPLearning.Mvc.ViewModels.Auxiliaries;
using JAPLearning.Mvc.ViewModels.Entities;
using JAPLearning.Mvc.ViewModels.Parameters;

namespace JAPLearning.Mvc.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                // Parameters (bidirectional)
                cfg.CreateMap<Module, ModuleViewModel>().ReverseMap();
                cfg.CreateMap<Permission, PermissionViewModel>()
                    .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.Module != null ? s.Module.Name : string.Empty))
                    .ReverseMap()
                    .ForMember(d => d.Module, o => o.Ignore())
                    .ForMember(d => d.RolePermissions, o => o.Ignore());
                cfg.CreateMap<Role, RoleViewModel>().ReverseMap();
                cfg.CreateMap<Category, CategoryViewModel>()
                    .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team != null ? s.Team.Name : string.Empty))
                    .ReverseMap()
                    .ForMember(d => d.Team, o => o.Ignore());
                cfg.CreateMap<Level, LevelViewModel>().ReverseMap();
                cfg.CreateMap<Subject, SubjectViewModel>().ReverseMap();
                cfg.CreateMap<Teacher, TeacherViewModel>().ReverseMap();
                cfg.CreateMap<Team, TeamViewModel>().ReverseMap();
                // User
                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))
                    .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team != null ? s.Team.Name : string.Empty))
                    .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                    .ForMember(d => d.Password, o => o.Ignore())
                    .ForMember(d => d.ConfirmPassword, o => o.Ignore());

                cfg.CreateMap<UserViewModel, User>()
                    .ForMember(d => d.Role, o => o.Ignore())
                    .ForMember(d => d.Team, o => o.Ignore())
                    .ForMember(d => d.Certificates, o => o.Ignore())
                    .ForMember(d => d.UserCourseLessons, o => o.Ignore())
                    .ForMember(d => d.UserLessonTests, o => o.Ignore())
                    .ForMember(d => d.Password, o => o.Ignore())
                    .ForMember(d => d.ResetToken, o => o.Ignore())
                    .ForMember(d => d.ResetTokenExpiry, o => o.Ignore());

                // Course
                cfg.CreateMap<Course, CourseViewModel>()
                    .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty))
                    .ForMember(d => d.TeamName,     o => o.MapFrom(s => s.Category != null && s.Category.Team != null ? s.Category.Team.Name : string.Empty))
                    .ForMember(d => d.TeacherName,  o => o.MapFrom(s => s.Teacher != null ? s.Teacher.Name : string.Empty))
                    .ForMember(d => d.LevelName,    o => o.MapFrom(s => s.Level != null ? s.Level.Name : string.Empty));

                cfg.CreateMap<CourseViewModel, Course>()
                    .ForMember(d => d.Category, o => o.Ignore())
                    .ForMember(d => d.Teacher, o => o.Ignore())
                    .ForMember(d => d.Level, o => o.Ignore())
                    .ForMember(d => d.Topics, o => o.Ignore())
                    .ForMember(d => d.Lessons, o => o.Ignore())
                    .ForMember(d => d.Certificates, o => o.Ignore())
                    .ForMember(d => d.Requirements, o => o.Ignore());

                // Topic
                cfg.CreateMap<Topic, TopicViewModel>()
                    .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course != null ? s.Course.Title : string.Empty))
                    .ForMember(d => d.TeamName,   o => o.MapFrom(s => s.Course != null && s.Course.Category != null && s.Course.Category.Team != null ? s.Course.Category.Team.Name : string.Empty));

                cfg.CreateMap<TopicViewModel, Topic>()
                    .ForMember(d => d.Course, o => o.Ignore())
                    .ForMember(d => d.Lessons, o => o.Ignore());

                // Lesson
                cfg.CreateMap<Lesson, LessonViewModel>()
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course != null ? s.Course.Title : string.Empty))
                    .ForMember(d => d.TeamName,    o => o.MapFrom(s => s.Course != null && s.Course.Category != null && s.Course.Category.Team != null ? s.Course.Category.Team.Name : string.Empty))
                    .ForMember(d => d.TopicName,   o => o.MapFrom(s => s.Topic != null ? s.Topic.Name : string.Empty));

                cfg.CreateMap<LessonViewModel, Lesson>()
                    .ForMember(d => d.Course, o => o.Ignore())
                    .ForMember(d => d.Topic, o => o.Ignore())
                    .ForMember(d => d.Questions, o => o.Ignore())
                    .ForMember(d => d.UserCourseLessons, o => o.Ignore())
                    .ForMember(d => d.UserLessonTests, o => o.Ignore());

                // Article
                cfg.CreateMap<Article, ArticleViewModel>()
                    .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name));

                cfg.CreateMap<ArticleViewModel, Article>()
                    .ForMember(d => d.Subject, o => o.Ignore());

                // Certificate
                cfg.CreateMap<Certificate, CertificateViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title));

                cfg.CreateMap<CertificateViewModel, Certificate>()
                    .ForMember(d => d.User, o => o.Ignore())
                    .ForMember(d => d.Course, o => o.Ignore());

                // Question
                cfg.CreateMap<Question, QuestionViewModel>()
                    .ForMember(d => d.LessonName, o => o.MapFrom(s => s.Lesson != null ? s.Lesson.Name : string.Empty))
                    .ForMember(d => d.CourseName,  o => o.MapFrom(s => s.Lesson != null && s.Lesson.Course != null ? s.Lesson.Course.Title : string.Empty))
                    .ForMember(d => d.TeamName,    o => o.MapFrom(s => s.Lesson != null && s.Lesson.Course != null && s.Lesson.Course.Category != null && s.Lesson.Course.Category.Team != null ? s.Lesson.Course.Category.Team.Name : string.Empty));

                cfg.CreateMap<QuestionViewModel, Question>()
                    .ForMember(d => d.Lesson, o => o.Ignore())
                    .ForMember(d => d.Options, o => o.Ignore())
                    .ForMember(d => d.UserLessonQuestions, o => o.Ignore());

                // QuestionOption
                cfg.CreateMap<QuestionOption, QuestionOptionViewModel>()
                    .ForMember(d => d.QuestionName, o => o.MapFrom(s => s.Question != null ? s.Question.Name : string.Empty))
                    .ForMember(d => d.LessonName,   o => o.MapFrom(s => s.Question != null && s.Question.Lesson != null ? s.Question.Lesson.Name : string.Empty))
                    .ForMember(d => d.CourseName,   o => o.MapFrom(s => s.Question != null && s.Question.Lesson != null && s.Question.Lesson.Course != null ? s.Question.Lesson.Course.Title : string.Empty))
                    .ForMember(d => d.TeamName,     o => o.MapFrom(s => s.Question != null && s.Question.Lesson != null && s.Question.Lesson.Course != null && s.Question.Lesson.Course.Category != null && s.Question.Lesson.Course.Category.Team != null ? s.Question.Lesson.Course.Category.Team.Name : string.Empty));
                cfg.CreateMap<QuestionOptionViewModel, QuestionOption>()
                    .ForMember(d => d.Question, o => o.Ignore())
                    .ForMember(d => d.UserLessonQuestions, o => o.Ignore());

                // Testimonial
                cfg.CreateMap<Testimonial, TestimonialViewModel>().ReverseMap()
                    .ForMember(d => d.User, o => o.Ignore());

                // AuditLog
                cfg.CreateMap<AuditLog, AuditLogViewModel>();
            });

            return services;
        }
    }
}
