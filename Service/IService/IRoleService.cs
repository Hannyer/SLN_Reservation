using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IRoleService
    {
        List<RoleE> GetList(RoleE RoleE);
        int Maintenance(RoleE RoleE);
    }
}
