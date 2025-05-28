using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IRoleRepository
    {
        List<RoleE> GetList(RoleE RoleE);
        int Maintenance(RoleE RoleE);
    }
}
