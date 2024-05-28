using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable
{
    public class TaxController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData()
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

                List<IDS.GeneralTable.Tax> taxs = IDS.GeneralTable.Tax.GetTax();

                //totalRecords = taxs.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    taxs = taxs.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLOwer = searchValue.ToLower();

                //    taxs = taxs.Where(x => x.TaxID.ToLower().Contains(searchValueLOwer) ||
                //                             x.TaxName.ToLower().Contains(searchValueLOwer) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLOwer) ||
                //                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLOwer)).ToList();
                //}

                //totalRecordsShowing = taxs.Count();

                //// Paging
                //if (pageSize > 0)
                //    taxs = taxs.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = taxs }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(taxs), JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GeneralTable/Tax
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

            ViewData["SelectListPrepaidAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource().Where(x => x.Value.StartsWith("2")), "Value", "Text");
            ViewData["SelectListPayableAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource().Where(x => x.Value.StartsWith("3")), "Value", "Text");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GeneralTable.Tax());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Tax tax)
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

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    tax.OperatorID = currentUser;

                    tax.InsUpDelTax((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string taxId)
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

            IDS.GeneralTable.Tax tax = IDS.GeneralTable.Tax.GetTax(taxId);

            ViewData["SelectListPrepaidAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource().Where(x => x.Value.StartsWith("2")), "Value", "Text", tax.PrepaidAccount);
            ViewData["SelectListPayableAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource().Where(x => x.Value.StartsWith("3")), "Value", "Text", tax.PayableAccount);

            ViewData["FormAction"] = 2;

            if (tax != null)
            {
                return PartialView("Create", tax);
            }
            else
            {
                return PartialView("Create", new IDS.GeneralTable.Tax());
            }
        }

        // POST: GeneralTable/Tax/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Tax tax)
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

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    tax.OperatorID = currentUser;

                    tax.InsUpDelTax((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string taxsIdList)
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

            if (string.IsNullOrWhiteSpace(taxsIdList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] taxsCode = taxsIdList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (taxsCode.Length > 0)
                {
                    IDS.GeneralTable.Tax tax = new IDS.GeneralTable.Tax();
                    tax.InsUpDelTax((int)IDS.Tool.PageActivity.Delete, taxsCode);
                }

                return Json("Tax data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTaxPPh(string taxType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Tax.GetTaxPPhForDataSource(taxType);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxIDValueForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Tax.GetTaxIDValueForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalculateOutstandingSalesTax(decimal amount, string cust, DateTime transDate, string invNo, string acc)
        {
            decimal Outstanding = 0;
            if (!string.IsNullOrEmpty(acc) && !string.IsNullOrEmpty(invNo))
            {
                IDS.GeneralTable.Tax tax = IDS.GeneralTable.Tax.GetTaxWithAcc(acc);
                if (tax != null)
                {
                    Outstanding = tax.CalculateOutstandingSalesTax(amount, cust, transDate, invNo);
                }
            }
            

            return Json(Outstanding, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJenisPenghasilan(string taxCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.tblJenisPenghasilan.GetKodePajakForDataSource(taxCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}