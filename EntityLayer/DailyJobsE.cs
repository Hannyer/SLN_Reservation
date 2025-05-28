
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EntityLayer
{
    public class DailyJobsE : BaseE
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Tipo es obligatorio.")]
        [Display(Name = "Tipo")]
        public string Type { get; set; }
        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio.")]
        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "El campo Estado es obligatorio.")]
        [Display(Name = "Estado")]
        public string Status { get; set; }
        [Required(ErrorMessage = "El campo Colaborador es obligatorio.")]
        [Display(Name = "Colaborador")]
        public string Collaborator { get; set; }
    
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Frecuencia es obligatorio.")]
        [Display(Name = "Frecuencia")]
        public string Frequency { get; set; }
        [Display(Name = "Días sin realizar")]
        public int Dias_Sin_Realizar { get; set; }
    }
}
