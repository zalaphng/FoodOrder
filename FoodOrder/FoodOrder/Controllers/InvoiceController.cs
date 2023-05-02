using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FoodOrder.Models
{
    public class InvoiceController : Controller
    {
        private FoodDB db = new FoodDB();

        // GET: Invoice/Edit/5
        public ActionResult Edit(string id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                InvoiceModels invoiceModels = db.InvoiceModels.Find(id);
                if (invoiceModels == null)
                {
                    return HttpNotFound();
                }
                ViewBag.FKUserID = new SelectList(db.Users, "userid", "Name", invoiceModels.FKUserID);
                return View(invoiceModels);
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

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "InvoiceID,DateInvoice,Total_Bill,Status,FKUserID")] InvoiceModels invoiceModels)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(invoiceModels).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("ListOfInvoices", "Admin");
        //    }
        //    ViewBag.FKUserID = new SelectList(db.Users, "userid", "Name", invoiceModels.FKUserID);
        //    return View(invoiceModels);
        //}

        public ActionResult Edit([Bind(Include = "InvoiceID,Status")] InvoiceModels invoiceModels)
        {
            if (ModelState.IsValid)
            {
                var invoice = db.InvoiceModels.Find(invoiceModels.InvoiceID);
                invoice.Status = invoiceModels.Status;
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListOfInvoices", "Admin");
            }
            ViewBag.FKUserID = new SelectList(db.Users, "userid", "Name", invoiceModels.FKUserID);
            return View(invoiceModels);
        }

        // GET: Invoice/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                InvoiceModels invoiceModels = db.InvoiceModels.Find(id);
                var orders = db.Orders.Where(o => o.FkInvoiceID == id);

                foreach (var order in orders)
                {
                    db.Orders.Remove(order);
                }

                db.InvoiceModels.Remove(invoiceModels);
                db.SaveChanges();
                return RedirectToAction("ListOfInvoices", "Admin");
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

        public ActionResult OrderDetails(string FKInvoiceID)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var orders = db.Orders.Include(o => o.Products)
                                   .Where(o => o.FkInvoiceID == FKInvoiceID)
                                   .ToList();
                var invoice = db.InvoiceModels.FirstOrDefault(i => i.InvoiceID == FKInvoiceID);
                TempData["InvoiceTotal"] = invoice.Total_Bill;

                return View(orders);
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

        public ActionResult InvoicesList(string FKUserID)
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
                    if (userInCookie.Values["idUser"] == FKUserID)
                    {
                        var invoices = db.InvoiceModels.Where(i => i.FKUserID == FKUserID).ToList();
                        if (invoices == null || invoices.Count == 0)
                        {
                            TempData["Message"] = "Chưa có đơn đặt hàng";
                        }
                        ViewBag.FKUserID = FKUserID;
                        return View(invoices);
                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        public ActionResult Cancel(string InvoiceID)
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
                    string uid = userInCookie != null ? userInCookie.Values["idUser"] : null;

                    if (uid != null)
                    {
                        InvoiceModels invoice = db.InvoiceModels.FirstOrDefault(i => i.FKUserID == uid && i.InvoiceID == InvoiceID && i.Status != 4);

                        if (invoice != null)
                        {
                            invoice.Status = 3;
                            db.Entry(invoice).State = EntityState.Modified;
                            db.SaveChanges();

                            TempData["SuccessMsg"] = "The invoice has been cancelled.";
                        }
                        else
                        {
                            TempData["ErrorMsg"] = "There is no invoice to cancel.";
                        }
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "User not logged in.";
                    }

                    return RedirectToAction("InvoicesList", "Invoice", new { FKUserId = uid });
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

    }
}
