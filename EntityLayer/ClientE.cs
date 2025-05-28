using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ClientE : BaseE
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Full_Name { get; set; }
      
        public int IdentificationType_Id { get; set; }
        public int RateType_Id { get; set; }
        [Required(ErrorMessage = "El campo Ceédula es obligatorio.")]
        [Display(Name = "Cedula")]
        public string IdCard { get; set; }
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Telefono debe contener solo números.")]
        [Display(Name = "Numero1")]
        public string Phone_number1 { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Telefono debe contener solo números.")]
        [Display(Name = "Numero2")]
        public string Phone_number2 { get; set; }
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [Display(Name = "Correo")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "El campo Correo es obligatorio.")]
        [Display(Name = "Detalle")]
        public string Detail { get; set; }
        
        [Display(Name = "Tipo de identificación")]
        public string IdentificationType_Description { get; set; }
        [Display(Name = "Tipo de tarifa")]
        public string RateType_Description { get; set; }

    }
}
