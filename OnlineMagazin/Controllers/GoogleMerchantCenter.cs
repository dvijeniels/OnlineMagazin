using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineMagazin.Data;

namespace MyApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class GoogleMerchantCenterController : ControllerBase
    {
        private readonly OnlineMagazinContext _context;
        private readonly ILogger<GoogleMerchantCenterController> _logger;

        public GoogleMerchantCenterController(ILogger<GoogleMerchantCenterController> logger, OnlineMagazinContext context)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetProductsXml()
        {
            var products = new List<GoogleProduct>();
            foreach (var product in _context.Products.ToList())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(product.Icerik.Substring(0, 240));
                string plainText = doc.DocumentNode.InnerText;
                plainText = Regex.Replace(plainText, @"\s+", " ");
                plainText = Regex.Replace(plainText, @"&laquo;|&raquo;|&nbsp;|&", " ");
                var offer = new GoogleProduct
                {
                    Id = product.ProductId.ToString(),
                    Title = product.Baslik,
                    Description = plainText,
                    Link = "https://pskanker.ru/Home/ProductDetail/" + product.ProductId,
                    ImageLink = "https://pskanker.ru/image/" + product.Foto,
                    Condition = "new",
                    Availability = "in_stock",
                    Price = product.Price+" RUB",
                    Brand = "Pskanker",
                    UpdateType= "merge",
                    Region= "RU-MOW",
                    Country= "RU"
                };

                products.Add(offer);
            }
            var xml = new XElement("rss",
            new XAttribute("version", "2.0"),
            new XAttribute("xmlns_g", "http://base.google.com/ns/1.0"),
            new XElement("channel",
                new XElement("title", "Все товары"),
                new XElement("link", "https://pskanker.ru/Home/GetProducts"),
                new XElement("description", "Продукты - Vector Строй маркет"),
                products.Select(product =>
                    new XElement("item",
                        new XElement("g_id", product.Id),
                        new XElement("title", product.Title),
                        new XElement("description", product.Description),
                        new XElement("g_link", product.Link),
                        new XElement("g_image_link", product.ImageLink),
                        new XElement("g_condition", product.Condition),
                        new XElement("g_availability", product.Availability),
                        new XElement("g_price", product.Price),
                        new XElement("g_brand", product.Brand),
                        new XElement("g_update_type", product.UpdateType),
                        new XElement("g_country", product.Country),
                        new XElement("g_region", new XAttribute("id",product.Region))
                        )
                    )
                )
            );
            return Content(xml.ToString(), "application/xml");
        }

    }
    public class GoogleProduct
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string Condition { get; set; }
        public string Availability { get; set; }
        public string Price { get; set; }
        public string Brand { get; set; }
        public string UpdateType { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}