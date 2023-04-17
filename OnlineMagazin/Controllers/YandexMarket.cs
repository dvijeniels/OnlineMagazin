using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
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
    public class YandexMarketController : ControllerBase
    {
        private readonly OnlineMagazinContext _context;
        private readonly ILogger<YandexMarketController> _logger;

        public YandexMarketController(ILogger<YandexMarketController> logger, OnlineMagazinContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {

            var feed = new YandexMarket
            {
                Version = "2.0",
                Shop = new YandexMarketShop
                {
                    Name = "Vector Строй Маркет",
                    Company = "Строительный Маркет",
                    Url = "https://pskanker.ru",
                    Currencies = new List<YandexMarketCurrency>
                    {
                        new YandexMarketCurrency()
                        {
                            Id = "RUB",
                            Rate = "1"
                        }
                    },
                    Categories = new List<YandexMarketCategory>(),
                    Offers = new List<YandexMarketOffer>()
                }
            };

            foreach (var category in _context.Category.ToList())
            {
                string categoryText = Regex.Replace(category.CategoriName, "&", "");
                var offerCategory = new YandexMarketCategory
                {
                    Id = category.CategoryId.ToString(),
                    Name = categoryText,
                };

                feed.Shop.Categories.Add(offerCategory);
            }
            foreach (var product in _context.Products.ToList())
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(product.Icerik.Substring(0, 240));
                string plainText = doc.DocumentNode.InnerText;
                plainText = Regex.Replace(plainText, @"\s+", " ");
                plainText = Regex.Replace(plainText, @"&laquo;|&raquo;|&nbsp;|&", " ");
                var offer = new YandexMarketOffer
                {
                    Id = product.ProductId.ToString(),
                    Available = "true",
                    Url = "https://pskanker.ru/Home/ProductDetail/" + product.ProductId,
                    Price = (decimal)product.Price,
                    CurrencyId = "RUB",
                    CategoryId = product.CategoryId.ToString(),
                    Picture = "https://pskanker.ru/image/" + product.Foto,
                    Pickup= "true",
                    Delivery = "true",
                    Name = product.Baslik,
                    Description = plainText,
                };

                feed.Shop.Offers.Add(offer);
            }
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    var serializer = new XmlSerializer(typeof(YandexMarket));
                    serializer.Serialize(writer, feed);
                }
                return File(stream.ToArray(), "application/xml");
            }
        }
    }

    [XmlRoot(ElementName = "yml_catalog")]
    public class YandexMarket
    {
        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; } = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "shop")]
        public YandexMarketShop Shop { get; set; }
    }

    public class YandexMarketShop
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "company")]
        public string Company { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlArray("currencies")]
        [XmlArrayItem("currency")]
        public List<YandexMarketCurrency> Currencies { get; set; }

        [XmlArray("categories")]
        [XmlArrayItem("category")]
        public List<YandexMarketCategory> Categories { get; set; }

        [XmlArray("offers")]
        [XmlArrayItem("offer")]
        public List<YandexMarketOffer> Offers { get; set; }
    }

    public class YandexMarketCurrency
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "rate")]
        public string Rate { get; set; }
    }

    public class YandexMarketCategory
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "parentId")]
        public string ParentId { get; set; }

        [XmlText]
        public string Name { get; set; }
    }

    public class YandexMarketOffer
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "available")]
        public string Available { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "price")]
        public decimal Price { get; set; }

        [XmlElement(ElementName = "currencyId")]
        public string CurrencyId { get; set; }

        [XmlElement(ElementName = "categoryId")]
        public string CategoryId { get; set; }

        [XmlElement(ElementName = "picture")]
        public string Picture { get; set; }

        [XmlElement(ElementName = "pickup")]
        public string Pickup { get; set; }

        [XmlElement(ElementName = "delivery")]
        public string Delivery { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }
    }
}