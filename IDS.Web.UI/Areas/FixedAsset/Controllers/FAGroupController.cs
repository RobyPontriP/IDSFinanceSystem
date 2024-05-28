using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;


namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FAGroupController : IDS.Web.UI.Controllers.MenuController
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

                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
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

                List<IDS.FixedAsset.FAGroup> group = IDS.FixedAsset.FAGroup.GetFAGroup();

                totalRecords = group.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    group = group.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string lowerSearchValue = searchValue.ToLower();

                    group = group.Where(x => Tool.GeneralHelper.NullToString(x.Code).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.Description).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.GLAccDepreExpense).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.GLAccGainLoss).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.GLAccItemGroup).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.GLAccumDepre).ToLower().Contains(lowerSearchValue) ||
                                             ((IDS.FixedAsset.FADepreMethod)x.DepreMethod).ToString().Replace("_", " ").ToLower().Contains(lowerSearchValue) ||
                                             x.OperatorID.ToLower().Contains(lowerSearchValue) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(lowerSearchValue)).ToList();
                }

                totalRecordsShowing = group.Count();

                // Paging
                if (pageSize > 0)
                    group = group.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = group }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
            }

            return result;
        }

        // GET: FixedAsset/FAAssetGroup
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

        // GET: FixedAsset/FAAssetGroup/Create
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

            ViewData["COAList"] = IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["DepreMethodList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FADepreMethod>(IDS.FixedAsset.FADepreMethod.Double_Declining);

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.FixedAsset.FAGroup());
        }

        // POST: FixedAsset/FAAssetGroup/Create
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.FixedAsset.FAGroup group)
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
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    group.LastUpdate = DateTime.Now;
                    group.OperatorID = currentUser;

                    group.InsUpDel((int)IDS.Tool.PageActivity.Insert);

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

        // GET: FixedAsset/FAAssetGroup/Edit/5
        public ActionResult Edit(string groupCode)
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

            ViewData["COAList"] = IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["DepreMethodList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FADepreMethod>(IDS.FixedAsset.FADepreMethod.Straight_Line);

            IDS.FixedAsset.FAGroup group = IDS.FixedAsset.FAGroup.GetFAGroup(groupCode);
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            ViewData["FormAction"] = 2;

            if (group != null)
            {
                return PartialView("Create", group);
            }
            else
            {
                return PartialView("Create", new IDS.FixedAsset.FAGroup());
            }
        }

        // POST: FixedAsset/FAAssetGroup/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.FixedAsset.FAGroup group)
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
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    group.LastUpdate = DateTime.Now;
                    group.OperatorID = currentUser;

                    group.InsUpDel((int)IDS.Tool.PageActivity.Edit);

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

        // POST: FixedAsset/FAAssetGroup/Delete/5
        [HttpPost]
        public ActionResult Delete(string groupCodeList)
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

            if (string.IsNullOrWhiteSpace(groupCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] groupsCode = groupCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (groupsCode.Length > 0)
                {
                    IDS.FixedAsset.FAGroup group = new IDS.FixedAsset.FAGroup();
                    group.InsUpDel(groupsCode);
                }

                return Json("Fixed asset group data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CalculateRate(string DepreMethod, string year)
        {
            if (string.IsNullOrWhiteSpace(year) || year == "0")
                return Json("0", JsonRequestBehavior.AllowGet);

            double Rate = 0;

            switch (DepreMethod)
            {
                case "Straight_Line":
                    Rate = 100;
                    Rate = Math.Round(Rate / Convert.ToDouble(year), 2);
                    break;
                case "Double_Declining":
                    Rate = 200;
                    Rate = Math.Round(Rate / Convert.ToDouble(year), 2);
                    break;
                case "Not_Depreciate":
                    Rate = 0;
                    break;
            }

            return Json(Rate.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDepreMethodAndYear(string GroupCode)
        {
            IDS.FixedAsset.FAGroup group = IDS.FixedAsset.FAGroup.GetFAGroup(GroupCode);

            if (group != null)
            {
                return Json(group.DepreMethod.ToString().Replace("_", " ") + ";" + group.DepreYear, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("" + ";" + "0", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
