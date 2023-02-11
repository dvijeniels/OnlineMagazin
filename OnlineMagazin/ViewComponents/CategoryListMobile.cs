using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class CategoryListMobile : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public CategoryListMobile(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Category> categoryListAll = _context.Category.ToList();
            return View(categoryListAll);
        }
    }
}
