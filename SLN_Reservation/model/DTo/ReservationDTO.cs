using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLN_Reservation.model.DTo
{
    public class ReservationDTO
    {
        /// <summary>
        /// Identificador del cliente (cédula, pasaporte, ó Id interno)
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Fecha de entrada
        /// </summary>
        public DateTime CheckIn { get; set; }

        /// <summary>
        /// Fecha de salida
        /// </summary>
        public DateTime CheckOut { get; set; }

        /// <summary>
        /// Cantidad de huéspedes
        /// </summary>
        public int GuestCount { get; set; }

        /// <summary>
        /// Id de la habitación seleccionada
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// (Opcional) Descripción libre de la reserva
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Número de noches
        /// </summary>
        public int Nights { get; set; }
    }
}
