using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class VoidInvoiceController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string view,bool chkFilterDate,string cust,string inv,DateTime from, DateTime to)
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

                List<IDS.Sales.Invoice> invH = new List<IDS.Sales.Invoice>();

                if (view == "4")
                {
                    invH = IDS.Sales.Invoice.GetSalesVoidInvoiceExportFaktur(chkFilterDate, cust, inv, from, to);
                }
                else
                {
                    invH = IDS.Sales.Invoice.GetSalesVoidInvoice(chkFilterDate, cust, inv, from, to);
                    invH = invH.Where(x => x.StatusInv.ToString().Contains(view)).ToList();
                }
                //invH = invH.OrderByDescending(x => x.InvoiceDate).ToList();

                totalRecords = invH.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            invH = invH.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    invH = invH.Where(x => x.InvoiceNumber.ToString().ToLower().Contains(searchValueLower) ||
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

                totalRecordsShowing = invH.Count();

                // Paging
                invH = invH.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = invH }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
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
                List<IDS.Sales.InvoiceDetail> invDetail = IDS.Sales.InvoiceDetail.GetInvoiceDetail(invNo);
                //
                totalRecords = invDetail.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            invDetail = invDetail.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    invDetail = invDetail.Where(x => x.InvoiceNumber.ToLower().Contains(searchValueLower) ||
                                             x.Remark.ToLower().Contains(searchValueLower) ||
                                             x.Amount.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = invDetail.Count();

                // Paging
                if (pageSize > 0)
                    invDetail = invDetail.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = invDetail }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataNewVoid(string cust, string inv)
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

                List<IDS.Sales.Invoice> invH = IDS.Sales.Invoice.GetNewVoidInvoice(cust,inv);
                
                totalRecords = invH.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            invH = invH.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    invH = invH.Where(x => x.InvoiceNumber.ToString().ToLower().Contains(searchValueLower) ||
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

                totalRecordsShowing = invH.Count();

                // Paging
                invH = invH.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = invH }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        // GET: Sales/VoidInvoice
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
            ViewData["InvNoList"] = new SelectList(IDS.Sales.Invoice.GetInvoiceNoForDataSource(""), "Value", "Text");
            //ViewData["TypeList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.PaymentType>(Tool.PaymentType.Cash, true,"0");
            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

            return View();
        }

        public ActionResult ProcessVoidInvoice(string invNoList, string invDateList, string custList)
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

            if (string.IsNullOrWhiteSpace(invNoList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] invNo = invNoList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] invDate = invDateList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] cust = custList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (invNo.Length > 0)
                {
                    IDS.Sales.Invoice.ProcessVoid(invNo, invDate, cust, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
                }

                return Json("Selected invoices has been void.", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult ExportFaktur(string invoiceCollection)
        {
            Tool.ExportToCSV csv = new Tool.ExportToCSV(System.Web.HttpContext.Current.Response);
            System.Text.StringBuilder _sb = new System.Text.StringBuilder();
            _sb.Append("\"").Append(string.Join("\",\"", new string[] { "RK", "NPWP", "NAMA", "KD_JENIS_TRANSAKSI", "FG_PENGGANTI", "NOMOR_FAKTUR", "TANGGAL_FAKTUR", "NOMOR_DOKUMEN_RETUR", "TANGGAL_RETUR", "MASA_PAJAK_RETUR", "TAHUN_PAJAK_RETUR", "NILAI_RETUR_DPP", "NILAI_RETUR_PPN", "NILAI_RETUR_PPNBM" }.ToArray())).Append("\"\n");

            string[] eachItem = invoiceCollection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            eachItem = IDS.Sales.TaxNumber.CheckExportFaktur(eachItem,1);

            for (int i = 0; i < eachItem.Length; i++)
            {
                Tool.ExportToCSV.setData(IDS.Sales.Invoice.GetVoidInvoiceHExportCSV(eachItem[i]), _sb);

                IDS.Sales.TaxNumber.UpdateStatusExport(eachItem[i]);
            }

            return File(System.Text.Encoding.ASCII.GetBytes(_sb.ToString()), "text/csv", "TaxOut" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
        }

        public JsonResult GetInvoice(string cust)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.Sales.Invoice.GetInvoiceNoForDataSource(cust);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoiceNewVoid(string cust)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.Sales.Invoice.GetInvoiceVoidNoForDataSource(cust);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}