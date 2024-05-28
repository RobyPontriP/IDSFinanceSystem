using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptCashBankApprovalList : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptApprovalCBList.rpt"));
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);

            rpt.SetParameterValue("@FromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
            rpt.SetParameterValue("@ToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();

            if (!IsPostBack)
            {

            }
            else
            {

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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}