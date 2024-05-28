using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptTransBySC : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillSC();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransBySC.rpt"));
                rpt.SetParameterValue("@pFromDate",Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + "01"));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@branchcode", cboBranch.SelectedValue);
                rpt.SetParameterValue("@pSCode", cboSC.SelectedValue);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransBySC.rpt"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@pSCode", Request.Params["ctl00$ContentPlaceHolder1$cboSC"]);
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
            this.Page.Title = "Transaction By Source Code Report";

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

        private void FillSC()
        {
            List<System.Web.Mvc.SelectListItem> list = IDS.GLTable.SourceCode.GetSourceCodeForDataSource();
            list.Insert(0,new System.Web.Mvc.SelectListItem() { Text = "ALL", Value = "ALL" });
            
            cboSC.DataSource = list;
            cboSC.DataTextField = "Value";
            cboSC.DataValueField = "Value";
            cboSC.SelectedValue = "ALL";
            cboSC.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
        //CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        //IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        FillBranch();
        //        FillSC();

        //        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransBySC.rpt"));
        //        rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01" : Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]);
        //        rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() : Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]);
        //        rpt.SetParameterValue("@branchcode", cboBranch.SelectedValue);
        //        rpt.SetParameterValue("@pSCode", cboSC.SelectedValue);
        //        rptHelper.SetDefaultFormulaField(rpt);
        //        //rpt.SetDataSource(rpt);
        //        rptHelper.SetLogOn(rpt);
        //    }
        //    else
        //    {
        //        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransBySC.rpt"));
        //        rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01" : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]).ToString("yyyy-MM-dd"));
        //        rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]).ToString("yyyy-MM-dd"));
        //        rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
        //        rpt.SetParameterValue("@pSCode", Request.Params["ctl00$ContentPlaceHolder1$cboSC"]);
        //        rptHelper.SetDefaultFormulaField(rpt);
        //        //rpt.SetDataSource(rpt);
        //        rptHelper.SetLogOn(rpt);
        //    }

        //    CRViewer.EnableDatabaseLogonPrompt = true;
        //    CRViewer.ReportSource = rpt;
        //    CRViewer.DataBind();
        //}

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    this.Page.Title = "Transaction By Source Code Report";

        //    if (!IsPostBack)
        //    {
        //        CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
        //    }
        //}

        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    if (rpt != null)
        //    {
        //        rpt.Close();
        //        rpt.Dispose();
        //    }

        //    if (CRViewer != null)
        //        CRViewer.Dispose();

        //    GC.Collect();
        //}

        //private void FillBranch()
        //{
        //    cboBranch.DataSource = IDS.GeneralTable.Branch.GetBranchForDatasource();
        //    cboBranch.DataTextField = "Text";
        //    cboBranch.DataValueField = "Value";
        //    cboBranch.DataBind();
        //    cboBranch.SelectedValue= Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        //}

        //private void FillSC()
        //{
        //    cboSC.DataSource = IDS.GLTable.SourceCode.GetSourceCodeForDataSource();
        //    cboSC.DataTextField = "Value";
        //    cboSC.DataValueField = "Value";
        //    cboSC.DataBind();
        //}

        //protected void btnPreview_Click(object sender, EventArgs e)
        //{
        //    CRViewer.ReportSource = rpt;
        //}
    }
}