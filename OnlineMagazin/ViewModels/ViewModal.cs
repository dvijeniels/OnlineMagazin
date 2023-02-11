using OnlineMagazin.Models;
using System.Collections.Generic;

namespace OnlineMagazin.ViewModels
{
    public class ViewModal
    {
        public Products Product { get; set; }
        public IEnumerable<Products> Products { get; set; }
    }
}
