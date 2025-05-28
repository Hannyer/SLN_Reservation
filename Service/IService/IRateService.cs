using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IRateService
    {
        List<RateE> GetList(RateE Rate);
        bool Maintenance(RateE Rate);
    }
}
