using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductsController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var onlineMagazinContext = _context.Products.Include(p => p.Category);
            return View(await onlineMagazinContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Baslik,ProductDate,Icerik,FotoFile,CategoryId,Price")] Products products, IFormFile files)
        {
            if (ModelState.IsValid)
            {
                if (products.FotoFile.Count < 7)
                {
                    int i = 0;
                    string[] FotoArray = new string[6];
                    foreach (var photo in products.FotoFile)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(photo.FileName);
                        string extension = Path.GetExtension(photo.FileName);
                        FotoArray[i] = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/image/", fileName);
                        Image img = Image.FromStream(photo.OpenReadStream(), true, true);
                        var newImg = new Bitmap(200, 200);
                        using (var a = Graphics.FromImage(newImg))
                        {
                            a.DrawImage(img, 0, 0, 200, 200);
                            newImg.Save(path);
                        }
                        i++;
                    }
                    products.Foto = FotoArray[0];
                    products.Foto2 = FotoArray[1];
                    products.Foto3 = FotoArray[2];
                    products.Foto4 = FotoArray[3];
                    products.Foto5 = FotoArray[4];
                    products.Foto6 = FotoArray[5];
                    _context.Add(products);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.ImgCount = products.FotoFile.Count;
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            return View(products);
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductDate,Baslik,Icerik,FotoFile,FotoFile2,FotoFile3,FotoFile4,FotoFile5,FotoFile6,CategoryId,Price")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (products.FotoFile.Count < 7)
                {
                //Save image wwwRoow/allimage
                int i = 0;
                string[] FotoArray = new string[6];
                foreach (var photo in products.FotoFile)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(photo.FileName);
                    string extension = Path.GetExtension(photo.FileName);
                    FotoArray[i] = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await photo.CopyToAsync(fileStream);
                    }
                    i++;
                }
                products.Foto = FotoArray[0];
                products.Foto2 = FotoArray[1];
                products.Foto3 = FotoArray[2];
                products.Foto4 = FotoArray[3];
                products.Foto5 = FotoArray[4];
                products.Foto6 = FotoArray[5];
                //Save image wwwRoow/allimage
                try
                {
                    products.ProductDate = DateTime.Now.Date;
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                }
                ViewBag.ImgCount = products.FotoFile.Count;
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            //Удаление фото находящееся в папке
            deleteImage(products.Foto);
            deleteImage(products.Foto2);
            deleteImage(products.Foto3);
            deleteImage(products.Foto4);
            deleteImage(products.Foto5);
            deleteImage(products.Foto6);
            //Удаление фото находящееся в папке
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private void deleteImage(string image)
        {
            if (image!=null)
            {
                var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", image);
                if (System.IO.File.Exists(imagePath))
                { System.IO.File.Delete(imagePath); }
            }
        }
        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
