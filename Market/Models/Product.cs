using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Debe ingresar la {0}")]
        [Display(Name ="Descripcion")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }
        [Display(Name = "Porcentaje")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public float Margin { get; set; }
        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }
        //lado uno de la relacion muchos a muchos
        
        public ICollection<OrderDetail> OrderDetails { get; set; }

    }
}