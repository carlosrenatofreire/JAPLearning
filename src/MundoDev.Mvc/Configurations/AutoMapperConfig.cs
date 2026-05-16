using MundoDev.Business.Models.Domains.Auxiliaries;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Mvc.ViewModels.Auxiliaries;
using MundoDev.Mvc.ViewModels.Entities;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                // Parameters
                cfg.CreateMap<Role, RoleViewModel>();
                cfg.CreateMap<Category, CategoryViewModel>();
                cfg.CreateMap<Level, LevelViewModel>();
                cfg.CreateMap<Subject, SubjectViewModel>();
                cfg.CreateMap<Teacher, TeacherViewModel>();
                cfg.CreateMap<OrderStatus, OrderStatusViewModel>();

                // User
                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
                    .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));

                // Course
                cfg.CreateMap<Course, CourseViewModel>()
                    .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                    .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher.Name))
                    .ForMember(d => d.LevelName, o => o.MapFrom(s => s.Level.Name));

                // Topic
                cfg.CreateMap<Topic, TopicViewModel>()
                    .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.Title));

                // Lesson
                cfg.CreateMap<Lesson, LessonViewModel>()
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title))
                    .ForMember(d => d.TopicName, o => o.MapFrom(s => s.Topic.Name));

                // Article
                cfg.CreateMap<Article, ArticleViewModel>()
                    .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name));

                // Certificate
                cfg.CreateMap<Certificate, CertificateViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                    .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title));

                // Question
                cfg.CreateMap<Question, QuestionViewModel>()
                    .ForMember(d => d.LessonName, o => o.MapFrom(s => s.Lesson.Name));

                // QuestionOption
                cfg.CreateMap<QuestionOption, QuestionOptionViewModel>();

                // Testimonial
                cfg.CreateMap<Testimonial, TestimonialViewModel>();

                // Plan
                cfg.CreateMap<Plan, PlanViewModel>();

                // Order
                cfg.CreateMap<Order, OrderViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                    .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                    .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.Name));

                // Payment
                cfg.CreateMap<Payment, PaymentViewModel>()
                    .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));

                // AuditLog
                cfg.CreateMap<AuditLog, AuditLogViewModel>();
            });

            return services;
        }
    }
}
