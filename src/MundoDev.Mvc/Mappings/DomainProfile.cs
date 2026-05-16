using AutoMapper;
using MundoDev.Business.Models.Domains.Auxiliaries;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Mvc.ViewModels.Auxiliaries;
using MundoDev.Mvc.ViewModels.Entities;
using MundoDev.Mvc.ViewModels.Parameters;

namespace MundoDev.Mvc.Mappings
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            // Parameters
            CreateMap<Role, RoleViewModel>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<Level, LevelViewModel>();
            CreateMap<Subject, SubjectViewModel>();
            CreateMap<Teacher, TeacherViewModel>();
            CreateMap<OrderStatus, OrderStatusViewModel>();

            // User
            CreateMap<User, UserViewModel>()
                .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role.Name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}"));

            // Course
            CreateMap<Course, CourseViewModel>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher.Name))
                .ForMember(d => d.LevelName, o => o.MapFrom(s => s.Level.Name));

            // Topic
            CreateMap<Topic, TopicViewModel>()
                .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Course.Title));

            // Lesson
            CreateMap<Lesson, LessonViewModel>()
                .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title))
                .ForMember(d => d.TopicName, o => o.MapFrom(s => s.Topic.Name));

            // Article
            CreateMap<Article, ArticleViewModel>()
                .ForMember(d => d.SubjectName, o => o.MapFrom(s => s.Subject.Name));

            // Certificate
            CreateMap<Certificate, CertificateViewModel>()
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                .ForMember(d => d.CourseTitle, o => o.MapFrom(s => s.Course.Title));

            // Question
            CreateMap<Question, QuestionViewModel>()
                .ForMember(d => d.LessonName, o => o.MapFrom(s => s.Lesson.Name));

            // QuestionOption
            CreateMap<QuestionOption, QuestionOptionViewModel>();

            // Testimonial
            CreateMap<Testimonial, TestimonialViewModel>();

            // Plan
            CreateMap<Plan, PlanViewModel>();

            // Order
            CreateMap<Order, OrderViewModel>()
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"))
                .ForMember(d => d.PlanName, o => o.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.Name));

            // Payment
            CreateMap<Payment, PaymentViewModel>()
                .ForMember(d => d.UserFullName, o => o.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));

            // AuditLog
            CreateMap<AuditLog, AuditLogViewModel>();
        }
    }
}
