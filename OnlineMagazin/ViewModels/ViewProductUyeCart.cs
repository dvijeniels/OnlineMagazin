using OnlineMagazin.Models;
using System.Collections.Generic;

namespace OnlineMagazin.ViewModels
{
    public class ViewProductUyeCart
    {
        public Products products { get; set; }
        public Uye uye { get; set; }
        public Carts carts { get; set; }
        public List<Sliders> sliders { get; set; }
    }
}
