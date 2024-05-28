using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptCustomerProject : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "CUSTOMER PROJECT REPORT";
            if (!IsPostBack)
            {
                Refresh();
            }
            else
            {
                Refresh();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
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
        }// Page_Unload

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            string oYear = cboMontYear.Text;
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptCustomerProject.rpt"));
            string bef_ToYear = "";
            if (string.IsNullOrEmpty(cboMontYear.Text))
            {
                bef_ToYear = System.DateTime.Now.ToString("yyyyMM");
            }
            else
            {
                DateTime datePeriod = System.Convert.ToDateTime(cboMontYear.Text);
                bef_ToYear = datePeriod.ToString("yyyyMM");
            }
            System.DateTime expenddt1 = System.DateTime.ParseExact(bef_ToYear, "yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string To_month = expenddt1.ToString("yyyyMM"); //+ "/01/" + expenddt.Year;
            rpt.SetParameterValue("@period", To_month);
            rpt.SetParameterValue("@Branch", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
        }

        public static string Left(string param, int length)
        {
            string result = param.Substring(0, length);
            return result;
        }



    }
}