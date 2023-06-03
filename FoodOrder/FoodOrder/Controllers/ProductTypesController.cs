using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodOrder.Models;

namespace FoodOrder.Controllers
{
    public class ProductTypesController : Controller
    {
        private FoodDB db = new FoodDB();

        // GET: ProductTypes
        [HttpGet]
        public ActionResult Index(string productTypeName)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var productTypes = db.ProductTypes.ToList();

                if (!string.IsNullOrEmpty(productTypeName))
                {
                    productTypes = productTypes.Where(p => p.ProductTypeName.Contains(productTypeName)).ToList();
                }

                return View(productTypes);
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


            // GET: ProductTypes/Details/5
            public ActionResult Details(int? id)
        {

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductTypes productTypes = db.ProductTypes.Find(id);
                if (productTypes == null)
                {
                    return HttpNotFound();
                }
                return View(productTypes);
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

        // GET: ProductTypes/Create
        public ActionResult Create()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
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

        // POST: ProductTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ProductTypeName")] ProductTypes productTypes)
        {

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (ModelState.IsValid)
                {
                    db.ProductTypes.Add(productTypes);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(productTypes);
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

        // GET: ProductTypes/Edit/5
        public ActionResult Edit(int? id)
        {

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductTypes productTypes = db.ProductTypes.Find(id);
                if (productTypes == null)
                {
                    return HttpNotFound();
                }
                return View(productTypes);
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

        // POST: ProductTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ProductTypeName")] ProductTypes productTypes)
        {

            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(productTypes).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(productTypes);
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
        public ActionResult Delete(int? id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductTypes productType = db.ProductTypes.Find(id);
                if (productType == null)
                {
                    return HttpNotFound();
                }

                var productsToDelete = db.Products.Where(p => p.FKProductType == productType.id);
                foreach (var product in productsToDelete)
                {
                    db.Products.Remove(product);
                }

                db.ProductTypes.Remove(productType);
                db.SaveChanges();

                return RedirectToAction("Index", "ProductTypes");
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



        // POST: ProductTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                ProductTypes productTypes = db.ProductTypes.Find(id);
                db.ProductTypes.Remove(productTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
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
