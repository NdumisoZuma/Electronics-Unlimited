using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class StockOrder
    {
        public StockOrder()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
        [Key]
        public int OrderID { get; set; }

        public System.DateTime OrderDate { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public virtual Supplier suppliers { get; set; }
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}