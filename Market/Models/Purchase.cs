using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Compra")]
        [Required(ErrorMessage = "Necesita ingresar una {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DateBuy { get; set; }
        public int SupplierID { get; set; }
        public decimal Total { get; set; }

        public virtual ProductPurchase Product { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }

    }
}