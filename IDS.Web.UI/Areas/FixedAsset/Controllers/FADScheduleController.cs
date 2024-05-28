using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FADScheduleController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: FixedAsset/FADSchedule
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }


        [HttpPost]
        public JsonResult GetAssetNoJsonResult()
        {
            List<System.Web.Mvc.SelectListItem> xl = null;
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (MyTool.ValidateJSON(json))
            {
                var o = Newtonsoft.Json.Linq.JObject.Parse(json);
                var _branch = o.SelectToken("branch").ToString();
                xl = IDS.FixedAsset.FAAsset.getFAAssetForDatasourceOPT(_branch);
            }
            return Json(xl, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepScedule(string branch, string assetno)
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
            List<IDS.FixedAsset.FASchedule> acc = IDS.FixedAsset.FASchedule.GetFASchedule_Stat(assetno, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), 0);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.Period.ToString().ToLower().Contains(searchValueLower) || x.BegVal.ToString().ToLower().Contains(searchValueLower) || x.Depreciation.ToString().ToLower().Contains(searchValueLower) || x.AccumDepre.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }

        // Amortize 1
        public JsonResult GoAmprDepr(string branch, string assetno)
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
            List<IDS.FixedAsset.FASchedule> acc = IDS.FixedAsset.FASchedule.GetFASchedule_Stat(assetno, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), 1);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.Period.ToString().ToLower().Contains(searchValueLower) || x.BegVal.ToString().ToLower().Contains(searchValueLower) || x.Depreciation.ToString().ToLower().Contains(searchValueLower) || x.AccumDepre.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_IsAlreadyJournal(string branch, string assetno)
        {
            return Json(IDS.FixedAsset.FASchedule.IsAlreadyJournal(assetno, branch), JsonRequestBehavior.AllowGet);
        }


        //========================================================= option new Page =========================================
        public JsonResult GetDepSceduleTax(string branch, string assetno)
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
            List<IDS.FixedAsset.FASchedule> acc = IDS.FixedAsset.FASchedule.GetFASchedule_TAX(assetno, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), 0);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.Period.ToString().ToLower().Contains(searchValueLower) || x.BegVal.ToString().ToLower().Contains(searchValueLower) || x.Depreciation.ToString().ToLower().Contains(searchValueLower) || x.AccumDepre.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepSceduleTaxAmort(string branch, string assetno)
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
            List<IDS.FixedAsset.FASchedule> acc = IDS.FixedAsset.FASchedule.GetFASchedule_TAX(assetno, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), 1);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.Period.ToString().ToLower().Contains(searchValueLower) || x.BegVal.ToString().ToLower().Contains(searchValueLower) || x.Depreciation.ToString().ToLower().Contains(searchValueLower) || x.AccumDepre.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_IsAlreadyJournal_Tax(string branch, string assetno)
        {
            return Json(IDS.FixedAsset.FASchedule.IsAlreadyJournalTax(assetno, branch), JsonRequestBehavior.AllowGet);
        }

    }
}