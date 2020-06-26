using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Device : Product
    {
        //[Key]
        //public int PhoneId { get; set; }
        [Display(Name = "Monthly installment")]
        public double monthly_p { get; set; }
        [Display(Name = "Contract Length(Months)")]
        public int months { get; set; }
        [Display(Name = "Interest rate")]
        public double interestRate { get; set; }
        public string Purchase { get; set; }
        //[ForeignKey("bundlePackage")]
        //public int BundlePackageId { get; set; }
        //[Display(Name ="Payment Option")]
        //public string contract { get; set; }
        //public double Payment_Op { get; set; }
        public Device() : base()
        {

        }

        public Device(int ProdId, int Category_Id, string Prod_Name, string Purchase, byte[] Prod_Pic, string Prod_Description, double Prod_Price, double VAT_Percent, int Prod_Qty, double Total, int Supplier_Id, double Monthly_P, int months, double Interest)
            : base(ProdId, Category_Id, Prod_Name, Prod_Pic, Prod_Description, Prod_Price, VAT_Percent, Prod_Qty, Total, Supplier_Id)
        {

            this.monthly_p = Monthly_P;
            this.months = months;
            this.interestRate = Interest;
            this.Purchase = Purchase;
            //this.BundlePackageId = BundlePackageId;
        }




        public override double CalcTotal()
        {
            return (base.CalcTotal() + (base.CalcTotal() * (interestRate / 100)));
        }

        public double purchase()
        {
            double result;

            if (Purchase == "Prepaid")
            {

                result = CalcTotal();


            }
            else
            {

                result = monthly();


            }

            return result;
        }

        public double monthly()
        {
            return (CalcTotal()) / months;
        }
        public override string ToString()
        {
            return base.ToString() + Prod_Description;

        }

        //public double PaymentOp(double tot)
        //{
        //    double p;
        //    if (contract == "Prepaid")
        //    {
        //        return p = base.CalcTotal(tot);
        //    }
        //    else
        //    {
        //        return CalcTotal(tot);
        //    }
        //}




        //public virtual BundlePackage bundlePackage { get; set; }

    }
}