using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FAAssetController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.FixedAsset.FAAsset> asset = IDS.FixedAsset.FAAsset.GetFAAsset(branch);

                totalRecords = asset.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    asset = asset.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string lowerSearchValue = searchValue.ToLower();

                    asset = asset.Where(x => Tool.GeneralHelper.NullToString(x.AssetNo).ToLower().Contains(lowerSearchValue) ||
                                             Tool.GeneralHelper.NullToString(x.Description).ToLower().Contains(lowerSearchValue) ||

                                             ((IDS.FixedAsset.FAAssetStatus)x.Status).ToString().Replace("_", " ").ToLower().Contains(lowerSearchValue) ||
                                             x.OperatorID.ToLower().Contains(lowerSearchValue) ||
                                             x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(lowerSearchValue)).ToList();
                }

                totalRecordsShowing = asset.Count();

                // Paging
                if (pageSize > 0)
                    asset = asset.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = asset }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        // GET: FixedAsset/FAAsset
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

        // GET: FixedAsset/FAAsset/Create
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

            if (Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] != null && Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["LocationList"] = new SelectList(IDS.GeneralTable.Location.GetLocationDatasource(), "Value", "Text", null);
            ViewData["FAGroupList"] = new SelectList(IDS.FixedAsset.FAGroup.getFAGroupForDatasource(), "Value", "Text", null);
            ViewData["FATaxCategory"] = new SelectList(IDS.FixedAsset.FATaxCategory.GetFATaxCategoryForDatasource(), "Value", "Text", null);

            ViewData["FAPartOf"] = new SelectList(IDS.FixedAsset.FAAsset.getFAAssetForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["VendorList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text", null);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["FAAssetStatus"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FAAssetStatus>(IDS.FixedAsset.FAAssetStatus.Active);


            ViewData["COAList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text", null);
            ViewData["DepreMethodList"] = new SelectList(IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FADepreMethod>(IDS.FixedAsset.FADepreMethod.Double_Declining), "Value", "Text", null);

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 1;

            return View("Create", new IDS.FixedAsset.FAAsset() { Qty = 1, BusinessUse = 100, ExchangeRate = 1, UnitPrice = 0, TotalPrice = 0, BaseTotalPrice = 0, AssetNo = "< A U T O >" });
        }

        // POST: FixedAsset/FAAsset/Create
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

                IDS.FixedAsset.FAAsset asset = new IDS.FixedAsset.FAAsset();
                asset.AssetNo = IDS.Tool.GeneralHelper.NullToString(data["AssetNo"]);
                asset.BranchCode = IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]);
                asset.Department = IDS.Tool.GeneralHelper.NullToString(data["Department"]);
                asset.Location = IDS.Tool.GeneralHelper.NullToString(data["Location"]);
                asset.PartOf = IDS.Tool.GeneralHelper.NullToString(data["PartOf"]);

                if (data["Group.Code"] != null)
                {
                    asset.Group = new IDS.FixedAsset.FAGroup();
                    asset.Group.Code = IDS.Tool.GeneralHelper.NullToString(data["Group.Code"]);
                }

                asset.TaxCategoryID = IDS.Tool.GeneralHelper.NullToString(data["TaxCategoryID"]);
                asset.Description = IDS.Tool.GeneralHelper.NullToString(data["Description"]);
                asset.AssetVoucher = IDS.Tool.GeneralHelper.NullToString(data["AssetVoucher"]);
                asset.SerialNo = IDS.Tool.GeneralHelper.NullToString(data["SerialNo"]);
                asset.Vendor = IDS.Tool.GeneralHelper.NullToString(data["Vendor"]);
                asset.Qty = IDS.Tool.GeneralHelper.NullToDecimal(data["Qty"], 0);
                asset.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                asset.UnitPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["UnitPrice"], 0);
                asset.ResidualValue = IDS.Tool.GeneralHelper.NullToDecimal(data["ResidualValue"], 0);
                asset.TotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["TotalPrice"], 0);
                asset.BaseTotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["BaseTotalPrice"], 0);
                asset.BusinessUse = IDS.Tool.GeneralHelper.NullToDecimal(data["BusinessUse"], 0);
                asset.AcquisitionDate = IDS.Tool.GeneralHelper.NullToDateTime(data["AcquisitionDate"], DateTime.Now.Date);
                asset.StartingDate = IDS.Tool.GeneralHelper.NullToDateTime(data["StatingDate"], DateTime.Now.Date);
                asset.EndDate = IDS.Tool.GeneralHelper.NullToDateTime(data["EndDate"], DateTime.Now.Date);
                asset.Status = (IDS.FixedAsset.FAAssetStatus)Enum.Parse(typeof(IDS.FixedAsset.FAAssetStatus), Tool.GeneralHelper.NullToString(data["Status"]));
                asset.StatusDate = Tool.GeneralHelper.NullToDateTime(data["StatusDate"], DateTime.Now.Date);
                asset.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(data["ExchangeRate"], 0);
                asset.OperatorID = IDS.Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_ID]);
                asset.EntryUser = asset.OperatorID;
                asset.LastUpdate = asset.EntryDate = DateTime.Now;

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

                    return Json(new { msg = "Success", code = "200", id = asset.AssetNo, branchCode = asset.BranchCode }, JsonRequestBehavior.AllowGet);
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

        // GET: FixedAsset/FAAsset/Edit/5
        public ActionResult Edit(string assetNo, string branchCode)
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

            if (Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS] != null && Convert.ToBoolean(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true)
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }
            else
            {
                ViewData["BranchList"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]));
            }

            ViewData["DeptList"] = new SelectList(IDS.GeneralTable.Department.GetDepartmentForDataSource(Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_BRANCH_CODE])), "Value", "Text", null);
            ViewData["LocationList"] = new SelectList(IDS.GeneralTable.Location.GetLocationDatasource(), "Value", "Text", null);
            ViewData["FAGroupList"] = new SelectList(IDS.FixedAsset.FAGroup.getFAGroupForDatasource(), "Value", "Text", null);
            ViewData["FATaxCategory"] = new SelectList(IDS.FixedAsset.FATaxCategory.GetFATaxCategoryForDatasource(), "Value", "Text", null);

            ViewData["FAPartOf"] = new SelectList(IDS.FixedAsset.FAAsset.getFAAssetForDatasource(branchCode), "Value", "Text", null);
            //ViewData["VendorList"] = new SelectList(IDS.GeneralTable.Supplier.GetSupplier(), "Value", "Text", null);
            ViewData["VendorList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text", null);
            ViewData["CCyList"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            ViewData["FAAssetStatus"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FAAssetStatus>(IDS.FixedAsset.FAAssetStatus.Active);


            ViewData["COAList"] = new SelectList(IDS.GLTable.ChartOfAccount.GetCOAExProtAccForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy), "Value", "Text", null);
            ViewData["DepreMethodList"] = new SelectList(IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FADepreMethod>(IDS.FixedAsset.FADepreMethod.Double_Declining), "Value", "Text", null);

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 2;

            ViewData["Journal"] = IDS.FixedAsset.FASchedule.IsJournalExists(assetNo, branchCode);

            IDS.FixedAsset.FAAsset asset = IDS.FixedAsset.FAAsset.GetFAAsset(assetNo, branchCode);

            return View("Create", asset);
        }

        // POST: FixedAsset/FAAsset/Edit/5
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection data)
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

                if (Tool.GeneralHelper.NullToInt(data["Journal"], 1) > 0)
                {
                    throw new Exception("Data has been journaled, can not be edit.");
                }

                IDS.FixedAsset.FAAsset asset = new IDS.FixedAsset.FAAsset();
                asset.AssetNo = IDS.Tool.GeneralHelper.NullToString(data["AssetNo"]);
                asset.BranchCode = IDS.Tool.GeneralHelper.NullToString(data["BranchCode"]);
                asset.Department = IDS.Tool.GeneralHelper.NullToString(data["Department"]);
                asset.Location = IDS.Tool.GeneralHelper.NullToString(data["Location"]);
                asset.PartOf = IDS.Tool.GeneralHelper.NullToString(data["PartOf"]);

                if (data["Group.Code"] != null)
                {
                    asset.Group = new IDS.FixedAsset.FAGroup();
                    asset.Group.Code = IDS.Tool.GeneralHelper.NullToString(data["Group.Code"]);
                }

                asset.PartOf = IDS.Tool.GeneralHelper.NullToString(data["PartOf"]);
                asset.TaxCategoryID = IDS.Tool.GeneralHelper.NullToString(data["TaxCategoryID"]);
                asset.Description = IDS.Tool.GeneralHelper.NullToString(data["Description"]);
                asset.AssetVoucher = IDS.Tool.GeneralHelper.NullToString(data["AssetVoucher"]);
                asset.SerialNo = IDS.Tool.GeneralHelper.NullToString(data["SerialNo"]);
                asset.Vendor = IDS.Tool.GeneralHelper.NullToString(data["Vendor"]);
                asset.Qty = IDS.Tool.GeneralHelper.NullToDecimal(data["Qty"], 0);
                asset.Currency = IDS.Tool.GeneralHelper.NullToString(data["Currency"]);
                asset.UnitPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["UnitPrice"], 0);
                asset.ResidualValue = IDS.Tool.GeneralHelper.NullToDecimal(data["ResidualValue"], 0);
                asset.TotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["TotalPrice"], 0);
                asset.BaseTotalPrice = IDS.Tool.GeneralHelper.NullToDecimal(data["BaseTotalPrice"], 0);
                asset.BusinessUse = IDS.Tool.GeneralHelper.NullToDecimal(data["BusinessUse"], 0);
                asset.AcquisitionDate = IDS.Tool.GeneralHelper.NullToDateTime(data["AcquisitionDate"], DateTime.Now.Date);
                asset.StartingDate = IDS.Tool.GeneralHelper.NullToDateTime(data["StatingDate"], DateTime.Now.Date);
                asset.EndDate = IDS.Tool.GeneralHelper.NullToDateTime(data["EndDate"], DateTime.Now.Date);
                asset.Status = (IDS.FixedAsset.FAAssetStatus)Enum.Parse(typeof(IDS.FixedAsset.FAAssetStatus), Tool.GeneralHelper.NullToString(data["Status"]));
                asset.StatusDate = Tool.GeneralHelper.NullToDateTime(data["StatusDate"], DateTime.Now.Date);
                asset.ExchangeRate = IDS.Tool.GeneralHelper.NullToDecimal(data["ExchangeRate"], 0);
                asset.OperatorID = IDS.Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_ID]);
                asset.EntryUser = asset.OperatorID;
                asset.LastUpdate = asset.EntryDate = DateTime.Now;

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
                    asset.InsUpDel((int)Tool.PageActivity.Edit);

                    return Json(new { msg = "Success", code = "200", id = asset.AssetNo, branchCode = asset.BranchCode }, JsonRequestBehavior.AllowGet);
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

        private bool ValidateInput(IDS.FixedAsset.FAAsset asset, StringBuilder sb, Tool.PageActivity activity)
        {
            bool valid = true;

            if (sb == null)
                sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(asset.AssetNo))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Can not find Asset no.");
            }

            if (asset.Group == null || string.IsNullOrWhiteSpace(asset.Group.Code))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Asset group is required");
            }

            if (string.IsNullOrWhiteSpace(asset.Description))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Asset description is required.");
            }

            if (asset.Status == null || !Enum.IsDefined(typeof(IDS.FixedAsset.FAAssetStatus), asset.Status))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Invalid fixed asset status");
            }

            if (asset.StatusDate > DateTime.Now.Date)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Status date can not be greater than today");
            }

            if (string.IsNullOrWhiteSpace(asset.Currency))
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Currency is required");
            }

            if (asset.Qty <= 0)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Quantity can not be lower or equals to zero");
            }

            if (asset.UnitPrice <= 0)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Unit price can not be lower or equals to zero");
            }

            if (asset.ResidualValue < 0)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Residual value can not be lower or equals to zero");
            }

            if (asset.TotalPrice <= 0)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Total price can not be lower or equals to zero");
            }

            if (asset.BaseTotalPrice <= 0)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Base total price can not be lower or equals to zero");
            }

            if (asset.BusinessUse < 0 || asset.BusinessUse > 100)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Invalid busines of use");
            }

            if (asset.AcquisitionDate > DateTime.Now.Date)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Acquitision date can not be greater than today");
            }
            else
            {
                if (asset.AcquisitionDate.Date == DateTime.MinValue.Date)
                {
                    valid = false;
                    sb.Append(Environment.NewLine).Append("- Invalid acquisition date");
                }
            }

            if (asset.StartingDate.Date == DateTime.MinValue.Date)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Invalid starting date");
            }

            if (asset.EndDate.Date == DateTime.MinValue.Date)
            {
                valid = false;
                sb.Append(Environment.NewLine).Append("- Invalid end date");
            }

            return valid;
        }

        // POST: FixedAsset/FAAsset/Delete/5
        [HttpPost]
        public ActionResult Delete(string assetList)
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

            if (string.IsNullOrWhiteSpace(assetList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] assetCode = assetList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (assetCode.Length > 0)
                {
                    IDS.FixedAsset.FAAsset group = new IDS.FixedAsset.FAAsset();
                    group.InsUpDel(assetCode);
                }

                return Json("Fixed asset data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CalculatePrice(string ccy, decimal price, decimal qty, decimal exRate)
        {
            try
            {
                IDS.GeneralTable.Currency currency = IDS.GeneralTable.Currency.GetCurrency(ccy);

                decimal totalPrice, baseTotalPrice = 0;

                if (currency != null)
                {
                    totalPrice = Math.Round((price * qty), currency.DecimalPlaces);
                    baseTotalPrice = Math.Round((totalPrice * exRate), 0);
                }
                else
                {
                    totalPrice = Math.Round((price * qty), 0);
                    baseTotalPrice = Math.Round((totalPrice * exRate), 0);
                }

                return Json((totalPrice.ToString("N2") + ";" + baseTotalPrice.ToString("N2")), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Content(ex.ToString());
            }
        }

        public ActionResult GenerateFASchedule(string assetNo, string branchCode)
        {
            try
            {
                if (Session[Tool.GlobalVariable.SESSION_USER_ID] == null)
                    throw new Exception("You do not have access to load data or your sesssion is expired. Please re-login or contact your administrator.");

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

                List<IDS.FixedAsset.FASchedule> schedule = IDS.FixedAsset.FASchedule.GetFASchedule(assetNo, branchCode, Tool.GeneralHelper.NullToString(Session[Tool.GlobalVariable.SESSION_USER_ID]), 1);

                totalRecords = schedule.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    schedule = schedule.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                //// Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string lowerSearchValue = searchValue.ToLower();

                //    schedule = schedule.Where(x => x.CityName.ToLower().Contains(lowerSearchValue) ||
                //                                x.CityCode.ToLower().Contains(lowerSearchValue) ||
                //                                x.Country.CountryName.ToLower().Contains(lowerSearchValue) ||
                //                                x.OperatorID.ToLower().Contains(lowerSearchValue) ||
                //                                x.LastUpdate.ToString(Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(lowerSearchValue)).ToList();
                //}

                totalRecordsShowing = schedule.Count();

                // Paging
                if (pageSize > 0)
                    schedule = schedule.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                return this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = schedule }, JsonRequestBehavior.AllowGet);



                //return Json(new { data = schedule }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                return Json(ex.Message);
            }
        }

        public ActionResult GetAsset(string branchCode, string assetNo)
        {
            IDS.FixedAsset.FAAsset asset = IDS.FixedAsset.FAAsset.GetFAAsset(assetNo, branchCode);

            return Json(asset, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBookValue(string assetNo, DateTime period)
        {
            decimal result = 0;

            if (string.IsNullOrWhiteSpace(assetNo))
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            result = IDS.FixedAsset.FASchedule.GetBookValue(period.ToString("yyyyMM"), assetNo);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}