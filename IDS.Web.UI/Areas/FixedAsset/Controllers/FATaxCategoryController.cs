using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace IDS.Web.UI.Areas.FixedAsset.Controllers
{
    public class FATaxCategoryController : IDS.Web.UI.Controllers.MenuController
    {
        // GET: FixedAsset/FATaxCategory
        public ActionResult Index()
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                return RedirectToAction("index", "Main", new { area = "" });

            IDS.Web.UI.Models.GroupAccessLevel AccessLevel = IDS.Web.UI.Models.GroupAccessLevel.GetFormGroupAccess(Convert.ToString(Session[IDS.Tool.GlobalVariable.SESSION_USER_GROUP_CODE]), this.ControllerContext.RouteData.Values["controller"].ToString());

            if (AccessLevel.CreateAccess == -1 || AccessLevel.EditAccess == -1 || AccessLevel.DeleteAccess == -1)
            {
                RedirectToAction("Index", "Main", new { Area = "" });
            }

            ViewData["SelectListTask"] = new SelectList(IDS.FixedAsset.FATaxCategory.GetFATaxCategoryForDatasource(), "Value", "Text");
            ViewData["DepreMethodList"] = IDS.Tool.EnumExtensions.ToSelectList<IDS.FixedAsset.FADepreMethod>(IDS.FixedAsset.FADepreMethod.Double_Declining);

            ViewBag.UserMenu = MainMenu;
            ViewBag.UserLogin = Session[Tool.GlobalVariable.SESSION_USER_ID].ToString();
            return View();
        }

        public ActionResult GetData(string GrouP, string Method)
        {
            if (Session[IDS.Tool.GlobalVariable.SESSION_USER_ID] == null)
                throw new Exception("You do not have access to access this page");

            switch (Method.ToLower())
            {
                case "straight_line":
                    Method = "0";
                    break;
                case "double_declining":
                    Method = "1";
                    break;
                case "not_depreciate":
                    Method = "2";
                    break;
                default:
                    Method = "";
                    break;
            }

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
                        //pageSize = 0;
                        break;
                    default:
                        pageSize = Convert.ToInt32(length);
                        break;
                }

                int skip = start != null ? Convert.ToInt32(start) : 0;
                int totalRecords = 0; // Total keseluruhan data
                int totalRecordsShowing = 0; // Total data setelah filter / search

                List<IDS.FixedAsset.FATaxCategory> acc = IDS.FixedAsset.FATaxCategory.GetData(GrouP, Method);
                var v = acc.SelectMany(x => x.TaxID.Select(y => new
                {
                    TaxID = x.TaxID,
                    TaxDescription = x.TaxDescription,
                    DepreMethodToString = x.DepreMethodToString,
                    DepreMethod = x.DepreMethod,
                })).Distinct();

                totalRecords = v.Count();

                // Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    v = v.OrderBy(sortColumn + " " + sortColumnDir).ToList();
                }

                // search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    string searchValueLower = searchValue.ToLower();
                    v = v.Where(x => x.TaxID.ToString().ToLower().Contains(searchValueLower) ||
                    x.TaxDescription.ToString().ToLower().Contains(searchValueLower) ||
                    x.DepreMethod.ToString().ToLower().Contains(searchValueLower) ||
                    x.DepreMethodToString.ToString().ToLower().Contains(searchValueLower)).ToList();
                }

                totalRecordsShowing = acc.Count();
                if (pageSize > 0)
                {
                    acc = acc.Skip(skip).Take(pageSize).ToList();
                }
                result = this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return result;
        }//GetData
        // bawah asli
        //public ActionResult GetData(string GrouP, string Method)
        //{
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int totalRecords = 0; // Total keseluruhan data
        //    int totalRecordsShowing = 0; // Total data setelah filter / search
        //    List<IDS.FixedAsset.FATaxCategory> acc = IDS.FixedAsset.FATaxCategory.GetData(GrouP, Method);
        //    totalRecords = acc.Count;
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        string searchValueLower = searchValue.ToLower();
        //        acc = acc.Where(x => x.TaxID.ToString().ToLower().Contains(searchValueLower) || x.TaxDescription.ToString().ToLower().Contains(searchValueLower) || x.DepreMethod.ToString().ToLower().Contains(searchValueLower) || x.OperatorID.ToString().ToLower().Contains(searchValueLower)).ToList();
        //    }
        //    totalRecordsShowing = acc.Count();
        //    acc = acc.Skip(skip).Take(pageSize).ToList();
        //    return this.Json(new { recordsFiltered = totalRecordsShowing, recordsTotal = totalRecords, data = acc }, JsonRequestBehavior.AllowGet);
        //}//GetData

        [HttpPost]
        public string GetSaveOrEdit(string data)
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();

            if (MyTool.ValidateJSON(json))
            {
                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                var type_ = myJObject.SelectToken("type").ToString();
                var taxid_ = myJObject.SelectToken("tax.TaxCatID").ToString();
                var TaxCatDesc_ = myJObject.SelectToken("tax.TaxCatDesc").ToString();
                var TaxCatDepMethod_ = myJObject.SelectToken("tax.TaxCatDepMethod").ToString();
                var TaxCatRate_ = myJObject.SelectToken("tax.TaxCatRate").ToString();
                var TaxCatDepYear_ = myJObject.SelectToken("tax.TaxCatDepYear").ToString();


                switch (type_.ToLower())
                {
                    case "0"://Insert
                        return_ = ShowFormNew("Add New Category");
                        break;
                    case "1"://Update
                        return_ = ShowFormEdit(IDS.FixedAsset.FATaxCategory.GetDataGroup(taxid_), Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString());
                        break;
                    case "2"://Delete
                        return_ = ShowFormDelete(IDS.FixedAsset.FATaxCategory.GetDataGroup(taxid_), Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString());
                        break;
                }

            }
            return return_;
        }//GetData

        [HttpPost]
        public string GoSaveOrEdit(string data)
        {
            string return_ = "";
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();

            if (MyTool.ValidateJSON(json))
            {
                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                var type_ = myJObject.SelectToken("type").ToString();
                var taxid_ = myJObject.SelectToken("tax.TaxCatID").ToString();
                var TaxCatDesc_ = myJObject.SelectToken("tax.TaxCatDesc").ToString();
                var TaxCatDepMethod_ = myJObject.SelectToken("tax.TaxCatDepMethod").ToString();
                var TaxCatRate_ = myJObject.SelectToken("tax.TaxCatRate").ToString();
                var TaxCatDepYear_ = myJObject.SelectToken("tax.TaxCatDepYear").ToString();
                //var s = "type_=>" + type_+'\n'+ "Taxt_id=>" + taxid_ +  "\n" + "TaxCatDesc=>" + TaxCatDesc_+ "\n" + "TaxCatDepMethod_"+TaxCatDepMethod_ + "\n" + "Taxx Year=>" + TaxCatDepYear_ ;
                //System.Diagnostics.Debug.WriteLine(s);

                switch (type_.ToLower())
                {
                    case "0"://Insert
                        if (string.IsNullOrEmpty(taxid_) || string.IsNullOrEmpty(TaxCatDesc_) || string.IsNullOrEmpty(TaxCatDepMethod_) || string.IsNullOrEmpty(TaxCatRate_) || string.IsNullOrEmpty(TaxCatDepYear_))
                        {
                            return_ = MsgResult("Data Not Complete!");
                        }
                        else
                        {
                            return_ = MsgResult(IDS.FixedAsset.FATaxCategory.SaveFATaxCategory(new IDS.FixedAsset.FATaxCategory()
                            {
                                DepreMethodToString = TaxCatDepMethod_,
                                EntryDate = System.DateTime.Now,
                                EntryUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                                LastUpdate = DateTime.Now,
                                OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                                Rate = int.Parse(TaxCatRate_),
                                TaxDescription = TaxCatDesc_,
                                TaxID = taxid_,
                                Year = int.Parse(TaxCatDepYear_)
                            }, 4));
                        }
                        break;
                    case "1"://Update
                        return_ = MsgResult(IDS.FixedAsset.FATaxCategory.SaveFATaxCategory(new IDS.FixedAsset.FATaxCategory()
                        {
                            DepreMethodToString = TaxCatDepMethod_,
                            EntryDate = System.DateTime.Now,
                            EntryUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                            LastUpdate = DateTime.Now,
                            OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                            Rate = int.Parse(TaxCatRate_),
                            TaxDescription = TaxCatDesc_,
                            TaxID = taxid_,
                            Year = int.Parse(TaxCatDepYear_)
                        }, 5));
                        break;
                    case "2"://Delete
                        return_ = MsgResult(IDS.FixedAsset.FATaxCategory.SaveFATaxCategory(new IDS.FixedAsset.FATaxCategory()
                        {
                            DepreMethodToString = TaxCatDepMethod_,
                            EntryDate = System.DateTime.Now,
                            EntryUser = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                            LastUpdate = DateTime.Now,
                            OperatorID = Session[IDS.Tool.GlobalVariable.SESSION_USER_ID].ToString(),
                            Rate = int.Parse(TaxCatRate_),
                            TaxDescription = TaxCatDesc_,
                            TaxID = taxid_,
                            Year = int.Parse(TaxCatDepYear_)
                        }, 6));
                        break;

                }

            }
            return return_;
        }//GetData

        private string MsgResult(string msg)
        {
            string retur_ = "";
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgok' tabindex='-1' role='dialog' aria-labelledby='msgok' aria-hidden='true'>");
            b.AppendLine("   <div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("     <div class='modal-content'>");
            b.AppendLine("         <div class='modal-header'>");
            b.AppendLine("             <h4 class='modal-title' id='cc'>Confirm Message</h4>");
            b.AppendLine("             <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeConfirm()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("           </button>");
            b.AppendLine("       </div>");
            b.AppendLine("        <div class='modal-body'>");
            b.AppendLine("            <h6>" + msg + "</h6>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-footer'>");
            b.AppendLine("           <div class='container'>");
            b.AppendLine("               <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-primary btn-block' data-dismiss='modal' onclick='closeConfirm()'>Close</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("       </div>");
            b.AppendLine("     </div>");
            b.AppendLine(" </div>");
            b.AppendLine("</div>");
            retur_ = b.ToString();
            return retur_;
        }

        private string ShowFormNew(string titleMsg)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgbox' tabindex='-1' role='dialog' aria-labelledby='msgbox' aria-hidden='true'>");
            b.AppendLine("<div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("   <div class='modal-content'>");
            b.AppendLine("      <div class='modal-header'>");
            b.AppendLine("         <h6 class='modal-title' id='msgtitle'>" + titleMsg + "</h6>");
            b.AppendLine("          <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeMsgBox()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("            </button>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-body'>");
            b.AppendLine("           <form>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                   <label for='txttaxid' class='col-sm-2 col-form-label form-control-sm'>Tax ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-5'>");
            b.AppendLine("                        <input type='text' class='form-control form-control-sm' id='txttaxid' placeholder='Tax ID' required min='3' max='200'> ");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtmethod' class='col-sm-2 col-form-label form-control-sm'>Method</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <select class='form-control form-control-sm' id='txtmethod' onchange='txtmethod_Onchange(event)' required min='1' max='200' >");
            b.AppendLine("                            <option></option>");
            b.AppendLine("                            <option value='0'>Straight Line</option>");
            b.AppendLine("                            <option value='1'>Double Declining</option>");
            b.AppendLine("                            <option value='2'>Not Depreciate</option>");
            b.AppendLine("                       </select>");
            b.AppendLine("                     </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtyear' class='col-sm-2 col-form-label form-control-sm'>Year</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='number' class='form-control form-control-sm' id='txtyear' placeholder='Year' oninput='txtyear_Onchange()' required min='1' max='10' >");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtrate'  class='col-sm-2 col-form-label form-control-sm'>Rate</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' class='form-control form-control-sm' id='txtrate' readonly placeholder='Rate' required min='1' max='200' >");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtarea' class='col-sm-2 col-form-label form-control-sm'>Description</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <textarea class='form-control form-control-sm' id='txtdesc' rows='2' required min='5' max='200'></textarea>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtoperatorid' class='col-sm-2 col-form-label form-control-sm'>Operator_ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtoperatorid' value='admin1'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtlastupdate' class='col-sm-2 col-form-label form-control-sm'>Last_Update</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtlastupdate' value='1/6/2022 3:09:56 PM'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </form>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='closeMsgBox()'>");
            b.AppendLine("                            Close");
            b.AppendLine("                        </button>");
            b.AppendLine("                        <button type='button' class='btn btn-primary' onclick='goSave()'>Save</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");

            return b.ToString();
        }

        private string ShowFormEdit(IDS.FixedAsset.FATaxCategory d, string OprtId)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgboxupdate' tabindex='-1' role='dialog' aria-labelledby='msgbox' aria-hidden='true'>");
            b.AppendLine("<div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("   <div class='modal-content'>");
            b.AppendLine("      <div class='modal-header'>");
            b.AppendLine("         <h5 class='modal-title' id='msgtitle'>Edit Category</h5>");
            b.AppendLine("          <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeUpdate()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("            </button>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-body'>");
            b.AppendLine("           <form>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                   <label for='txttaxid' class='col-sm-2 col-form-label form-control-sm'>Tax ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-5'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txttaxid' placeholder='Tax ID' value='" + d.TaxID + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtmethod' class='col-sm-2 col-form-label form-control-sm'>Method</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <select class='form-control form-control-sm' id='txtmethod' onchange='txtmethod_Onchange(event)'>");
            switch (d.DepreMethodToString.ToString().ToLower())
            {
                case "straight line":
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    break;
                case "double declining":
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    break;
                case "not depreciate":
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    break;
            }
            b.AppendLine("                       </select>");
            b.AppendLine("                     </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtyear' class='col-sm-2 col-form-label form-control-sm'>Year</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='number' class='form-control form-control-sm' id='txtyear' placeholder='Year' value='" + d.Year + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtrate' class='col-sm-2 col-form-label form-control-sm'>Rate</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' class='form-control form-control-sm' id='txtrate' placeholder='Rate' value='" + d.Rate + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtdesc' class='col-sm-2 col-form-label form-control-sm'>Description</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <textarea class='form-control form-control-sm' id='txtdesc' rows='2' value='" + d.TaxDescription + "'>" + d.TaxDescription + "</textarea>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtoperatorid' class='col-sm-2 col-form-label form-control-sm'>Operator_ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtoperatorid' value='" + OprtId + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtlastupdate' class='col-sm-2 col-form-label form-control-sm'>Last_Update</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtlastupdate' value='" + System.DateTime.Now + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </form>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='closeUpdate()'>");
            b.AppendLine("                            Close");
            b.AppendLine("                        </button>");
            b.AppendLine("                        <button type='button' class='btn btn-primary' onclick='goUpdate()'>Update</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");

            return b.ToString();
        }

        private string ShowFormDelete(IDS.FixedAsset.FATaxCategory d, string OprtId)
        {
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            b.AppendLine("<div class='modal fade' id='msgboxdelete' tabindex='-1' role='dialog' aria-labelledby='msgbox' aria-hidden='true'>");
            b.AppendLine("<div class='modal-dialog modal-dialog-centered' role='document'>");
            b.AppendLine("   <div class='modal-content'>");
            b.AppendLine("      <div class='modal-header'>");
            b.AppendLine("         <h6 class='modal-title' id='msgtitle'>Delete Category</h6>");
            b.AppendLine("          <button type='button' class='close' data-dismiss='modal' aria-label='Close' onclick='closeDelete()'>");
            b.AppendLine("                <span aria-hidden='true'>&times;</span>");
            b.AppendLine("            </button>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-body'>");
            b.AppendLine("           <form>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                   <label for='txttaxid' class='col-sm-2 col-form-label form-control-sm'>Tax ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-5'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txttaxid' placeholder='Tax ID' value='" + d.TaxID + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtmethod' class='col-sm-2 col-form-label form-control-sm'>Method</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <select class='form-control form-control-sm' id='txtmethod' onchange='txtmethod_Onchange(event)'>");
            switch (d.DepreMethodToString.ToString().ToLower())
            {
                case "straight line":
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    break;
                case "double declining":
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    break;
                case "not depreciate":
                    b.AppendLine("<option value='2'>Not Depreciate</option>");
                    b.AppendLine("<option value='1'>Double Declining</option>");
                    b.AppendLine("<option value='0'>Straight Line</option>");
                    break;
            }
            b.AppendLine("                       </select>");
            b.AppendLine("                     </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtyear' class='col-sm-2 col-form-label form-control-sm'>Year</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='number' class='form-control form-control-sm' id='txtyear' placeholder='Year' value='" + d.Year + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtrate' class='col-sm-2 col-form-label form-control-sm'>Rate</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' class='form-control form-control-sm' id='txtrate' placeholder='Rate' value='" + d.Rate + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtdesc' class='col-sm-2 col-form-label form-control-sm'>Description</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <textarea class='form-control form-control-sm' id='txtdesc' rows='2' value='" + d.TaxDescription + "'>" + d.TaxDescription + "</textarea>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                <div class='form-group row'>");
            b.AppendLine("                    <label for='txtoperatorid' class='col-sm-2 col-form-label form-control-sm'>Operator_ID</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtoperatorid' value='" + OprtId + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("                 <div class='form-group row'>");
            b.AppendLine("                    <label for='txtlastupdate' class='col-sm-2 col-form-label form-control-sm'>Last_Update</label>");
            b.AppendLine("");
            b.AppendLine("                    <div class='col-sm-9'>");
            b.AppendLine("                        <input type='text' readonly class='form-control form-control-sm' id='txtlastupdate' value='" + System.DateTime.Now + "'>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </form>");
            b.AppendLine("        </div>");
            b.AppendLine("        <div class='modal-footer'>");
            b.AppendLine("            <div class='container'>");
            b.AppendLine("                <div class='row'>");
            b.AppendLine("                    <div class='col text-center'>");
            b.AppendLine("                        <button type='button' class='btn btn-danger' data-dismiss='modal' onclick='closeDelete()'>");
            b.AppendLine("                            Close");
            b.AppendLine("                        </button>");
            b.AppendLine("                        <button type='button' class='btn btn-primary' onclick='goDelete()'>Delete</button>");
            b.AppendLine("                    </div>");
            b.AppendLine("                </div>");
            b.AppendLine("            </div>");
            b.AppendLine("        </div>");
            b.AppendLine("    </div>");
            b.AppendLine("</div>");
            b.AppendLine("</div>");

            return b.ToString();
        }


        [HttpPost]
        public string CalculateRatex(string data)
        {
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            string return_ = "";
            double Rate = 0;
            if (MyTool.ValidateJSON(json))
            {
                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                var type_ = myJObject.SelectToken("type").ToString();
                var year_ = myJObject.SelectToken("year").ToString();
                if (string.IsNullOrEmpty(year_))
                {
                    //System.Diagnostics.Debug.WriteLine("ini Noll");
                    year_ = "0";
                }
                //if (string.IsNullOrWhiteSpace(year_)) year_ = "0";
                switch (type_)
                {
                    case "0":
                        Rate = 100;
                        string RateVal0 = Convert.ToString(Math.Round(Rate / Convert.ToDouble(year_), 2));
                        var dt0 = new { ReadOnly = false, Rate = RateVal0 };
                        return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt0);
                        break;
                    case "1": //Double Declining  
                        Rate = 200;
                        string RateVal = Convert.ToString(Math.Round(Rate / Convert.ToDouble(year_), 2));
                        var dt = new { ReadOnly = false, Rate = RateVal };
                        return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
                        break;
                    case "2":  //Not Depreciate</option>
                        Rate = 0;
                        string RateVal2 = Convert.ToString(Math.Round(Rate / Convert.ToDouble(year_), 2));
                        var dt2 = new { ReadOnly = true, Rate = RateVal2 };
                        return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt2);
                        break;
                }
            }
            return return_;
        }// calculate rate


        [HttpPost]
        public string CalculateRate(string data)
        {
            System.IO.Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new System.IO.StreamReader(req).ReadToEnd();
            string return_ = "";
            //System.Diagnostics.Debug.WriteLine(json);

            double Rate = 0;
            if (MyTool.ValidateJSON(json))
            {
                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                var type_ = myJObject.SelectToken("type").ToString();
                var year_ = myJObject.SelectToken("year").ToString();
                //System.Diagnostics.Debug.WriteLine("type_==>"+type_+ "  year_=>"+year_);
                try
                {
                    if (Convert.ToInt32(year_) > 0)
                    {
                        switch (type_)
                        {
                            case "0":// straing line
                                Rate = 100;
                                Rate = Math.Round(Rate / Convert.ToDouble(year_), 2);
                                var dt0 = new { ReadOnly = true, Rate = Rate };
                                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt0);
                                break;
                            case "1":// double decline
                                Rate = 200;
                                Rate = Math.Round(Rate / Convert.ToDouble(year_), 2);
                                var dt1 = new { ReadOnly = true, Rate = Rate };
                                return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt1);
                                break;
                            default:
                                Rate = 0;
                                break;
                        }
                    }
                }
                catch (Exception Ex)
                {
                    var dt0 = new { ReadOnly = true, Rate = 0 };
                    return_ = Newtonsoft.Json.JsonConvert.SerializeObject(dt0);
                }


            }
            //System.Diagnostics.Debug.WriteLine(return_);
            return return_;
        }
    }

    static class MyTool
    {
        public static bool ValidateJSON(this string s)
        {
            try
            {
                Newtonsoft.Json.Linq.JToken.Parse(s);
                return true;
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                return false;
            }
        }
    }

}