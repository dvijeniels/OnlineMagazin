using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class SearchAllProduct : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public SearchAllProduct(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var category = _context.Category.ToList();
            var viewcat = new ViewModels.ViewCategory
            {
                Categories = category
            };
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoriName");
            return View(viewcat);
        }

    }
}
