using AspNetCore.SEOHelper.Sitemap;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace OnlineMagazin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OnlineMagazinContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string plainText = "";
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
        public string CreateYMLForYandex()
        {
            var stream = new YamlStream();
            var mapping = new YamlMappingNode();
            mapping.Add("name", "Vector Строй Маркет");

            // Add the "company" details
            var company = new YamlMappingNode();
            company.Add("name", "Vector Строй Маркет");
            company.Add("url", "https://pskanker.ru");
            mapping.Add("company", company);

            // Add the "currencies" section
            var currencies = new YamlMappingNode();
            var currency = new YamlMappingNode();
            currency.Add("id", "RUR");
            currency.Add("rate", "1");
            currencies.Add("currency", currency);
            mapping.Add("currencies", currencies);


            // Add the "categories" section
            var categories = new YamlSequenceNode();
            foreach (var category in _context.Category.ToList())
            {
                var categoryByOne = new YamlMappingNode();
                categoryByOne.Add("id", category.CategoryId.ToString());
                categoryByOne.Add("name", category.CategoriName.ToString());
                categories.Add(categoryByOne);
            }
            mapping.Add("categories", categories);

            // Add the "offers" section
            var offers = new YamlSequenceNode();
            foreach (var product in _context.Products.ToList())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(product.Icerik);
                plainText = doc.DocumentNode.InnerText;
                plainText = Regex.Replace(plainText, @"\s+", " ");
                plainText = Regex.Replace(plainText, @"&laquo;|&raquo;|&nbsp;", " ");
                var offer1 = new YamlMappingNode();
                offer1.Add("id", product.ProductId.ToString());
                offer1.Add("available", "true");
                offer1.Add("url", "https://pskanker.ru/Home/ProductDetail/"+product.ProductId);
                offer1.Add("price", product.Price.ToString());
                offer1.Add("currencyId", "RUR");
                offer1.Add("categoryId", product.CategoryId.ToString());
                offer1.Add("picture", "https://pskanker.ru/image/"+product.Foto);
                offer1.Add("delivery", "true");
                offer1.Add("name", product.Baslik.ToString());
                offer1.Add("description", plainText);
                offers.Add(offer1);
            }
            mapping.Add("offers", offers);
            var yamlDoc = new YamlDocument(mapping);
            stream.Add(yamlDoc);
            using (var writer = new StreamWriter("yandex_market.yml"))
            {
                stream.Save(writer, false);
            }
            return "yandex_market.yml";
        }
        public string CreateYMLForGoogle()
        {
            var products = new List<Dictionary<string, object>>();
            foreach (var product in _context.Products.Include(p => p.Category).ToList())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(product.Icerik);
                plainText = doc.DocumentNode.InnerText;
                plainText = Regex.Replace(plainText, @"\s+", " ");
                plainText = Regex.Replace(plainText, @"&laquo;|&raquo;|&nbsp;", " ");
                var productData = new Dictionary<string, object>
                {
                    { "id", product.ProductId },
                    { "title", product.Baslik },
                    { "description", plainText },
                    { "price", (decimal)product.Price },
                    { "brand", "" },
                    { "category", product.Category.CategoriName },
                    { "image_link", "https://pskanker.ru/image/"+product.Foto },
                    { "availability", "in stock" },
                    { "condition", "new" },
                    { "link", "https://pskanker.ru/Home/ProductDetail/"+product.ProductId },
                    { "google_product_category", "Строительные материалы >"+product.Category.CategoriName }
                };
                products.Add(productData);
            }
            var serializer = new SerializerBuilder().Build();
            string yaml = serializer.Serialize(products);
            System.IO.File.WriteAllText("google_market.yaml", yaml);
            return "Успешно создан";
        }
    }
}
