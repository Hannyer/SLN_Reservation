using EntityLayer;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class AboutService : IAboutService
    {
        IAboutRepository  _aboutRepository;
        
        public AboutService(IAboutRepository aboutRepository) { 
        _aboutRepository = aboutRepository;
        }
        public List<AboutE> GetList(AboutE about)
        {
            return _aboutRepository.GetList(about);
        }
    }
}