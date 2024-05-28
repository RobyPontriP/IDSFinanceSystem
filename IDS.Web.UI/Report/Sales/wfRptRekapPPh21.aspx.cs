using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptRekapPPh21 : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Rekap PPh 21";
            if (!IsPostBack)
            {
                LoadTaxList();
                Refresh();
            }
            else
            {
                LoadTaxList();
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
            var year_ = Request.Params["ctl00$ContentPlaceHolder1$cboMonth"];
            var reportType_ = Request.Params["ctl00$ContentPlaceHolder1$cboReportType"];
            if (!string.IsNullOrEmpty(reportType_) && reportType_.ToString().ToLower().Contains("rptrekappph21"))
            {
                rpt.Load(Server.MapPath(@"~/Report/Sales/CR/" + reportType_ + ".rpt"));
                rpt.SetParameterValue("@YEAR", year_);
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
            }
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

        private void LoadTaxList()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "Summary", Value = "rptRekapPPh21ver1" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "Summary By Customer", Value = "rptRekapPPh21ver2" });
            cboReportType.DataSource = RP;
            cboReportType.DataTextField = "Text";
            cboReportType.DataValueField = "Value";
            cboReportType.DataBind();
        }

    }
}