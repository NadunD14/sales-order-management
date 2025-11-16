using Microsoft.AspNetCore.Mvc;
using SalesOrderManagement.Application.Models;
using SalesOrderManagement.Application.Interfaces;

namespace SalesOrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;

        public SalesOrdersController(ISalesOrderService salesOrderService)
        {
            _salesOrderService = salesOrderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderDto>>> GetSalesOrders()
        {
            var salesOrders = await _salesOrderService.GetAllSalesOrdersAsync();
            return Ok(salesOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderDto>> GetSalesOrder(int id)
        {
            var salesOrder = await _salesOrderService.GetSalesOrderByIdAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return Ok(salesOrder);
        }

        [HttpPost]
        public async Task<ActionResult<SalesOrderDto>> CreateSalesOrder(CreateSalesOrderDto createDto)
        {
            try
            {
                var salesOrder = await _salesOrderService.CreateSalesOrderAsync(createDto);
                return CreatedAtAction(nameof(GetSalesOrder), new { id = salesOrder.Id }, salesOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalesOrderDto>> UpdateSalesOrder(int id, UpdateSalesOrderDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                var salesOrder = await _salesOrderService.UpdateSalesOrderAsync(updateDto);
                return Ok(salesOrder);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrder(int id)
        {
            try
            {
                await _salesOrderService.DeleteSalesOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("generate-invoice-number")]
        public async Task<ActionResult<string>> GenerateInvoiceNumber()
        {
            var invoiceNumber = await _salesOrderService.GenerateInvoiceNumberAsync();
            return Ok(invoiceNumber);
        }
    }
}