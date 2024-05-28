using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class KelurahanController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GeneralTable.Kelurahan> kelurahan = IDS.GeneralTable.Kelurahan.GetKelurahan();

                totalRecords = kelurahan.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    kelurahan = kelurahan.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    kelurahan = kelurahan.Where(x => x.KelurahanName.ToLower().Contains(searchValueLower) ||
                                             x.KelurahanCode.ToLower().Contains(searchValueLower) ||
                                             x.CountryKelurahan.CountryName.ToLower().Contains(searchValueLower) ||
                                             x.CityKelurahan.CityName.ToLower().Contains(searchValueLower) ||
                                             x.KecamatanKelurahan.KecamatanName.ToLower().Contains(searchValueLower) ||
                                             x.ZipCode.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = kelurahan.Count();

                // Paging
                if (pageSize > 0)
                    kelurahan = kelurahan.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = kelurahan }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GeneralTable/Kelurahan
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
            ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["SelectListKecamatan"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GeneralTable.Kelurahan());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Kelurahan kelurahan)
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

            ValidateModel(kelurahan);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    kelurahan.OperatorID = currentUser;

                    kelurahan.InsUpDelKelurahan((int)IDS.Tool.PageActivity.Insert);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            }
            else
            {
                return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(string kelurahanCode, string countryCode, string cityCode, string kecamatanCode)
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

            IDS.GeneralTable.Kelurahan kelurahan = IDS.GeneralTable.Kelurahan.GetKelurahan(kelurahanCode, countryCode, cityCode, kecamatanCode);

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", kelurahan.CountryKelurahan.CountryCode);

            if (kelurahan.CityKelurahan != null)
            {
                if (kelurahan.CountryKelurahan != null || kelurahan.CountryKelurahan.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(kelurahan.CountryKelurahan.CountryCode), "Value", "Text", kelurahan.CityKelurahan.CityCode);
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (kelurahan.CountryKelurahan != null || kelurahan.CountryKelurahan.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(kelurahan.CountryKelurahan.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }

            if (kelurahan.KecamatanKelurahan != null)
            {
                if (kelurahan.CityKelurahan != null || kelurahan.CityKelurahan.CityCode != null)
                {
                    ViewData["SelectListKecamatan"] = new SelectList(IDS.GeneralTable.Kecamatan.GetKecamatanForDataSource(kelurahan.CityKelurahan.CityCode), "Value", "Text", kelurahan.CityKelurahan.CityCode);
                }
                else
                {
                    ViewData["SelectListKecamatan"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (kelurahan.CountryKelurahan != null || kelurahan.CountryKelurahan.CountryCode != null)
                {
                    ViewData["SelectListKecamatan"] = new SelectList(IDS.GeneralTable.Kecamatan.GetKecamatanForDataSource(kelurahan.CityKelurahan.CityCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListKecamatan"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            //ViewData["SelectListKecamatan"] = new SelectList(IDS.GeneralTable.Kecamatan.GetKecamatanForDataSource(), "Value", "Text", kelurahan.KecamatanKelurahan.KecamatanCode);

            ViewData["FormAction"] = 2;

            if (kelurahan != null)
            {
                return PartialView("Create", kelurahan);
            }
            else
            {
                return PartialView("Create", new IDS.GeneralTable.Kelurahan());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Kelurahan kelurahan)
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

            ValidateModel(kelurahan);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    kelurahan.OperatorID = currentUser;

                    kelurahan.InsUpDelKelurahan((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string kelurahansCodeList, string countryCodeList, string cityCodeList, string kecamatansCodeList)
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

            if (string.IsNullOrWhiteSpace(kelurahansCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] kelurahansCode = kelurahansCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] countriesCode = countryCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] citiesCode = cityCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] kecamatansCode = kecamatansCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (kecamatansCode.Length > 0)
                {
                    IDS.GeneralTable.Kelurahan kelurahan = new IDS.GeneralTable.Kelurahan();
                    kelurahan.InsUpDelKelurahan((int)IDS.Tool.PageActivity.Delete, kelurahansCode, countriesCode, citiesCode, kecamatansCode);
                }

                return Json("Kelurahan data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}