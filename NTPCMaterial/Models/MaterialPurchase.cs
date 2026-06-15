using System;
using System.ComponentModel.DataAnnotations;
namespace NTPCMaterial.Models
{
    public class MaterialPurchase
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string PONumber { get; set; }
        [Required, MaxLength(50)]
        public string MaterialCode { get; set; }
        [Required, MaxLength(200)]
        public string MaterialName { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AddedBy { get; set; }
    }
}