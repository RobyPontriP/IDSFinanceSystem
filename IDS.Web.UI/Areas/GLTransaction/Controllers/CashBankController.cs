using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using ClosedXML.Excel;
using System.IO;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class CashBankController : IDS.Web.UI.Controllers.MenuController
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
                //
                List<IDS.GLTransaction.CashBankH> cbH = IDS.GLTransaction.CashBankH.GetCashBankH(period);
                //
                //totalRecords = pphD.Count;

                // Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        default:
                //            pphD = pphD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                // Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    pphD = pphD.Where(x => x.Month.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TaxRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotongan.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotonganKumulatif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Tarif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = pphD.Count();

                //// Paging
                //if (pageSize > 0)
                //    pphD = pphD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = pphD }, JsonRequestBehavior.AllowGet);
                //result = this.Json(new { data = pphD }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(cbH), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public JsonResult GetDataApprovalList(string period)
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
                //
                List<IDS.GLTransaction.CashBankH> cbH = IDS.GLTransaction.CashBankH.GetApprovalList(period);
                //
                //totalRecords = pphD.Count;

                // Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        default:
                //            pphD = pphD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                // Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    pphD = pphD.Where(x => x.Month.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TaxRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotongan.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotonganKumulatif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Tarif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = pphD.Count();

                //// Paging
                //if (pageSize > 0)
                //    pphD = pphD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = pphD }, JsonRequestBehavior.AllowGet);
                //result = this.Json(new { data = pphD }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(cbH), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string cbNo)
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
                //
                List<IDS.GLTransaction.CashBankD> cbD = IDS.GLTransaction.CashBankD.GetCashBankD(cbNo);
                //
                totalRecords = cbD.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            cbD = cbD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    cbD = cbD.Where(x => x.Counter.ToString().ToLower().Contains(searchValueLower) ||
                                            x.SubCounter.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Type.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Amount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Remark.ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = cbD.Count();

                // Paging
                if (pageSize > 0)
                    cbD = cbD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = cbD }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GLTransaction/CashBank
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

        [HttpGet]
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
            
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            List<SelectListItem> SupList = IDS.GeneralTable.Supplier.GetACFVENDForDataSource();
            //SupList.Insert(0, new System.Web.Mvc.SelectListItem { Text = "", Value = "" });
            ViewData["SelectListSup"] = new SelectList(SupList, "Value", "Text");

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

            #region Detail
            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            List<SelectListItem> typeList = IDS.GLTransaction.CashBankD.GetTypeCBD();

            ViewData["CcyList"] = new SelectList(ccyList, "Value", "Text");
            ViewData["TypeList"] = new SelectList(typeList, "Value", "Text");
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtCounter-' + index + '\" name=\"Counter\" onchange=\"OnCounterChange(this)\" onkeyup=\"InputNumber(this)\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSubSeqAuto + '\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" disabled=\"disabled\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboType-' + index + '\" name=\"TypeD\" onchange=\"TypeChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\">");
                //.Append("<option value=\"\">--Type--</option>");

                foreach (SelectListItem item in typeList)
                {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                }

            sb.Append("</select></td>");

                sb.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"Amount\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumberMinus(this)\" value=\"0\" onchange=\"OnAmountChange(this)\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            #endregion
            
            return View("Create", new IDS.GLTransaction.CashBankH() { CBDate = DateTime.Now.Date, DueDate = DateTime.Now.Date, CashBankNumber = "A U T O"});
        }

        [HttpPost]
        public ActionResult Create(int? FormAction, IDS.GLTransaction.CashBankH cbH)
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

            #endregion

            ModelState.Clear();

            string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
            }

            cbH.OperatorID = currentUser;
            //cbH.ExchangeRate = IDS.GLTable.ExchangeRate.GetMidRate(inv.CCy.CurrencyCode, IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            cbH.Branch = new IDS.GeneralTable.Branch();
            cbH.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
           
            ValidateModel(cbH);

            if (ModelState.IsValid)
            {
                //string isValid = "";
                try
                {
                    string newCbNo = "";
                    int result = cbH.InsUpDelCB((int)FormAction, ref newCbNo);

                    if (result > 0)
                    {
                        if (FormAction == 1)
                        {
                            if (string.IsNullOrWhiteSpace(newCbNo))
                                return Json("New Cash/Bank has been save", JsonRequestBehavior.DenyGet);
                            else
                                return Json(new { msg = "New Cash/Bank has been save. Cash/Bank No: " + newCbNo, cbno = newCbNo }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { msg = "Cash/Bank has been edited. Cash/Bank No: " + newCbNo, cbno = newCbNo }, JsonRequestBehavior.AllowGet);
                        }
                        
                    }
                    else
                        throw new Exception("Error");
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.DenyGet);
                }

                //if (string.IsNullOrEmpty(cbH.Validation()))
                //{
                //    try
                //    {
                //        string newSalesInvNo = "";
                //        int result = inv.InsUpDelInvoice((int)IDS.Tool.PageActivity.Insert);

                //        if (result > 0)
                //        {
                //            if (string.IsNullOrWhiteSpace(newSalesInvNo))
                //                return Json("New Sales Invoice has been save", JsonRequestBehavior.DenyGet);
                //            else
                //                return Json(string.Format("New voucher has been save. Sales Invoice No: {0}", newSalesInvNo), JsonRequestBehavior.DenyGet);
                //        }
                //        else
                //            throw new Exception("Error");
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json(ex.Message, JsonRequestBehavior.DenyGet);
                //    }
                //}
                //else
                //{
                //    return Json(isValid, JsonRequestBehavior.AllowGet);
                //}

            }
            else
            {
                return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string cbNo)
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

            IDS.GLTransaction.CashBankH cbh = IDS.GLTransaction.CashBankH.GetCashBankHWithDetail(cbNo);
            ViewData["FormAction"] = 2;
            ViewData["UserLogin"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID];
            
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",cbh.Ccy.CurrencyCode);
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCForDatasource((int)cbh.Type == 0 ? "KS" : "BN",cbh.Ccy.CurrencyCode), "Value", "Text",cbh.Account.Account);

            List<SelectListItem> SupList = IDS.GeneralTable.Supplier.GetACFVENDForDataSource();
            SupList.Insert(0, new System.Web.Mvc.SelectListItem { Text = "", Value = "" });
            ViewData["SelectListSup"] = new SelectList(SupList, "Value", "Text",cbh.Supplier.SupCode);

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

            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            List<SelectListItem> typeList = IDS.GLTransaction.CashBankD.GetTypeCBD();

            ViewData["CcyList"] = new SelectList(ccyList, "Value", "Text");
            ViewData["TypeList"] = new SelectList(typeList, "Value", "Text");
            System.Text.StringBuilder sbEditTemplate = new System.Text.StringBuilder();

            #region EditTemplateHTML
            for (int i = 0; i < cbh.CBDetail.Count; i++)
            {
                sbEditTemplate.Append("<tr id=\"" + "r" + (i + 1) + "\">")
                //.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtSeqNo-' + index + '\" name=\"SeqNo\" style=\"padding: 3px; width: 30px; border: none;\" disabled=\"disabled\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"" + cbh.CBDetail[i].Counter.ToString() + "\" id=\"txtCounter-" + (i + 1) + "\" name=\"Counter\" onchange=\"OnCounterChange(this)\" onkeyup=\"InputNumber(this)\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"" + cbh.CBDetail[i].SubCounter.ToString() + "\" id=\"txtSubCounter-" + (i + 1) + "\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" disabled=\"disabled\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                //.Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboAlloType-" + i + "\" name=\"AlloType\" onchange=\"AlloTypeChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\" disabled>");
                .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboType-" + (i + 1) + "\" name=\"TypeD\" onchange=\"TypeChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\">");

                foreach (SelectListItem item in typeList)
                {
                    if (item.Value == cbh.CBDetail[i].Type.ToString())
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                    else
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                }

                sbEditTemplate.Append("</select></td>");

                sbEditTemplate.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"" + cbh.CBDetail[i].Remark+ "\" id=\"txtRemark-" + (i + 1) + "\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}",cbh.CBDetail[i].Amount) + "\" id=\"txtAmt-" + (i + 1) + "\" name=\"Amount\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumberMinus(this)\" value=\"0\" onchange=\"OnAmountChange(this)\"/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-" + (i + 1) + "\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                    .Append("</tr>");
            }
            ViewData["EditRowTemplate"] = sbEditTemplate.ToString();
            #endregion

            #region Detail
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtCounter-' + index + '\" name=\"Counter\" onchange=\"OnCounterChange(this)\" onkeyup=\"InputNumber(this)\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSubSeqAuto + '\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" disabled=\"disabled\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboType-' + index + '\" name=\"TypeD\" onchange=\"TypeChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\">");
            //.Append("<option value=\"\">--Type--</option>");

            foreach (SelectListItem item in typeList)
            {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            sb.Append("</select></td>");

            sb.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
            .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"Amount\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumberMinus(this)\" value=\"0\" onchange=\"OnAmountChange(this)\"/></td>")
            .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
            .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            #endregion

            if (cbh != null)
            {
                return PartialView("Create", cbh);
            }
            else
            {
                return PartialView("Create", new IDS.GLTransaction.CashBankH());
            }
        }

        public ActionResult Delete(string cbList)
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

            if (string.IsNullOrWhiteSpace(cbList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] cbsCode = cbList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (cbsCode.Length > 0)
                {
                    IDS.GLTransaction.CashBankH cbH = new IDS.GLTransaction.CashBankH();
                    cbH.InsUpDelCB((int)IDS.Tool.PageActivity.Delete, cbsCode);
                }

                return Json("Cash/Bank data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ApprovalList()
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

        public ActionResult Approval(string password,string cbNo)
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

            Tool.clsCryptho crypt = new Tool.clsCryptho();
            string encryptPassword = crypt.Encrypt(password, "ids");
            IDS.Maintenance.User user = IDS.Maintenance.User.UserLogin(Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), encryptPassword);

            string result = "";

            if (user != null)
            {
                result = IDS.GLTransaction.CashBankH.SetApproval(Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), cbNo);
            }
            else
            {
                return Json("Incorrect Password.", JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RedirectToRptCB(string cbNo,string amt)
        {
            string url = "~/Report/GLReport/wfRptCashBank.aspx?cbNo=" + cbNo+"&amt="+amt;
            //string url = "~/Report/Sales/wfRptTaxInvoice.aspx";
            return Redirect(url);
        }

        public ActionResult RedirectToRptApprovalCBList()
        {
            string url = "~/Report/GLReport/wfRptCashBankApprovalList.aspx";
            //string url = "~/Report/Sales/wfRptTaxInvoice.aspx";
            return Redirect(url);
        }

        public FileResult Export(string period,string cbno)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }
            //NorthwindEntities entities = new NorthwindEntities();
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.AddRange(new System.Data.DataColumn[12] { new System.Data.DataColumn("Cash Bank Code"),
                                            new System.Data.DataColumn("Branch"),
                                            new System.Data.DataColumn("Branch Address"),
                                            new System.Data.DataColumn("Branch Phone"),
                                            new System.Data.DataColumn("Supplier Name"),
                                            new System.Data.DataColumn("Supplier Acc"),
                                            new System.Data.DataColumn("Supplier Bank"),
                                            new System.Data.DataColumn("Acc"),
                                            new System.Data.DataColumn("Payment Amount"),
                                            new System.Data.DataColumn("Bank Charges"),
                                            new System.Data.DataColumn("Amount Total"),
                                            new System.Data.DataColumn("Terbilang") });

            //var customers = from customer in entities.Customers.Take(10)
            //                select customer;

            List<IDS.GLTransaction.CashBankH> cbH = IDS.GLTransaction.CashBankH.GetCashBankHExcel(period,cbno);
            decimal bankcharges = 0;
            foreach (var item in cbH)
            {
                bankcharges = IDS.GLTransaction.CashBankD.GetBankCharges(item.CashBankNumber);
                dt.Rows.Add(item.CashBankNumber,item.Branch.BranchName,item.Branch.FullAddress,item.Branch.Phone1,item.Supplier.SupName,item.Supplier.BenBankAcc,item.Supplier.BenBank,item.Account.Account,item.CBAmount-bankcharges, bankcharges, item.CBAmount, new IDS.Tool.clsNumberToWord().NumToWordInd(Convert.ToDouble(item.CBAmount), 1));
            }

            dt.TableName = "Cash Bank";
            string excelName = "CashBankTransaction" + period+".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);//"Grid.xlsx");
                }
            }
        }
    }
}