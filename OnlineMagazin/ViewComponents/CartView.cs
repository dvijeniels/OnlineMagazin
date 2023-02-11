using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class CartView : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public CartView(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Carts>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag.cart = cart;
                var CartSum = cart.Sum(item => item.Products.Price * item.qty);
                ViewBag.FinalPrice = CartSum;
                ViewBag.CountProduct = cart.Count();
            }
            
            return View(cart);
        }
    }
}
