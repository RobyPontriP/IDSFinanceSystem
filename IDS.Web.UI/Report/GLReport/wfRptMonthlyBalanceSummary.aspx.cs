using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptMonthlyBalanceSummary : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            FillBranch();
            FillDivide();
            if (!IsPostBack)
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptMonthlyBalanceSummary.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@dtFrom", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@dtTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@acc", DBNull.Value);
                rpt.SetParameterValue("@acc2", DBNull.Value);
                rpt.SetParameterValue("@curr", DBNull.Value);
                rpt.SetParameterValue("@curr2", DBNull.Value);
                rpt.SetParameterValue("@div", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDivide"])? "No": Request.Params["ctl00$ContentPlaceHolder1$cboDivide"]);
                rpt.SetParameterValue("@xx", 0);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptMonthlyBalanceSummary.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
                
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@dtFrom", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@dtTo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@acc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@acc2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@curr", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]);
                rpt.SetParameterValue("@curr2", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]);
                rpt.SetParameterValue("@div", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDivide"]) ? "No" : Request.Params["ctl00$ContentPlaceHolder1$cboDivide"]);

                int init = 0;

                if (Request.Params["ctl00$ContentPlaceHolder1$chkPeriod"] != "on" && Request.Params["ctl00$ContentPlaceHolder1$chkAddFilter"] != "on")
                {
                    init = 0;
                }
                else if (Request.Params["ctl00$ContentPlaceHolder1$chkPeriod"] == "on" && Request.Params["ctl00$ContentPlaceHolder1$chkAddFilter"] == "on")
                {
                    init = 2;
                }
                else if (Request.Params["ctl00$ContentPlaceHolder1$chkPeriod"] == "on" && Request.Params["ctl00$ContentPlaceHolder1$chkAddFilter"] != "on")
                {
                    init = 3;
                }
                rpt.SetParameterValue("@xx", init);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Monthly Balance Summary Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkAddFilter"]))
                {
                    cboCcyFrom.Items.Clear();
                    cboCcyTo.Items.Clear();
                    cboAccFrom.Items.Clear();
                    cboAccTo.Items.Clear();

                    cboAccTo.Enabled = cboAccFrom.Enabled = cboCcyTo.Enabled = cboCcyFrom.Enabled = false;
                }
                else
                {
                    cboAccTo.Enabled = cboAccFrom.Enabled = cboCcyTo.Enabled = cboCcyFrom.Enabled = true;

                    FillCcyy();
                    FillAccFrom(string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]);
                    FillAccToo(string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]);
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

        private void FillCcyy()
        {
            cboCcyFrom.DataSource = cboCcyTo.DataSource = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            cboCcyFrom.DataTextField = cboCcyTo.DataTextField = "Value";
            cboCcyFrom.DataValueField = cboCcyTo.DataValueField = "Value";

            cboCcyFrom.DataBind();
            cboCcyTo.DataBind();

            cboCcyFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"];
            cboCcyTo.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"];
        }

        private void FillAccFrom(string ccy)
        {
            cboAccFrom.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(ccy);
            cboAccFrom.DataTextField = "Text";
            cboAccFrom.DataValueField = "Value";
            cboAccFrom.DataBind();

            cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"];
        }

        private void FillAccToo(string ccy)
        {
            cboAccTo.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDatasource(ccy);
            cboAccTo.DataTextField = "Text";
            cboAccTo.DataValueField = "Value";
            cboAccTo.DataBind();

            cboAccTo.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"];
        }

        private void FillDivide()
        {
            List<System.Web.Mvc.SelectListItem> Divide = new List<System.Web.Mvc.SelectListItem>();
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "In Billion", Value = "B" });
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "In Hundred", Value = "H" });
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "In Million", Value = "M" });
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "In Ten", Value = "Ten" });
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "In Thousand", Value = "Tho" });
            Divide.Add(new System.Web.Mvc.SelectListItem() { Text = "No Divide", Value = "No" });

            cboDivide.DataSource = Divide;
            cboDivide.DataTextField = "Text";
            cboDivide.DataValueField = "Value";
            cboDivide.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}