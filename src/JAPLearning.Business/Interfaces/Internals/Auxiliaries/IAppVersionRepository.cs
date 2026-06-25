using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Interfaces.Internals.Auxiliaries
{
    public interface IAppVersionRepository
    {
        Task<List<AppVersion>> GetAllWithItemsAsync();
        Task<AppVersion?> GetByIdWithItemsAsync(Guid id);
        Task<AppVersion?> GetLatestPublishedAsync();
        Task Add(AppVersion entity);
        Task Update(AppVersion entity);
        Task Delete(AppVersion entity);
        Task AddItem(AppVersionItem item);
        Task DeleteItem(AppVersionItem item);
        Task<AppVersionItem?> GetItemByIdAsync(Guid id);
    }
}
