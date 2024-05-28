using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class InvoiceController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.Sales.Invoice> inv = IDS.Sales.Invoice.GetInvoice(period);

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

            List<IDS.Sales.InvoiceDetail> invDetail = IDS.Sales.InvoiceDetail.GetInvoiceDetail(invNo);

            result = this.Json(new { data = invDetail }, JsonRequestBehavior.AllowGet);
            return result;
        }
        // GET: Sales/Invoice
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

        public ActionResult RedirectToAspx(string invNo)
        {
            string url = "~/Report/Sales/wfRptInvoice.aspx?invNo=" + invNo;
            return Redirect(url);
        }

        public ActionResult RedirectToRptTaxInvoice(string invNo)
        {
            string url = "~/Report/Sales/wfRptTaxInvoice.aspx?invNo=" + invNo;
            //string url = "~/Report/Sales/wfRptTaxInvoice.aspx";
            return Redirect(url);
        }

        public ActionResult RedirectToRptTaxInvoiceUpdate(string invNo, string ppnNo)
        {
            string MsgResult = "";
            IDS.Sales.Invoice inv = new IDS.Sales.Invoice();
            string invPPnNo = inv.CheckPPnNo(ppnNo);
            if (string.IsNullOrEmpty(invPPnNo))
            {
                inv.UpdatePPnNo(invNo, ppnNo);
            }
            else
            {
                MsgResult = "Tax invoice has been use for invoice: " + invPPnNo;
            }

            return Json(MsgResult, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInvoice(string invNo, DateTime payDate, string cust)
        {
            List<IDS.Sales.Invoice> list = new List<IDS.Sales.Invoice>();
            list = IDS.Sales.Invoice.GetInvoice(invNo, payDate, cust);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxInvoice(string invNo, DateTime payDate, string cust)
        {
            List<IDS.Sales.Invoice> list = new List<IDS.Sales.Invoice>();
            list = IDS.Sales.Invoice.GetTaxInvoice(invNo, payDate, cust);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditPPN(string invNo)
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
            listTaxNo.Add(new SelectListItem() { Value = invoice.TaxNumber.SerialNo, Text = invoice.TaxNumber.SerialNo });
            ViewData["SelectListTaxNo"] = new SelectList(listTaxNo.OrderBy("Value"), "Value", "Text", invoice.TaxNumber.SerialNo);
            ViewData["SelectListTaxTransType"] = new SelectList(IDS.Sales.TaxNumber.GetTransType(), "Value", "Text", invoice.TaxNumber.TaxNoTo);
            ViewData["SelectListTaxType"] = new SelectList(IDS.Sales.TaxNumber.GetTaxType(), "Value", "Text", invoice.TaxNumber.JenisFaktur);

            ViewData["ExportStatus"] = IDS.Sales.TaxNumber.CheckStatusExportFaktur(invNo);


            ViewData["FormAction"] = 2;

            if (invoice != null)
            {
                return PartialView("EditPPN", invoice);
            }
            else
            {
                return PartialView("EditPPN", new IDS.Sales.Invoice());
            }
        }

        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult EditPPN(IDS.Sales.Invoice inv)
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

            string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
            }

            try
            {
                inv.TaxNumber.OperatorID = currentUser;
                inv.TaxNumber.UpdateTaxNumber(inv.InvoiceNumber);

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            //    ModelState.Clear();

            //    ValidateModel(inv);

            //    if (ModelState.IsValid)
            //    {

            //    }
            //    else
            //    {
            //        return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
            //    }
            //}
        }
    }
}