using System;
using System.Linq;
using System.Web.Mvc;
using NTPCMaterial.Data;
using NTPCMaterial.Models;
using NTPCMaterial.Models.DTOs;
namespace NTPCMaterial.Controllers
{
    [Authorize]
    public class MaterialPurchaseController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        private bool IsAdmin()
        {
            return Session["Role"]?.ToString() == "Admin";
        }

        // GET: /MaterialPurchase/Records  -> full list (sidebar "Purchase Records")
        public ActionResult Records()
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            var records = _db.MaterialPurchases.OrderByDescending(p => p.CreatedAt).ToList();
            return View(records);
        }

        // GET: /MaterialPurchase/Add  -> empty form
        [HttpGet]
        public ActionResult Add()
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            return View(new MaterialPurchaseDto());
        }

        // POST: /MaterialPurchase/Add -> save new record
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MaterialPurchaseDto dto)
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            if (!ModelState.IsValid)
                return View(dto);

            var record = new MaterialPurchase
            {
                PONumber = dto.PONumber,
                MaterialCode = dto.MaterialCode,
                MaterialName = dto.MaterialName,
                Amount = dto.Amount,
                PurchaseDate = dto.PurchaseDate,
                CreatedAt = DateTime.Now,
                AddedBy = Session["EmployeeNumber"]?.ToString()
            };

            _db.MaterialPurchases.Add(record);
            _db.SaveChanges();

            TempData["Success"] = "Purchase record added successfully.";
            return RedirectToAction("Records");
        }

        // GET: /MaterialPurchase/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            var record = _db.MaterialPurchases.FirstOrDefault(p => p.Id == id);
            if (record == null) return HttpNotFound();

            var dto = new MaterialPurchaseDto
            {
                PONumber = record.PONumber,
                MaterialCode = record.MaterialCode,
                MaterialName = record.MaterialName,
                Amount = record.Amount,
                PurchaseDate = record.PurchaseDate
            };
            ViewBag.Id = record.Id;
            return View(dto);
        }

        // POST: /MaterialPurchase/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MaterialPurchaseDto dto)
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            if (!ModelState.IsValid)
            {
                ViewBag.Id = id;
                return View(dto);
            }

            var record = _db.MaterialPurchases.FirstOrDefault(p => p.Id == id);
            if (record == null) return HttpNotFound();

            record.PONumber = dto.PONumber;
            record.MaterialCode = dto.MaterialCode;
            record.MaterialName = dto.MaterialName;
            record.Amount = dto.Amount;
            record.PurchaseDate = dto.PurchaseDate;

            _db.SaveChanges();
            TempData["Success"] = "Purchase record updated successfully.";
            return RedirectToAction("Records");
        }

        // POST: /MaterialPurchase/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("UserDashboard", "Dashboard");

            var record = _db.MaterialPurchases.FirstOrDefault(p => p.Id == id);
            if (record != null)
            {
                _db.MaterialPurchases.Remove(record);
                _db.SaveChanges();
                TempData["Success"] = "Purchase record deleted successfully.";
            }
            return RedirectToAction("Records");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}