using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptCustomerAging : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillCcy();
                FillDept();
                FillRP();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCustAgingAnalysis.rpt"));
                rpt.SetParameterValue("@PRP", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboRP"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboRP"]);
                rpt.SetParameterValue("@PFdept", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDept"])? "" : Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
                rpt.SetParameterValue("@P1", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP1"])? "0": Request.Params["ctl00$ContentPlaceHolder1$txtP1"]);
                rpt.SetParameterValue("@P2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP2"]) ? "30" : Request.Params["ctl00$ContentPlaceHolder1$txtP2"]);
                rpt.SetParameterValue("@P3", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP3"]) ? "60" : Request.Params["ctl00$ContentPlaceHolder1$txtP3"]);
                rpt.SetParameterValue("@P4", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP4"]) ? "90" : Request.Params["ctl00$ContentPlaceHolder1$txtP4"]);
                rpt.SetParameterValue("@P5", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP5"]) ? "120" : Request.Params["ctl00$ContentPlaceHolder1$txtP5"]);
                rpt.SetParameterValue("@PDATE", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]));
                rpt.SetParameterValue("@PCCY", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCustAgingAnalysis.rpt"));
                rpt.SetParameterValue("@PRP", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboRP"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboRP"]);
                rpt.SetParameterValue("@PCCY", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
                rpt.SetParameterValue("@P1", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP1"]) ? "0" : Request.Params["ctl00$ContentPlaceHolder1$txtP1"]);
                rpt.SetParameterValue("@P2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP2"]) ? "30" : Request.Params["ctl00$ContentPlaceHolder1$txtP2"]);
                rpt.SetParameterValue("@P3", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP3"]) ? "60" : Request.Params["ctl00$ContentPlaceHolder1$txtP3"]);
                rpt.SetParameterValue("@P4", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP4"]) ? "90" : Request.Params["ctl00$ContentPlaceHolder1$txtP4"]);
                rpt.SetParameterValue("@P5", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtP5"]) ? "120" : Request.Params["ctl00$ContentPlaceHolder1$txtP5"]);
                rpt.SetParameterValue("@PDATE", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]));
                rpt.SetParameterValue("@PFdept", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDept"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboDept"]);
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
            this.Page.Title = "Customer Aging Analysis Report";

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

        private void FillCcy()
        {
            cboCcy.DataSource = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            cboCcy.DataTextField = "Value";
            cboCcy.DataValueField = "Value";

            cboCcy.DataBind();
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

        private void FillRP()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "R", Value = "R" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "P", Value = "P" });

            cboRP.DataSource = RP;
            cboRP.DataTextField = "Value";
            cboRP.DataValueField = "Value";
            cboRP.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}