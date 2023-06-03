using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class AdminController : Controller
    {
        FoodDB db = new FoodDB();
        // GET: Admin
        public ActionResult Index()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                int orderCount = db.Orders.Count();
                ViewBag.OrderCount = orderCount;
                int userCount = db.Users.Count();
                ViewBag.UserCount = userCount;
                return View();
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }

        }
        [HttpGet]
        public ActionResult LoginAdmin()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin"); ;
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    ViewBag.Message = TempData["ErrorMessage"];
                    return View();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(UserLoginModels model)
        {
            var hashPassword = UserController.GetMd5Hash(model.Password);
            var data = db.AdminLogins.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(hashPassword)).ToList();
            if (data.Count() > 0)
            {
                HttpCookie cooskie = new HttpCookie("AdminInfo");
                cooskie.Values["idAdmin"] = Convert.ToString(data.FirstOrDefault().adminid);
                cooskie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
                cooskie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cooskie);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                TempData["ErrorMessage"] = "Login failed";
                return RedirectToAction("LoginAdmin");
            }
        }
        public ActionResult LogoutAdmin()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("AdminInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["AdminInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("LoginAdmin");
        }

        public ActionResult ListOfOrders(string orderID)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                float t = 0;
                List<Orders> orderDetails;

                if (!string.IsNullOrEmpty(orderID))
                {
                    orderDetails = db.Orders.Where(od => od.FkInvoiceID == orderID).ToList();
                }
                else
                {
                    orderDetails = db.Orders.ToList();
                }

                foreach (var item in orderDetails)
                {
                    t += item.Order_Bill;
                }

                TempData["OrderTotal"] = t;
                return View(orderDetails);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }


        public ActionResult ListOfInvoices(string userName)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                float t = 0;
                List<InvoiceModels> invoice;

                if (!string.IsNullOrEmpty(userName))
                {
                    var user = db.Users.FirstOrDefault(u => u.Name == userName);
                    if (user != null)
                    {
                        invoice = db.InvoiceModels.Where(i => i.FKUserID == user.userid).ToList();
                    }
                    else
                    {
                        invoice = new List<InvoiceModels>();
                    }
                }
                else
                {
                    invoice = db.InvoiceModels.ToList();
                }

                foreach (var item in invoice)
                {
                    t += item.Total_Bill;
                }

                TempData["InvoiceTotal"] = t;
                return View(invoice);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }


        public ActionResult ListOfUsers(string name)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var users = db.Users.ToList();

                if (!string.IsNullOrEmpty(name))
                {
                    users = users.Where(u => u.Name.Contains(name)).ToList();
                }

                return View(users);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        public ActionResult DeleteUser(string id)
        {

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var invoices = db.InvoiceModels.Where(i => i.FKUserID == id).ToList();

                if (invoices.Count != 0)
                {
                    foreach (var invoice in invoices)
                    {
                        var orders = db.Orders.Where(o => o.FkInvoiceID == invoice.InvoiceID);

                        foreach (var order in orders)
                        {
                            db.Orders.Remove(order);
                        }
                        db.InvoiceModels.Remove(invoice);
                    }
                }

                var user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                db.Users.Remove(user);
                db.SaveChanges();

                return RedirectToAction("ListOfUsers", "Admin");
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        public ActionResult EditUser(string uid)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var user = db.Users.FirstOrDefault(u => u.userid == uid);
                if (user == null)
                {
                    return HttpNotFound();
                }
                // Hiển thị view với dữ liệu của người dùng tìm được
                var userInformation = user.ToUserInformationModel();
                return View(userInformation);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "userId,Name,Email,Address,PhoneNumber")] UserInformationModels user)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.Users.Find(user.UserId);
                    if (existingUser == null)
                    {
                        return HttpNotFound();
                    }
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Address = user.Address;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    db.Entry(existingUser).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "Cập nhật thông tin thành công";
                    ViewBag.Message = TempData["Message"];
                    return RedirectToAction("ListOfUsers", new { uid = user.UserId });
                }
                return View(user);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        public ActionResult EditUserPassword(string uid)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var user = db.Users.FirstOrDefault(u => u.userid == uid);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Hiển thị view với dữ liệu của người dùng tìm được
                var userCP = user.ToUserChangePasswordModel();
                return View(userCP);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserPassword([Bind(Include = "userId,Email,CurrentPassword,NewPassword,ConfirmNewPassword")] UserChangePasswordAdminModels user)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.Users.FirstOrDefault(u => u.userid == user.UserId);
                    if (existingUser == null)
                    {
                        return HttpNotFound();
                    }

                    // Cập nhật mật khẩu mới
                    string hashPassword = UserController.GetMd5Hash(user.NewPassword);
                    existingUser.Password = hashPassword;
                    existingUser.ConfirmPassword = hashPassword;
                    db.Entry(existingUser).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["Message"] = "Cập nhật mật khẩu thành công";
                    return RedirectToAction("ListOfUsers", new { uid = existingUser.userid });
                }
                return View(user);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }
        public ActionResult UserInvoices(string UserID)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var invoices = db.InvoiceModels.Where(i => i.FKUserID == UserID).ToList();
                if (invoices == null || invoices.Count == 0)
                {
                    TempData["Message"] = "Chưa có đơn đặt hàng";
                }
                return View(invoices);
            }
            else
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }
    }
}