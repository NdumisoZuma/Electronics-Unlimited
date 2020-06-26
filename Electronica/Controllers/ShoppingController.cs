using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Electronica.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid;
using SendGrid.Helpers.Mail;
using PagedList;

namespace Electronica.Controllers
{


    // GET: Shopping
    public class ShopController : Controller
    {
        public ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Shopping
        private Product ph = new Device();
        private Product acc = new Accessory();

        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        private bool Pymnt;

        public ActionResult Index()
        {
            return View();

        }
        
        public ActionResult CatPhones()
        {
            return View(_db.Devices.Where(x => x.category.CategoryName == "Phones"));
        }

        public ActionResult CatLaptop()
        {
            return View(_db.Devices.Where(x => x.category.CategoryName == "Laptops"));
        }
       


        public ActionResult Categories()
        {
            return View(_db.Categories.ToList());
        }


        public ActionResult DeviceIndex( string search ,string sortOrder, string currentFilter, string searchString, int? page)
        {
            DeviceIndexViewModel1 model = new DeviceIndexViewModel1();

            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc"
            //    : "";
            //ViewBag.TotalSortParm = sortOrder == "Total" ? "total_desc" : "Total";

            //var model = from s in _db.Devices select s;

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

           



            return View(model.CreateModel(search,10,page));
        }

        public ActionResult AccessoryIndex(string search,  int? page)
        {
            AccessoryIndexViewModel model = new AccessoryIndexViewModel();
         

           

            return View(model.CreateModel(search ,10, page));

        }
        // For Phones
        public ActionResult Add_to_cart(int id)
        {
            var item = _db.Devices.Find(id);
            if (item != null)
            {
                add_item_to_cart(id);
                return RedirectToAction("DeviceIndex");

            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }
        public ActionResult Add_to_cart2(int id)
        {
            var item = _db.Accessories.Find(id);
            if (item != null)
            {
                add_item_to_cart2(id);
                return RedirectToAction("AccessoryIndex");

            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }
        }

        public ActionResult remove_from_cart(string id)
        {
            var item = _db.cart_Items.Find(id);
            if (item != null)
            {
                remove_item_from_cart(id: id);
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                return RedirectToAction("Not_Found", "Error");
            }

        }

        public ActionResult ShoppingCart()
        {
            shoppingCartID = GetCartID();
            ViewBag.Total = get_cart_total(id: shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.Cartid == shoppingCartID).Sum(q => q.quantity);

            return View(_db.cart_Items.ToList().FindAll(x => x.Cartid == shoppingCartID));
        }

        [HttpPost]
        public ActionResult ShoppingCart(List<Cart_Item> items)
        {
            shoppingCartID = GetCartID();
            foreach (var i in items)
            {
                updateCart(i.Cart_Itme_Id, i.quantity);
            }
            ViewBag.Total = get_cart_total(shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.Cartid == shoppingCartID).Sum(q => q.quantity);

            return View(_db.cart_Items.ToList().FindAll(x => x.Cartid == shoppingCartID));

        }

        public ActionResult countCartItems()
        {
            int qty = get_Cart_Items().Count();
            return Content(qty.ToString());
        }

        public void add_item_to_cart(int id)
        {
            shoppingCartID = GetCartID();

            var item = _db.Devices.Find(id);
            if (item != null)
            {
                var cartItem =
                    _db.cart_Items.FirstOrDefault(x => x.Cartid == shoppingCartID && x.Item_ID == item.Prod_Id);


                if (cartItem == null)
                {
                    var cart = _db.carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        _db.carts.Add(entity: new Cart()
                        {
                            Cart_Id = shoppingCartID,
                            DateCreated = DateTime.Now
                        });
                        _db.SaveChanges();
                    }
                    _db.cart_Items.Add(entity: new Cart_Item()
                    {
                        Cart_Itme_Id = Guid.NewGuid().ToString(),
                        Cartid = shoppingCartID,
                        Item_ID = item.Prod_Id,
                        quantity = 1,
                        price = item.monthly_p
                    });
                }
                else
                {
                    cartItem.quantity++;
                }
                _db.SaveChanges();
            }



        }


        public void add_item_to_cart2(int id)
        {
            shoppingCartID = GetCartID();

            var item = _db.Accessories.Find(id);
            if (item != null)
            {
                var cartItem =
                    _db.cart_Items.FirstOrDefault(x => x.Cartid == shoppingCartID && x.Item_ID == item.Prod_Id);


                if (cartItem == null)
                {
                    var cart = _db.carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        _db.carts.Add(entity: new Cart()
                        {
                            Cart_Id = shoppingCartID,
                            DateCreated = DateTime.Now
                        });
                        _db.SaveChanges();
                    }
                    _db.cart_Items.Add(entity: new Cart_Item()
                    {
                        Cart_Itme_Id = Guid.NewGuid().ToString(),
                        Cartid = shoppingCartID,
                        Item_ID = item.Prod_Id,
                        quantity = 1,
                        price = item.Total
                    });
                }
                else
                {
                    cartItem.quantity++;
                }
                _db.SaveChanges();
            }



        }





