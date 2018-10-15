using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Market.Models;
using Market.ViewModels;

namespace Market.Controllers
{
    public class PurchasesController : Controller
    {
        private MarketContext db = new MarketContext();

        // GET: Purchases
        public ActionResult Index()
        {
            var purchaseDetailView = new PurchaseDetailView();
            purchaseDetailView.Purchases = db.Purchases.ToList();

            Session["purchaseDetailView"] = purchaseDetailView;
            
            return View(purchaseDetailView);
        }

        public ActionResult NewPurchase()
        {

            var purchaseView = new PurchaseView();
            purchaseView.Supplier = new Supplier();
            purchaseView.Products = new List<ProductPurchase>();
            Session["purchaseView"] = purchaseView;

            var list = db.Suppliers.ToList();
            list.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
            list = list.OrderBy(c => c.Name).ToList();
            ViewBag.SupplierID = new SelectList(list, "SupplierID", "Name");

            return View(purchaseView);
        }
        [HttpPost]
        public ActionResult NewPurchase(PurchaseView purchaseView)
        {

            purchaseView = Session["purchaseView"] as PurchaseView;

            var SupplierID = int.Parse(Request["SupplierID"]);

            if (SupplierID == 0)
            {
                var list = db.Suppliers.ToList();
                list.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
                list = list.OrderBy(c => c.Name).ToList();
                ViewBag.SupplierID = new SelectList(list, "SupplierID", "Name");
                ViewBag.Error = "Debe seleccionar un Cliente";
                return View(purchaseView);
            }

            var supplier = db.Suppliers.Find(SupplierID);
            if (supplier == null)
            {
                var list = db.Suppliers.ToList();
                list.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
                list = list.OrderBy(c => c.Name).ToList();
                ViewBag.SupplierID = new SelectList(list, "SupplierID", "Name");
                ViewBag.Error = "El cliente no existe";
                return View(purchaseView);
            }

            if (purchaseView.Products.Count == 0)
            {
                var list = db.Suppliers.ToList();
                list.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
                list = list.OrderBy(c => c.Name).ToList();
                ViewBag.SupplierID = new SelectList(list, "SupplierID", "Name");
                ViewBag.Error = "Debe ingresar Detalle";
                return View(purchaseView);
            }

            var purchaseID = 0;

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var purchase = new Purchase
                    {
                        SupplierID = SupplierID,
                        DateBuy = DateTime.Now
                    };
                    db.Purchases.Add(purchase);
                    db.SaveChanges();

                    purchaseID = db.Purchases.ToList().Select(s => s.PurchaseID).Max();

                    foreach (var item in purchaseView.Products)
                    {
                        var purchaseProduct = new PurchaseProduct
                        {
                            ProductID = item.ProductID,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Total = item.Value,
                            PurchaseID = purchaseID
                        };
                        db.PurchaseProducts.Add(purchaseProduct);
                        db.ProductInventories.Find(purchaseProduct.ProductID).Stock += (int)purchaseProduct.Quantity;
                        db.ProductInventories.Find(purchaseProduct.ProductID).Price = (purchaseProduct.Price * (decimal)db.Products.Find(purchaseProduct.ProductID).Margin) + purchaseProduct.Price;
                        db.Products.Find(purchaseProduct.ProductID).Price = db.ProductInventories.Find(purchaseProduct.ProductID).Price;
                        purchase.Total += purchaseProduct.Total;
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error" + e.Message;
                    return View(purchaseView);
                }

            }


            ViewBag.Message = string.Format("La orden: {0}, grabada OK", purchaseID);


            var listp = db.Suppliers.ToList();
            listp.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
            listp = listp.OrderBy(c => c.Name).ToList();
            ViewBag.SupplierID = new SelectList(listp, "SupplierID", "Name");

            //RedirectToAction("NewOrder");

            purchaseView = new PurchaseView();
            purchaseView.Supplier = new Supplier();
            purchaseView.Products = new List<ProductPurchase>();
            Session["purchaseView"] = purchaseView;
            return View(purchaseView);
        }

        //// GET: Purchases/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Purchase purchase = db.Purchases.Find(id);
        //    if (purchase == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(purchase);
        //}

        //// GET: Purchases/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description");
        //    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name");
        //    return View();
        //}

