using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    public class UyesController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UyesController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Uyes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Uye.ToListAsync());
        }

        // GET: Uyes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uye
                .FirstOrDefaultAsync(m => m.UyeId == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }

        // GET: Uyes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uyes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UyeId,KullaniciAdi,Email,PhoneNumber,Region,City,Description,BirthDate,Address,Address2,Sifre,ConfirmPassword,AdSoyad,ResimFile,Yetki")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                
                    //Save image wwwRoow/allimage
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(uye.ResimFile.FileName);
                    string extension = Path.GetExtension(uye.ResimFile.FileName);
                    uye.Foto = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await uye.ResimFile.CopyToAsync(fileStream);
                    }
                
                //Save image wwwRoow/allimage
                _context.Add(uye);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uye);
        }

        // GET: Uyes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var uye = await _context.Uye.FindAsync(id);
            if (uye == null)
            {
                return NotFound();
            }
            return View(uye);
        }

        // POST: Uyes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UyeId, KullaniciAdi, Email, PhoneNumber, Region, City, Description, BirthDate, Address, Address2, Sifre, ConfirmPassword, AdSoyad, ResimFile, Yetki")] Uye uye)
        {
            if (id != uye.UyeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                    //Save image wwwRoow/allimage
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(uye.ResimFile.FileName);
                    string extension = Path.GetExtension(uye.ResimFile.FileName);
                    uye.Foto = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await uye.ResimFile.CopyToAsync(fileStream);
                    }
                
                //Save image wwwRoow/allimage
                try
                {
                    _context.Update(uye);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UyeExists(uye.UyeId))
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
            return View(uye);
        }

        // GET: Uyes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uye = await _context.Uye
                .FirstOrDefaultAsync(m => m.UyeId == id);
            if (uye == null)
            {
                return NotFound();
            }

            return View(uye);
        }

        // POST: Uyes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uye = await _context.Uye.FindAsync(id);
            // Удаление фото находящееся в папке
            if (uye.Foto != null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", uye.Foto);
                if (System.IO.File.Exists(imagePath))
                { System.IO.File.Delete(imagePath); }
            }
            //Удаление фото находящееся в папке
            _context.Uye.Remove(uye);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UyeExists(int id)
        {
            return _context.Uye.Any(e => e.UyeId == id);
        }
    }
}
