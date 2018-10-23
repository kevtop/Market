using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Market.Models;

namespace Market.Controllers
{
    public class ProductInventoriesController : Controller
    {
        private MarketContext db = new MarketContext();
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: ProductInventories
        public ActionResult Index()
        {
            var productInventories = db.ProductInventories.Include(p => p.Product).Include(p => p.Supplier);
            return View(productInventories.ToList());
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: ProductInventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInventory productInventory = db.ProductInventories.Find(id);
            if (productInventory == null)
            {
                return HttpNotFound();
            }
            return View(productInventory);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: ProductInventories/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
            return View();
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // POST: ProductInventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductInventoryID,ProductID,Description,Stock,Price,SupplierID,LastBuy")] ProductInventory productInventory)
        {
            if (ModelState.IsValid)
            {
                if (db.Products.Find(productInventory.ProductID) == null)
                {
                    db.Products.Find(productInventory.ProductID).Price = productInventory.Price;
                    db.ProductInventories.Add(productInventory);
                    db.SaveChanges();
                }
                db.ProductInventories.Add(productInventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", productInventory.ProductID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", productInventory.SupplierID);
            return View(productInventory);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: ProductInventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInventory productInventory = db.ProductInventories.Find(id);
            if (productInventory == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", productInventory.ProductID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", productInventory.SupplierID);
            return View(productInventory);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // POST: ProductInventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductInventoryID,ProductID,Description,Stock,Price,SupplierID,LastBuy")] ProductInventory productInventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productInventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", productInventory.ProductID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", productInventory.SupplierID);
            return View(productInventory);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: ProductInventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInventory productInventory = db.ProductInventories.Find(id);
            if (productInventory == null)
            {
                return HttpNotFound();
            }
            return View(productInventory);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // POST: ProductInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductInventory productInventory = db.ProductInventories.Find(id);
            db.ProductInventories.Remove(productInventory);
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
