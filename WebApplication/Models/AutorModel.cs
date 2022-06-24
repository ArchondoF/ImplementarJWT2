using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication.Validations;

namespace WebApplication.Models
{
    public class AutorModel
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        [NombreAutorEnUsoValidationAtribute]
        public string Nombre { get; set; }
    }
}