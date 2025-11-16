using SalesOrderManagement.Domain.Entities;

namespace SalesOrderManagement.Application.Interfaces
{
    public interface ISalesOrderRepository
    {
        Task<IEnumerable<SalesOrder>> GetAllAsync();
        Task<SalesOrder?> GetByIdAsync(int id);
        Task<SalesOrder?> GetByInvoiceNoAsync(string invoiceNo);
        Task<SalesOrder> AddAsync(SalesOrder salesOrder);
        Task<SalesOrder> UpdateAsync(SalesOrder salesOrder);
        Task DeleteAsync(int id);
        Task<string> GenerateInvoiceNumberAsync();
    }
}