using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using NTPCMaterial.Data;
using NTPCMaterial.Models.DTOs;

namespace NTPCMaterial.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        // GET: /Account/Login
        [HttpGet]
        public ActionResult Login()
        {
           
            if (User.Identity.IsAuthenticated)
            {
                if (Session["Role"]?.ToString() == "Admin")
                    return RedirectToAction("AdminDashboard", "Dashboard");
                else
                    return RedirectToAction("UserDashboard", "Dashboard");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(EmployeeDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            string hashed = HashPassword(dto.Password);
            var employee = _db.Employees.FirstOrDefault(e =>
                e.EmployeeNumber == dto.EmployeeNumber &&
                e.PasswordHash == hashed);

            if (employee == null)
            {
                ModelState.AddModelError("", "Invalid Employee Number or Password.");
                return View(dto);
            }

            employee.LastLoginAt = DateTime.Now;
            _db.SaveChanges();

            Session["EmployeeNumber"] = employee.EmployeeNumber;
            Session["Role"] = employee.Role;

            FormsAuthentication.SetAuthCookie(employee.EmployeeNumber, dto.RememberMe);

            if (employee.Role == "Admin")
                return RedirectToAction("AdminDashboard", "Dashboard");
            else
                return RedirectToAction("UserDashboard", "Dashboard");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }

        private static string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}