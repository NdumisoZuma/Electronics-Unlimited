using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Electronica.Models;

namespace Electronica.Controllers
{
    public class AccessoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accessories
        public ActionResult Index()
        {
            var products = db.Accessories.Include(a => a.category).Include(a => a.supplier);
            return View(products.ToList());
        }

        // GET: Accessories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accessory accessory = db.Accessories.Find(id);
            if (accessory == null)
            {
                return HttpNotFound();
            }
            return View(accessory);
        }

        // GET: Accessories/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name");
            return View();
        }

        // POST: Accessories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total")] Accessory accessory, HttpPostedFileBase img_upload)
        {

            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            accessory.prod_Pic = data;
            accessory.Total = accessory.CalcTotal();
            

            if (ModelState.IsValid)
            {
                db.Accessories.Add(accessory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", accessory.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", accessory.Supplier_Id);
            return View(accessory);
        }

        // GET: Accessories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accessory accessory = db.Accessories.Find(id);
            if (accessory == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", accessory.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", accessory.Supplier_Id);
            return View(accessory);
        }

        // POST: Accessories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total")] Accessory accessory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accessory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", accessory.Category_Id);
            ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", accessory.Supplier_Id);
            return View(accessory);
        }

        // GET: Accessories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accessory accessory = db.Accessories.Find(id);
            if (accessory == null)
            {
                return HttpNotFound();
            }
            return View(accessory);
        }

        // POST: Accessories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accessory accessory = db.Accessories.Find(id);
            db.Products.Remove(accessory);
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
