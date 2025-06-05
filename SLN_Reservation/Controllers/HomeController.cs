using EntityLayer;

using Repository.Repository;
using Service.IService;
using Service.Service;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SLN_Reservation.Controllers
{
    public class HomeController : Controller
    {

        private IUserService _service;
        private IMenuService _menuService;
        IDailyJobsService _dailyJobsService;
        IUserService _UserService;
        public HomeController(IUserService service, IMenuService menuService , IDailyJobsService dailyJobsService, IUserService userService) { 
            _service = service;
            _menuService = menuService;
            _dailyJobsService = dailyJobsService;
            _UserService = userService;
        }
        public async Task<ActionResult> Index()
        {
            try
            {
                if (Session["User"] == null)
                {

                    return RedirectToAction("Index", "Login");
                }
                UserE userSession = Session["User"] as UserE;

                if (userSession.ResetPassword)
                {
                    var userFromDb = _service.GetList(new UserE() { Opcion = 0, ID = userSession.ID }).FirstOrDefault();

                    if (userFromDb != null && userFromDb.ResetPassword)
                    {
                        ViewBag.ShowChangePasswordModal = true;
                    }
                    else
                    {
                        userSession.ResetPassword = false;
                        Session["User"] = userSession;
                        ViewBag.ShowChangePasswordModal = false;
                    }
                }
                else
                {
                    ViewBag.ShowChangePasswordModal = false;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangePasswordFromModal(string newPassword, string confirmNewPassword)
        {
            UserE currentUser = Session["User"] as UserE;

            if (currentUser == null)
            {
                return Json(new { success = false, message = "Tu sesión ha expirado. Por favor, inicia sesión de nuevo." });
            }

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                return Json(new { success = false, message = "Por favor, ingresa y confirma la nueva contraseña." });
            }

            if (newPassword != confirmNewPassword)
            {
                return Json(new { success = false, message = "La nueva contraseña y la confirmación no coinciden." });
            }

            try
            {
                var userToUpdate = _UserService.GetList(new UserE() { Opcion = 0, ID = currentUser.ID }).FirstOrDefault();

                if (userToUpdate == null)
                {
                    return Json(new { success = false, message = "Error: No se encontró la información del usuario en la base de datos." });
                }

                userToUpdate.Password = newPassword;
                userToUpdate.ResetPassword = false; 
                userToUpdate.Opcion = 2;

                bool updateResult = _UserService.Maintenance(userToUpdate);

                if (updateResult)
                {                    
                    currentUser.Password = newPassword;
                    currentUser.ResetPassword = false;
                    Session["User"] = currentUser;

                    return Json(new { success = true, message = "Contraseña cambiada exitosamente. Ahora puedes continuar." });
                }
                else
                {
                    return Json(new { success = false, message = "Error: No se pudo actualizar la contraseña en la base de datos." });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en ChangePasswordFromModal: {ex.Message}");
                return Json(new { success = false, message = "Ocurrió un error inesperado al cambiar la contraseña. Por favor, inténtalo de nuevo más tarde." });
            }
        }

    }
}