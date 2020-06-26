using Electronica.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Electronica.Models
{
    public class AccessoryIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext context = new ApplicationDbContext();
        public IPagedList<Accessory> ListOfProducts { get; set; }
        public AccessoryIndexViewModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",search??(object)DBNull.Value)
            };
            IPagedList<Accessory> data = context.Accessories.ToList().ToPagedList(page ?? 1, pageSize);
            return new AccessoryIndexViewModel
            {
                ListOfProducts = data
            };
        }
    }
}