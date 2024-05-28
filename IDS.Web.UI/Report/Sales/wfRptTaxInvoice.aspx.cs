using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptTaxInvoice : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            IDS.Sales.Invoice inv = IDS.Sales.Invoice.GetTotal(Request.QueryString["invNo"]);

            Tool.clsNumberToWord cls = new Tool.clsNumberToWord();
            if (!IsPostBack)
            {
                rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptTaxInvoice.rpt"));
                rpt.SetParameterValue("@invcode", Request.QueryString["invNo"]);
                string nppkp = string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkShowNPPKP"]) ? "0" : "1";
                rpt.DataDefinition.FormulaFields["IsVisibleNPPKP"].Text = "\"" + nppkp + "\"";
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                InvNo.Value = Request.QueryString["invNo"];

                txtTaxNo.Text = inv.PPnNo;
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptTaxInvoice.rpt"));
                rpt.SetParameterValue("@invcode", Request.QueryString["invNo"]);
                string nppkp = string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkShowNPPKP"]) ? "0" : "1";
                rpt.DataDefinition.FormulaFields["IsVisibleNPPKP"].Text = "\"" + nppkp + "\"";
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Tax Invoice Report";

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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

        protected void btnSetTaxNo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTaxNo.Text))
            {

            }

            //MsgUpdTaxNo.Value = "dasdada";
        }
    }
}