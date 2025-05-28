using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class RoleE:BaseE
    {
        [Display(Name = "ID")]
        public int ID_Role { get; set; }
        [Display(Name = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Estado")]
        public bool Status { get; set; }
    }
}
