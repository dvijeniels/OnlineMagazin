using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libwebp.Net.utility;
using Libwebp.Net;
using Libwebp.Standard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Create([Bind("SliderId,SliderResimFile,SliderName,SliderDescription")] Sliders sliders)
        {
            if (ModelState.IsValid)
            {
                //Save image wwwRoow/allimage
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(sliders.SliderResimFile.FileName);
                var config = new WebpConfigurationBuilder().Preset(Preset.PHOTO).Output($"{fileName}.webp").Build();
                var encoder = new WebpEncoder(config);
                var ms = new MemoryStream();
                sliders.SliderResimFile.CopyTo(ms);
                Stream fs = await encoder.EncodeAsync(ms, sliders.SliderResimFile.FileName);
                sliders.SliderResim = fileName = fileName + DateTime.Now.ToString("yymsf") + ".webp";
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fs.CopyToAsync(fileStream);
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
        public async Task<IActionResult> Edit(int id, [Bind("SliderId,SliderResimFile,SliderName,SliderDescription")] Sliders sliders)
        {
            var SliderResimGet=_context.Sliders.Where(x=>x.SliderId==id).Select(x=>x.SliderResim).First();
            if (id != sliders.SliderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(sliders.SliderResimFile.FileName);
                var config = new WebpConfigurationBuilder().Preset(Preset.PHOTO).Output($"{fileName}.webp").Build();
                var encoder = new WebpEncoder(config);
                var ms = new MemoryStream();
                sliders.SliderResimFile.CopyTo(ms);
                Stream fs = await encoder.EncodeAsync(ms, sliders.SliderResimFile.FileName);
                sliders.SliderResim = fileName = fileName + DateTime.Now.ToString("yymsf") + ".webp";
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fs.CopyToAsync(fileStream);
                }
                deleteImage(SliderResimGet);
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
        private void deleteImage(string image)
        {
            if (image != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", image);
                if (System.IO.File.Exists(imagePath))
                { System.IO.File.Delete(imagePath); }
            }
        }
        private bool SlidersExists(int id)
        {
            return _context.Sliders.Any(e => e.SliderId == id);
        }
    }
}
