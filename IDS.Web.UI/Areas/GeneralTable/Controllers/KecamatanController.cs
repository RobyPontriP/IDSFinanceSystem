using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class KecamatanController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GeneralTable.Kecamatan> kecamatans = IDS.GeneralTable.Kecamatan.GetKecamatan();

                totalRecords = kecamatans.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        case "country":
                            kecamatans = kecamatans.OrderBy("cities.CountryCity.ToString()" + " " + sortColumnDir).ToList();
                            break;
                        default:
                            kecamatans = kecamatans.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    kecamatans = kecamatans.Where(x => x.KecamatanName.ToLower().Contains(searchValueLower) ||
                                             x.KecamatanCode.ToLower().Contains(searchValueLower) ||
                                             x.CountryKecamatan.CountryName.ToLower().Contains(searchValueLower) ||
                                             x.CityKecamatan.CityName.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = kecamatans.Count();

                // Paging
                if (pageSize > 0)
                    kecamatans = kecamatans.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = kecamatans }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GeneralTable/Kecamatan
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

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GeneralTable.Kecamatan());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GeneralTable.Kecamatan kecamatan)
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

            ValidateModel(kecamatan);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    kecamatan.OperatorID = currentUser;

                    kecamatan.InsUpDelKecamatan((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string kecamatanCode,string countryCode, string cityCode)
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

            IDS.GeneralTable.Kecamatan kecamatan = IDS.GeneralTable.Kecamatan.GetKecamatan(kecamatanCode,countryCode,cityCode);

            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text",kecamatan.CountryKecamatan.CountryCode);
            ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(), "Value", "Text", kecamatan.CityKecamatan.CityCode);

            if (kecamatan.CityKecamatan != null)
            {
                if (kecamatan.CountryKecamatan != null || kecamatan.CountryKecamatan.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(kecamatan.CountryKecamatan.CountryCode), "Value", "Text", kecamatan.CityKecamatan.CityCode);
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }
            else
            {
                if (kecamatan.CountryKecamatan != null || kecamatan.CountryKecamatan.CountryCode != null)
                {
                    ViewData["SelectListCity"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(kecamatan.CountryKecamatan.CountryCode), "Value", "Text");
                }
                else
                {
                    ViewData["SelectListCity"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
                }
            }

            ViewData["FormAction"] = 2;

            if (kecamatan != null)
            {
                return PartialView("Create", kecamatan);
            }
            else
            {
                return PartialView("Create", new IDS.GeneralTable.Kecamatan());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Kecamatan kecamatan)
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

            ValidateModel(kecamatan);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    kecamatan.OperatorID = currentUser;

                    kecamatan.InsUpDelKecamatan((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string kecamatansCodeList, string countryCodeList,string cityCodeList)
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

            if (string.IsNullOrWhiteSpace(kecamatansCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] kecamatansCode = kecamatansCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] countriesCode = countryCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] citiesCode = cityCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (kecamatansCode.Length > 0)
                {
                    IDS.GeneralTable.Kecamatan kecamatan = new IDS.GeneralTable.Kecamatan();
                    kecamatan.InsUpDelKecamatan((int)IDS.Tool.PageActivity.Delete, kecamatansCode, countriesCode, citiesCode);
                }

                return Json("Kecamatan data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetKecamatanFromCity(string cityCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Kecamatan.GetKecamatanForDataSource(cityCode);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}