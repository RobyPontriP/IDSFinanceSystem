using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfSlsRptDetailIncomeStatementwithVAT : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Additional Listing AR";
            if (!IsPostBack)
            {
            }
            else
            {
                //FillInvNo(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], Request.Params["ctl00$ContentPlaceHolder1$cboCust"],Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]).ToString("yyyyMM"),true);
            }
            FillCboRoll();
            FillCust();
            FillBranch();
            Refresh();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                
            }
            else
            {
                FillInvNo(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], Request.Params["ctl00$ContentPlaceHolder1$cboCust"], Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]).ToString("yyyyMM"), true);
                cboInvoiceNO.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"];
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static void getFeature(string data)
        {
            //System.Diagnostics.Debug.WriteLine(data);
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
        }// Page_Unload

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            Refresh();
        }


        private void Refresh()
        {
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptDetailIncomeStatementwithVAT.rpt"));
            rpt.SetParameterValue("@Branch", IDS.Tool.GeneralHelper.StringToDBNull(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]));
            rpt.SetParameterValue("@Cust", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboCust"],""));
            rpt.SetParameterValue("@InvNo", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"]));//string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"]);
            //rpt.SetParameterValue("@Period", IsvalidDatetime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]) ? DatetimeTOString(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"], "yyyyMM") : DateTime.Now.ToString("yyyyMM"));
            rpt.SetParameterValue("@Period", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]).ToString("yyyyMM"));//IsvalidDatetime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]) ? DatetimeTOString(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"], "yyyyMM") : DateTime.Now.ToString("yyyyMM"));
            //rpt.SetParameterValue("@InvRole", cboInvoiceRol.SelectedIndex == 0 ? -1 : cboInvoiceRol.SelectedIndex);
            rpt.SetParameterValue("@InvRole", IDS.Tool.GeneralHelper.NullToInt(Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceRol"],-1));
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;

        }

        private void Refresh_()
        {
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptDetailIncomeStatementwithVAT.rpt"));
            rpt.SetParameterValue("@Branch", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            rpt.SetParameterValue("@Cust", "ALL");
            rpt.SetParameterValue("@InvNo", "ALL");
            rpt.SetParameterValue("@Period", "201107");
            rpt.SetParameterValue("@InvRole", "-1");
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
        }

        public static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
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

        public static string DatetimeTOString(string datetime_, string format_)
        {
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return datePeriod.ToString(format_);
        }

        public static DateTime StringToDatetime(string datetime_)
        {
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return datePeriod;
        }

        private void FillCboRoll()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "ALL", Value = "-1" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "SMI", Value = "0" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "CASI", Value = "1" });
            cboInvoiceRol.DataSource = RP;
            cboInvoiceRol.DataTextField = "Text";
            cboInvoiceRol.DataValueField = "Value";
            cboInvoiceRol.DataBind();
        }

        private void FillCust()
        {
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";
            cboCust.DataBind();
        }

        private void FillInvNo(string branch,string cust,string period,bool withAll)
        {
            if (string.IsNullOrEmpty(cust))
                cust = "All";

            cboInvoiceNO.DataSource = IDS.Sales.Invoice.GetInvoiceNoForDataSource(branch, cust, period, withAll);
            cboInvoiceNO.DataTextField = "Text";
            cboInvoiceNO.DataValueField = "Value";
            cboInvoiceNO.DataBind();
        }

        private void FillBranch()
        {

            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }
        private static bool IsNotEmpty(string d)
        {
            bool s = false;
            if (!string.IsNullOrEmpty(d))
            {
                s = true;
            }
            return s;
        }
    }
}