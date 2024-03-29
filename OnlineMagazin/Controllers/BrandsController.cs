﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libwebp.Net.utility;
using Libwebp.Net;
using Libwebp.Standard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    public class BrandsController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BrandsController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
              return View(await _context.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandsId == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandsId,BrandsResimFile,BrandsName")] Brands brands)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(brands.BrandsResimFile.FileName);
                var config = new WebpConfigurationBuilder().Preset(Preset.PHOTO).Output($"{fileName}.webp").Build();
                var encoder = new WebpEncoder(config);
                var ms = new MemoryStream();
                brands.BrandsResimFile.CopyTo(ms);
                Stream fs = await encoder.EncodeAsync(ms, brands.BrandsResimFile.FileName);
                brands.BrandsResim = fileName = fileName + DateTime.Now.ToString("yymsf") + ".webp";
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await fs.CopyToAsync(fileStream);
                }
                _context.Add(brands);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brands);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands.FindAsync(id);
            if (brands == null)
            {
                return NotFound();
            }
            return View(brands);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandsId,BrandsResimFile,BrandsName")] Brands brands)
        {
            var BrandImageGet = _context.Brands.Where(x => x.BrandsId == id).Select(x => x.BrandsResim).First();
            if (id != brands.BrandsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(brands.BrandsResimFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(brands.BrandsResimFile.FileName);
                        var config = new WebpConfigurationBuilder().Preset(Preset.PHOTO).Output($"{fileName}.webp").Build();
                        var encoder = new WebpEncoder(config);
                        var ms = new MemoryStream();
                        brands.BrandsResimFile.CopyTo(ms);
                        Stream fs = await encoder.EncodeAsync(ms, brands.BrandsResimFile.FileName);
                        brands.BrandsResim = fileName = fileName + DateTime.Now.ToString("yymsf") + ".webp";
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await fs.CopyToAsync(fileStream);
                        }
                        deleteImage(BrandImageGet);
                    }
                    else brands.BrandsResim = await _context.Brands.Where(x=>x.BrandsId==id).Select(x => x.BrandsResim).FirstOrDefaultAsync();
                    _context.Update(brands);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandsExists(brands.BrandsId))
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
            return View(brands);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brands = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandsId == id);
            if (brands == null)
            {
                return NotFound();
            }

            return View(brands);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Brands == null)
            {
                return Problem("Entity set 'OnlineMagazinContext.Brands'  is null.");
            }
            var brands = await _context.Brands.FindAsync(id);
            if (brands != null)
            {
                _context.Brands.Remove(brands);
            }
            
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
        private bool BrandsExists(int id)
        {
          return _context.Brands.Any(e => e.BrandsId == id);
        }
    }
}
