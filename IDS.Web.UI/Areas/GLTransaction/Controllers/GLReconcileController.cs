using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using IDS.Tool;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class GLReconcileController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: GLTransaction/BankReconcile
        [HttpGet]
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                return RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();


            //IDS.Tool.RekeningKoranHeader rek = new Tool.RekeningKoranBCA();
            //rek.ReadCSVFile(HttpContext.Server.MapPath("~/" + "CorpAcctStmt202211325617466.csv"));
            //rek.InsertToDB();



            if (TempData["FormData"] == null)
            {
                ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
                ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");

                if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
                {
                    ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
                }
                else
                {
                    ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
                }

                ViewData["Period"] = DateTime.Now.Date.ToString("MMM yyyy");
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, "2");

                return View();
            }
            else
            {
                IDS.GLTransaction.BankStatement FormData = TempData["FormData"] as IDS.GLTransaction.BankStatement;

                ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", FormData.Currency);
                ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(FormData.Currency), "Value", "Text", FormData.AccountNo);

                if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
                {
                    ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", FormData.BranchCode);
                }
                else
                {
                    ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", FormData.BranchCode);
                }

                ViewData["Period"] = FormData.Period;
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, FormData.MatchStatus.ToString());
                ViewData["RefreshData"] = 1;

                return View(TempData["FormData"] as IDS.GLTransaction.BankStatement);
            }
        }


        // POST: GLTransaction/BankReconcile/Create
        [HttpPost]
        [ActionName("Matching")]
        public ActionResult Index(FormCollection data)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1)
            {
                return RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            StringBuilder sb = new StringBuilder();

            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(data["Currency"]));
            ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(data["Account"]));

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(data["Branch"]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", IDS.Tool.GeneralHelper.NullToString(data["Branch"]));
            }

            ViewData["Period"] = IDS.Tool.GeneralHelper.NullToString(data["Period"], DateTime.Now.ToString("yyyyMM"));

            if (!string.IsNullOrEmpty(data["Status"]))
            {
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, IDS.Tool.GeneralHelper.NullToString(data["Status"], "2"));
            }
            else
            {
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, "2");
            }

            try
            {
                ViewBag.UserMenu = MainMenu;

                if (data == null)
                    throw new Exception("Data is not valid");

                #region Validation
                if (data["Branch"] == null || string.IsNullOrWhiteSpace(data["Branch"]))
                    sb.Append(Environment.NewLine).Append("- Branch code is required");

                if (data["Period"] == null || string.IsNullOrWhiteSpace(data["Period"]))
                    sb.Append(Environment.NewLine).Append("- Period is required");

                if (data["Currency"] == null || string.IsNullOrWhiteSpace(data["Currency"]))
                    sb.Append(Environment.NewLine).Append("- Currency is required");

                if (data["Account"] == null || string.IsNullOrWhiteSpace(data["Account"]))
                    sb.Append(Environment.NewLine).Append("- Account No is required");

                if (sb.Length > 0)
                {
                    sb.Insert(0, "Please check your data:").Append(Environment.NewLine);

                    ViewData["Msg"] = sb.ToString();

                    //return RedirectToAction("Index");
                    return View("Index");
                }
                #endregion

                //// Set Un-match all data
                //IDS.GLTransaction.BankStatement bankStatement = new IDS.GLTransaction.BankStatement();
                //bankStatement.Period = Convert.ToDateTime(data["Period"]).ToPeriod();
                //bankStatement.BranchCode = data["Branch"];
                //bankStatement.Currency = data["Currency"];
                //bankStatement.AccountNo = data["Account"];

                //bankStatement.UpdateMatchStatus(data["bankStatItem"], data["transD"]);

                //IDS.GLTransaction.BankStatement formData = new IDS.GLTransaction.BankStatement() { BranchCode = data["Branch"], AccountNo = data["Account"], Currency = data["Currency"], Period = data["Period"], MatchStatus = string.IsNullOrWhiteSpace(data["MatchStatus"]) ? 2 : Convert.ToInt16(data["Status"]) };
                //TempData["FormData"] = formData;

                IDS.GLTransaction.BankStatement item = new IDS.GLTransaction.BankStatement();
                item.BranchCode = IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]);
                item.AccountNo = IDS.Tool.GeneralHelper.NullToString(data["AccountNo"]);
                item.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                item.AmountBank = IDS.Tool.GeneralHelper.NullToDouble(data["AmountBank"], 0);
                item.DocBank = IDS.Tool.GeneralHelper.NullToString(data["DocBank"]);
                item.Remark = IDS.Tool.GeneralHelper.NullToString(data["Remark"]);
                item.TransDate = IDS.Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date);

                item.UpdateMatchStatus(data[10], data[9]);

                ViewData["Msg"] = "Matching sucessfull.";

                return View("Index");
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadFile()
        {
            HttpFileCollectionBase files = Request.Files;

            var httpParam = HttpContext.Request.Params;
            string branchCode = IDS.Tool.GeneralHelper.NullToString(httpParam["branch"]);
            string accNo = IDS.Tool.GeneralHelper.NullToString(httpParam["Account"]);
            string CCy = IDS.Tool.GeneralHelper.NullToString(httpParam["Currency"]);
            string period = IDS.Tool.GeneralHelper.NullToString(httpParam["period"]);
            string status = IDS.Tool.GeneralHelper.NullToString(httpParam["status"]);


            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1)
            {
                return RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewBag.UserMenu = MainMenu;

            StringBuilder sb = new StringBuilder();

            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(CCy));
            ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(accNo));

            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", IDS.Tool.GeneralHelper.NullToString(branchCode));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", IDS.Tool.GeneralHelper.NullToString(branchCode));
            }

            ViewData["Period"] = IDS.Tool.GeneralHelper.NullToString(period, DateTime.Now.ToString("yyyyMM"));

            if (!string.IsNullOrEmpty(branchCode))
            {
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, IDS.Tool.GeneralHelper.NullToString(status, "2"));
            }
            else
            {
                ViewData["Status"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.Tool.GLBankStatementMatchStatus>(Tool.GLBankStatementMatchStatus.All, true, "2");
            }

            try
            {

                if (string.IsNullOrWhiteSpace(branchCode))
                {
                    ViewData["Msg"] = "Please select branch code";
                    return View("Index");
                }

                if (string.IsNullOrWhiteSpace(accNo))
                {
                    ViewData["Msg"] = "Please select account";
                    return View("Index");
                }

                if (string.IsNullOrWhiteSpace(CCy))
                {
                    ViewData["Msg"] = "Please select currency";
                    return View("Index");
                }

                if (string.IsNullOrWhiteSpace(period))
                {
                    ViewData["Msg"] = "Please select period";
                    return View("Index");
                }

                period = Convert.ToDateTime(period).ToPeriod();


                #region Validation
                if (files.Count == 0)
                {
                    return Json("File has not been set. Please select CSV file to be upload.");
                }

                // Check data apakah sudah ada atau belum yang sudah di match
                bool matchDataExists = IDS.GLTransaction.BankStatement.CheckMatchDataExists(branchCode, period, accNo, CCy);

                if (matchDataExists)
                {
                    ViewData["Msg"] = "Data can not be overwrite while match data exists for this period and account no";
                    return View("Index");
                }

                FileInfo fi = new FileInfo(files[0].FileName);
                if (fi.Extension != ".csv")
                {
                    ViewData["Msg"] = "Invalid filename. Only .csv file allowed";
                    return View("Index");
                }
                #endregion

                if (files[0].ContentLength > 0)
                {
                    string filename = Path.GetFileName(files[0].FileName);
                    string path = Path.Combine(Server.MapPath("~/Upload/BankStatement/"), filename + "_" + DateTime.Now.Date.ToString("yyyyMMMdd"));
                    files[0].SaveAs(path);

                    IDS.Tool.RekeningKoranHeader rek = new Tool.RekeningKoranBCA();
                    rek.ReadCSVFile(path);

                    rek.InsertToDB(branchCode, accNo, CCy, period);
                }

                ViewData["Msg"] = "File uploaded successfully";

                return View("Index", new IDS.GLTransaction.BankStatement() { AccountNo = accNo, BranchCode = branchCode, Currency = CCy, Period = period, MatchStatus = IDS.Tool.GeneralHelper.NullToInt(status, 2) });

            }
            catch (Exception ex)
            {
                ViewData["Msg"] = "File upload failed. Please check your data and re-upload. If problem occured, please contact your administrator.";
                return View("Index", new IDS.GLTransaction.BankStatement() { AccountNo = accNo, BranchCode = branchCode, Currency = CCy, Period = period, MatchStatus = IDS.Tool.GeneralHelper.NullToInt(status, 2) });
            }
        }

        // GET: GLTransaction/BankReconcile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GLTransaction/BankReconcile/Create
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

            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");
            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            return PartialView("Create", new IDS.GLTransaction.BankStatement() { Counter = -1, TransDate = DateTime.Now.Date, Currency = IDS.GeneralTable.Syspar.GetInstance().BaseCCy });
        }

        // POST: GLTransaction/BankReconcile/Create
        [HttpPost]
        public ActionResult Create(FormCollection data)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                    return RedirectToAction("index", "Main", new { area = "" });

                IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

                if (AccessLevel.ReadAccess == -1 || AccessLevel.CreateAccess == 0)
                {
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                    return RedirectToAction("error403", "error", new { area = "" });
                }

                ViewBag.UserMenu = MainMenu;
                ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

                if (data == null)
                    throw new Exception("Data is not valid");

                #region Validation
                if (data["BranchCode"] == null || string.IsNullOrWhiteSpace(data["BranchCode"]))
                    sb.Append(Environment.NewLine).Append("- Branch code is required");

                if (data["TransDate"] == null || string.IsNullOrWhiteSpace(data["TransDate"]))
                    sb.Append(Environment.NewLine).Append("- Transaction date is required");

                if (data["Currency"] == null || string.IsNullOrWhiteSpace(data["Currency"]))
                    sb.Append(Environment.NewLine).Append("- Currency is required");

                if (data["AccountNo"] == null || string.IsNullOrWhiteSpace(data["AccountNo"]))
                    sb.Append(Environment.NewLine).Append("- AccountNo is required");

                if (data["AmountBank"] == null || string.IsNullOrWhiteSpace(data["AmountBank"]))
                    sb.Append(Environment.NewLine).Append("- AmountBank is required");

                if (data["Remark"] == null || string.IsNullOrWhiteSpace(data["Remark"]))
                    sb.Append(Environment.NewLine).Append("- Remark is required");

                if (sb.Length > 0)
                {
                    sb.Insert(0, "Please check your data:").Append(Environment.NewLine);

                    return Json(new { status = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, sb.ToString()) }, JsonRequestBehavior.AllowGet);
                }
                #endregion

                //// Set Un-match all data
                //IDS.GLTransaction.BankStatement bankStatement = new IDS.GLTransaction.BankStatement();
                //bankStatement.Period = Convert.ToDateTime(data["Period"]).ToPeriod();
                //bankStatement.BranchCode = data["Branch"];
                //bankStatement.Currency = data["Currency"];
                //bankStatement.AccountNo = data["Account"];

                //bankStatement.UpdateMatchStatus(data["bankStatItem"], data["transD"]);

                //IDS.GLTransaction.BankStatement formData = new IDS.GLTransaction.BankStatement() { BranchCode = data["Branch"], AccountNo = data["Account"], Currency = data["Currency"], Period = data["Period"], MatchStatus = string.IsNullOrWhiteSpace(data["MatchStatus"]) ? 2 : Convert.ToInt16(data["Status"]) };
                //TempData["FormData"] = formData;

                IDS.GLTransaction.BankStatement item = new IDS.GLTransaction.BankStatement();
                item.BranchCode = IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]);
                item.AccountNo = IDS.Tool.GeneralHelper.NullToString(data["AccountNo"]);
                item.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                item.AmountBank = IDS.Tool.GeneralHelper.NullToDouble(data["AmountBank"], 0);
                item.DocBank = IDS.Tool.GeneralHelper.NullToString(data["DocBank"]);
                item.Remark = IDS.Tool.GeneralHelper.NullToString(data["Remark"]);
                item.TransDate = IDS.Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date);
                item.Period = Convert.ToDateTime(IDS.Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date)).ToPeriod();

                int newCounter = item.InsUpDel((int)IDS.Tool.PageActivity.Insert);

                return Json(new { status = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK, "Data has been save succesfully"), counter = newCounter }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = new HttpStatusCodeResult(System.Net.HttpStatusCode.InternalServerError, ex.Message), counter = -1, }, JsonRequestBehavior.AllowGet);
            }
        }



        // GET: GLTransaction/BankReconcile/Edit/5
        public ActionResult Edit(int? ctrNo, string branch)
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

            ViewData["FormAction"] = 2;

            if (ctrNo == null)
                return PartialView("Create", new IDS.GLTransaction.BankStatement());

            ViewData["ctr"] = ctrNo;
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");
            ViewData["AccList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text");
            if (Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Text", "Value", Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            IDS.GLTransaction.BankStatement bs = IDS.GLTransaction.BankStatement.GetBankStatement(ctrNo, branch);

            if (bs != null)
            {
                return PartialView("Create", bs);
            }
            else
            {
                return PartialView("Create", new IDS.GLTransaction.BankStatement());
            }
        }

        // POST: GLTransaction/BankReconcile/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection data)
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

                ViewData["FormAction"] = 2;

                if (data == null)
                {
                    return Json("Invalid data. Please check your data", JsonRequestBehavior.AllowGet);
                }

                IDS.GLTransaction.BankStatement bs = IDS.GLTransaction.BankStatement.GetBankStatement(((data["Counter"] == "A U T O") ? -1 : IDS.Tool.GeneralHelper.NullToInt(data["Counter"], -1)), IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]));

                if (bs == null)
                {
                    bs = new IDS.GLTransaction.BankStatement();
                }

                bs.BranchCode = IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]);
                bs.Counter = ((data["Counter"] == "A U T O") ? -1 : IDS.Tool.GeneralHelper.NullToInt(data["Counter"], -1));
                bs.TransDate = IDS.Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date);
                bs.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                bs.AccountNo = IDS.Tool.GeneralHelper.NullToString(data["AccountNo"]);
                bs.AmountBank = IDS.Tool.GeneralHelper.NullToDouble(data["AmountBank"], 0);
                bs.Remark = IDS.Tool.GeneralHelper.NullToString(data["Remark"]);
                bs.DocBank = IDS.Tool.GeneralHelper.NullToString(data["DocBank"]);
                bs.InsUpDel((int)IDS.Tool.PageActivity.Edit);

                return Json("Data has been update successfully");
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: GLTransaction/BankReconcile/Delete/5
        [HttpPost]
        public ActionResult Delete(string data)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.DeleteAccess == 0)
            {
                return Json("No Access", JsonRequestBehavior.AllowGet);
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            if (string.IsNullOrWhiteSpace(data))
                return Json("Failed to delete data", JsonRequestBehavior.AllowGet);

            try
            {
                string[] bankStatementData = data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (bankStatementData.Length > 0)
                {
                    IDS.GLTransaction.BankStatement bs = new IDS.GLTransaction.BankStatement();
                    bs.InsUpDel((int)IDS.Tool.PageActivity.Delete, bankStatementData);
                }

                return Json("Bank statement data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBankStatement(string branchCode, string period, string accNo, string ccy, int? status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(period))
                {
                    return Json("Period parameter not found", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(branchCode))
                {
                    return Json("Branch parameter not found", JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrWhiteSpace(accNo))
                {
                    return Json("Account No parameter not found", JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrWhiteSpace(ccy))
                {
                    return Json("Currency parameter not found", JsonRequestBehavior.AllowGet);
                }

                period = IDS.Tool.DateTimeExtension.MonthYearToPeriod(period);

                List<IDS.GLTransaction.BankStatement> result = IDS.GLTransaction.BankStatement.GetBankStatement(period, ccy, accNo, branchCode, status);

                //var data = null;
                //    //result.Where(x => x.Detail != null && x.Detail.Count > 0).SelectMany(x => x.Detail.Select(y =>
                //    //new
                //    //{
                //    //    Voucher = y.Voucher,
                //    //    SourceCode = x.SCode.Code,
                //    //    TransDate = x.TransDate,
                //    //    Amount = y.Amount,
                //    //    CCy = y.CCy.CurrencyCode,
                //    //    Descrip = y.Descrip,
                //    //    Description = x.Description
                //    //}));

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBankStatementBalance(string branch, string period, string accNo, string ccy)
        {
            if (string.IsNullOrWhiteSpace(branch))
                return Json("0|0", JsonRequestBehavior.AllowGet);

            if (string.IsNullOrWhiteSpace(period))
                return Json("0|0", JsonRequestBehavior.AllowGet);

            if (string.IsNullOrWhiteSpace(accNo))
                return Json("0|0", JsonRequestBehavior.AllowGet);

            if (string.IsNullOrWhiteSpace(ccy))
                return Json("0|0", JsonRequestBehavior.AllowGet);

            try
            {
                string result = IDS.GLTransaction.BankStatement.GetBeginningAndEndingBalance(branch, Convert.ToDateTime(period).ToPeriod(), accNo, ccy);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }


    }
}