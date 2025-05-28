using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SLN_Reservation.Filters
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class AuthorizeUser:AuthorizeAttribute
    {

        private UserE user;




    }
}