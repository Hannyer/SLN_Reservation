
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IDailyJobsService
    {
        List<DailyJobsE> GetList(DailyJobsE dailyJobsE);
        bool Maintenance(DailyJobsE dailyJob);
    }
}
