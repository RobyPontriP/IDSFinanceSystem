using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class PayableTaxController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string custCode,string taxType,string period)
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

                List<IDS.Sales.PayableTax> payableTax = IDS.Sales.PayableTax.GetPayableTax(custCode,period,taxType);

                totalRecords = payableTax.Count;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            payableTax = payableTax.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    payableTax = payableTax.Where(x => x.InvoiceNumber.ToString().Contains(searchValueLower) ||
                    x.SerialNo.ToString().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = payableTax.Count();

                // Paging
                payableTax = payableTax.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = payableTax }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: Sales/PayableTax
        public ActionResult Index()
        {
            IDS.GeneralTable.Branch branch = new IDS.GeneralTable.Branch();

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
            
            ViewData["SelectListCustSupp"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["SelectListTax"] = new SelectList(IDS.GeneralTable.Tax.GetTaxForDataSource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        public ActionResult Edit(string invNo, string serialNo)
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

            IDS.Sales.PayableTax payableTax = IDS.Sales.PayableTax.GetPayableTax(invNo, serialNo);
            
            ViewData["SelectListTaxObject"] = new SelectList(IDS.Sales.JenisPenghasilan.GetJPForDataSource(), "Value", "Text", payableTax.TaxObjectID.JPID.ToString());
            ViewData["SelectListKomoditi"] = new SelectList(IDS.Sales.Komoditi.GetKomoditiForDataSource(), "Value", "Text", payableTax.KomoditiID);

            ViewData["FormAction"] = 2;

            if (payableTax != null)
            {
                return PartialView("Create", payableTax);
            }
            else
            {
                return PartialView("Create", new IDS.Sales.PayableTax());
            }
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ModelState.Clear();

            ValidateModel(collection);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    IDS.Sales.PayableTax payableTax = new IDS.Sales.PayableTax();

                    payableTax.InvoiceNumber = collection["InvoiceNumber"];
                    payableTax.SerialNo = collection["SerialNo"];
                    payableTax.NoBuktiPotong = collection["NoBuktiPotong"];

                    payableTax.TaxObjectID = new IDS.Sales.JenisPenghasilan();
                    payableTax.TaxObjectID.JPID = Tool.GeneralHelper.NullToInt(collection["TaxObjectID.JPID"],0);

                    payableTax.KomoditiID = collection["KomoditiID"];
                    payableTax.Description = collection["Description"];
                    payableTax.TaxRate = Tool.GeneralHelper.NullToDecimal(collection["Description"],0);
                    payableTax.Amount = Tool.GeneralHelper.NullToDecimal(collection["Amount"], 0);
                    payableTax.Tarif = Tool.GeneralHelper.NullToDecimal(collection["Tarif"], 0);
                    payableTax.TarifNonNPWP= Tool.GeneralHelper.NullToDecimal(collection["TarifNonNPWP"], 0);
                    payableTax.TanggalSetor = Convert.ToDateTime(collection["TanggalSetor"]);
                    payableTax.TanggalLapor = Convert.ToDateTime(collection["TanggalLapor"]);

                    payableTax.InsUpDelPayableTax((int)IDS.Tool.PageActivity.Edit);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(string invNosCodeList, string serialNosCodeList)
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

            if (string.IsNullOrWhiteSpace(invNosCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] invsCode = invNosCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] sersCode = serialNosCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (invsCode.Length > 0)
                {
                    IDS.Sales.PayableTax payableTax = new IDS.Sales.PayableTax();
                    payableTax.InsUpDelPayableTax(IDS.Tool.PageActivity.Delete, invsCode,sersCode);
                }

                return Json("Payable Tax data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}