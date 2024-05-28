using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptCashBankReport : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            FillBranch();
            FillReportOf();

            if (!IsPostBack)
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCSBNK.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@Trans", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]));
                rpt.SetParameterValue("@Trans2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]));

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$rbRptType"]))
                {
                    rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 2 : 0);
                }
                else
                {
                    if (Request.Params["ctl00$ContentPlaceHolder1$rbRptType"] == "rbDetail")
                    {
                        rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 2 : 0);
                    }
                    else
                    {
                        rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 3 : 1);
                    }
                }

                rpt.SetParameterValue("@CHK", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkFilterAcc"]) ? 0 : 1);
                rpt.SetParameterValue("@ACC", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@ACCTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@SPType", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
            else
            {
                if (Request.Params["ctl00$ContentPlaceHolder1$rbRptType"] == "rbDetail")
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCSBNK.rpt"));
                }
                else
                {
                    rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptSummaryCSBNK.rpt"));
                }

                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@Trans", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]));
                rpt.SetParameterValue("@Trans2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]));

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$rbRptType"]))
                {
                    rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 2 : 0);
                }
                else
                {
                    if (Request.Params["ctl00$ContentPlaceHolder1$rbRptType"] == "rbDetail")
                    {
                        rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 2 : 0);
                    }
                    else
                    {
                        rpt.SetParameterValue("@Init", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"] == "BOTH" ? 3 : 1);
                    }
                }

                rpt.SetParameterValue("@CHK", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkFilterAcc"]) ? 0 : 1);
                rpt.SetParameterValue("@ACC", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@ACCTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@SPType", Request.Params["ctl00$ContentPlaceHolder1$cboRptOf"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Cash Bank Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkFilterAcc"]))
                {
                    cboAccFrom.Items.Clear();
                    cboAccTo.Items.Clear();

                    cboAccTo.Enabled = cboAccFrom.Enabled = false;
                }
                else
                {
                    cboAccTo.Enabled = cboAccFrom.Enabled = true;
                    
                    FillAccFrom();
                    FillAccTo();
                }
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
        
        private void FillAccFrom()
        {
            cboAccFrom.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource();
            cboAccFrom.DataTextField = "Text";
            cboAccFrom.DataValueField = "Value";
            cboAccFrom.DataBind();

            cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"];
        }

        private void FillAccTo()
        {
            cboAccTo.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource();
            cboAccTo.DataTextField = "Text";
            cboAccTo.DataValueField = "Value";
            cboAccTo.DataBind();

            cboAccTo.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"];
        }

        private void FillReportOf()
        {
            List<System.Web.Mvc.SelectListItem> RO = new List<System.Web.Mvc.SelectListItem>();
            RO.Add(new System.Web.Mvc.SelectListItem() { Text = "BN", Value = "BN" });
            RO.Add(new System.Web.Mvc.SelectListItem() { Text = "BOTH", Value = "BOTH" });
            RO.Add(new System.Web.Mvc.SelectListItem() { Text = "KS", Value = "KS" });

            cboRptOf.DataSource = RO;
            cboRptOf.DataTextField = "Text";
            cboRptOf.DataValueField = "Value";
            cboRptOf.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}