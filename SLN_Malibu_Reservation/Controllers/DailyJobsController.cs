
using EntityLayer;
using Service.IService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Malibu_Reservation.Controllers
{
    public class DailyJobsController : Controller
    {
        IDailyJobsService _dailyJobsService;
        IConfigurationService _configurationService;
        IUserService _userService;

               
        public DailyJobsController(IDailyJobsService idailyJobsService, IConfigurationService configurationService, IUserService userService)
        {
            this._dailyJobsService = idailyJobsService;
            _configurationService = configurationService;
            _userService = userService;
        }
        // GET: DailyJobs
        public ActionResult Index()
        {

            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListSeachCollaborator();
            var list = _dailyJobsService.GetList(new DailyJobsE());
            return View(list);
        }

        public string NewJobs(DailyJobsE dailyJob)
        {
            string answer = "";
            bool tmpAnswer = _dailyJobsService.Maintenance(dailyJob);
            if (tmpAnswer)
            {
                answer = "Tarea agregada con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }
        public string ModifyJobs(DailyJobsE dailyJob)
        {
            string answer = "";
            bool tmpAnswer = _dailyJobsService.Maintenance(dailyJob);
            if (tmpAnswer)
            {
                answer = "Tarea Modificada con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }

        public string DeleteJobs(int dailyJob)
        {
            
            string answer = "";
            bool tmpAnswer = _dailyJobsService.Maintenance(new DailyJobsE() {Opcion=1,Id=dailyJob, Date=DateTime.Now });
            if (tmpAnswer)
            {
                answer = "Labor eliminada con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }

        public string ToEndJobs(DailyJobsE dailyJob)
        {
            string answer = "";
            bool tmpAnswer = _dailyJobsService.Maintenance(dailyJob);
            if (tmpAnswer)
            {
                answer = "Tarea Finalizada con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }
        public ActionResult IndexDailyJob()
        {

            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            // FillDropDownListTypeDailyJob();
            var list = _dailyJobsService.GetList(new DailyJobsE() { Opcion = 1 });
            return View(list);
        }

        public PartialViewResult DailyJobListView(List<DailyJobsE> List)
        {

            return PartialView(List);
        }
        [HttpGet]
        public PartialViewResult UpdateTable()
        {
            var list = _dailyJobsService.GetList(new DailyJobsE() { Opcion = 1 });
            return PartialView("DailyJobListView", list);
        }
        [HttpPost]
        public string JobDone(DailyJobsE dailyJob)
        {
            var config = _configurationService.GetList(new ConfigurationE()
            {
                Opcion = 0,
                KEY01 = "PARAMETRO",
                KEY02 = "FUNCIONALIDAD",
                KEY03 = "MRB",
                KEY04 = "TIPO_LABOR",
                KEY05 = "",
                KEY06 = "",
            });

            dailyJob.Opcion = 2;
            dailyJob.Status = "0";
            if (dailyJob.Type.Equals(config.Where(X => X.KEY05 == "DIARIA").FirstOrDefault().VALUE))
            {
                dailyJob.Date = DateTime.Now.AddDays(1);
            }
            else
            {
                dailyJob.Date = DateTime.Now.AddDays(Convert.ToInt32(dailyJob.Frequency));
            }
            string answer = "";
            bool tmpAnswer = _dailyJobsService.Maintenance(dailyJob);
            if (tmpAnswer)
            {
                answer = "Tarea realizada exitosamente";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }
        public void FillDropDownListTypeDailyJob()
        {
            var List = _configurationService.GetList(new ConfigurationE() { Opcion = 0, KEY01 = "PARAMETRO", KEY02 = "FUNCIONALIDAD", KEY03 = "MRB", KEY04 = "TIPO_LABOR" });


            var dailyJobList = List.Select(config => new SelectListItem
            {
                Value = config.DisplayName.ToString(),
                Text = config.VALUE,

            });


            ViewBag.DailyJobList = dailyJobList;

        }
        public void FillDropDownListSeachCollaborator()
        {

            var AdminId = _configurationService.GetList(new ConfigurationE() { Opcion = 0, KEY01 = "PARAMETRO", KEY02 = "FUNCIONALIDAD", KEY03 = "MRB", KEY04 = "ROL", KEY05 = "ADMINISTRADOR" }).FirstOrDefault();

            var Users = _userService.GetList(new UserE() { Opcion = 0 }).Where(x => x.Id_Role != Convert.ToInt32(AdminId.VALUE));


            var userList = Users.Select(userL => new SelectListItem
            {
                Value = userL.User,
                Text = userL.Name,

            });


            ViewBag.userList = userList;

        }

    }
}