using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GLProcess.Controllers
{
    public class ProcessController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: GLProcess/Process
        [HttpGet]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            IDS.GeneralTable.Branch branch = new IDS.GeneralTable.Branch();

            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }

            ViewData["dtpValue"] = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]    
        public ActionResult Index(string dtpPeriod, string chkToProcess,string branch)
        {
            IDS.GeneralTable.Branch branchObj = new IDS.GeneralTable.Branch();

            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", branch);
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }

            IDS.GLProcess.Process process = new IDS.GLProcess.Process();
            process.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

            int procFailed = 0;
            int procSuccess = 0;

            

            string ccy = IDS.GeneralTable.Syspar.GetInstance().BaseCCy;
            //string result = "";

            //for (int i = 1; i < process.GLProcess(Convert.ToDateTime(dtpPeriod), branch, chkToProcess).Count; i++)
            //{

            //}

            ViewData["result"] = process.GLProcess(Convert.ToDateTime(dtpPeriod), branch, chkToProcess);

            ViewData["dtpValue"] = dtpPeriod;

            ViewData["MessageError"] = process.MessageError;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        public JsonResult GetClosingStatus(string period, string branch)
        {
            return Json(IDS.GLTable.ACFMCTR.GetClosingStatus(Convert.ToDateTime(period).ToString("yyyyMM"), branch), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetClosingStatusWithDate(string period, string branch)
        //{
        //    period = Convert.ToDateTime(period).ToString("yyyyMM");
        //    return Json(IDS.GLTable.ACFMCTR.GetClosingStatus(Convert.ToDateTime(period).ToString("yyyyMM"), branch), JsonRequestBehavior.AllowGet);
        //}
    }
}