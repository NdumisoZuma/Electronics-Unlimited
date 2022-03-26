using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Cart_Item
    {
        [Key]
        public string Cart_Itme_Id { get; set; }
        [ForeignKey("product")]
        public int Item_ID { get; set; }
        [ForeignKey("cart")]
        public string Cartid { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        public virtual Product product { get; set; }
        //public virtual Phone phone { get; set; }
        //public virtual Accessories accessories { get; set; }
        public virtual Cart cart { get; set; }
    }
}