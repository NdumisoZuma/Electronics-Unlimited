using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class OrderVM
    {
        public int SupplierId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}