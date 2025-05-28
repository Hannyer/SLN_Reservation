using EntityLayer;
using Newtonsoft.Json;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Malibu_Reservation.Controllers
{
    public class ConfigurationController : Controller
    {
        IConfigurationService _configurationService;
        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }
       
        public ActionResult Index()
        {
            try
            {
                if (Session["User"] == null || Session["List_Menu"] == null)
                {

                    return RedirectToAction("Index", "Login");
                }
                List<ConfigurationE> List = _configurationService.GetList(new ConfigurationE() { Opcion=0});

            return View(List);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public string UpdateConfiguration(ConfigurationE configuracion)
        {

            try
            {
                string answer = "";
                int result = _configurationService.Maintenance(configuracion);
                if (result>0)
                {
                    answer = "¡Configuración modificada exitosamente!";
                    RedirectToAction("Index");
                    
                }
                else
                {
                    answer = "Ha ocurrido un error";
                }
                return answer;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}