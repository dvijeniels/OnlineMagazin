using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using System;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class RatingStar:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public RatingStar(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int id)
        {
            ViewBag.RatingAverage = _context.Comments.Where(x=>x.ProductId==id && x.Status==true).Average(x => x.Score);
            return View(ViewBag.RatingAverage);
        }
    }
}
