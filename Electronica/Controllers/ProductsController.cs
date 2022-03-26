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
using Electronica.View_Models;

namespace Electronica.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        //GET: Products
        [AllowAnonymous]

        public ActionResult Index2(string category, string search, string sortBy, int? page)
        {
            // instantiate bew viewmodel

            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            //Select the products including category
            var products = db.Products.Include(p => p.category);

            //filter by category
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.category.CategoryName == category);
            }

            //search Product
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Prod_Name.Contains(search)
                || p.Prod_Description.Contains(search)
                || p.Prod_Price.ToString().Equals(search)
                || p.category.CategoryName.Contains(search));

                viewModel.Search = search;
            }

            //group search results into categories and count how many items in each category

            viewModel.CatsWithCount = from mp in products
                                      where
                                      mp.Category_Id != null
                                      group mp by mp.category.CategoryName into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };
            //filter by category

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.category.CategoryName == category);
                viewModel.Category = category;
            }

            // sort the results
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Prod_Price);
                        break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Prod_Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Prod_Name);
                    break;

            }

            int currentPage = page ?? 1;
            viewModel.Products = products.ToPagedList(currentPage, constant.PageItems);
            viewModel.sortBy = sortBy;

            // hold data to populate sort select element
            viewModel.sorts = new Dictionary<string, string>
            {
                {"Price low to high","price_lowest" },
                {"Price high to low","price_highest" }
            };

            return View(viewModel);




        }














        //// GET: Products
        //public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        //{

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc"
        //        : "";
        //    ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";

        //    if (searchString != null)
        //    {
        //        page = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }

        //    ViewBag.CurrentFilter = searchString;

        //    var products = from s in db.Products.Include(p => p.category).Include(p => p.supplier)
        //                   select s;




        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        products = products.Where(s => s.Prod_Name.Contains(searchString));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            products = products.OrderByDescending(s => s.Prod_Name);
        //            break;
        //        case "Total":
        //            products = products.OrderBy(s => s.Total);
        //            break;
        //        case "total_desc":
        //            products = products.OrderByDescending(s => s.Total);
        //            break;
        //        default:
        //            products = products.OrderBy(s => s.Prod_Name);
        //            break;

        //    }
        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);





          


        //    return View(products.ToPagedList(pageSize,pageNumber));
        //}

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
            // ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName");
            // ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name");

            //Add Images to a new Product
            ProductVM viewModel = new ProductVM();
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryId", "CategoryName");
            viewModel.SupplierList = new SelectList(db.Suppliers, "Supplier_id", "Name");
            viewModel.ImageList = new List<SelectList>();
            for (int i = 0; i < constant.NumberOfProductImages; i++)
            {
                viewModel.ImageList.Add(new SelectList(db.productImages, "ID", "FileName"));

            }

            
            return View(viewModel);
        }




        //Post: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (ProductVM viewModel)
        {
            //Manually mapping products to products VM

            Product product = new Product();
            product.Total = product.CalcTotal();
            product.Prod_Name = viewModel.Name;
            product.Prod_Qty = viewModel.Quantity;
            product.Prod_Description = viewModel.Description;
            product.Prod_Price = viewModel.price;
            product.Category_Id = viewModel.CategoryID;
            product.Supplier_Id = viewModel.SupplierID;
            product.productImageMappings = new List<ProductImageMapping>();

            //get a list of selected images without any blanks

            string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
            //
            for (int i = 0; i < productImages.Length; i++)
            {
                product.productImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = db.productImages.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index2");

            }
            viewModel.CategoryList = new SelectList(db.Categories, "CategoryID", "CategoryName", product.Category_Id);
            viewModel.SupplierList = new SelectList(db.Suppliers, "Supplier_id", "Name", product.Supplier_Id);
            viewModel.ImageList = new List<SelectList>();
            for (int i = 0; i < constant.NumberOfProductImages; i++)
            {
                viewModel.ImageList.Add(new SelectList(db.productImages, "ID", "FileName", viewModel.ProductImages[i]));

            }
            return View(viewModel);


        }








        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Prod_Id,Prod_Name,Category_Id,Supplier_Id,prod_Pic,Prod_Description,Prod_Price,VAT_Percent,Prod_Qty,Total")] Product product, HttpPostedFileBase upload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (upload != null & upload.ContentLength>0)
        //        {
        //            int fileLength = upload.ContentLength;
        //            Byte[] array = new Byte[fileLength];
        //            upload.InputStream.Read(array, 0, fileLength);
        //            product.prod_Pic = array;

        //            db.Products.Add(product);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");

        //        }


                
        //    }

        //    ViewBag.Category_Id = new SelectList(db.Categories, "CategoryId", "CategoryName", product.Category_Id);
        //    ViewBag.Supplier_Id = new SelectList(db.Suppliers, "Supplier_id", "Name", product.Supplier_Id);
        //    return View(product);
        //}

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

            //Prevent foreign key violation
            var orderLines = db.OrderLines.Where(ol => ol.ProductID == id);
            foreach (var ol in orderLines)
            {
                ol.ProductID = null;
            }
            //
            db.SaveChanges();
            return RedirectToAction("Index2");
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
