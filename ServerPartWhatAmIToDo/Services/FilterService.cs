
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IFilterService
    {
        bool DoesIdExist(int id, CancellationToken token);
       Task<IEnumerable<FilterEntity>> GetAllFiltersAsync(CancellationToken token);
        Task<FilterEntity?> GetFilterByIdAsync(int filterId, CancellationToken token);
        Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId, CancellationToken token);
        Task<FilterEntity> AddFilterAsync(int userId, FilterRequest newFilter, CancellationToken token);
        Task<FilterEntity> UpdateFilterAsync(UpdateFilterRequest newFilter, CancellationToken token);
        Task DeleteFilterAsync(int filterId, CancellationToken token);
    }
    
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;

        public FilterService(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }
        
        public bool DoesIdExist(int id, CancellationToken token)
        {
            var result = GetFilterByIdAsync(id, token).Result;
            return result != null;
        }

        public async Task<IEnumerable<FilterEntity>> GetAllFiltersAsync(CancellationToken token)
        {
            return await _filterRepository.GetAllFiltersAsync(token);
        }

        public async Task<FilterEntity?> GetFilterByIdAsync(int filterId, CancellationToken token)
        {
            return await _filterRepository.GetFilterByIdAsync(filterId, token);
        }

        public async Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId, CancellationToken token)
        {
            return await _filterRepository.GetFiltersByUserIdAsync(userId, token);
        }

        public async Task<FilterEntity> AddFilterAsync(int userId, FilterRequest newFilter, CancellationToken token)
        {
            var filterEntity = new FilterEntity();
            filterEntity.UserId = userId;
            filterEntity.Title = newFilter.Title;
            filterEntity.Color = newFilter.Color;
            
            return await _filterRepository.AddFilterAsync(filterEntity, token);
        }

        public async Task<FilterEntity> UpdateFilterAsync(UpdateFilterRequest newFilter, CancellationToken token)
        {
            var filterEntity = await _filterRepository.GetFilterByIdAsync(newFilter.Id, token);
            filterEntity.Title = newFilter.Title;
            filterEntity.Color = newFilter.Color;
            return await _filterRepository.UpdateFilterAsync(filterEntity, token);
        }

        public async Task DeleteFilterAsync(int filterId, CancellationToken token)
        {
            await _filterRepository.DeleteFilterAsync(filterId, token);
        }
    }
}
