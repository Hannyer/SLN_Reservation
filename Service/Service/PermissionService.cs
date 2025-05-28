using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class PermissionService : IPermissionService
    {
        private IPermissionRepository _PermissionRepository;
        public PermissionService(IPermissionRepository permissionRepository)
        {
            this._PermissionRepository = permissionRepository;
        }

        public List<PermissionE> GetList(PermissionE permission)
        {
            throw new NotImplementedException();
        }

        public void Maintenance(PermissionE permission)
        {
            _PermissionRepository.Maintenance(permission);
        }
    }
}