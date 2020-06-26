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
    public class DevicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Devices
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

            var products = from s in db.Devices.Include(d => d.category).Include(d => d.supplier)
              select s ;

          

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


            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total,monthly_p,months,interestRate,Purchase")] Device device, HttpPostedFileBase img_upload)
        {

            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            device.prod_Pic = data;
            device.Total = device.CalcTotal();
            device.monthly_p = device.monthly();
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", device.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", device.Supplier_Id);
            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", device.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", device.Supplier_Id);
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total,monthly_p,months,interestRate,Purchase")] Device device, HttpPostedFileBase img_upload)
        {

            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            device.prod_Pic = data;
            device.Total = device.CalcTotal();
            device.monthly_p = device.monthly();

            if (ModelState.IsValid)
            {
                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", device.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", device.Supplier_Id);
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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
