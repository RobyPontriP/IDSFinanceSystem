using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class ProcessAccrualController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Sales/ProcessAccrual
        [HttpGet]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
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

            ViewData["PeriodList"] = new SelectList(IDS.Sales.ProcessAccrual.GetPeriodForDatasource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        //[HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        ////[ValidateAntiForgeryToken]    
        //public ActionResult Index(string dtpPeriod)
        //{
        //    string periodTemp = dtpPeriod;

        //    if (string.IsNullOrEmpty(dtpPeriod))
        //        dtpPeriod = DateTime.Now.ToString("yyyyMM");
        //    else
        //    {
        //        DateTime datePeriod = Convert.ToDateTime(dtpPeriod);
        //        dtpPeriod = datePeriod.ToString("yyyyMM");
        //    }

        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

        //    IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
        //    {
        //        RedirectToAction("Index", "Main", new { Area = "" });
        //    }

        //    ViewData["Page.Insert"] = AccessLevel.CreateAccess;
        //    ViewData["Page.Edit"] = AccessLevel.EditAccess;
        //    ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

        //    IDS.Sales.ProcessAccrual processAccrual = new IDS.Sales.ProcessAccrual();
        //    processAccrual.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

        //    string ccy = IDS.GeneralTable.Syspar.GetInstance().BaseCCy;

        //    ViewData["result"] = processAccrual.Process(dtpPeriod, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());

        //    ViewData["dtpValue"] = periodTemp;
        //    ViewData["PeriodList"] = new SelectList(IDS.Sales.ProcessAccrual.GetPeriodForDatasource(), "Value", "Text");
        //    ViewBag.UserMenu = MainMenu;


        //    return View();
        //}
        
        public ActionResult Process(string period)
        {
        //    string periodTemp = dtpPeriod;

        //    if (string.IsNullOrEmpty(dtpPeriod))
        //        dtpPeriod = DateTime.Now.ToString("yyyyMM");
        //    else
        //    {
        //        DateTime datePeriod = Convert.ToDateTime(dtpPeriod);
        //        dtpPeriod = datePeriod.ToString("yyyyMM");
        //    }

        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
        //    {
        //        RedirectToAction("Index", "Main", new { Area = "" });
        //    }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            IDS.Sales.ProcessAccrual processAccrual = new IDS.Sales.ProcessAccrual();
            processAccrual.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

            string ccy = IDS.GeneralTable.Syspar.GetInstance().BaseCCy;
            
            try
            {
                string result = processAccrual.Process(period, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());

                SelectList list = new SelectList(IDS.Sales.ProcessAccrual.GetPeriodForDatasource(), "Value", "Text");

                return Json(new { msg = result,list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}