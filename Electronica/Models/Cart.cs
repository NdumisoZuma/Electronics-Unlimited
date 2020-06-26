using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Cart
    {
        [Key]
        public string Cart_Id { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Cart_Item> cart_Items { get; set; }
    }
}