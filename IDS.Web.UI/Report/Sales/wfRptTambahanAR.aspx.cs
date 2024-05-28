using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptTambahanAR : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Additional AR Lists";
            if (!IsPostBack)
            {
            }
            else
            {
                
            }
            FillCust();
            Refresh();
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReportAR.rpt"));
            rpt.SetParameterValue("@PERIODE", IsvalidDatetime(cboMonth.Text) ? DatetimeTOString(cboMonth.Text) : System.DateTime.Now.ToString("yyyyMM"));
            //rpt.SetParameterValue("@CUST", cboCustomer.Text);
            //rpt.SetParameterValue("@CUST", IDS.Tool.GeneralHelper.StringToDBNull(cboCustomer.SelectedValue));
            rpt.SetParameterValue("@CUST", IDS.Tool.GeneralHelper.NullToString(cboCustomer.SelectedValue,"All"));
            rpt.SetParameterValue("Periode", Month_Year(cboMonth.Text));
            int Year = IsvalidDatetime(cboMonth.Text) ? Get_Year(cboMonth.Text, "year") : DateTime.Now.Year;
            int month = IsvalidDatetime(cboMonth.Text) ? Get_Year(cboMonth.Text, "month") : DateTime.Now.Month;
            DateTime from = new DateTime(Year, month, 1);
            rpt.SetParameterValue("FROM", from);
            rpt.SetParameterValue("TO", from.AddMonths(1).AddDays(-1));
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

        private void FillCust()
        {
            //GetCustFoDataSource
            cboCustomer.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCustomer.DataTextField = "Text";
            cboCustomer.DataValueField = "Value";
            cboCustomer.DataBind();
        }

        private static int Get_Year(string Data_, string Month_or_Year)
        {
            int return_ = 0;
            string bef_FromYear = "";
            if (string.IsNullOrEmpty(Data_))
            {
                bef_FromYear = System.DateTime.Now.ToString("yyyyMM");
            }
            else
            {
                DateTime datePeriod = System.Convert.ToDateTime(Data_);
                bef_FromYear = datePeriod.ToString("yyyyMM");
            }
            System.DateTime expenddt = System.DateTime.ParseExact(bef_FromYear, "yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            switch (Month_or_Year.ToLower())
            {
                case "month":
                    return_ = expenddt.Month;
                    break;
                case "year":
                    return_ = expenddt.Year;
                    break;
            }
            return return_;
        }
    }
}