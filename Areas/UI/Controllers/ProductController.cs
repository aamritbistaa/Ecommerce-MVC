using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.UI.Controllers
{
    [Area("UI")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ApplicationDbContext db,ILogger<ProductController> logger)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(c=>c.ProductTypes).Include(c=>c.SpecialTags).ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var product = _db.Products.Include(c=>c.ProductTypes).FirstOrDefault(c=>c.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult ProductDetail(int? id)
        {
            List<Products> products = new List<Products>();

            if(id == null) return NotFound();
            var product = _db.Products.Include(c=>c.ProductTypes).FirstOrDefault(c=>c.Id==id);
            if (product == null) return NotFound();
            products = HttpContext.Session.Get<List<Products>>("products");
            if (products == null)
            {
                products= new List<Products>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction("Index");
        }

        [ActionName("Remove")]
        public IActionResult RemoveItemCart(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null) return NotFound();
            var product = products.FirstOrDefault(c=>c.Id == id);
            if(product == null) return NotFound();
            products.Remove(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int? id)
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if (products != null) { return NotFound(); }
            var product = products.FirstOrDefault(c => c.Id == id);
            if (product == null) return NotFound();
            products.Remove(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction("Index");
        }
        public IActionResult Cart()
        {
            List<Products> products = HttpContext.Session.Get<List<Products>>("products");
            if(products == null) { products = new List<Products>(); }
            return View(products);
        }
    }
}
