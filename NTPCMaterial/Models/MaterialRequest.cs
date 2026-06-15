using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NTPCMaterial.Models
{
    public class MaterialRequest
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string RequestNumber { get; set; }   // auto-generated e.g. "MRQ-20240612-0001"

        // Link to the purchase catalogue item
        [Required]
        public int MaterialPurchaseId { get; set; }

        [ForeignKey("MaterialPurchaseId")]
        public virtual MaterialPurchase MaterialPurchase { get; set; }

        [Required, MaxLength(50)]
        public string MaterialCode { get; set; }

        [Required, MaxLength(200)]
        public string MaterialName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int QuantityRequested { get; set; }

        [Required, MaxLength(100)]
        public string Department { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime RequiredByDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        // "Pending" | "Approved" | "Rejected"
        [Required, MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required, MaxLength(50)]
        public string RequestedBy { get; set; }     // Session["EmployeeNumber"]

        // Filled by admin on approval/rejection
        public DateTime? ReviewedAt { get; set; }
        [MaxLength(50)]
        public string ReviewedBy { get; set; }
        [MaxLength(500)]
        public string ReviewRemarks { get; set; }
    }
}