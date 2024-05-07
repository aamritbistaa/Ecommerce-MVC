using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Seller + "," + SD.Role_Admin)]

    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;            
        }
        public IActionResult Index()
        {
            return View(_db.ProductTypes.ToList());
        }
        
        public IActionResult Create() {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes obj)
        {
            if(ModelState.IsValid)
            {
                _db.ProductTypes.Add(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Product Type has been Created Successfully";

                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id) {
            if (id == null)
            {
                return NotFound();
            }

            var product = _db.ProductTypes.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes obj)
        {
            if(ModelState.IsValid)
            {
                _db.ProductTypes.Update(obj);
                await _db.SaveChangesAsync();

                TempData["success"] = "Product Type has been Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();

            }
            var product = _db.ProductTypes.Find(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ProductTypes obj)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _db.ProductTypes.Find(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductTypes obj)
        {
            if (id==null||obj==null)
            {
                return NotFound();
            }
            if(id!=obj.Id)
            {
                return NotFound();
            }
           
            var product = _db.ProductTypes.Find(id);
            if (product == null)
            {
                return NotFound();

            }
            if (ModelState.IsValid)
            {

                _db.ProductTypes.Remove(product);
                await _db.SaveChangesAsync();
                TempData["success"] = "Product Type has be Deleted Successfully";
                return RedirectToAction("Index");

            }
            return View(product);
        }


    }
}
