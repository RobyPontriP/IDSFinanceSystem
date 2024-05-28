using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class RenewPassController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Maintenance/RenewPass
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            ViewBag.Title = "Renew Password";
            return View();
        }

        public JsonResult ChangePass()
        {
            var result = new { status = "error", msg = "ErrDesc" };
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var oldpass = o.SelectToken("oldpass").ToString();
                var newpass = o.SelectToken("newpass").ToString();
                var confirmpass = o.SelectToken("confirmpass").ToString();
                if (IDS.Maintenance.User.UserPassChange(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_ID]), new Tool.clsCryptho().Encrypt(oldpass, "ids"), new Tool.clsCryptho().Encrypt(newpass, "ids"), new Tool.clsCryptho().Encrypt(confirmpass, "ids")))
                {
                    result = new { status = "success", msg = "Sucess Change Password" };
                }
                else
                {
                    result = new { status = "error", msg = "Wrong Password" };
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}