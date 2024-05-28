using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptSumTBDept : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptSumTrialBalanceDept.rpt"));
                rpt.SetParameterValue("@pMonth", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@type", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkPrintZeroAmt"]) ? 2 : 1);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@Dept", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDept"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);

                FillKarep(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptSumTrialBalanceDept.rpt"));
                rpt.SetParameterValue("@pMonth", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@type", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkPrintZeroAmt"]) ? 2 : 1);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@Dept", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDept"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
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
            this.Page.Title = "Summary Trial Balance By Dept Report";

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

        private void FillKarep(string branchCode)
        {
            cboDept.DataSource = IDS.GeneralTable.Department.GetDepartmentForDataSource(branchCode);
            cboDept.DataTextField = "Value";
            cboDept.DataValueField = "Value";
            cboDept.DataBind();
            cboDept.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboDept"];
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}