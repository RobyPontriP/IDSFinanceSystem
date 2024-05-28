using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class MonthlyBalanceBudgetController : IDS.Web.UI.Controllers.MenuController
    {
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

                List<IDS.GLTable.MonthlyBalanceBudget> monthlyBalancesBudget = new List<IDS.GLTable.MonthlyBalanceBudget>();

                //if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
                //{
                //    monthlyBalances = IDS.GL.GLTable.MonthlyBalance.GetMonthlyBalance();
                //}
                //else
                //{
                //    monthlyBalances = IDS.GL.GLTable.MonthlyBalance.GetMonthlyBalance.GetDepartmentList(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
                //}

                monthlyBalancesBudget = IDS.GLTable.MonthlyBalanceBudget.GetMonthlyBalanceBudget(period, branchCode);

                //totalRecords = monthlyBalancesBudget.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        default:
                //            monthlyBalancesBudget = monthlyBalancesBudget.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    monthlyBalancesBudget = monthlyBalancesBudget.Where(x => x.CurrencyMonthlyBalanceBudget.CurrencyCode.ToLower().Contains(searchValueLower) ||
                //                             x.COAMonthlyBalanceBudget.AccountName.ToLower().Contains(searchValueLower) ||
                //                             //x.BranchMonthlyBalanceBudget.BranchName.ToLower().Contains(searchValueLower) ||
                //                             x.Budget.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.BegBal.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.EndBal.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = monthlyBalancesBudget.Count();

                //// Paging
                //if (pageSize > 0)
                //    monthlyBalancesBudget = monthlyBalancesBudget.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = monthlyBalancesBudget }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(monthlyBalancesBudget), JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
            }
            catch (Exception e)
            {

            }

            return result;
        }
        // GET: GLTable/MonthlyBalanceBudget
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

            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListCOA"] = new SelectList(new SelectList(new List<SelectListItem>(), "Value", "Text"));

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.MonthlyBalanceBudget());
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
                    IDS.GLTable.MonthlyBalanceBudget monthlyBalanceBudget = new IDS.GLTable.MonthlyBalanceBudget();

                    monthlyBalanceBudget.BranchMonthlyBalanceBudget = IDS.GeneralTable.Branch.GetBranch(collection["BranchMonthlyBalanceBudget.BranchCode"]);

                    monthlyBalanceBudget.COAMonthlyBalanceBudget = new IDS.GLTable.ChartOfAccount();
                    monthlyBalanceBudget.COAMonthlyBalanceBudget.Account = collection["COAMonthlyBalanceBudget.Account"];

                    monthlyBalanceBudget.CurrencyMonthlyBalanceBudget = new IDS.GeneralTable.Currency();
                    monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode = collection["CurrencyMonthlyBalanceBudget.CurrencyCode"];

                    monthlyBalanceBudget.Period = collection["Period"];
                    if (string.IsNullOrEmpty(monthlyBalanceBudget.Period))
                        monthlyBalanceBudget.Period = DateTime.Now.ToString("yyyyMM");
                    else
                    {
                        DateTime datePeriod = Convert.ToDateTime(monthlyBalanceBudget.Period);
                        monthlyBalanceBudget.Period = datePeriod.ToString("yyyyMM");
                    }

                    monthlyBalanceBudget.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    monthlyBalanceBudget.Budget = Convert.ToDecimal(collection["Budget"]);
                    monthlyBalanceBudget.EntryUser = monthlyBalanceBudget.OperatorID = currentUser;

                    monthlyBalanceBudget.InsUpDelMonthlyBalanceBudget((int)IDS.Tool.PageActivity.Insert);

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

            IDS.GLTable.MonthlyBalanceBudget monthlyBalanceBudget = IDS.GLTable.MonthlyBalanceBudget.GetMonthlyBalanceBudget(period, branchCode, coa, currency);

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

            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode);
            //ViewData["SelectListCOA"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithAccountGroup(monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode), "Value", "Text", monthlyBalanceBudget.COAMonthlyBalanceBudget.Account);
            ViewData["SelectListCOA"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode), "Value", "Text", monthlyBalanceBudget.COAMonthlyBalanceBudget.Account);

            ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["FormAction"] = 2;

            if (monthlyBalanceBudget != null)
            {
                return PartialView("Create", monthlyBalanceBudget);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.MonthlyBalanceBudget());
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
                    IDS.GLTable.MonthlyBalanceBudget monthlyBalanceBudget = new IDS.GLTable.MonthlyBalanceBudget();

                    monthlyBalanceBudget.BranchMonthlyBalanceBudget = IDS.GeneralTable.Branch.GetBranch(collection["BranchMonthlyBalanceBudget.BranchCode"]);

                    monthlyBalanceBudget.COAMonthlyBalanceBudget = new IDS.GLTable.ChartOfAccount();
                    monthlyBalanceBudget.COAMonthlyBalanceBudget.Account = collection["COAMonthlyBalanceBudget.Account"];

                    monthlyBalanceBudget.CurrencyMonthlyBalanceBudget = new IDS.GeneralTable.Currency();
                    monthlyBalanceBudget.CurrencyMonthlyBalanceBudget.CurrencyCode = collection["CurrencyMonthlyBalanceBudget.CurrencyCode"];

                    monthlyBalanceBudget.Period = collection["Period"];
                    if (string.IsNullOrEmpty(monthlyBalanceBudget.Period))
                        monthlyBalanceBudget.Period = DateTime.Now.ToString("yyyyMM");
                    else
                    {
                        DateTime datePeriod = Convert.ToDateTime(monthlyBalanceBudget.Period);
                        monthlyBalanceBudget.Period = datePeriod.ToString("yyyyMM");
                    }

                    monthlyBalanceBudget.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    monthlyBalanceBudget.Budget = Convert.ToDecimal(collection["Budget"]);
                    monthlyBalanceBudget.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    monthlyBalanceBudget.EntryUser = monthlyBalanceBudget.OperatorID = currentUser;

                    monthlyBalanceBudget.InsUpDelMonthlyBalanceBudget((int)IDS.Tool.PageActivity.Edit);

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
                    IDS.GLTable.MonthlyBalance monthlyBalanceBudget = new IDS.GLTable.MonthlyBalance();
                    monthlyBalanceBudget.InsUpDelMonthlyBalance((int)IDS.Tool.PageActivity.Delete, periods, branchCode, coas, currencies);
                }

                return Json("Monthly Balance Budget data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}