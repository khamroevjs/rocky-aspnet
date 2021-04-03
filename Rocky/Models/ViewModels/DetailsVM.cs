using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Models.ViewModels
{
    public class DetailsVM
    {
        public Product Product { get; set; } = new Product();
        public bool ExistsInCart { get; set; }
    }
}
