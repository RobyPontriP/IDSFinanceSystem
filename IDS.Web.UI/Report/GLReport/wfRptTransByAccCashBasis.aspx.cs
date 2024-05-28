using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptTransByAccCashBasis : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillAcc();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransByAccCashBasis.rpt"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pFacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@pTacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransByAccCashBasis.rpt"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pFacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@pTacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
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
            this.Page.Title = "Transaction Listing By Account Cash Basis Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"];
                cboAccTo.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"];
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

        private void FillAcc()
        {
            //cboAccFrom.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource();
            cboAccFrom.DataSource = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            cboAccFrom.DataTextField = "Text";
            cboAccFrom.DataValueField = "Value";
            cboAccFrom.DataBind();

            //cboAccTo.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource();
            cboAccTo.DataSource = IDS.GLTable.CashBasisAccount.GetCashBasisAccountForDatasource(IDS.GeneralTable.Syspar.GetInstance().BaseCCy);
            cboAccTo.DataTextField = "Text";
            cboAccTo.DataValueField = "Value";
            cboAccTo.DataBind();

            //cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"];
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}