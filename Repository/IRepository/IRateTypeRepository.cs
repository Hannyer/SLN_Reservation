﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IRateTypeRepository
    {
        List<RateTypeE> GetList(RateTypeE RateType);
        bool Maintenance(RateTypeE RateType);
    }
}
