using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Sales.Controllers
{
    public class AccrualCustProjectController  : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string period)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

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

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Sales.AccrualCustProject> accrualCustProj = IDS.Sales.AccrualCustProject.GetListAccrualCustProject(period);

                totalRecords = accrualCustProj.Count;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            accrualCustProj = accrualCustProj.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    accrualCustProj = accrualCustProj.Where(x => x.Period.ToString().Contains(searchValueLower) || 
                    x.Total.ToString().Contains(searchValueLower) || 
                    Tool.GeneralHelper.NullToString(x.Name, "").ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = accrualCustProj.Count();

                // Paging
                accrualCustProj = accrualCustProj.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = accrualCustProj }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataDetail(string period)
        {
            //if (string.IsNullOrEmpty(period))
            //    period = DateTime.Now.ToString("yyyyMM");
            //else
            //{
            //    DateTime datePeriod = Convert.ToDateTime(period);
            //    period = datePeriod.ToString("yyyyMM");
            //}

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

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Sales.AccrualCustProject> accrualCustProjDet = IDS.Sales.AccrualCustProject.GetAccrualCustProjectDetail(period);

                totalRecords = accrualCustProjDet.Count;

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            accrualCustProjDet = accrualCustProjDet.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    accrualCustProjDet = accrualCustProjDet.Where(x => x.Period.ToString().Contains(searchValueLower) ||
                    x.InvoiceNo.ToString().Contains(searchValueLower) ||
                    x.Amount.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = accrualCustProjDet.Count();

                // Paging
                accrualCustProjDet = accrualCustProjDet.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = accrualCustProjDet }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: Sales/AccrualCustProject
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
        public ActionResult Edit(string invNo,string branch,string period)
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

            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            IDS.Sales.AccrualCustProject accrualCustProj = IDS.Sales.AccrualCustProject.GetAccrualCustProject(period,invNo,branch);

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]))
            {
                ViewData["HOStatus"] = 1;
                ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", branch);
            }
            else
            {
                ViewData["HOStatus"] = 0;
                ViewData["SelectListBranch"] = ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString()), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }

            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", accrualCustProj.Ccy);

            ViewData["FormAction"] = 2;

            
            ViewData["PeriodDef"] = DateTime.ParseExact(period, "yyyyMM", null).ToString("MMM yyyy");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Create",accrualCustProj);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
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

            ValidateModel(collection);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    IDS.Sales.AccrualCustProject accrCustProj = new IDS.Sales.AccrualCustProject();

                    accrCustProj.InvoiceNo = collection["InvoiceNo"];
                    accrCustProj.Branch = collection["Branch"];
                    accrCustProj.Period = Convert.ToDateTime(collection["Period"]).ToString("yyyyMM");
                    accrCustProj.Amount = Tool.GeneralHelper.NullToDecimal(collection["Amount"], 0);

                    accrCustProj.OperatorID = currentUser;

                    accrCustProj.InsUpDelAccrCustProj(IDS.Tool.PageActivity.Edit);

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
    }
}