using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class CashBasisAccountController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string setParent)
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

                ////int pageSize = (length != null ? Convert.ToInt32(length) : 0);
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

                var coa = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForGrid(setParent)
                    .Select(x => new
                    {
                        Account = x.Account,
                        AccountName = x.AccountName,
                        AccountGroup = x.AccountGroup.ToString(),
                        AccountTotalDetail = (x.AccountTotalDetail == true ? "Total" : "Detail"),
                        CCy = x.CCy.CurrencyCode,
                        Level = x.Level,
                        OperatorID = x.OperatorID,
                        LastUpdate = x.LastUpdate
                    }).ToList();

                //totalRecords = coa.Count;

                //// Sorting
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    coa = coa.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    coa = coa.Where(x => x.Account.ToLower().Contains(searchValueLower) ||
                //                             x.AccountName.ToLower().Contains(searchValueLower) ||
                //                             x.CCy.ToLower().Contains(searchValueLower) ||
                //                             x.AccountTotalDetail.ToLower().Contains(searchValueLower) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = coa.Count();

                //// Paging
                //if (pageSize > 0)
                //    coa = coa.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = coa }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(coa), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// Untk View Chart Of Account Tree
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDataForTree()
        {
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

                //int pageSize = (length != null ? Convert.ToInt32(length) : 0);
                int pageSize = 0;

                switch (length)
                {
                    case null:
                        break;
                    case "-1":
                        pageSize = 0;
                        break;
                    default:
                        pageSize = Convert.ToInt32(length);
                        break;
                }



                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search


                IDS.GLTable.COAView view = new IDS.GLTable.COAView();
                List<IDS.GLTable.COAView> coa = view.GetCOAViewForGrid();

                //List<IDS.GL.CashBasisAccount> coa = IDS.GL.CashBasisAccount.GetCOAForGrid();

                totalRecords = coa.Count;

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    coa = coa.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    coa = coa.Where(x => x.Account.ToLower().Contains(searchValue) ||
                                             x.AccountName.ToLower().Contains(searchValue) ||
                                             x.CCy.CurrencyName.ToLower().Contains(searchValue) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = coa.Count();

                // Paging
                if (pageSize > 0)
                    coa = coa.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = coa }, JsonRequestBehavior.AllowGet);
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = coa }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
            }

            return result;
        }

        // GET: GLTable/CashBasisAccount
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                return RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            //ViewData["AccNode"] = IDS.GLTable.CashBasisAccount.TreeNode.GetChartAccountAllNodeRawHtml();

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


            ViewData["AccGroupList"] = Tool.EnumExtensions.ToSelectList<Tool.GLAccountGroup>(Tool.GLAccountGroup.Asset);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.CashBasisAccount());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GLTable.CashBasisAccount coa)
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

            ValidateModel(coa);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    coa.OperatorID = currentUser;

                    coa.InsUpDel((int)IDS.Tool.PageActivity.Insert);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string accNo, string ccy)
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

            ViewData["FormAction"] = 2;

            IDS.GLTable.CashBasisAccount coa = IDS.GLTable.CashBasisAccount.GetCashBasisAccount(ccy, accNo);

            ViewData["AccGroupList"] = Tool.EnumExtensions.ToSelectList<Tool.GLAccountGroup>(Tool.GLAccountGroup.Asset);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");


            if (coa != null)
            {
                return PartialView("Create", coa);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.CashBasisAccount());
            }
        }

        // POST: GLTable/CashBasisAccount/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GLTable.CashBasisAccount COA)
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

            ValidateModel(COA);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    COA.OperatorID = currentUser;

                    COA.InsUpDel((int)IDS.Tool.PageActivity.Edit);

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

        // POST: GLTable/CashBasisAccount/Delete/5
        public ActionResult Delete(string datas)
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

            if (datas == null || datas.Length == 0)
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] COACode = datas.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (COACode.Length > 0)
                {
                    IDS.GLTable.CashBasisAccount coa = new IDS.GLTable.CashBasisAccount();
                    coa.OperatorID = Tool.GeneralHelper.NullToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_ID]);
                    coa.InsUpDel((int)(int)IDS.Tool.PageActivity.Delete, COACode);
                }

                return Json("Cash Basis data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCOAFromCCY(string currencyCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(currencyCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TransByAccount(string branchCode, string period, string account, string ccy)
        {
            ViewData["BranchList"] = IDS.GeneralTable.Branch.GetBranchForDatasource();
            ViewData["selBranch"] = branchCode;
            ViewData["selPeriod"] = period;
            ViewData["selAccount"] = account;
            ViewData["selAccountName"] = "";
            ViewData["selCCy"] = ccy;

            if (!string.IsNullOrWhiteSpace(account))
            {
                IDS.GLTable.CashBasisAccount coa = IDS.GLTable.CashBasisAccount.GetCashBasisAccount(account, ccy);

                if (coa != null)
                    ViewData["selAccountName"] = Tool.GeneralHelper.NullToString(coa.AccountName);
            }

            return View("TransByAccount");
        }

        public JsonResult GetTransByAccount(string branchCode, string period, string account, string ccy)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
            {
                //throw new Exception("You do not have access to access this page");
                Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                Response.SuppressFormsAuthenticationRedirect = true;
                return Json("STO", JsonRequestBehavior.AllowGet); // Session Timeout
            }

            JsonResult result = new JsonResult();

            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //int pageSize = (length != null ? Convert.ToInt32(length) : 0);
                int pageSize = 0;

                switch (length)
                {
                    case null:
                        break;
                    case "-1":
                        pageSize = 0;
                        break;
                    default:
                        pageSize = Convert.ToInt32(length);
                        break;
                }



                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.GLTransaction.GLVoucherH> v = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetailByAccountAndCurrency(period, branchCode, account, ccy);

                var voucher = v.SelectMany(x => x.Detail.Select(y => new
                {
                    SCode = Tool.GeneralHelper.NullToString(x.SCode.Code),
                    BranchCode = Tool.GeneralHelper.NullToString(x.VBranch.BranchCode),
                    Voucher = Tool.GeneralHelper.NullToString(x.Voucher),
                    x.TransDate,
                    Account = Tool.GeneralHelper.NullToString(y.COA.Account),
                    Currency = Tool.GeneralHelper.NullToString(y.CCy.CurrencyCode),
                    Debet = (y.Amount >= 0 ? y.Amount : 0),
                    Credit = (y.Amount < 0 ? y.Amount : 0),
                    y.Descrip
                }));

                totalRecords = voucher.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    voucher = voucher.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    voucher = voucher.Where(x => x.Voucher.ToLower().Contains(searchValue) ||
                                             x.BranchCode.ToLower().Contains(searchValue) ||
                                             x.Currency.ToLower().Contains(searchValue) ||
                                             x.SCode.ToLower().Contains(searchValue) ||
                                             x.Voucher.ToLower().Contains(searchValueLower) ||
                                             x.Account.ToLower().Contains(searchValueLower) ||
                                             x.Debet.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Credit.ToString().ToLower().Contains(searchValueLower) ||
                                             x.TransDate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = voucher.Count();

                // Paging
                if (pageSize > 0)
                    voucher = voucher.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = voucher }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public ActionResult ViewTransByAccount(string voucher, string sCode, string branchCode, string ccy)
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

            ViewData["FormAction"] = 2;

            IDS.GLTransaction.GLVoucherH transH = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetail(voucher, sCode, branchCode);

            ViewData["AccGroupList"] = Tool.EnumExtensions.ToSelectList<Tool.GLAccountGroup>(Tool.GLAccountGroup.Asset);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");


            if (transH != null)
            {
                return PartialView("ViewVoucherPopUp", transH);
            }
            else
            {
                return PartialView("ViewVoucherPopUp", new IDS.GLTransaction.GLVoucherH());
            }
        }

        public JsonResult GetCOAList(string currency, string prefix)
        {
            List<SelectListItem> list = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(currency);
            list = list.Where(x => x.Text.ToLower().StartsWith(prefix.ToLower())).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCashBasisAccountByCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                return Json(new List<SelectListItem>());

            try
            {
                IList<SelectListItem> list = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(currency);
                return Json(list);
            }
            catch
            {
                throw;
            }
            
        }
    }
}
