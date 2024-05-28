using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptReceiptAll : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCust();
            }

            try
            {
                string menuCodeEncrypted = Request.QueryString["rpt"];

                if (string.IsNullOrWhiteSpace(menuCodeEncrypted))
                    Response.Redirect("~/Error/Error403");

                string menuCodeDecrypted = IDS.Tool.UrlEncryption.DecryptParam(menuCodeEncrypted);

                int groupAccess = IDS.Web.UI.Models.GroupAccessLevel.GetGroupAccessLevelByMenuCode(Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE] as string, menuCodeDecrypted);

                if (groupAccess <= 0)
                    Response.Redirect("~/Error/Error403");
                switch (menuCodeDecrypted)
                {
                    case "0305220000000000": // 
                        string judul_ = "Receipt";
                        this.Page.Title = judul_;
                        this.txtJudul.InnerHtml = judul_;
                        var Split_Val = Request.Params["ctl00$ContentPlaceHolder1$txtInvoiceNo"];
                        var cust_ = Request.Params["ctl00$ContentPlaceHolder1$cboCustomer"];
                        rpt.Load(Server.MapPath(@"~/Report/Sales/CR/Receipt.rpt"));
                        rpt.SetParameterValue("@BranchCode", "DTN");
                        decimal amount = 0;
                        var NoReceipt_ = "";
                        var NoInvoice_ = "";

                        if (!string.IsNullOrEmpty(Split_Val) && IsExist(Split_Val, "="))
                        {
                            if (Split_Val.Split('=')[3] == "1")
                            {
                                NoInvoice_ = Split_Val.Split('=')[0];
                            }
                            else
                            {
                                NoReceipt_ = Split_Val.Split('=')[1];
                            }
                            
                            
                            amount = Convert.ToDecimal(Split_Val.Split('=')[2]);

                            Fill_Invoice(cust_);

                            //System.Diagnostics.Debug.WriteLine(new IDS.Tool.clsNumberToWord().NumToWordInd(Convert.ToDouble(amount), 1));
                            
                            if (string.IsNullOrEmpty(NoReceipt_))
                            {
                                rpt.SetParameterValue("@NoReceipt", "");
                            }
                            else
                            {
                                rpt.SetParameterValue("@NoReceipt", IDS.Tool.GeneralHelper.StringToDBNull(NoReceipt_));
                            }
                            if (string.IsNullOrEmpty(NoInvoice_))
                            {
                                //rpt.SetParameterValue("@NoInvoice", DBNull.Value);
                                rpt.SetParameterValue("@NoInvoice", "");
                            }
                            else
                            {
                                rpt.SetParameterValue("@NoInvoice", IDS.Tool.GeneralHelper.StringToDBNull(NoInvoice_));
                            }
                            if (string.IsNullOrEmpty(NoReceipt_))
                            {
                                //rpt.SetParameterValue("@NoReceipt",DBNull.Value, "ReceiptSub");
                                rpt.SetParameterValue("@NoReceipt", "", "ReceiptSub");
                            }
                            else
                            {
                                rpt.SetParameterValue("@NoReceipt", NoReceipt_, "ReceiptSub");
                            }

                            if (string.IsNullOrEmpty(NoInvoice_))
                            {
                                //rpt.SetParameterValue("@NoInvoice", DBNull.Value, "ReceiptSub");
                                rpt.SetParameterValue("@NoInvoice", "", "ReceiptSub");
                            }
                            else
                            {
                                rpt.SetParameterValue("@NoInvoice", NoInvoice_, "ReceiptSub");
                            }
                            
                            
                            //amount = Convert.ToDecimal(inVno_);
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine("kOSONG");
                            //rpt.SetParameterValue("@NoReceipt", DBNull.Value);
                            rpt.SetParameterValue("@NoReceipt", "");
                            rpt.SetParameterValue("@NoInvoice", Convert.ToString(Split_Val));
                            //rpt.SetParameterValue("@NoReceipt", DBNull.Value, "ReceiptSub");
                            rpt.SetParameterValue("@NoReceipt", "", "ReceiptSub");
                            rpt.SetParameterValue("@NoInvoice", Convert.ToString(Split_Val), "ReceiptSub");
                        }
                        //amount = Convert.ToDecimal(inVno_);
                        rpt.SetParameterValue("@NumberToWord", new IDS.Tool.clsNumberToWord().NumToWordInd(Convert.ToDouble(amount), 1));
                        rpt.SetParameterValue("@BranchCode", "DTN", "ReceiptSub");
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                Response.Redirect("~/Error/Error403");
            }
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                //string selValTemp = txtInvoiceNo.SelectedValue;
                Fill_Invoice(cboCustomer.SelectedValue);
                txtInvoiceNo.SelectedValue = string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtInvoiceNo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$txtInvoiceNo"];
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (rpt != null)
            {
                rpt.Close();
                rpt.Dispose();
            }

            if (CRViewer != null)
                CRViewer.Dispose();

            GC.Collect();
        }

        public static bool IsvalidDatetime(string datetime)
        {
            bool valid_ = false;
            try
            {
                System.Convert.ToDateTime(datetime);
                valid_ = true;
            }
            catch
            {
                valid_ = false;
            }
            return valid_;
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

        private void FillCust()
        {
            cboCustomer.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSource();
            cboCustomer.DataTextField = "Text";
            cboCustomer.DataValueField = "Value";
            cboCustomer.DataBind();
        }

        private void Fill_Invoice(string cust)
        {
            txtInvoiceNo.DataSource = IDS.Sales.INVPRINT.GetINVPRINTForDatasource(cust);
            txtInvoiceNo.DataTextField = "Text";
            txtInvoiceNo.DataValueField = "Value";
            txtInvoiceNo.DataBind();
        }

        private static string GetINVPRINTForDatasource(string custCode)
        {
            List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
            list = IDS.Sales.INVPRINT.GetINVPRINTForDatasource(custCode);
            return ToJSON(list);
        }
        static string ToJSON(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        [System.Web.Services.WebMethod]
        public static string GetInvoiceFromCustomer(string cust)
        {
            return GetINVPRINTForDatasource(cust);
        }

        private bool IsExist(string s, string s_containt)
        {
            return s.Contains(s_containt);
        }
    }
}