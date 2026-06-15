using System;
using System.ComponentModel.DataAnnotations;
namespace NTPCMaterial.Models.DTOs
{
    public class MaterialPurchaseDto
    {
        [Required(ErrorMessage = "PO Number is required.")]
        [MaxLength(50)]
        public string PONumber { get; set; }

        [Required(ErrorMessage = "Material Code is required.")]
        [MaxLength(50)]
        public string MaterialCode { get; set; }

        [Required(ErrorMessage = "Material Name is required.")]
        [MaxLength(200)]
        public string MaterialName { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Purchase Date is required.")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate { get; set; }
    }
}