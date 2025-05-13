using ServerPartWhatAmIToDo.Models.DataBase;

namespace ServerPartWhatAmIToDo.Repositories.Protocols
{
    public interface IFilterRepository
    {
        Task<IEnumerable<FilterEntity>> GetAllFiltersAsync(CancellationToken cancellationToken);
        Task<FilterEntity> GetFilterByIdAsync(int filterId, CancellationToken cancellationToken);
        Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId, CancellationToken cancellationToken);
        Task<FilterEntity> AddFilterAsync(FilterEntity filter, CancellationToken cancellationToken);
        Task<FilterEntity> UpdateFilterAsync(FilterEntity filter, CancellationToken cancellationToken);
        Task DeleteFilterAsync(int filterId, CancellationToken cancellationToken);
    }
}