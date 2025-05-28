using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ConfigurationE : BaseE
    {
        public long PK_CONFIGURATION { get; set; }
        public bool ESTADO { get; set; }
        [Display(Name = "Descripción")]
        public string DESCRIPTION { get; set; }
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }
        public string OBSERVACION { get; set; }
        [Display(Name = "Llave_01")]
        public string KEY01 { get; set; }
        [Display(Name = "Llave_02")]
        public string KEY02 { get; set; }
        [Display(Name = "Llave_03")]
        public string KEY03 { get; set; }
        [Display(Name = "Llave_04")]
        public string KEY04 { get; set; }
        [Display(Name = "Llave_05")]
        public string KEY05 { get; set; }
        [Display(Name = "Llave_06")]
        public string KEY06 { get; set; }
        [Display(Name = "Valor")]
        public string VALUE { get; set; }
    }
}
