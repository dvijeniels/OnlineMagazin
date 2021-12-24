using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.ViewComponents
{
    public class CartView : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public CartView(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var uyeID = HttpContext.Session.GetInt32("uyeId");
            var onlineMagazinContext = _context.Carts.Where(a => a.UyeId == uyeID && a.InOrder == false).Include(c => c.Products).Include(c => c.Uye);
            int ToplamRate = Convert.ToInt32(((from t in onlineMagazinContext select t.FinalPrice).Sum()));
            int countProduct = Convert.ToInt32(((from t in onlineMagazinContext select t.CartId).Count()));
            ViewBag.FinalPrice = ToplamRate;
            ViewBag.CountProduct = countProduct;
            return View(onlineMagazinContext.ToList());
        }
    }
}
