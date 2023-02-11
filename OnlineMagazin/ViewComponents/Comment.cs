using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineMagazin.Data;
using System.Linq;

namespace OnlineMagazin.ViewComponents
{
    [AllowAnonymous]
    public class Comment:ViewComponent
    {
        private readonly OnlineMagazinContext _context;
        public Comment(OnlineMagazinContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int? id)
        {
            var CommentView = _context.Comments.Where(p=>p.ProductId==id && p.Status==true).ToList();
            //List<Products> productView = _context.Products.ToList();
            return View(CommentView);
        }
    }
}
