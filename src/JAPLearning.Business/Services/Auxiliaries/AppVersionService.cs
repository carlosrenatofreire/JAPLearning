using JAPLearning.Business.Interfaces.Internals.Auxiliaries;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Models.Domains.Auxiliaries;

namespace JAPLearning.Business.Services.Auxiliaries
{
    public class AppVersionService : IAppVersionService
    {
        private readonly IAppVersionRepository _repository;
        private readonly IUnitOfWork           _uow;

        public AppVersionService(IAppVersionRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow        = uow;
        }

        public Task<List<AppVersion>> GetAllWithItemsAsync() =>
            _repository.GetAllWithItemsAsync();

        public Task<AppVersion?> GetByIdWithItemsAsync(Guid id) =>
            _repository.GetByIdWithItemsAsync(id);

        public Task<AppVersion?> GetLatestPublishedAsync() =>
            _repository.GetLatestPublishedAsync();

        public async Task<bool> AddAsync(AppVersion entity)
        {
            await _repository.Add(entity);
            return await _uow.Commit();
        }

        public async Task<bool> UpdateAsync(AppVersion entity)
        {
            _context_Update(entity);
            await _repository.Update(entity);
            return await _uow.Commit();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdWithItemsAsync(id);
            if (entity == null) return false;
            await _repository.Delete(entity);
            return await _uow.Commit();
        }

        public async Task<bool> AddItemAsync(AppVersionItem item)
        {
            await _repository.AddItem(item);
            return await _uow.Commit();
        }

        public async Task<bool> DeleteItemAsync(Guid itemId)
        {
            var item = await _repository.GetItemByIdAsync(itemId);
            if (item == null) return false;
            await _repository.DeleteItem(item);
            return await _uow.Commit();
        }

        // helper vazio — não há validação de negócio aqui
        private static void _context_Update(AppVersion _) { }
    }
}
