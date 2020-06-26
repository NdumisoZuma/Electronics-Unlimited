using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Special
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int specialId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        [Display(Name = "Qty in Stock")]
        public int QuantityInStock { get; set; }
        //[Required]
        [Display(Name = "Picture")]
        //[DataType(DataType.Upload)]
        public byte[] Picture { get; set; }


        [Required]
        [Display(Name = "Old Price")]
        [DataType(DataType.Currency)]
        public double oldPrice { get; set; }

        [Required]
        [Display(Name = "Discount Percent")]
        public double discountPercent { get; set; }

        [Display(Name = "New Price")]
        [DataType(DataType.Currency)]
        public double newPrice { get; set; }

        [Display(Name = "Discount Price")]
        [DataType(DataType.Currency)]
        public double discount { get; set; }


        [Required]
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public int Category_ID { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [ForeignKey("Size")]
        [Display(Name = "Size")]
        public int SizeId { get; set; }
       // public virtual Size Size { get; set; }

        //[Required]
        //[Display(Name = "Item")]
        //[ForeignKey("Item_ID")]
        //public virtual Item Item { get; set; }
        //public int? Item_ID { get; set; }

        public virtual ICollection<Cart_Item> Cart_Items { get; set; }

        public void SetDiscount()
        {
            discount = oldPrice * (discountPercent / 100);
        }
        public void SetPrice()
        {
            newPrice = oldPrice - discount;
        }
    }



}
