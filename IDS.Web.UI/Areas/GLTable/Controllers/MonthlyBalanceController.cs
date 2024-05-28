using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Globalization;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class MonthlyBalanceController : IDS.Web.UI.Controllers.MenuController
    {
        //public JsonResult GetData(string period = "200909", string branchCode = "BDG")
        //{
        //    if (string.IsNullOrEmpty(period))
        //        period = DateTime.Now.ToString("yyyyMM");
        //    else
        //    {
        //        DateTime datePeriod = Convert.ToDateTime(period);
        //        period = datePeriod.ToString("yyyyMM");
        //    }

        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        throw new Exception("You do not have access to access this page");

        //    JsonResult result = new JsonResult();

        //    try
        //    {
        //        var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //        var start = Request.Form.GetValues("start").FirstOrDefault();
        //        var length = Request.Form.GetValues("length").FirstOrDefault();
        //        var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //        var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //        var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int totalRecords = 0; // Total keseluruhan data
        //        int totalRecordsShowing = 0; // Total data setelah filter / search

        //        List<IDS.GLTable.MonthlyBalance> monthlyBalances = new List<IDS.GLTable.MonthlyBalance>();

        //        //if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
        //        //{
        //        //    monthlyBalances = IDS.GL.GLTable.MonthlyBalance.GetMonthlyBalance();
        //        //}
        //        //else
        //        //{
        //        //    monthlyBalances = IDS.GL.GLTable.MonthlyBalance.GetMonthlyBalance.GetDepartmentList(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
        //        //}

        //        monthlyBalances = IDS.GLTable.MonthlyBalance.GetMonthlyBalanceWithPeriodBranch(period, branchCode);

        //        totalRecords = monthlyBalances.Count;

        //        // Sorting    
        //        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
        //        {
        //            switch (sortColumn.ToLower())
        //            {
        //                default:
        //                    monthlyBalances = monthlyBalances.OrderBy(sortColumn + " " + sortColumnDir).ToList();
        //                    break;
        //            }
        //        }

        //        // Search    
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            string searchValueLower = searchValue.ToLower();

        //            monthlyBalances = monthlyBalances.Where(x => x.CurrencyMonthlyBalance.CurrencyCode.ToLower().Contains(searchValueLower) ||
        //                                     x.COAMonthlyBalance.AccountName.ToLower().Contains(searchValueLower) ||
        //                                     //x.BranchMonthlyBalance.BranchName.ToLower().Contains(searchValueLower) ||
        //                                     x.Budget.ToString().ToLower().Contains(searchValueLower) ||
        //                                     x.BegBal.ToString().ToLower().Contains(searchValueLower) ||
        //                                     x.Debit.ToString().ToLower().Contains(searchValueLower) ||
        //                                     x.Credit.ToString().ToLower().Contains(searchValueLower) ||
        //                                     x.EndBal.ToString().ToLower().Contains(searchValueLower) ||
        //                                     x.OperatorID.ToLower().Contains(searchValueLower) ||
        //                                     x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
        //        }

        //        totalRecordsShowing = monthlyBalances.Count();

        //        // Paging
        //        monthlyBalances = monthlyBalances.Skip(skip).Take(pageSize).ToList();

        //        // Returning Json Data
        //        result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = monthlyBalances }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return result;
        //}

        public JsonResult GetData(string period, string branchCode)
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
                //var draw = Request.Form.GetValues("draw").FirstOrDefault();
                //var start = Request.Form.GetValues("start").FirstOrDefault();
                //var length = Request.Form.GetValues("length").FirstOrDefault();
                //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                //var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //int pageSize = 0;
                //switch (length)
                //{
                //    case null:
                //        break;
                //    case "-1":
                //        pageSize = 0;
                //        break;
                //    default:
                //        pageSize = Convert.ToInt32(length);
                //        break;
                //}

                //int skip = start != null ? Convert.ToInt32(start) : 0;
                //int totalRecords = 0; // Total keseluruhan data
                //int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.GLTable.MonthlyBalance> monthlyBalances = IDS.GLTable.MonthlyBalance.GetMonthlyBalance(period,branchCode);

                //totalRecords = monthlyBalances.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    monthlyBalances = monthlyBalances.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    monthlyBalances = monthlyBalances.Where(x => x.COAMonthlyBalance.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                //                             x.COAMonthlyBalance.Account.ToLower().Contains(searchValueLower) ||
                //                             x.COAMonthlyBalance.AccountName.ToLower().Contains(searchValueLower) ||
                //                             x.Budget.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.BegBal.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Debit.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Credit.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.EndBal.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = monthlyBalances.Count();

                //// Paging
                //if (pageSize > 0)
                //    monthlyBalances = monthlyBalances.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = monthlyBalances }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(monthlyBalances), JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
            }
            catch
            {

            }

            return result;
        }

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

            //ViewData["DefBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            ViewData["SelectListYear"] = new SelectList(IDS.GLTable.MonthlyBalance.GetYearForDataSource(), "Value", "Text", DateTime.Now.Year.ToString());
            ViewData["SelectListMonth"] = new SelectList(IDS.GLTable.MonthlyBalance.GetMonthForDataSource(), "Value", "Text", DateTime.Now.Month.ToString().PadLeft(2, '0'));

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

            ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["SelectListCOA"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Value");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.MonthlyBalance());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, FormCollection collection)
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

            ModelState.Clear();

            ValidateModel(collection);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    IDS.GLTable.MonthlyBalance monthlyBalance = new IDS.GLTable.MonthlyBalance();

                    monthlyBalance.BranchMonthlyBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchMonthlyBalance.BranchCode"]);

                    monthlyBalance.COAMonthlyBalance = new IDS.GLTable.ChartOfAccount();
                    monthlyBalance.COAMonthlyBalance.Account = collection["COAMonthlyBalance.Account"];

                    monthlyBalance.CurrencyMonthlyBalance = new IDS.GeneralTable.Currency();
                    monthlyBalance.CurrencyMonthlyBalance.CurrencyCode = collection["CurrencyMonthlyBalance.CurrencyCode"];

                    monthlyBalance.Period = collection["Period"];
                    if (string.IsNullOrEmpty(monthlyBalance.Period))
                        monthlyBalance.Period = DateTime.Now.ToString("yyyyMM");
                    else
                    {
                        DateTime datePeriod = Convert.ToDateTime(monthlyBalance.Period);
                        monthlyBalance.Period = datePeriod.ToString("yyyyMM");
                    }

                    monthlyBalance.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    monthlyBalance.Debit = Convert.ToDecimal(collection["Debit"]);
                    monthlyBalance.Credit = Convert.ToDecimal(collection["Credit"]);
                    monthlyBalance.Budget = Convert.ToDecimal(collection["Budget"]);
                    monthlyBalance.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    monthlyBalance.EntryUser = monthlyBalance.OperatorID = currentUser;

                    monthlyBalance.InsUpDelMonthlyBalance((int)IDS.Tool.PageActivity.Insert);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string coa, string currency, string period, string branchCode)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            IDS.GLTable.MonthlyBalance monthlyBalance = IDS.GLTable.MonthlyBalance.GetMonthlyBalance(period, branchCode, coa, currency);

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", branchCode);
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", branchCode);
            }

            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", monthlyBalance.CurrencyMonthlyBalance.CurrencyCode);
            //ViewData["SelectListCOA"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithAccountGroup(monthlyBalance.CurrencyMonthlyBalance.CurrencyCode), "Value", "Text", monthlyBalance.COAMonthlyBalance.Account);
            ViewData["SelectListCOA"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(monthlyBalance.CurrencyMonthlyBalance.CurrencyCode), "Value", "Text", monthlyBalance.COAMonthlyBalance.Account);

            ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["FormAction"] = 2;

            if (monthlyBalance != null)
            {
                return PartialView("Create", monthlyBalance);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.MonthlyBalance());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ModelState.Clear();

            ValidateModel(collection);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    IDS.GLTable.MonthlyBalance monthlyBalance = new IDS.GLTable.MonthlyBalance();

                    monthlyBalance.BranchMonthlyBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchMonthlyBalance.BranchCode"]);

                    monthlyBalance.COAMonthlyBalance = new IDS.GLTable.ChartOfAccount();
                    monthlyBalance.COAMonthlyBalance.Account = collection["COAMonthlyBalance.Account"];

                    monthlyBalance.CurrencyMonthlyBalance = new IDS.GeneralTable.Currency();
                    monthlyBalance.CurrencyMonthlyBalance.CurrencyCode = collection["CurrencyMonthlyBalance.CurrencyCode"];

                    monthlyBalance.Period = collection["Period"];
                    if (string.IsNullOrEmpty(monthlyBalance.Period))
                        monthlyBalance.Period = DateTime.Now.ToString("yyyyMM");
                    else
                    {
                        DateTime datePeriod = Convert.ToDateTime(monthlyBalance.Period);
                        monthlyBalance.Period = datePeriod.ToString("yyyyMM");
                    }

                    monthlyBalance.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    monthlyBalance.Debit = Convert.ToDecimal(collection["Debit"]);
                    monthlyBalance.Credit = Convert.ToDecimal(collection["Credit"]);
                    monthlyBalance.Budget = Convert.ToDecimal(collection["Budget"]);
                    monthlyBalance.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    monthlyBalance.EntryUser = monthlyBalance.OperatorID = currentUser;

                    monthlyBalance.InsUpDelMonthlyBalance((int)IDS.Tool.PageActivity.Edit);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(string periodsList, string branchCode, string coasList, string currenciesList)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.DeleteAccess == 0)
            {
                return Json("NoAccess", JsonRequestBehavior.AllowGet);
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            if (string.IsNullOrWhiteSpace(periodsList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] periods = periodsList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] coas = coasList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] currencies = currenciesList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (periods.Length > 0)
                {
                    IDS.GLTable.MonthlyBalance monthlyBalance = new IDS.GLTable.MonthlyBalance();
                    monthlyBalance.InsUpDelMonthlyBalance((int)IDS.Tool.PageActivity.Delete, periods, branchCode, coas, currencies);
                }

                return Json("Monthly Balance data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ShowTree()
        {
            IDS.GeneralTable.Branch branch = new IDS.GeneralTable.Branch();

            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            //if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            //{
            //    RedirectToAction("Index", "Main", new { Area = "" });
            //}

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            List<IDS.GLTable.MonthlyBalance> monthlyBalances = IDS.GLTable.MonthlyBalance.GetMonthlyBalance("202205", "DTN");
            //var rootNodes = monthlyBalances.TreeNodes.Where(o => o.Parent == null);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table class=\"tree table table-bordered table-hover w-100\">")
                .Append("<tbody>");
            foreach (var item in monthlyBalances)
            {
                if (item.COAMonthlyBalance.Account == "1000000000")
                {
                    sb.Append("<tr class=\"treegrid-1 expanded\"><td>" + item.COAMonthlyBalance.Account + "</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-2 treegrid-parent-1\"><td>Node 1-1</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-3 treegrid-parent-1\"><td>Node 1-2</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-4 treegrid-parent-3\"><td>Node 1-2-1</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-5 treegrid-parent-3\"><td>Node 1-2-2</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-6\"><td>Root node</td><td>Additional info</td></tr>")
                    .Append("<tr class=\"treegrid-7 treegrid-parent-6\"><td>Node 2-1</td><td>Additional info</td></tr>");
                }
                
            }
            //sb.Append("<tr class=\"treegrid-1 expanded\"><td>" + "10000000" + "</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-2 treegrid-parent-1\"><td>Node 1-1</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-3 treegrid-parent-1\"><td>Node 1-2</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-4 treegrid-parent-3\"><td>Node 1-2-1</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-5 treegrid-parent-3\"><td>Node 1-2-2</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-6\"><td>Root node</td><td>Additional info</td></tr>")
            //        .Append("<tr class=\"treegrid-7 treegrid-parent-6\"><td>Node 2-1</td><td>Additional info</td></tr>");

            sb.Append("</tbody>")
            .Append("</table>");

            ViewData["TreeGridTemplate"] = sb.ToString();

            return View();
        }
    }
}