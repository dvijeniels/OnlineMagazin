using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    public class CategorySearch:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public CategorySearch(OnlineMagazinContext context)
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
