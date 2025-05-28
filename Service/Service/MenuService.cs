using EntityLayer;
using Repository.IRepository;
using Repository.Repository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class MenuService : IMenuService
    {
        private IMenuRepository _menuRepository;
        public MenuService(IMenuRepository IMenuService)
        {
            this._menuRepository = IMenuService;
        }

        public List<MenuE> GetList(MenuE menu)
        {
            try
            {
                return _menuRepository.GetList(menu);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Maintenance(MenuE menu)
        {
            throw new NotImplementedException();
        }
    }
}