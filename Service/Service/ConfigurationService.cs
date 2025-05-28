using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ConfigurationService : IConfigurationService
    {
        IConfigurationRepository _configuration;

        public ConfigurationService(IConfigurationRepository configuration) { 
        _configuration = configuration;
        }
        public List<ConfigurationE> GetList(ConfigurationE configurationE)
        {
            return _configuration.GetList(configurationE); 
        }

        public int Maintenance(ConfigurationE configurationE)
        {
           return _configuration.Maintenance(configurationE);
        }
    }
}
