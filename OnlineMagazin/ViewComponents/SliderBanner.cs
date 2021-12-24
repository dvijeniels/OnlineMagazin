using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using OnlineMagazin.Models;
using System.Collections.Generic;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    public class SliderBanner : ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public SliderBanner(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Sliders> sliderlist = _context.Sliders.ToList();
            return View(sliderlist);
        }
    }
}
