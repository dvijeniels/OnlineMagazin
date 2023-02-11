using OnlineMagazin.Models;
using System.Collections.Generic;

namespace OnlineMagazin.ViewModels
{
    public class ViewCategory
    {
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
