using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Malibu_Reservation.Filters
{
    public class Filter:ActionFilterAttribute
    {
        private UserE user;

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
            base.OnActionExecuted(filterContext);

                user = (UserE)HttpContext.Current.Session["user"];
                if(user == null) {
                    if (filterContext.Controller is LoginController == false) {
                        filterContext.HttpContext.Response.Redirect("/Login/Index");
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}