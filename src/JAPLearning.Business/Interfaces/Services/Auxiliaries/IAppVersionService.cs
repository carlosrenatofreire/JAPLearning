using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Interfaces.Services.Auxiliaries
{
    public interface IAppVersionService
    {
        Task<List<AppVersion>> GetAllWithItemsAsync();
        Task<AppVersion?> GetByIdWithItemsAsync(Guid id);
        Task<AppVersion?> GetLatestPublishedAsync();
        Task<bool> AddAsync(AppVersion entity);
        Task<bool> UpdateAsync(AppVersion entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> AddItemAsync(AppVersionItem item);
        Task<bool> DeleteItemAsync(Guid itemId);
    }
}
