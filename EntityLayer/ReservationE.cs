using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EntityLayer
{
    public class ReservationE : BaseE
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Cédula es obligatorio.")]
        [Display(Name = "Cédula")]
        public string IdCard_Client { get; set; }
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Full_Name { get; set; }

        [Display(Name = "Nombre")]
        public string Client_Mail { get; set; }
        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        [Display(Name = "Descripción")]
        public string Reservation_Description { get; set; }
        [Required(ErrorMessage = "El campo Entrada es obligatorio.")]
        [Display(Name = "Entrada")]
        public DateTime CheckIn { get; set; }
        [Required(ErrorMessage = "El campo Salida es obligatorio.")]
        [Display(Name = "Salida")]
        public DateTime CheckOut { get; set; }
        [Required(ErrorMessage = "El campo Estado es obligatorio.")]
        [Display(Name = "Estado")]
        public string Status { get; set; }
        [Required(ErrorMessage = "El campo Días es obligatorio.")]
        [Display(Name = "Días")]
        public int Days { get; set; }
        [Required(ErrorMessage = "Debe definir el total de ocupantes de la habitación")]
        [Display(Name = "Cantidad ocupantes")]
        public int ID_Rate { get; set; }
        [Display(Name = "Tarifa")]
        public string DisplayName_Rate { get; set; }
        [Display(Name = "Descripción_Tarifa")]
        public string Rate_Description { get; set; }
        [Display(Name = "Precio")]
        public decimal Price { get; set; }
        [Display(Name = "Moneda")]
        public string Currency { get; set; }
        [Display(Name = "Tipo de tarifa")]
        public string RateType_Description { get; set; }
        [Display(Name = "ID_ROOM")]
        public int ID_ROOM { get; set; }
        [Display(Name = "DESCRIPTION_HOTELROOM")]
        public string DESCRIPTION_HOTELROOM { get; set; }
        [Display(Name = "ID_USER")]
        public int ID_USER { get; set; }
        [Display(Name = "Description_Room")]
        public int Description_Room { get; set; }
        [Display(Name = "Depósito")]
        public decimal Deposit { get; set; }

        public double SubtotalWithoutTax { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount{ get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }

        [Display(Name = "Entrada Formateada")]
        public string FormattedCheckIn
        {
            get
            {
            
                if (CheckIn == DateTime.MinValue)
                {
                    return "Sin Fecha";
                }
                return CheckIn.ToString("dd/MM/yyyy");
            }
        }
        [Display(Name = "Salida Formateada")]
        public string FormattedCheckOut
        {
            get
            {

                if (CheckOut == DateTime.MinValue)
                {
                    return "Sin Fecha";
                }
                return CheckOut.ToString("dd/MM/yyyy");
            }
        }


    }
}
