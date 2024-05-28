using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class ProcessInvoiceController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Sales/ProcessInvoice
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

            ViewData["dtpValue"] = DateTime.Now.ToString("dd-MMM-yyyy");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string period, string invNo, string invDate)
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

            IDS.Sales.ProcessInvoice invProcess = new IDS.Sales.ProcessInvoice();
            invProcess.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //string ccy = IDS.GeneralTable.Syspar.GetInstance().BaseCCy;
            
            ViewData["result"] = invProcess.InvoiceProcess(period, invNo, Convert.ToDateTime(invDate));
            //Input SSP

            //if (inv.InvoiceAmount - inv.DiscountAmount + inv.PPnAmount > IDS.Sales.ReceiveSSP.GetBUMNLimit(Server.MapPath("~/BUMNLimit.txt")))
            //{
            //    IDS.Sales.ReceiveSSP.SaveSSP((int)IDS.Tool.PageActivity.Edit, inv.Branch.BranchCode, inv.InvoiceNumber, inv.Cust.CUSTCode, inv.OperatorID);
            //}

            //
            ViewData["period"] = period;
            ViewData["invdate"] = invDate;

            //ViewData["dtpValue"] = dtpPeriod;

            //ViewData["MessageError"] = process.MessageError;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        public JsonResult GetNextInvoiceNumber(int offset, string period)
        {
            string strResult = "";

            IDS.Sales.ProcessInvoice procInv = new IDS.Sales.ProcessInvoice();

            strResult = procInv.GetNextInvoiceNumber(offset, period);

            ViewData["NextInvoiceNumberHTML"] = strResult;

            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
    }
}