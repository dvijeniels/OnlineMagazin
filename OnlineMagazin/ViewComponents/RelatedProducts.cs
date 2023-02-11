using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class RelatedProducts : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public RelatedProducts(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int? id)
        {
            var products = _context.Products.Where(x=>x.ProductId==id).FirstOrDefault();
            List<Products> relatedProducts = _context.Products.Where(a => a.CategoryId == products.CategoryId).ToList();
            var itemToRemove = relatedProducts.Single(r => r.ProductId == id);
            relatedProducts.Remove(itemToRemove);
            return View(relatedProducts);
            
        }
    }
}
