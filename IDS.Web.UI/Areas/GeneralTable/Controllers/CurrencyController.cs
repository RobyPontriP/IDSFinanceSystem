using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class CurrencyController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GeneralTable.Currency> currencies = IDS.GeneralTable.Currency.GetCurrencyList();

                //totalRecords = currencies.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        case "country":
                //            currencies = currencies.OrderBy("cities.Country.ToString()" + " " + sortColumnDir).ToList();
                //            break;
                //        default:
                //            currencies = currencies.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    currencies = currencies.Where(x => x.CurrencyName.ToLower().Contains(searchValueLower) ||
                //                             x.CurrencyCode.ToLower().Contains(searchValueLower) ||
                //                             x.CountryCurrency.CountryName.ToLower().Contains(searchValueLower) ||
                //                             x.EntryUser.ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = currencies.Count();

                //// Paging
                //if (pageSize > 0)
                //    currencies = currencies.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = currencies }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(currencies), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        // GET: GeneralTable/Currency
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

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GeneralTable.Currency());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Currency currency)
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

            ValidateModel(currency);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    currency.EntryUser = currency.OperatorID = currentUser;

                    currency.InsUpDelCurrency((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string currencyCode)
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

            IDS.GeneralTable.Currency currency = IDS.GeneralTable.Currency.GetCurrency(currencyCode);

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text",currency.CountryCurrency.CountryCode);

            ViewData["FormAction"] = 2;

            if (currency != null)
            {
                return PartialView("Create", currency);
            }
            else
            {
                return PartialView("Create", new IDS.GeneralTable.Currency());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Currency currency)
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

            ValidateModel(currency);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    currency.OperatorID = currentUser;

                    currency.InsUpDelCurrency((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string currenciesCodeList)
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

            if (string.IsNullOrWhiteSpace(currenciesCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] currenciesCode = currenciesCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (currenciesCode.Length > 0)
                {
                    IDS.GeneralTable.Currency currency = new IDS.GeneralTable.Currency();
                    currency.InsUpDelCurrency((int)IDS.Tool.PageActivity.Delete, currenciesCode);
                }

                return Json("Currency data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetCurrencyList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Currency.GetCurrencyForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetFormatMidRate(decimal midRate, string ccy)
        {
            return Json(IDS.GeneralTable.Currency.SetMidRateFormat(midRate, ccy),JsonRequestBehavior.AllowGet);
        }
    }
}