using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.IO;


namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class SalesInvoiceController : IDS.Web.UI.Controllers.MenuController
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
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Sales.Invoice> inv = IDS.Sales.Invoice.GetSalesInvoice(period);
                totalRecords = inv.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            inv = inv.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    inv = inv.Where(x => x.InvoiceNumber.ToString().ToLower().Contains(searchValueLower) ||
                                             x.InvoiceDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                                             x.Cust.CUSTCode.ToLower().Contains(searchValueLower) ||
                                             x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                             x.InvoiceAmount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.DiscountAmount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.PPnAmount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.PPhAmount.ToString().ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT.ToLower()).Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = inv.Count();

                // Paging
                inv = inv.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = inv }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string invNo)
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

                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                //int totalRecords = 0; // Total keseluruhan data
                //int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Sales.InvoiceDetail> invDetail = IDS.Sales.InvoiceDetail.GetInvoiceDetail(invNo);

                //totalRecords = invDetail.Count;

                // Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        default:
                //            invDetail = invDetail.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                // Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    invDetail = invDetail.Where(x => x.Counter.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.SubCounter.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Remark.ToLower().Contains(searchValueLower) ||
                //                             x.Amount.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.SubAmount.ToString().ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = invDetail.Count();

                // Paging
                //invDetail = invDetail.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = invDetail }, JsonRequestBehavior.AllowGet);
                result = this.Json(new { data = invDetail }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetTempDataDetail(string invNo,int counter, int subCounter,string description,int inTax,decimal transAmt, decimal amt)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            List<IDS.Sales.InvoiceDetail> invDetail = IDS.Sales.InvoiceDetail.GetInvoiceDetail(invNo);

            invDetail.Add(new IDS.Sales.InvoiceDetail { InvoiceNumber = invNo, Counter = counter, SubCounter = subCounter, Remark = description, SubAmount=transAmt.ToString()});

            //result = this.Json(new { data = invDetail }, JsonRequestBehavior.AllowGet);
            return result;
        }

        // GET: Sales/SalesInvoice
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

            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy, "AR"), "Value", "Text");
            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["SelectListTaxNo"] = new SelectList(IDS.Sales.TaxNumber.GetTaxNumberForDataSource(), "Value", "Text");
            ViewData["SelectListTaxTransType"] = new SelectList(IDS.Sales.TaxNumber.GetTransType(), "Value", "Text");
            ViewData["SelectListTaxType"] = new SelectList(IDS.Sales.TaxNumber.GetTaxType(), "Value", "Text");
            ViewData["SelectListWHTaxPercent"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            string NextInvNo = IDS.Sales.Invoice.GetNextInvoiceNumber(Tool.DateTimeExtension.ToPeriod(DateTime.Now), Tool.DateTimeExtension.ToPeriod(DateTime.Now).Substring(0, 4));
            ViewData["lblNextInvNo"] = "Next invoice number : " + NextInvNo;
            ViewData["NextInvoiceNo"] = NextInvNo.Substring(5);
            ViewData["NextInvNumber"] = NextInvNo.Substring(0, 5);
            //NextInvNo.Substring(0, 5);

            #region HtmlTableDetail
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtCounter-' + index + '\" name=\"Counter\" onchange=\"OnCounterChange(this)\" onkeyup=\"InputNumber(this)\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSubSeqAuto + '\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" disabled=\"disabled\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"text-align:center; padding: 1px 2px !important; width: 70px;\"><input type=\"checkbox\" id=\"chkTaxInv-' + index + '\" name=\"TaxInvoice\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtSubAmt-' + index + '\" name=\"SubAmount\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtSubAmt-' + index + '\" name=\"SubAmount\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"Amount\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" onchange=\"OnAmountChange(this)\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");
            
            ViewData["NewRowTemplate"] = sb.ToString();
            #endregion

            ViewBag.UserMenu = MainMenu;

            return View("Create", new IDS.Sales.Invoice());
        }

        [HttpPost]
        public ActionResult Create(int? FormAction, IDS.Sales.Invoice inv)
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

            inv.OperatorID = currentUser;
            inv.ExchangeRate = IDS.GLTable.ExchangeRate.GetMidRate(inv.CCy.CurrencyCode, IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            inv.Branch = new IDS.GeneralTable.Branch();
            inv.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            inv.IsPPn = inv.TaxStatus;

            if (!string.IsNullOrEmpty(inv.WHTaxCode))
            {
                string[] whtaxArr = inv.WHTaxCode.Split('-');
                inv.WHTaxCode = whtaxArr[0];
                inv.WHTaxPercent = Tool.GeneralHelper.NullToDouble(whtaxArr[1], 0);
            }

            ValidateModel(inv);

            if (ModelState.IsValid)
            {
                string isValid = "";
                if (string.IsNullOrEmpty(inv.Validation()))
                {
                    try
                    {
                        string newSalesInvNo = "";
                        int result = inv.InsUpDelInvoice((int)IDS.Tool.PageActivity.Insert);
                        //Input SSP
                        if (IDS.GeneralTable.Customer.GetCustomer(inv.Cust.CUSTCode).CustType == "1")
                        {
                            if (inv.InvoiceAmount - inv.DiscountAmount + inv.PPnAmount > IDS.Sales.ReceiveSSP.GetBUMNLimit(Server.MapPath("~/BUMNLimit.txt")))
                            {
                                IDS.Sales.ReceiveSSP.SaveSSP((int)IDS.Tool.PageActivity.Insert, inv.Branch.BranchCode, inv.InvoiceNumber, inv.Cust.CUSTCode, inv.OperatorID);
                            }
                        }

                        //
                        if (result > 0)
                        {
                            if (string.IsNullOrWhiteSpace(newSalesInvNo))
                                return Json("New Sales Invoice has been save", JsonRequestBehavior.DenyGet);
                            else
                                return Json(string.Format("New voucher has been save. Sales Invoice No: {0}", newSalesInvNo), JsonRequestBehavior.DenyGet);
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
                    return Json(isValid, JsonRequestBehavior.AllowGet);
                }
                
            }
            else
            {
                return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(string invoiceCollection)
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

            if (string.IsNullOrWhiteSpace(invoiceCollection))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            // Check voucher Posted Status


            try
            {
                string[] eachItem = invoiceCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (eachItem.Length > 0)
                {
                    IDS.Sales.Invoice invoice = new IDS.Sales.Invoice();

                    //// Terdapat voucher posted
                    //if (voucher.CheckPostedVoucherExists(eachItem))
                    //{
                    //    return Json("One or more selected voucher are posted and can not be delete. Delete process canceled by system.");
                    //}

                    invoice.InsUpDel((int)IDS.Tool.PageActivity.Delete, eachItem);
                }

                return Json("Invoice has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string invNo)
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

            IDS.Sales.Invoice invoice = IDS.Sales.Invoice.GetSalesInvoiceWithDetail(invNo);

            List<SelectListItem> listTaxNo = IDS.Sales.TaxNumber.GetTaxNumberForDataSource();
            if (invoice.TaxNumber != null)
            {
                listTaxNo.Add(new SelectListItem() { Value = invoice.TaxNumber.SerialNo, Text = invoice.TaxNumber.SerialNo });
            }
            else
            {
                listTaxNo.Add(new SelectListItem() { Value = "", Text = "" });
            }
            

            ViewData["FormAction"] = 2;

            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",invoice.CCy.CurrencyCode);
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource(invoice.CCy.CurrencyCode,"AR"), "Value", "Text", invoice.Account.Account);
            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text",invoice.Cust.CUSTCode);
            ViewData["SelectListWHTaxPercent"] = new SelectList(IDS.GeneralTable.Tax.GetTaxIDValueForDataSource(), "Value", "Text",invoice.WHTaxCode);
            ViewData["SelectListTaxNo"] = new SelectList(listTaxNo.OrderBy("Value"), "Value", "Text",invoice.TaxNumber.SerialNo);
            ViewData["SelectListTaxTransType"] = new SelectList(IDS.Sales.TaxNumber.GetTransType(), "Value", "Text", invoice.TaxNumber.TaxNoTo);
            ViewData["SelectListTaxType"] = new SelectList(IDS.Sales.TaxNumber.GetTaxType(), "Value", "Text", invoice.TaxNumber.JenisFaktur);

            string NextInvNo = IDS.Sales.Invoice.GetNextInvoiceNumber(Tool.DateTimeExtension.ToPeriod(DateTime.Now), Tool.DateTimeExtension.ToPeriod(DateTime.Now).Substring(0, 4));
            ViewData["lblNextInvNo"] = "Next invoice number : " + NextInvNo;
            ViewData["NextInvoiceNo"] = invoice.InvoiceNumber.Substring(5);

            invoice.InvoiceNumber = invoice.InvoiceNumber.Substring(0, 5);

            ViewData["InvDate"] = invoice.InvoiceDate.ToString("dd/MMM/yyyy");
            ViewData["InvTransDate"] = invoice.InvoiceTransDate.ToString("dd/MMM/yyyy");
            ViewData["InvPayDate"] = invoice.InvoiceTransDate.AddDays(invoice.TermOfPayment).ToString("dd/MMM/yyyy");
            ViewData["ExportStatus"] = IDS.Sales.TaxNumber.CheckStatusExportFaktur(invNo);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtCounter-' + index + '\" name=\"Counter\" onchange=\"OnCounterChange(this)\" onkeyup=\"InputNumber(this)\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 1px !important; width: 60px;\"><input type=\"text\" value=\"' + indexSubSeqAuto + '\" id=\"txtSubCounter-' + index + '\" name=\"SubCounter\" onkeyup=\"InputNumber(this)\" disabled=\"disabled\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtRemark-' + index + '\" name=\"Remark\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"text-align:center; padding: 1px 2px !important; width: 70px;\"><input type=\"checkbox\" id=\"chkTaxInv-' + index + '\" name=\"TaxInvoice\" style=\"padding: 1px; width: 100%; border: none;\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtSubAmt-' + index + '\" name=\"SubAmount\" class=\"text-right\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmt-' + index + '\" name=\"Amount\" class=\"text-right\" style=\"width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" onchange=\"OnAmountChange(this)\"/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnRemove-' + index + '\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            if (invoice != null)
            {
                return PartialView("Edit", invoice);
            }
            else
            {
                return PartialView("Edit", new IDS.Sales.Invoice());
            }
        }

        [HttpPost]
        public ActionResult Edit(int? FormAction, IDS.Sales.Invoice inv)
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

            inv.OperatorID = currentUser;
            inv.ExchangeRate = IDS.GLTable.ExchangeRate.GetMidRate(inv.CCy.CurrencyCode, "IDR", inv.InvoiceDate);

            inv.Branch = new IDS.GeneralTable.Branch();
            inv.Branch.BranchCode = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            inv.IsPPn = inv.TaxStatus;

            if (!string.IsNullOrEmpty(inv.WHTaxCode))
            {
                string[] whtaxArr = inv.WHTaxCode.Split('-');
                inv.WHTaxCode = whtaxArr[0];
                inv.WHTaxPercent = Tool.GeneralHelper.NullToDouble(whtaxArr[1], 0);
            }

            ValidateModel(inv);

            if (ModelState.IsValid)
            {
                try
                {
                    string newVoucherNo = "";
                    int result = inv.InsUpDelInvoice((int)IDS.Tool.PageActivity.Edit);

                    //Input SSP
                    
                    if (IDS.GeneralTable.Customer.GetCustomer(inv.Cust.CUSTCode).CustType == "1")
                    {
                        if (inv.InvoiceAmount - inv.DiscountAmount + inv.PPnAmount > IDS.Sales.ReceiveSSP.GetBUMNLimit(Server.MapPath("~/BUMNLimit.txt")))
                        {
                            IDS.Sales.ReceiveSSP.SaveSSP((int)IDS.Tool.PageActivity.Edit, inv.Branch.BranchCode, inv.InvoiceNumber, inv.Cust.CUSTCode, inv.OperatorID);
                        }
                    }
                    
                    //

                    if (result > 0)
                    {
                        if (string.IsNullOrWhiteSpace(newVoucherNo))
                            return Json("Edit Sales Invoice has been save", JsonRequestBehavior.DenyGet);
                        else
                            return Json(string.Format("Edit voucher has been save. Voucher No: {0}", newVoucherNo), JsonRequestBehavior.DenyGet);
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

        public FileResult ExportFaktur(string invoiceCollection)
        {
            Tool.ExportToCSV csv = new Tool.ExportToCSV(System.Web.HttpContext.Current.Response);
            System.Text.StringBuilder _sb = new System.Text.StringBuilder();
            _sb.Append("\"").Append(string.Join("\",\"", new string[] { "FK", "KD_JENIS_TRANSAKSI", "FG_PENGGANTI", "NOMOR_FAKTUR", "MASA_PAJAK", "TAHUN_PAJAK", "TANGGAL_FAKTUR", "NPWP", "NAMA", "ALAMAT_LENGKAP", "JUMLAH_DPP", "JUMLAH_PPN", "JUMLAH_PPNBM", "ID_KETERANGAN_TAMBAHAN", "FG_UANG_MUKA", "UANG_MUKA_DPP", "UANG_MUKA_PPN", "UANG_MUKA_PPNBM", "REFERENSI", "KODE_DOKUMEN_PENDUKUNG" }.ToArray())).Append("\"\n");
            _sb.Append("\"").Append(string.Join("\",\"", new string[] { "LT", "NPWP", "NAMA", "JALAN", "BLOK", "NOMOR", "RT", "RW", "KECAMATAN", "KELURAHAN", "KABUPATEN", "PROPINSI", "KODE_POS", "NOMOR_TELEPON" }.ToArray())).Append("\"\n");
            _sb.Append("\"").Append(string.Join("\",\"", new string[] { "OF", "KODE_OBJEK", "NAMA", "HARGA_SATUAN", "JUMLAH_BARANG", "HARGA_TOTAL", "DISKON", "DPP", "PPN", "TARIF_PPNBM", "PPNBM" }.ToArray())).Append("\"\n");
            
            string[] eachItem = invoiceCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            eachItem = IDS.Sales.TaxNumber.CheckExportFaktur(eachItem,0);

            for (int i = 0; i < eachItem.Length; i++)
            {
                Tool.ExportToCSV.setData(IDS.Sales.Invoice.GetInvoiceHExportCSV(eachItem[i]), _sb);
                Tool.ExportToCSV.setData(IDS.Sales.Invoice.GetSysparExportCSV(), _sb);
                Tool.ExportToCSV.setData(IDS.Sales.Invoice.GetInvoiceDExportCSV(eachItem[i]), _sb);

                IDS.Sales.TaxNumber.UpdateStatusExport(eachItem[i]);
            }

            return File(System.Text.Encoding.ASCII.GetBytes(_sb.ToString()), "text/csv", "TaxIN"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
        }

        public JsonResult CheckInvoiceNo(string invNo)
        {
            string msgResult = "";
            if (IDS.Sales.Invoice.CheckInvNo(invNo))
            {
                msgResult = "Data with that invoice number is already exist.";
            }

            return Json(msgResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceForDataSource(string Branch,string Cust,string period)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.Sales.Invoice.GetInvoiceNoForDataSource(Branch, Cust, period);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceNoForDataSource(string Branch, string Cust, string period, bool withAll)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.Sales.Invoice.GetInvoiceNoForDataSource(Branch, Cust, period,withAll);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceForDataSources(int type, string branch, DateTime docdate, string custPrin, string[] docNo)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.Sales.Invoice.GetInvoiceNoForDataSource(type, branch, docdate, custPrin);

            if (/*docNo.Length > 0 && */docNo != null)
            {
                for (int i = 0; i < docNo.Length; i++)
                {
                    list = list.Where(x => x.Value != docNo[i]).ToList();
                }
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNextInvoiceNumber(DateTime period)
        {
            string NextInvNo = IDS.Sales.Invoice.GetNextInvoiceNumber(Tool.DateTimeExtension.ToPeriod(period), Tool.DateTimeExtension.ToPeriod(period).Substring(0, 4));
            //string result = "Next invoice number : " + NextInvNo;
            //ViewData["NextInvNumber"] = NextInvNo.Substring(0, 5);
            return Json(NextInvNo, JsonRequestBehavior.AllowGet);
        }
        

        //[HttpGet]
        //public ActionResult CreateDetail()
        //{
        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

        //    IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
        //    {
        //        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        //        return RedirectToAction("error403", "error", new { area = "" });
        //    }

        //    ViewData["Page.Insert"] = AccessLevel.CreateAccess;
        //    ViewData["Page.Edit"] = AccessLevel.EditAccess;
        //    ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

        //    ViewData["FormAction"] = 1;

        //    ViewBag.UserMenu = MainMenu;

        //    return View("CreateDetail", new IDS.Sales.InvoiceDetail());
        //}
    }
}