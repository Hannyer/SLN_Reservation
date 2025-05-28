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
        public string Client { get; set;}
        public string Status { get; set; }
        public string Days { get; set; }
        public string ReservationType { get; set;}
  
        public string Descripction { get; set;}
        public string checkIn { get; set; }
        public string checkOut { get; set; }
        public double SubTotalWithOutTax { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }


        


    }
}
