using System.Collections.Generic;

namespace NTPCMaterial.Models
{
    public class UserDashboardViewModel
    {
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int PendingRequests { get; set; }
        public int RejectedRequests { get; set; }

        public IList<MaterialPurchase> Purchases { get; set; } = new List<MaterialPurchase>();
        public IList<MaterialRequest> RecentRequests { get; set; } = new List<MaterialRequest>();
    }
}