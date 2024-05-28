using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class GLStandingOrderController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string branchCode, DateTime? dateFrom, DateTime? dateTo, int dateType)
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

                List<IDS.GLTransaction.GLStandingOrderH> SO = IDS.GLTransaction.GLStandingOrderH.GetStandingorderHeaderForGrid(branchCode, dateFrom, dateTo, dateType);

                totalRecords = SO.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    SO = SO.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    SO = SO.Where(x => x.Code.ToLower().Contains(searchValue) ||
                                             Tool.GeneralHelper.NullToString(x.VBranch?.BranchCode).ToLower().Contains(searchValue) ||
                                             x.StartDate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToString().ToLower().Contains(searchValue) ||
                                             x.ExpiryDate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToString().Contains(searchValue) ||
                                             x.Description.ToString().Contains(searchValue) ||
                                             x.ExecDate.ToString().Contains(searchValue) ||
                                             x.ExecCode.ToString().ToLower().Contains(searchValue) ||
                                             Tool.GeneralHelper.NullToString(x.SCode?.Code).ToLower().Contains(searchValue) ||
                                             Tool.GeneralHelper.NullToBool(x.ActiveStatus).ToString().ToLower().Contains(searchValue) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = SO.Count();

                // Paging
                if (pageSize > 0)
                    SO = SO.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = SO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        // GET: GLTransaction/GLStandingOrder
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

            ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));

            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

            return View();
        }

        // GET: GLTransaction/GLStandingOrder/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GLTransaction/GLStandingOrder/Create
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

            ViewData["FormAction"] = 1;

            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource().ToList(), "Value", "Text");

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            //ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text");
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<tr id=\"r' + index + '\">")
                //.Append("<td style=\"padding: 1px 2px !important; width: 65px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 65px !important;\">")
                .Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--CCy--</option>");

            foreach (SelectListItem item in ccyList)
            {
                if (item.Value == IDS.GeneralTable.Syspar.GetInstance().BaseCCy)
                    sb.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                else
                    sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");

            }

            sb.Append("</select></td>")
                //.Append("<td style=\"padding: 1px 2px; !important; width: 140px;\"><input type=\"text\" id=\"txtAcc-' + index + '\" name=\"COA.Account\" style=\"padding: 1px; border: none; max-width: 145px !important;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"txtAcc-' + index + '\" name=\"COA.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtDescription-' + index + '\" name=\"Descrip\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 56px;\"><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" style=\"padding: 3px; border: 0; max-width: 60px !important;\">")
                //.Append("<td style=\"padding: 1px 2px !important; width: 56px;\"><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" class=\"select2DDList\" style=\"padding: 3px; border: 0; max-width: 60px !important;\">")
                .Append("<td style=\"padding: 1px 2px !important; width: 56px;\" hidden><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" style=\"padding: 3px; border: 0; max-width: 60px !important;\" hidden>")
                .Append("<option value=\"\" placeholder=\"Dept\" title=\"Please select Department\"></option>");

            foreach (SelectListItem item in deptList)
            {
                sb.Append("<option value=\"" + item.Value + "\" title=\"" + item.Text + "\">" + item.Value + "</option>");
            }

            sb.Append("</select></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 88px;\"><select id=\"CashAcc-' + index + '\" name=\"CashAccount\" style=\"padding: 3px; border: 0; max-width: 80px !important; max-width: 80px !important;\" onchange=\"OnCashBasisAccountChange(this)\" title=\"\" disabled></select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 88px;\"><select id=\"CashAcc-' + index + '\" name=\"CashAccount\" style=\"padding: 3px; border: 0; max-width: 80px !important; max-width: 80px !important;\" class=\"select2DDList\" onchange=\"OnCashBasisAccountChange(this)\" title=\"\" disabled></select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtDebit-' + index + '\" name=\"Debit\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"debitValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtCredit-' + index + '\" name=\"Credit\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"creditValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();

            return View("Create", new IDS.GLTransaction.GLStandingOrderH() { ExecDate = 1, StartDate = DateTime.Today.Date, ExpiryDate = DateTime.Today.Date });
        }

        // POST: GLTransaction/GLStandingOrder/Create
        [HttpPost]
        public ActionResult Create(int? FormAction, string hfUrlReferrer, IDS.GLTransaction.GLStandingOrderH SO)
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

            ViewBag.UserMenu = MainMenu;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool valid = true;

            #region Header Validation
            if (string.IsNullOrWhiteSpace(SO.Code))
            {
                valid = false;
                sb.Append("- Standing Order code is required.");
            }

            if (SO.VBranch?.BranchCode == null || string.IsNullOrEmpty(SO.VBranch?.BranchCode))
            {
                valid = false;
                sb.Append("- Branch is required").Append(Environment.NewLine);
            }

            if (SO.StartDate == null || SO.StartDate == DateTime.MinValue)
            {
                valid = false;
                sb.Append("- Standing Order start date is required or start date is invalid.").Append(Environment.NewLine);
            }

            if (SO.ExpiryDate == null || SO.ExpiryDate == DateTime.MinValue)
            {
                valid = false;
                sb.Append("- Standing Order expiry date is required or expiry date is invalid.").Append(Environment.NewLine);
            }

            int tryExeDay = 0;

            if (string.IsNullOrWhiteSpace(SO.ExecDate.ToString()) || !int.TryParse(SO.ExecDate.ToString(), out tryExeDay) || Convert.ToInt16(SO.ExecDate) > 31 || Convert.ToInt16(SO.ExecDate) < 1)
            {
                valid = false;
                sb.Append("- Standing Order execution day is required or execution day is invalid.");
            }

            if (string.IsNullOrEmpty(SO.SCode?.Code))
            {
                valid = false;
                sb.Append("- Standing Order Source code is required").Append(Environment.NewLine);
            }
            #endregion

            #region Detail Validation
            if (SO.Detail == null || SO.Detail.Count == 0)
            {
                valid = false;
                sb.Append("Can not save voucher without Journal Item.");
            }
            else
            {
                double difference = SO.Detail.Sum(x => x.Amount);

                if (difference != 0)
                {
                    valid = false;
                    sb.Append("Journal is not balance");
                }

                for (int i = 0; i < SO.Detail.Count; i++)
                {
                    if (SO.Detail[i].CCy?.CurrencyCode == null || string.IsNullOrEmpty(SO.Detail[0].CCy?.CurrencyCode))
                    {
                        valid = false;
                        sb.Append("- One of journal item currency is not set or empty.").Append(Environment.NewLine);
                    }

                    if (SO.Detail[i].COA?.Account == null || string.IsNullOrEmpty(SO.Detail[0].COA.Account))
                    {
                        valid = false;
                        sb.Append("- One of journal item Chart of Account is not set or empty.").Append(Environment.NewLine);
                    }
                    else
                    {
                        if (SO.Detail[i].CCy?.CurrencyCode != null && !string.IsNullOrEmpty(SO.Detail[i].CCy?.CurrencyCode) &&
                            SO.Detail[i].COA?.Account != null && !string.IsNullOrEmpty(SO.Detail[i].COA?.Account))
                        {
                            bool isCashAccount = IDS.GLTable.ChartOfAccount.IsCashAccount(SO.Detail[i].CCy.CurrencyCode, SO.Detail[i].COA.Account);

                            if (isCashAccount && string.IsNullOrEmpty(SO.Detail[i].CashAccount?.Account))
                            {
                                valid = false;
                                sb.Append("- One of journal item cash account is not set while chart of account is cash account").Append(Environment.NewLine);
                            }
                            else if (!isCashAccount && !string.IsNullOrEmpty(SO.Detail[i].CashAccount?.Account))
                            {
                                valid = false;
                                sb.Append("- One of journal item cash account is set while chart of account is not cash account").Append(Environment.NewLine);
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(SO.Detail[i].Description))
                    {
                        valid = false;
                        sb.Append("- Journal item description is required.");
                    }

                    if (SO.Detail[i].Amount == 0)
                    {
                        valid = false;
                        sb.Append("- One of journal item amount is not set or zero").Append(Environment.NewLine);
                    }
                }
            }
            #endregion

            ModelState.Clear();

            string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
            }

            SO.OperatorID = currentUser;
            SO.LastUpdate = DateTime.Now;

            // TODO: Sesuaikan Account aslinya dari Autocomplete
            foreach (IDS.GLTransaction.GLStandingOrderD d in SO.Detail)
            {
                String[] separator = { " - " };
                string[] split = d.COA.Account.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                d.COA.Account = split[0];
            }

            ValidateModel(SO);

            if (ModelState.IsValid)
            {
                try
                {
                    int result = SO.InsUpDel((int)IDS.Tool.PageActivity.Insert);

                    if (result > 0)
                    {
                        return Json(new { msg = "New Standing Order has been save." }, JsonRequestBehavior.AllowGet);
                        //return Json("New Standing Order has been save", JsonRequestBehavior.DenyGet);
                    }
                    else
                        throw new Exception("Error");
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GLTransaction/GLStandingOrder/Edit/5
        [HttpGet,
            AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string code, string branchCode)
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

            ViewData["FormAction"] = 2;

            IDS.GLTransaction.GLStandingOrderH header = IDS.GLTransaction.GLStandingOrderH.GetStandingOrderWithDetail(branchCode, code);
            header.ExecCode = header.StartDate.AddMonths(IDS.Tool.GeneralHelper.NullToInt((header.ExecCode), 0)).ToString("MMM yyyy");

            if (header == null || header?.Code == null)
            {
                return Json("Can not find Standing Order number", JsonRequestBehavior.AllowGet);
            }

            string[] ccyInDetail = null;

            if (header.Detail != null && header.Detail.Count > 0)
            {
                ccyInDetail = header.Detail.Select(x => new { CCy = Tool.GeneralHelper.NullToString(x.CCy.CurrencyCode) })
                    .Select(x => x.CCy).Distinct().ToArray();
            }

            if (ccyInDetail != null && ccyInDetail.Length > 0)
            {
                for (int i = 0; i < ccyInDetail.Length; i++)
                {
                    List<SelectListItem> ccy = new List<SelectListItem>();
                    ccy = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(ccyInDetail[i]);
                    ccy.Insert(0, new SelectListItem() { Text = "", Value = "" });
                    ViewData[ccyInDetail[i]] = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(ccyInDetail[i]);
                }
            }

            if (header == null)
            {
                throw new Exception("Can not find voucher.");
            }

            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource()
                .Where(x => x.Value != "AR" && x.Value != "AP")
                .ToList(), "Value", "Text", header.SCode.Code);

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(branchCode));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            //ViewData["CurrDate"] = vh.TransDate;
            ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            //SelectListItem emptyCCy = new SelectListItem() { Text = "", Value = "--CCy--" };
            //ccyList.Insert(0, emptyCCy);
            ////ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text");
            ViewData["CCyList"] = ccyList;

            List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();
            deptList.Insert(0, new SelectListItem() { Text = "--Dept--", Value = "" });
            ViewData["deptList"] = deptList;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<tr id=\"r' + index + '\">")
                //.Append("<td style=\"padding: 1px 2px !important; width: 65px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 65px !important;\">");
                .Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\">");
            //.Append("<option value=\"\">--CCy--</option>");

            string defaultCCy = IDS.Tool.GeneralHelper.NullToString(IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            //foreach (SelectListItem item in ccyList)
            //{
            //    switch (item.Value.ToUpper())
            //    {
            //        case "IDR":
            //            sb.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
            //            break;
            //        default:
            //            sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            //            break;
            //    }
            //}
            foreach (SelectListItem item in ccyList)
            {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            sb.Append("</select></td>")
                //.Append("<td style=\"padding: 1px 2px; !important; width: 140px;\"><input type=\"text\" id=\"txtAcc-' + index + '\" name=\"COA.Account\" style=\"padding: 1px; border: none; max-width: 145px !important;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"txtAcc-' + index + '\" name=\"COA.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtDescription-' + index + '\" name=\"Descrip\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 56px;\"><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" style=\"padding: 3px; border: 0; max-width: 60px !important;\">");
                //.Append("<td style=\"padding: 1px 2px !important; width: 56px;\"><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" class=\"select2DDList\" style=\"padding: 3px; border: 0; max-width: 60px !important;\">");
                .Append("<td style=\"padding: 1px 2px !important; width: 56px;\" hidden><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" style=\"padding: 3px; border: 0; max-width: 60px !important;\" hidden>");
            //.Append("<option value=\"\" placeholder=\"Dept\" title=\"Please select Department\"></option>");

            foreach (SelectListItem item in deptList)
            {
                sb.Append("<option value=\"" + item.Value + "\" title=\"" + item.Text + "\">" + item.Value + "</option>");
            }

            sb.Append("</select></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 88px;\"><select id=\"CashAcc-' + index + '\" name=\"CashAccount\" style=\"padding: 3px; border: 0; max-width: 80px !important; max-width: 80px !important;\" onchange=\"OnCashBasisAccountChange(this)\" title=\"\" disabled></select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 88px;\"><select id=\"CashAcc-' + index + '\" name=\"CashAccount\" style=\"padding: 3px; border: 0; max-width: 80px !important; max-width: 80px !important;\" class=\"select2DDList\" onchange=\"OnCashBasisAccountChange(this)\" title=\"\" disabled></select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtDebit-' + index + '\" name=\"Debit\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"debitValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtCredit-' + index + '\" name=\"Credit\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"creditValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();

            return View("Edit", header);
        }

        // POST: GLTransaction/GLStandingOrder/Edit/5
        [HttpPost]
        public ActionResult Edit(int? FormAction, string hfUrlReferrer, IDS.GLTransaction.GLStandingOrderH so)
        {
            try
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

                ViewBag.UserMenu = MainMenu;

                if (so == null)
                {
                    return Json("Error: Invalid data. Please check or complete your data then retry. If problem persist, please contact your administrator.", JsonRequestBehavior.DenyGet);
                }

                #region Validation
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                bool valid = true;

                if (so.SCode?.Code == null || string.IsNullOrEmpty(so.SCode?.Code))
                {
                    valid = false;
                    sb.Append("- Source code is required").Append(Environment.NewLine);
                }

                if (so.VBranch?.BranchCode == null || string.IsNullOrEmpty(so.VBranch?.BranchCode))
                {
                    valid = false;
                    sb.Append("- Branch is required").Append(Environment.NewLine);
                }

                if (so.StartDate == null || so.StartDate == DateTime.MinValue)
                {
                    valid = false;
                    sb.Append("- Start date is required or start date is invalid.").Append(Environment.NewLine);
                }

                if (so.ExpiryDate == null || so.ExpiryDate == DateTime.MinValue)
                {
                    valid = false;
                    sb.Append("- Expiry date is required or expiry date is invalid.").Append(Environment.NewLine);
                }

                if (so.Detail == null || so.Detail.Count == 0)
                {
                    valid = false;
                    sb.Append("Can not save standing order without Journal Item.");
                }
                else
                {
                    double difference = so.Detail.Sum(x => x.Amount);

                    if (difference != 0)
                    {
                        valid = false;
                        sb.Append("Standing order journal entry is not balance");
                    }

                    for (int i = 0; i < so.Detail.Count; i++)
                    {
                        if (so.Detail[i].CCy?.CurrencyCode == null || string.IsNullOrEmpty(so.Detail[0].CCy?.CurrencyCode))
                        {
                            valid = false;
                            sb.Append("- One of journal item currency is not set or empty.").Append(Environment.NewLine);
                        }

                        if (so.Detail[i].COA?.Account == null || string.IsNullOrEmpty(so.Detail[0].COA.Account))
                        {
                            valid = false;
                            sb.Append("- One of journal item Chart of Account is not set or empty.").Append(Environment.NewLine);
                        }
                        else
                        {
                            if (so.Detail[i].CCy?.CurrencyCode != null && !string.IsNullOrEmpty(so.Detail[i].CCy?.CurrencyCode) &&
                                so.Detail[i].COA?.Account != null && !string.IsNullOrEmpty(so.Detail[i].COA?.Account))
                            {
                                bool isCashAccount = IDS.GLTable.ChartOfAccount.IsCashAccount(so.Detail[i].CCy.CurrencyCode, so.Detail[i].COA.Account);

                                if (isCashAccount && string.IsNullOrEmpty(so.Detail[i].CashAccount?.Account ?? null))
                                {
                                    valid = false;
                                    sb.Append("- One of journal item cash account is not set while chart of account is cash account").Append(Environment.NewLine);
                                }
                                else if (!isCashAccount && !string.IsNullOrEmpty(so.Detail[i].CashAccount?.Account ?? null))
                                {
                                    valid = false;
                                    sb.Append("- One of journal item cash account is set while chart of account is not cash account").Append(Environment.NewLine);
                                }
                            }
                        }

                        if (so.Detail[i].Amount == 0)
                        {
                            valid = false;
                            sb.Append("- One of journal item amount is not set or zero").Append(Environment.NewLine);
                        }
                    }
                }
                #endregion

                ModelState.Clear();

                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                so.OperatorID = currentUser;
                so.LastUpdate = DateTime.Now;

                // TODO: Sesuaikan Account aslinya dari Autocomplete
                foreach (IDS.GLTransaction.GLStandingOrderD d in so.Detail)
                {
                    String[] separator = { " - " };
                    string[] split = d.COA.Account.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    d.COA.Account = split[0];
                }

                ValidateModel(so);

                if (ModelState.IsValid)
                {
                    try
                    {
                        string newVoucherNo = "";
                        int result = so.InsUpDel((int)IDS.Tool.PageActivity.Edit);

                        if (result > 0)
                        {
                            if (string.IsNullOrWhiteSpace(newVoucherNo))
                                return Json("Standing order has been update", JsonRequestBehavior.DenyGet);
                            else
                                return Json("Standing order has been update.", JsonRequestBehavior.DenyGet);
                        }
                        else
                            throw new Exception("Error");
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.Message, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: GLTransaction/GLStandingOrder/Delete/5
        public ActionResult Delete(string voucherCollection)
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

            if (string.IsNullOrWhiteSpace(voucherCollection))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            // Check voucher Posted Status


            try
            {
                string[] eachItem = voucherCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (eachItem.Length > 0)
                {
                    IDS.GLTransaction.GLStandingOrderH soh = new IDS.GLTransaction.GLStandingOrderH();

                    // Terdapat voucher posted
                    //if (voucher.CheckPostedVoucherExists(eachItem))
                    //{
                    //    return Json("One or more selected voucher are posted and can not be delete. Delete process canceled by system.");
                    //}

                    soh.InsUpDel((int)IDS.Tool.PageActivity.Delete, eachItem);
                }

                return Json("Standing Order has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
