using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Seller + "," + SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;
        public ProductsController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index()
        {
            return View(_db.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTags).ToList());
        }


        //Create
        public IActionResult Create()
        {
            ViewBag.ProductTypeId = new SelectList(_db.ProductTypes, "Id", "ProductType");
            ViewBag.TagId = new SelectList(_db.SpecialTags, "Id", "SpecialTag");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products obj, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == obj.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "Item with this name already exists";
                    ViewBag.ProductTypeId = new SelectList(_db.ProductTypes, "Id", "ProductType");
                    ViewBag.TagId = new SelectList(_db.SpecialTags, "Id", "SpecialTag");
                    return View(obj);
                }
                if (image != null)
                {
                    string directoryPath = Path.Combine(_he.WebRootPath, "Images");

                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string filePath = Path.Combine(directoryPath, Path.GetFileName(image.FileName));

                    await image.CopyToAsync(new FileStream(filePath, FileMode.Create));

                    obj.Image = "Images/" + image.FileName;
                }
                else
                {
                    obj.Image = "/Images/noimage.png";
                }

                _db.Products.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If ModelState is not valid, return to the view with the model to display validation errors
            ViewBag.ProductTypeId = new SelectList(_db.ProductTypes, "Id", "ProductType");
            ViewBag.TagId = new SelectList(_db.SpecialTags, "Id", "SpecialTag");
            return View(obj);
        }

        //Edit

        public IActionResult Edit(int? id)
        {
            ViewBag.ProductTypeId = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewBag.TagId = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");

            if (id == null) return NoContent();

            var item = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTags).FirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products obj, IFormFile? image)
        {

            ViewBag.ProductTypeId = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewBag.TagId = new SelectList(_db.SpecialTags.ToList(), "Id", "SpecialTag");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    obj.Image = "Images/" + image.FileName;
                }
                else
                {
                    //var existingItem = await _db.Products.FindAsync(obj.Id);
                    //obj.Image= existingItem.Image;
                    obj.Image = "Images/"; // since this cant be null
                }

                _db.Products.Update(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //Detail
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = _db.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTags).FirstOrDefault(c => c.Id == id);
            if (item == null) { return NotFound(); }
            return View(item);
        }

        //Delete
        public IActionResult Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var item = _db.Products.Include(c => c.ProductTypes).Include(d => d.SpecialTags).Where(c => c.Id == id).FirstOrDefault();
            if (item == null) { return NotFound(); }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var item = _db.Products.FirstOrDefault(c => c.Id == id);
            if (item == null) { return NotFound(); }
            _db.Remove(item);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
