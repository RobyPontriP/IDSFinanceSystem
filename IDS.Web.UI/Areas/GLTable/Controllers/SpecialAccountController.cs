using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class SpecialAccountController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GLTable.SpecialAccount> specialAccount = IDS.GLTable.SpecialAccount.GetSpecialAccount();

                totalRecords = specialAccount.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    specialAccount = specialAccount.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    specialAccount = specialAccount.Where(x => x.ID.ToString().Contains(searchValueLower) ||
                                             x.TypeACC.ToLower().Contains(searchValueLower) ||
                                             x.FromCCY.ToLower().Contains(searchValueLower) ||
                                             x.FromACC.ToLower().Contains(searchValueLower) ||
                                             x.ToCCY.ToLower().Contains(searchValueLower) ||
                                             x.ToCCY.ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = specialAccount.Count();

                // Paging
                if (pageSize > 0)
                    specialAccount = specialAccount.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = specialAccount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataWithTypeAcc(string typeAcc)
        {
            //if (string.IsNullOrEmpty(branchCode))
            //    branchCode = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

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

                List<IDS.GLTable.SpecialAccount> specialAccount = IDS.GLTable.SpecialAccount.GetSpecialAccountWithTypeAcc(typeAcc);

                //if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
                //{
                //    departments = IDS.GeneralTable.Department.GetDepartment();
                //}
                //else
                //{
                //    departments = IDS.GeneralTable.Department.GetDepartmentList(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
                //}
                //totalRecords = specialAccount.Count;

                //// Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    specialAccount = specialAccount.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //}

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    specialAccount = specialAccount.Where(x => x.TypeACC.ToLower().Contains(searchValueLower) ||
                //                             x.FromCCY.ToLower().Contains(searchValueLower) ||
                //                             x.FromACC.ToLower().Contains(searchValueLower) ||
                //                             x.ToCCY.ToLower().Contains(searchValueLower) ||
                //                             x.ToACC.ToLower().Contains(searchValueLower) ||
                //                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                //                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = specialAccount.Count();

                //// Paging
                //if (pageSize > 0)
                //    specialAccount = specialAccount.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = specialAccount }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(specialAccount), JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: GL/SpecialAccount
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["SelectListTypeAcc"] = new SelectList(IDS.GLTable.SpecialAccount.GetTypeAccForDatasource(), "Value", "Text");

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }

        public ActionResult Create(string typeACC)
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

            ViewData["LabelCCY"] = "Currency";
            ViewData["LabelACC"] = "Account Number";

            ViewData["TypeAccParam"] = typeACC;

            ViewData["SelectListProfitOrLostType"] = new SelectList(IDS.GLTable.SpecialAccount.GetProfitOrLostTypeForDatasource(), "Value", "Text");

            ViewData["SelectListCCYFrom"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListACCFrom"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["SelectListCCYTo"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListACCTo"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            return PartialView("Create", new IDS.GLTable.SpecialAccount());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GLTable.SpecialAccount specialAccount)
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

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    specialAccount.OperatorID = specialAccount.EntryUser = currentUser;

                    specialAccount.InsUpDelSpecialAccount((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string specialAccID, string typeACC)
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

            IDS.GLTable.SpecialAccount specialAccount = IDS.GLTable.SpecialAccount.GetSpecialAccount(specialAccID);

            ViewData["DefaultBranch"] = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();

            ViewData["FormAction"] = 2;

            ViewData["LabelCCY"] = "Currency";
            ViewData["LabelACC"] = "Account Number";

            ViewData["TypeAccParam"] = typeACC;

            ViewData["SelectListProfitOrLostType"] = new SelectList(IDS.GLTable.SpecialAccount.GetProfitOrLostTypeForDatasource(), "Value", "Text", specialAccount.FromACC);

            ViewData["SelectListCCYFrom"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", specialAccount.FromCCY);
            ViewData["SelectListACCFrom"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithAccountGroup(specialAccount.FromCCY), "Value", "Text",specialAccount.FromACC);
            ViewData["SelectListCCYTo"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", specialAccount.ToCCY);
            ViewData["SelectListACCTo"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithAccountGroup(specialAccount.ToCCY), "Value", "Text",specialAccount.ToACC);

            if (specialAccount != null)
            {
                return PartialView("Create", specialAccount);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.SpecialAccount());
            }
        }

        // POST: GeneralTable/SpecialAccount/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GLTable.SpecialAccount specialAccount)
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

            ValidateModel(specialAccount);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {

                    specialAccount.OperatorID = currentUser;

                    specialAccount.InsUpDelSpecialAccount((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string specialAccountsIDList)
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

            if (string.IsNullOrWhiteSpace(specialAccountsIDList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] specialAccountID = specialAccountsIDList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (specialAccountID.Length > 0)
                {
                    IDS.GLTable.SpecialAccount specialAccount = new IDS.GLTable.SpecialAccount();
                    specialAccount.InsUpDelSpecialAccount((int)IDS.Tool.PageActivity.Delete, specialAccountID);
                }

                return Json("Special Account data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSPACCForDataSource(string rptOf)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.SpecialAccount.GetSPACCForDatasource(rptOf);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSPACCWithCcyForDataSource(string rptOf,string ccy)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.SpecialAccount.GetSPACCForDatasource(rptOf,ccy);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSPACCForDataSourceWithPayType(string ccy,int payType)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list = IDS.GLTable.SpecialAccount.GetSPACCWithCcyForDataSource(ccy,payType);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}