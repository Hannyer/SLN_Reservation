using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class PermissionE:BaseE
    {
        public int ID { get; set; }
        public int FK_Role { get; set; }
        public int FK_Menu { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public bool Create { get; set; }
        public bool Edit { get; set; }
        public bool View { get; set; }
        public bool Send { get; set; }
        public string ListMenu { set; get; }
        public string ListPermmison { set; get; }
    }
}
