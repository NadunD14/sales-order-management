namespace SalesOrderManagement.API.Models
{
    public class SalesOrderItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public decimal ExclAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InclAmount { get; set; }
    }

    public class CreateSalesOrderItemDto
    {
        public int ItemId { get; set; }
        public string Note { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal TaxRate { get; set; }
    }
}