using Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModels
{
    public class PurchaseView
    {
        public Purchase Purchase { get; set; }
        public PurchaseProduct PurchaseProduct { get; set; }
        public List<PurchaseProduct> PurchaseProducts { get; set; }
        public Supplier Supplier { get; set; }
        public ProductPurchase Product { get; set; }
        public List<ProductPurchase> Products { get; set; }
    }
}