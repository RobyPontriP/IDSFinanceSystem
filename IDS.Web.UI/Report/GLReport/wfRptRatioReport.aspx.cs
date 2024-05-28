using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptRatioReport : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillFormula();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptRatio.rpt"));
                rpt.SetParameterValue("@DTFrom", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyy") : Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"].ToString());
                rpt.SetParameterValue("@DTTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyy") : Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"].ToString());
                rpt.SetParameterValue("@RatioCode", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboFormula"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboFormula"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptRatio.rpt"));
                rpt.SetParameterValue("@DTFrom", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyy") : Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"].ToString());
                rpt.SetParameterValue("@DTTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyy") : Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"].ToString());
                rpt.SetParameterValue("@RatioCode", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboFormula"]) ? "R001" : Request.Params["ctl00$ContentPlaceHolder1$cboFormula"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Financial Ratio Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                cboFormula.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboFormula"];
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

        private void FillFormula()
        {
            cboFormula.DataSource = IDS.GLTable.FinancialRatio.GetFinancialRatioForDataSource();
            cboFormula.DataTextField = "Text";
            cboFormula.DataValueField = "Value";
            cboFormula.DataBind();

            //cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"];
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;

            IDS.GLTable.FinancialRatio.UpdateRatioTemp(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"].ToString(), Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"].ToString(), Request.Params["ctl00$ContentPlaceHolder1$cboBranch"].ToString());
        }
    }
}