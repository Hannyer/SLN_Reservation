
using EntityLayer;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace SLN_Malibu_Reservation.Controllers
{
    public class UserController : Controller
    {
        private IUserService _service;
        IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            this._service = userService;
            _roleService = roleService;
        }
        // GET: User
        public ActionResult Index()
        {

            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListRole();
            var list = _service.GetList(new UserE());           
            return View(list);
        }
        public void FillDropDownListRole()
        {
            var roles = _roleService.GetList(new RoleE() { Opcion = 1 });


            var roleList = roles.Select(role => new SelectListItem
            {
                Value = role.ID_Role.ToString(),
                Text = role.Description
            });


            ViewBag.RoleList = roleList;

        }
        public string NewUser(int P_OPCION, int P_ID, string P_USER, string P_PASSWORD, int P_ROLE)
        {
            string answer = "";
            bool tmpAnswer = _service.Maintenance(P_OPCION, P_ID, P_USER, P_PASSWORD, P_ROLE);
            if (tmpAnswer)
            {
                answer = "Usuario agregada con éxito";
                RedirectToAction("Index");
            }
            else
            {
                answer = "Ha ocurrido un error";
            }
            return answer;
        }

        [HttpPost]
        public string UpdateUser(UserE UserRequest)
        {

            try
            {
                string answer = "";
                bool result = _service.Maintenance(UserRequest);
                if (result)
                {
                    answer = "¡Usuario modificado exitosamente!";
                    RedirectToAction("Index");

                }
                else
                {
                    answer = "Ha ocurrido un error al modificar el usuario";
                }
                return answer;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public string AddUser(UserE UserRequest)
        {

            try
            {
                string answer = "";
                bool result = _service.Maintenance(UserRequest);
                if (result)
                {
                    answer = "¡Usuario agregado exitosamente!";
                    RedirectToAction("Index");

                }
                else
                {
                    answer = "Ha ocurrido un error al agregar el usuario";
                }
                return answer;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        public string DeleteUser(UserE UserRequest)
        {

            try
            {
                string answer = "";
                bool result = _service.Maintenance(UserRequest);
                if (result)
                {
                    answer = "¡Usuario eliminado exitosamente!";
                    RedirectToAction("Index");

                }
                else
                {
                    answer = "Ha ocurrido un error al agregar el usuario";
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