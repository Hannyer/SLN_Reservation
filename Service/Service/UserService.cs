using EntityLayer;
using Repository.IRepository;
using Repository.Supabase;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository iUserRepository)
        {
            this._userRepository = iUserRepository;
        }

        #region SQL
        public List<UserE> GetList(UserE user)
        {
            try
            {
                return _userRepository.GetList(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Maintenance(int P_OPCION, int P_ID, string P_USER, string P_PASSWORD, int P_ROLE)
        {
            throw new NotImplementedException();
        }

        public bool Maintenance(UserE user)
        {
            try
            {
                return _userRepository.Maintenance(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion


        #region SUPABASE

        public async Task<List<UserE>> GetListAsync(UserE user)
        {
            try
            {
                return await _userRepository.GetListAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> MaintenanceAsync(UserE user)
        {
            try
            {
                return await _userRepository.MaintenanceAsync(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> MaintenanceAsync(int opcion, int id, string user, string password, int role)
        {
            try
            {
                return await _userRepository.MaintenanceAsync(opcion, id, user, password, role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
