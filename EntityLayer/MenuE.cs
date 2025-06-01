using EntityLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class MenuE : BaseE
    {
        [JsonProperty("ID_MENU")]
        public int ID { get; set; }
        public int IDP_ROLE { get; set; }
        public string PARENT_CODE { get; set; }
        [Display(Name = "Descripción")]
        public string DESCRIPTION { get; set; }
        public string ICON { get; set; }
        public string CONTROLLER { get; set; }
        public string ACTION { get; set; }
        [Display(Name = "Acceso")]
        public bool STATUS_Menu { get; set; }
        [Display(Name = "Crear")]
        public bool Creeate_Menu { get; set; }
        [Display(Name = "Editar")]
        public bool Edit_Menu { get; set; }
        [Display(Name = "Ver")]
        public bool View_Menu { get; set; }
        [Display(Name = "Enviar")]
        public bool Send_Menu { get; set; }
        public bool Permisson_Status { get; set; }
        public bool Permisson_Create { get; set; }
        public bool Permisson_Edit { get; set; }
        public bool Permisson_View { get; set; }
        public bool Permisson_Send { get; set; }

        public int Order { get; set; }
        public string Image { get; set; }


        public void Limpiar() {
            STATUS_Menu = false; Creeate_Menu = false;
            Edit_Menu = false; View_Menu = false; Send_Menu=false; Permisson_Status = false; Permisson_Create = false; Permisson_Edit = false; Permisson_View = false; Permisson_Send = false;      


        }
    }
}

