using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    public class CategoryList:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public CategoryList(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Category> categoryList = _context.Category.ToList();
            return View(categoryList);
        }
    }
}
