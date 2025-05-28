using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IMenuService
    {
        List<MenuE> GetList(MenuE menu);
        void Maintenance(MenuE menu);
    }
}
