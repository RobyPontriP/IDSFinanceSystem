using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class CustomerController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GeneralTable.Customer> customer = IDS.GeneralTable.Customer.GetCustomer();

                totalRecords = customer.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    customer = customer.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    customer = customer.Where(x => x.CUSTName.ToLower().Contains(searchValueLower) ||
                                             x.CUSTCode.ToLower().Contains(searchValueLower) ||
                                             x.CustCountry.CountryCode.ToLower().Contains(searchValueLower) ||
                                             x.CustCity.CityCode.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = customer.Count();

                // Paging
                if (pageSize > 0)
                    customer = customer.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = customer }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }
        // GET: GeneralTable/Customer
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

            ViewData["SelectListCustAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("","AR"), "Value", "Text");
            ViewData["SelectListSalesAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExcludeProtAccForDatasource(), "Value", "Text");
            ViewData["SelectListVTAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("", "VT"), "Value", "Text");
            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text");
            ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            // Billing
            ViewData["SelectListBillCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text");
            ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            //

            // Tax
            ViewData["SelectListTaxCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text");
            ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            //

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Create", new IDS.GeneralTable.Customer());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Customer customer)
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

            ValidateModel(customer);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    customer.OperatorID = currentUser;

                    customer.InsUpDelCustomer((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string CustCode)
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

            IDS.GeneralTable.Customer cust = IDS.GeneralTable.Customer.GetCustomer(CustCode);

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", cust.CustCountry.CountryCode);
            ViewData["SelectListBillCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", cust.BillCountry.CountryCode);
            ViewData["SelectListTaxCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", cust.TaxCountry.CountryCode);

            if (cust.CustCity != null)
            {
                if (cust.CustCountry != null || cust.CustCountry.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.CustCountry.CountryCode), "Value", "Text", cust.CustCity.CityCode);
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (cust.CustCountry != null || cust.CustCountry.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.CustCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }

            //Bill
            if (cust.BillCity != null)
            {
                if (cust.BillCountry != null || cust.BillCountry.CountryCode != null)
                {
                    ViewData["SelectListBillCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.BillCountry.CountryCode), "Value", "Text", cust.BillCity.CityCode);
                }
                else
                {
                    ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (cust.BillCountry != null || cust.BillCountry.CountryCode != null)
                {
                    ViewData["SelectListBillCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.BillCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListBillCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            //

            //Tax
            if (cust.TaxCity != null)
            {
                if (cust.TaxCountry != null || cust.TaxCountry.CountryCode != null)
                {
                    ViewData["SelectListTaxCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.TaxCountry.CountryCode), "Value", "Text", cust.TaxCity.CityCode);
                }
                else
                {
                    ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (cust.TaxCountry != null || cust.TaxCountry.CountryCode != null)
                {
                    ViewData["SelectListTaxCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(cust.TaxCountry.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListTaxCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            //

            ViewData["SelectListCustAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExcludeProtAccForDatasource(), "Value", "Text", cust.CustAcc.Account);
            ViewData["SelectListSalesAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExcludeProtAccForDatasource(), "Value", "Text", cust.SalesAcc.Account);
            ViewData["SelectListVTAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAWithTypeExcludeProtAccForDataSource("", "VT"), "Value", "Text",cust.VATAcc.Account);
            
            ViewData["FormAction"] = 2;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            if (cust != null)
            {
                return View("Create", cust);
                //return RedirectToAction("Create",cust);
            }
            else
            {
                return View("Create", new IDS.GeneralTable.Customer());
                //return RedirectToAction("Create", new IDS.GeneralTable.Customer());
            }
        }

        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Customer customer)
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

            ValidateModel(customer);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    customer.OperatorID = currentUser;

                    customer.InsUpDelCustomer((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string customersCodeList)
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

            if (string.IsNullOrWhiteSpace(customersCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] customersCode = customersCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (customersCode.Length > 0)
                {
                    IDS.GeneralTable.Customer cust = new IDS.GeneralTable.Customer();
                    cust.InsUpDelCustomer(IDS.Tool.PageActivity.Delete, customersCode);
                }

                return Json("Customer data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCustomer(string custCode)
        {
            return Json(IDS.GeneralTable.Customer.GetCustomer(custCode), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustAccForDataSource(string custCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Customer.GetACCACFCUSTForDataSource(custCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustForDataSource()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Customer.GetACFCUSTForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}