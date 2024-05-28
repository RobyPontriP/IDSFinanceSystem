using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class VoucherController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string srcCode, string branchCode, DateTime? dateFrom, DateTime? dateTo, int dateType,string upd)
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

                List<IDS.GLTransaction.GLVoucherH> vou = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetail(branchCode, srcCode, dateFrom, dateTo, dateType,upd);
                var voucher = vou.SelectMany(x => x.Detail.Select(y => new
                {
                    Voucher = x.Voucher,
                    SCode = x.SCode.Code,
                    BranchCode = x.VBranch.BranchCode,
                    TransDate = x.TransDate,
                    Entry_Date = x.Entry_Date,
                    Description = x.Description,
                    PostedStatus = (x.Detail.Count > 0 ? Convert.ToBoolean(Convert.ToInt16(Tool.GeneralHelper.NullToString(y.UPD, "0000000000").Substring(0, 1)) == 1) : false),
                    RevVoucher = x.ReversedVoucher,
                    OperatorID = x.OperatorID,
                    LastUpdate = x.LastUpdate
                })).Distinct();

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

                    voucher = voucher.Where(x => x.SCode.ToLower().Contains(searchValue) ||
                                             x.Voucher.ToLower().Contains(searchValue) ||
                                             x.PostedStatus.ToString().ToLower().Contains(searchValue) ||
                                             x.Entry_Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT).Contains(searchValue) ||
                                             x.TransDate.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT).Contains(searchValue) ||
                                             x.RevVoucher.ToString().ToLower().Contains(searchValue) ||
                                             x.Description.ToLower().Contains(searchValue) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
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

        public JsonResult GetDataDetail(string branchCode, string srcCode, string voucher)
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

                //List<IDS.GLTransaction.GLVoucherH> vou = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetail(branchCode, srcCode, dateFrom, dateTo, dateType);
                List<IDS.GLTransaction.GLVoucherD> voucherDetail = IDS.GLTransaction.GLVoucherD.GetVoucherDetail(branchCode, srcCode, voucher, 3);

                totalRecords = voucherDetail.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    voucherDetail = voucherDetail.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    voucherDetail = voucherDetail.Where(x => x.Counter.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.COA.Account.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                                 x.Dept.DepartmentCode.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.Descrip.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.DocumentNo.ToString().ToLower().Contains(searchValueLower)).ToList();
                    
                }
                totalRecordsShowing = voucherDetail.Count();
                // Paging
                if (pageSize > 0)
                        voucherDetail = voucherDetail.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = voucherDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
            //if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
            //    throw new Exception("You do not have access to access this page");

            //JsonResult result = new JsonResult();

            //try
            //{
            //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
            //    var start = Request.Form.GetValues("start").FirstOrDefault();
            //    var length = Request.Form.GetValues("length").FirstOrDefault();
            //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            //    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
            //    int skip = start != null ? Convert.ToInt32(start) : 0;
            //    int totalRecords = 0; // Total keseluruhan data
            //    int totalRecordsShowing = 0; // Total data setelah filter / search

            //    List<IDS.GLTransaction.GLVoucherD> voucherDetail = IDS.GLTransaction.GLVoucherD.GetVoucherDetail(branchCode, srcCode, voucher, 3);

            //    totalRecords = voucherDetail.Count;

            //    //Sorting
            //    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //    {
            //        switch (sortColumn.ToLower())
            //        {
            //            default:
            //                voucherDetail = voucherDetail.OrderBy(sortColumn + " " + sortColumnDir).ToList();
            //                break;
            //        }
            //    }

            //    //Search
            //    if (!string.IsNullOrEmpty(searchValue))
            //    {
            //        string searchValueLower = searchValue.ToLower();

            //        voucherDetail = voucherDetail.Where(x => x.Counter.ToString().ToLower().Contains(searchValueLower) ||
            //                                 x.COA.Account.ToString().ToLower().Contains(searchValueLower) ||
            //                                 x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
            //                                 x.Dept.DepartmentCode.ToString().ToLower().Contains(searchValueLower) ||
            //                                 x.Descrip.ToString().ToLower().Contains(searchValueLower) ||
            //                                 x.DocumentNo.ToString().ToLower().Contains(searchValueLower)).ToList();
            //    }

            //    totalRecordsShowing = voucherDetail.Count();

            //   //Paging
            //   voucherDetail = voucherDetail.Skip(skip).Take(pageSize).ToList();

            //   //Returning Json Data
            //   result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = voucherDetail }, JsonRequestBehavior.AllowGet);
            //    //result = this.Json(new { data = voucherDetail }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{

            //}

            //return result;
        }

        // GET: GLTransaction/Voucher
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

            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource().Where(x => x.Value != "AR" && x.Value != "AP").ToList(), "Value", "Text", null);
            ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            ViewData["StatusPostedList"] = new SelectList(IDS.GLTransaction.GLVoucherH.GetStatusPostedForDataSource(), "Value", "Text");

            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

            return PartialView("index");
        }

        // GET: GLTransaction/Voucher/Create
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

            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource()
                .Where(x => x.Value != "AR" && x.Value != "AP")
                .ToList(), "Value", "Text");

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }                

            //ViewData["CurrDate"] = DateTime.Now.Date.ToString("dd/MMM/yyyy");

            //ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text",IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<tr id=\"r' + index + '\">")
                //.Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" class=\"select2DDList\" style=\"padding: 3px; border: 0; max-width: 120px !important;\">")
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
                //.Append("<td style=\"padding: 1px 2px; !important; width: 140px;\"><input type=\"text\" id=\"txtAcc-' + index + '\" name=\"COA.Account\" class=\"select2DDList\" style=\"padding: 1px; border: none; max-width: 145px !important;\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"txtAcc-' + index + '\" name=\"COA.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--Acc--</option>")
                .Append("<td style=\"padding: 1px 2px !important; width: 500px;\"><input type=\"text\" id=\"txtDescription-' + index + '\" name=\"Descrip\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 56px;\"><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" class=\"select2DDList\" style=\"padding: 3px; border: 0; max-width: 60px !important;\">")
                .Append("<td style=\"padding: 1px 2px !important; width: 56px;\" hidden><select id=\"cboDept-' + index + '\" name=\"Dept.DepartmentCode\" style=\"padding: 3px; border: 0; max-width: 60px !important;\" hidden>")
                .Append("<option value=\"\" placeholder=\"Dept\" title=\"Please select Department\"></option>");

            foreach (SelectListItem item in deptList)
            {
                sb.Append("<option value=\"" + item.Value + "\" title=\"" + item.Text + "\">" + item.Value + "</option>");
            }

            sb.Append("</select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 88px;\"><select id=\"CashAcc-' + index + '\" name=\"CashAccount\" style=\"padding: 3px; border: 0; max-width: 80px !important; max-width: 80px !important;\" class=\"select2DDList\" onchange=\"OnCashBasisAccountChange(this)\" title=\"\" disabled></select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtDebit-' + index + '\" name=\"Debit\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"debitValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtCredit-' + index + '\" name=\"Credit\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"creditValue\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            
            return View("Create", new IDS.GLTransaction.GLVoucherH() { TransDate = DateTime.Now.Date, Voucher = "AUTO" });
        }

        // POST: GLTransaction/Voucher/Create
        [HttpPost]
        public ActionResult Create(int? FormAction, IDS.GLTransaction.GLVoucherH voucher)
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

            #region Validation
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            bool valid = true;

            if (voucher.SCode?.Code == null || string.IsNullOrEmpty(voucher.SCode?.Code))
            {
                valid = false;
                sb.Append("- Source code is required").Append(Environment.NewLine);
            }

            if (voucher.VBranch?.BranchCode == null || string.IsNullOrEmpty(voucher.VBranch?.BranchCode))
            {
                valid = false;
                sb.Append("- Branch is required").Append(Environment.NewLine);
            }

            if (voucher.TransDate == null || voucher.TransDate == DateTime.MinValue)
            {
                valid = false;
                sb.Append("- Transaction date is required or transaction date is invalid.").Append(Environment.NewLine);
            }

            if (voucher.Detail == null || voucher.Detail.Count == 0)
            {
                valid = false;
                sb.Append("Can not save voucher without Journal Item.");
            }
            else
            {
                double difference = voucher.Detail.Sum(x => x.Amount);

                if (difference != 0)
                {
                    valid = false;
                    sb.Append("Journal entry is not balance");
                }

                for (int i = 0; i < voucher.Detail.Count; i++)
                {
                    if (voucher.Detail[i].CCy?.CurrencyCode == null || string.IsNullOrEmpty(voucher.Detail[0].CCy?.CurrencyCode))
                    {
                        valid = false;
                        sb.Append("- One of journal item currency is not set or empty.").Append(Environment.NewLine);
                    }

                    if (voucher.Detail[i].COA?.Account == null || string.IsNullOrEmpty(voucher.Detail[0].COA.Account))
                    {
                        valid = false;
                        sb.Append("- One of journal item Chart of Account is not set or empty.").Append(Environment.NewLine);
                    }
                    else
                    {
                        if (voucher.Detail[i].CCy?.CurrencyCode != null && !string.IsNullOrEmpty(voucher.Detail[i].CCy?.CurrencyCode) &&
                            voucher.Detail[i].COA?.Account != null && !string.IsNullOrEmpty(voucher.Detail[i].COA?.Account))
                        {
                            bool isCashAccount = IDS.GLTable.ChartOfAccount.IsCashAccount(voucher.Detail[i].CCy.CurrencyCode, voucher.Detail[i].COA.Account);

                            if (isCashAccount && string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                            {
                                valid = false;
                                sb.Append("- One of journal item cash account is not set while chart of account is cash account").Append(Environment.NewLine);
                            }
                            else if (!isCashAccount && !string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                            {
                                valid = false;
                                sb.Append("- One of journal item cash account is set while chart of account is not cash account").Append(Environment.NewLine);
                            }
                        }
                    }

                    if (voucher.Detail[i].Amount == 0)
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

            voucher.OperatorID = currentUser;
            voucher.LastUpdate = DateTime.Now;

            // TODO: Sesuaikan Account aslinya dari Autocomplete
            foreach (IDS.GLTransaction.GLVoucherD d in voucher.Detail)
            {
                String[] separator = { " - " };
                string[] split = d.COA.Account.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                d.COA.Account = split[0];
            }

            ValidateModel(voucher);

            if (ModelState.IsValid)
            {
                try
                {
                    string newVoucherNo = "";
                    int result = voucher.InsUpDel((int)IDS.Tool.PageActivity.Insert, ref newVoucherNo);

                    if (result > 0)
                    {
                        if (string.IsNullOrWhiteSpace(newVoucherNo))
                            return Json("New Voucher has been save", JsonRequestBehavior.DenyGet);
                        else
                            return Json(new { msg = "New voucher has been save. Voucher No: "+ newVoucherNo, vno = newVoucherNo }, JsonRequestBehavior.AllowGet);
                        //return Json(string.Format("New voucher has been save. Voucher No: {0}", newVoucherNo), JsonRequestBehavior.DenyGet);
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

        // GET: GLTransaction/Voucher/Edit/5
        [HttpGet, 
            AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(string sCode, string voucher, string branchCode)
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

            IDS.GLTransaction.GLVoucherH vh = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetail(voucher, sCode, branchCode);

            string[] ccyInDetail = null;
            bool postedStatus = false;

            for (int i = 0; i < vh.Detail.Count; i++)
            {
                if (vh.Detail[i].UPD.Substring(0, 1) == "1")
                    postedStatus = true;
            }

            ViewData["VDPostedStatus"] = postedStatus;

            if (vh.Detail != null && vh.Detail.Count > 0)
            {
                ccyInDetail = vh.Detail.Select(x => new { CCy = Tool.GeneralHelper.NullToString(x.CCy.CurrencyCode) })
                    .Select(x => x.CCy).Distinct().ToArray();
            }

            if (ccyInDetail.Length > 0)
            {
                for (int i = 0; i < ccyInDetail.Length; i++)
                {
                    List<SelectListItem> ccy = new List<SelectListItem>();
                    ccy = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(ccyInDetail[i]);
                    ccy.Insert(0, new SelectListItem() { Text = "", Value = "" });
                    ViewData[ccyInDetail[i]] = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(ccyInDetail[i]);
                }
            }

            if (vh == null)
            {
                throw new Exception("Can not find voucher.");
            }

            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource()
                .Where(x => x.Value != "AR" && x.Value != "AP")
                .ToList(), "Value", "Text", vh.SCode.Code);

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(branchCode));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            //ViewData["CurrDate"] = vh.TransDate;
            //ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            //SelectListItem emptyCCy = new SelectListItem() { Text = "", Value = "--CCy--" };
            //ccyList.Insert(0, emptyCCy);
            ////ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text");
            ViewData["CCyList"] = ccyList;

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            //ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();
            deptList.Insert(0, new SelectListItem() { Text = "--Dept--", Value = "" });
            ViewData["deptList"] = deptList;
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<tr id=\"r' + index + '\">")
                //.Append("<td style=\"padding: 1px 2px !important; width: 65px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 65px !important;\">");
                .Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\">");
                //.Append("<option value=\"\">--CCy--</option>");

            foreach (SelectListItem item in ccyList)
            {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            sb.Append("</select></td>")
                //.Append("<td style=\"padding: 1px 2px; !important; width: 140px;\"><input type=\"text\" id=\"txtAcc-' + index + '\" name=\"COA.Account\" class=\"notd\" style=\"padding: 1px; border: none; max-width: 145px !important;\" disabled /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"txtAcc-' + index + '\" name=\"COA.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtDescription-' + index + '\" name=\"Descrip\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
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

            return View("Edit", vh);
        }

        // POST: GLTransaction/Voucher/Edit/5
        [HttpPost]
        public ActionResult Edit(int? FormAction, IDS.GLTransaction.GLVoucherH voucher)
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

                #region Validation
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                bool valid = true;

                if (voucher.SCode?.Code == null || string.IsNullOrEmpty(voucher.SCode?.Code))
                {
                    valid = false;
                    sb.Append("- Source code is required").Append(Environment.NewLine);
                }

                if (voucher.VBranch?.BranchCode == null || string.IsNullOrEmpty(voucher.VBranch?.BranchCode))
                {
                    valid = false;
                    sb.Append("- Branch is required").Append(Environment.NewLine);
                }

                if (voucher.TransDate == null || voucher.TransDate == DateTime.MinValue)
                {
                    valid = false;
                    sb.Append("- Transaction date is required or transaction date is invalid.").Append(Environment.NewLine);
                }

                if (voucher.Detail == null || voucher.Detail.Count == 0)
                {
                    valid = false;
                    sb.Append("Can not save voucher without Journal Item.");
                }
                else
                {
                    double difference = voucher.Detail.Sum(x => x.Amount);

                    if (difference != 0)
                    {
                        valid = false;
                        sb.Append("Journal entry is not balance");
                    }

                    for (int i = 0; i < voucher.Detail.Count; i++)
                    {
                        if (voucher.Detail[i].CCy?.CurrencyCode == null || string.IsNullOrEmpty(voucher.Detail[0].CCy?.CurrencyCode))
                        {
                            valid = false;
                            sb.Append("- One of journal item currency is not set or empty.").Append(Environment.NewLine);
                        }

                        if (voucher.Detail[i].COA?.Account == null || string.IsNullOrEmpty(voucher.Detail[0].COA.Account))
                        {
                            valid = false;
                            sb.Append("- One of journal item Chart of Account is not set or empty.").Append(Environment.NewLine);
                        }
                        else
                        {
                            if (voucher.Detail[i].CCy?.CurrencyCode != null && !string.IsNullOrEmpty(voucher.Detail[i].CCy?.CurrencyCode) &&
                                voucher.Detail[i].COA?.Account != null && !string.IsNullOrEmpty(voucher.Detail[i].COA?.Account))
                            {
                                bool isCashAccount = IDS.GLTable.ChartOfAccount.IsCashAccount(voucher.Detail[i].CCy.CurrencyCode, voucher.Detail[i].COA.Account);

                                if (isCashAccount && string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                                {
                                    valid = false;
                                    sb.Append("- One of journal item cash account is not set while chart of account is cash account").Append(Environment.NewLine);
                                }
                                else if (!isCashAccount && !string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                                {
                                    valid = false;
                                    sb.Append("- One of journal item cash account is set while chart of account is not cash account").Append(Environment.NewLine);
                                }
                            }
                        }

                        if (voucher.Detail[i].Amount == 0)
                        {
                            valid = false;
                            sb.Append("- One of journal item amount is not set or zero").Append(Environment.NewLine);
                        }
                    }
                }
                #endregion

                ModelState.Clear();

                if (!string.IsNullOrEmpty(voucher?.Voucher ?? null) && !string.IsNullOrEmpty(voucher?.VBranch?.BranchCode ?? null) && !string.IsNullOrEmpty(voucher?.SCode.Code ?? null))
                {
                    if (voucher.CheckPostedVoucherExists(new string[] { voucher.SCode.Code.ToString() + ";" + voucher.Voucher.ToString() + ";" + voucher.VBranch.BranchCode.ToString() }))
                    {
                        return Json("Voucher status is posted and can not be modified. Update process terminated.");
                    }
                }

                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                voucher.OperatorID = currentUser;
                voucher.LastUpdate = DateTime.Now;

                // TODO: Sesuaikan Account aslinya dari Autocomplete
                foreach (IDS.GLTransaction.GLVoucherD d in voucher.Detail)
                {
                    String[] separator = { " - " };
                    string[] split = d.COA.Account.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    d.COA.Account = split[0];
                }

                ValidateModel(voucher);

                if (ModelState.IsValid)
                {
                    try
                    {
                        string newVoucherNo = "";
                        int result = voucher.InsUpDel((int)IDS.Tool.PageActivity.Edit, ref newVoucherNo);

                        if (result > 0)
                        {
                            if (string.IsNullOrWhiteSpace(newVoucherNo))
                                return Json("Voucher has been update", JsonRequestBehavior.DenyGet);
                            else
                                return Json(string.Format("Voucher has been update. Voucher No: {0}", newVoucherNo), JsonRequestBehavior.DenyGet);
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: GLTransaction/Voucher/Delete/5
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

            try
            {
                string[] eachItem = voucherCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (eachItem.Length > 0)
                {
                    IDS.GLTransaction.GLVoucherH voucher = new IDS.GLTransaction.GLVoucherH();

                    // Terdapat voucher posted
                    if (voucher.CheckPostedVoucherExists(eachItem))
                    {
                        return Json("One or more selected voucher are posted and can not be delete. Delete process canceled by system.");
                    }

                    voucher.InsUpDel((int)IDS.Tool.PageActivity.Delete, eachItem);
                }

                return Json("Voucher has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVoucherForDataSource(string scode, string branchcode, string from, string to)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTransaction.GLVoucherH.GetVoucherForDataSource(scode, branchcode, Convert.ToDateTime(from), Convert.ToDateTime(to));

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVoucherForDataSourceWithAll(string scode, string branchcode, string from, string to,bool withAll)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTransaction.GLVoucherH.GetVoucherForDataSource(scode, branchcode, Convert.ToDateTime(from), Convert.ToDateTime(to),true);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVoucherForBankStatementList(string branchCode, string period, string accNo, string ccy, int? status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(period))
                {
                    return Json("Period parameter not found", JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrWhiteSpace(branchCode))
                {
                    return Json("Branch parameter not found", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(accNo))
                {
                    return Json("Account No parameter not found", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(ccy))
                {
                    return Json("Currency parameter not found", JsonRequestBehavior.AllowGet);
                }

                period = IDS.Tool.DateTimeExtension.MonthYearToPeriod(period);


                List<IDS.GLTransaction.GLVoucherH> result = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetailByAccountAndCurrency(period, branchCode, accNo, ccy);

                var data = result.Where(x => x.Detail != null && x.Detail.Count > 0).SelectMany(x => x.Detail.Select(y =>
                    new
                    {
                        Voucher = y.Voucher,
                        BranchCode = y.VBranch.BranchCode,
                        Account = y.COA.Account,
                        Counter = y.Counter,
                        SourceCode = x.SCode.Code,
                        TransDate = x.TransDate,
                        Amount = y.Amount,
                        CCy = y.CCy.CurrencyCode,
                        Descrip = y.Descrip,
                        Description = x.Description
                    }));

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}