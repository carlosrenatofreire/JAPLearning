using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Models.Domains.Auxiliaries;
using MundoDev.Business.Models.Domains.Entities;
using MundoDev.Business.Models.Domains.Parameters;
using MundoDev.Business.Models.Domains.Relationships;

namespace MundoDev.Data.Contexts
{
    public class MainDbContext : DbContext
    {
        // Parameters
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }

        // Entities
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // Relationships
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserCourseLesson> UserCourseLessons { get; set; }
        public DbSet<CourseRequirement> CourseRequirements { get; set; }
        public DbSet<UserLessonTest> UserLessonTests { get; set; }
        public DbSet<UserLessonQuestion> UserLessonQuestions { get; set; }

        // Auxiliaries
        public DbSet<AuditLog> AuditLogs { get; set; }

        public MainDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("CreatedDate").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("CreatedDate").IsModified = false;
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("ChangedDate") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("ChangedDate").IsModified = false;

                if (entry.State == EntityState.Modified)
                    entry.Property("ChangedDate").CurrentValue = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
