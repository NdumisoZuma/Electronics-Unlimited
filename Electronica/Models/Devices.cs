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
        public decimal monthly_p { get; set; }
        [Display(Name = "Contract Length(Months)")]
        public int months { get; set; }
        [Display(Name = "Interest rate")]
        public decimal interestRate { get; set; }
        public string Purchase { get; set; }
        //[ForeignKey("bundlePackage")]
        //public int BundlePackageId { get; set; }
        //[Display(Name ="Payment Option")]
        //public string contract { get; set; }
        //public double Payment_Op { get; set; }
        public Device() : base()
        {

        }

        //public Device(int ProdId, int Category_Id, string Prod_Name, string Purchase, byte[] Prod_Pic, string Prod_Description, decimal Prod_Price, decimal VAT_Percent, int Prod_Qty, decimal Total, int Supplier_Id, decimal Monthly_P, int months, decimal Interest)
        //    : base(ProdId, Category_Id, Prod_Name, Prod_Pic, Prod_Description, Prod_Price, VAT_Percent, Prod_Qty, Total, Supplier_Id)
        //{

        //    this.monthly_p = Monthly_P;
        //    this.months = months;
        //    this.interestRate = Interest;
        //    this.Purchase = Purchase;
        //    //this.BundlePackageId = BundlePackageId;
        //}




        //public override decimal CalcTotal()
        //{
        //    return (base.CalcTotal() + (base.CalcTotal() * (interestRate / 100)));
        //}

        public decimal purchase()
        {
            decimal result;

            if (Purchase == "contract")
            {

                result = CalcTotal();


            }
            else
            {

                result = monthly();


            }

            return result;
        }

        public decimal monthly()
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
        //    }'['pp
        //    else
        //    {
        //        return CalcTotal(tot);
        //    }
        //}




        //public virtual BundlePackage bundlePackage { get; set; }

    }
}