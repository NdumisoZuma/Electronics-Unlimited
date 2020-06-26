using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Supplier
    {
        [Key]
        public int Supplier_id { get; set; }

        [Required]
        [Display(Name = "Supplier Name")]
        [RegularExpression(pattern:@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage ="Numbers and special characters are not allowed.")]
        [StringLength(maximumLength:35, ErrorMessage ="Supplier Name must atleast be 3 characters long.", MinimumLength =3)]
        public string Name { get; set; }
        [Display(Name ="Phone Number")]
        [DataType(dataType: DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [DataType(dataType: DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        [Display(Name = "Is Active")]
        public bool status { get; set; }

        public virtual ICollection<StockOrder> StockOrders { get; set; }
        public virtual IEnumerable<Product> products { get; set; }

    }
}