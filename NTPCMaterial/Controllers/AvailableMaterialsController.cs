using System;
using System.Linq;
using System.Web.Mvc;
using NTPCMaterial.Data;          // your DbContext namespace — adjust if different
using NTPCMaterial.Models.DTOs;
using NTPCMaterial.Models;

namespace NTPCMaterial.Controllers
{
    /// <summary>
    /// Handles the Available Materials catalogue page for employees.
    /// Requires the user to be logged in (session check).
    /// </summary>
    [Authorize]
    public class AvailableMaterialsController : Controller
    {
        private readonly AppDbContext _db;

        public AvailableMaterialsController()
        {
            _db = new AppDbContext();
        }

       
        // If you use dependency injection, replace the constructor above with:
        // public AvailableMaterialsController(ApplicationDbContext db) { _db = db; }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }

        // ─────────────────────────────────────────────────────────────
        // GET: /AvailableMaterials/
        // Shows the full materials catalogue, newest first.
        // ─────────────────────────────────────────────────────────────
        public ActionResult Index()
        {
            // Guard: employee must be logged in
            if (Session["EmployeeNumber"] == null)
                return RedirectToAction("Login", "Account");

            // Fetch all purchases, map to DTO (no sensitive fields exposed)
            var materials = _db.MaterialPurchases
                .OrderByDescending(p => p.PurchaseDate)
                .ToList()                          // execute query, then map in memory
                .Select(p => new AvailableMaterialDto
                {
                    Id = p.Id,
                    PONumber = p.PONumber,
                    MaterialCode = p.MaterialCode,
                    MaterialName = p.MaterialName,
                    Quantity = p.Quantity,
                    PurchaseDate = p.PurchaseDate
                })
                .ToList();

            var vm = new AvailableMaterialsViewModel
            {
                Materials = materials
            };

            return View(vm);
        }
    }
}