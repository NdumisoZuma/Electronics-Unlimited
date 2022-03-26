using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using PagedList;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Electronica.Models;
using Electronica.View_Models;
using System.Threading.Tasks;

namespace Electronica.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index( )
        {
            

            return View();
        }


        public async Task<ActionResult> Index2()
        {
            var topSellers = (from topProducts in db.OrderLines
                              where (topProducts.ProductID != null)
                              group topProducts by topProducts.Product
                               into topGroup
                              select new BestSellerVM
                              {
                                  product = topGroup.Key,
                                  salesCount = topGroup.Sum(tp => tp.Quality),
                                  productImage = topGroup.Key.productImageMappings.
                                  OrderBy(pim => pim.ImageNumber).FirstOrDefault().ProductImage.FileName

                              }).OrderByDescending(tg => tg.salesCount).Take(6);
            return View(await topSellers.ToListAsync());


        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}