using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.Sales.Controllers
{

    public class PPhOldController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: Sales/wfPPhOld
        public ActionResult GetData(string year)
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

                List<IDS.Sales.PPhHeader> pphHeader = IDS.Sales.PPhHeader.GetPPhHeader();
                pphHeader = pphHeader.Where(x => x.Year.ToString() == year).ToList();
                //var v = pphHeader.SelectMany(x => x.Customer.CUSTCode.Select(y => new
                //{
                //    CustSuppCode = x.Customer.CUSTCode,
                //    NPWP = x.NPWP,
                //    PPhType = x.PPhType,
                //    Type = x.Type,
                //    Year = x.Year,
                //})).Distinct();

                totalRecords = pphHeader.Count();

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            pphHeader = pphHeader.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    pphHeader = pphHeader.Where(x => x.Supplier.SupCode.ToString().ToLower().Contains(searchValueLower) ||
                                                //x.NPWP.ToString().ToLower().Contains(searchValueLower) ||
                                                x.Type.ToString().ToLower().Contains(searchValueLower) ||
                                                x.Year.ToString().ToLower().Contains(searchValueLower) ||
                                                x.PPhType.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                

                totalRecordsShowing = pphHeader.Count();

                // Paging
                if (pageSize > 0)
                    pphHeader = pphHeader.Skip(skip).Take(pageSize).ToList();

                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = pphHeader }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public JsonResult GetDataDetail(string sup,int year, string type)
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
                //
                List<IDS.Sales.PPhDetail> pphD = IDS.Sales.PPhDetail.GetPPhDetail(sup,year, type);
                //
                totalRecords = pphD.Count;

                // Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn.ToLower())
                    {
                        default:
                            pphD = pphD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                            break;
                    }
                }

                // Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();

                    pphD = pphD.Where(x => x.Month.ToString().ToLower().Contains(searchValueLower) ||
                                             x.TaxRate.ToString().ToLower().Contains(searchValueLower) ||
                                             x.DasarPemotongan.ToString().ToLower().Contains(searchValueLower) ||
                                             x.DasarPemotonganKumulatif.ToString().ToLower().Contains(searchValueLower) ||
                                             x.Tarif.ToString().ToLower().Contains(searchValueLower) ||
                                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower) ||
                                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = pphD.Count();

                // Paging
                if (pageSize > 0)
                    pphD = pphD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = pphD }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

            }

            return result;
        }

        public JsonResult GetDataEditDetail(string sup, int year)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            JsonResult result = new JsonResult();

            try
            {
                //var draw = Request.Form.GetValues("draw").FirstOrDefault();
                //var start = Request.Form.GetValues("start").FirstOrDefault();
                //var length = Request.Form.GetValues("length").FirstOrDefault();
                //var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                //var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                //var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                //int pageSize = 0;

                //switch (length)
                //{
                //    case null:
                //        break;
                //    case "-1":
                //        pageSize = 0;
                //        break;
                //    default:
                //        pageSize = Convert.ToInt32(length);
                //        break;
                //}

                //int skip = start != null ? Convert.ToInt32(start) : 0;
                //int totalRecords = 0; // Total keseluruhan data
                //int totalRecordsShowing = 0; // Total data setelah filter / search
                //
                List<IDS.Sales.PPhDetail> pphD = IDS.Sales.PPhDetail.GetPPhDetail(sup, year);
                //
                //totalRecords = pphD.Count;

                // Sorting    
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                //{
                //    switch (sortColumn.ToLower())
                //    {
                //        default:
                //            pphD = pphD.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                //            break;
                //    }
                //}

                // Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    string searchValueLower = searchValue.ToLower();

                //    pphD = pphD.Where(x => x.Month.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TaxRate.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotongan.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.DasarPemotonganKumulatif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.Tarif.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower) ||
                //                             x.TarifNonNPWP.ToString().ToLower().Contains(searchValueLower)).ToList();
                //}

                //totalRecordsShowing = pphD.Count();

                //// Paging
                //if (pageSize > 0)
                //    pphD = pphD.Skip(skip).Take(pageSize).ToList();

                // Returning Json Data
                //result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = pphD }, JsonRequestBehavior.AllowGet);
                //result = this.Json(new { data = pphD }, JsonRequestBehavior.AllowGet);
                result = Json(Newtonsoft.Json.JsonConvert.SerializeObject(pphD), JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {

            }

            return result;
        }

        public JsonResult CheckDataDetail(string sup, int year)
        {
            List<IDS.Sales.PPhDetail> pphD = IDS.Sales.PPhDetail.GetPPhDetail(sup, year);

            return this.Json(new { recordsTotal = pphD.Count }, JsonRequestBehavior.AllowGet);
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

            ViewData["SelectListCust"] = new SelectList(IDS.GeneralTable.Customer.GetACFCUSTForDataSource(), "Value", "Text");
            ViewData["SelectListTask"] = new SelectList(IDS.GeneralTable.Tax.GetTaxForDataSource(), "Value", "Text");
            ViewData["SelectJenisPenghasilan"] = new SelectList(IDS.Sales.JenisPenghasilan.GetJPForDataSource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }// Index

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

            ViewData["PayTypeList"] = new SelectList(IDS.Sales.PaymentH.GetPaymentTypeForDataSource(), "Value", "Text", 0);
            ViewData["SupList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text");
            ViewData["TaxList"] = new SelectList(IDS.GeneralTable.Tax.GetTaxForDataSource(), "Value", "Text");
            ViewData["MonthList"] = new SelectList(IDS.Sales.PPhHeader.GetMonthForDataSource(), "Value", "Text");
            ViewData["TaxTypeList"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyBaseOnChartOfAccountForDatasource();
            List<SelectListItem> ccyList = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            ViewData["CCyList"] = new SelectList(ccyList, "Value", "Text", IDS.GeneralTable.Syspar.GetInstance().BaseCCy);

            List<SelectListItem> alloTypeList = IDS.Sales.PaymentD.GetAllocationTypeForDataSource().Where(x => x.Value != "5").ToList();
            ViewData["alloTypeList"] = new SelectList(alloTypeList, "Value", "Text", ((int)IDS.Tool.AllocationType.Receivable).ToString().ToString());

            //List<SelectListItem> deptList = IDS.GeneralTable.Department.GetDepartmentForDataSource();

            
            ViewData["CurDate"] = DateTime.Now.ToString("dd/MMM/yyyy");
            //return View("Create", new IDS.Sales.PPhHeader() { PayDate = DateTime.Now.Date, SerialNo = "AUTO", ChequeDate = DateTime.Now });
            return View("Create", new IDS.Sales.PPhHeader() { Year=DateTime.Now.Year});
            //return View("Create", new IDS.Sales.PaymentH() { TransDate = DateTime.Now.Date, Voucher = "AUTO" });
        }

        public ActionResult Edit(string sup, int year, string type)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.ReadAccess == -1 || AccessLevel.EditAccess == 0)
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                return RedirectToAction("error403", "error", new { area = "" });
            }

            ViewData["Page.Insert"] = AccessLevel.CreateAccess;
            ViewData["Page.Edit"] = AccessLevel.EditAccess;
            ViewData["Page.Delete"] = AccessLevel.DeleteAccess;

            ViewData["FormAction"] = 2;

            IDS.Sales.PPhHeader pphH = IDS.Sales.PPhHeader.GetPPhHeader(sup, year, type);
            
            ViewData["SupList"] = new SelectList(IDS.GeneralTable.Supplier.GetACFVENDForDataSource(), "Value", "Text",pphH.Supplier.SupCode);
            ViewData["TaxList"] = new SelectList(IDS.GeneralTable.Tax.GetTaxForDataSource(), "Value", "Text", pphH.PPhType);
            ViewData["TaxTypeList"] = new SelectList(IDS.GeneralTable.tblJenisPenghasilan.GetKodePajakForDataSource(pphH.PPhType, pphH.JenisPenghasilan), "Value", "Text",pphH.PPhType);
            ViewData["MonthList"] = new SelectList(IDS.Sales.PPhHeader.GetMonthForDataSource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            
            //return View("Create", new IDS.Sales.PPhHeader() { PayDate = DateTime.Now.Date, SerialNo = "AUTO", ChequeDate = DateTime.Now });
            return View("Create", pphH);
            //return View("Create", new IDS.Sales.PaymentH() { TransDate = DateTime.Now.Date, Voucher = "AUTO" });
        }

        [HttpPost]
        //[AcceptVerbs(HttpVerbs.Post)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(int? FormAction, int? FormActionDetail, IDS.Sales.PPhHeader pphH)
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

            //city.Country = IDS.GeneralTable.Country.GetCountry(city.Country.CountryCode);
            //if (city.Country.SLIKCode == null)
            //    city.Country.SLIKCode = "";

            ModelState.Clear();

            //ValidateModel(city);

            if (ModelState.IsValid)
            {
                string currentUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] as string;

                pphH.Detail = IDS.Sales.PPhDetail.GetPPhDetail(pphH.Supplier.SupCode, pphH.Year);

                //if (pphH.Detail.Count == 0)
                //{
                //    FormAction = 1;
                //}
                //else
                //{
                //    FormAction = 2;
                //}

                if (FormActionDetail == 2)
                {
                    pphH.Detail.RemoveAll(x => x.Supplier.SupCode == pphH.Supplier.SupCode && x.Year == pphH.Year && x.Description == pphH.Type && x.SeqNo == pphH.PPhDetail.SeqNo && x.Month == pphH.PPhDetail.Month);
                }

                pphH = pphH.CalculateDetail(pphH, (int)FormActionDetail);

                if (string.IsNullOrWhiteSpace(currentUser))
                {
                    return Json("SessionTimeOut", JsonRequestBehavior.AllowGet);
                }

                try
                {
                    // city.OperatorID = currentUser;
                    pphH.InsUpDelPPh((int)FormAction);
                    //city.InsUpDelCity(IDS.Tool.PageActivity.Insert);

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

        public ActionResult DeleteH(string SupsCodeList, int year, string typeList)
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

            if (string.IsNullOrWhiteSpace(SupsCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] supsCode = SupsCodeList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string[] types = typeList.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (supsCode.Length > 0)
                {
                    IDS.Sales.PPhHeader pphH = new IDS.Sales.PPhHeader();
                    pphH.InsUpDel(3, supsCode, year, types);
                    //IDS.Sales.PPhHeader pphH = IDS.Sales.PPhHeader.GetPPhHeader(sup, year, type);
                    //pphH.Detail = IDS.Sales.PPhDetail.GetPPhDetail(sup, year);

                    //for (int i = 0; i < supsCode.Length; i++)
                    //{
                    //    pphH.Detail.RemoveAll(x => x.Supplier.SupCode == sup && x.Year == year && x.Description == type && x.SeqNo == IDS.Tool.GeneralHelper.NullToInt(seqNosCode[i], 0) && x.Month == IDS.Tool.GeneralHelper.NullToInt(monthsCode[i], 0));
                    //}

                    ////pphH.PPhDetail = IDS.Sales.PPhDetail.GetPPhDetail(sup, year,, type);
                    //if (pphH.Detail.Count > 0)
                    //{
                    //    pphH = pphH.CalculateDetail(pphH, 3);
                    //}

                    //pphH.InsUpDelPPh(2);
                }

                return Json("Data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(string SeqNosCodeList, string MonthCodeList, string sup, int year,string type)
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

            if (string.IsNullOrWhiteSpace(SeqNosCodeList))
                return Json("Failed", JsonRequestBehavior.AllowGet);

            try
            {
                string[] seqNosCode = SeqNosCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] monthsCode = MonthCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (seqNosCode.Length > 0)
                {
                    IDS.Sales.PPhHeader pphH = IDS.Sales.PPhHeader.GetPPhHeader(sup, year,type);
                    pphH.Detail = IDS.Sales.PPhDetail.GetPPhDetail(sup, year);

                    for (int i = 0; i < seqNosCode.Length; i++)
                    {
                        pphH.Detail.RemoveAll(x => x.Supplier.SupCode == sup && x.Year == year && x.Description == type && x.SeqNo == IDS.Tool.GeneralHelper.NullToInt(seqNosCode[i],0) && x.Month == IDS.Tool.GeneralHelper.NullToInt(monthsCode[i],0));
                    }

                    //pphH.PPhDetail = IDS.Sales.PPhDetail.GetPPhDetail(sup, year,, type);
                    if (pphH.Detail.Count > 0)
                    {
                        pphH = pphH.CalculateDetail(pphH, 3);
                    }
                    
                    pphH.InsUpDelPPh(2);
                }

                return Json("Data has been delete successfull", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}