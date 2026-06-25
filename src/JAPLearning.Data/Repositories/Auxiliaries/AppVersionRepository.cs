using Microsoft.EntityFrameworkCore;
using JAPLearning.Business.Interfaces.Internals.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;
using JAPLearning.Data.Contexts;

namespace JAPLearning.Data.Repositories.Auxiliaries
{
    public class AppVersionRepository : IAppVersionRepository
    {
        private readonly MainDbContext _context;

        public AppVersionRepository(MainDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppVersion>> GetAllWithItemsAsync() =>
            await _context.AppVersions
                .Include(v => v.Items.OrderBy(i => i.Order))
                .OrderByDescending(v => v.ReleaseDate)
                .AsNoTracking()
                .ToListAsync();

        public async Task<AppVersion?> GetByIdWithItemsAsync(Guid id) =>
            await _context.AppVersions
                .Include(v => v.Items.OrderBy(i => i.Order))
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

        public async Task<AppVersion?> GetLatestPublishedAsync() =>
            await _context.AppVersions
                .Include(v => v.Items.OrderBy(i => i.Order))
                .Where(v => v.IsPublished)
                .OrderByDescending(v => v.ReleaseDate)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        public async Task Add(AppVersion entity)
        {
            await _context.AppVersions.AddAsync(entity);
        }

        public Task Update(AppVersion entity)
        {
            _context.AppVersions.Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(AppVersion entity)
        {
            _context.AppVersions.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task AddItem(AppVersionItem item)
        {
            await _context.AppVersionItems.AddAsync(item);
        }

        public Task DeleteItem(AppVersionItem item)
        {
            _context.AppVersionItems.Remove(item);
            return Task.CompletedTask;
        }

        public async Task<AppVersionItem?> GetItemByIdAsync(Guid id) =>
            await _context.AppVersionItems
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
    }
}
