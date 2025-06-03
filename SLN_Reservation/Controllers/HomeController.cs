using EntityLayer;

using Repository.Repository;
using Service.IService;
using Service.Service;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SLN_Reservation.Controllers
{
    public class HomeController : Controller
    {

        private IUserService _service;
        private IMenuService _menuService;
        IDailyJobsService _dailyJobsService;
        public HomeController(IUserService service, IMenuService menuService , IDailyJobsService dailyJobsService) { 
            _service = service;
            _menuService = menuService;
            _dailyJobsService = dailyJobsService;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                if (Session["User"] == null)
                {

                    return RedirectToAction("Index", "Login");
                }
                ViewBag.DailyJobCount= _dailyJobsService.GetList(new DailyJobsE() { Opcion = 1 }).Count;
                var menuService = new MenuService(new MenuRepository());
                if (Session["List_Menu"] == null) { 
                var menuList = menuService.GetList(new MenuE() { IDP_ROLE = (Session["User"] as UserE).Id_Role, STATUS_Menu = true, Permisson_Status = true });
                Session["List_Menu"] = menuList;
                }

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}