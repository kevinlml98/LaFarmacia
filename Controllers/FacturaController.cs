using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaFarmacia.Models;

namespace LaFarmacia.Controllers
{
    public class FacturaController : Controller
    {
        private LaFarmaciaDBEntities db = new LaFarmaciaDBEntities();

        // GET: Factura
        public ActionResult Index()
        {
            var t_InvoiceHeader = db.T_InvoiceHeader.Include(t => t.T_Client).Include(t => t.T_MethodPay);
            return View(t_InvoiceHeader.ToList());
        }

        // GET: Factura/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_InvoiceHeader t_InvoiceHeader = db.T_InvoiceHeader.Find(id);
            if (t_InvoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(t_InvoiceHeader);
        }

        // GET: Factura/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.T_Client, "Id", "Name");
            ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description");
            return View();
        }

        // POST: Factura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Date,ClientId,PayMethodTypeId,SubTotal,Tax,Discount,Total")] T_InvoiceHeader t_InvoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.T_InvoiceHeader.Add(t_InvoiceHeader);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.T_Client, "Id", "Name", t_InvoiceHeader.ClientId);
            ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
            return View(t_InvoiceHeader);
        }

        // GET: Factura/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_InvoiceHeader t_InvoiceHeader = db.T_InvoiceHeader.Find(id);
            if (t_InvoiceHeader == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.T_Client, "Id", "Name", t_InvoiceHeader.ClientId);
            ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
            return View(t_InvoiceHeader);
        }

        // POST: Factura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Date,ClientId,PayMethodTypeId,SubTotal,Tax,Discount,Total")] T_InvoiceHeader t_InvoiceHeader)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_InvoiceHeader).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.T_Client, "Id", "Name", t_InvoiceHeader.ClientId);
            ViewBag.PayMethodTypeId = new SelectList(db.T_MethodPay, "Id", "Description", t_InvoiceHeader.PayMethodTypeId);
            return View(t_InvoiceHeader);
        }

        // GET: Factura/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_InvoiceHeader t_InvoiceHeader = db.T_InvoiceHeader.Find(id);
            if (t_InvoiceHeader == null)
            {
                return HttpNotFound();
            }
            return View(t_InvoiceHeader);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            T_InvoiceHeader t_InvoiceHeader = db.T_InvoiceHeader.Find(id);
            db.T_InvoiceHeader.Remove(t_InvoiceHeader);
            db.SaveChanges();
            return RedirectToAction("Index");
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
