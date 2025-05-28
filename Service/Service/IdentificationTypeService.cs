using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class IdentificationTypeService : IIdentificationTypeService
    {
        private IIdentificationTypeRepository _repository;
        public IdentificationTypeService(IIdentificationTypeRepository repository)
        {
            _repository = repository;
        }
        public List<IdentificationTypeE> GetList(IdentificationTypeE IdentificationTypeE)
        {
            try
            {
                return _repository.GetList(IdentificationTypeE);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Maintenance(IdentificationTypeE identificationType)
        {
            throw new NotImplementedException();
        }
    }
}