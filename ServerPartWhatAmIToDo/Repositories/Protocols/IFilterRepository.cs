using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IFilterRepository
    {
        Task<IEnumerable<FilterEntity>> GetAllFiltersAsync();
        Task<FilterEntity> GetFilterByIdAsync(int filterId);
        Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId);
        Task<FilterEntity> AddFilterAsync(FilterEntity filter);
        Task<FilterEntity> UpdateFilterAsync(FilterEntity filter);
        Task DeleteFilterAsync(int filterId);
    }
}