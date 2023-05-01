using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class UserController : Controller
    {
        FoodDB db = new FoodDB();

        string getUserId()
        {
            string lastId = db.Users.OrderByDescending(x => x.userid).FirstOrDefault()?.userid ?? "U00000";
            int newNumber = int.Parse(lastId.Substring(1)) + 1;
            string newId = "U" + newNumber.ToString("D5");
            return newId;
        }

        /*Mã hóa mật khẩu*/
        public static string GetMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Chuyển chuỗi vào mảng byte
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Chuyển mảng byte thành chuỗi hexa
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

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
                    signup.userid = getUserId();
                    string passwordHash = GetMd5Hash(signup.Password);
                    signup.Password = passwordHash;
                    signup.ConfirmPassword = passwordHash;
                    db.Users.Add(signup);
                    db.SaveChanges();

                    TempData["Message"] = "Đăng ký thành công! Mời bạn đăng nhập!";

                    return RedirectToAction("Login", "User");
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
        public ActionResult Login(UserLoginModels model)
        {
            if (ModelState.IsValid)
            {
                string passwordMd5 = GetMd5Hash(model.Password);
                var data = db.Users.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(passwordMd5)).ToList();
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
                    TempData["ErrorMessage"] = "Login failed";
                    return RedirectToAction("Login");
                }

            }
            return View();
        }
        public ActionResult Logout()
        {

            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UserInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["UserInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                cookie = this.ControllerContext.HttpContext.Request.Cookies["cartCount"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                }
            }
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Edit(string uid)
        {
            // Tìm kiếm người dùng theo id
            var user = db.Users.FirstOrDefault(u => u.userid == uid);
            if (user == null)
            {
                return HttpNotFound();
            }
            // Hiển thị view với dữ liệu của người dùng tìm được
            var userInformation = user.ToUserInformationModel();
            return View(userInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userId,Name,Email,Address,PhoneNumber")] UserInformationModels user)
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
                /* Cập nhật lại cookie */
                var userCookie = new HttpCookie("UserInfo");
                userCookie.Values["idUser"] = existingUser.userid;
                userCookie["FullName"] = existingUser.Name;
                userCookie["Email"] = existingUser.Email;
                userCookie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(userCookie);

                TempData["Message"] = "Cập nhật thông tin thành công";
                ViewBag.Message = TempData["Message"];
                return RedirectToAction("Edit", new { uid = user.UserId });
            }

            return View(user);
        }

        //public ActionResult Edit(string uid)
        //{
        //    // Tìm kiếm người dùng theo id
        //    var user = db.Users.FirstOrDefault(u => u.userid == uid);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    // Hiển thị view với dữ liệu của người dùng tìm được
        //    return View(user);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "userid,Name,Email,Address,PhoneNumber")] Users user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChanges();
        //        /* Cập nhật lại cookie */
        //        var userCookie = new HttpCookie("UserInfo");
        //        userCookie.Values["idUser"] = user.userid;
        //        userCookie["FullName"] = user.Name;
        //        userCookie["Email"] = user.Email;
        //        userCookie.Expires = DateTime.Now.AddMonths(1);
        //        Response.Cookies.Add(userCookie);

        //        TempData["Message"] = "Cập nhật thông tin thành công";
        //        ViewBag.Message = TempData["Message"];
        //        return RedirectToAction("Edit", new { uid = user.userid });
        //    }
        //    ViewBag.Message = "Không có thay đổi!";
        //    return View(user);
        //}

        public ActionResult ChangePassword(string uid)
        {
            // Tìm kiếm người dùng theo id
            var user = db.Users.FirstOrDefault(u => u.userid == uid);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Hiển thị view với dữ liệu của người dùng tìm được
            var userCP = user.ToUserChangePasswordModel();
            return View(userCP);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "userId,Email,CurrentPassword,NewPassword,ConfirmNewPassword")] UserChangePasswordModels user)
        {
            // Tìm kiếm người dùng theo id
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.FirstOrDefault(u => u.userid == user.UserId);
                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                if (existingUser.Password != user.CurrentPassword)
                {
                    ViewBag.ErrorMessage = "Mật khẩu cũ không đúng!";
                    return View("ChangePassword", user);
                }

                // Cập nhật mật khẩu mới
                string hashPassword = GetMd5Hash(user.NewPassword);
                existingUser.Password = hashPassword;
                existingUser.ConfirmPassword = hashPassword;
                db.Entry(existingUser).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Message"] = "Cập nhật mật khẩu thành công";
                return RedirectToAction("ChangePassword", new { uid = existingUser.userid });
            }
            return View(user);
        }

    }
}