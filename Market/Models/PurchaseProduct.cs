using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class PurchaseProduct
    {
        [Key]
        public int PurchaseProductID { get; set; }
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [Display(Name = "Cantidad")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public float Quantity { get; set; }
        public decimal Total { get; set; }

        public virtual Purchase Purchase { get; set; }
        public virtual ProductPurchase Product { get; set; }
    }
}