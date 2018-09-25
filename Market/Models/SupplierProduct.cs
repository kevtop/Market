using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class SupplierProduct
    {
        //tabla intermedia relacion de muchos a muchos
        [Key]
        public int SupplierProductID { get; set; }
        public int ProductID { get; set; }
        public int SupplierID { get; set; }
        //lado varios de la relacion se colocan los objetos a los cuales se hara referencia
        public virtual Supplier Supplier { get; set; }
        public virtual Product Product { get; set; }
    }
}