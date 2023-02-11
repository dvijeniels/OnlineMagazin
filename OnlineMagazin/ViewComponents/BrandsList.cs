using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class BrandsList:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public BrandsList(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var brandView = _context.Brands.ToList();
            //List<Products> productView = _context.Products.ToList();
            return View(brandView);
        }
    }
}
