using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptARSummary : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "ADDITIONAL LISTING A‎/‎R ‎- ‎SUMMARY‎";
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReportARSummary.rpt"));
            rpt.SetParameterValue("@PERIOD", IsvalidDatetime(cboMonth.Text) ? DatetimeTOString(cboMonth.Text) : System.DateTime.Now.ToString("yyyyMM"));
            rpt.SetParameterValue("Period", Month_Year(cboMonth.Text));
            rpt.DataDefinition.FormulaFields["OpID"].Text = "\"" + IDS.Tool.GlobalVariable.SESSION_USER_ID + "\"";
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
        public static bool IsvalidDatetime(string datetime)
        {
            bool valid_ = false;
            try
            {
                System.Convert.ToDateTime(datetime);
                valid_ = true;
            }
            catch
            {
                valid_ = false;
            }
            return valid_;
        }

        public static string DatetimeTOString(string datetime_)
        {
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return datePeriod.ToString("yyyyMM");
        }

        public static string Month_Year(string datetime_)
        {
            string return_ = "";
            if (string.IsNullOrEmpty(datetime_))
            {
                return_ = System.DateTime.Now.ToString("MM") + " " + System.DateTime.Now.ToString("yyyy");
            }
            else
            {
                DateTime datePeriod = System.Convert.ToDateTime(datetime_);
                return_ = datePeriod.ToString("MM") + " " + datePeriod.ToString("yyyy");
            }
            return return_;
        }
    }
}