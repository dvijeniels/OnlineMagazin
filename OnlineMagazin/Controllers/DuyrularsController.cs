
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DuyrularsController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DuyrularsController(OnlineMagazinContext context,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Duyrulars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Duyrular.ToListAsync());
        }

        // GET: Duyrulars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyrular = await _context.Duyrular
                .FirstOrDefaultAsync(m => m.DuyuruId == id);
            if (duyrular == null)
            {
                return NotFound();
            }

            return View(duyrular);
        }

        // GET: Duyrulars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Duyrulars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DuyuruId,DuyuruAd,Duyuruicerik,DuyuruLink,DuyuruResimFile")] Duyrular duyrular)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(duyrular.DuyuruResimFile.FileName);
                string extension = Path.GetExtension(duyrular.DuyuruResimFile.FileName);
                duyrular.DuyuruResim = fileName=fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await duyrular.DuyuruResimFile.CopyToAsync(fileStream);
                }
                duyrular.DuyuruDate = DateTime.Now;
                _context.Add(duyrular);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(duyrular);
        }

        // GET: Duyrulars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyrular = await _context.Duyrular.FindAsync(id);
            if (duyrular == null)
            {
                return NotFound();
            }
            return View(duyrular);
        }

        // POST: Duyrulars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DuyuruId,DuyuruAd,Duyuruicerik,DuyuruLink,DuyuruResimFile")] Duyrular duyrular)
        {
            if (id != duyrular.DuyuruId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //First delete image and Save image wwwRoow/allimage
                //var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", duyrular.DuyuruResim);
                //if (System.IO.File.Exists(imagePath))
                //{ System.IO.File.Delete(imagePath); }
                //First delete image and Save image wwwRoow/allimage
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(duyrular.DuyuruResimFile.FileName);
                string extension = Path.GetExtension(duyrular.DuyuruResimFile.FileName);
                duyrular.DuyuruResim = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                duyrular.DuyuruDate = DateTime.Now;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await duyrular.DuyuruResimFile.CopyToAsync(fileStream);
                }
                //Save image wwwRoow/allimage
                try
                {
                    _context.Update(duyrular);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuyrularExists(duyrular.DuyuruId))
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
            return View(duyrular);
        }

        // GET: Duyrulars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var duyrular = await _context.Duyrular
                .FirstOrDefaultAsync(m => m.DuyuruId == id);
            if (duyrular == null)
            {
                return NotFound();
            }

            return View(duyrular);
        }

        // POST: Duyrulars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var duyrular = await _context.Duyrular.FindAsync(id);
            //Удаление фото находящееся в папке
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", duyrular.DuyuruResim);
            if (System.IO.File.Exists(imagePath))
            { System.IO.File.Delete(imagePath); }
            //Удаление фото находящееся в папке
            _context.Duyrular.Remove(duyrular);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DuyrularExists(int id)
        {
            return _context.Duyrular.Any(e => e.DuyuruId == id);
        }
    }
}
