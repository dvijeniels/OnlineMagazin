using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var onlineMagazinContext = _context.Products.Include(p => p.Category).Include(p => p.TypeProduct);
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
                .Include(p => p.Category).Include(p => p.TypeProduct)
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
            ViewData["TypeId"] = new SelectList(_context.TypeProducts, "TypeId", "TypeName");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Baslik,ProductDate,Icerik,FotoFile,CategoryId,TypeId,Price")] Products products)
        {
            if (ModelState.IsValid)
            {
                if (products.FotoFile != null)
                {
                    if (products.FotoFile.Count < 7)
                    {
                        int i = 0;
                        string[] FotoArray = new string[6];
                        foreach (var photo in products.FotoFile.OrderBy(x=>x.FileName))
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
                            //string wwwRootPath = _hostEnvironment.WebRootPath;
                            //string fileName = Path.GetFileNameWithoutExtension(photo.FileName);
                            //string extension = Path.GetExtension(photo.FileName);
                            //FotoArray[i] = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                            //string path = Path.Combine(wwwRootPath + "/image/", fileName);
                            //Image img = Image.FromStream(photo.OpenReadStream(), true, true);
                            //if (img.Height == img.Width)
                            //{
                            //    if (img.Height > img.Width)
                            //    {
                            //        var newImg = new Bitmap(img.Height, img.Height);
                            //        using (var a = Graphics.FromImage(newImg))
                            //        {
                            //            a.DrawImage(img, 0, 0, newImg.Height, newImg.Height);
                            //            newImg.Save(path);
                            //        }
                            //        i++;
                            //    }
                            //    else
                            //    {
                            //        var newImg = new Bitmap(img.Width, img.Width);
                            //        using (var a = Graphics.FromImage(newImg))
                            //        {
                            //            a.DrawImage(img, 0, 0, newImg.Width, newImg.Width);
                            //            newImg.Save(path);
                            //        }
                            //        i++;
                            //    }
                            //}
                        }
                        products.Foto = FotoArray[0];
                        products.Foto2 = FotoArray[1];
                        products.Foto3 = FotoArray[2];
                        products.Foto4 = FotoArray[3];
                        products.Foto5 = FotoArray[4];
                        products.Foto6 = FotoArray[5];
                        products.ProductDate = DateTime.Now.Date;
                        _context.Add(products);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ViewBag.ImgCount = products.FotoFile.Count;
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.TypeProducts, "TypeId", "TypeName", products.TypeId);
            return View(products);
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var products = await _context.Products.FindAsync(id);
            var products = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.TypeProducts, "TypeId", "TypeName", products.TypeId);
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Baslik,ProductDate,Foto,Foto2,Foto3,Foto4,Foto5,Foto6,Icerik,FotoFile,CategoryId,TypeId,Price")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (products.FotoFile != null)
                {
                    if (products.FotoFile.Count < 7)
                    {
                        int i = 0;
                        string[] FotoArray = new string[6];
                        foreach (var photo in products.FotoFile.OrderBy(x=>x.FileName))
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
                        deleteImage(products.Foto);
                        deleteImage(products.Foto2);
                        deleteImage(products.Foto3);
                        deleteImage(products.Foto4);
                        deleteImage(products.Foto5);
                        deleteImage(products.Foto6);
                        products.Foto = FotoArray[0];
                        products.Foto2 = FotoArray[1];
                        products.Foto3 = FotoArray[2];
                        products.Foto4 = FotoArray[3];
                        products.Foto5 = FotoArray[4];
                        products.Foto6 = FotoArray[5];
                    }
                }
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName", products.CategoryId);
            ViewData["TypeId"] = new SelectList(_context.TypeProducts, "TypeId", "TypeName", products.TypeId);
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
                .Include(p => p.Category).Include(p => p.TypeProduct)
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
            if (image != null)
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
