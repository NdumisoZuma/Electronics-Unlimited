using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name = "Category Name")]
        [RegularExpression(pattern:@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage ="Numbers and special characters are not allowed.")]
        [StringLength(maximumLength:35 , ErrorMessage ="Name must be longer than 3 characters", MinimumLength =3)]
        public string CategoryName { get; set; }
        public IEnumerable<Product> products { get; set; }
    }
}