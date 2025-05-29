using EntityLayer;
using Newtonsoft.Json;
using Service.IService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace SLN_Reservation.Controllers
{
    public class SecurityController : Controller
    {
        IPermissionService _permissionService;
        IRoleService _roleService;
        IMenuService _menuService;
        public SecurityController(IPermissionService service, IRoleService roleService, IMenuService menu)
        {
            _permissionService = service;
            _roleService = roleService;
            _menuService = menu;
        }

        public ActionResult Permissons()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListRole();

            List<MenuE> List = _menuService.GetList(new MenuE() { Opcion = 1 });
            ViewBag.SelectedRoleId = 0;


            return View(List);
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
        [HttpPost]
        public ActionResult SendData(string selectedData, int IdRoleselected)
        {

            try
            {
                List<MenuE> selectedMenuData = JsonConvert.DeserializeObject<List<MenuE>>(selectedData);
                PermissionE obPermisson = new PermissionE();
                string ListSelectedMenu =  GetSelectedMenus(selectedMenuData);
                string ListPermissonSelected = GetSelectedPermisson(selectedMenuData);
                obPermisson.Opcion = 0;

                obPermisson.FK_Role = IdRoleselected;
                obPermisson.ListMenu = ListSelectedMenu;
                obPermisson.ListPermmison = ListPermissonSelected;
                _permissionService.Maintenance(obPermisson);

                List<MenuE> updatedMenuData = _menuService.GetList(new MenuE()
                {
                    Opcion = 0,
                    IDP_ROLE = 0,
                    STATUS_Menu = false,
                    Permisson_Status = false
                });
                LimpiarCheck(updatedMenuData, 1);
                return Json(new { success = true });
            }
            catch (Exception)
            {

                return Json(new { success = false });
            }

        }



        public string GetSelectedMenus(List<MenuE> Lista)
        {
            List<MenuE> listaA = Lista.Where(x => x.STATUS_Menu).ToList();


            string SelectecMenu = "";
            foreach (MenuE menuE in listaA) { SelectecMenu += menuE.ID.ToString() + ","; }
            return SelectecMenu;
        }

        public string GetSelectedPermisson(List<MenuE> Lista)
        {
            List<MenuE> listaA = Lista.Where(x => x.STATUS_Menu).ToList();

            string SelectecPermison = "";
            foreach (MenuE menuE in listaA)
            {

                if (menuE.Creeate_Menu)
                {
                    SelectecPermison += "CREATE/" + menuE.ID + ",";
                }
                if (menuE.Edit_Menu)
                {
                    SelectecPermison += "EDIT/" + menuE.ID + ",";
                }
                if (menuE.View_Menu)
                {
                    SelectecPermison += "VIEW/" + menuE.ID + ",";
                }
                if (menuE.Send_Menu)
                {
                    SelectecPermison += "SEND/" + menuE.ID + ",";
                }



            }
            return SelectecPermison;
        }

        public ActionResult GetMenuDataByRole(string roleId)
        {
            if (!roleId.Equals(""))
            {

                List<MenuE> listMenu = _menuService.GetList(new MenuE()
                {
                    Opcion = 1,
                    IDP_ROLE = Convert.ToInt32(roleId),
                    STATUS_Menu = false,
                    Permisson_Status = false

                });
                FillDropDownListRole();
                return View("Permissons", listMenu);


            }
            else
            {
                FillDropDownListRole();
                List<MenuE> List = _menuService.GetList(new MenuE() { Opcion = 0 });
                return View("Permissons", List);
            }






        }

        public ActionResult RefreshPermissions(int roleId)
        {
            FillDropDownListRole();
            List<MenuE> updatedMenuData = _menuService.GetList(new MenuE()
            {
                Opcion = 0,
                IDP_ROLE = roleId,
                STATUS_Menu = false,
                Permisson_Status = false
            });


            return View("Permissons", updatedMenuData);
        }


        public ActionResult PartialPermissionView(int roleId)
        {
            var sw = new Stopwatch();

            if (roleId != 0)
            {
                sw.Start();
                List<MenuE> updatedMenuData = _menuService.GetList(new MenuE()
                {
                    Opcion = 0,
                    IDP_ROLE = roleId,
                    STATUS_Menu = false,
                    Permisson_Status = false
                });
                sw.Stop();
                var tiempoGetList = sw.Elapsed;
                Debug.WriteLine($"GetList tardó: {tiempoGetList.TotalMilliseconds} ms");
                LimpiarCheck(updatedMenuData, 1);
                return PartialView(OrderMenus(updatedMenuData));
            }
            else
            {
                List<MenuE> updatedMenuData = _menuService.GetList(new MenuE()
                {
                    Opcion = 0,
                    IDP_ROLE = roleId,
                    STATUS_Menu = false,
                    Permisson_Status = false
                });
                LimpiarCheck(updatedMenuData, 0);
                return PartialView(updatedMenuData);
            }




        }

        public void LimpiarCheck(List<MenuE> updatedMenuData, int Option)
        {
            if (Option == 0)
            {
                if (updatedMenuData != null)
                {
                    foreach (MenuE item in updatedMenuData)
                    {
                        item.Limpiar();
                    }
                }
            }
            else
            {
                if (updatedMenuData != null)
                {
                    foreach (MenuE item in updatedMenuData)
                    {
                        if (item.STATUS_Menu == false || item.Permisson_Status == false)
                        {
                            item.Limpiar();

                        }
                    }
                }
            }
        }

        public ActionResult RoleIndex()
        {
            if (Session["User"] == null || Session["List_Menu"] == null)
            {

                return RedirectToAction("Index", "Login");
            }
            FillDropDownListRole();

            List<RoleE> List = _roleService.GetList(new RoleE() { Opcion = 0 });
            ViewBag.SelectedRoleId = 0;


            return View(List);
        }
        [HttpPost]
        public string NewRole(RoleE role)
        {

            try
            {
                string answer = "";
                int result = _roleService.Maintenance(role);
                if (result > 0)
                {
                    answer = "¡Rol agregado exitosamente!";
                    RedirectToAction("RoleIndex");

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

        [HttpPost]
        public string UpdateRole(RoleE role)
        {

            try
            {
                string answer = "";
                int result = _roleService.Maintenance(role);
                if (result > 0)
                {
                    answer = "¡Rol modificado exitosamente!";
                    RedirectToAction("RoleIndex");

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
        [HttpPost]
        public string DeleteRole(RoleE role)
        {

            try
            {
                string answer = "";
                int result = _roleService.Maintenance(role);
                if (result > 0)
                {
                    answer = "¡Rol eliminado exitosamente!";
                    RedirectToAction("RoleIndex");

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
        public static List<MenuE> OrderMenus(List<MenuE> menus)
        {
            int counterParent = 1;
            int counterChild = 1;
            var parentMap = menus
                .Where(m => m.PARENT_CODE == "0" && m.Order != 0)
                .OrderBy(m => m.Order)
                .ToDictionary(m => m.ID.ToString(), m => m);

           
            var orderedMenus = new List<MenuE>();

           
            void AddChildren(MenuE parent)
            {
               
                var children = menus
                    .Where(m => m.PARENT_CODE == parent.ID.ToString())
                    .OrderBy(m => m.Order);

                
                foreach (var child in children)
                {
                    child.DESCRIPTION = "          "+counterParent+"."+counterChild+" " + child.DESCRIPTION;
                    counterChild++;
                    orderedMenus.Add(child);
                    AddChildren(child);
                }
            }

           
            foreach (var parent in parentMap.Values)
            {
                parent.DESCRIPTION = counterParent+" "+ parent.DESCRIPTION;
               
                counterChild = 1;
                orderedMenus.Add(parent); 
                AddChildren(parent);
                counterParent++;
            }

            return orderedMenus;
        }

    }
}