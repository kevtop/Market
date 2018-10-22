using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class H_Ventas
    {
        [Key]
        public int venta { get; set; }
        public int OrderID { get; set; }
        public int OrderDetailID { get; set; }
        public int ProductID { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime DateOrder { get; set; }
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
    }
}