using Electronica.Business_Logic;
using Electronica.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Electronica;


namespace Electronica.Controllers
{
    public class OrdersController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        OrderBusiness ob = new OrderBusiness();

        //GET Orders 2


        //GET: Orders

        public async Task<ActionResult> shopOrders(string orderSearch, string startDate, string endDate, string sortOrder, int? page)
        {      //call orders into task and order them by the creation date and include Orderlines table
            var orders = db.Orders.OrderBy(o => o.date_created).Include(o => o.OrderLines);

            // Display Orders as non admin user

            if (!User.IsInRole("Admin"))
            {
                return View(db.Orders.Where(o => o.UserID == User.Identity.Name));
            }

            //Search required order fields

            //if orderSearch string is not null or empty 
            if (!String.IsNullOrEmpty(orderSearch))
            {
                //orders is equal to orders where all properties are equal to or contain the string in orderSearch parameter
                orders = orders.Where(o => o.Order_ID.ToString().Equals(orderSearch) ||
                                        o.DeliveryName.Contains(orderSearch) ||
                                        o.TotalPrice.ToString().Equals(orderSearch) ||
                                        o.date_created.ToString().Contains(orderSearch)||
                                        o.UserID.ToString().Contains(orderSearch)  ||
                                        o.Email.Contains(orderSearch)||
                                        o.status.Contains(orderSearch)||
                                        o.Order_Address.street.Contains(orderSearch)||
                                        o.Order_Address.city.Contains(orderSearch)||
                                        o.Order_Address.zipcode.Contains(orderSearch)||
                                        o.OrderLines.Any(ol => ol.ProductName.Contains(orderSearch)));

            }

            //View Orders on selected date ranges 

            DateTime parsedStartDate;
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                orders = orders.Where(o => o.date_created >= parsedStartDate);
            }

            DateTime parsedEndDate;

            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                orders = orders.Where(o => o.date_created <= parsedEndDate);
            }


            //Sort Orders using ViewBags and dswith statements

            ViewBag.sortDate = String.IsNullOrEmpty(sortOrder) ? "date" : "";
            ViewBag.UserSort = sortOrder == "user" ? "use_desc" : "user";
            ViewBag.PriceSort = sortOrder == "price" ? "price_desc" : "user";
            //Remember search term n date search
            ViewBag.CurrentOrderSearch = orderSearch;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            switch (sortOrder)
            {
                case "user":
                    orders = orders.OrderBy(o => o.UserID);
                    break;

                case "user_desc":
                    orders = orders.OrderByDescending(o => o.UserID);
                    break;
                case "price":
                    orders = orders.OrderBy(o => o.TotalPrice);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(o => o.TotalPrice);
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.date_created);
                    break;

                default:
                    orders = orders.OrderByDescending(o => o.date_created);
                    break;
            }



            //async paging
            int currentPage = (page ?? 1);
            //Add pageLinks using ViewBag
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)orders.Count() / constant.PageItems);
            //
            var currentPageOfOrders = await orders.ReturnPages(currentPage, constant.PageItems);
            //store current sort order as ViewBag
            ViewBag.CurrentSortOrder = sortOrder;
            return View(currentPageOfOrders);



        }















        // GET: Orders 1
        public ActionResult Customer_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.status = "All";
                return View(ob.cust_all());
            }
            else
            {
                ViewBag.status = id;
                return View(ob.cust_find_by_status(id));
            }
        }

        public ActionResult New_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.status = "All";
                return View(ob.cust_all());
            }
            else
            {
                ViewBag.Status = id;
                return View(ob.cust_find_by_status(id));
            }
        }
        public ActionResult Order_Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bad_Request", "Error");
            }
            if (ob.cust_find_by_id(id) != null)
            {
                ViewBag.Order_Items = ob.cust_Order_items(id);
                ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == id);
                ViewBag.Total = get_order_total((int)id);

                return View(ob.cust_find_by_id(id));
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }
              
        public ActionResult Order_Tracking(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Bad_Request", "Error");
            }
            if (ob.cust_find_by_id(id)!= null)
            {
                ViewBag.Order = ob.cust_find_by_id(id);
                return View(ob.get_trackng_report(id));
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }

        public ActionResult Mark_As_Packed(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ob.cust_find_by_id(id) != null)
            {
                ob.mark_as_packed(id);
                return RedirectToAction("Order_Details", new { id = id });
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public decimal get_order_total(int id)
        {
            decimal amount = 0;
            foreach (var item in db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }

        public ActionResult Order_History()
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser customer = UserManager.FindById(User.Identity.GetUserId());



            return View(ob.cust_all().Where(x => x.Email == User.Identity.Name));
        }

        public ActionResult MyOrders(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

            // string UserId = user.Id;

            List<Order> orders = db.Orders.Where(x => x.Email == user.Email).ToList();

            foreach (var item in orders)
            {
                List<Order_Item> orderItems = db.Order_Items.Where(x => x.Order_id == item.Order_ID).ToList();
                decimal total = 0;
                item.Order_Items = orderItems;
                foreach (var ting in orderItems)
                {
                    ting.Item = db.Products.Find(ting.item_id);
                    ting.Item.category = db.Categories.Find(ting.Item.Category_Id);

                    total += ting.price;
                    ViewData["i" + ting.Order_Item_id] = ting.Item.Prod_Name + " " + ting.Item.Prod_Description + " (" + ting.Item.category.CategoryName + ")";
                }
                ViewData["p" + item.Order_ID] = total;
            }
            var pageNumber = page ?? 1;

            var onePageOfOrders = orders.ToPagedList(pageNumber, 10);
            ViewBag.OnePageOfOrders = onePageOfOrders;

            return View(orders);

        }



    }
    }