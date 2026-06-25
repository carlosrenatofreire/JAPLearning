using Microsoft.EntityFrameworkCore;
using MundoDev.Business.Interfaces.Internals.Relationships;
using MundoDev.Business.Models.Domains.Relationships;
using MundoDev.Data.Contexts;
using MundoDev.Data.Repositories.Shareds;

namespace MundoDev.Data.Repositories.Relationships
{
    public class UserCourseLessonRepository : Repository<UserCourseLesson>, IUserCourseLessonRepository
    {
        public UserCourseLessonRepository(MainDbContext db) : base(db) { }

        public async Task<List<UserCourseLesson>> GetByUserAsync(Guid userId)
        {
            return await DbSet.AsNoTracking()
                .Include(x => x.Lesson)
                    .ThenInclude(l => l.Topic)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserCourseLesson?> GetByUserAndLessonAsync(Guid userId, Guid lessonId)
        {
            return await DbSet.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);
        }
    }
}
