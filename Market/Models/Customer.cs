using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        
        public int DocumentTypeID { get; set; }

        [NotMapped]
        public string FullName { get => string.Format("{0} {1}", FirstName, LastName); }
        
        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}