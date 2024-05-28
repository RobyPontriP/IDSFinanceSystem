using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class PaymentController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string serialNo, string reffNo, string type, string dateFrom, string dateTo, int chkByDate, string vendor, string flag)
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

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search
                //
                List<IDS.Sales.PaymentH> paymentH = IDS.Sales.PaymentH.GetPaymentH(serialNo,reffNo,type,Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo),chkByDate,vendor,flag,0);
                //
                totalRecords = paymentH.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            paymentH = paymentH.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    paymentH = paymentH.Where(x => x.SerialNo.ToLower().Contains(searchValueLower) ||
                                             x.ReffNo.ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT.ToLower()).Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = paymentH.Count();

                // Paging
                paymentH = paymentH.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = paymentH }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: Sales/Payment
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

            ViewData["VendorList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text");
            ViewData["TypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text");

            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

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

            ViewData["FormAction"] = 1;

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Text");
            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource(), "Value", "Text");
            ViewData["CcyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["PayTypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text", 0);
            ViewData["CustList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text");
            ViewData["SpaccList"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCWithCcyForDataSource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy, 0), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            List<SelectListItem> alloTypeList = IDS.Sales.PaymentD.GetAllocationTypeForDataSource().Where(x => x.Value != "5").ToList();
            ViewData["alloTypeList"] = new SelectList(alloTypeList, "Value", "Text", ((int)IDS.Tool.AllocationType.Receivable).ToString().ToString());

            //List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();


            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<tr id=\"r' + index + '\">")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtSeqNo-' + index + '\" name=\"SeqNo\" style=\"padding: 3px; width: 30px; border: none;\" disabled=\"disabled\" /></td>")
                //.Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" class=\"select2DDList\" style=\"padding: 3px; border: 0; max-width: 120px !important;\">")
                .Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboAlloType-' + index + '\" name=\"AlloType\" onchange=\"AlloTypeChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\">");
            foreach (SelectListItem item in alloTypeList)
            {
                //if (item.Value == IDS.GeneralTable.Syspar.GetInstance().BaseCCy)
                //    sb.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                //else
                sb.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");

            }
            //sb.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtInvNo-' + index + '\" name=\"InvNo\" style=\"padding: 1px; width: 80px; border: none;\" /></td>");
            sb.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"cboInvNo-' + index + '\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" hidden> <input type=\"text\" id=\"txtInvNo-' + index + '\" name=\"TxtInvNo\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\" hidden/>");
            //sb.Append("<td style=\"padding: 1px 2px !important; width: 105; display:none;\"><input type=\"text\" id=\"txtInvDate-' + index + '\" name=\"InvDate\" style=\"padding: 1px; width:105px; border: none;\" hidden/></td>");
            sb.Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-' + index + '\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\">")
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
                .Append("<td style=\"padding: 1px 2px !important; width: 250px;\"><select id=\"cboAcc-' + index + '\" name=\"Acc\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 250px !important;\" class=\"select2DDList\">")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" id=\"txtInvDate-' + index + '\" name=\"InvDate\" style=\"padding: 1px; width:70px; border: none;\" class=\"mt-1\" disabled/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAmount-' + index + '\" name=\"Amount\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtOutstanding-' + index + '\" name=\"Outstanding\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtAlloAmount-' + index + '\" onchange=\"AlloAmtChange(this)\" name=\"AlloAmount\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 60px;\"><input type=\"text\" id=\"txtExchRate-' + index + '\" name=\"ExchRate\" class=\"text-right mt-1\" style=\"max-width: 60px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtEquivAmt-' + index + '\" name=\"EquivAmt\" onchange=\"EquivAmtChange(this)\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" id=\"txtOutstandingTemp-' + index + '\" name=\"OutstandingTemp\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"8\" hidden disabled/></td>");
            //foreach (SelectListItem item in deptList)
            //{
            //    sb.Append("<option value=\"" + item.Value + "\" title=\"" + item.Text + "\">" + item.Value + "</option>");
            //}

            sb.Append("</select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnEdit-' + index + '\" name=\"btnEdit\" class=\"fa fa-edit\" onclick=\"EditRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>");

            sb.Append("</select></td>")
                .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                .Append("</tr>");

            ViewData["NewRowTemplate"] = sb.ToString();
            ViewData["CurDate"] = DateTime.Now.ToString("dd/MMM/yyyy");
            return View("Create", new IDS.Sales.PaymentH() { PayDate = DateTime.Now.Date, SerialNo = "AUTO", ChequeDate = DateTime.Now });
        }

        // POST: Sales/Payment/Create
        [HttpPost]
        public ActionResult Create(int? FormAction, IDS.Sales.PaymentH payment)
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

            if (payment.SCode == null || string.IsNullOrEmpty(payment.SCode))
            {
                valid = false;
                sb.Append("- Source code is required").Append(Environment.NewLine);
            }

            if (payment.Branch?.BranchCode == null || string.IsNullOrEmpty(payment.Branch?.BranchCode))
            {
                valid = false;
                sb.Append("- Branch is required").Append(Environment.NewLine);
            }

            if (payment.PayDate == null || payment.PayDate == DateTime.MinValue)
            {
                valid = false;
                sb.Append("- Payment date is required or Payment date is invalid.").Append(Environment.NewLine);
            }

            if (payment.Detail == null || payment.Detail.Count == 0)
            {
                valid = false;
                sb.Append("Can not save voucher without Detail Item.");
            }
            else
            {
                //decimal difference = payment.Detail.Sum(x => x.EquivAmount);

                //if (difference != 0)
                //{
                //    valid = false;
                //    sb.Append("Journal entry is not balance");
                //}

                for (int i = 0; i < payment.Detail.Count; i++)
                {
                    if (payment.Detail[i].Ccy?.CurrencyCode == null || string.IsNullOrEmpty(payment.Detail[0].Ccy?.CurrencyCode))
                    {
                        valid = false;
                        sb.Append("- One of Detail item currency is not set or empty.").Append(Environment.NewLine);
                    }

                    if (payment.Detail[i].Acc?.Account == null || string.IsNullOrEmpty(payment.Detail[0].Acc.Account))
                    {
                        valid = false;
                        sb.Append("- One of Detail item Chart of Account is not set or empty.").Append(Environment.NewLine);
                    }
                    //else
                    //{
                    //    if (voucher.Detail[i].CCy?.CurrencyCode != null && !string.IsNullOrEmpty(voucher.Detail[i].CCy?.CurrencyCode) &&
                    //        voucher.Detail[i].COA?.Account != null && !string.IsNullOrEmpty(voucher.Detail[i].COA?.Account))
                    //    {
                    //        bool isCashAccount = IDS.GLTable.ChartOfAccount.IsCashAccount(voucher.Detail[i].CCy.CurrencyCode, voucher.Detail[i].COA.Account);

                    //        if (isCashAccount && string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                    //        {
                    //            valid = false;
                    //            sb.Append("- One of journal item cash account is not set while chart of account is cash account").Append(Environment.NewLine);
                    //        }
                    //        else if (!isCashAccount && !string.IsNullOrEmpty(voucher.Detail[i].CashAccount))
                    //        {
                    //            valid = false;
                    //            sb.Append("- One of journal item cash account is set while chart of account is not cash account").Append(Environment.NewLine);
                    //        }
                    //    }
                    //}

                    if (payment.Detail[i].EquivAmount == 0)
                    {
                        valid = false;
                        sb.Append("- One of Detail item amount is not set or zero").Append(Environment.NewLine);
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

            payment.OperatorID = currentUser;
            payment.LastUpdate = DateTime.Now;

            payment.Bank = new IDS.GeneralTable.Bank();
            payment.Bank.BankCode = IDS.GeneralTable.Bank.GetBankCodeWithAcc(payment.Account.Account);

            payment.PayMethod = payment.PaymentType == "0" || payment.PaymentType == "3" ? false : true;

            // TODO: Sesuaikan Account aslinya dari Autocomplete
            foreach (IDS.Sales.PaymentD d in payment.Detail)
            {
                String[] separator = { " - " };
                string[] split = d.Acc.Account.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                d.Acc.Account = split[0];
            }

            ValidateModel(payment);

            if (ModelState.IsValid)
            {
                try
                {
                    string newSerialNo = "";
                    //int result = payment.InsUpDel((int)IDS.Tool.PageActivity.Insert, ref newSerialNo);
                    int result = payment.InsUpDelPayment((int)FormAction, ref newSerialNo);

                    if (result > 0)
                    {
                        if ((int)FormAction == 1)
                        {
                            if (string.IsNullOrWhiteSpace(newSerialNo))
                                return Json("New Payment has been save", JsonRequestBehavior.DenyGet);
                            else
                                return Json(new { msg = "New Payment has been save. Serial No: " + newSerialNo, sno = newSerialNo }, JsonRequestBehavior.AllowGet);
                            //return Json(string.Format("New voucher has been save. Voucher No: {0}", newVoucherNo), JsonRequestBehavior.DenyGet);
                        }
                        else
                        {
                            return Json(new { msg = "Edit Payment has been save." }, JsonRequestBehavior.AllowGet);
                        }

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
    }
}