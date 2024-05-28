using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptDaftarBuktiPotongPPh23 : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Page.Title = "List of Collected Prepaid PPh23";
            if (!IsPostBack)
            {
                txtPeriod.Text = DateTime.Today.ToString("MMM yyyy");
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptDaftarBuktiPotong.rpt"));
            var period_ = Request.Params["ctl00$ContentPlaceHolder1$txtPeriod"];
            int year;
            int month;
            if (!string.IsNullOrEmpty(period_) && IsvalidDatetime(period_))
            {
                DateTime DateTime_ = System.Convert.ToDateTime(period_);
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
            DateTime date_ = new DateTime(year, month, 1);
            rpt.SetParameterValue("FROM", date_);
            rpt.SetParameterValue("TO", date_.AddMonths(1).AddDays(-1));
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

        public static string DatetimeTOString(string datetime_, string format_)
        {
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return datePeriod.ToString(format_);
        }

        public static DateTime StringToDatetime(string datetime_)
        {
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return datePeriod;
        }


    }
}