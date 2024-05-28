using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FATransController : IDS.Web.UI.Controllers.MenuController
    {
        public JsonResult GetData(string branch)
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

                List<IDS.FixedAsset.FATrans> trans = IDS.FixedAsset.FATrans.GetFATrans(branch);

                totalRecords = trans.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    trans = trans.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string lowerSearchValue = searchValue.ToLower();

                    trans = trans.Where(x => Tool.GeneralHelper.NullToString(x.TransNo).ToLower().Contains(lowerSearchValue) ||
                                            Tool.GeneralHelper.NullToString(x.TransCode).ToLower().Contains(lowerSearchValue) ||
                                            Tool.GeneralHelper.NullToString(x.AssetNo).ToLower().Contains(lowerSearchValue) ||
                                            Tool.GeneralHelper.NullToString(x.Currency).ToLower().Contains(lowerSearchValue) ||
                                            Tool.GeneralHelper.NullToDateTime(x.TransDate, DateTime.Now.Date).ToString(Tool.GlobalVariable.DEFAULT_DATE_FORMAT).ToLower().Contains(lowerSearchValue) ||
                                            Tool.GeneralHelper.NullToString(x.Department).ToLower().Contains(lowerSearchValue) ||
                                            x.OperatorID.ToLower().Contains(lowerSearchValue) ||
                                            x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(lowerSearchValue)).ToList();
                }

                totalRecordsShowing = trans.Count();

                // Paging
                if (pageSize > 0)
                    trans = trans.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = trans }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
            }

            return result;
        }

        // GET: FixedAsset/FATrans
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

            ViewData["HO"] = 0;

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["HO"] = 1;
            }

            if (Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] != null && Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            return View();
        }

        // GET: FixedAsset/FATrans/Create
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

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["HO"] = 1;
            }

            List<SelectListItem> branches = IDS.GeneralTable.Branch.GetBranchForDatasource();

            if (Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] != null && Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(branches, "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            ViewData["BranchListAll"] = new SelectList(branches, "Value", "Text");
            ViewData["FATransCodeList"] = new SelectList(IDS.FixedAsset.FATrans.GetFATransCodeList(), "Value", "Text");
            ViewData["AssetList"] = new SelectList(IDS.FixedAsset.FAAsset.getFAAssetNoForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["COAList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["ToBranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["LocationList"] = new SelectList(IDS.GeneralTable.Location.GetLocationDatasource(), "Value", "Text");

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 1;

            return View("Create", new IDS.FixedAsset.FATrans() { TransNo = "< A U T O >", BaseTotalPrice = 0, BookValue = IDS.FixedAsset.FASchedule.GetBookValue(DateTime.Now.ToString("yyyyMM"),""), Currency = IDS.GeneralTable.Syspar.GetInstance().BaseCCy, UnitPrice = 0, TotalPrice = 0 });
        }

        // POST: FixedAsset/FATrans/Create
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
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

                IDS.FixedAsset.FATrans trans = new IDS.FixedAsset.FATrans();
                trans.TransNo = IDS.Tool.GeneralHelper.NullToString(data["TransNo"]);
                trans.TransCode = IDS.Tool.GeneralHelper.NullToString(data["TransCode"]);
                trans.AssetNo = IDS.Tool.GeneralHelper.NullToString(data["AssetNo"]);
                trans.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                trans.Qty = IDS.Tool.GeneralHelper.NullToDecimal(data["Qty"], 0);
                trans.UnitPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["UnitPrice"], 0);
                trans.TotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["TotalPrice"], 0);
                trans.BaseTotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["BaseTotalPrice"], 0);
                trans.FromAcc = IDS.Tool.GeneralHelper.NullToString(data["FromAcc"]);
                trans.ToAcc = IDS.Tool.GeneralHelper.NullToString(data["ToAcc"]);
                trans.BookValue = IDS.Tool.GeneralHelper.NullToDecimal(data["BookValue"], 0);
                trans.AccumDepre = IDS.Tool.GeneralHelper.NullToDecimal(data["AccumDepr"], 0);
                trans.TransDate = Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date);
                trans.Description = IDS.Tool.GeneralHelper.NullToString(data["Description"]);
                trans.FromBranch = IDS.Tool.GeneralHelper.NullToString(data["FromBranch"]);
                trans.ToBranch = IDS.Tool.GeneralHelper.NullToString(data["ToBranch"]);
                trans.Department = IDS.Tool.GeneralHelper.NullToString(data["Department"]);
                trans.Location = IDS.Tool.GeneralHelper.NullToString(data["Location"]);
                trans.SerialNo = IDS.Tool.GeneralHelper.NullToString(data["SerialNo"]);
                trans.Status = IDS.Tool.GeneralHelper.NullToInt(data["Status"], 0);
                trans.VoucherNoFrom = IDS.Tool.GeneralHelper.NullToString(data["VoucherNoFrom"]);
                trans.VoucherNoTo = IDS.Tool.GeneralHelper.NullToString(data["VoucherNoTo"]);
                trans.CancelledVoucher = IDS.Tool.GeneralHelper.NullToString(data["CancelledVoucher"]);
                trans.CancelledVoucherTo = IDS.Tool.GeneralHelper.NullToString(data["CancelledVoucherTo"]);
                trans.MoveOut = IDS.Tool.GeneralHelper.NullToInt(data["MoveOut"], 0);
                trans.OperatorID = IDS.Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_ID]);
                trans.EntryUser = trans.OperatorID;
                trans.LastUpdate = trans.EntryDate = DateTime.Now;

                //asset.AssetVoucher = IDS.Tool.GeneralHelper.NullToString(data["AssetVoucher"]);                
                //asset.Vendor = IDS.Tool.GeneralHelper.NullToString(data["Vendor"]);

                //asset.BusinessUse = IDS.Tool.GeneralHelper.NullToDecimal(data["BusinessUse"], 0);
                //asset.AcquisitionDate = IDS.Tool.GeneralHelper.NullToDateTime(data["AcquisitionDate"], DateTime.Now.Date);
                //asset.StartingDate = IDS.Tool.GeneralHelper.NullToDateTime(data["StatingDate"], DateTime.Now.Date);
                //asset.EndDate = IDS.Tool.GeneralHelper.NullToDateTime(data["EndDate"], DateTime.Now.Date);

                //asset.StatusDate = Tool.GeneralHelper.NullToDateTime(data["StatusDate"], DateTime.Now.Date);
                //asset.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(data["ExchangeRate"], 0);


                bool isValid = this.ValidateInput(trans, sb, Tool.PageActivity.Insert);

                if (!isValid)
                {
                    if (sb.Length > 0)
                    {
                        sb.Insert(0, "Please check your input: ").Append(Environment.NewLine);
                    }

                    throw new Exception(sb.ToString());
                }
                else
                {
                    trans.InsUpDel((int)Tool.PageActivity.Insert);

                    return Json(new { msg = "Success", code = "200", id = trans.TransNo, branchCode = trans.FromBranch }, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Edit", new { asset.AssetNo } );
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                if (sb.Length == 0)
                {
                    sb.Append(ex.Message);
                }

                return Json(new { msg = sb.ToString(), code = 500 }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool ValidateInput(IDS.FixedAsset.FATrans trans, StringBuilder sb, Tool.PageActivity activity)
        {
            bool valid = true;

            if (sb == null)
                sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(trans.TransNo))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Can not find Trans No");
            }

            if (string.IsNullOrWhiteSpace(trans.TransCode))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Transaction code is required");
            }
            else
            {
                switch (trans.TransCode)
                {
                    case "MO":
                        if (string.IsNullOrWhiteSpace(trans.ToAcc))
                        {
                            valid = false;
                            sb.Append(Environment.NewLine).Append("- Account to is required while transaction is Move");
                        }

                        if (string.IsNullOrWhiteSpace(trans.ToBranch))
                        {
                            valid = false;
                            sb.Append(Environment.NewLine).Append("- To branch is required while transaction is Move");
                        }
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace(trans.ToAcc))
                        {
                            valid = false;
                            sb.Append(Environment.NewLine).Append("- Account to must be empty while not Move transaction");
                        }

                        if (!string.IsNullOrWhiteSpace(trans.ToBranch))
                        {
                            valid = false;
                            sb.Append(Environment.NewLine).Append("- Branch to must be empty while not Move transaction");
                        }
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(trans.AssetNo))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Asset No is required");
            }

            #region Sold Transaction Validation
            if (trans.TransCode == "SO")
            {
                if (string.IsNullOrWhiteSpace(trans.Currency))
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Currency is required");
                }

                if (trans.Qty <= 0)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Quantity can not be lower or equals to zero");
                }

                if (trans.UnitPrice <= 0)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Unit price can not be lower or equals to zero");
                }

                if (trans.TotalPrice <= 0)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Total price can not be lower or equals to zero");
                }

                if (trans.BaseTotalPrice <= 0)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Base total price can not be lower or equals to zero");
                }

                if (trans.BookValue < 0)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Residual value can not be lower or equals to zero");
                }
            }
            #endregion

            #region Move Transaction Validation
            if (trans.TransCode == "MO")
            {
                if (string.IsNullOrWhiteSpace(trans.ToBranch))
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Move to branch is required");
                }

                if (string.IsNullOrWhiteSpace(trans.ToAcc))
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- To Acc is required");
                }
            }
            #endregion

            if (string.IsNullOrWhiteSpace(trans.Description))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Description is required");
            }

            if (trans.TransDate > DateTime.Now.Date)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Transaction date can not be greater than today");
            }
            else
            {
                if (trans.TransDate.Date == DateTime.MinValue.Date)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Invalid transaction date");
                }
            }

            return valid;
        }

        // GET: FixedAsset/FATrans/Edit/5
        public ActionResult Edit(string branchCode, string transNo)
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

            if (Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["HO"] = 1;
            }

            List<SelectListItem> branches = IDS.GeneralTable.Branch.GetBranchForDatasource();

            if (Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] != null && Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(branches, "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            IDS.FixedAsset.FATrans trans = IDS.FixedAsset.FATrans.GetFATrans(branchCode, transNo);

            ViewData["BranchListAll"] = new SelectList(branches, "Value", "Text");
            ViewData["FATransCodeList"] = new SelectList(IDS.FixedAsset.FATrans.GetFATransCodeList(), "Value", "Text");
            ViewData["AssetList"] = new SelectList(IDS.FixedAsset.FAAsset.getFAAssetNoForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", trans.AssetNo);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", trans.Currency);
            ViewData["COAList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(trans.Currency), "Value", "Text", trans.Currency);
            ViewData["ToBranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", trans.ToBranch);
            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(trans.FromBranch), "Value", "Text", trans.Department);
            ViewData["LocationList"] = new SelectList(IDS.GeneralTable.Location.GetLocationDatasource(), "Value", "Text", trans.Location);

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 2;

            ViewData["Processed"] = IDS.FixedAsset.FATrans.GetFATransStatus(branchCode, transNo);

            return View("Create", trans);
        }

        // POST: FixedAsset/FATrans/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection data)
        {
            try
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

                    if (Tool.GeneralHelper.NullToInt(data["Processed"], 1) > 0)
                    {
                        throw new Exception("Data has been processed / cancelled and can not be edit");
                    }

                    IDS.FixedAsset.FATrans asset = new IDS.FixedAsset.FATrans();
                    asset.TransNo = IDS.Tool.GeneralHelper.NullToString(data["TransNo"]);
                    asset.TransCode = IDS.Tool.GeneralHelper.NullToString(data["TransCode"]);
                    asset.AssetNo = IDS.Tool.GeneralHelper.NullToString(data["AssetNo"]);
                    asset.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                    asset.Qty = IDS.Tool.GeneralHelper.NullToDecimal(data["Qty"], 0);
                    asset.UnitPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["UnitPrice"], 0);
                    asset.TotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["TotalPrice"], 0);
                    asset.BaseTotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["BaseTotalPrice"], 0);
                    asset.FromAcc = IDS.Tool.GeneralHelper.NullToString(data["FromAcc"]);
                    asset.ToAcc = IDS.Tool.GeneralHelper.NullToString(data["ToAcc"]);
                    asset.BookValue = IDS.Tool.GeneralHelper.NullToDecimal(data["BookValue"], 0);
                    asset.AccumDepre = IDS.Tool.GeneralHelper.NullToDecimal(data["AccumDepr"], 0);
                    asset.TransDate = Tool.GeneralHelper.NullToDateTime(data["TransDate"], DateTime.Now.Date);
                    asset.Description = IDS.Tool.GeneralHelper.NullToString(data["Description"]);
                    asset.FromBranch = IDS.Tool.GeneralHelper.NullToString(data["FromBranch"]);
                    asset.ToBranch = IDS.Tool.GeneralHelper.NullToString(data["ToBranch"]);
                    asset.Department = IDS.Tool.GeneralHelper.NullToString(data["Department"]);
                    asset.Location = IDS.Tool.GeneralHelper.NullToString(data["Location"]);
                    asset.SerialNo = IDS.Tool.GeneralHelper.NullToString(data["SerialNo"]);
                    asset.Status = IDS.Tool.GeneralHelper.NullToInt(data["Status"], 0);
                    asset.VoucherNoFrom = IDS.Tool.GeneralHelper.NullToString(data["VoucherNoFrom"]);
                    asset.VoucherNoTo = IDS.Tool.GeneralHelper.NullToString(data["VoucherNoTo"]);
                    asset.CancelledVoucher = IDS.Tool.GeneralHelper.NullToString(data["CancelledVoucher"]);
                    asset.CancelledVoucherTo = IDS.Tool.GeneralHelper.NullToString(data["CancelledVoucherTo"]);
                    asset.MoveOut = IDS.Tool.GeneralHelper.NullToInt(data["MoveOut"], 0);
                    asset.OperatorID = IDS.Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_ID]);
                    asset.EntryUser = asset.OperatorID;
                    asset.LastUpdate = asset.EntryDate = DateTime.Now;

                    //asset.AssetVoucher = IDS.Tool.GeneralHelper.NullToString(data["AssetVoucher"]);                
                    //asset.Vendor = IDS.Tool.GeneralHelper.NullToString(data["Vendor"]);

                    //asset.BusinessUse = IDS.Tool.GeneralHelper.NullToDecimal(data["BusinessUse"], 0);
                    //asset.AcquisitionDate = IDS.Tool.GeneralHelper.NullToDateTime(data["AcquisitionDate"], DateTime.Now.Date);
                    //asset.StartingDate = IDS.Tool.GeneralHelper.NullToDateTime(data["StatingDate"], DateTime.Now.Date);
                    //asset.EndDate = IDS.Tool.GeneralHelper.NullToDateTime(data["EndDate"], DateTime.Now.Date);

                    //asset.StatusDate = Tool.GeneralHelper.NullToDateTime(data["StatusDate"], DateTime.Now.Date);
                    //asset.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(data["ExchangeRate"], 0);


                    bool isValid = this.ValidateInput(asset, sb, Tool.PageActivity.Insert);

                    if (!isValid)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Insert(0, "Please check your input: ").Append(Environment.NewLine);
                        }

                        throw new Exception(sb.ToString());
                    }
                    else
                    {
                        asset.InsUpDel((int)Tool.PageActivity.Insert);

                        return Json(new { msg = "Success", code = "200", id = asset.TransNo, branchCode = asset.FromBranch }, JsonRequestBehavior.AllowGet);
                        //return RedirectToAction("Edit", new { asset.AssetNo } );
                    }
                }
                catch (Exception ex)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                    if (sb.Length == 0)
                    {
                        sb.Append(ex.Message);
                    }

                    return Json(new { msg = sb.ToString(), code = 500 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return View();
            }
        }

        // POST: FixedAsset/FATrans/Delete/5
        [HttpPost]
        public ActionResult Delete(string transList)
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

            if (string.IsNullOrWhiteSpace(transList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] transCode = transList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (transCode.Length > 0)
                {
                    IDS.FixedAsset.FATrans trans = new IDS.FixedAsset.FATrans();
                    trans.OperatorID = Convert.ToString(Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE]);
                    trans.InsUpDel(transCode);
                }

                return Json("Fixed asset data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
