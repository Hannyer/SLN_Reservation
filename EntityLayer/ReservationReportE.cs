using EntityLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ReservationReportE : BaseE
    {
        public int Id { get; set; }
        public string Identification { get; set; }
        public string Days { get; set; }
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public double SubTotalWithOutTax { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RoomDescription { get; set; }
        public string RoomName { get; set; }
        public int RoomCapacity { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; }

    }
}
