using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DemoInternalServices.Models.Usuario
{
    public class UsuarioModel
    {
        
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}