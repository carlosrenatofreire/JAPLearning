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
                cfg.CreateMap<Role, RoleViewModel>().ReverseMap();
                cfg.CreateMap<Category, CategoryViewModel>().ReverseMap();
                cfg.CreateMap<Level, LevelViewModel>().ReverseMap();
                cfg.CreateMap<Subject, SubjectViewModel>().ReverseMap();
                cfg.CreateMap<Teacher, TeacherViewModel>().ReverseMap();
                cfg.CreateMap<OrderStatus, OrderStatusViewModel>().ReverseMap();

                // User
                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.Name : string.Empty))
                    .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                    .ForMember(d => d.Password, o => o.Ignore())
                    .ForMember(d => d.ConfirmPassword, o => o.Ignore());

                cfg.CreateMap<UserViewModel, User>()
                    .ForMember(d => d.Role, o => o.Ignore())
                    .ForMember(d => d.Orders, o => o.Ignore())
                    .ForMember(d => d.Payments, o => o.Ignore())
                    .ForMember(d => d.Certificates, o => o.Ignore())
                    .ForMember(d => d.UserCourseLessons, o => o.Ignore())
                    .ForMember(d => d.UserLessonTests, o => o.Ignore())
                    .ForMember(d => d.Password, o => o.Ignore())
                    .ForMember(d => d.ResetToken, o => o.Ignore())
                    .ForMember(d => d.ResetTokenExpiry, o => o.Ignore());

                // Course
                cfg.CreateMap<Course, CourseViewModel>()
                    .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                    .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher.Name))
                    .ForMember(d => d.LevelName, o => o.MapFrom(s => s.Level.Name));

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
                    .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.Title));

                cfg.CreateMap<TopicViewModel, Topic>()
                    .ForMember(d => d.Course, o => o.Ignore())
                    .ForMember(d => d.Lessons, o => o.Ignore());

                // Lesson
                cfg.CreateMap<Lesson, LessonViewModel>()
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title))
                    .ForMember(d => d.TopicName, o => o.MapFrom(s => s.Topic.Name));

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

                // Plan (bidirectional)
                cfg.CreateMap<Plan, PlanViewModel>().ReverseMap()
                    .ForMember(d => d.Orders, o => o.Ignore());

                // Certificate
                cfg.CreateMap<Certificate, CertificateViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title));

                cfg.CreateMap<CertificateViewModel, Certificate>()
                    .ForMember(d => d.User, o => o.Ignore())
                    .ForMember(d => d.Course, o => o.Ignore());

                // Question
                cfg.CreateMap<Question, QuestionViewModel>()
                    .ForMember(d => d.LessonName, o => o.MapFrom(s => s.Lesson.Name));

                cfg.CreateMap<QuestionViewModel, Question>()
                    .ForMember(d => d.Lesson, o => o.Ignore())
                    .ForMember(d => d.Options, o => o.Ignore())
                    .ForMember(d => d.UserLessonQuestions, o => o.Ignore());

                // QuestionOption
                cfg.CreateMap<QuestionOption, QuestionOptionViewModel>();
                cfg.CreateMap<QuestionOptionViewModel, QuestionOption>()
                    .ForMember(d => d.Question, o => o.Ignore())
                    .ForMember(d => d.UserLessonQuestions, o => o.Ignore());

                // Testimonial
                cfg.CreateMap<Testimonial, TestimonialViewModel>().ReverseMap()
                    .ForMember(d => d.User, o => o.Ignore());

                // Plan — handled above with reverse mapping

                // Order
                cfg.CreateMap<Order, OrderViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                    .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                    .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.Name));

                cfg.CreateMap<OrderViewModel, Order>()
                    .ForMember(d => d.User, o => o.Ignore())
                    .ForMember(d => d.Plan, o => o.Ignore())
                    .ForMember(d => d.Status, o => o.Ignore())
                    .ForMember(d => d.Payments, o => o.Ignore());

                // Payment
                cfg.CreateMap<Payment, PaymentViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));

                cfg.CreateMap<PaymentViewModel, Payment>()
                    .ForMember(d => d.User, o => o.Ignore())
                    .ForMember(d => d.Order, o => o.Ignore());

                // AuditLog
                cfg.CreateMap<AuditLog, AuditLogViewModel>();
            });

            return services;
        }
    }
}
