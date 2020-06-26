using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class Accessory : Product
    {
        public Accessory()
        {

        }

        public Accessory(int ProdId, int Category_Id, string Prod_Name, byte[] Prod_Pic, string Prod_Description, double Prod_Price, double VAT_Percent, int Prod_Qty, double Total, int Supplier_Id)
            : base(ProdId, Category_Id, Prod_Name, Prod_Pic, Prod_Description, Prod_Price, VAT_Percent, Prod_Qty, Total, Supplier_Id)
        {

        }
        public override string ToString()
        {
            return base.ToString() + Prod_Description;

        }
    }

}