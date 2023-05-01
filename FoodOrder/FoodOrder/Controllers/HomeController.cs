using FoodOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodWeb.Controllers
{
    public class HomeController : Controller
    {
        FoodDB db = new FoodDB();
        // GET: HomePageTest
        public ActionResult Index()
        {
                List<Products> products = db.Products.ToList<Products>();
                return View(products);
           
          
        }
    }
}