using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class UserGroupController : IDS.Web.UI.Controllers.MenuController
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

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.Maintenance.UserGroup> userGroups = IDS.Maintenance.UserGroup.GetUserGroup();

                totalRecords = userGroups.Count;

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    userGroups = userGroups.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    userGroups = userGroups.Where(x => x.GroupCode.ToLower().Contains(searchValueLower) ||
                                             x.GroupName.ToLower().Contains(searchValueLower) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = userGroups.Count();

                // Paging
                userGroups = userGroups.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = userGroups }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        // GET: Maintenance/UserGroup
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

            // return PartialView("Index");

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

            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.Maintenance.UserGroup());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, IDS.Maintenance.UserGroup userGroup)
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
                    userGroup.OperatorID = userGroup.EntryUser = currentUser;

                    userGroup.InsUpDelUserGroup((int)IDS.Tool.PageActivity.Insert);

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

            IDS.Maintenance.UserGroup userGroup = IDS.Maintenance.UserGroup.GetUserGroup(groupCode);

            ViewData["FormAction"] = 2;

            if (userGroup != null)
            {
                return PartialView("Create", userGroup);
            }
            else
            {
                return PartialView("Create", new IDS.Maintenance.UserGroup());
            }
        }

        // POST: Maintenance/UserGroup/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.Maintenance.UserGroup userGroup)
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

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    userGroup.OperatorID = currentUser;

                    userGroup.InsUpDelUserGroup((int)IDS.Tool.PageActivity.Edit);

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

        public ActionResult Delete(string userGroupCodeList)
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

            if (string.IsNullOrWhiteSpace(userGroupCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] userGroupCode = userGroupCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (userGroupCode.Length > 0)
                {
                    IDS.Maintenance.UserGroup userGroup = new IDS.Maintenance.UserGroup();
                    userGroup.InsUpDelUserGroup((int)IDS.Tool.PageActivity.Delete, userGroupCode);
                }

                return Json("User Group data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPrivileges()
        {
            return PartialView("Privileges");
        }


        [HttpPost]
        public JsonResult LoadMntGroupUser()
        {
            JsonResult bj = null;
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var ProjectName = o.SelectToken("ProjectName").ToString();
                var Groupcode = o.SelectToken("Groupcode").ToString();
                System.Data.DataTable dt = IDS.Maintenance.GroupAccess.GetGroupAccess_(ProjectName, Groupcode);
                var studentNamesWithPercentage = dt.AsEnumerable().Select(x => new
                {
                    GroupCode = x.Field<string>("GroupCode"),
                    MenuName = x.Field<string>("MenuName"),
                    FrmName = x.Field<string>("FrmName"),
                    ProjectName = x.Field<string>("ProjectName"),
                    Akses = x.Field<string>("Akses"),
                    GroupName = x.Field<string>("GroupName")
                }).ToList();
                bj = Json(studentNamesWithPercentage, JsonRequestBehavior.AllowGet);
            }
            return bj;
        }

        [HttpPost]
        public JsonResult GetProjectMain()
        {
            return Json(IDS.Maintenance.GroupAccess.GetProjectMain(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPerMission()
        {
            List<System.Web.Mvc.SelectListItem> permis = new List<System.Web.Mvc.SelectListItem>();
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "None", Value = "0" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Read Only", Value = "1" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Create", Value = "2" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Edit", Value = "3" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Delete", Value = "5" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Create Edit", Value = "6" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Create Delete", Value = "7" });
            permis.Add(new System.Web.Mvc.SelectListItem() { Text = "Create Edit Delete", Value = "8" });
            return Json(permis, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MultiSaveToMntGroupAccess()
        {
            JsonResult bj = null;
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var MultiSaveToMntGroupAccess_ = Newtonsoft.Json.JsonConvert.DeserializeObject<IDS.Maintenance.MultiSaveToMntGroupAccess>(json);
                if (IDS.Maintenance.GroupAccess.MultiSaveToMntGroupAccess(MultiSaveToMntGroupAccess_) > 0)
                {
                    bj = Json(new { status = "success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bj = Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
                }
            }
            return bj;
        }
    }
}
