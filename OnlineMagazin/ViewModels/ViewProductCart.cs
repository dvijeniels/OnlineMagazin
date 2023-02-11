using OnlineMagazin.Models;
using System.Collections.Generic;

namespace OnlineMagazin.ViewModels
{
    public class ViewProductCart
    {
        public Products products { get; set; }

        public List<ProductFeatures> productFeatures { get; set; }
        public ProductFeatures productFeature { get; set; }
        public Carts carts { get; set; }
        public List<Sliders> sliders { get; set; }
    }
}
