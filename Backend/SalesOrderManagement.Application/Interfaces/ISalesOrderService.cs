using SalesOrderManagement.Application.Models;

namespace SalesOrderManagement.Application.Interfaces
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<SalesOrderDto>> GetAllSalesOrdersAsync();
        Task<SalesOrderDto?> GetSalesOrderByIdAsync(int id);
        Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderDto createDto);
        Task<SalesOrderDto> UpdateSalesOrderAsync(UpdateSalesOrderDto updateDto);
        Task DeleteSalesOrderAsync(int id);
        Task<string> GenerateInvoiceNumberAsync();
    }
}