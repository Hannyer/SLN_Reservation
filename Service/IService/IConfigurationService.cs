﻿using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IConfigurationService
    {
        List<ConfigurationE> GetList(ConfigurationE configurationE);
        int Maintenance(ConfigurationE configurationE);
    }
}
