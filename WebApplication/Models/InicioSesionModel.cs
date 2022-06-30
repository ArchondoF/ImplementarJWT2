using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class InicioSesionModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [Required]
        [Display(Name = "Recordarme")]
        public bool Remember { get; set; }
    }
}