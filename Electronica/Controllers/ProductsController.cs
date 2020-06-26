using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Electronica.Models;
using PagedList;

namespace Electronica.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc"
                : "";
            ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var products = from s in db.Products.Include(p => p.category).Include(p => p.supplier)
                           select s;




            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Prod_Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Prod_Name);
                    break;
                case "Total":
                    products = products.OrderBy(s => s.Total);
                    break;
                case "total_desc":
                    products = products.OrderByDescending(s => s.Total);
                    break;
                default:
                    products = products.OrderBy(s => s.Prod_Name);
                    break;

            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);





          


            return View(products.ToPagedList(pageSize,pageNumber));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", product.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", product.Supplier_Id);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", product.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", product.Supplier_Id);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", product.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", product.Supplier_Id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
