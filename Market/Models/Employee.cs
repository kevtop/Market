using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Market.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [Display(Name = "Primer Nombre")]
        [StringLength(30, ErrorMessage = "El campo {0} debe estar entre {2} y {1}", MinimumLength = 3)]
        public string firstName { get; set; }
        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [StringLength(30, ErrorMessage = "El campo {0} debe estar entre {2} y {1}", MinimumLength = 3)]
        public string lastName { get; set; }
        [Display(Name = "Salario")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Salary { get; set; }
        [Display(Name = "Porcentaje")]
        [DisplayFormat(DataFormatString = @"{0:#\%}", ApplyFormatInEditMode = false)]
        public float bonusPercent { get; set; }
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}",ApplyFormatInEditMode =true)]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Hora de Entrada")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        [DisplayFormat(DataFormatString ="{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Url)]
        public string URL { get; set; }
        [Display(Name = "Documento")]
        public int DocumentTypeID { get; set; }
        
        public virtual DocumentType DocumentType { get; set; }
        [Display(Name = "Numero de Documento")]
        [Required(ErrorMessage = "Necesita ingresar un {0}")]
        public string DocumentNumber { get; set; }
    }
}