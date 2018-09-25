using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        //lado uno de la relacion muchos a muchos
        public ICollection<SupplierProduct> SupplierProducts { get; set; }
    }
}