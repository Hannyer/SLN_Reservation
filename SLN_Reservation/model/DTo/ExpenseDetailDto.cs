using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLN_Reservation.model.DTo
{
    public class ExpenseDetailDto
    {
        public int Nights { get; set; }
        public decimal PricePerNight { get; set; }
        public double SubTotal { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}