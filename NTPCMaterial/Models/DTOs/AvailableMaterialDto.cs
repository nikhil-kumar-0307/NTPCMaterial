using System;
using System.Collections.Generic;
using System.Linq;

namespace NTPCMaterial.Models.DTOs
{
    
    public class AvailableMaterialDto
    {
        public int Id { get; set; }

        public string PONumber { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Computed: true when stock is below 10. Used to show orange warning pill in UI.
        /// </summary>
        public bool IsLowStock => Quantity < 10;
    }

    /// <summary>
    /// Wraps the catalogue list passed to the AvailableMaterials view.
    /// Kept in the same file since it only exists to hold AvailableMaterialDto items.
    /// </summary>
    public class AvailableMaterialsViewModel
    {
        public List<AvailableMaterialDto> Materials { get; set; } = new List<AvailableMaterialDto>();

        /// <summary>
        /// Total items — used for the count badge in the view.
        /// </summary>
        public int TotalCount => Materials.Count;
    }
}