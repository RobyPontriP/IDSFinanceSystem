using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GLTransaction
{
    public class GLTransactionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GLTransaction";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GLTransaction_default",
                "GLTransaction/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}