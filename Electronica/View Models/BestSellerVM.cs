using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Electronica.Models;

namespace Electronica.View_Models
{
    public class BestSellerVM
    {
        public Product product { get; set; }
        public int salesCount { get; set; }

        public string productImage { get; set; }
    }
}