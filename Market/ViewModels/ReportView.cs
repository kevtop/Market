using Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModels
{
    public class ReportView
    {
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Customer Customer { get; set; }
        public ProductOrder Product { get; set; }
        public List<ProductOrder> Products { get; set; }
        public Purchase Purchase { get; set; }
        public PurchaseProduct PurchaseProduct { get; set; }
        public List<Purchase> Purchases { get; set; }
        public Supplier Supplier { get; set; }
    }
}