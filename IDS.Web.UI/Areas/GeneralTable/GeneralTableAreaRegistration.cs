using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GeneralTable
{
    public class GeneralTableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GeneralTable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            //context.Routes.Clear();

            context.MapRoute(
                "GeneralTable_default",
                "GeneralTable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}