using System.Linq;
using System.Web.Mvc;
using NTPCMaterial.Data;

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
            ViewBag.TotalAmount = _db.MaterialPurchases.Sum(p => (decimal?)p.Amount) ?? 0;
            ViewBag.RecentOrders = _db.MaterialPurchases
                                       .OrderByDescending(p => p.CreatedAt)
                                       .Take(8).ToList();
            ViewBag.TotalEmployees = _db.Employees.Count();
            return View();
        }

        // GET: /Dashboard/UserDashboard
        public ActionResult UserDashboard()
        {
            return View();
        }
    }
}