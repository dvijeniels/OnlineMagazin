using OnlineMagazin.Models;
using System.Collections.Generic;

namespace OnlineMagazin.ViewModels
{
    public class ViewProductUye
    {
        public IEnumerable<Products> products { get; set; }
        public Uye uye { get; set; }
    }
}
