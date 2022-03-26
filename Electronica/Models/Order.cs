using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Order
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Order_ID { get; set; }
        [Display(Name ="User")]
        public string UserID { get; set; }

        [Display(Name ="Deliver To:")]
        public string DeliveryName { get; set; }

        [Display(Name = "Time of Order")]
        [DataType(DataType.DateTime)]
        public DateTime date_created { get; set; }


        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalPrice { get; set; }
        public string Email { get; set; }
        public bool shipped { get; set; }
        public string status { get; set; }
        public bool packed { get; set; }

        public Order_Address Order_Address { get; set; }


        public List<OrderLine> OrderLines { get; set; }
        public virtual ICollection<Order_Item> Order_Items { get; set; }
        public virtual ICollection<Order_Address> Order_Addresses { get; set; }
        // public virtual Customer Customer { get; set; }

        //  public virtual ICollection<Order_Tracking> Order_Tracking { get; set; }



        public virtual ApplicationUser Customer { get; set; }
        // public string UserId { get; set; }


    }
}