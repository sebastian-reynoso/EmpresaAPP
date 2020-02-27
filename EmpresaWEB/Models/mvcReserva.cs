using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmpresaWEB.Models
{
    public class mvcReserva
    {
        public int ReservaId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Paquete")]
        public int PaqueteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Registro")]
        public System.DateTime FechaRegistro { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Pago")]
        public DateTime? FechaPago { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Estado { get; set; }

        public virtual mvcPaquete Paquete { get; set; }
        public virtual mvcUsuario Usuario { get; set; }

        [NotMapped]
        public List<mvcUsuario> UsuarioCollection { get; set; }

        [NotMapped]
        public List<mvcPaquete> PaqueteCollection { get; set; }

        public mvcReserva()
        {
            FechaRegistro = System.DateTime.Now;
        }
    }
}