        //// POST: Purchases/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PurchaseID,ProductID,Price,Quantity,DateBuy,SupplierID")] Purchase purchase)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        db.Purchases.Add(purchase);
        //        var product = db.ProductInventories.Find(purchase.ProductID);
        //        if (product == null)
        //        {
        //            ProductInventory productInventory = new ProductInventory
        //            {
        //                Stock = (int)purchase.Quantity,
        //                Description = db.Products.Find(purchase.ProductID).Description,
        //                ProductID = purchase.ProductID,
        //                Price = purchase.Price * (purchase.Price * (decimal)db.Products.Find(purchase.ProductID).Margin) + purchase.Price,
        //                SupplierID = purchase.SupplierID,
        //                LastBuy = purchase.DateBuy
        //            };
        //            db.ProductInventories.Add(productInventory);
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            db.ProductInventories.Find(purchase.ProductID).Stock += (int)purchase.Quantity;
        //            db.ProductInventories.Find(purchase.ProductID).Price = (purchase.Price * (decimal)db.Products.Find(purchase.ProductID).Margin) + purchase.Price;
        //            db.Products.Find(purchase.ProductID).Price = db.ProductInventories.Find(purchase.ProductID).Price;
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", purchase.ProductID);
        //    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
        //    return View(purchase);
        //}

        //// GET: Purchases/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Purchase purchase = db.Purchases.Find(id);
        //    if (purchase == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", purchase.ProductID);
        //    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
        //    return View(purchase);
        //}

        //// POST: Purchases/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PurchaseID,ProductID,Price,Quantity,DateBuy,SupplierID")] Purchase purchase)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(purchase).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Description", purchase.ProductID);
        //    ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "Name", purchase.SupplierID);
        //    return View(purchase);
        //}

        //// GET: Purchases/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Purchase purchase = db.Purchases.Find(id);
        //    if (purchase == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(purchase);
        //}

        //// POST: Purchases/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Purchase purchase = db.Purchases.Find(id);
        //    db.Purchases.Remove(purchase);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        //public ActionResult AddProduct()
        //{
        //    var list = db.ProductInventories.ToList();
        //    list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
        //    list = list.OrderBy(p => p.Description).ToList();
        //    ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
        //    return View();
        //}

        public ActionResult AddProduct()
        {
            var list = db.ProductInventories.ToList();
            list.Add(new ProductPurchase { ProductID = 0, Description = "[Selecciona un Producto]" });
            list = list.OrderBy(p => p.Description).ToList();
            ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
            return View();
        }


        [HttpPost]
        public ActionResult AddProduct(ProductPurchase productPurchase)
        {
            var purchaseView = Session["purchaseView"] as PurchaseView;

            var productID = int.Parse(Request["ProductID"]);
            
            if (productID == 0)
            {
                var list = db.ProductInventories.ToList();
                list.Add(new ProductPurchase { ProductID = 0, Description = "[Selecciona un Producto]" });
                list = list.OrderBy(p => p.Description).ToList();
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                ViewBag.Error = "Debe seleccionar un producto";
                return View(productPurchase);
            }

            var product = db.Products.Find(productID);
            if (product == null)
            {
                var list = db.ProductInventories.ToList();
                list.Add(new ProductPurchase { ProductID = 0, Description = "[Selecciona un Producto]" });
                list = list.OrderBy(p => p.Description).ToList();
                ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                ViewBag.Error = "Producto no existe";
                return View(productPurchase);
            }
            productPurchase = purchaseView.Products.Find(p => p.ProductID == productID);
            if (productPurchase == null)
            {

                if (float.TryParse(Request["Quantity"], out float result))
                {
                    productPurchase = new ProductPurchase
                    {
                        Description = product.Description,
                        Price = decimal.Parse(Request["Price"]),
                        ProductID = product.ProductID,
                        Quantity = result
                    };
                    purchaseView.Products.Add(productPurchase);


                }
                else
                {
                    var list = db.ProductInventories.ToList();
                    list.Add(new ProductPurchase { ProductID = 0, Description = "[Selecciona un Producto]" });
                    list = list.OrderBy(p => p.Description).ToList();
                    ViewBag.ProductID = new SelectList(list, "ProductID", "Description");
                    ViewBag.Error = "Debe ingresar una cantidad";
                    return View(productPurchase);

                }
            }
            else
            {
                productPurchase.Quantity += float.Parse(Request["Quantity"]);
            }
            var lists = db.Suppliers.ToList();
            lists.Add(new Supplier { SupplierID = 0, Name = "[Seleccione un Proveedor]" });
            lists = lists.OrderBy(c => c.Name).ToList();
            ViewBag.SupplierID = new SelectList(lists, "SupplierID", "Name");

            return View("NewPurchase", purchaseView);
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
