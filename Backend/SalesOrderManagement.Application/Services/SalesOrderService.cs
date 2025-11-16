using SalesOrderManagement.Application.Models;
using SalesOrderManagement.Application.Interfaces;
using SalesOrderManagement.Domain.Entities;

namespace SalesOrderManagement.Application.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IClientRepository _clientRepository;

        public SalesOrderService(
            ISalesOrderRepository salesOrderRepository,
            IItemRepository itemRepository,
            IClientRepository clientRepository)
        {
            _salesOrderRepository = salesOrderRepository;
            _itemRepository = itemRepository;
            _clientRepository = clientRepository;
        }

        public async Task<IEnumerable<SalesOrderDto>> GetAllSalesOrdersAsync()
        {
            var salesOrders = await _salesOrderRepository.GetAllAsync();
            return salesOrders.Select(MapToDto).ToList();
        }

        public async Task<SalesOrderDto?> GetSalesOrderByIdAsync(int id)
        {
            var salesOrder = await _salesOrderRepository.GetByIdAsync(id);
            return salesOrder == null ? null : MapToDto(salesOrder);
        }

        public async Task<SalesOrderDto> CreateSalesOrderAsync(CreateSalesOrderDto createDto)
        {
            var salesOrder = new SalesOrder
            {
                ClientId = createDto.ClientId,
                InvoiceNo = createDto.InvoiceNo,
                InvoiceDate = createDto.InvoiceDate,
                ReferenceNo = createDto.ReferenceNo,
                Note = createDto.Note,
                Address1 = createDto.Address1,
                Address2 = createDto.Address2,
                Address3 = createDto.Address3,
                Suburb = createDto.Suburb,
                State = createDto.State,
                PostCode = createDto.PostCode,
                SalesOrderItems = new List<SalesOrderItem>()
            };

            // Add items and calculate prices
            foreach (var itemDto in createDto.Items)
            {
                var item = await _itemRepository.GetByIdAsync(itemDto.ItemId);
                if (item != null)
                {
                    salesOrder.SalesOrderItems.Add(new SalesOrderItem
                    {
                        ItemId = itemDto.ItemId,
                        Note = itemDto.Note,
                        Quantity = itemDto.Quantity,
                        Price = item.Price,
                        TaxRate = itemDto.TaxRate
                    });
                }
            }

            var createdOrder = await _salesOrderRepository.AddAsync(salesOrder);
            return MapToDto(createdOrder);
        }

        public async Task<SalesOrderDto> UpdateSalesOrderAsync(UpdateSalesOrderDto updateDto)
        {
            var existingSalesOrder = await _salesOrderRepository.GetByIdAsync(updateDto.Id);
            if (existingSalesOrder == null)
            {
                throw new ArgumentException("Sales order not found");
            }

            existingSalesOrder.ClientId = updateDto.ClientId;
            existingSalesOrder.InvoiceNo = updateDto.InvoiceNo;
            existingSalesOrder.InvoiceDate = updateDto.InvoiceDate;
            existingSalesOrder.ReferenceNo = updateDto.ReferenceNo;
            existingSalesOrder.Note = updateDto.Note;
            existingSalesOrder.Address1 = updateDto.Address1;
            existingSalesOrder.Address2 = updateDto.Address2;
            existingSalesOrder.Address3 = updateDto.Address3;
            existingSalesOrder.Suburb = updateDto.Suburb;
            existingSalesOrder.State = updateDto.State;
            existingSalesOrder.PostCode = updateDto.PostCode;

            // Clear existing items and add new ones
            existingSalesOrder.SalesOrderItems.Clear();
            foreach (var itemDto in updateDto.Items)
            {
                var item = await _itemRepository.GetByIdAsync(itemDto.ItemId);
                if (item != null)
                {
                    existingSalesOrder.SalesOrderItems.Add(new SalesOrderItem
                    {
                        ItemId = itemDto.ItemId,
                        Note = itemDto.Note,
                        Quantity = itemDto.Quantity,
                        Price = item.Price,
                        TaxRate = itemDto.TaxRate
                    });
                }
            }

            var updatedOrder = await _salesOrderRepository.UpdateAsync(existingSalesOrder);
            return MapToDto(updatedOrder);
        }

        public async Task DeleteSalesOrderAsync(int id)
        {
            await _salesOrderRepository.DeleteAsync(id);
        }

        public async Task<string> GenerateInvoiceNumberAsync()
        {
            return await _salesOrderRepository.GenerateInvoiceNumberAsync();
        }

        private static SalesOrderDto MapToDto(SalesOrder salesOrder)
        {
            return new SalesOrderDto
            {
                Id = salesOrder.Id,
                ClientId = salesOrder.ClientId,
                CustomerName = salesOrder.Client?.CustomerName ?? "",
                InvoiceNo = salesOrder.InvoiceNo,
                InvoiceDate = salesOrder.InvoiceDate,
                ReferenceNo = salesOrder.ReferenceNo,
                Note = salesOrder.Note,
                Address1 = salesOrder.Address1,
                Address2 = salesOrder.Address2,
                Address3 = salesOrder.Address3,
                Suburb = salesOrder.Suburb,
                State = salesOrder.State,
                PostCode = salesOrder.PostCode,
                TotalExcl = salesOrder.TotalExcl,
                TotalTax = salesOrder.TotalTax,
                TotalIncl = salesOrder.TotalIncl,
                CreatedDate = salesOrder.CreatedDate,
                ModifiedDate = salesOrder.ModifiedDate,
                Items = salesOrder.SalesOrderItems.Select(soi => new SalesOrderItemDto
                {
                    Id = soi.Id,
                    ItemId = soi.ItemId,
                    ItemCode = soi.Item?.ItemCode ?? "",
                    ItemDescription = soi.Item?.Description ?? "",
                    Note = soi.Note,
                    Quantity = soi.Quantity,
                    Price = soi.Price,
                    TaxRate = soi.TaxRate,
                    ExclAmount = soi.ExclAmount,
                    TaxAmount = soi.TaxAmount,
                    InclAmount = soi.InclAmount
                }).ToList()
            };
        }
    }
}