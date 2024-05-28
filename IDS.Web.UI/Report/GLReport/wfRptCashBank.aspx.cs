using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptCashBank : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCashBank.rpt"));
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);

            rpt.SetParameterValue("@cbNo", Request.QueryString["cbNo"]);
            rpt.SetParameterValue("@NumberToWord", new IDS.Tool.clsNumberToWord().NumToWordInd(Convert.ToDouble(Request.QueryString["amt"]), 1));
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