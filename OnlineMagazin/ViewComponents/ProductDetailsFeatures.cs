using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class ProductDetailsFeatures : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public ProductDetailsFeatures(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int? id)
        {
            //var onlineMagazinContext = _context.ProductFeatures.Include(p => p.CategoryFeature).Distinct().ToList();

            //var list = _context.ProductFeatures.Select(a => a.Value).ToList();
            //var q = from x in list
            //        group x by x into g
            //        let count = g.Count()
            //        orderby count descending
            //        select new { Value = g.Key, Count = count };
            //ViewBag.Value = q.ToList();
            //return View(onlineMagazinContext);



            List<ProductFeatures> productFeatures = _context.ProductFeatures.Include(a => a.CategoryFeature).Where(a => a.ProductId == id).ToList();
            ViewBag.FeatureNameCount = _context.ProductFeatures.Where(a=>a.ProductId==id).Select(a => a.CategoryFeature.FeatureName).Distinct().ToList();
            return View(productFeatures.Distinct());
        }
    }
}
