using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class RateService : IRateService
    {
        private IRateRepository _repository;
        public RateService(IRateRepository rateRepository) { 
        _repository = rateRepository;
        }
        public List<RateE> GetList(RateE Rate)
        {
            try
            {
                return _repository.GetList(Rate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Maintenance(RateE Rate)
        {
            throw new NotImplementedException();
        }
    }
}