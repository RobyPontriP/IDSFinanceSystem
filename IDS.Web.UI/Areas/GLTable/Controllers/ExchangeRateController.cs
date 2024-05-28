using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class ExchangeRateController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GLTable.ExchangeRate> exchRates = IDS.GLTable.ExchangeRate.GetExchangeRate();

                //totalRecords = exchRates.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    exchRates = exchRates.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    exchRates = exchRates.Where(x => x.ExchangeDate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower) ||
                //                             x.Currency1.CurrencyCode.ToLower().Contains(searchValueLower) ||
                //                             x.Currency2.CurrencyCode.ToLower().Contains(searchValueLower) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                //                             x.BidRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.OfferRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.MidRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = exchRates.Count();

                //// Paging
                //if (pageSize > 0)
                //    exchRates = exchRates.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = exchRates }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(exchRates), JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GL/ExchangeRate
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

            ViewData["SelectListCurrency1"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["SelectListCurrency2"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.ExchangeRate());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GLTable.ExchangeRate exchangeRate)
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

            ValidateModel(exchangeRate);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    exchangeRate.OperatorID = currentUser;

                    exchangeRate.InsUpDelExchangeRate((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string exchDate,string currency1,string currency2)
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
            
            ViewData["FormAction"] = 2;
            DateTime dtExchDate = Convert.ToDateTime(exchDate);
            IDS.GLTable.ExchangeRate exchRate = IDS.GLTable.ExchangeRate.GetExchangeRate(Convert.ToDateTime(exchDate), currency1, currency2);

            ViewData["SelectListCurrency1"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",currency1);
            ViewData["SelectListCurrency2"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",currency2);
            

            if (exchRate != null)
            {
                return PartialView("Create", exchRate);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.ExchangeRate());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GLTable.ExchangeRate exchangeRate)
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

            ValidateModel(exchangeRate);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    exchangeRate.OperatorID = currentUser;

                    exchangeRate.InsUpDelExchangeRate((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string exchDateList, string currency1CodeList, string currency2CodeList)
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

            if (string.IsNullOrWhiteSpace(exchDateList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] exchDate = exchDateList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] currency1Code = currency1CodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] currency2Code = currency2CodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (exchDate.Length > 0)
                {
                    IDS.GLTable.ExchangeRate exchDateObj = new IDS.GLTable.ExchangeRate();
                    exchDateObj.InsUpDelExchangeRate((int)IDS.Tool.PageActivity.Delete, exchDate, currency1Code, currency2Code);
                }

                return Json("Exchange Rate data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMidRate(string ccy1, string ccy2)
        {
            return Json(IDS.GLTable.ExchangeRate.GetMidRate(ccy1,ccy2), JsonRequestBehavior.AllowGet);
        }

        [ActionName("GetMidRateBaseCCY")]
        public ActionResult GetMidRate(string ccy1)
        {
            if (ccy1 == IDS.GeneralTable.Syspar.GetInstance().BaseCCy)
                return Json(1M, JsonRequestBehavior.AllowGet);

            return Json(IDS.GLTable.ExchangeRate.GetMidRate(ccy1, IDS.GeneralTable.Syspar.GetInstance().BaseCCy), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNearestExchangeRate(string ccyTo, DateTime date)
        {
            if (ccyTo == IDS.GeneralTable.Syspar.GetInstance().BaseCCy)
                return Json(1M, JsonRequestBehavior.AllowGet);

            return Json(IDS.GLTable.ExchangeRate.GetMidRate(ccyTo, IDS.GeneralTable.Syspar.GetInstance().BaseCCy), JsonRequestBehavior.AllowGet);
            //IDS.GLTable.ExchangeRate exchRate = IDS.GLTable.ExchangeRate.GetExchangeRate(date, ccyTo);

            //if (exchRate != null)
            //{
            //    return Json(IDS.Tool.GeneralHelper.NullToDecimal(exchRate.MidRate, 0), JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(0M, JsonRequestBehavior.AllowGet);
            //}
        }

        //public ActionResult CalcMidRate(string basedCcy)
        //{
        //    decimal result = 0;
        //    IDS.GeneralTable.Currency ccy = IDS.GeneralTable.Currency.GetCurrency(basedCcy);

        //    return Json(IDS.GLTable.ExchangeRate.GetMidRate(based, IDS.GeneralTable.Syspar.GetInstance().BaseCCy), JsonRequestBehavior.AllowGet);
        //}
    }
}