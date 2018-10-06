using Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.ViewModels
{
    public class OrderDetailView
    {
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public List<Order> Orders { get; set; }
        public Customer Customer { get; set; }
        public ProductOrder Product { get; set; }
        public List<ProductOrder> Products { get; set; }
    }
}