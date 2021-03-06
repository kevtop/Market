﻿using Market.Models;
using Market.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Market.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        MarketContext db = new MarketContext();

        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: Orders
        public ActionResult NewOrder()
        {

            var orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["orderView"] = orderView;

            var list = db.Customers.ToList();
            list.Add(new Customer { CustomerID = 0, FirstName="[Seleccione un Cliente]"});
            list = list.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");

            return View(orderView);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        [HttpPost]
        public ActionResult NewOrder(OrderView orderView)
        {

            orderView = Session["orderView"] as OrderView;

            var customerID = int.Parse(Request["CustomerID"]);

            if (customerID == 0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un Cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe seleccionar un Cliente";
                return View(orderView);
            }

            var customer = db.Customers.Find(customerID);
            if (customer == null)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un Cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "El cliente no existe";
                return View(orderView);
            }

            if(orderView.Products.Count == 0)
            {
                var list = db.Customers.ToList();
                list.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un Cliente]" });
                list = list.OrderBy(c => c.FullName).ToList();
                ViewBag.CustomerID = new SelectList(list, "CustomerID", "FullName");
                ViewBag.Error = "Debe ingresar Detalle";
                return View(orderView);
            }

            var orderID = 0;

            using(var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        CustomerID = customerID,
                        DateOrder = DateTime.Now,
                        OrderStatus = OrderStatus.Created
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    orderID = db.Orders.ToList().Select(o => o.OrderID).Max();

                    foreach (var item in orderView.Products)
                    {
                        var orderDetail = new OrderDetail
                        {
                            ProductID = item.ProductID,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity,
                            Total = item.Value,
                            OrderID = orderID
                        };
                        db.OrderDetail.Add(orderDetail);
                        db.ProductInventories.Where(a => a.ProductID == orderDetail.ProductID).First().Stock -= (int)orderDetail.Quantity;
                        order.Total += orderDetail.Total;
                        db.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    ViewBag.Error = "Error" + e.Message;
                    return View(orderView);
                }
                
            }

            
            ViewBag.Message = string.Format("La orden: {0}, grabada OK", orderID);


            var listc = db.Customers.ToList();
            listc.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un Cliente]" });
            listc = listc.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listc, "CustomerID", "FullName");

            //RedirectToAction("NewOrder");

            orderView = new OrderView();
            orderView.Customer = new Customer();
            orderView.Products = new List<ProductOrder>();
            Session["orderView"] = orderView;
            return View(orderView);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        public ActionResult AddProduct()
        {
            var list = db.ProductInventories.ToList();
            list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
            list = list.OrderBy(p => p.Description).ToList();
            ViewBag.lista = list;
            return View();
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        [HttpPost]
        public ActionResult AddProduct(ProductOrder productOrder)
        {
            var orderView = Session["orderView"] as OrderView;

            var productID = int.Parse(Request["ProductID"]);

            if (productID == 0)
            {
                var list = db.ProductInventories.ToList();
                list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
                list = list.OrderBy(p => p.Description).ToList();
                ViewBag.lista = list;
                ViewBag.Error = "Debe seleccionar un producto";
                return View(productOrder);
            }

            var product = db.Products.Find(productID);
            if (product == null)
            {
                var list = db.ProductInventories.ToList();
                list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
                list = list.OrderBy(p => p.Description).ToList();
                ViewBag.lista = list;
                ViewBag.Error = "Producto no existe";
                return View(productOrder);
            }
            productOrder = orderView.Products.Find(p => p.ProductID == productID);
            if (productOrder == null)
            {

                if (float.TryParse(Request["Quantity"], out float result))
                {



                }
                else
                {
                    var list = db.ProductInventories.ToList();
                    list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
                    list = list.OrderBy(p => p.Description).ToList();
                    ViewBag.lista = list;
                    ViewBag.Error = "Debe ingresar una cantidad";
                    return View(productOrder);

                }
                if (int.Parse(Request["Quantity"]) < db.ProductInventories.Where(a=>a.ProductID==productID).First().Stock)
                {
                    productOrder = new ProductOrder
                    {
                        Description = product.Description,
                        Price = product.Price,
                        ProductID = product.ProductID,
                        Quantity = float.Parse(Request["Quantity"])
                    };
                    orderView.Products.Add(productOrder);
                }
                else
                {
                    var list = db.ProductInventories.ToList();
                    list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
                    list = list.OrderBy(p => p.Description).ToList();
                    ViewBag.lista = list;
                    ViewBag.Error = "La cantidad es mayor que el stock disponible";
                    return View(productOrder);
                }
            }
            else
            {
                var stock = db.ProductInventories.Where(a => a.ProductID == productID).First().Stock - productOrder.Quantity;
                if (stock >= int.Parse(Request["Quantity"]))
                {
                    productOrder.Quantity += float.Parse(Request["Quantity"]);
                }
                else
                {
                    var list = db.ProductInventories.ToList();
                    list.Add(new ProductOrder { ProductID = 0, Description = "[Selecciona un Producto]" });
                    list = list.OrderBy(p => p.Description).ToList();
                    ViewBag.lista = list;
                    ViewBag.Error = string.Format("La cantidad es mayor que el stock disponible {0}", stock);
                    return View(productOrder);
                }
            }
            var listC = db.Customers.ToList();
            listC.Add(new Customer { CustomerID = 0, FirstName = "[Seleccione un Cliente]" });
            listC = listC.OrderBy(c => c.FullName).ToList();
            ViewBag.CustomerID = new SelectList(listC, "CustomerID", "FullName");

            return View("NewOrder", orderView);
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