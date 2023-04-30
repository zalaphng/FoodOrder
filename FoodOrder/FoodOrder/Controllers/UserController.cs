using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class UserController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        FoodDB db2 = new FoodDB();
        
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Signup()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                var adminInCookie = Request.Cookies["AdminInfo"];
                if (adminInCookie != null)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return View();
                }
                
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(Users signup)
        {
            if (ModelState.IsValid)
            {
                var isEmailAlreadyExists = db.Users.Any(x => x.Email == signup.Email);
                if (isEmailAlreadyExists)
                {
                    ViewBag.Message = "Email Already Registered. Please Try Again With Another Email";
                    return View();
                }
                else
                {
                    db.Users.Add(signup);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Products");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                var adminInCookie = Request.Cookies["AdminInfo"];
                if (adminInCookie != null)
                {
                    return RedirectToAction("Index", "Admin");
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
        public ActionResult Login(Users model)
        {
                var data = db.Users.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
                if (data.Count() > 0)
                {
                    Session["uid"] = data.FirstOrDefault().userid;
                    HttpCookie cooskie = new HttpCookie("UserInfo");
                    cooskie.Values["idUser"] = Convert.ToString(data.FirstOrDefault().userid);
                cooskie.Values["FullName"] = Convert.ToString(data.FirstOrDefault().Name);
                    cooskie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
                    cooskie.Expires = DateTime.Now.AddMonths(1);
                    Response.Cookies.Add(cooskie);
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                //ViewBag.Message = "Login failed";
                TempData["ErrorMessage"] = "Login failed";
                return RedirectToAction("Login");
            }
        }
        public ActionResult Logout()
        {

            if(this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["UserInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Edit(string uid)
        {
            // Tìm kiếm người dùng theo id
            var user = db2.Users.FirstOrDefault(u => u.userid == uid);
            if (user == null)
            {
                return View(user);
                //return HttpNotFound();
            }

            // Hiển thị view với dữ liệu của người dùng tìm được
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,Name,Email,Password,ConfirmPassword,Address,PhoneNumber")] Users user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                /* Cập nhật lại cookie */
                var userCookie = new HttpCookie("UserInfo");
                userCookie.Values["idUser"] = user.userid;
                userCookie["FullName"] = user.Name;
                userCookie["Email"] = user.Email;
                userCookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(userCookie);

                TempData["Message"] = "Cập nhật thông tin thành công";
                ViewBag.Message = TempData["Message"];
                return RedirectToAction("Edit", new { uid = user.userid });
            }
            return View(user);
        }



    }
}