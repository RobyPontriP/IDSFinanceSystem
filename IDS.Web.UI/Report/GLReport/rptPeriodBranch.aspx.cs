using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class rptPeriodBranch : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
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
                    case "0301010700000000": // Report Forex Revaluation
                        this.Page.Title = "Forex Revaluation Report";
                        lblTitle.Text = "Forex Revaluation Report";
                        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptFXReval.rpt"));
                        rpt.SetParameterValue("@pTPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                        rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                        break;

                    case "0301010600000000": // Report SO Maturity Schedule
                        this.Page.Title = "SO Maturity Schedule Report";
                        lblTitle.Text = "SO Maturity Schedule Report";
                        FillCcy();
                        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptSOMaturitySchedule.rpt"));
                        rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                        rpt.SetParameterValue("@pCurr", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
                        rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
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
            //this.Page.Title = "FX Revaluation Report";

            if (!IsPostBack)
            {
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
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

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        private void FillCcy()
        {
            cboCcy.DataSource = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            cboCcy.DataTextField = "Value";
            cboCcy.DataValueField = "Value";
            cboCcy.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}