using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FAProcessTransController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: FixedAsset/FAProcessTrans
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["SelectListBranch"] = new SelectList(IDS.GeneralTable.Branch.GetBranchForDatasource(), "Value", "Text", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE]);

            ViewBag.Session_UserId = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID];
            ViewBag.Session_Branch = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE];
            ViewBag.Session_HoStatus = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS];

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();

            return View();
        }

        [HttpPost]
        public string GetTrans(string branch)
        {
            List<IDS.FixedAsset.FAProcessTrans> list = IDS.FixedAsset.FAProcessTrans.getFAProcessTrans(branch);
            System.Data.DataTable t = new System.Data.DataTable();
            t.Columns.Add("DESC", typeof(string));
            t.Columns.Add("VAL", typeof(string));
            foreach (var x in list)
            {
                t.Rows.Add(x.TransNo, x.Descript);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(t);
        }

        [HttpPost]
        public string CheckStatusHO(string HoStatus)
        {
            string return_ = "false";
            //result->IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS=Login.HOStatus
            //result->Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]=true
            //System.Diagnostics.Debug.WriteLine(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]);
            string h = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS].ToString().ToLower();
            if (h == "true")
            {
                return_ = "true";
            }
            return return_;
        }

        [HttpPost]
        public string CheckStatus(string branch, string TransNo)
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            if (MyTool.ValidateJSON(json))
            {
                System.Diagnostics.Debug.WriteLine(json);
                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                var branch_ = myJObject.SelectToken("branch").ToString();
                var TransNo_ = myJObject.SelectToken("TransNo").ToString();
                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(IDS.FixedAsset.FAProcessTrans.getStatus(branch_, TransNo_));
            }
            return return_;
        }
        [HttpPost]
        public string ConfirmProsesFirst(string branch, string TransNo)
        {
            return msgBoxFirst(IDS.FixedAsset.FAProcessTrans.GetDataTRx(branch, TransNo));
        }

        [HttpPost]
        public string ConfirmCancelProcess(string branch, string TransNo)//konfirm cancell msg 1
        {
            return ConfirmMsgForCancel(IDS.FixedAsset.FAProcessTrans.GetDataTRx(branch, TransNo));
        }
        [HttpPost]
        public string ConfirmCancelNext(string branch, string TransNo)//konfirm cancell msg 2
        {
            return MsgCancelSuccess(IDS.FixedAsset.FAProcessTrans.GoProsessCancell(TransNo, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), DateTime.Now));
        }

        private string ConfirmMsgForCancel(IDS.FixedAsset.FAProcessTrans x)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgcancel' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("    <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("       <div class='modal-content'>");
            b.AppendLine("          <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='ee'>Confirm Cancel Process</h5>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goCancellClose()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("            </button>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-body'>");
            b.AppendLine("            <table id='tblcancel' class='table'>");
            b.AppendLine("                <tbody>");
            b.AppendLine("                <tr>");
            b.AppendLine("                     <th scope='row'>TransNo</th>");
            b.AppendLine("                   <th scope='row'>:</th>");
            b.AppendLine("                   <td colspan='2' scope='row'>" + x.TransNo + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <th scope='row'>Price</th>");
            b.AppendLine("                    <th scope='row'>:</th>");
            b.AppendLine("                    <td colspan='3' scope='row'>" + x.Price + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <th scope='row'>Voucher</th>");
            b.AppendLine("                    <th scope='row'>:</th>");
            b.AppendLine("                    <td colspan='3' scope='row'>" + x.VoucherNoFrom + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <th scope='row'>Branch</th>");
            b.AppendLine("                    <th scope='row'>:</th>");
            b.AppendLine("                     <td colspan='3'>" + x.FromBranch + "</td>");
            b.AppendLine("                 </tr>");
            b.AppendLine("                <tr>");
            b.AppendLine("                    <th scope='row'>Desc</th>");
            b.AppendLine("                    <th scope='row'>:</th>");
            b.AppendLine("                    <td colspan='3'>" + x.Descript + "</td>");
            b.AppendLine("                </tr>");
            b.AppendLine("                </tbody>");
            b.AppendLine("            </table>");
            b.AppendLine("            <table class='table' id='tbltextconfirm'>");
            b.AppendLine("               <tr>");
            b.AppendLine("                     <th scope='row'>Are You Sure to Cancel?</th>");
            b.AppendLine("                 </tr>");
            b.AppendLine("            </table>");
            b.AppendLine("       </div>");
            b.AppendLine("       <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='goCancellClose()'>No</button>");
            b.AppendLine("                        <button type='button' class='btn btn-primary' onclick='goCancell()'>Yes</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine(" </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }


        [HttpPost]
        public string ConfirmProsesNext(string branch, string TransNo)
        {
            string return_ = "";
            string msg = "";
            return_ = MsgOK(IDS.FixedAsset.FAProcessTrans.GoProsessTrans(TransNo, branch, Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(), DateTime.Now, msg));
            return return_;
        }

        private string msgBoxFirst(IDS.FixedAsset.FAProcessTrans x)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgok' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("     <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("        <div class='modal-content'>");
            b.AppendLine("           <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='ee'>Confirm Process</h5>");
            b.AppendLine("            <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='goClose()'>");
            b.AppendLine("                                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("          </button>");
            b.AppendLine("      </div>");
            b.AppendLine("      <div class='modal-body'>");
            b.AppendLine("                   <table id='tbltrx' class='table'>");
            b.AppendLine("                             <tbody>");
            b.AppendLine("                                    <tr>");
            b.AppendLine("                                        <th scope='row'>TransNo</th>");
            b.AppendLine("                                        <th scope='row'>:</th>");
            b.AppendLine("                                        <td colspan='2' scope='row'>" + x.TransNo + "</td>");
            b.AppendLine("                                     </tr>");
            b.AppendLine("                                      <tr>");
            b.AppendLine("                                          <th scope='row'>Price</th>");
            b.AppendLine("                                          <th scope='row'>:</th>");
            b.AppendLine("                                         <td colspan='3' scope='row'>" + x.Price + "</td>");
            b.AppendLine("                                     </tr>");
            b.AppendLine("                                     <tr>");
            b.AppendLine("                                          <th scope='row'>Voucher</th>");
            b.AppendLine("                                        <th scope='row'>:</th>");
            b.AppendLine("                                         <td colspan='3' scope='row'>" + x.VoucherNoFrom + "</td>");
            b.AppendLine("                                    </tr>");
            b.AppendLine("                                    <tr>");
            b.AppendLine("                                        <th scope='row'>Branch</th>");
            b.AppendLine("                                        <th scope='row'>:</th>");
            b.AppendLine("                                        <td colspan='3' >" + x.FromBranch + "</td>");
            b.AppendLine("                                    </tr>");
            b.AppendLine("                                  <tr>");
            b.AppendLine("                                       <th scope='row'>Desc</th>");
            b.AppendLine("                                      <th scope='row'>:</th>");
            b.AppendLine("                                      <td colspan='3' >" + x.Descript + "</td>");
            b.AppendLine("                                  </tr>");
            b.AppendLine("                              </tbody>");
            b.AppendLine("                          </table>");
            b.AppendLine("                    <table class='table' id='tbltextconfirm'>");
            b.AppendLine("                              <tr>");
            b.AppendLine("                                       <th scope='row'>Are You Sure?</th>");
            b.AppendLine("                                   </tr>");
            b.AppendLine("                          </table>");
            b.AppendLine("      </div>");
            b.AppendLine("      <div class='modal-footer'>");
            b.AppendLine("                     <div class='container'>");
            b.AppendLine("                             <div class='row'>");
            b.AppendLine("                                      <div class='col text-center'>");
            b.AppendLine("                                              <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='goClose()'>No</button>");
            b.AppendLine("                                               <button type='button' class='btn btn-primary' onclick='goProccess()'>Yes</button>");
            b.AppendLine("                                           </div>");
            b.AppendLine("                                    </div>");
            b.AppendLine("                      </div>");
            b.AppendLine("       </div>");
            b.AppendLine("    </div>");
            b.AppendLine("  </div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

        private string MsgCancelSuccess(string msg)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();

            b.AppendLine("<div class='modal fade' id='msgccl' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("    <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("       <div class='modal-content'>");
            b.AppendLine("           <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='ee'>Confirm Process</h5>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='cancelcloseMsg()()'>");
            b.AppendLine("                 <span aria-hidden='true'>&times;</span>");
            b.AppendLine("             </button>");
            b.AppendLine("         </div>");
            b.AppendLine("         <div class='modal-body'>");
            b.AppendLine("                <h2>" + msg + "</h2>");
            b.AppendLine("        </div>");
            b.AppendLine("       <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-primary btn-block' onclick='cancelcloseMsg()'>Yes</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");
            return b.ToString();
        }

        //MsgCancelSuccess   MsgOK

        private string MsgOK(string msg)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();

            b.AppendLine("<div class='modal fade' id='msgok' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("    <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("       <div class='modal-content'>");
            b.AppendLine("           <div class='modal-header'>");
            b.AppendLine("              <h5 class='modal-title' id='ee'>Confirm Process</h5>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeMsg()'>");
            b.AppendLine("                 <span aria-hidden='true'>&times;</span>");
            b.AppendLine("             </button>");
            b.AppendLine("         </div>");
            b.AppendLine("         <div class='modal-body'>");
            b.AppendLine("                <h2>" + msg + "</h2>");
            b.AppendLine("        </div>");
            b.AppendLine("       <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-primary btn-block' onclick='closeMsg()'>Yes</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");
            return b.ToString();
        }
    }
}