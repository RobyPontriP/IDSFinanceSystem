using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class ReceiveController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string serialNo, string reffNo, string type, string dateFrom, string dateTo, int chkByDate, string cust, string flag)
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
                List<IDS.Sales.PaymentH> paymentH = IDS.Sales.PaymentH.GetPaymentH(serialNo, reffNo, type, Convert.ToDateTime(dateFrom), Convert.ToDateTime(dateTo), chkByDate, cust, flag,1);
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

                paymentH = paymentH.OrderByDescending(x => x.PayDate).ToList();

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
                if (pageSize > 0)
                    paymentH = paymentH.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = paymentH }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string serialNo)
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
                List<IDS.Sales.PaymentD> paymentD = IDS.Sales.PaymentD.GetPaymentDetail(serialNo, 0);
                //
                totalRecords = paymentD.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            paymentD = paymentD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    paymentD = paymentD.Where(x => x.SerialNo.ToLower().Contains(searchValueLower) ||
                                             x.Type.ToLower().Contains(searchValueLower) ||
                                             x.Acc.Account.ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = paymentD.Count();

                // Paging
                if (pageSize > 0)
                    paymentD = paymentD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = paymentD }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: Sales/Receive
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

            ViewData["CustList"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["TypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text");
            //ViewData["TypeList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.PaymentType>(Tool.PaymentType.Cash, true,"0");
            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

            return View();
        }

        //[HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        ////[ValidateAntiForgeryToken]
        //public ActionResult Index(string serialNoList,int init)
        //{
        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

        //    IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
        //    {
        //        RedirectToAction("Index", "Main", new { Area = "" });
        //    }

        //    ViewData["Page.Insert"] = AccessLevel.CreateAccess;
        //    ViewData["Page.Edit"] = AccessLevel.EditAccess;
        //    ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

        //    try
        //    {
        //        string[] serialNo = serialNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        //        string msgError = "";
        //        for (int i = 0; i < serialNo.Length; i++)
        //        {
        //            IDS.Sales.PaymentH payH = new IDS.Sales.PaymentH();
        //            if (init == 1)
        //            {
        //                msgError = payH.ProcessJournal(serialNo, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] as string);
        //            }
        //            else
        //            {
        //                msgError = payH.ReverseJournal(serialNo, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] as string);
        //            }
                    
        //        }

        //        ViewData["MessageError"] = msgError;
        //    }
        //    catch
        //    {

        //        throw;
        //    }

        //    ViewData["CustList"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
        //    ViewData["TypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text");
        //    //ViewData["TypeList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.PaymentType>(Tool.PaymentType.Cash, true,"0");
        //    DateTime now = DateTime.Now;
        //    ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
        //    ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

        //    ViewBag.UserMenu = MainMenu;


        //    return View();
        //}

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

            //ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource()
            //    .Where(x => x.Value != "AR" && x.Value != "AP")
            //    .ToList(), "Value", "Text");

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
            ViewData["PayTypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text",0);
            ViewData["CustList"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["SpaccList"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCWithCcyForDataSource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy,0), "Value", "Text");
            
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
            return View("Create", new IDS.Sales.PaymentH() { PayDate = DateTime.Now.Date, SerialNo = "AUTO",ChequeDate=DateTime.Now });
            //return View("Create", new IDS.Sales.PaymentH() { TransDate = DateTime.Now.Date, Voucher = "AUTO" });
        }

        public ActionResult Edit(string serialNo)
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

            IDS.Sales.PaymentH paymentH = IDS.Sales.PaymentH.GetSalesPaymentHWithDetail(serialNo);

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(paymentH.Branch.BranchCode));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(paymentH.Branch.BranchCode)), "Value", "Text", Tool.GeneralHelper.NullToString(paymentH.Branch.BranchCode));
            }

            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Text");
            ViewData["SCodeList"] = new SelectList(IDS.GLTable.SourceCode.GetSourceCodeForDataSource(), "Value", "Text", paymentH.SCode);
            ViewData["CcyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", paymentH.Ccy.CurrencyCode);
            ViewData["PayTypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text", paymentH.PaymentType);
            ViewData["CustList"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text",paymentH.Customer.CUSTCode);
            ViewData["SpaccList"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCWithCcyForDataSource(paymentH.Ccy.CurrencyCode, Convert.ToInt16(paymentH.PaymentType)), "Value", "Text",paymentH.Account.Account);
            ViewData["Flag"] = paymentH.Flag;
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text", paymentH.Ccy.CurrencyCode);

            List<SelectListItem> alloTypeList = IDS.Sales.PaymentD.GetAllocationTypeForDataSource().Where(x => x.Value != "5").ToList();
            ViewData["alloTypeList"] = new SelectList(alloTypeList, "Value", "Text", ((int)IDS.Tool.AllocationType.Receivable).ToString().ToString());

            //List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();
            ViewData["InvNoList"] = new SelectList(new SelectList(new List<SelectListItem>(), "Value", "Text"));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sbEditTemplate = new System.Text.StringBuilder();

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


            #region EditTemplateHTML
            for (int i = 0; i < paymentH.Detail.Count; i++)
            {
                sbEditTemplate.Append("<tr id=\"" + "r"+i + "\">")
                //.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"' + indexSeqAuto + '\" id=\"txtSeqNo-' + index + '\" name=\"SeqNo\" style=\"padding: 3px; width: 30px; border: none;\" disabled=\"disabled\" /></td>")
                .Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"" + paymentH.Detail[i].SeqNo.ToString() + "\" id=\"txtSeqNo-" + i + "\" name=\"SeqNo\" style=\"padding: 3px; width: 30px; border: none;\" disabled=\"disabled\" /></td>")
                .Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboAlloType-" + i + "\" name=\"AlloType\" onchange=\"AlloTypeChange(this)\" style=\"padding: 3px; border: 0;\" class=\"select2DDList\" disabled>");

                foreach (SelectListItem item in alloTypeList)
                {
                    if (item.Value == paymentH.Detail[i].AlloType.ToString())
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                    else
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
                }

                if (paymentH.Detail[i].AlloType != ((int)IDS.Tool.AllocationType.Others))
                {
                    if (paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.BankCharges))
                    {
                        //sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"" + "cboInvNo-" + i + "\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" disabled>");
                        sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"" + "cboInvNo-" + i + "\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" disabled> <input type=\"text\" id=\"txtInvNo-" + i + "\" name=\"TxtInvNo\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\" hidden/>");
                    }
                    else
                    {
                        //sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"" + "cboInvNo-" + i + "\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" disabled>");
                        sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"" + "cboInvNo-" + i + "\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" disabled> <option value=\"" + paymentH.Detail[i].InvNo + "\" selected>" + paymentH.Detail[i].InvNo + "</option> <input type=\"text\" id=\"txtInvNo-" + i + "\" name=\"TxtInvNo\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\" hidden/>");
                        //sbEditTemplate.Append("<option value=\"" + paymentH.Detail[i].InvNo + "\" selected>" + paymentH.Detail[i].InvNo + "</option>");
                        //sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><input type=\"text\" id=\"txtInvNo-" + i + "\" name=\"TxtInvNo\" value=\"" + paymentH.Detail[i].InvNo + "\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\"/ hidden></td>");
                    }
                }
                else
                {
                    //sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><input type=\"text\" id=\"txtInvNo-" + i + "\" name=\"TxtInvNo\" value=\"" + paymentH.Detail[i].InvNo + "\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\"/></td>");
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 130;\"><select id=\"" + "cboInvNo-" + i + "\" name=\"InvNo\" onchange=\"InvNoChange(this)\" style=\"padding: 3px; width:130px; border: 0; max-width: 130px !important;\" class=\"select2DDList\" hidden> <input type=\"text\" id=\"txtInvNo-" + i + "\" name=\"TxtInvNo\" value=\"" + paymentH.Detail[i].InvNo + "\" style=\"padding: 1px; width:130px; border: none;\" class=\"mt-1\"/>");
                }

                if (paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.BankCharges) || paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.Others))
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-" + i + "\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; width:100%;\" class=\"select2DDList\">");
                }
                else
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px; width: 70px;\"><select id=\"cboCCy-" + i + "\" name=\"CCy.Currency\" onchange=\"CCyChange(this)\" style=\"padding: 3px; border: 0; width:100%;\" class=\"select2DDList\" disabled>");
                }


                sbEditTemplate.Append("<option value=\"\">--CCy--</option>");

                foreach (SelectListItem item in ccyList)
                {
                    if (item.Value == paymentH.Detail[i].Ccy.CurrencyCode)
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                    else
                        sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");

                }

                sbEditTemplate.Append("</select></td>");
                    
                if (paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.BankCharges) || paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.Others))
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 250px;\"><select id=\"cboAcc-" + i + "\" name=\"Acc\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 250px !important;\" class=\"select2DDList\">");
                }
                else
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 250px;\"><select id=\"cboAcc-" + i + "\" name=\"Acc\" onchange=\"AccChange(this)\" style=\"padding: 3px; width:290px; border: 0; max-width: 250px !important;\" class=\"select2DDList\" disabled>");
                }

                //.Append("<option value=\"\">--CCy--</option>");
                #region Untuk coa detail
                sbEditTemplate.Append("<option value=\"" + paymentH.Detail[i].Acc.Account + "\" selected>" + paymentH.Detail[i].Acc.Account +" - " + paymentH.Detail[i].Acc.AccountName + "</option>");
                //List<SelectListItem> accList = new List<SelectListItem>();
                //if (paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.Receivable))
                //{
                //    accList = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(paymentH.Detail[i].Ccy.CurrencyCode);
                //}
                //else if (paymentH.Detail[i].AlloType == ((int)IDS.Tool.AllocationType.Tax))
                //{
                //    accList = IDS.GLTable.ChartOfAccount.GetCOAForSalesDataSource("", "PREPAID");
                //}

                //if (accList.Count > 0)
                //{
                //    foreach (SelectListItem item in accList)
                //    {
                //        if (item.Value == paymentH.Detail[i].Acc.Account)
                //            sbEditTemplate.Append("<option value=\"" + item.Value + "\" selected>" + item.Text + "</option>");
                //        else
                //            sbEditTemplate.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");

                //    }
                //}
                #endregion

                if (paymentH.Detail[i].Invoice.InvoiceDate.ToString("dd/MMM/yyyy") == "01/Jan/0001")
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"\" id=\"txtInvDate-" + i + "\" name=\"InvDate\" style=\"padding: 1px; width:70px; border: none;\" class=\"mt-1\" disabled/></td>");
                }
                else
                {
                    sbEditTemplate.Append("<td style=\"padding: 1px 2px !important;\"><input type=\"text\" value=\"" + paymentH.Detail[i].Invoice.InvoiceDate.ToString("dd/MMM/yyyy") + "\" id=\"txtInvDate-" + i + "\" name=\"InvDate\" style=\"padding: 1px; width:70px; border: none;\" class=\"mt-1\" disabled/></td>");
                }

                sbEditTemplate.Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", paymentH.Detail[i].Amount) + "\" id=\"txtAmount-" + i + "\" name=\"Amount\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", paymentH.Detail[i].Invoice.OutstandingAmount) + "\" id=\"txtOutstanding-" + i + "\" name=\"Outstanding\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", paymentH.Detail[i].AlloAmount) + "\" id=\"txtAlloAmount-" + i + "\" onchange=\"AlloAmtChange(this)\" name=\"AlloAmount\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" /></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 60px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", paymentH.Detail[i].ExchRate) + "\" id=\"txtExchRate-" + i + "\" name=\"ExchRate\" class=\"text-right mt-1\" style=\"max-width: 60px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + String.Format("{0:n0}", paymentH.Detail[i].EquivAmount) + "\" id=\"txtEquivAmt-" + i + "\" name=\"EquivAmt\" onchange=\"EquivAmtChange(this)\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"0\" disabled/></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 120px;\"><input type=\"text\" value=\"" + paymentH.Detail[i].Invoice.OutstandingAmount + "\" id=\"txtOutstandingTemp-" + i + "\" name=\"OutstandingTemp\" class=\"text-right mt-1\" style=\"max-width: 120px !important; border: none;\" onkeyup=\"InputNumber(this)\" value=\"8\" hidden disabled/></td>");
                sbEditTemplate.Append("</select></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" id=\"btnEdit-" + i + "\" name=\"btnEdit\" class=\"fa fa-edit\" onclick=\"EditRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>");

                sbEditTemplate.Append("</select></td>")
                    .Append("<td style=\"padding: 1px 2px !important; width: 26px;\"><button type=\"button\" name=\"btnRemove\" class=\"fa fa-trash\" onclick=\"RemoveRow(this)\" style=\"width: 26px !important; height: 23px;\"></i></td>")
                    .Append("</tr>");
            }
            //
            //sbEditTemplate.Append("<tr id=\"AddLine\">");
            //sbEditTemplate.Append("<td colspan=\"12\">");
            //sbEditTemplate.Append("<a href=\"#\" role=\"button\" id=\"linkNewRow\">Add New Row</a>");
            //sbEditTemplate.Append("</td>");
            //sbEditTemplate.Append("</tr>");
            #endregion

            ViewData["NewRowTemplate"] = sb.ToString();
            ViewData["EditRowTemplate"] = sbEditTemplate.ToString();
            ViewData["CurDate"] = DateTime.Now.ToString("dd/MMM/yyyy");
            return View("Create", paymentH);
        }

        // POST: Sales/Receive/Create
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
                    int result = payment.InsUpDel((int)FormAction, ref newSerialNo);

                    if (result > 0)
                    {
                        if ((int)FormAction == 1)
                        {
                            if (string.IsNullOrWhiteSpace(newSerialNo))
                                return Json("New Receive has been save", JsonRequestBehavior.DenyGet);
                            else
                                return Json(new { msg = "New Receive has been save. Serial No: " + newSerialNo, sno = newSerialNo }, JsonRequestBehavior.AllowGet);
                            //return Json(string.Format("New voucher has been save. Voucher No: {0}", newVoucherNo), JsonRequestBehavior.DenyGet);
                        }
                        else
                        {
                            return Json(new { msg = "Edit Receive has been save."}, JsonRequestBehavior.AllowGet);
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

        public ActionResult Delete(string receiveCollection)
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

            if (string.IsNullOrWhiteSpace(receiveCollection))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            // Check voucher Posted Status


            try
            {
                string[] eachItem = receiveCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (eachItem.Length > 0)
                {
                    IDS.Sales.PaymentH payment = new IDS.Sales.PaymentH();

                    //// Terdapat voucher posted

                    payment.InsUpDel((int)IDS.Tool.PageActivity.Delete, eachItem);
                }

                return Json("Receive has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult Process(string serialNoList, int init)
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
                string[] serialNo = serialNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string msgError = "";
                for (int i = 0; i < serialNo.Length; i++)
                {
                    IDS.Sales.PaymentH payH = new IDS.Sales.PaymentH();
                    if (init == 1)
                    {
                        msgError = payH.ProcessJournal(serialNo, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] as string);
                    }
                    else
                    {
                        msgError = payH.ReverseJournal(serialNo, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string, Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] as string);
                    }

                }

                return Json(msgError, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
    }
}