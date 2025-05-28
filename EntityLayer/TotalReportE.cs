using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class TotalReportE : BaseE
    {
        public string Descripction { get; set; }
        public string ReservationType { get; set; }
        public double SubTotalWithOutTax { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public string checkIn { get; set; }
        public string checkOut { get; set; }
    }
}
