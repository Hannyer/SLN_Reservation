using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class RateE:BaseE
    {
        public int ID { get;set; }
        public string DisplayName { get;set; }
        public string Description { get;set; }
        public decimal Price { get;set; }
        public int ID_RateTýpe { get;set;}
        public string Currency { get; set; }
        public string Description_RateType { get; set; }
        public string Expense_Details { get; set; }
        public int Number_People { get; set; }
    }
}
