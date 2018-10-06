using CrystalDecisions.CrystalReports.Engine;
using Market.Models;
using Market.ViewModels;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class OrderDetailController : Controller
    {
        private MarketContext db = new MarketContext();
        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetailView = new OrderDetailView();
            orderDetailView.Orders = db.Orders.ToList();
            
            Session["orderDetailView"] = orderDetailView;
            
            return View(orderDetailView);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            OrderDetailView orderDetailView = new OrderDetailView();
            orderDetailView.Order = db.Orders.Find(id);
            
            if (orderDetailView == null)
            {
                return HttpNotFound();
            }
            return View(orderDetailView);
        }
        public ActionResult Print(int? id)
        {

            var q = new ActionAsPdf($"Detalles/{id}")
            {
                PageSize = Rotativa.Options.Size.A6,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(0,0,0,0),
                PageWidth = 120,
                PageHeight = 80
            };
            return q;
        }
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetailView orderDetailView = new OrderDetailView();
            orderDetailView.Order = db.Orders.Find(id);

            if (orderDetailView == null)
            {
                return HttpNotFound();
            }
            return View(orderDetailView);
            
            
        }
    }
}