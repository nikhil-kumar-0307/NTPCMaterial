using System;
using System.Linq;
using System.Web.Mvc;
using NTPCMaterial.Data;
using NTPCMaterial.Models;
using NTPCMaterial.Models.DTOs;

namespace NTPCMaterial.Controllers
{
    [Authorize]
    public class MaterialRequestController : Controller
    {
        private readonly AppDbContext _db = new AppDbContext();

        private string CurrentEmployee => Session["EmployeeNumber"]?.ToString();
        private bool IsAdmin() => Session["Role"]?.ToString() == "Admin";

        // ─────────────────────────────────────────────────────────────
        // GET: /MaterialRequest/Create?purchaseId=5
        // Employee opens the request form pre-filled with the chosen material
        // ─────────────────────────────────────────────────────────────
        [HttpGet]
        public ActionResult Create(int? purchaseId)
        {
            // Populate department dropdown
            ViewBag.Departments = DepartmentList();

            // Populate material dropdown (all purchase records)
            ViewBag.Materials = MaterialSelectList(purchaseId);

            var dto = new MaterialRequestDto
            {
                RequiredByDate = DateTime.Today.AddDays(7)
            };

            if (purchaseId.HasValue)
            {
                var purchase = _db.MaterialPurchases.FirstOrDefault(p => p.Id == purchaseId.Value);
                if (purchase != null)
                {
                    dto.MaterialPurchaseId = purchase.Id;
                    dto.MaterialCode = purchase.MaterialCode;
                    dto.MaterialName = purchase.MaterialName;
                }
            }

            return View(dto);
        }

        // ─────────────────────────────────────────────────────────────
        // POST: /MaterialRequest/Create
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MaterialRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = DepartmentList();
                ViewBag.Materials = MaterialSelectList(dto.MaterialPurchaseId);
                return View(dto);
            }

            var purchase = _db.MaterialPurchases.FirstOrDefault(p => p.Id == dto.MaterialPurchaseId);
            if (purchase == null)
            {
                ModelState.AddModelError("MaterialPurchaseId", "Selected material not found.");
                ViewBag.Departments = DepartmentList();
                ViewBag.Materials = MaterialSelectList(null);
                return View(dto);
            }

            var request = new MaterialRequest
            {
                RequestNumber = GenerateRequestNumber(),
                MaterialPurchaseId = purchase.Id,
                MaterialCode = purchase.MaterialCode,
                MaterialName = purchase.MaterialName,
                QuantityRequested = dto.QuantityRequested,
                Department = dto.Department,
                RequiredByDate = dto.RequiredByDate,
                Remarks = dto.Remarks,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                RequestedBy = CurrentEmployee
            };

            _db.MaterialRequests.Add(request);
            _db.SaveChanges();

            TempData["Success"] = $"Request {request.RequestNumber} submitted successfully.";
            return RedirectToAction("MyRequests");
        }

        // ─────────────────────────────────────────────────────────────
        // GET: /MaterialRequest/MyRequests
        // Employee sees only their own requests
        // ─────────────────────────────────────────────────────────────
        public ActionResult MyRequests()
        {
            var list = _db.MaterialRequests
                          .Where(r => r.RequestedBy == CurrentEmployee)
                          .OrderByDescending(r => r.CreatedAt)
                          .ToList();
            return View(list);
        }

        // ─────────────────────────────────────────────────────────────
        // GET: /MaterialRequest/Details/5
        // ─────────────────────────────────────────────────────────────
        public ActionResult Details(int id)
        {
            var request = _db.MaterialRequests.FirstOrDefault(r => r.Id == id);
            if (request == null) return HttpNotFound();

            // Non-admins can only see their own
            if (!IsAdmin() && request.RequestedBy != CurrentEmployee)
                return new HttpUnauthorizedResult();

            return View(request);
        }

        // ─────────────────────────────────────────────────────────────
        // ADMIN: GET /MaterialRequest/AllRequests
        // ─────────────────────────────────────────────────────────────
        public ActionResult AllRequests()
        {
            if (!IsAdmin())
                return RedirectToAction("MyRequests");

            var list = _db.MaterialRequests
                          .OrderByDescending(r => r.CreatedAt)
                          .ToList();
            return View(list);
        }

        // ─────────────────────────────────────────────────────────────
        // ADMIN: POST /MaterialRequest/Approve/5
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(int id, string reviewRemarks)
        {
            if (!IsAdmin()) return new HttpUnauthorizedResult();

            var request = _db.MaterialRequests.FirstOrDefault(r => r.Id == id);
            if (request == null) return HttpNotFound();

            request.Status = "Approved";
            request.ReviewedAt = DateTime.Now;
            request.ReviewedBy = CurrentEmployee;
            request.ReviewRemarks = reviewRemarks;
            _db.SaveChanges();

            TempData["Success"] = $"Request {request.RequestNumber} approved.";
            return RedirectToAction("AllRequests");
        }

        // ─────────────────────────────────────────────────────────────
        // ADMIN: POST /MaterialRequest/Reject/5
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reject(int id, string reviewRemarks)
        {
            if (!IsAdmin()) return new HttpUnauthorizedResult();

            var request = _db.MaterialRequests.FirstOrDefault(r => r.Id == id);
            if (request == null) return HttpNotFound();

            request.Status = "Rejected";
            request.ReviewedAt = DateTime.Now;
            request.ReviewedBy = CurrentEmployee;
            request.ReviewRemarks = reviewRemarks;
            _db.SaveChanges();

            TempData["Success"] = $"Request {request.RequestNumber} rejected.";
            return RedirectToAction("AllRequests");
        }

        // ─────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────
        private string GenerateRequestNumber()
        {
            string datePart = DateTime.Now.ToString("yyyyMMdd");
            int count = _db.MaterialRequests.Count(r => r.RequestNumber.Contains(datePart)) + 1;
            return $"MRQ-{datePart}-{count:D4}";
        }

        private SelectList MaterialSelectList(int? selectedId)
        {
            var items = _db.MaterialPurchases
                           .OrderBy(p => p.MaterialName)
                           .Select(p => new
                           {
                               p.Id,
                               Display = p.MaterialCode + " — " + p.MaterialName
                           })
                           .ToList();
            return new SelectList(items, "Id", "Display", selectedId);
        }

        private SelectList DepartmentList()
        {
            var depts = new[]
            {
                "Operations", "Maintenance", "Electrical", "Civil",
                "Instrumentation", "Safety & Environment", "IT",
                "Finance", "HR", "Stores", "Projects"
            };
            return new SelectList(depts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _db.Dispose();
            base.Dispose(disposing);
        }
    }
}