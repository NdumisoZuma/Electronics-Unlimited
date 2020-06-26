using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Order_Tracking
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int tracking_ID { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Order")]
        public int order_ID { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public string Recipient { get; set; }

        public virtual Order Order { get; set; }

    }
}