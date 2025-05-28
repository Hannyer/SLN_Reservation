using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IUserService
    {
        List<UserE> GetList(UserE user);
        bool Maintenance(int P_OPCION, int P_ID, string P_USER, string P_PASSWORD, int P_ROLE);
        bool Maintenance(UserE user);
    }
}
