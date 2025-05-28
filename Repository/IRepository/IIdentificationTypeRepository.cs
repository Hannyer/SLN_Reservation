
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IIdentificationTypeRepository
    {
        List<IdentificationTypeE> GetList(IdentificationTypeE IdentificationTypeE);

        bool Maintenance(IdentificationTypeE identificationType);
    }
}
