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
        public ActionResult MSProduct()
        {
            ReportView reportView = new ReportView();
            reportView.OrderDetails = db.OrderDetail.ToList();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            foreach (var item in reportView.OrderDetails)
            {
                xValue.Add(item.Description);
                yValue.Add(item.Quantity);
            }
            

            new Chart(width: 600, height: 400, theme: ChartTheme.Vanilla)
            .AddTitle("Chart for Growth [Column Chart]")
            .AddSeries("Default", chartType: "column", xValue: xValue, yValues: yValue)
            .Write("jpeg");

            return View();
        }
    }
}