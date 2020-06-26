using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public abstract class Product
    {
        [Key]

        public int Prod_Id { get; set; }
        [Display(Name = "Name")]
       
        public string Prod_Name { get; set; }
        [ForeignKey("category")]
        [Display(Name = "Category")]
        public int Category_Id { get; set; }
        [Display(Name = "Supplier")]
        [ForeignKey("supplier")]
        public int Supplier_Id { get; set; }
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

        public virtual double CalcTotal()
        {
            double tot;
            tot = +(Prod_Price) + (Prod_Price * (VAT_Percent / 100));
            return tot;
        }

        //public virtual int SellProd(int sell)
        //{

        //    sell =+ Prod_Qty - 1;
        //    return sell;
        //}
        public override string ToString()
        {
            return Prod_Name;

        }
        public Product()
        {

        }

        public Product(int ProdId, int Category_Id, string Prod_Name, byte[] Prod_Pic, string Prod_Description, double Prod_Price, double VAT_Percent, int Prod_Qty, double Total, int Supplier_Id)
        {
            this.Prod_Id = ProdId;
            this.Prod_Name = Prod_Name;
            this.prod_Pic = Prod_Pic;
            this.Prod_Description = Prod_Description;
            this.Prod_Price = Prod_Price;
            this.VAT_Percent = VAT_Percent;
            this.Total = Total;
            this.Prod_Qty = Prod_Qty;
            this.Supplier_Id = Supplier_Id;
            this.Category_Id = Category_Id;

        }




        public virtual Supplier supplier { get; set; }
        public virtual Category category { get; set; }
        public ICollection<Cart_Item> cart_Items { get; set; }
    }
}