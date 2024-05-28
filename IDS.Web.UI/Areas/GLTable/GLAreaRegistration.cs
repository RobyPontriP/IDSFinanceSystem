using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GLTable
{
    public class GLAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GLTable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GLTable_default",
                "GLTable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}