using MundoDev.Business.Interfaces.Internals.Auxiliaries;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Parameters;
using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Interfaces.Internals.Shareds;
using MundoDev.Business.Interfaces.Services.Auxiliaries;
using MundoDev.Business.Interfaces.Services.Entities;
using MundoDev.Business.Interfaces.Services.Parameters;
using MundoDev.Business.Interfaces.Services.Relationships;
using MundoDev.Business.Notifications;
using MundoDev.Business.Services.Auxiliaries;
using MundoDev.Business.Services.Entities;
using MundoDev.Business.Services.Parameters;
using MundoDev.Business.Services.Relationships;
using MundoDev.Data.Repositories.Auxiliaries;
using MundoDev.Data.Repositories.Entities;
using MundoDev.Data.Repositories.Parameters;
using MundoDev.Data.Repositories.Relationships;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Mvc.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Parameters
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILevelRepository, LevelRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();

            // Entities
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICertificateRepository, CertificateRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IQuestionOptionRepository, QuestionOptionRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            // Relationships
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IUserCourseLessonRepository, UserCourseLessonRepository>();
            services.AddScoped<ICourseRequirementRepository, CourseRequirementRepository>();
            services.AddScoped<IUserLessonTestRepository, UserLessonTestRepository>();
            services.AddScoped<IUserLessonQuestionRepository, UserLessonQuestionRepository>();

            // Auxiliaries
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Notificator (Scoped per request)
            services.AddScoped<INotificator, Notificator>();

            // Services - Parameters
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();

            // Services - Entities
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IQuestionOptionService, QuestionOptionService>();
            services.AddScoped<ITestimonialService, TestimonialService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            // Services - Relationships
            services.AddScoped<IUserCourseLessonService, UserCourseLessonService>();

            // Services - Auxiliaries
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }
    }
}
