using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class DeleteInvoiceController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Sales/DeleteInvoice
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

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }

            ViewData["DefBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");


            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }///

        public JsonResult GetInvoiceNumber(string period, string cust, string typx)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }
            List<IDS.Sales.Invoice> acc = IDS.Sales.Invoice.GetInvoicenNumber(period, cust, typx);
            return Json(acc, JsonRequestBehavior.AllowGet);
        }


        public string FormConfirmDelete(string tPeriod, string tBranch, string tCust, string tInvoice)
        {
            return (IDS.Sales.DeleteInvoice.Confirm_Delete_Invoice(tInvoice, tCust, tPeriod, tBranch));
        }

        public string ProcessDeleteInv(string cust, string inv, string sCode, string voucher, string branch, string ccy, string invoiceDate, string pphamount, string invoiceamount)
        {
            return (IDS.Sales.DeleteInvoice.Finish_Delete_Invoice_With_SP(cust, inv, sCode, voucher, branch, ccy, invoiceDate, pphamount, invoiceamount));
        }

        private string WebMsgApp_WebMsgBox_Show(string comment)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='ConfirmForm' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'>");
            b.AppendLine(" <div class='modal-dialog modal-dialog-centered'>");
            b.AppendLine("     <div class='modal-content'>");
            b.AppendLine("       <div class='modal-header'><h5 class='modal-title' id='exampleModalLongTitle'> Confirm </h5>");
            b.AppendLine("           <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goClose()'>");
            b.AppendLine("               <span aria-hidden='true'>&times;</span>");
            b.AppendLine("           </button>");
            b.AppendLine("      </div>");
            b.AppendLine("      <div class='modal-body'>");
            b.AppendLine("         <h6>" + comment + "</h6>");
            b.AppendLine("     </div>");
            b.AppendLine("      <div class='modal-footer'>");
            b.AppendLine("          <div class='container my-3 bg-light'>");
            b.AppendLine("              <div class='col-md-12 text-center'>");
            b.AppendLine("                  <button type='button' class='btn btn-primary btn-block' data-dismiss='modal' onclick='goClose()'>Close</button>");
            b.AppendLine("               </div>");
            b.AppendLine("           </div>");
            b.AppendLine("       </div>");
            b.AppendLine("     </div>");
            b.AppendLine(" </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

    }// public class
}// name space