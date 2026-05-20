using JAPLearning.Business.Interfaces.Externals;
using JAPLearning.Business.Interfaces.Internals.Auxiliaries;
using JAPLearning.Business.Interfaces.Internals.Entities;
using JAPLearning.Business.Interfaces.Internals.Parameters;
using JAPLearning.Business.Interfaces.Internals.Relationships;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Business.Interfaces.Services.Parameters;
using JAPLearning.Business.Interfaces.Services.Relationships;
using JAPLearning.Business.Models.Settings;
using JAPLearning.Business.Notifications;
using JAPLearning.Business.Services.Externals;
using JAPLearning.Business.Services.Auxiliaries;
using JAPLearning.Business.Services.Entities;
using JAPLearning.Business.Services.Parameters;
using JAPLearning.Business.Services.Relationships;
using JAPLearning.Data.Repositories.Auxiliaries;
using JAPLearning.Data.Repositories.Entities;
using JAPLearning.Data.Repositories.Parameters;
using JAPLearning.Data.Repositories.Relationships;
using JAPLearning.Data.Repositories.Shareds;

namespace JAPLearning.Mvc.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // HttpContextAccessor
            services.AddHttpContextAccessor();

            // Cloudinary
            services.Configure<CloudinarySetting>(configuration.GetSection("CloudinarySettings"));
            services.AddScoped<ICloudinaryService, CloudinaryService>();

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

            // Services - Relationships
            services.AddScoped<IUserCourseLessonService, UserCourseLessonService>();
            services.AddScoped<ICourseRequirementService, CourseRequirementService>();

            // Services - Auxiliaries
            services.AddScoped<IAuditLogService, AuditLogService>();

            return services;
        }
    }
}
