
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
    public class DailyJobsService : IDailyJobsService
    {
        private IDailyJobsRepository _dailyJobsRepository;

        public DailyJobsService(IDailyJobsRepository idailyJobsRepository)
        {
            this._dailyJobsRepository = idailyJobsRepository;
        }
        public List<DailyJobsE> GetList(DailyJobsE dailyJobsE)
        {
            try
            {
                return _dailyJobsRepository.GetList(dailyJobsE);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Maintenance(DailyJobsE dailyJob)
        {
            try
            {
                return _dailyJobsRepository.Maintenance(dailyJob);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}