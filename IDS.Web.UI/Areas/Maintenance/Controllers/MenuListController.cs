using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class MenuListController : Controller
    {
        // GET: Maintenance/MenuList
        public ActionResult Index()
        {
            ViewBag.Title = "Web Menu Master";
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            return PartialView("Index");
        }

        public JsonResult RefreshMenuList()
        {
            System.Data.DataTable dt = IDS.Maintenance.User.GetWebMenuMaster();
            var studentNamesWithPercentage = dt.AsEnumerable().Select(x => new
            {
                MenuCode = x.Field<string>("MenuCode"),
                GroupCode = x.Field<string>("GroupCode"),
                FrmName = x.Field<string>("FrmName"),
                ProjectName = x.Field<string>("ProjectName"),
                Akses = x.Field<string>("Akses")
            }).ToList();
            return Json(studentNamesWithPercentage, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMenuListForDataSource()
        {
            return Json(IDS.Maintenance.User.GetMenuListForDataSource(), JsonRequestBehavior.AllowGet);
        }

        // List<System.Web.Mvc.SelectListItem> GetMenuListForDataSource()
    }
}