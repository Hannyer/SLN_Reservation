
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IDailyJobsRepository
    {
        List<DailyJobsE> GetList(DailyJobsE dailyJobsE);
        bool Maintenance(DailyJobsE dailyJob);
    }
}
