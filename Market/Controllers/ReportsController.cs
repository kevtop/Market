using Market.Models;
using Market.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Market.Controllers
{
    public class ReportsController : Controller
    {
        private MarketContext db = new MarketContext();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        //public ActionResult MSProduct()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult MSProduct(ReportView reportView)
        {
            
            reportView.OrderDetails = new List<OrderDetail>();
            var list = db.OrderDetail.Where(l => l.Order.DateOrder > reportView.StartTime  && l.Order.DateOrder < reportView.StopTime).ToList();
            
            foreach (var item in list)
            {
                if (reportView.OrderDetails.Count == 0)
                {
                    reportView.OrderDetails.Add(item);
                }
                if (reportView.OrderDetails.Find(a => a.ProductID == item.ProductID) == null)
                {
                    reportView.OrderDetails.Add(item);
                }
                else
                {
                    reportView.OrderDetails.Find(a => a.ProductID == item.ProductID).Quantity += item.Quantity;
                }
            }
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            foreach (var item in reportView.OrderDetails)
            {
                xValue.Add(item.Description);
                yValue.Add(item.Quantity);
            }


            new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
            .AddTitle($"Producto mas vendido de {reportView.StartTime.ToShortDateString()} a {reportView.StopTime.ToShortDateString()}")
            .AddSeries("Default", chartType: "column", xValue: xValue, yValues: yValue)
            .Write("jpeg");

            return View();
        }
    }
}