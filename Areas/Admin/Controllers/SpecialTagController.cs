using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Seller + "," + SD.Role_Admin)]

    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags obj)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Add(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Special Tag has been created";
                return RedirectToAction("Index");   
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spTag = _db.SpecialTags.Find(id);
            if (spTag == null)
            {
                return NotFound();
            }
            return View(spTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTags obj)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Update(obj);
                await _db.SaveChangesAsync();

                TempData["success"] = "Special Tag has been Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var spTag = _db.SpecialTags.Find(id);
            if (spTag == null)
            {
                return NotFound();
            }
            return View(spTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(SpecialTags obj)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var spTag = _db.SpecialTags.Find(id);
            if (spTag == null)
            {
                return NotFound();
            }
            return View(spTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTags obj)
        {
            if (id == null || obj == null)
            {
                return NotFound();
            }
            if (id != obj.Id)
            {
                return NotFound();
            }

            var spTag = _db.SpecialTags.Find(id);
            if (spTag == null)
            {
                return NotFound();

            }
            if (ModelState.IsValid)
            {

                _db.SpecialTags.Remove(spTag);
                await _db.SaveChangesAsync();
                TempData["success"] = "Product Type has be Deleted Successfully";
                return RedirectToAction("Index");

            }
            return View(spTag);
        }


    }
}
