using System;
using System.ComponentModel.DataAnnotations;

namespace NTPCMaterial.Models.DTOs
{
    public class MaterialRequestDto
    {
        [Required(ErrorMessage = "Please select a material.")]
        public int MaterialPurchaseId { get; set; }

        // Read-only display helpers (populated from the selected purchase record)
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int QuantityRequested { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [MaxLength(100)]
        public string Department { get; set; }

        [Required(ErrorMessage = "Required-by date is required.")]
        [DataType(DataType.Date)]
        public DateTime RequiredByDate { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }
    }
}