using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class CustOutstandingController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string period)
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

                List<IDS.Sales.CustomerOutstanding> custOuts = IDS.Sales.CustomerOutstanding.GetCustOutstanding(period);

                totalRecords = custOuts.Count;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            custOuts = custOuts.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    custOuts = custOuts.Where(x => x.Customer.CUSTCode.ToLower().Contains(searchValueLower) ||
                                             x.Period.ToLower().Contains(searchValueLower) ||
                                             x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                             x.BegBal.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Debit.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Credit.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = custOuts.Count();

                // Paging
                custOuts = custOuts.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = custOuts }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: Sales/CustOutstanding
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

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        public ActionResult Create()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["SelectListCustCode"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListCurrency2"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.ExchangeRate());
        }

        public JsonResult RecalcOutstanding(string period)
        {
            

            string strResult = "";
            try
            {
                IDS.Sales.OutstandingRecalculate.NewSPBasedCalculator.Recalculate(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE], null, null, period);
            }
            catch (Exception e)
            {
                strResult = e.Message;
            }
            return Json(strResult, JsonRequestBehavior.AllowGet);
        }
    }
}