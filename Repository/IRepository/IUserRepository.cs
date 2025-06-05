using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public  interface IUserRepository
    {
        #region SQL

        List<UserE> GetList(UserE user);
        bool Maintenance(int P_OPCION, int P_ID, string P_USER, string P_PASSWORD,int P_ROLE);
        bool Maintenance(UserE user);

        #endregion



        #region SUPABASE

        Task<List<UserE>> GetListAsync(UserE user);
        Task<bool> MaintenanceAsync(UserE user);
        Task<bool> MaintenanceAsync(int opcion, int id, string user, string password, int role);

        #endregion

    }
}
