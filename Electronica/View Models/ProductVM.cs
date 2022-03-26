using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Electronica.View_Models
{
    public class ProductVM
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "The product name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a product between 3 and 50 characters in length")]
       // [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$)", ErrorMessage = "Please enter a name that contains letters and numbers only")]
        public string Name { get; set; }
        [Required(ErrorMessage ="The product description cannot be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage ="The product description must be a between 10 and 200 characters")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage ="The product description must only contain numbers and letters")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "The product price must not be left blank")]
        [DataType(DataType.Currency)]
       // [RegularExpression("[0-9]+ (\\.[0-9][0-9]?)?", ErrorMessage = "The price must be a number and be two decimal places")]

        public int Quantity { get; set; }

        public decimal price { get; set; }
        [Display(Name ="Category")]
        public int CategoryID { get; set; }
        [Display(Name ="Supplier")]
        public int SupplierID { get; set; }
        public SelectList SupplierList { get; set; }

        public SelectList CategoryList { get; set; }
        public List<SelectList> ImageList { get; set; }
        public string[] ProductImages { get; set; }





    }
}