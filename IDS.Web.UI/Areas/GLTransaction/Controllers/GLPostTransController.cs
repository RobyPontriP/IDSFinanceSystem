using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GLTransaction.Controllers
{
    public class GLPostTransController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string branchCode, string dateFrom, string dateTo)
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

                //int pageSize = (length != null ? Convert.ToInt32(length) : 0);
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

                List<IDS.GLTransaction.GLVoucherH> postTrans = IDS.GLTransaction.GLVoucherH.GetPostTrans(branchCode, Convert.ToDateTime(dateFrom),Convert.ToDateTime(dateTo));

                totalRecords = postTrans.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)) && sortColumn != "chkProcess")
                {
                    postTrans = postTrans.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    postTrans = postTrans.Where(x => x.SCode.Code.ToLower().Contains(searchValue) ||
                                             x.Voucher.ToLower().Contains(searchValue) ||
                                             x.Entry_Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT).Contains(searchValue) ||
                                             x.TransDate.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT).Contains(searchValue) ||
                                             x.Description.ToLower().Contains(searchValue) ||
                                             x.OperatorID.ToLower().Contains(searchValueLower) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = postTrans.Count();

                // Paging
                if (pageSize > 0)
                    postTrans = postTrans.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = postTrans }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public JsonResult GetDataDetail(string branchCode, string srcCode, string voucher)
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

                //int pageSize = (length != null ? Convert.ToInt32(length) : 0);
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

                //List<IDS.GLTransaction.GLVoucherH> vou = IDS.GLTransaction.GLVoucherH.GetVoucherWithDetail(branchCode, srcCode, dateFrom, dateTo, dateType);
                List<IDS.GLTransaction.GLVoucherD> voucherDetail = IDS.GLTransaction.GLVoucherD.GetVoucherDetail(branchCode, srcCode, voucher, 3);

                totalRecords = voucherDetail.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    voucherDetail = voucherDetail.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    voucherDetail = voucherDetail.Where(x => x.Counter.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.COA.Account.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.CCy.CurrencyCode.ToLower().Contains(searchValueLower) ||
                                                 x.Dept.DepartmentCode.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.Descrip.ToString().ToLower().Contains(searchValueLower) ||
                                                 x.DocumentNo.ToString().ToLower().Contains(searchValueLower)).ToList();

                }
                totalRecordsShowing = voucherDetail.Count();
                // Paging
                if (pageSize > 0)
                    voucherDetail = voucherDetail.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data                
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = voucherDetail }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        // GET: GLTransaction/GLPostTrans
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

            ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));

            DateTime now = DateTime.Now;
            ViewData["CurrDate"] = now.Date.ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);
            ViewData["FirstDayOfMonth"] = new DateTime(now.Year, now.Month, 1).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT);

            ViewData["dtFrom"] = "";
            ViewData["dtTo"] = "";
            return PartialView("index");
        }

        public ActionResult Posting(string branchCodeList, string scodeList, string voucherList, string rpList)
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

            //if (string.IsNullOrWhiteSpace(citiesCodeList))
            //    return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] branchCode = branchCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] scode = scodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] voucher = voucherList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] rp = rpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                string msgError = "";
                for (int i = 0; i < voucher.Length; i++)
                {
                    IDS.GLTransaction.GLVoucherH vouH = new IDS.GLTransaction.GLVoucherH();

                    msgError += vouH.PostTrans(branchCode[i], scode[i], voucher[i], rp[i], Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString());
                }

                return Json(msgError, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}