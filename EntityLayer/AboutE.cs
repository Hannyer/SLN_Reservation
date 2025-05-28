using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class AboutE:BaseE
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ID_Hotel { get; set; }
        public string SecurityCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Province { get; set; }
        public string Canton { get; set; }
        public string Distric { get; set; }

        public string OtherSigns { get; set; }
        public string Cabys_service { get; set; }
        public string ID_Type { get; set; }
        public string ActivityCode { get; set; }
    }
}
