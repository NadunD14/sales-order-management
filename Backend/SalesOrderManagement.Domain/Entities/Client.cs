using System.ComponentModel.DataAnnotations;

namespace SalesOrderManagement.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Address1 { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Address2 { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string Address3 { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string Suburb { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string State { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string PostCode { get; set; } = string.Empty;
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
    }
}