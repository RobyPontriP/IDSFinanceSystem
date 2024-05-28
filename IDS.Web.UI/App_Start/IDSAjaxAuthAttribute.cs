using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IDSAjaxAuthAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                HttpContextBase context = filterContext.HttpContext;
                HttpSessionStateBase session = context.Session;

                if (session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null || string.IsNullOrEmpty(Convert.ToString(session[IDS.Tool.GlobalVariable.SESSION_USER_ID])))
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.StatusDescription = "Your session is expired or you do not have access to access supporting data";
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "Error403" }));
                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = new { message = "Your session is expired or you do not have access to access supporting data" },
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                    filterContext.HttpContext.Response.End();
                }
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}