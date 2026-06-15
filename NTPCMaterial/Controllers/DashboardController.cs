using System.Linq;
using System.Web.Mvc;
using NTPCMaterial.Data;
using NTPCMaterial.Models;

namespace NTPCMaterial.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: /Dashboard/AdminDashboard
        public ActionResult AdminDashboard()
        {
            if (Session["Role"]?.ToString() != "Admin")
                return RedirectToAction("UserDashboard");

            ViewBag.TotalOrders = _db.MaterialPurchases.Count();
            ViewBag.TotalQuantity = _db.MaterialPurchases.Sum(p => (int?)p.Quantity) ?? 0;  // changed
            ViewBag.TotalEmployees = _db.Employees.Count();
            ViewBag.RecentOrders = _db.MaterialPurchases
                                        .OrderByDescending(p => p.CreatedAt)
                                        .Take(8)
                                        .ToList();

            ViewBag.PendingCount = _db.MaterialRequests.Count(r => r.Status == "Pending");
            ViewBag.ApprovedCount = _db.MaterialRequests.Count(r => r.Status == "Approved");
            ViewBag.RejectedCount = _db.MaterialRequests.Count(r => r.Status == "Rejected");

            ViewBag.PendingRequests = _db.MaterialRequests
                                         .Where(r => r.Status == "Pending")
                                         .OrderByDescending(r => r.CreatedAt)
                                         .Take(10)
                                         .ToList();
            return View();
        }

        // GET: /Dashboard/UserDashboard
        public ActionResult UserDashboard()
        {
            string emp = Session["EmployeeNumber"]?.ToString();

            var myRequests = _db.MaterialRequests
                                .Where(r => r.RequestedBy == emp)
                                .ToList();

            var vm = new UserDashboardViewModel
            {
                TotalRequests = myRequests.Count,
                ApprovedRequests = myRequests.Count(r => r.Status == "Approved"),
                PendingRequests = myRequests.Count(r => r.Status == "Pending"),
                RejectedRequests = myRequests.Count(r => r.Status == "Rejected"),
                Purchases = _db.MaterialPurchases
                                      .OrderBy(p => p.MaterialName)
                                      .ToList(),
                RecentRequests = myRequests
                                      .OrderByDescending(r => r.CreatedAt)
                                      .Take(10)
                                      .ToList()
            };

            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}