using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _Rolerepository;
        public RoleService(IRoleRepository service)
        {
            this._Rolerepository = service;
        }
        List<RoleE> IRoleService.GetList(RoleE RoleE)
        {
           return _Rolerepository.GetList(RoleE);
        }

        int IRoleService.Maintenance(RoleE RoleE)
        {
           return _Rolerepository.Maintenance(RoleE);
        }
    }
}