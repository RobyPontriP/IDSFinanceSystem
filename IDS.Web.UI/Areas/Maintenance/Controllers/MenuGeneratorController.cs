using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class MenuGeneratorController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.Maintenance.UserMenu> userMenu = IDS.Maintenance.UserMenu.GetAllUserMenu();

                totalRecords = userMenu.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    userMenu = userMenu.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    userMenu = userMenu.Where(x => x.MenuNumber.ToString().ToLower().Contains(searchValueLower) ||
                                             x.MenuCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.MenuProject.ToLower().Contains(searchValueLower) ||
                                             x.MenuLevel.ToString().ToLower().Contains(searchValueLower) ||
                                             x.MenuName.ToLower().Contains(searchValueLower) ||
                                             x.MenuURL.ToLower().Contains(searchValueLower) ||
                                             x.MenuToolTip.ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = userMenu.Count();

                //Paging
                userMenu = userMenu.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data    
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = userMenu }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return result;
        }

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

            //return PartialView("Index");

            return View();
        }

        [HttpGet, AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Create()
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

            ViewData["ProjectList"] = IDS.Maintenance.UserMenuProject.GetUserMenuProjectForDatasource();
            ViewData["MenuLevel"] = new List<SelectListItem>() {
                new SelectListItem() { Value = "0", Text = "0" },
                new SelectListItem() { Value = "1", Text = "1" },
                new SelectListItem() { Value = "2", Text = "2" },
                new SelectListItem() { Value = "3", Text = "3" },
                new SelectListItem() { Value = "4", Text = "4" },
                new SelectListItem() { Value = "5", Text = "5" },
                new SelectListItem() { Value = "6", Text = "6" },
                new SelectListItem() { Value = "7", Text = "7" }
            };
            ViewData["ParentMenu"] = new List<SelectListItem>();


            ViewData["FormAction"] = 1;
            return PartialView("Create", new IDS.Maintenance.UserMenu());
        }

        //public string CreateData()
        //{
        //    string hasil="";
        //    System.IO.Stream req = Request.InputStream;
        //    req.Seek(0, System.IO.SeekOrigin.Begin);
        //    string json = new System.IO.StreamReader(req).ReadToEnd();
        //    System.Diagnostics.Debug.WriteLine(json);
        //    return hasil;
        //}

        public string CreateData()
        {
            string hasil = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var userMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<IDS.Maintenance.UserMenu>(json);
                // System.Diagnostics.Debug.WriteLine(userMenu.MenuName);

                //System.Diagnostics.Debug.WriteLine("Response From Server! -->"+userMenu.MenuProject);
                IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

                //if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
                //{
                //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                //   // return RedirectToAction("error403", "error", new { area = "" });
                //}

                ViewData["Page.Insert"] = AccessLevel.CreateAccess;
                ViewData["Page.Edit"] = AccessLevel.EditAccess;
                ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

                //if (ModelState.IsValid)
                //{
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                //if (string.IsNullOrWhiteSpace(currentUser))
                //{
                //    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                //}

                try
                {
                    userMenu.OperatorID = userMenu.EntryUser = currentUser;

                    switch (userMenu.MenuProject.ToUpper())
                    {
                        case "REPORTS":
                            break;
                        default:
                            if (!string.IsNullOrWhiteSpace(userMenu.Area) && !string.IsNullOrWhiteSpace(userMenu.Controller))
                            {
                                userMenu.MenuURL = userMenu.Area + '/' + userMenu.Controller;
                            }
                            else if (string.IsNullOrWhiteSpace(userMenu.Area) && !string.IsNullOrWhiteSpace(userMenu.Controller))
                            {
                                userMenu.MenuURL = userMenu.Controller;
                            }
                            break;
                    }
                    userMenu.InsUpDel(IDS.Tool.PageActivity.Insert);
                    string returnjson = "{'status':'Success'}";
                    hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                }
                catch (Exception ex)
                {
                    string returnjson = "{'status':'Error'}";
                    hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                }

                //string output = isAdmin ? "Welcome to the Admin User" : "Welcome to the User";
                //return Json(output, JsonRequestBehavior.AllowGet);

                //}
                //else
                //{
                //    return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
                //}
            }
            return hasil;
        }

        //[HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(int? FormAction, IDS.Maintenance.UserMenu userMenu)
        //{
        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

        //    IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
        //    {
        //        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        //        return RedirectToAction("error403", "error", new { area = "" });
        //    }

        //    ViewData["Page.Insert"] = AccessLevel.CreateAccess;
        //    ViewData["Page.Edit"] = AccessLevel.EditAccess;
        //    ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

        //    if (ModelState.IsValid)
        //    {
        //        string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

        //        if (string.IsNullOrWhiteSpace(currentUser))
        //        {
        //            return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
        //        }

        //        try
        //        {
        //            userMenu.OperatorID = userMenu.EntryUser = currentUser;

        //            switch (userMenu.MenuProject.ToUpper())
        //            {
        //                case "REPORTS":                            
        //                    break;
        //                default:
        //                    if (!string.IsNullOrWhiteSpace(userMenu.Area) && !string.IsNullOrWhiteSpace(userMenu.Controller))
        //                    {
        //                        userMenu.MenuURL = userMenu.Area + '/' + userMenu.Controller;
        //                    }
        //                    else if (string.IsNullOrWhiteSpace(userMenu.Area) && !string.IsNullOrWhiteSpace(userMenu.Controller))
        //                    {
        //                        userMenu.MenuURL = userMenu.Controller;
        //                    }
        //                    break;
        //            }

        //            userMenu.InsUpDel(IDS.Tool.PageActivity.Insert);

        //            return Json("Success", JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Json("Not Valid Mode", JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult GetMenuParent(string MenuProject, int level)
        {
            List<IDS.Maintenance.UserMenu> menuName = IDS.Maintenance.UserMenu.GetUserMenuByProjectAndLevel(MenuProject, level == 0 ? 255 : level - 1);
            return Json(menuName, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, AcceptVerbs(HttpVerbs.Get)]
        // GET: GeneralTable/Country/Edit/5
        public ActionResult Edit(string menuNumber)
        {
            try
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

                int menuNumberInt = Convert.ToInt32(menuNumber);

                IDS.Maintenance.UserMenu userMenu = IDS.Maintenance.UserMenu.GetUserMenuByMenuNumber(menuNumberInt);

                ViewData["ProjectList"] = IDS.Maintenance.UserMenuProject.GetUserMenuProjectForDatasource();
                ViewData["MenuLevel"] = new List<SelectListItem>() {
                    new SelectListItem() { Value = "0", Text = "0" },
                    new SelectListItem() { Value = "1", Text = "1" },
                    new SelectListItem() { Value = "2", Text = "2" },
                    new SelectListItem() { Value = "3", Text = "3" },
                    new SelectListItem() { Value = "4", Text = "4" },
                    new SelectListItem() { Value = "5", Text = "5" },
                    new SelectListItem() { Value = "6", Text = "6" },
                    new SelectListItem() { Value = "7", Text = "7" }
                };

                ViewData["ParentMenu"] = new List<SelectListItem>();

                ViewData["FormAction"] = 2;

                if (userMenu != null)
                {
                    return PartialView("Create", userMenu);
                }
                else
                {
                    return PartialView("Create", new IDS.Maintenance.UserMenu());
                }
            }
            catch
            {
                return null;
            }
        }

        // [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public string Edit()
        {
            string hasil = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var userMenu = Newtonsoft.Json.JsonConvert.DeserializeObject<IDS.Maintenance.UserMenu>(json);
                IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

                ViewData["Page.Insert"] = AccessLevel.CreateAccess;
                ViewData["Page.Edit"] = AccessLevel.EditAccess;
                ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                IDS.Maintenance.UserMenu menu = new IDS.Maintenance.UserMenu();
                menu.EntryDate = menu.LastUpdate = DateTime.Now;
                menu.EntryUser = menu.OperatorID = currentUser;
                menu.MenuProject = o.SelectToken("MenuProject").ToString();
                menu.MenuLevel = int.Parse(o.SelectToken("MenuLevel").ToString());
                menu.MenuParentCode = o.SelectToken("MenuParentCode").ToString();
                menu.MenuCode = o.SelectToken("MenuCode").ToString();
                menu.MenuName = o.SelectToken("MenuName").ToString();
                menu.Controller = o.SelectToken("Controller").ToString();
                menu.Area = o.SelectToken("Area").ToString();
                menu.MenuURL = o.SelectToken("MenuURL").ToString();
                menu.MenuToolTip = o.SelectToken("MenuToolTip").ToString();
                menu.MenuNumber = IDS.Maintenance.UserMenu.GetMenuNumberFromMenuName(menu.MenuName);

                //ModelState.Clear();

                //ValidateModel(menu);
                try
                {
                    menu.OperatorID = currentUser;

                    menu.InsUpDel(IDS.Tool.PageActivity.Edit);
                    string returnjson = "{'status':'Success'}";
                    hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                }
                catch (Exception ex)
                {
                    string returnjson = "{'status':'Error'}";
                    hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                }

                //if (ModelState.IsValid)
                //{
                //    //string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                //    if (string.IsNullOrWhiteSpace(currentUser))
                //    {
                //        string returnjson = "{'status':'Error'}";
                //        hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                //    }


                //}
                //else
                //{
                //    string returnjson = "{'status':'Error'}";
                //    hasil = Newtonsoft.Json.JsonConvert.SerializeObject(returnjson);
                //}
            }
            //if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
            //    return RedirectToAction("index", "Main", new { area = "" });
            return hasil;

        }

        //public ActionResult Edit(IDS.Maintenance.UserMenu menu)
        //{
        //    if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
        //        return RedirectToAction("index", "Main", new { area = "" });

        //    IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

        //    if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
        //    {
        //        return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        //    }

        //    ViewData["Page.Insert"] = AccessLevel.CreateAccess;
        //    ViewData["Page.Edit"] = AccessLevel.EditAccess;
        //    ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

        //    string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

        //    menu.EntryDate = menu.LastUpdate = DateTime.Now;
        //    menu.EntryUser = menu.OperatorID = currentUser;

        //    ModelState.Clear();

        //    ValidateModel(menu);

        //    if (ModelState.IsValid)
        //    {
        //        //string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

        //        if (string.IsNullOrWhiteSpace(currentUser))
        //        {
        //            return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
        //        }

        //        try
        //        {
        //            menu.OperatorID = currentUser;

        //            menu.InsUpDel(IDS.Tool.PageActivity.Edit);

        //            return Json("Success", JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
        //    }
        //}


        //[ActionName("GetMenuParentName")]
        //public JsonResult GetMenuName(string MenuProject)
        //{
        //    List<string> menuName = IDS.Maintenance.UserMenu.GetMenuName(MenuProject, 0);
        //    return Json(menuName, JsonRequestBehavior.AllowGet);
        //}

        //[ActionName("GetMenuParentNameWithLevel")]
        //public JsonResult GetMenuName(string MenuProject, int MenuLevel)
        //{
        //    List<string> menuName = IDS.Maintenance.UserMenu.GetMenuName(MenuProject, MenuLevel);
        //    return Json(menuName, JsonRequestBehavior.AllowGet);
        //}

        //// GET: Maintenance/MenuGenerator/Edit/5
        //public ActionResult AddMenu()
        //{
        //    List<string> menuProjectList = IDS.Maintenance.UserMenu.GetMenuProject();

        //    ViewBag.menuProject_list = menuProjectList;
        //    return View();
        //}

        //var result = new { Result = "Successed", ID = "32" };

        public JsonResult DeleteMenuGenerator()
        {
            var result = new { status = "Error" };
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {

                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var MenuNumber = o.SelectToken("MenuNumber").ToString();
                var MenuCode = o.SelectToken("MenuCode").ToString();
                var MenuName = o.SelectToken("MenuName").ToString();
                if (IDS.Maintenance.UserMenu.DeleteMenuFromMenuNumber(MenuNumber, MenuCode, MenuName))
                {
                    var resultok = new { status = "Success" };
                    result = resultok;
                }
                else
                {
                    var resulterror = new { status = "Error" };
                    result = resulterror;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddMenuu(IDS.Maintenance.UserMenu userMenu)
        {
            int startIndex = userMenu.MenuLevel * 2 + 2;
            string StrMenuCode = userMenu.MenuCode.Substring(0, startIndex);
            StrMenuCode = StrMenuCode.Substring(1);

            int menuCod = Convert.ToInt32(StrMenuCode);
            string status = "Successfully Added Menu";
            try
            {
                //country.LastUpdate = DateTime.Now;
                //if (IDS.GeneralTable.Country.InsertCountry(country, 0))
                //{

                //}
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}
