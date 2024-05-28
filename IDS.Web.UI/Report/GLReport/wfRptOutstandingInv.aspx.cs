using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptOutstandingInv : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillCcy();
                FillRP();
                FillCust(IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboRP"],"R"));

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCustOutsInv.rpt"));
                rpt.SetParameterValue("@pRp", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboRP"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboRP"]);
                rpt.SetParameterValue("@pfcust", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCust"]);
                rpt.SetParameterValue("@pAsOfDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]));
                rpt.SetParameterValue("@pCcy", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCustOutsInv.rpt"));
                rpt.SetParameterValue("@pRp", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboRP"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboRP"]);
                rpt.SetParameterValue("@pfcust", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboCust"]);
                rpt.SetParameterValue("@pAsOfDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]));
                rpt.SetParameterValue("@pCcy", Request.Params["ctl00$ContentPlaceHolder1$cboCcy"]);
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
            this.Page.Title = "OutStanding Invoices Report";

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

        private void FillCust(string custtype)
        {
            cboCust.DataSource = custtype == "R" ? IDS.GeneralTable.Supplier.GetACFVENDForDataSource() : IDS.GeneralTable.Customer.GetACFCUSTForDataSource();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";

            cboCust.DataBind();
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