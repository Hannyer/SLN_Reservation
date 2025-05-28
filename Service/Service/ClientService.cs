using Repository.Repository;
using Repository.IRepository;
using EntityLayer;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class ClientService : IClientService
    {
        private IClientRepository _clienRepository;

        public ClientService(IClientRepository icliendService)
        {
            this._clienRepository = icliendService;
        }
        public List<ClientE> GetList(ClientE cient)
        {
            try
            {
                return _clienRepository.GetList(cient);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Maintenance(ClientE client)
        {
            try
            {
                return _clienRepository.Maintenance(client);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}