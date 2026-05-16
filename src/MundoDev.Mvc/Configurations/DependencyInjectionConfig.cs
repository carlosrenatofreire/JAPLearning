using MundoDev.Business.Interfaces.Internals.Auxiliaries;
using MundoDev.Business.Interfaces.Internals.Entities;
using MundoDev.Business.Interfaces.Internals.Parameters;
using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Interfaces.Internals.Shareds;
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

            return services;
        }
    }
}
