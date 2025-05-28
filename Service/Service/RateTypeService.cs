using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class RateTypeService : IRateTypeService
    {
        private IRateTypeRepository _rateTypeRepository;
        public RateTypeService(IRateTypeRepository rateTypeRepository)
        {
            _rateTypeRepository = rateTypeRepository;
        }
        public List<RateTypeE> GetList(RateTypeE RateType)
        {
            return _rateTypeRepository.GetList(RateType);
        }

        public bool Maintenance(RateTypeE RateType)
        {
            throw new NotImplementedException();
        }
    }
}