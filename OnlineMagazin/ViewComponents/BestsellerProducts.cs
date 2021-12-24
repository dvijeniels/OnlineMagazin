using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    public class BestsellerProducts:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public BestsellerProducts(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var productView = _context.Products.ToList();
            //List<Products> productView = _context.Products.ToList();
            return View(productView);
        }
    }
}
