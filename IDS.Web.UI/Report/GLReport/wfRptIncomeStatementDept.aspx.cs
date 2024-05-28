using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptIncomeStatementDept : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillDept();
                FillCodeACFGEN();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptIncomeStatementDEpt.rpt"));
                rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rpt.SetParameterValue("@dept", Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptIncomeStatementDEpt.rpt"));
                rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rpt.SetParameterValue("@dept", Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
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
            this.Page.Title = "Income Statement For Rep. Office Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
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

        private void FillDept(string branchCode)
        {
            cboDept.DataSource = IDS.GeneralTable.Department.GetDepartmentForDataSource(branchCode);
            cboDept.DataTextField = "Value";
            cboDept.DataValueField = "Value";
            cboDept.DataBind();
            cboDept.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboDept"];
        }

        private void FillDept()
        {
            cboDept.DataSource = IDS.GeneralTable.Department.GetDepartmentForDataSource();
            cboDept.DataTextField = "Value";
            cboDept.DataValueField = "Value";
            cboDept.DataBind();
            cboDept.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboDept"];
        }

        private void FillCodeACFGEN()
        {
            cboCode.DataSource = IDS.GLTable.ReportGenerator.GetCodeForDatasource().Where(x => x.Value.StartsWith("P")).ToList();
            cboCode.DataTextField = "Text";
            cboCode.DataValueField = "Value";
            cboCode.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}