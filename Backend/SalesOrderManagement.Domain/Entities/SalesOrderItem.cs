using System.ComponentModel.DataAnnotations;

namespace SalesOrderManagement.Domain.Entities
{
    public class SalesOrderItem
    {
        public int Id { get; set; }
        
        [Required]
        public int SalesOrderId { get; set; }
        
        [Required]
        public int ItemId { get; set; }
        
        [StringLength(500)]
        public string Note { get; set; } = string.Empty;
        
        [Required]
        public decimal Quantity { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public decimal TaxRate { get; set; }
        
        public decimal ExclAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal InclAmount { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual SalesOrder SalesOrder { get; set; } = null!;
        public virtual Item Item { get; set; } = null!;
    }
}