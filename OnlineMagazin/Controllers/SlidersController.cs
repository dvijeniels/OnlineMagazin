using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    public class SlidersController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SlidersController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        
        // GET: Sliders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        // GET: Sliders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliders = await _context.Sliders
                .FirstOrDefaultAsync(m => m.SliderId == id);
            if (sliders == null)
            {
                return NotFound();
            }

            return View(sliders);
        }

        // GET: Sliders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SliderId,SliderResimFile")] Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Save image wwwRoow/allimage
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(sliders.SliderResimFile.FileName);
                string extension = Path.GetExtension(sliders.SliderResimFile.FileName);
                sliders.SliderResim = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await sliders.SliderResimFile.CopyToAsync(fileStream);
                }
                //Save image wwwRoow/allimage
                _context.Add(sliders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sliders);
        }

        // GET: Sliders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliders = await _context.Sliders.FindAsync(id);
            if (sliders == null)
            {
                return NotFound();
            }
            return View(sliders);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SliderId,SliderResimFile")] Sliders sliders)
        {
            if (id != sliders.SliderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Save image wwwRoow/allimage
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(sliders.SliderResimFile.FileName);
                string extension = Path.GetExtension(sliders.SliderResimFile.FileName);
                sliders.SliderResim = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await sliders.SliderResimFile.CopyToAsync(fileStream);
                }
                //Save image wwwRoow/allimage
                try
                {
                    _context.Update(sliders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlidersExists(sliders.SliderId))
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
            return View(sliders);
        }

        // GET: Sliders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sliders = await _context.Sliders
                .FirstOrDefaultAsync(m => m.SliderId == id);
            if (sliders == null)
            {
                return NotFound();
            }

            return View(sliders);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sliders = await _context.Sliders.FindAsync(id);
            //Удаление фото находящееся в папке
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", sliders.SliderResim);
            if (System.IO.File.Exists(imagePath))
            { System.IO.File.Delete(imagePath); }
            //Удаление фото находящееся в папке
            _context.Sliders.Remove(sliders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SlidersExists(int id)
        {
            return _context.Sliders.Any(e => e.SliderId == id);
        }
    }
}
