using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptDetBal : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            FillBranch();
            if (!IsPostBack)
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptDetailTrialBalance.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@pFPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pTPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pFAcc", DBNull.Value);
                rpt.SetParameterValue("@pTAcc", DBNull.Value);
                rpt.SetParameterValue("@pFCcy", DBNull.Value);
                rpt.SetParameterValue("@pTCcy", DBNull.Value);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptDetailTrialBalance.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
                
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@pFPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodFrom"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pTPeriod", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriodTo"]).ToString("yyyyMM"));
                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]))
                {
                    //rpt.SetParameterValue("@pFAcc", DBNull.Value);
                    rpt.SetParameterValue("@pFAcc", "");
                }
                else
                {
                    rpt.SetParameterValue("@pFAcc", Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                }

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]))
                {
                    //rpt.SetParameterValue("@pTAcc", DBNull.Value);
                    rpt.SetParameterValue("@pTAcc", "");
                }
                else
                {
                    rpt.SetParameterValue("@pTAcc", Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                }

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]))
                {
                    //rpt.SetParameterValue("@pFCcy", DBNull.Value);
                    rpt.SetParameterValue("@pFCcy", "");
                }
                else
                {
                    rpt.SetParameterValue("@pFCcy", Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]);
                }

                if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]))
                {
                    //rpt.SetParameterValue("@pTCcy", DBNull.Value);
                    rpt.SetParameterValue("@pTCcy", "");
                }
                else
                {
                    rpt.SetParameterValue("@pTCcy", Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]);
                }
                //rpt.SetParameterValue("@pFAcc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                //rpt.SetParameterValue("@pTAcc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                //rpt.SetParameterValue("@pFCcy", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyFrom"]);
                //rpt.SetParameterValue("@pTCcy", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCcyTo"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Detail Trial Balance Report";

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

        [WebMethod]
        public static List<System.Web.Mvc.SelectListItem> FillCcy()
        {
            return IDS.GeneralTable.Currency.GetCurrencyForDataSource();
        }

        [WebMethod]
        public static List<System.Web.Mvc.SelectListItem> FillAccTo(string ccy)
        {
            return IDS.GLTable.ChartOfAccount.GetCOAForDatasource(ccy);
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}