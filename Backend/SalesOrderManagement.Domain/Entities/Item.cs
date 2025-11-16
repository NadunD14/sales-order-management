using System.ComponentModel.DataAnnotations;

namespace SalesOrderManagement.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ItemCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal Price { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}