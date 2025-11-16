using Microsoft.EntityFrameworkCore;
using SalesOrderManagement.Application.Interfaces;
using SalesOrderManagement.Domain.Entities;
using SalesOrderManagement.Infrastructure.Data;

namespace SalesOrderManagement.Infrastructure.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            return await _context.SalesOrders
                .Include(so => so.Client)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(soi => soi.Item)
                .OrderByDescending(so => so.CreatedDate)
                .ToListAsync();
        }

        public async Task<SalesOrder?> GetByIdAsync(int id)
        {
            return await _context.SalesOrders
                .Include(so => so.Client)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(soi => soi.Item)
                .FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task<SalesOrder?> GetByInvoiceNoAsync(string invoiceNo)
        {
            return await _context.SalesOrders
                .Include(so => so.Client)
                .Include(so => so.SalesOrderItems)
                    .ThenInclude(soi => soi.Item)
                .FirstOrDefaultAsync(so => so.InvoiceNo == invoiceNo);
        }

        public async Task<SalesOrder> AddAsync(SalesOrder salesOrder)
        {
            // Calculate totals before saving
            CalculateTotals(salesOrder);
            
            _context.SalesOrders.Add(salesOrder);
            await _context.SaveChangesAsync();
            
            // Return the saved order with includes
            return await GetByIdAsync(salesOrder.Id) ?? salesOrder;
        }

        public async Task<SalesOrder> UpdateAsync(SalesOrder salesOrder)
        {
            salesOrder.ModifiedDate = DateTime.UtcNow;
            
            // Remove existing items
            var existingItems = await _context.SalesOrderItems
                .Where(soi => soi.SalesOrderId == salesOrder.Id)
                .ToListAsync();
            _context.SalesOrderItems.RemoveRange(existingItems);
            
            // Calculate totals before saving
            CalculateTotals(salesOrder);
            
            _context.Entry(salesOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            // Return the updated order with includes
            return await GetByIdAsync(salesOrder.Id) ?? salesOrder;
        }

        public async Task DeleteAsync(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                _context.SalesOrders.Remove(salesOrder);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateInvoiceNumberAsync()
        {
            var lastOrder = await _context.SalesOrders
                .OrderByDescending(so => so.Id)
                .FirstOrDefaultAsync();
            
            if (lastOrder == null)
            {
                return "INV-0001";
            }
            
            // Extract number from last invoice
            var lastInvoiceNo = lastOrder.InvoiceNo;
            if (lastInvoiceNo.StartsWith("INV-"))
            {
                var numberPart = lastInvoiceNo.Substring(4);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    return $"INV-{(lastNumber + 1):D4}";
                }
            }
            
            return $"INV-{DateTime.Now:yyyyMMdd}-001";
        }

        private void CalculateTotals(SalesOrder salesOrder)
        {
            decimal totalExcl = 0;
            decimal totalTax = 0;
            
            foreach (var item in salesOrder.SalesOrderItems)
            {
                // Calculate line amounts
                item.ExclAmount = item.Quantity * item.Price;
                item.TaxAmount = item.ExclAmount * item.TaxRate / 100;
                item.InclAmount = item.ExclAmount + item.TaxAmount;
                
                // Add to totals
                totalExcl += item.ExclAmount;
                totalTax += item.TaxAmount;
            }
            
            salesOrder.TotalExcl = totalExcl;
            salesOrder.TotalTax = totalTax;
            salesOrder.TotalIncl = totalExcl + totalTax;
        }
    }
}