using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class ProductInventory
    {
        [Key]
        public int ProductInventoryID { get; set; }
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Debe ingresar la {0}")]
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        public int Stock { get; set; }
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        public int SupplierID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ultima Compra")]
        [Required(ErrorMessage = "Necesita ingresar una {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime LastBuy { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ProductPurchase Product { get; set; }
    }
}