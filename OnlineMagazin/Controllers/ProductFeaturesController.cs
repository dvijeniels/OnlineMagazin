using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductFeaturesController : Controller
    {
        private readonly OnlineMagazinContext _context;

        public ProductFeaturesController(OnlineMagazinContext context)
        {
            _context = context;
        }

        // GET: ProductFeatures

        public JsonResult GetCategory(int id)
        {
            var categorys = _context.CategoryFeature.Where(p => p.CategoryId == id).ToList();

            return Json(new SelectList(categorys, "CategoryId", "CategoriName"));
        }
        public JsonResult GetProductByCategory(int id)
        {
            var product = _context.Products.Where(p=>p.CategoryId==id).ToList();

            return Json(new SelectList(product, "ProductId", "Baslik"));
        }
        public JsonResult GetCategoryFeaturesByCategory(int id)
        {
            var categoryFeatures = _context.CategoryFeature.Where(p => p.CategoryId == id).ToList();

            return Json(new SelectList(categoryFeatures, "CategoryFeatureId", "FeatureName"));
        }
        // GET: ProductFeatures/Create
        public async Task<IActionResult> Index()
        {
            var onlineMagazinContext = _context.ProductFeatures.Include(p => p.Category).Include(p => p.CategoryFeature).Include(p => p.Products);
            return View(await onlineMagazinContext.ToListAsync());
        }

        // GET: ProductFeatures1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFeatures = await _context.ProductFeatures
                .Include(p => p.Category)
                .Include(p => p.CategoryFeature)
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.ProductFeatureId == id);
            if (productFeatures == null)
            {
                return NotFound();
            }

            return View(productFeatures);
        }

        // GET: ProductFeatures1/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName");
            ViewData["CategoryFeatureId"] = new SelectList(_context.CategoryFeature, "CategoryFeatureId", "FeatureName");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Baslik");
            return View();
        }

        // POST: ProductFeatures1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductFeatureId,CategoryId,CategoryFeatureId,ProductId,Value")] ProductFeatures productFeatures)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productFeatures);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", productFeatures.CategoryId);
            ViewData["CategoryFeatureId"] = new SelectList(_context.CategoryFeature, "CategoryFeatureId", "FeatureName", productFeatures.CategoryFeatureId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Baslik", productFeatures.ProductId);
            return View(productFeatures);
        }

        // GET: ProductFeatures1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFeatures = await _context.ProductFeatures.FindAsync(id);
            if (productFeatures == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", productFeatures.CategoryId);
            ViewData["CategoryFeatureId"] = new SelectList(_context.CategoryFeature, "CategoryFeatureId", "FeatureName", productFeatures.CategoryFeatureId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Baslik", productFeatures.ProductId);
            return View(productFeatures);
        }

        // POST: ProductFeatures1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductFeatureId,CategoryId,CategoryFeatureId,ProductId,Value")] ProductFeatures productFeatures)
        {
            if (id != productFeatures.ProductFeatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productFeatures);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductFeaturesExists(productFeatures.ProductFeatureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", productFeatures.CategoryId);
            ViewData["CategoryFeatureId"] = new SelectList(_context.CategoryFeature, "CategoryFeatureId", "FeatureName", productFeatures.CategoryFeatureId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "Baslik", productFeatures.ProductId);
            return View(productFeatures);
        }

        // GET: ProductFeatures1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productFeatures = await _context.ProductFeatures
                .Include(p => p.Category)
                .Include(p => p.CategoryFeature)
                .Include(p => p.Products)
                .FirstOrDefaultAsync(m => m.ProductFeatureId == id);
            if (productFeatures == null)
            {
                return NotFound();
            }

            return View(productFeatures);
        }

        // POST: ProductFeatures1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productFeatures = await _context.ProductFeatures.FindAsync(id);
            _context.ProductFeatures.Remove(productFeatures);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductFeaturesExists(int id)
        {
            return _context.ProductFeatures.Any(e => e.ProductFeatureId == id);
        }
    }
}
