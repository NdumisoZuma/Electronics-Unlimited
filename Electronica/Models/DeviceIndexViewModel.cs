using Electronica.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Electronica.Controllers;
namespace Electronica.Models
{
    public class DeviceIndexViewModel1
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext context = new ApplicationDbContext();

        public IPagedList<Device> ListOfProducts { get; set; }
        public DeviceIndexViewModel1 CreateModel(string search, int pageSize, int? page/*,*/ /*string sortOrder, string searchString , string currentFilter*/)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",search??(object)DBNull.Value)
            };

            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc"
            //    : "";
            //ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";

            //var model = from s in context.Devices select s;

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    model = model.Where(s => s.Prod_Name.Contains(searchString));
            //}

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    model = model.Where(s => s.Prod_Name.Contains(searchString));
            //}
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        model = model.OrderByDescending(s => s.Prod_Name);
            //        break;
            //    case "Total":
            //        model = model.OrderBy(s => s.Total);
            //        break;
            //    case "total_desc":
            //        model = model.OrderByDescending(s => s.Total);
            //        break;
            //    default:
            //        model = model.OrderBy(s => s.Prod_Name);
            //        break;

            //}

            IPagedList<Device> data = context.Devices.ToList().ToPagedList(page ?? 1, pageSize);
            return new DeviceIndexViewModel1
            {
                ListOfProducts = data
            };
        }
    }
}