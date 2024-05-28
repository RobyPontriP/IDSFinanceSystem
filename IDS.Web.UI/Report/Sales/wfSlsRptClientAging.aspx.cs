using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfSlsRptClientAging : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "List of Bills";
            if (!IsPostBack)
            {
                FillBranch();
                Refresh();
            }
            else
            {
                FillBranch();
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptClientAging.rpt"));
            rpt.SetParameterValue("@BranchCode", cboXBranch.Text);
            //if (IsvalidDatetime(cboDate.Text))
            //{
            //    rpt.SetParameterValue("@vDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"]));
            //}
            //else
            //{
            //    rpt.SetParameterValue("@vDate", DBNull.Value);
            //}
            rpt.SetParameterValue("@vDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboDate"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboDate"]));
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
        //public static DateTime StringToDateTime(string datetime_, string format_)
        //{
        //    return Convert.ToDateTime("3/10/2014  11:59:59 AM").ToString("yyyy-MM-dd hh:mm:ss");
        //   //return DateTime.ParseExact(datetime_, format_, System.Globalization.CultureInfo.InvariantCulture);
        //}

        private void FillBranch()
        {
            //GetCustFoDataSource
            cboXBranch.DataSource = IDS.GeneralTable.Branch.GetBranchForDatasource();
            cboXBranch.DataTextField = "Text";
            cboXBranch.DataValueField = "Value";
            cboXBranch.DataBind();
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