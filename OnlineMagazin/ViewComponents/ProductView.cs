using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    public class ProductView:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public ProductView(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int ? id=1)
        {
            var productView = _context.Products.FirstOrDefault(x => x.ProductId == id);
            //List<Products> productView = _context.Products.ToList();
            return View(productView);
        }
    }
}
