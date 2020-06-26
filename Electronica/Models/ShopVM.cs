using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class ShopVM
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        
        public int category { get; set; }
        
        [Display(Name = "Thumbnail")]
        public byte[] prod_Pic { get; set; }
        [Display(Name = "Description")]
        public string Prod_Description { get; set; }
        [Display(Name = "Price")]
        public double Prod_Price { get; set; }
        [Display(Name = "VAT")]
        public double VAT_Percent { get; set; }
        [Display(Name = "Quantity")]
        public int Prod_Qty { get; set; }

        public double Total { get; set; }
        [Display(Name = "Monthly installment")]
        public double monthly_p { get; set; }
        [Display(Name = "Contract Length(Months)")]
        public int months { get; set; }
        [Display(Name = "Interest rate")]
        public double interestRate { get; set; }
        public string Purchase { get; set; }

    }
}