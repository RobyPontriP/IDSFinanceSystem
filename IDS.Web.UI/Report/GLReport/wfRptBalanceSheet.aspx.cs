using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptBalanceSheet : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            int type = 0;
            int isRight = 0;
            if (!IsPostBack)
            {
                FillBranch();
                FillCodeACFGEN();

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetMdf.rpt"));
                    type = 1;
                }
                else if (!string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheet.rpt"));
                    type = 0;
                }
                else if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && !string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetMdfIsRight.rpt"));
                    type = 1;
                    isRight = 1;
                }
                else if (!string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && !string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetIsRight.rpt"));
                    isRight = 1;
                    type = 0;
                }

                rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@type", type);
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);

                if (isRight == 1)
                {
                    rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                    rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                    rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                    rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);

                    rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                    rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                    rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                    rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                }
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetMdf.rpt"));
                    type = 1;
                }
                else if (!string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheet.rpt"));
                    type = 0;
                }
                else if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && !string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetMdfIsRight.rpt"));
                    type = 1;
                    isRight = 1;
                }
                else if (!string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkDefault"]) && !string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkLandscape"]))
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptBalanceSheetIsRight.rpt"));
                    type = 0;
                    isRight = 1;
                }

                rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@type", type);
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);

                if (isRight == 1)
                {
                    if (type == 0)
                    {
                        rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                        rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                        rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);
                        rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetLeft.rpt"].Name);

                        rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                        rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                        rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                        rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetRight.rpt"].Name);
                    }
                    else
                    {
                        rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetMdfLeft.rpt"].Name);
                        rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetMdfLeft.rpt"].Name);
                        rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetMdfLeft.rpt"].Name);
                        rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetMdfLeft.rpt"].Name);

                        rpt.SetParameterValue("@pPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"), rpt.Subreports["RptBalanceSheetMdfRight.rpt"].Name);
                        rpt.SetParameterValue("@type", type, rpt.Subreports["RptBalanceSheetMdfRight.rpt"].Name);
                        rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"], rpt.Subreports["RptBalanceSheetMdfRight.rpt"].Name);
                        rpt.SetParameterValue("@pBranch", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], rpt.Subreports["RptBalanceSheetMdfRight.rpt"].Name);
                    }
                }

                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
            CRViewer.EnableDrillDown = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Balance Sheet Report";

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

        private void FillCodeACFGEN()
        {
            cboCode.DataSource = IDS.GLTable.ReportGenerator.GetCodeForDatasource().Where(x => x.Value.StartsWith("B")).ToList();
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