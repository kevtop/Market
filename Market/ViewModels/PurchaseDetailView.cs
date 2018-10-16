using Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModels
{
    public class PurchaseDetailView
    {
        public Purchase Purchase { get; set; }
        public PurchaseProduct PurchaseProduct { get; set; }
        public List<Purchase> Purchases { get; set; }
        public Supplier Supplier { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
    }
}