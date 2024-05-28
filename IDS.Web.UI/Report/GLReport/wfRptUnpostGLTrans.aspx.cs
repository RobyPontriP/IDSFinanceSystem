using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptUnpostGLTrans : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillCcy();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptUnpostedGLTransaction.rpt"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pCurr", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptUnpostedGLTransaction.rpt"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pCurr", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
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
            this.Page.Title = "Unposted G/L transaction Report";

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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

    }
}