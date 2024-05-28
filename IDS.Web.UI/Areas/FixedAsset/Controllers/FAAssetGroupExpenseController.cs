using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FAAssetGroupExpenseController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: FixedAsset/FAAssetGroupExpense
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["AssetGroupExpense"] = new SelectList(IDS.FixedAsset.FAAssetGroupExpense.FAAssetGroupExpenseForDatasource(), "Value", "Text");
            ViewData["ACFGLMHForDatasource"] = new SelectList(IDS.FixedAsset.FAAssetGroupExpense.GetCOAForDatasource(), "Value", "Text");
            ViewBag.Title = "Asset Group Expense";
            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }//--> Index

        [HttpPost]
        public ActionResult GetData(string grup)
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
            List<IDS.FixedAsset.FAAssetGroupExpense> acc = IDS.FixedAsset.FAAssetGroupExpense.GetData(grup);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.AssetGroup.ToString().ToLower().Contains(searchValueLower) || x.AssetGroupDesc.ToString().ToLower().Contains(searchValueLower) || x.DepYear.ToString().ToLower().Contains(searchValueLower) || x.OperatorID.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }//GetData

        public System.Web.Mvc.JsonResult Test(string grup)
        {
            return this.Json("title:'1111111',start:'2014-03-23T16:00:00.000',end:'2014-03-23T18:00:00.000',id:107,hdtid:1,color:'#c732bd',allDay:false,description:''", JsonRequestBehavior.AllowGet);
        }//GetData

    }
}