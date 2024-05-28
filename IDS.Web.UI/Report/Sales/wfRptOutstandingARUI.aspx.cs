using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptOutstandingARUI : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Page.Title = "Summary Description Outstanding AR";
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
                cboDate.Text = DateTime.Today.ToString("MMM yyyy");
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptOutstandingARUI.rpt"));
            var date_ = Request.Params["ctl00$ContentPlaceHolder1$cboDate"];

            int year;
            int month;
            if (!string.IsNullOrEmpty(date_) && IsvalidDatetime(date_))
            {
                DateTime DateTime_ = System.Convert.ToDateTime(date_);
                rpt.SetParameterValue("@PERIOD", DateTime_.ToString("yyyyMM"));
                year = DateTime_.Year;
                month = DateTime_.Month;
            }
            else
            {
                DateTime DateTime_ = DateTime.Today;
                rpt.SetParameterValue("@PERIOD", DateTime_.ToString("yyyyMM"));
                year = DateTime_.Year;
                month = DateTime_.Month;
            }
            DateTime dat_ = new DateTime(year, month, 1);
            rpt.SetParameterValue("CURRENT", dat_.AddMonths(1).AddDays(-1));
            rpt.SetParameterValue("PREVIOUS", dat_.AddDays(-1));
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;

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

    }
}