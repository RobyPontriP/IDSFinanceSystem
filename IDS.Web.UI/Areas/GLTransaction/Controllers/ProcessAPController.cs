using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class ProcessAPController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string arap, string supCode, string ccy, string specialAcc, bool process)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            try
            {
                //
                List<IDS.GLTable.ACFARAP> ProcAP = IDS.GLTable.ACFARAP.GetACFARAPVEND(arap, supCode, ccy, specialAcc);
                //
                // Sorting    


                ProcAP = ProcAP.OrderByDescending(x => x.DocNo).ToList();

                ProcAP = ProcAP.Where(x => x.ProcessInv == process).ToList();

                ProcAP = ProcAP.OrderByDescending(x => x.DocNo.Substring(Math.Max(0, x.DocNo.Length - 4))).ToList();
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(ProcAP), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string docNo, string ccy,string rp, string branchCode)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            try
            {
                //
                List<IDS.GLTable.SUBACFARAP> subarap = IDS.GLTable.SUBACFARAP.GetSUBACFARAP(docNo,"", ccy, branchCode).Where(x => x.RP == "P").ToList();
                //
                // Sorting  

                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(subarap), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        // GET: GLTransaction/CashBankProcess
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

            ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text");
            ViewData["SelectListSupCode"] = new SelectList(IDS.GLTable.ACFARAP.GetACFARAPVENDForDataSource("P"), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");

            return View();
        }

        public ActionResult EditAP(string rp, string acc, string ccy, string sup, string docno, string branch)
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

            IDS.GLTable.ACFARAP acfarap = IDS.GLTable.ACFARAP.GetACFARAPVendWithDetail(rp, sup, ccy, acc, docno, branch);

            if (rp == "R")
            {
                ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AR - Account Receivable")).ToList(), "Value", "Text", "2");
            }
            else if (rp == "P")
            {
                ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text", "3");
            }
            
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",acfarap.CCy.CurrencyCode);
            //ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource("A" + rp, ccy), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ACFARAP.GetAccACFARAPForDataSource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text",acfarap.Acc.Account);
            ViewData["SelectListSup"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text", sup);
            //ViewData["SelectListRelatedInv"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(), "Value", "Text",acfarap.Invoice.InvoiceNumber);
            ViewData["SelectListRelatedInv"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(), "Value", "Text");
            ViewData["SelectListSCode"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource(), "Value", "Text", acfarap.SCode.Code);

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
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text");
            
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

            #region Edit Detail
            System.Text.StringBuilder sbEditTemplate = new System.Text.StringBuilder();

            for (int i = 0; i < acfarap.ListSUBACFARAP.Count; i++)
            {
                sbEditTemplate.Append("<tr id=\"" + "r" + (i + 1) + "\">")
                    .Append("<td style=\"padding: 1px 2px !important; width: 60px;\"><input type=\"text\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" value=\"" + acfarap.ListSUBACFARAP[i].SeqNo.ToString() + "\" onkeyup=\"InputNumber(this)\" style=\"padding: 3px; max-width: 30px !important; border: none;\" disabled=\"disabled\"/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 70px;\"><select id=\"cboCCy-" + (i + 1) + "\" name=\"CCySUBACFARAP.CurrencyCode\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; max-width: 70px !important;\" class=\"select2DDList\" disabled=\"disabled\">");

                foreach (SelectListItem item in ccyList)
                {
                    if (item.Value == acfarap.ListSUBACFARAP[i].CCySUBACFARAP.CurrencyCode)
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                    else
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                }
                sbEditTemplate.Append("</select></td>");

                sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 290px;\"><select id=\"cboAcc-" + (i + 1) + "\" name=\"SubAcc.Account\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 290px !important;\" class=\"select2DDList\">");

                List<SelectListItem> accList = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(acfarap.ListSUBACFARAP[i].CCySUBACFARAP.CurrencyCode);
                accList.Insert(0, new SelectListItem() { Text = "", Value = "" });
                foreach (SelectListItem item in accList)
                {

                    if (item.Value == acfarap.ListSUBACFARAP[i].SubAcc.Account)
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text.Replace("'", "").Trim() + "</option>");
                    else
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text.Replace("'", "").Trim() + "</option>");
                }
                sbEditTemplate.Append("</select></td>");

                sbEditTemplate.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"" + acfarap.ListSUBACFARAP[i].Description + "\" id=\"txtRemark-" + (i + 1) + "\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", acfarap.ListSUBACFARAP[i].SubAmount) + "\" id=\"txtAmt-" + (i + 1) + "\" name=\"SubAmountDet\" onchange=\"AmtChange(this)\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled=\"disabled\"/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", acfarap.ListSUBACFARAP[i].EquivAmt) + "\" id=\"txtEquivAmt-" + (i + 1) + "\" name=\"EquivAmtDet\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled=\"disabled\"/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-" + (i + 1) + "\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\" hidden></i></td>")
                    .Append("</tr>");

                ViewData["EditRowTemplate"] = sbEditTemplate.ToString();
            }

           
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
        public ActionResult EditAP(int? FormAction, IDS.GLTable.ACFARAP acfarap)
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

            acfarap.OperatorID = currentUser;
            acfarap.Branch = new IDS.GeneralTable.Branch();
            acfarap.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ValidateModel(acfarap);

            if (ModelState.IsValid)
            {
                try
                {

                    int result = IDS.GLTable.SUBACFARAP.UpdateSUBACC(2, acfarap.ListSUBACFARAP,acfarap.SCode.Code);

                    if (result > 0)
                    {
                        return Json("Set SUB ACC Success", JsonRequestBehavior.DenyGet);
                        //if (string.IsNullOrWhiteSpace(newAPNo))
                        //    return Json("Edit A" + acfarapp.RP + " Success", JsonRequestBehavior.DenyGet);
                        //else
                        //    return Json(string.Format("Edit AP Success. A" + acfarapp.RP + " No: {0}", newAPNo), JsonRequestBehavior.DenyGet);
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

        public ActionResult Process(string rpList, string accList, string ccyList, string supList, string docNoList, string branchCodeList)
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

            ViewData["SelectListARAP"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource().Where(x => x.Text.Contains("AR - Account Receivable") || x.Text.Contains("AP - Account Payable")).ToList(), "Value", "Text");
            ViewData["SelectListCustCode"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["SelectListCCY"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            try
            {
                string[] rp = rpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] acc = accList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] ccy = ccyList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] sup = supList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] docNo = docNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] branchCode = branchCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string result = "";
                string docnoFailed = "";
                string docnoSucces = "";

                for (int i = 0; i < docNo.Length; i++)
                {
                    IDS.GLTable.ACFARAP procArap = new IDS.GLTable.ACFARAP();

                    procArap = IDS.GLTable.ACFARAP.GetACFARAPVendWithDetail(rp[i], sup[i], ccy[i], acc[i], docNo[i], branchCode[i]);
                    procArap.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                    int approved = 0;
                    
                    if (!string.IsNullOrEmpty(procArap.CashBank.EntryUser)) 
                    {
                        approved++;
                    }
                    if (!string.IsNullOrEmpty(procArap.CashBank.Approval1))
                    {
                        approved++;
                    }
                    if (!string.IsNullOrEmpty(procArap.CashBank.Approval2))
                    {
                        approved++;
                    }
                    if (!string.IsNullOrEmpty(procArap.CashBank.Approval3))
                    {
                        approved++;
                    }

                    if (approved < 3)
                    {
                        docnoFailed += docNo[i]+", ";
                    }
                    else
                    {
                        docnoSucces += procArap.APProcess()+"( "+docNo[i] +" )";
                    }
                }

                if (!string.IsNullOrEmpty(docnoFailed))
                {
                    docnoFailed = "This transaction " + docnoFailed + "must be approved first \n \n";
                }
                
                result = docnoFailed + docnoSucces;

                return Json(result, JsonRequestBehavior.AllowGet);
                //return Json("Selected AP has been Processed.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        
    }
}