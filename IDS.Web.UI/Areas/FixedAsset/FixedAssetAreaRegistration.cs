using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset
{
    public class FixedAssetAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FixedAsset";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FixedAsset_default",
                "FixedAsset/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}