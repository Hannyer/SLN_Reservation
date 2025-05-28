using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IPermissionRepository
    {
        List<PermissionE> GetList(PermissionE permission);
        void Maintenance(PermissionE permission);
    }
}
