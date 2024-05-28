using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTable.Controllers
{
    public class RptGenController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GLTable.ReportGenerator> repGens = IDS.GLTable.ReportGenerator.GetRepGen();

                totalRecords = repGens.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    repGens = repGens.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLOwer = searchValue.ToLower();

                    repGens = repGens.Where(x => x.Code.ToLower().Contains(searchValueLOwer) ||
                                             x.Description.ToLower().Contains(searchValueLOwer) ||
                                             x.OperatorID.ToLower().Contains(searchValueLOwer) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLOwer)).ToList();
                }

                totalRecordsShowing = repGens.Count();

                // Paging
                if (pageSize > 0)
                    repGens = repGens.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = repGens }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }

        public JsonResult GetDataWithRptCode(string rptCode)
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

                List<IDS.GLTable.ReportGenerator> repGens = IDS.GLTable.ReportGenerator.GetRepGen(rptCode);

                totalRecords = repGens.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    repGens = repGens.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLOwer = searchValue.ToLower();

                    repGens = repGens.Where(x => x.Code.ToLower().Contains(searchValueLOwer) ||
                                             x.Description.ToLower().Contains(searchValueLOwer) ||
                                             x.OperatorID.ToLower().Contains(searchValueLOwer) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLOwer)).ToList();
                }

                totalRecordsShowing = repGens.Count();

                // Paging
                if (pageSize > 0)
                    repGens = repGens.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = repGens }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }

            return result;
        }
        // GET: GLTable/ReportGenerator
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

            ViewData["SelectListCode"] = new SelectList(IDS.GLTable.ReportGenerator.GetCodeForDatasource(), "Value", "Value");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        [HttpGet, AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create(string rptCode)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
            {
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["SelectListCode"] = new SelectList(IDS.GLTable.ReportGenerator.GetCodeForDatasource(), "Value", "Value",rptCode);
            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value");
            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Value");
            ViewData["SelectListDC"] = new SelectList(IDS.GLTable.ReportGenerator.GetDbtCrdForDatasource(), "Value", "Text");
            ViewData["SelectListFromAcc"] = new SelectList(IDS.GLTable.ReportGenerator.GetFromAccForDatasource(), "Value", "Text");
            ViewData["SelectListCol"] = new SelectList(IDS.GLTable.ReportGenerator.GetColForDatasource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(new SelectList(new List<SelectListItem>(), "Value", "Text"));

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.GLTable.ReportGenerator());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.GLTable.ReportGenerator repGen)
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
                    repGen.OperatorID = currentUser;

                    repGen.InsUpDelRepGen((int)IDS.Tool.PageActivity.Insert);

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

        public ActionResult Edit(string rptGenCode, string line)
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

            IDS.GLTable.ReportGenerator repGen = IDS.GLTable.ReportGenerator.GetReportGen(rptGenCode, line);

            ViewData["SelectListCode"] = new SelectList(IDS.GLTable.ReportGenerator.GetCodeForDatasource(), "Value", "Value", rptGenCode);
            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Value", repGen.CurrencyACFGEN);
            ViewData["SelectListDept"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(), "Value", "Value",repGen.DepartmentACFGEN);
            ViewData["SelectListDC"] = new SelectList(IDS.GLTable.ReportGenerator.GetDbtCrdForDatasource(), "Value", "Text",repGen.DebitCredit);
            ViewData["SelectListFromAcc"] = new SelectList(IDS.GLTable.ReportGenerator.GetFromAccForDatasource(), "Value", "Text",repGen.FromACC);
            ViewData["SelectListCol"] = new SelectList(IDS.GLTable.ReportGenerator.GetColForDatasource(), "Value", "Text");
            ViewData["SelectListAcc"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDataSource(repGen.CurrencyACFGEN), "Value", "Text",repGen.AccACFGEN);

            ViewData["FormAction"] = 2;

            ViewData["C1ViewData"] = repGen.C1;

            if (repGen != null)
            {
                return PartialView("Create", repGen);
            }
            else
            {
                return PartialView("Create", new IDS.GLTable.ReportGenerator());
            }
        }

        // POST: GeneralTable/Bank/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GLTable.ReportGenerator repGen)
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
                    repGen.OperatorID = currentUser;

                    repGen.InsUpDelRepGen((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string rptGenCodeList, string lineList)
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

            if (string.IsNullOrWhiteSpace(rptGenCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] rptGensCode = rptGenCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] linesCode = lineList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (rptGensCode.Length > 0)
                {
                    IDS.GLTable.ReportGenerator repGen = new IDS.GLTable.ReportGenerator();
                    repGen.InsUpDelRepGen((int)IDS.Tool.PageActivity.Delete, rptGensCode, linesCode);
                }

                return Json("Report Generator data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}