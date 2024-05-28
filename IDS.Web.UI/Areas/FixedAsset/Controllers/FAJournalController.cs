using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FAJournalController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: FixedAsset/FAJournal
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text");
            ViewData["SelectListCurrency"] = new SelectList(IDS.GeneralTable.Currency.GetCurrencyForDataSource(), "Value", "Text");

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }

        public JsonResult GetData(string period, string ccy, string branch)
        {
            if (string.IsNullOrEmpty(period))
                period = System.DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = DateTime.ParseExact(period, "MMM yyyy", null);
                period = datePeriod.ToString("yyyyMM");
            }
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
            List<IDS.FixedAsset.FAJournalSelect> acc = IDS.FixedAsset.FAJournal.GetFAJournal(period, ccy, branch);
            totalRecords = acc.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                acc = acc.Where(x => x.AccumAccName.ToString().ToLower().Contains(searchValueLower) || x.KEY.ToString().ToLower().Contains(searchValueLower) || x.DepAccName.ToString().ToLower().Contains(searchValueLower) || x.ItemGroup.ToString().ToLower().Contains(searchValueLower)).ToList();
            }
            totalRecordsShowing = acc.Count();
            acc = acc.Skip(skip).Take(pageSize).ToList();
            return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        }//JsonResult GetData

        [HttpPost]
        public string GetDataDetail(string ccy, string period, string group, string branch)
        {
            //ccy=IDR&period=202202&group=OE2&branch=DTN
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (MyTool.ValidateJSON(json))
            {
                var O = Newtonsoft.Json.Linq.JObject.Parse(json);
                var ccy_ = O.SelectToken("ccy").ToString();
                var period_ = O.SelectToken("period").ToString();
                var group_ = O.SelectToken("group").ToString();
                var branch_ = O.SelectToken("branch").ToString();
                var id_ = O.SelectToken("id").ToString();
                IDS.FixedAsset.FAGroup d = IDS.FixedAsset.FAJournal.GetFaGroup(group_);
                List<IDS.FixedAsset.FAAssetGroupDetail> dt = IDS.FixedAsset.FAJournal.GetFAAssetGroupDetail(ccy_, period_, group_, branch_);
                return_ = generateTable(d, dt, id_);
            }
            //{"ccy":"IDR","period=":"202212","group":"BL1","branch":"DTN"}
            return return_;
        }//JsonResult GetData

        private string generateTable(IDS.FixedAsset.FAGroup ss, List<IDS.FixedAsset.FAAssetGroupDetail> lx, string idtable)
        {
            var b = new System.Text.StringBuilder();
            //b.AppendLine("<table> ");
            //b.AppendLine("    <tr> ");
            //b.AppendLine("        <td>Depreciation Accumulation Acc</td> ");
            //b.AppendLine("        <td><b>:</b></td> ");
            //b.AppendLine("        <td><b>" + ss.GLAccumDepre + "</b></td> ");
            //b.AppendLine("        <td><b>-</b></td> ");
            //b.AppendLine("        <td><b>Acc.Depr.</b></td> ");
            //b.AppendLine("        <td><b>-</b></td> ");
            //b.AppendLine("        <td><b>" + ss.Description + "</b></td> ");
            //b.AppendLine("    </tr> ");
            //b.AppendLine("    <tr> ");
            //b.AppendLine("        <td>Expense Account</td> ");
            //b.AppendLine("        <td><b>:</b></td> ");
            //b.AppendLine("        <td><b>" + ss.GLAccDepreExpense + "</b></td> ");
            //b.AppendLine("        <td><b>-</b></td> ");
            //b.AppendLine("        <td><b>Exp - Depre</b></td> ");
            //b.AppendLine("        <td><b>-</b></td> ");
            //b.AppendLine("       <td><b>" + ss.Description + "</b></td> ");
            //b.AppendLine("   </tr> ");
            //b.AppendLine("</table> ");



            // Mulai looping dari sini

            // card start
            b.AppendLine("<div class='card'>");
            b.AppendLine("    <div class='card-body'>");

            b.AppendLine("<div class='row mb-1'>");
            b.AppendLine("    <div class='form-group row mb-1 col-sm-6'>");
            b.AppendLine("       <label class='col-form-label-sm col-sm-8'>Depreciation Accumulation Acc</label>");
            b.AppendLine("       <div class='col-sm-4'>");
            b.AppendLine("           <input class='form-control form-control-sm' type='text' value='" + ss.GLAccumDepre + "' placeholder='" + ss.GLAccumDepre + "' readonly>");
            b.AppendLine("       </div>");
            b.AppendLine("   </div>");
            b.AppendLine("   <div class='form-group row mb-1 col-sm-6'>");
            b.AppendLine("       <label class='col-form-label-sm col-sm-3'>Acc.Depr.</label>");
            b.AppendLine("       <div class='col-sm-9'>");
            b.AppendLine("          <input class='form-control form-control-sm' type='text' value='" + ss.Description + "' placeholder='" + ss.Description + "' readonly>");
            b.AppendLine("      </div>");
            b.AppendLine(" </div>");
            b.AppendLine("</div>");
            b.AppendLine("<div class='row mb-1'>");
            b.AppendLine("    <div class='form-group row mb-1 col-sm-6'>");
            b.AppendLine("       <label class='col-form-label-sm col-sm-8'>Expense Account</label>");
            b.AppendLine("       <div class='col-sm-4'>");
            b.AppendLine("           <input class='form-control form-control-sm'  type='text' value='" + ss.GLAccDepreExpense + "' placeholder='" + ss.GLAccDepreExpense + "' readonly>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("    <div class='form-group row mb-1 col-sm-6'>");
            b.AppendLine("       <label  class='col-form-label-sm col-sm-3'>Exp.Depr.</label>");
            b.AppendLine("       <div class='col-sm-9'>");
            b.AppendLine("           <input class='form-control form-control-sm' type='text' value='" + ss.Description + "' placeholder='" + ss.Description + "' readonly>");
            b.AppendLine("      </div>");
            b.AppendLine("  </div>");
            b.AppendLine("</div>");

            b.AppendLine("<table id='ctb" + idtable + "' class='table'>");
            b.AppendLine("    <thead class='thead-light'>");
            b.AppendLine("    <tr>");
            b.AppendLine("        <th scope='col'>Asset No.</th>");
            b.AppendLine("       <th scope='col'>Group</th>");
            b.AppendLine("       <th scope='col'>Beginning Value</th>");
            b.AppendLine("       <th scope='col'>Increment</th>");
            b.AppendLine("        <th scope='col'>Decrement</th>");
            b.AppendLine("        <th scope='col'>Depreciation</th>");
            b.AppendLine("       <th scope='col'>Ending Value</th>");
            b.AppendLine("   </tr>");
            b.AppendLine("  </thead>");
            b.AppendLine("  <tbody>");
            foreach (var x in lx)
            {
                b.AppendLine("<tr>");
                b.AppendLine("<td>" + x.AssetNo + "</td>");
                b.AppendLine("<td>" + x.AssetGroup + "</td>");
                b.AppendLine("<td>" + x.BegVal + "</td>");
                b.AppendLine("<td>" + x.Increment + "</td>");
                b.AppendLine("<td>" + String.Format("{0:C}", x.Decrement) + "</td>");
                b.AppendLine("<td>" + x.Depreciation + "</td>");
                b.AppendLine("<td>" + x.EndVal + "</td>");
                b.AppendLine("</tr>");
            }
            b.AppendLine("  </tbody>");
            b.AppendLine("</table>");

            // card stop
            b.AppendLine("   </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

        [HttpPost]
        public string Confirm(List<IDS.FixedAsset.FAProsessCmd> lstctr)
        {
            return CreAteMessageBox(lstctr);
        }
        [HttpPost]
        public string ConfirmToProccess(List<IDS.FixedAsset.FAProsessCmd> lstctr)
        {
            string return_ = "";

            IDS.FixedAsset.FAJournal ssUSERiD = new IDS.FixedAsset.FAJournal();
            ssUSERiD.OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString();

            //if (ssUSERiD.ProccessGo(lstctr))
            //{
            //    return_ = simplemsg();
            //}
            return_ = ssUSERiD.ProccessGo(lstctr);

            if (string.IsNullOrEmpty(return_))
            {
                return_ = "Process Success";
            }

            return return_;
        }

        string CreAteMessageBox(List<IDS.FixedAsset.FAProsessCmd> lstctr)
        {
            var b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgbox' tabindex='-1' role='dialog' aria-labelledby='exampleModalCenterTitle' aria-hidden='true'>");
            b.AppendLine("   <div class='modal-dialog modal-dialog-centered modal-dialog-scrollable' role='document'>");
            b.AppendLine("        <div class='modal-content'>");
            b.AppendLine("           <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='exampleModalCenterTitle'>Confirm Process</h5>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goClose()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("            </button>");
            b.AppendLine("       </div>");
            b.AppendLine("       <div class='modal-body'>");
            b.AppendLine("          <table class='table' id='tblmsgbox'>");
            b.AppendLine("              <thead>");
            b.AppendLine("              <tr>");
            b.AppendLine("                  <th scope='col'>#</th>");
            b.AppendLine("                   <th scope='col'>Item Group</th>");
            b.AppendLine("                   <th scope='col'>Period</th>");
            b.AppendLine("                   <th scope='col'>Currency</th>");
            b.AppendLine("                   <th scope='col'>Branch</th>");
            b.AppendLine("              </tr>");
            b.AppendLine("               </thead>");
            b.AppendLine("               <tbody>");

            int indext = 0;
            foreach (var x in lstctr)
            {
                indext += 1;
                b.AppendLine("               <tr>");
                b.AppendLine("                    <th scope='row'>" + indext + ".</th>");
                b.AppendLine("                     <td>" + x.ItemGroup + "</td>");
                b.AppendLine("                     <td>" + x.Period + "</td>");
                b.AppendLine("                     <td>" + x.IDR + "</td>");
                b.AppendLine("                     <td>" + x.Branch + "</td>");
                b.AppendLine("                 </tr>");
            }

            b.AppendLine("                 </tbody>");
            b.AppendLine("             </table>");
            b.AppendLine("            <table class='table' id='tbltextconfirm'>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <th scope='row'>Are You Sure?</th>");
            b.AppendLine("                </tr>");
            b.AppendLine("            </table>");
            b.AppendLine("        </div>");
            b.AppendLine("       <div class='modal-footer'>");
            b.AppendLine("           <div class='container'>");
            b.AppendLine("               <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='goClose()'>No</button>");
            b.AppendLine("                        <button type='button' class='btn btn-primary' onclick='goProccess()'>Yes</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                 </div>");
            b.AppendLine("            </div>");
            b.AppendLine("       </div>");
            b.AppendLine("   </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

        string simplemsg()
        {
            var b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgok' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("   <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("      <div class='modal-content'>");
            b.AppendLine("          <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='ee'>Confirm Process</h5>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeMsg()'>");
            b.AppendLine("                 <span aria-hidden='true'>&times;</span>");
            b.AppendLine("           </button>");
            b.AppendLine("        </div>");
            b.AppendLine("       <div class='modal-body'>");
            b.AppendLine("           <h2>Proccess Hasben Succesfully</h2>");
            b.AppendLine("       </div>");
            b.AppendLine("       <div class='modal-footer'>");
            b.AppendLine("           <button type='button' class='btn btn-primary btn-block' data-dismiss='modal' onclick='closeMsg()'>Close</button>");
            b.AppendLine("      </div>");
            b.AppendLine("    </div>");
            b.AppendLine("  </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

    }
}