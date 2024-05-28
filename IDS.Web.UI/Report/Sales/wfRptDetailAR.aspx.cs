using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptDetailAR : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Detail of Account Receivable";
            if (!IsPostBack)
            {
                Refresh();
                FillCust();
            }
            else
            {
                Refresh();
                FillCust();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                cboPeriod.Text = System.DateTime.Now.ToString("MMM yyyy");
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
            System.Drawing.Printing.PrintDocument PrintDoc = new System.Drawing.Printing.PrintDocument();
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReportDetailAR.rpt"));
            rpt.SetParameterValue("@PERIOD", IsvalidDatetime(cboPeriod.Text) ? DatetimeTOString(cboPeriod.Text, "yyyyMM") : DateTime.Now.ToString("yyyyMM"));
            rpt.SetParameterValue("@CUST", cboCust.Text);
            int Year = IsvalidDatetime(cboPeriod.Text) ? StringToDatetime(cboPeriod.Text).Year : DateTime.Now.Year;
            int month = IsvalidDatetime(cboPeriod.Text) ? StringToDatetime(cboPeriod.Text).Month : DateTime.Now.Month;
            DateTime from = new DateTime(Year, month, 1);
            rpt.SetParameterValue("FROM", from);
            rpt.SetParameterValue("TO", from.AddMonths(1).AddDays(-1));

            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;

            System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
            doctoprint.PrinterSettings.PrinterName = PrintDoc.PrinterSettings.PrinterName;//'(ex. "Epson SQ-1170 ESC/P 2")

            int rawKind1;
            for (int i = 0; i <= Convert.ToDecimal(doctoprint.PrinterSettings.PaperSizes.Count) - 1; i++)
            {
                if (doctoprint.PrinterSettings.PaperSizes[i].PaperName == "8.5x11")
                {
                    rawKind1 = Convert.ToInt32(doctoprint.PrinterSettings.PaperSizes[i].GetType().GetField("kind", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(doctoprint.PrinterSettings.PaperSizes[i]));
                    rpt.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind1;
                    break;
                }
            }

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

        private void FillCust()
        {
            //GetCustFoDataSource
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";
            cboCust.DataBind();
        }
    }
}