using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class DepartmentBalanceController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string period, string branchCode, string dept, string ccy, string acc)
        {
            period = string.IsNullOrEmpty(period) ? DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') : Convert.ToDateTime(period).Year.ToString() + Convert.ToDateTime(period).Month.ToString().PadLeft(2, '0');

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

                List<IDS.GLTable.DepartmentBalance> deptBals = IDS.GLTable.DepartmentBalance.GetDepartmentBalance();

                deptBals = deptBals.Where(x => x.BranchDepartmentBalance.BranchCode.ToLower().Contains(Tool.GeneralHelper.NullToString(branchCode).ToLower())
                || x.Code.Contains(Tool.GeneralHelper.NullToString(dept)) || x.Period.Contains(Tool.GeneralHelper.NullToString(period)) || x.CurrencyDepartmentBalance.CurrencyCode.Contains(Tool.GeneralHelper.NullToString(ccy)) || x.COADepartmentBalance.Account.Contains(Tool.GeneralHelper.NullToString(acc))).ToList();

                //totalRecords = deptBals.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    deptBals = deptBals.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                deptBals = deptBals.Where(x => x.BranchDepartmentBalance.BranchCode.ToLower().Contains(branchCode.ToLower())).ToList();
                deptBals = deptBals.Where(x => x.Code.ToLower().Contains(dept.ToLower())).ToList();
                deptBals = deptBals.Where(x => x.Period.ToLower().Contains(period.ToLower())).ToList();
                deptBals = deptBals.Where(x => x.CurrencyDepartmentBalance.CurrencyCode.ToLower().Contains(ccy.ToLower())).ToList();

                //if (!string.IsNullOrWhiteSpace(acc))
                //{
                //    deptBals = deptBals.Where(x => x.COADepartmentBalance.Account.ToLower().Contains(acc.ToLower())).ToList();
                //}


                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLOwer = searchValue.ToLower();

                //    deptBals = deptBals.Where(x => x.Code.ToLower().Contains(searchValueLOwer) ||
                //                             x.COADepartmentBalance.Account.ToLower().Contains(searchValueLOwer) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLOwer) ||
                //                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLOwer)).ToList();
                //}

                //totalRecordsShowing = deptBals.Count();

                //// Paging
                //if (pageSize > 0)
                //    deptBals = deptBals.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = deptBals }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(deptBals), JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: GLTable/DepartmentBalance
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

            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Value");
            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListAcc"] = new SelectList(new List<SelectListItem>(), "Value", "Value");
            //ViewData["DefBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            //ViewData["SelectListYear"] = new SelectList(IDS.GLTable.DepartmentBalance.GetYearForDataSource(), "Value", "Text", DateTime.Now.Year.ToString());
            //ViewData["SelectListMonth"] = new SelectList(IDS.GLTable.DepartmentBalance.GetMonthForDataSource(), "Value", "Text", DateTime.Now.Month.ToString().PadLeft(2, '0'));

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

            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListAcc"] = new SelectList(new SelectList(new List<SelectListItem>(), "Value", "Text"));
            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Value");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.DepartmentBalance());
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
                    IDS.GLTable.DepartmentBalance deptBal = new IDS.GLTable.DepartmentBalance();

                    deptBal.Code = collection["Code"];

                    deptBal.BranchDepartmentBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchDepartmentBalance.BranchCode"]);

                    deptBal.COADepartmentBalance = new IDS.GLTable.ChartOfAccount();
                    deptBal.COADepartmentBalance.Account = collection["COADepartmentBalance.Account"];

                    deptBal.CurrencyDepartmentBalance = new IDS.GeneralTable.Currency();
                    deptBal.CurrencyDepartmentBalance.CurrencyCode = collection["CurrencyDepartmentBalance.CurrencyCode"];

                    deptBal.Period = string.IsNullOrEmpty(collection["Period"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(collection["Period"]).ToString("yyyyMM");

                    //if (string.IsNullOrEmpty(monthlyBalance.Period))
                    //    monthlyBalance.Period = DateTime.Now.ToString("yyyyMM");
                    //else
                    //{
                    //    DateTime datePeriod = Convert.ToDateTime(monthlyBalance.Period);
                    //    monthlyBalance.Period = datePeriod.ToString("yyyyMM");
                    //}

                    deptBal.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    deptBal.Debit = Convert.ToDecimal(collection["Debit"]);
                    deptBal.Credit = Convert.ToDecimal(collection["Credit"]);
                    deptBal.Budget = Convert.ToDecimal(collection["Budget"]);
                    deptBal.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    deptBal.OperatorID = currentUser;

                    deptBal.InsUpDelDepartmentBalance((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string branchCode, string deptCode, string period, string ccy, string acc)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            IDS.GLTable.DepartmentBalance deptBal = IDS.GLTable.DepartmentBalance.GetDepartmentBalance(period, branchCode,deptCode, acc, ccy);

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", branchCode);
            }

            //ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value",ccy);
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(ccy), "Value", "Text",acc);
            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Value",deptCode);

            ViewData["FormAction"] = 2;

            if (deptBal != null)
            {
                return PartialView("Create", deptBal);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.DepartmentBalance());
            }
        }

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
                    IDS.GLTable.DepartmentBalance deptBal = new IDS.GLTable.DepartmentBalance();

                    deptBal.Code = collection["Code"];

                    deptBal.BranchDepartmentBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchDepartmentBalance.BranchCode"]);

                    deptBal.COADepartmentBalance = new IDS.GLTable.ChartOfAccount();
                    deptBal.COADepartmentBalance.Account = collection["COADepartmentBalance.Account"];

                    deptBal.CurrencyDepartmentBalance = new IDS.GeneralTable.Currency();
                    deptBal.CurrencyDepartmentBalance.CurrencyCode = collection["CurrencyDepartmentBalance.CurrencyCode"];

                    deptBal.Period = string.IsNullOrEmpty(collection["Period"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(collection["Period"]).ToString("yyyyMM");
                    
                    deptBal.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    deptBal.Debit = Convert.ToDecimal(collection["Debit"]);
                    deptBal.Credit = Convert.ToDecimal(collection["Credit"]);
                    deptBal.Budget = Convert.ToDecimal(collection["Budget"]);
                    deptBal.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    deptBal.OperatorID = currentUser;

                    deptBal.InsUpDelDepartmentBalance((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string periodsList, string branchCode, string code, string coasList, string currenciesList)
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
                    IDS.GLTable.DepartmentBalance deptBal = new IDS.GLTable.DepartmentBalance();
                    deptBal.InsUpDelDepartmentBalance((int)IDS.Tool.PageActivity.Delete, periods, branchCode,code, coas, currencies);
                }

                return Json("Department Balance data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}