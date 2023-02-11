using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Data;
using System.Security.Principal;
using System.Linq;
using System.Security.Claims;
using OnlineMagazin.Models;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class OrderHistory : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        private readonly UserManager<OnlineMagazinUser> _userManager;
        public OrderHistory(OnlineMagazinContext context, UserManager<OnlineMagazinUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            //var order = _context.Orders.Where(u => u.UserId == userId).OrderByDescending(a => a.CreatedDate).Select(i => new OrderDetails()
            //{
            //    OrderId = i.OrderId,
            //    OrderNumber = i.OrderNumber,
            //    FirstAndLastName = i.FirstAndLastName,
            //    BuyingType = i.BuyingType,
            //    Comment = i.Comment,
            //    Address = i.Address,
            //    Address2 = i.Address2,
            //    Region = i.Region,
            //    City = i.City,
            //    PhoneNumber = i.PhoneNumber,
            //    Status = i.Status,
            //    OrderDate = i.OrderDate,
            //    CreatedDate = i.CreatedDate,
            //    OrderDetailLines = i.OrderLines.Select(a => new OrderDetailLines()
            //    {
            //        ProductId = a.ProductId,
            //        ProductName = a.Product.Baslik,
            //        Foto = a.Product.Foto,
            //        Foto2 = a.Product.Foto2,
            //        qty = a.qty,
            //        Price = a.Price,
            //    }).ToList()

            //});
            var orders = _context.Orders.Where(o => o.UserId == userId).OrderByDescending(a=>a.CreatedDate);
            return View(orders);
        }
    }
}
