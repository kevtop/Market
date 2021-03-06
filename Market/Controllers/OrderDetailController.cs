﻿using CrystalDecisions.CrystalReports.Engine;
using Market.Models;
using Market.ViewModels;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        // GET: OrderDetails
        public ActionResult Index()
        {
            var orderDetailView = new OrderDetailView();
            orderDetailView.Orders = db.Orders.ToList();
            
            Session["orderDetailView"] = orderDetailView;
            
            return View(orderDetailView);
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
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
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        public ActionResult Print(int? id)
        {

            var q = new ActionAsPdf($"Detalles/{id}")
            {
                PageSize = Rotativa.Options.Size.A6,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = new Rotativa.Options.Margins(0,0,0,0),
                PageWidth = 60
            };
            return q;
        }
        [Authorize(Roles = "Empleado,Gerente,Administrador")]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetailView orderDetailView = new OrderDetailView();
            orderDetailView.Order = db.Orders.Find(id);
            db.Orders.Find(id).OrderStatus = OrderStatus.Delivered;
            db.SaveChanges();
            if (orderDetailView == null)
            {
                return HttpNotFound();
            }
            return View(orderDetailView);
            
            
        }
    }
}