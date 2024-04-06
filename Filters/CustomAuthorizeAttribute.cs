using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagementProject.Filters
{
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session["UserId"] == null || (int)HttpContext.Current.Session["UserId"] != 1)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}