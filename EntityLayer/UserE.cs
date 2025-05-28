using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class UserE:BaseE
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        public string User { get; set; }
        
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un perfil para el usuario.")]
        [Display(Name = "Perfil")]
        public int Id_Role { get; set; }
        [Display(Name = "Descripcion")]
        public string Description { get; set; }
        [Display(Name = "Estado")]
        public bool Status { get; set; }

    }
}
