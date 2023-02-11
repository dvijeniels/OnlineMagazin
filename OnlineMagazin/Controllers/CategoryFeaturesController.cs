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
    public class CategoryFeaturesController : Controller
    {
        private readonly OnlineMagazinContext _context;

        public CategoryFeaturesController(OnlineMagazinContext context)
        {
            _context = context;
        }

        // GET: CategoryFeatures
        public async Task<IActionResult> Index()
        {
            var onlineMagazinContext = _context.CategoryFeature.Include(c => c.Category);
            return View(await onlineMagazinContext.ToListAsync());
        }

        // GET: CategoryFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFeature = await _context.CategoryFeature
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CategoryFeatureId == id);
            if (categoryFeature == null)
            {
                return NotFound();
            }

            return View(categoryFeature);
        }

        // GET: CategoryFeatures/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName");
            return View();
        }

        // POST: CategoryFeatures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryFeatureId,CategoryId,FeatureName,Unit")] CategoryFeature categoryFeature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryFeature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", categoryFeature.CategoryId);
            return View(categoryFeature);
        }

        // GET: CategoryFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFeature = await _context.CategoryFeature.FindAsync(id);
            if (categoryFeature == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", categoryFeature.CategoryId);
            return View(categoryFeature);
        }

        // POST: CategoryFeatures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryFeatureId,CategoryId,FeatureName,Unit")] CategoryFeature categoryFeature)
        {
            if (id != categoryFeature.CategoryFeatureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryFeature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryFeatureExists(categoryFeature.CategoryFeatureId))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", categoryFeature.CategoryId);
            return View(categoryFeature);
        }

        // GET: CategoryFeatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryFeature = await _context.CategoryFeature
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CategoryFeatureId == id);
            if (categoryFeature == null)
            {
                return NotFound();
            }

            return View(categoryFeature);
        }

        // POST: CategoryFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryFeature = await _context.CategoryFeature.FindAsync(id);
            _context.CategoryFeature.Remove(categoryFeature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryFeatureExists(int id)
        {
            return _context.CategoryFeature.Any(e => e.CategoryFeatureId == id);
        }
    }
}
