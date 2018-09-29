using Market.Models;
using Market.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class OrdersDetailsController : Controller
    {
        MarketContext db = new MarketContext();

        // GET: Orders
        public ActionResult Index()
        {

            var orderDetailView = new OrderDetailView();
            orderDetailView.Customer = new Customer();
            orderDetailView.Orders = new List<Order>();
            orderDetailView.Products = new List<ProductOrder>();
            Session["orderDetailView"] = orderDetailView;

            var list = db.Orders.ToList();
            list = list.OrderBy(c => c.OrderID).ToList();
            ViewBag.OrderID = new SelectList(list, "OrderID", "OrderID");

            return View(orderDetailView);
        }
        [HttpPost]
        public ActionResult Index(Order orderDetail)
        {
            var orderDetailView = Session["orderDetailView"] as OrderView;

            var list = db.Orders.ToList();
            list = list.OrderBy(c => c.OrderID).ToList();
            ViewBag.OrderID = new SelectList(list, "OrderID", "OrderID");

            if (int.TryParse(Request["OrderID"], out int result))
            {
                var orderID = int.Parse(Request["OrderID"]);
            }
            else
            {
                ViewBag.OrderID = new SelectList(list, "OrderID", "OrderID");
                return View(orderDetailView);
            }
            var order = db.OrderDetails.Find(orderID);
            
            return View(orderDetailView);
        }
    }
}