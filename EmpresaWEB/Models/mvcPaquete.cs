using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmpresaWEB.Models
{
    public class mvcPaquete
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mvcPaquete()
        {
            this.Reservas = new HashSet<mvcReserva>();
            FechaInicio = System.DateTime.Now;
            FechaFin = System.DateTime.Now;
        }
        [Display(Name = "Paquete")]
        public int PaqueteId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Subtitulo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        [UIHint("DisplayTextArea")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.MultilineText)]
        [UIHint("DisplayTextArea")]
        public string Restricciones { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public System.DateTime FechaInicio { get; set; }
        

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Fin")]
        public System.DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Estado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mvcReserva> Reservas { get; set; }
       
    }
}