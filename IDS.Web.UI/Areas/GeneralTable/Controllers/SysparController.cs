using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class SysparController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: GeneralTable/Syspar
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 1;

            IDS.GeneralTable.Syspar syspar = IDS.GeneralTable.Syspar.GetInstance();
            syspar.RefreshData();
            
            ViewData["SelectListCountry"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text",syspar.CountryCode);
            ViewData["SelectListCcy"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text",syspar.BaseCCy);
            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Text",syspar.Department);

            ViewData["rbLanguage"] = syspar.Language.Trim();
            ViewData["DefaultDate"] = Convert.ToDateTime(syspar.StartFiscalYear).ToString("dd-MMM-yyyy");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Index", syspar);
        }
        
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, FormCollection collection)
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

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    IDS.GeneralTable.Syspar syspar = IDS.GeneralTable.Syspar.GetInstance();

                    if (AccessLevel.EditAccess == 1)
                    {
                        syspar.Version = collection["Version"];
                        syspar.Name = collection["Name"];
                        syspar.Address1 = collection["Address1"];
                        syspar.Address2 = collection["Address2"];
                        syspar.Address3 = collection["Address3"];
                        //syspar.InfoListDir = collection["InfoListDir"];
                        syspar.CountryCode = collection["CountryCode"];
                        syspar.BaseCCy = collection["BaseCCy"];
                        syspar.Department = collection["Department"];
                        syspar.StartFiscalYear = Convert.ToDateTime(collection["StartFiscalYear"]);
                        syspar.Phone = collection["Phone"];
                        syspar.Fax = collection["Fax"];
                        syspar.PrintName = collection["PrintName"] == "false" ? false : true;
                        syspar.PrintAddress = collection["PrintAddress"] == "false" ? false : true;
                        syspar.PrintCity = collection["PrintCity"] == "false" ? false : true;
                        syspar.PrintCountry = collection["PrintCountry"] == "false" ? false : true;
                        syspar.PrintDate = collection["PrintDate"] == "false" ? false : true;
                        syspar.PrintTime = collection["PrintTime"] == "false" ? false : true;
                        syspar.PrintPageNumber = collection["PrintPageNumber"] == "false" ? false : true;
                        syspar.Language = collection["Language"].ToUpper();
                        syspar.VAT = IDS.Tool.GeneralHelper.NullToDecimal(collection["VAT"],0);
                        //syspar.SignBy1 = collection["SignBy1"];
                        //syspar.SignBy2 = collection["SignBy2"];
                        //syspar.Occupation1 = collection["Occupation1"];
                        //syspar.Occupation2 = collection["Occupation2"];

                        syspar.UpdateSyspar();

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("You don't have access", JsonRequestBehavior.AllowGet);
                    }
                    
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

        public JsonResult GetSignBy()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Branch.GetSignByForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOccupation()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GeneralTable.Syspar.GetOccupationForDataSource();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}