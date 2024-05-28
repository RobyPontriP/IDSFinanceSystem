using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class SupplierController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData()
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

                List<IDS.GeneralTable.Supplier> supplier = IDS.GeneralTable.Supplier.GetSupplier();

                totalRecords = supplier.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    supplier = supplier.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    supplier = supplier.Where(x => x.SupName.ToLower().Contains(searchValueLower) ||
                                             x.SupCode.ToLower().Contains(searchValueLower) ||
                                             x.SupCountry.CountryCode.ToLower().Contains(searchValueLower) ||
                                             x.SupCity.CityCode.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = supplier.Count();

                // Paging
                if (pageSize > 0)
                    supplier = supplier.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = supplier }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: GeneralTable/Supplier
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

            ViewData["FormAction"] = 1;

            //ViewData["SelectListSupAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("", "AP"), "Value", "Text");
            ViewData["SelectListSupAcc"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCForDatasource("BN", IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");
            ViewData["SelectListVTAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("", "VT"), "Value", "Text");
            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().CountryCode);
            ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            // Billing
            ViewData["SelectListBillCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().CountryCode);
            ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            //

            // Tax
            ViewData["SelectListTaxCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().CountryCode);
            ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            //
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Create", new IDS.GeneralTable.Supplier());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Supplier supplier)
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

            ValidateModel(supplier);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    supplier.OperatorID = currentUser;

                    supplier.InsUpDelSupplier((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string SupCode)
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

            IDS.GeneralTable.Supplier sup = IDS.GeneralTable.Supplier.GetSupplier(SupCode);

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", sup.SupCountry.CountryCode);
            ViewData["SelectListBillCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", sup.BillCountry.CountryCode);
            ViewData["SelectListTaxCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", sup.TaxCountry.CountryCode);

            if (sup.SupCity!= null)
            {
                if (sup.SupCountry != null || sup.SupCountry.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.SupCountry.CountryCode), "Value", "Text", sup.SupCity.CityCode);
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (sup.SupCountry != null || sup.SupCountry.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.SupCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }

            //Bill
            if (sup.BillCity != null)
            {
                if (sup.BillCountry != null || sup.BillCountry.CountryCode != null)
                {
                    ViewData["SelectListBillCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.BillCountry.CountryCode), "Value", "Text", sup.BillCity.CityCode);
                }
                else
                {
                    ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (sup.BillCountry != null || sup.BillCountry.CountryCode != null)
                {
                    ViewData["SelectListBillCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.BillCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            //

            //Tax
            if (sup.TaxCity != null)
            {
                if (sup.TaxCountry != null || sup.TaxCountry.CountryCode != null)
                {
                    ViewData["SelectListTaxCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.TaxCountry.CountryCode), "Value", "Text", sup.TaxCity.CityCode);
                }
                else
                {
                    ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (sup.TaxCountry != null || sup.TaxCountry.CountryCode != null)
                {
                    ViewData["SelectListTaxCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(sup.TaxCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            //

            //ViewData["SelectListSupAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExcludeProtAccForDatasource(), "Value", "Text", sup.SupAcc.Account);
            ViewData["SelectListSupAcc"] = new SelectList(IDS.GLTable.SpecialAccount.GetSPACCForDatasource("BN", IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");
            ViewData["SelectListSalesAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExcludeProtAccForDatasource(), "Value", "Text", sup.SalesAcc.Account);
            ViewData["SelectListVTAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("", "VT"), "Value", "Text", sup.VATAcc.Account);

            ViewData["FormAction"] = 2;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            if (sup != null)
            {
                return View("Create", sup);
                //return RedirectToAction("Create",sup);
            }
            else
            {
                return View("Create", new IDS.GeneralTable.Supplier());
                //return RedirectToAction("Create", new IDS.GeneralTable.Supplier());
            }
        }

        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Supplier supplier)
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

            ValidateModel(supplier);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    supplier.OperatorID = currentUser;

                    supplier.InsUpDelSupplier((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string suppliersCodeList)
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

            if (string.IsNullOrWhiteSpace(suppliersCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] suppliersCode = suppliersCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (suppliersCode.Length > 0)
                {
                    IDS.GeneralTable.Supplier sup = new IDS.GeneralTable.Supplier();
                    sup.InsUpDelSupplier(IDS.Tool.PageActivity.Delete, suppliersCode);
                }

                return Json("Supplier data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetVendForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Supplier.GetACFVENDForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSupplier(string supCode)
        {
            IDS.GeneralTable.Supplier supplier = IDS.GeneralTable.Supplier.GetSupplier(supCode);
            return Json(supplier, JsonRequestBehavior.AllowGet);
        }
    }
}