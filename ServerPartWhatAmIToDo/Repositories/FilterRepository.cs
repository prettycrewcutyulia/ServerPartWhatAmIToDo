using Microsoft.EntityFrameworkCore;
using ServerPartWhatAmIToDo.Models.DataBase;
using ServerPartWhatAmIToDo.Repositories.Protocols;

namespace ServerPartWhatAmIToDo.Repositories
{
    public class FilterRepository : IFilterRepository
    {
        private readonly DataContext _context;

        public FilterRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FilterEntity>> GetAllFiltersAsync()
        {
            return await _context.Filters.ToListAsync();
        }

        public async Task<FilterEntity> GetFilterByIdAsync(int filterId)
        {
            return await _context.Filters.FindAsync(filterId);
        }

        public async Task<IEnumerable<FilterEntity>> GetFiltersByUserIdAsync(int userId)
        {
            return await _context.Filters.Where(filter => filter.UserId == userId).ToListAsync();
        }

        public async Task<FilterEntity> AddFilterAsync(FilterEntity filter)
        {
            var allUserFilters = await _context.Filters.Where(element => filter.UserId == element.UserId).ToListAsync();
            var oldFilters = allUserFilters.Where(element => filter.Title == element.Title);
            if (oldFilters.Count() > 0)
            {
                return oldFilters.First();
            }
            await _context.Filters.AddAsync(filter);
            await _context.SaveChangesAsync();
            return filter;
        }

        public async Task<FilterEntity> UpdateFilterAsync(FilterEntity filter)
        {
            _context.Filters.Update(filter);
            await _context.SaveChangesAsync();
            
            return filter;
        }

        public async Task DeleteFilterAsync(int filterId)
        {
            var filter = await GetFilterByIdAsync(filterId);
            if (filter != null)
            {
                _context.Filters.Remove(filter);
                await _context.SaveChangesAsync();
            }
        }
    }
}