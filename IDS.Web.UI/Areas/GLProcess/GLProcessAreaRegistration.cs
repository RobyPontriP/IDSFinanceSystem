using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GLProcess
{
    public class GLProcessAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GLProcess";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GLProcess_default",
                "GLProcess/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}