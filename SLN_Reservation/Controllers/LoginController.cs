
using EntityLayer;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation
{
    public class LoginController : Controller
    {
        IUserService _UserService;
        public LoginController(IUserService service) {
            _UserService = service;
        }
        public ActionResult Index()
        {
            Session["User"] = null;
            Session["List_Menu"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserE model)
        {
            if (ModelState.IsValid)
            {
                var user = _UserService.GetList(model).Where(x=>x.User.ToLower()==model.User.ToLower()).FirstOrDefault();
                
                if (user!=null  )
                {
                    if (user.Password == model.Password ) {
                        if (user.Status)
                        {
                            Session["User"] = user;
                            return RedirectToAction("Index", "Home");
                        }
                        else {
                            ModelState.AddModelError("", "El usuario se encuentra inactivo");
                        }
                    

                    }
                    else
                    {
                        ModelState.AddModelError("", "Credenciales incorrectas. Por favor, verifica tu usuario y contraseña.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Credenciales incorrectas. Por favor, verifica tu usuario y contraseña.");
                }
            }
            return View("Index",model);
        }
    }
}