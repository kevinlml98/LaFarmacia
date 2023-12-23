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
    public class ProductoController : Controller
    {
        private LaFarmaciaDBEntities2 db = new LaFarmaciaDBEntities2();

        // GET: Producto
        public ActionResult Index()
        {
            var t_Product = db.T_Product.Include(t => t.T_ProductType);
            return View(t_Product.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Product t_Product = db.T_Product.Find(id);
            if (t_Product == null)
            {
                return HttpNotFound();
            }
            return View(t_Product);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeCode = new SelectList(db.T_ProductType, "Code", "Description");
            return View();
        }

        // POST: Producto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Description,Price,Count,State,ProductTypeCode")] T_Product t_Product)
        {
            if (ModelState.IsValid)
            {
                db.T_Product.Add(t_Product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeCode = new SelectList(db.T_ProductType, "Code", "Description", t_Product.ProductTypeCode);
            return View(t_Product);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Product t_Product = db.T_Product.Find(id);
            if (t_Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeCode = new SelectList(db.T_ProductType, "Code", "Description", t_Product.ProductTypeCode);
            return View(t_Product);
        }

        // POST: Producto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Description,Price,Count,State,ProductTypeCode")] T_Product t_Product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeCode = new SelectList(db.T_ProductType, "Code", "Description", t_Product.ProductTypeCode);
            return View(t_Product);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Product t_Product = db.T_Product.Find(id);
            if (t_Product == null)
            {
                return HttpNotFound();
            }
            return View(t_Product);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            T_Product t_Product = db.T_Product.Find(id);
            db.T_Product.Remove(t_Product);
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
