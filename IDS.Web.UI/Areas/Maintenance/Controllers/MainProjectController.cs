using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class MainProjectController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Maintenance/MainProject
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["MntMainProject"] = Newtonsoft.Json.JsonConvert.SerializeObject(IDS.Maintenance.UserMntMainProject.MntMainProjectDataTable());

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View("Index");
        }

        public JsonResult InsMaintenanceProject()
        {
            string return_ = "{'status':'error','msg':'Data Cant be saved'}";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var projectname = o.SelectToken("projectname").ToString();
                var loguser = o.SelectToken("loguser").ToString();
                var type = o.SelectToken("type").ToString();
                if (IDS.Maintenance.UserMntMainProject.InsDelMNTMainProject(projectname, loguser, int.Parse(type), "import"))
                {
                    return_ = "{'status':'ok','msg':'Data Hasbeen saved'}";
                }
            }
            return Json(return_, JsonRequestBehavior.AllowGet);
        }

        public JsonResult refresh()
        {
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(IDS.Maintenance.UserMntMainProject.MntMainProjectDataTable()), JsonRequestBehavior.AllowGet);
        }

    }
}