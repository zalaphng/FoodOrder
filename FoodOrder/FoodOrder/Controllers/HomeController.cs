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
            var productTypes = db.ProductTypes.ToList();
            productTypes.Insert(0, new ProductTypes(0, "All"));
            ViewBag.ProductTypes = productTypes;
            return View(products);
        }

    }
}