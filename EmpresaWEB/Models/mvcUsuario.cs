using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmpresaWEB.Models
{
    public class mvcUsuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mvcUsuario()
        {
            this.Reservas = new HashSet<mvcReserva>();
        }

        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Required (ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Contraseña { get; set; }

        public string Rol { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mvcReserva> Reservas { get; set; }
    }
}