using AspNetCore.SEOHelper.Sitemap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminController(OnlineMagazinContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult AdminIndex()
        {
            List<Category> kategoriler = _context.Category.ToList();
            ViewBag.catSay = kategoriler.Count;
            List<Products> product = _context.Products.ToList();
            ViewBag.productSay = product.Count;
            List<Duyrular> duyurular = _context.Duyrular.ToList();
            ViewBag.duyuruSay = duyurular.Count;
            List<Message> mesaj = _context.Message.ToList();
            ViewBag.mesajSay = mesaj.Count;
            return View();
        }
        public string CreateSitemapInRootDirectory()
        {
            var list = new List<SitemapNode>();
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 1.0, Url = "https://pskanker.ru/", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/HomeIndex/", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/GetProducts", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/About", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/Message", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/WishList", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/Cart", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Identity/Account/Login", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Identity/Account/Register", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Identity/Account/ForgotPassword", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Identity/Account/ResendEmailConfirmation", Frequency = SitemapFrequency.Always });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/Delivery", Frequency = SitemapFrequency.Monthly });
            list.Add(new SitemapNode { LastModified = DateTime.UtcNow, Priority = 0.8, Url = "https://pskanker.ru/Home/Payment", Frequency = SitemapFrequency.Monthly });
            foreach (var item in _context.Category.ToList())
            {
                list.Add(
                   new SitemapNode()
                   {
                       LastModified = DateTime.UtcNow,
                       Url = "https://pskanker.ru/Home/ProductsFromCategory/" + item.CategoryId,
                       Frequency = SitemapFrequency.Monthly,
                       Priority = 0.8
                   });
            }
            foreach (var item in _context.Products.ToList())
            {
                list.Add(
                   new SitemapNode()
                   {
                       LastModified = DateTime.UtcNow,
                       Url = "https://pskanker.ru/Home/ProductDetail/" + item.ProductId,
                       Frequency = SitemapFrequency.Weekly,
                       Priority = 0.7
                   });
            }
            new SitemapDocument().CreateSitemapXML(list, _hostEnvironment.ContentRootPath);
            return "sitemap.xml";
        }
    }
}
