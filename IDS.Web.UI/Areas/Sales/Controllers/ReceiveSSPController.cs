using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class ReceiveSSPController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string custCode, string period,string rcvStatus)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Sales.ReceiveSSP> SSP = IDS.Sales.ReceiveSSP.GetReceiveSSP(custCode, period);

                totalRecords = SSP.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            SSP = SSP.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    SSP = SSP.Where(x => x.Branch.BranchCode.ToString().ToLower().Contains(searchValueLower) ||
                                    x.Customer.CUSTCode.ToString().ToLower().Contains(searchValueLower) ||
                                    x.Invoice.InvoiceNumber.ToString().ToLower().Contains(searchValueLower) ||
                                    x.Invoice.InvoiceDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                    x.Invoice.CCy.CurrencyCode.ToString().ToLower().Contains(searchValueLower) ||
                                    x.PrintDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                    x.ReceiveDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                    x.ReceiveOperator.ToLower().Contains(searchValueLower) ||
                                    x.ReceiveEntryDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT.ToLower()).Contains(searchValueLower)).ToList();
                }

                if (rcvStatus =="False")
                {
                    SSP = SSP.Where(x => !x.ReceiveStatus).ToList();
                }
                else
                {
                    SSP = SSP.Where(x => x.ReceiveStatus==true).ToList();
                }

                totalRecordsShowing = SSP.Count();

                // Paging
                SSP = SSP.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = SSP }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: Sales/ReceiveSSP
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource("1"), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }

        public ActionResult SaveSSP(string rcvDate,string invsNoList, string branchsList,string custCodesList)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0 || AccessLevel.CreateAccess == 0)
            {
                return Json("NoAccess", JsonRequestBehavior.AllowGet);
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            if (string.IsNullOrWhiteSpace(invsNoList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] invsNoCode = invsNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] branchsCode = branchsList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] custsCode = custCodesList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (invsNoCode.Length > 0)
                {
                    IDS.Sales.ReceiveSSP rcvSSP = new IDS.Sales.ReceiveSSP();
                    rcvSSP.ReceiveOperator = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

                    rcvSSP.SaveSSP(rcvDate, invsNoCode, branchsCode, custsCode);
                }

                return Json("Receive SSP data has been save successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}