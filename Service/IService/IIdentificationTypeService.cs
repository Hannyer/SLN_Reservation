using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.IService
{
    public interface IIdentificationTypeService
    {
        List<IdentificationTypeE> GetList(IdentificationTypeE cient);

        bool Maintenance(IdentificationTypeE identificationType);
    }
}