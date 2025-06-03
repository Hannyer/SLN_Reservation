using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer
{
    public class UserE : BaseE
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

        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
        public string Email { get; set; }

        [Display(Name = "Número de Teléfono")]
        [Phone(ErrorMessage = "Ingrese un número de teléfono válido.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Número de Documento")]
        public string DocumentID { get; set; }

        [Display(Name = "Cambio de Contraseña")]
        public bool ResetPassword { get; set; }

    }
}
