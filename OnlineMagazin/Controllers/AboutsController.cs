using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libwebp.Net.utility;
using Libwebp.Net;
using Libwebp.Standard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AboutsController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AboutsController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Abouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.About.ToListAsync());
        }

        // GET: Abouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // GET: Abouts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Abouts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MetinIcerik,ResimFile")] About about)
        {
            if (ModelState.IsValid)
            {
                //Save image wwwRoow/allimage
                string wwwRootPath =_hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(about.ResimFile.FileName);
                string extension = Path.GetExtension(about.ResimFile.FileName);
                about.Resim = fileName=fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath+"/image/",fileName);
                using(var fileStream=new FileStream(path,FileMode.Create))
                {
                    await about.ResimFile.CopyToAsync(fileStream);
                }
                //Save image wwwRoow/allimage
                _context.Add(about);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(about);
        }

        // GET: Abouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About.FindAsync(id);
            if (about == null)
            {
                return NotFound();
            }
            return View(about);
        }

        // POST: Abouts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MetinIcerik,ResimFile")] About about)
        {
            if (id != about.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(about.ResimFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(about.ResimFile.FileName);
                    var config = new WebpConfigurationBuilder().Preset(Preset.PHOTO).Output($"{fileName}.webp").Build();
                    var encoder = new WebpEncoder(config);
                    var ms = new MemoryStream();
                    about.ResimFile.CopyTo(ms);
                    Stream fs = await encoder.EncodeAsync(ms, about.ResimFile.FileName);
                    about.Resim = fileName = fileName + DateTime.Now.ToString("yymsf") + ".webp";
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await fs.CopyToAsync(fileStream);
                    }
                }
                else about.Resim = await _context.About.Where(x => x.Id == id).Select(x => x.Resim).FirstOrDefaultAsync();

                try
                {
                    _context.Update(about);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutExists(about.Id))
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
            return View(about);
        }

        // GET: Abouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var about = await _context.About
                .FirstOrDefaultAsync(m => m.Id == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var about = await _context.About.FindAsync(id);

            //Удаление фото находящееся в папке
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", about.Resim);
            if (System.IO.File.Exists(imagePath))
            { System.IO.File.Delete(imagePath); }
            //Удаление фото находящееся в папке

            _context.About.Remove(about);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AboutExists(int id)
        {
            return _context.About.Any(e => e.Id == id);
        }
    }
}
