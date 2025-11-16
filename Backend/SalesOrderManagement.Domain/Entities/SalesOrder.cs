using System.ComponentModel.DataAnnotations;

namespace SalesOrderManagement.Domain.Entities
{
    public class SalesOrder
    {
        public int Id { get; set; }
        
        [Required]
        public int ClientId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string InvoiceNo { get; set; } = string.Empty;
        
        [Required]
        public DateTime InvoiceDate { get; set; }
        
        [StringLength(100)]
        public string ReferenceNo { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Note { get; set; } = string.Empty;
        
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
        
        public decimal TotalExcl { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalIncl { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        
        // Navigation properties
        public virtual Client Client { get; set; } = null!;
        public virtual ICollection<SalesOrderItem> SalesOrderItems { get; set; } = new List<SalesOrderItem>();
    }
}