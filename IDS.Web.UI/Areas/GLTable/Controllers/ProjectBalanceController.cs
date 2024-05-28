using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class ProjectBalanceController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string branchCode, string proj, string ccy, string acc)
        {
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

                List<IDS.GLTable.ProjectBalance> projBals = IDS.GLTable.ProjectBalance.GetProjectBalance(branchCode);

                //totalRecords = projBals.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    projBals = projBals.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                projBals = projBals.Where(x => x.BranchProjectBalance.BranchCode.ToLower().Contains(branchCode.ToLower())).ToList();
                projBals = projBals.Where(x => x.Code.ToLower().Contains(proj.ToLower())).ToList();


                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLOwer = searchValue.ToLower();

                //    projBals = projBals.Where(x => x.Code.ToLower().Contains(searchValueLOwer) ||
                //                             x.COAProjectBalance.Account.ToLower().Contains(searchValueLOwer) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLOwer) ||
                //                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLOwer)).ToList();
                //}

                //totalRecordsShowing = projBals.Count();

                //// Paging
                //if (pageSize > 0)
                //    projBals = projBals.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(projBals), JsonRequestBehavior.AllowGet);
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = projBals }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: GLTable/ProjectBalance
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

            ViewData["SelectListProjCode"] = new SelectList(IDS.GLTable.Project.GetProjectForDataSourceWithBranch(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Value");

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
            ViewData["SelectListProj"] = new SelectList(new SelectList(new List<SelectListItem>(), "Value", "Value"));

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.ProjectBalance());
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
                    IDS.GLTable.ProjectBalance projBal = new IDS.GLTable.ProjectBalance();

                    projBal.Code = collection["Code"];

                    projBal.BranchProjectBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchProjectBalance.BranchCode"]);

                    projBal.COAProjectBalance = new IDS.GLTable.ChartOfAccount();
                    projBal.COAProjectBalance.Account = collection["COAProjectBalance.Account"];

                    projBal.CurrencyProjectBalance = new IDS.GeneralTable.Currency();
                    projBal.CurrencyProjectBalance.CurrencyCode = collection["CurrencyProjectBalance.CurrencyCode"];

                    projBal.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    projBal.Debit = Convert.ToDecimal(collection["Debit"]);
                    projBal.Credit = Convert.ToDecimal(collection["Credit"]);
                    projBal.Budget = Convert.ToDecimal(collection["Budget"]);
                    projBal.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    projBal.OperatorID = currentUser;

                    projBal.InsUpDelProjectBalance((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string branchCode, string projCode, string ccy, string acc)
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

            IDS.GLTable.ProjectBalance projBal = IDS.GLTable.ProjectBalance.GetProjectBalance(branchCode, projCode, acc, ccy);

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", branchCode);
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", branchCode);
            }

            //ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", ccy);
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(ccy), "Value", "Text", acc);
            ViewData["SelectListProj"] = new SelectList(IDS.GLTable.Project.GetProjectForDataSourceWithBranch(branchCode), "Value", "Value", projCode);

            ViewData["FormAction"] = 2;

            if (projBal != null)
            {
                return PartialView("Create", projBal);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.ProjectBalance());
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
                    IDS.GLTable.ProjectBalance projBal = new IDS.GLTable.ProjectBalance();

                    projBal.Code = collection["Code"];

                    projBal.BranchProjectBalance = IDS.GeneralTable.Branch.GetBranch(collection["BranchProjectBalance.BranchCode"]);

                    projBal.COAProjectBalance = new IDS.GLTable.ChartOfAccount();
                    projBal.COAProjectBalance.Account = collection["COAProjectBalance.Account"];

                    projBal.CurrencyProjectBalance = new IDS.GeneralTable.Currency();
                    projBal.CurrencyProjectBalance.CurrencyCode = collection["CurrencyProjectBalance.CurrencyCode"];

                    projBal.BegBal = Convert.ToDecimal(collection["BegBal"]);
                    projBal.Debit = Convert.ToDecimal(collection["Debit"]);
                    projBal.Credit = Convert.ToDecimal(collection["Credit"]);
                    projBal.Budget = Convert.ToDecimal(collection["Budget"]);
                    projBal.EndBal = Convert.ToDecimal(collection["EndBal"]);
                    projBal.OperatorID = currentUser;

                    projBal.InsUpDelProjectBalance((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string branchCode, string code, string coasList, string currenciesList)
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

            if (string.IsNullOrWhiteSpace(branchCode))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] coas = coasList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] currencies = currenciesList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (coas.Length > 0)
                {
                    IDS.GLTable.ProjectBalance projBal = new IDS.GLTable.ProjectBalance();
                    projBal.InsUpDelProjectBalance((int)IDS.Tool.PageActivity.Delete, branchCode, code, coas, currencies);
                }

                return Json("Project Balance data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}