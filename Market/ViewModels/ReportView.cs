using Market.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.ViewModels
{
    public class ReportView
    {
        public Order Order { get; set; }
        public OrderDetail OrderDetail { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public Customer Customer { get; set; }
        public ProductOrder Product { get; set; }
        public List<ProductOrder> Products { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [Display(Name = "Fecha de termino")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime StopTime { get; set; }
    }
}
