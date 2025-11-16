using SalesOrderManagement.Domain.Entities;

namespace SalesOrderManagement.Application.Interfaces
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item?> GetByIdAsync(int id);
        Task<Item?> GetByCodeAsync(string itemCode);
        Task<Item> AddAsync(Item item);
        Task<Item> UpdateAsync(Item item);
        Task DeleteAsync(int id);
    }
}