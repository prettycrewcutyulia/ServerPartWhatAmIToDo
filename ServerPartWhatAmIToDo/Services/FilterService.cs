
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Models.Goals;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Services
{
    public interface IFilterService
    {
        bool DoesIdExist(int id);
       Task<IEnumerable<FilterEntity>> GetAllFiltersAsync();
        Task<FilterEntity?> GetFilterByIdAsync(int filterId);
        Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId);
        Task<FilterEntity> AddFilterAsync(int userId, FilterRequest newFilter);
        Task<FilterEntity> UpdateFilterAsync(UpdateFilterRequest newFilter);
        Task DeleteFilterAsync(int filterId);
    }
    
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;

        public FilterService(IFilterRepository filterRepository)
        {
            _filterRepository = filterRepository;
        }
        
        public bool DoesIdExist(int id)
        {
            var result = GetFilterByIdAsync(id).Result;
            return result != null;
        }

        public async Task<IEnumerable<FilterEntity>> GetAllFiltersAsync()
        {
            return await _filterRepository.GetAllFiltersAsync();
        }

        public async Task<FilterEntity?> GetFilterByIdAsync(int filterId)
        {
            return await _filterRepository.GetFilterByIdAsync(filterId);
        }

        public async Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId)
        {
            return await _filterRepository.GetFiltersByUserIdAsync(userId);
        }

        public async Task<FilterEntity> AddFilterAsync(int userId, FilterRequest newFilter)
        {
            var filterEntity = new FilterEntity();
            filterEntity.UserId = userId;
            filterEntity.Title = newFilter.Title;
            filterEntity.Color = newFilter.Color;
            
            return await _filterRepository.AddFilterAsync(filterEntity);
        }

        public async Task<FilterEntity> UpdateFilterAsync(UpdateFilterRequest newFilter)
        {
            var filterEntity = await _filterRepository.GetFilterByIdAsync(newFilter.Id);
            filterEntity.Title = newFilter.Title;
            filterEntity.Color = newFilter.Color;
            return await _filterRepository.UpdateFilterAsync(filterEntity);
        }

        public async Task DeleteFilterAsync(int filterId)
        {
            await _filterRepository.DeleteFilterAsync(filterId);
        }
    }
}
