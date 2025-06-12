using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer

{
    public class Hotel_RoomE:BaseE
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime? StardDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public decimal DolarPrice { get; set; }
    }
}
