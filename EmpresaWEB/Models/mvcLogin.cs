using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmpresaWEB.Models
{
    public class mvcLogin
    {
        [DisplayName("Correo")]
        public string Correo { get; set; }

        [DisplayName("Contraseña")]
        public string Contraseña { get; set; }
    }
}