using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Areas.GeneralTable.Controllers
{
    public class BranchController : IDS.Web.UI.Controllers.MenuController
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

                List<IDS.GeneralTable.Branch> branches = IDS.GeneralTable.Branch.GetBranch();

                totalRecords = branches.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //switch (sortColumn.ToLower())
                    //{
                    //    case "city":
                    //        switch (sortColumnDir)
                    //        {
                    //            case "desc":
                    //                branches = branches.OrderByDescending(x => x.BranchCity?.CityName ?? "").ToList();
                    //                break;
                    //            default:
                    //                branches = branches.OrderBy(x => (x.BranchCity?.CityName ?? "")).ToList();
                    //                break;
                    //        }
                    //        break;
                    //    default:
                    //        branches = branches.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                    //        break;
                    //}

                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    branches = branches.Where(x => x.BranchName.ToLower().Contains(searchValue) ||
                                             x.BranchCode.ToLower().Contains(searchValue) ||
                                             x.OperatorID.ToLower().Contains(searchValue) ||
                                             x.LastUpdate.ToString(IDS.Tool.GlobalVariable.DEFAULT_DATETIME_FORMAT).ToLower().Contains(searchValue)).ToList();
                }

                totalRecordsShowing = branches.Count();

                // Paging
                if (pageSize > 0)
                    branches = branches.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { draw = draw, recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = branches }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        // GET: GeneralTable/Branch
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

            ViewData["Link.Create"] = Url.Action("Create", "Branch", new { Area = "GeneralTable" });
            ViewData["Link.Edit"] = Url.Action("Create", "Branch", new { Area = "GeneralTable" });



            //ViewBag.UserMenu = IDS.Maintenance.UserMenu.ParseMenuToHTML();
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

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

            ViewData["FormAction"] = 1;

            // City List
            //ViewData["CityList"] = IDS.GeneralTable.City.GetCityForDatasource();
            //ViewData["CountryList"] = new SelectList(new List<SelectListItem>());
            ViewData["CountryList"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text");
            ViewData["CityList"] = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewData["GetLanguage"] = new SelectList(IDS.GeneralTable.Branch.GetLanguage(), "Value", "Text");
            ViewData["GetSignBY"] = new SelectList(IDS.GeneralTable.Branch.GetSignBY(), "Value", "Text");
            ViewData["GetRepNo"] = new SelectList(IDS.GeneralTable.Branch.GetRepNo(), "Value", "Text");
            ViewData["GetHopNo"] = new SelectList(IDS.GeneralTable.Branch.GetRepNo(), "Value", "Text");
            ViewData["GetInvSignBY"] = new SelectList(IDS.GeneralTable.Branch.GetSignBY(), "Value", "Text");
            ViewData["GetTaxSignBY"] = new SelectList(IDS.GeneralTable.Branch.GetSignBY(), "Value", "Text");
            // HOPNo & RepNo
            // TODO: Currency di sesuaikan dengan SYSPAR
            ViewData["AccList"] = IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithAccountGroup("IDR");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View("Create", new IDS.GeneralTable.Branch());
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, [Bind(Exclude = "")]IDS.GeneralTable.Branch branch)
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

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            #region Validation
            if (string.IsNullOrWhiteSpace(branch.BranchCode))
                sb.Append(Environment.NewLine).Append("- Branch code is required.");

            if (string.IsNullOrWhiteSpace(branch.BranchName))
                sb.Append(Environment.NewLine).Append("- Branch Name is required.");




            if (string.IsNullOrWhiteSpace(branch.Address1))
                sb.Append(Environment.NewLine).Append("- Address 1 is required.");

            if (string.IsNullOrWhiteSpace(branch.Address2))
                sb.Append(Environment.NewLine).Append("- Address 2 is required.");

            if (string.IsNullOrWhiteSpace(branch.Address3))
                sb.Append(Environment.NewLine).Append("- Address 3 is required.");


            if (string.IsNullOrWhiteSpace(branch.Fax))








                sb.Append(Environment.NewLine).Append("- Fax is required.");


















            if (string.IsNullOrWhiteSpace(branch.PostalCode))
                sb.Append(Environment.NewLine).Append("- PostalCode is required.");
            if (string.IsNullOrWhiteSpace(branch.FinAccOfficer))
                sb.Append(Environment.NewLine).Append("- Finance Officer is required.");
            if (string.IsNullOrWhiteSpace(branch.NPWP))
                sb.Append(Environment.NewLine).Append("- NPWP is required.");
            if (string.IsNullOrWhiteSpace(branch.BranchManagerName))
                sb.Append(Environment.NewLine).Append("- Branch Manager Name is required.");
            if (string.IsNullOrWhiteSpace(branch.Repno))
                sb.Append(Environment.NewLine).Append("- Repno is required.");
            if (string.IsNullOrWhiteSpace(branch.Hopno))
                sb.Append(Environment.NewLine).Append("- Hopno is required.");
            if (string.IsNullOrWhiteSpace(branch.TaxSignBy))
                sb.Append(Environment.NewLine).Append("- TaxSignBy is required.");

            if (string.IsNullOrWhiteSpace(branch.TaxOccupation))
                sb.Append(Environment.NewLine).Append("- TaxOccupation is required.");
            if (string.IsNullOrWhiteSpace(branch.InvSignBy))















                sb.Append(Environment.NewLine).Append("- InvSignBy is required.");
            
            if (string.IsNullOrWhiteSpace(branch.InvOccupation))
                sb.Append(Environment.NewLine).Append("- InvOccupation is required.");
            #endregion

            if (sb.Length > 0)
            {

                sb.Insert(0, "please check data you input data: ").Append(Environment.NewLine);

                return Json(new { errorMsg = sb.ToString() }, JsonRequestBehavior.AllowGet);

            }
            else
            {

                ModelState.Clear();
                if (ModelState.IsValid)
                {


                    string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                    if (string.IsNullOrWhiteSpace(currentUser))
                    {

                        return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                    }

                    //System.Text.StringBuilder builder = new System.Text.StringBuilder();
                    //builder.AppendLine(branch.Language + " -->Branch Language");
                    //builder.AppendLine(branch.BranchCode + " -->Branch Code");
                    //builder.AppendLine(branch.BranchName + " -->Branch Name");
                    //builder.AppendLine(branch.HOStatus.ToString() + " -->Ho Status");
                    //builder.AppendLine(branch.Address1+ " -->address 1");
                    //builder.AppendLine(branch.Address2 + " -->address 2");
                    //builder.AppendLine(branch.Address3 + " -->address 3");
                    //builder.AppendLine(branch.BranchCountry.CountryCode + " -->BranchCountry 3");
                    //builder.AppendLine(branch.BranchCity.CityCode + " -->BranchCity 3");
                    //builder.AppendLine(branch.Phone1 + " -->Phone1 ");
                    //builder.AppendLine(branch.Phone2 + " -->Phone2 ");
                    //builder.AppendLine(branch.Phone3 + " -->Phone3 ");
                    //builder.AppendLine(branch.Fax + " -->Fax ");
                    //builder.AppendLine(branch.Telex + " -->Telex ");
                    //builder.AppendLine(branch.PostalCode + " -->PostalCode ");
                    //builder.AppendLine(branch.FinAccOfficer + " -->FinAccOfficer ");
                    //builder.AppendLine(branch.NPWP + " -->NPWP ");
                    //builder.AppendLine(branch.TaxEstablishDate + " -->TaxEstablishDate ");
                    //builder.AppendLine(branch.BranchManager + " -->BranchManager ");
                    //builder.AppendLine(branch.Repno + " -->Repno ");
                    //builder.AppendLine(branch.Hopno + " -->Hopno ");
                    //builder.AppendLine(branch.TaxSignBy + " -->TaxSignBy ");
                    //builder.AppendLine(branch.TaxOccupation + " -->TaxOccupation ");
                    //builder.AppendLine(branch.InvSignBy + " -->InvSignBy ");
                    //builder.AppendLine(branch.InvOccupation + " -->InvOccupation ");
                    //System.Diagnostics.Debug.WriteLine(builder.ToString());
                    branch.OptIndex = bool.Parse(branch.Language);

                    try
                    {
                        branch.LastUpdate = DateTime.Now;
                        branch.OperatorID = currentUser;

                        branch.InsUpDel((IDS.Tool.PageActivity)IDS.Tool.PageActivity.Insert);

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        return Json(ex.Message);
                    }
                }
                else
                {

                    return Json("Not Valid Model", JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet, AcceptVerbs(HttpVerbs.Get)]
        // GET: GeneralTable/Country/Edit/5
        public ActionResult Edit(string branchCode)
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

            IDS.GeneralTable.Branch branch = IDS.GeneralTable.Branch.GetBranch(branchCode);

            ViewData["CountryList"] = new SelectList(IDS.GeneralTable.Country.GetCountryForDatasource(), "Value", "Text", branch.BranchCountry.CountryCode);
            ViewData["CityList"] = new SelectList(IDS.GeneralTable.City.GetCityForDatasource(branch.BranchCountry.CountryCode), "Value", "Text", branch.BranchCity.CityCode);
            ViewData["GetLanguage"] = new SelectList(IDS.GeneralTable.Branch.GetLanguage(), "Value", "Text", branch.Language);
            ViewData["GetInvSignBY"] = new SelectList(IDS.GeneralTable.Branch.GetSignBY(), "Value", "Text", branch.InvSignBy);
            ViewData["GetTaxSignBY"] = new SelectList(IDS.GeneralTable.Branch.GetSignBY(), "Value", "Text", branch.TaxSignBy);
            ViewData["GetRepNo"] = new SelectList(IDS.GeneralTable.Branch.GetRepNo(), "Value", "Text", branch.Repno);
            ViewData["GetHopNo"] = new SelectList(IDS.GeneralTable.Branch.GetRepNo(), "Value", "Text", branch.Hopno);

            ViewData["FormAction"] = 2;

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            if (branch != null)
            {
                //branch.Telex = IDS.GeneralTable.Branch.GetItem_from_Branch("Telex", branch.BranchCode).ToString();
                //branch.InvOccupation = IDS.GeneralTable.Branch.GetItem_from_Branch("InvOccupation", branch.BranchCode).ToString();
                //branch.TaxOccupation = IDS.GeneralTable.Branch.GetItem_from_Branch("TaxOccupation", branch.BranchCode).ToString();
                //branch.ContractLimit = IDS.GeneralTable.Branch.GetContract_limit("ContractLimit", branch.BranchCode);
                //branch.TaxEstablishDate = IDS.GeneralTable.Branch.GetTaxEstablishDate("TaxEstablishDate", branch.BranchCode);
                if (branch.OptIndex)
                {
                    branch.Language = "True";
                }
                else
                {
                    branch.Language = "False";
                }
                return PartialView("Create", branch);
            }
            else
            {
                return PartialView("Create", new IDS.GeneralTable.Branch());
            }
        }

        // POST: GeneralTable/Country/Edit/5
        [HttpPost, AcceptVerbs(HttpVerbs.Post), ValidateAntiForgeryToken]
        public ActionResult Edit(IDS.GeneralTable.Branch branch)
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
                    branch.LastUpdate = DateTime.Now;
                    branch.OperatorID = currentUser;

                    branch.InsUpDel(IDS.Tool.PageActivity.Edit);

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

        // POST: GeneralTable/Country/Delete/5
        public ActionResult Delete(string branchsCodeList)
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

            if (string.IsNullOrWhiteSpace(branchsCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] branchsCode = branchsCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (branchsCode.Length > 0)
                {
                    IDS.GeneralTable.Branch branch = new IDS.GeneralTable.Branch();
                    branch.InsUpDel((int)IDS.Tool.PageActivity.Delete, branchsCode);
                }

                return Json("Branch data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Get_TaxEstablishDate(string Branch)
        {
            return Json(IDS.GeneralTable.Branch.GetTaxEstablishDate("TaxEstablishDate", Branch), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_HO_Exist(string Branch)
        {
            return Json(IDS.GeneralTable.Branch.GetHOStatus(), JsonRequestBehavior.AllowGet);
        }


    }
}