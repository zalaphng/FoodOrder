using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class InformationController : Controller
    {
        FoodDB db = new FoodDB();
        // GET: Information
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModels contact)
        {
            db.ContactModels.Add(contact);
            db.SaveChanges();
            return View();
        }
        public ActionResult MessageList()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                List<ContactModels> contacts = db.ContactModels.ToList<ContactModels>();
                return View(contacts);
                
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Products", "Index");

                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }
        public ActionResult AboutUs()
        {

            return View();
        }
        public ActionResult Blogs()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactModels contactModels = db.ContactModels.Find(id);
            db.ContactModels.Remove(contactModels);
            db.SaveChanges();
            return RedirectToAction("MessageList", "Information");
        }

    }
}