        public void remove_item_from_cart(string id)
        {
            shoppingCartID = GetCartID();

            var item = _db.cart_Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    _db.cart_Items.FirstOrDefault(predicate: x => x.Cartid == shoppingCartID && x.Item_ID == item.Item_ID);
                if (cartItem != null)
                {
                    _db.cart_Items.Remove(entity: cartItem);
                }
                _db.SaveChanges();
            }
        }

        public List<Cart_Item> get_Cart_Items()
        {
            shoppingCartID = GetCartID();
            return _db.cart_Items.ToList().FindAll(match: x => x.Cartid == shoppingCartID);
        }

        public void updateCart(string id, int qty)
        {
            var item = _db.cart_Items.Find(id);
            if (qty < 0)
            {
                item.quantity = qty / -1;
            }
            else if (qty == 0)
            {
                remove_item_from_cart(item.Cart_Itme_Id);
            }
            else
            {
                item.quantity = qty;
            }
            _db.SaveChanges();
        }
        public double get_cart_total(string id)
        {
            double amount = 0;
            foreach (var item in _db.cart_Items.ToList().FindAll(match: x => x.Cartid == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;

        }
        public void empty_Cart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in _db.cart_Items.ToList())
            {
                _db.cart_Items.Remove(item);
            }
            try
            {
                _db.carts.Remove(_db.carts.Find(shoppingCartID));
                _db.SaveChanges();
            }
            catch (Exception)
            {


            }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
        public ActionResult Checkout()
        {
            if (get_Cart_Items().Count == 0)
            {
                ViewBag.Err = "Opps... You should have atleast one item, please add items ";
                return RedirectToAction("DeviceIndex");
            }
            else
            {
                return RedirectToAction("DeliveryOption");
            }
        }
        [Authorize]
        public ActionResult DeliveryOption()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeliveryOption(string colorRadio, string street, string City, string PostalCode)
        {
            if (!String.IsNullOrEmpty(colorRadio))
            {
                if (colorRadio.Equals("StandardDelivery"))
                {
                    Session["street"] = street;
                    Session["City"] = City;
                    Session["PostalCode"] = PostalCode;
                    return RedirectToAction("PlaceOrder", new { id = "delivery" });
                }
            }
            return View();
        }

        //makes all certificates valid even when they arent...well this is not good but i had to do it!
        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public ActionResult PlaceOrder(string id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            ApplicationUser customer = UserManager.FindById(User.Identity.GetUserId());

            _db.Orders.Add(new Order
            {


                Email = customer.Email,
                date_created = DateTime.Now,
                shipped = false,
                status = "Payment Complete",
                packed = false


            });
            _db.SaveChanges();

            //
            //Send email after order has been placed
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            string Message = "<p>You have successfuly placed an order</p>,<p>Send us Feedback:</p>,<link><a href=https://abiytshirt3.azurewebsites.net/Feedback/Create > Click here! </a></link>";
            string FromEmail = "ABIY Ltd";
            string FromName = "Info@abiy.com";

            var message = new MailMessage();
            message.To.Add(new MailAddress(customer.Email));
            message.From = new MailAddress("ndumisozuma99@gmail.com");
            message.Subject = "Order";
            message.Body = string.Format(body, FromName, FromEmail, Message);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "ndumisozuma99@gmail.com",
                    Password = "Sphamandla@12"  //valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                //smtp.Send(message);

            }



            var order = _db.Orders.ToList()
                .FindAll(x => x.Email == customer.Email)
                .LastOrDefault();

            if (id == "deliver")
            {
                _db.Order_Addresses.Add(new Order_Address()
                {
                    Order_ID = order.Order_ID,
                    // distance = (int)Session["Distance"],
                    street = Session["Street"].ToString(),
                    city = Session["City"].ToString(),
                    zipcode = Session["PostalCode"].ToString()
                });
                _db.SaveChanges();
            }

            var items = get_Cart_Items();

            foreach (var item in items)
            {
                var x = new Order_Item()
                {
                    Order_id = order.Order_ID,
                    item_id = item.Item_ID,
                    quantity = item.quantity,
                    price = item.price

                };
                _db.Order_Items.Add(x);
                _db.SaveChanges();
            }
            empty_Cart();
            //order tracking
            _db.GetOrder_Trackings.Add(new Order_Tracking()
            {
                order_ID = order.Order_ID,
                date = DateTime.Now,
                status = "Awaiting Payment",
                Recipient = ""
            });
            _db.SaveChanges();

            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.Order_ID });
        }
        public ActionResult Payment(int? id)
        {
            var order = _db.Orders.Find(id);
            ViewBag.Order = order;
            //here
            ViewBag.Account = _db.Users.Find(order.Email);
            ViewBag.Address = _db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = _db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID);


            try
            {
                string url = "<a href=" + "https://www.payfast.co.za/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string itemsinoder = "<tr> " +
                                         "<td>" + item.Item.Prod_Name + " </td>" +
                                         "<td>" + item.quantity + " </td>" +
                                         "<td>R " + item.price + " </td>" +
                                         "<tr/>";
                    table += itemsinoder;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(((Customer)ViewBag.Account).Email, ((Customer)ViewBag.Account).FirstName + " " + ((Customer)ViewBag.Account).LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your order was placed, you can securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);

                Pymnt = true;
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Secure_Payment(int? id)
        {
            var order = _db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = _db.Customers.Find(order.Email);
            ViewBag.Address = _db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = _db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

            //foreach (var item in db.Order_Items.Where(x => x.Order_id == order.Order_ID))
            //{
            //    Item product = db.Items.Find(item.item_id);
            //    product.QuantityInStock -= item.quantity;
            //}
            //db.SaveChanges();

            ViewBag.Total = get_order_total(order.Order_ID);
            Pymnt = true;

            return Redirect(PaymentLink(get_order_total(order.Order_ID).ToString(), "Order Payment | Order No: " + order.Order_ID, order.Order_ID));
        }
        public ActionResult Payment_Cancelled(int? id)
        {
            var order = _db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = _db.Customers.Find(order.Email);
            ViewBag.Address = _db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = _db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

            ViewBag.Total = get_order_total(order.Order_ID);
            try
            {
                string url = "<a href=" + "https://www.payfast.co.za/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Prod_Name + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your order payment process was cancelled, you can still securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Payment_Successfull(int? id)
        {
            var order = _db.Orders.Find(id);
            try
            {
                order.status = "At warehouse";

                //order tracking
                _db.GetOrder_Trackings.Add(new Order_Tracking()
                {
                    order_ID = order.Order_ID,
                    date = DateTime.Now,
                    status = "Payment Recieved | Order still at warehouse",
                    Recipient = ""
                });
                _db.SaveChanges();
                _db.Payments.Add(new Payment()
                {
                    Date = DateTime.Now,
                    Email = _db.Customers.FirstOrDefault(p => p.Email == User.Identity.Name).Email,
                    AmountPaid = get_order_total(order.Order_ID),
                    PaymentFor = "Order " + id + " Payment",
                    PaymentMethod = "PayFast Online"
                });
                _db.SaveChanges();
                if (_db.Order_Addresses.Where(p => p.Order_ID == id) != null)
                {
                    var expected_Date = DateTime.Now.AddDays(2);
                    do
                    {
                        expected_Date = expected_Date.AddDays(1);
                    } while (expected_Date.DayOfWeek.ToString().ToLower() == "sunday" ||
                        expected_Date.DayOfWeek.ToString().ToLower() == "saturday");

                    //Delivery
                }
                _db.SaveChanges();
                ViewBag.Items = _db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

                update_Stock((int)id);

                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Prod_Name + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R 0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Payment Recieved";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", We recieved your payment, you will have your goodies any time from now " + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex) { }

            ViewBag.Order = order;
            ViewBag.Account = _db.Customers.Find(order.Email);
            ViewBag.Address = _db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID); //+ DeliveryCost();
            Pymnt = true;
            return View();
        }
        public double get_order_total(int id)
        {
            double amount = 0;
            foreach (var item in _db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }

        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10002201";
                merchantKey = "25lbpwmazv8rn";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Payment method unknown.");
            }
            var stringBuilder = new StringBuilder();
            //string url = Url.Action("Quotes", "Order",
            //    new System.Web.Routing.RouteValueDictionary(new { id = orderid }),
            //    "http", Request.Url.Host);

            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode("https://abiytshirt3.azurewebsites.net/Shopping/Payment_Successfull?id=" + order_id));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode("https://abiytshirt3.azurewebsites.net/Shopping/Payment_Cancelled?id=" + order_id));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_NotifyURL"]));

            string amt = totalCost;
            amt = amt.Replace(",", ".");

            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }

        public void update_Stock(int id)
        {
            var order = _db.Orders.Find(id);
            List<Order_Item> items = _db.Order_Items.ToList().FindAll(x => x.Order_id == id);
            foreach (var item in items)
            {
                var product = _db.Products.Find(item.item_id);
                if (product != null)
                {
                    if ((product.Prod_Qty -= item.quantity) >= 0)
                    {
                        product.Prod_Qty -= item.quantity;
                    }
                    else
                    {
                        item.quantity = product.Prod_Qty;
                        product.Prod_Qty = 0;
                    }
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }



        }
    }
}


