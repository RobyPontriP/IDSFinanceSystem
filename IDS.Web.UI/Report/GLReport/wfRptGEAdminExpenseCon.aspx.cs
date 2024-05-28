using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptGEAdminExpenseCon : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCodeACFGEN();

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptGenAdminConsolidation.rpt"));
                rpt.SetParameterValue("@period", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptGenAdminConsolidation.rpt"));
                rpt.SetParameterValue("@period", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]).ToString("yyyyMM"));
                rpt.SetParameterValue("@pCode", Request.Params["ctl00$ContentPlaceHolder1$cboCode"]);
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "General & Administrative Consolidation Report";

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
        
        private void FillCodeACFGEN()
        {
            cboCode.DataSource = IDS.GLTable.ReportGenerator.GetCodeForDatasource().Where(x => x.Value.StartsWith("G")).ToList();
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