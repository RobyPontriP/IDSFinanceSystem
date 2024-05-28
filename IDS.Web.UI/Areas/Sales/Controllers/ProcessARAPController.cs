using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class ProcessARAPController : IDS.Web.UI.Controllers.MenuController
    {
        [App_Start.IDSAjaxAuth]
        public JsonResult GetData(string arap, string custCode, string ccy, string specialAcc, bool process)
        {
            //if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
            //    throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
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
                List<IDS.GLTable.ACFARAP> ProcArap = IDS.GLTable.ACFARAP.GetACFARAP(arap,custCode,ccy,specialAcc);
                //
                totalRecords = ProcArap.Count;

                // Sorting    
                

                ProcArap = ProcArap.OrderByDescending(x => x.DocNo).ToList();

                ProcArap = ProcArap.Where(x => x.ProcessInv == process).ToList();

                ProcArap = ProcArap.OrderByDescending(x => x.DocNo.Substring(Math.Max(0, x.DocNo.Length - 4))).ToList();

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            ProcArap = ProcArap.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    ProcArap = ProcArap.Where(x => x.RP.ToString().ToLower().Contains(searchValueLower) ||
                                             x.DocDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                             x.DocNo.ToLower().Contains(searchValueLower) ||
                                             x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                             x.SalesType.ToLower().Contains(searchValueLower) ||
                                             x.CustomerACFARAP.CUSTCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Branch.BranchCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Acc.Account.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Amount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Payment.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Outstanding.ToString().ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT.ToLower()).Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = ProcArap.Count();

                // Paging
                // Paging
                if (pageSize > 0)
                    ProcArap = ProcArap.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = ProcArap }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string docNo, string custPrin, string ccy, string branchCode)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            List<IDS.GLTable.SUBACFARAP> subArap = IDS.GLTable.SUBACFARAP.GetSUBACFARAP(docNo, custPrin, ccy, branchCode);
            
            result = this.Json(new { data = subArap }, JsonRequestBehavior.AllowGet);
            ViewData["RowDetailHtml"] = "aaaaaaaaaaa";
            return result;
        }

        // GET: Sales/ProcessARAP
        [HttpGet]
        [AcceptVerbs(HttpVerbs.Get)]
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

            ViewData["MessageError"] = "";
            ViewData["RowDetailHtml"] = "";

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x=> x.Text.Contains("AR - Account Receivable") || x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text");
            ViewData["SelectListCustCode"] = new SelectList(IDS.GLTable.ACFARAP.GetACFARAPForDataSource("R"), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource("AR", IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");

            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(string rpList, string accList, string ccyList,string custPrinList,string docNoList,string branchCodeList)
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

            try
            {
                string[] rp = rpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] acc = accList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] ccy = ccyList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] custPrin = custPrinList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] docNo = docNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] branchCode = branchCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string msgError = "";
                for (int i = 0; i < docNo.Length; i++)
                {
                    IDS.GLTable.ACFARAP procArap = new IDS.GLTable.ACFARAP();

                    procArap = IDS.GLTable.ACFARAP.GetACFARAP(rp[i], custPrin[i], ccy[i], acc[i], docNo[i], branchCode[i]);
                    procArap.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                    msgError += procArap.ARAPProcess(docNo.Length, procArap);
                }

                ViewData["MessageError"] = msgError;
            }
            catch
            {

                throw;
            }

            ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AR - Account Receivable") || x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text");
            ViewData["SelectListCustCode"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;


            return View();
        }

        #region Update AP
        [HttpGet]
        public ActionResult CreateAP()
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

            ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text", "3");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource().Where(x => x.Value.Contains(IDS.GeneralTable.Syspar.GetInstance().BaseCCy)), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["SelectListRelatedInv"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(), "Value", "Text");

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

            ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            #region Detail
            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text");


            List<SelectListItem> accList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 2px !important; width: 60px;\"><input type=\"text\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" value=\"' + indexSeqNo + '\" onkeyup=\"InputNumber(this)\" style=\"padding: 3px; max-width: 30px !important; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCySUBACFARAP.CurrencyCode\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--CCy--</option>");

            foreach (SelectListItem item in ccyList)
            {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            sb.Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"cboAcc-' + index + '\" name=\"SubAcc.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--Acc--</option>")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"SubAmountDet\" onchange=\"AmtChange(this)\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtEquivAmt-' + index + '\" name=\"EquivAmtDet\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled=\"disabled\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            #endregion
            
            return View("CreateAP", new IDS.GLTable.ACFARAP());
        }

        [HttpPost]
        public ActionResult CreateAP(int? FormAction, IDS.GLTable.ACFARAP acfarapp)
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
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            #region Validation

            #endregion

            ModelState.Clear();

            string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
            }

            acfarapp.OperatorID = currentUser;
            //acfarap.ExchangeRate = IDS.GLTable.ExchangeRate.GetMidRate(inv.CCy.CurrencyCode, "IDR", inv.InvoiceDate);

            acfarapp.Branch = new IDS.GeneralTable.Branch();
            acfarapp.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            
            ValidateModel(acfarapp);

            if (ModelState.IsValid)
            {
                try
                {
                    string newAPNo = "";
                    int result = acfarapp.InsUpDelAP((int)IDS.Tool.PageActivity.Insert);

                    if (result > 0)
                    {
                        if (string.IsNullOrWhiteSpace(newAPNo))
                            return Json("New AP has been save", JsonRequestBehavior.DenyGet);
                        else
                            return Json(string.Format("New AP has been save. AP No: {0}", newAPNo), JsonRequestBehavior.DenyGet);
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

        [HttpGet]
        public ActionResult EditAP(string rp, string acc, string ccy,string cust, string docno,string branch)
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

            IDS.GLTable.ACFARAP acfarap = IDS.GLTable.ACFARAP.GetACFARAPWithDetail(rp, cust, ccy, acc, docno, branch);

            if (rp == "R")
            {
                ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AR - Account Receivable")).ToList(), "Value", "Text", "2");
            }
            else if(rp == "P")
            {
                ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text", "3");
            }
            
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource().Where(x => x.Value.Contains(IDS.GeneralTable.Syspar.GetInstance().BaseCCy)), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource("A" +rp, ccy), "Value", "Text");
            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text",cust);
            //ViewData["SelectListRelatedInv"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(), "Value", "Text",acfarap.Invoice.InvoiceNumber);
            ViewData["SelectListRelatedInv"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(), "Value", "Text");

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

            //ViewData["UrlReferrer"] = Request.UrlReferrer?.AbsoluteUri ?? Url.Action("index", "Voucher", new { Area = "GLTransaction" });
            ViewData["DocDateVD"] = acfarap.DocDate.ToString("dd/MMM/yyyy");
            ViewData["ReceivedDateVD"] = acfarap.ReceivedDate.ToString("dd/MMM/yyyy");
            //ViewData["InvTransDate"] = invoice.InvoiceTransDate.ToString("dd-MMM-yyyy");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            #region Detail
            //List<SelectListItem> accList = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(acfarap.CCy.CurrencyCode);
            //ViewData["AccList"] = new SelectList(accList, "Value", "Text");
            List<SelectListItem> AccList = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(acfarap.CCy.CurrencyCode);
            AccList.Insert(0, new SelectListItem() { Text = "", Value = "" });
            ViewData["AccList"] = AccList;
            
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ccyList.Insert(0, new SelectListItem() { Text = "", Value = "" });
            ViewData["CCyList"] = ccyList;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 2px !important; width: 60px;\"><input type=\"text\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" value=\"' + indexSeqNo + '\" onkeyup=\"InputNumber(this)\" style=\"padding: 3px; max-width: 30px !important; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCySUBACFARAP.CurrencyCode\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--CCy--</option>");

            foreach (SelectListItem item in ccyList)
            {
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            sb.Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"cboAcc-' + index + '\" name=\"SubAcc.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">")
                .Append("<option value=\"\">--Acc--</option>")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"SubAmountDet\" onchange=\"AmtChange(this)\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtEquivAmt-' + index + '\" name=\"EquivAmtDet\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled=\"disabled\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            #endregion

            if (acfarap != null)
            {
                return PartialView("EditAP", acfarap);
            }
            else
            {
                return PartialView("EditAP", new IDS.GLTable.ACFARAP());
            }
        }

        [HttpPost]
        public ActionResult EditAP(int? FormAction, IDS.GLTable.ACFARAP acfarapp)
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

            acfarapp.OperatorID = currentUser;
            //acfarap.ExchangeRate = IDS.GLTable.ExchangeRate.GetMidRate(inv.CCy.CurrencyCode, "IDR", inv.InvoiceDate);

            acfarapp.Branch = new IDS.GeneralTable.Branch();
            acfarapp.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            //if (!string.IsNullOrEmpty(inv.WHTaxCode))
            //{
            //    string[] whtaxArr = inv.WHTaxCode.Split('-');
            //    inv.WHTaxCode = whtaxArr[0];
            //    inv.WHTaxPercent = Tool.GeneralHelper.NullToDouble(whtaxArr[1], 0);
            //}

            ValidateModel(acfarapp);

            if (ModelState.IsValid)
            {
                try
                {
                    string newAPNo = "";
                    int result = acfarapp.InsUpDelAP((int)IDS.Tool.PageActivity.Edit);

                    if (result > 0)
                    {
                        if (string.IsNullOrWhiteSpace(newAPNo))
                            return Json("Edit A"+acfarapp.RP+" Success", JsonRequestBehavior.DenyGet);
                        else
                            return Json(string.Format("Edit AP Success. A" + acfarapp.RP + " No: {0}", newAPNo), JsonRequestBehavior.DenyGet);
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
        #endregion

        public ActionResult Delete(string arapCollection)
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

            if (string.IsNullOrWhiteSpace(arapCollection))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            // Check voucher Posted Status


            try
            {
                string[] eachItem = arapCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (eachItem.Length > 0)
                {
                    IDS.GLTable.ACFARAP arap = new IDS.GLTable.ACFARAP();

                    //// Terdapat voucher posted
                    //if (voucher.CheckPostedVoucherExists(eachItem))
                    //{
                    //    return Json("One or more selected voucher are posted and can not be delete. Delete process canceled by system.");
                    //}

                    arap.DelACFARAP(IDS.Tool.PageActivity.Delete, eachItem);
                }

                return Json("AP has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomerCode(string arap)
        {
            arap = string.IsNullOrEmpty(arap) ? "" : arap.Substring(arap.Length-1);

            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.ACFARAP.GetACFARAPForDataSource(arap);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [App_Start.IDSAjaxAuth]
        public JsonResult GetAccForDataSource(string arap,string ccy)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource(arap,ccy);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFilterData(List<IDS.GLTable.ACFARAP> ProcArap)
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

                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
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
                totalRecords = ProcArap.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            ProcArap = ProcArap.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    ProcArap = ProcArap.Where(x => x.RP.ToString().ToLower().Contains(searchValueLower) ||
                                             x.DocDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                             x.DocNo.ToLower().Contains(searchValueLower) ||
                                             x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                             x.SalesType.ToLower().Contains(searchValueLower) ||
                                             x.CustomerACFARAP.CUSTCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Branch.BranchCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Acc.Account.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Amount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Payment.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Outstanding.ToString().ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT.ToLower()).Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = ProcArap.Count();

                // Paging
                if (pageSize > 0)
                    ProcArap = ProcArap.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = ProcArap }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }


    }
}