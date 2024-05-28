using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.Maintenance.Controllers
{
    public class UserController : IDS.Web.UI.Controllers.MenuController
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

                IList<IDS.Maintenance.User> user = IDS.Maintenance.User.GetUser();

                totalRecords = user.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    user = user.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    user = user.Where(x => x.UserID.ToString().ToLower().Contains(searchValueLower) ||
                                             x.UserName.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Branch.BranchCode.ToLower().Contains(searchValueLower) ||
                                             x.UserGroup.GroupCode.ToString().ToLower().Contains(searchValueLower) ||
                                             x.EmailAddress.ToLower().Contains(searchValueLower) ||
                                             x.ExpiredCode.ToLower().Contains(searchValueLower) ||
                                             x.Status.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = user.Count();

                //Paging
                user = user.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data    
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = user }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
            }
            return result;
        }
        
        // GET: Maintenance/User
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

            ViewData["Branch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text");

            // Pak Fajar
            ViewData["ListUser"] = Newtonsoft.Json.JsonConvert.SerializeObject(GetDataTable());
            //
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Index");
        }

        // GET: Maintenance/User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Maintenance/User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maintenance/User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Maintenance/User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Maintenance/User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Maintenance/User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Maintenance/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private System.Data.DataTable GetDataTable()
        {
            System.Data.DataTable dt_ = new System.Data.DataTable();
            dt_.Clear();
            dt_.Columns.Add("userid");
            dt_.Columns.Add("username");
            dt_.Columns.Add("email");
            dt_.Columns.Add("group");
            dt_.Columns.Add("datecreated");
            dt_.Columns.Add("expired");
            dt_.Columns.Add("accum");
            dt_.Columns.Add("active");
            IDS.Maintenance.User.GetUserFromBranch("", dt_);
            return dt_;
        }

        public string RefreshUser()
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var Branch = o.SelectToken("Branch").ToString();

                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(GetDataTable(), Newtonsoft.Json.Formatting.Indented);

            }
            return return_;
        }
        private System.Data.DataTable GetDataTable2(string branch)
        {
            System.Data.DataTable dt_ = new System.Data.DataTable();
            dt_.Clear();
            dt_.Columns.Add("userid");
            dt_.Columns.Add("username");
            dt_.Columns.Add("email");
            dt_.Columns.Add("group");
            dt_.Columns.Add("datecreated");
            dt_.Columns.Add("expired");
            dt_.Columns.Add("accum");
            dt_.Columns.Add("active");
            IDS.Maintenance.User.GetUserFromBranch(branch, dt_);
            return dt_;
        }
        public string RefreshUser2()
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var Branch = o.SelectToken("Branch").ToString();

                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(GetDataTable2(Branch), Newtonsoft.Json.Formatting.Indented);

            }
            return return_;
        }

        public JsonResult GetSecurityCode()
        {
            return Json(IDS.Maintenance.User.GetSecurityCode(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Getgroupcode()
        {
            return Json(IDS.Maintenance.User.Getgroupcode(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranch()
        {
            return Json(IDS.GeneralTable.Branch.GetBranchForDatasource(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExp()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "7 Day", Value = "7D" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "14 Day", Value = "14D" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "1 Month", Value = "1M" });
            return Json(RP, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUser()
        {
            ReplayToClient c = new ReplayToClient();
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var userid = o.SelectToken("userid").ToString();
                var username = o.SelectToken("username").ToString();
                var password = o.SelectToken("password").ToString();
                var email = o.SelectToken("email").ToString();
                var group = o.SelectToken("group").ToString();
                var AMGroupCode = o.SelectToken("amgroupcode").ToString();
                var date = o.SelectToken("date").ToString();
                var exp = o.SelectToken("exp").ToString();
                var scode = o.SelectToken("scode").ToString();
                var sansw = o.SelectToken("sansw").ToString();
                var akumulasi = Tool.GeneralHelper.NullToInt(o.SelectToken("akumulasi"), 0);
                var status = o.SelectToken("status").ToString();
                var branch = o.SelectToken("branch").ToString();

                if (IDS.Maintenance.User.UserExist(userid))
                {
                    c.msg_ = "Data Already Exist!";
                    c.response_ = "error";
                }
                else
                {
                    if (IDS.Maintenance.User.SaveUser(userid, username, new Tool.clsCryptho().Encrypt(password, "ids"), email, group, stringToDatetime(date), exp, scode, sansw, akumulasi, Convert.ToBoolean(status), branch, Session[Tool.GlobalVariable.SESSION_USER_ID].ToString()))
                    {
                        c.msg_ = "success";
                        c.response_ = "ok";
                    }
                    else
                    {
                        c.msg_ = "Same thing Wrong!";
                        c.response_ = "error";
                    }
                }
            }
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        private static System.DateTime stringToDatetime(string dateString)
        {
            System.DateTime return_ = DateTime.Now;
            string dateTime = dateString;
            if (IsvalidDatetIme(dateTime))
            {
                return_ = System.Convert.ToDateTime(dateTime);
            }
            return return_;
        }

        private static bool IsvalidDatetIme(string s)
        {
            bool return_ = false;
            try
            {
                System.Convert.ToDateTime(s);
                return_ = true;
            }
            catch (Exception x)
            {
                return_ = false;
            }
            return return_;
        }

        public string GetUserByUserId()
        {
            string return_ = "Eesponse From Server!";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var userid = o.SelectToken("userid").ToString();
                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(IDS.Maintenance.User.GetUserFromId(userid), Newtonsoft.Json.Formatting.Indented);
            }
            return return_;
        }

        public JsonResult UpdateUser()
        {
            ReplayToClient c = new ReplayToClient();
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var userid = o.SelectToken("userid").ToString();
                var username = o.SelectToken("username").ToString();
                var password = o.SelectToken("password").ToString();
                var email = o.SelectToken("email").ToString();
                var group = o.SelectToken("group").ToString();
                var AMGroupCode = o.SelectToken("amgroupcode").ToString();
                var date = o.SelectToken("date").ToString();
                var exp = o.SelectToken("exp").ToString();
                var scode = o.SelectToken("scode").ToString();
                var sansw = o.SelectToken("sansw").ToString();
                var akumulasi = o.SelectToken("akumulasi").ToString();
                var status = o.SelectToken("status").ToString();
                var branch = o.SelectToken("branch").ToString();

                if (IDS.Maintenance.User.UpdateUser(userid, username, password, email, group, stringToDatetime(date), exp, scode, sansw, akumulasi, Convert.ToBoolean(status), branch, AMGroupCode, userid))
                {
                    c.msg_ = "success";
                    c.response_ = "ok";
                }
                else
                {
                    c.msg_ = "Same thing Wrong!";
                    c.response_ = "error";
                }

            }
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        public string DeleteUserId()
        {
            string return_ = "{'msg_':'data cant be deleted','response_':'error'}";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (Tool.GeneralHelper.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var userid = o.SelectToken("userid").ToString();
                if (IDS.Maintenance.User.DeleteUserId(userid))
                {
                    return_ = Newtonsoft.Json.JsonConvert.SerializeObject(new ReplayToClient() { msg_ = "data hasbeen deleted!", response_ = "ok" });
                }
            }
            return return_;
        }

        public JsonResult GetBranchStatus()
        {
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            string ho_ = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS].ToString();
            string branch_ = Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            return Json(new { ho = ho_, branch = branch_ }, JsonRequestBehavior.AllowGet);
        }
    }
    public class ReplayToClient
    {
        public string response_ { get; set; }
        public string msg_ { get; set; }

    }
}
