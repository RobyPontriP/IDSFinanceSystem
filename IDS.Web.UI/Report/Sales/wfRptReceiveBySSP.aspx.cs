using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptReceiveBySSP : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "List of Receive SSP per No Bukti";
            if (!IsPostBack)
            {
                FillCust();
                Refresh();
            }
            else
            {
                FillCust();
                Refresh();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboDate.Text = System.DateTime.Today.ToString("MMM yyyy");
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
            var cust_ = Request.Params["ctl00$ContentPlaceHolder1$cboCust"];
            var date_ = Request.Params["ctl00$ContentPlaceHolder1$cboDate"];

            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReceiveBySSP.rpt"));
            if (string.IsNullOrEmpty(cust_) || cust_ == "ALL")
            {
                rpt.SetParameterValue("@cust", "");
            }
            else
            {
                rpt.SetParameterValue("@cust", cust_);
            }
            if (!string.IsNullOrEmpty(date_) && IsvalidDatetime(date_))
            {
                DateTime d = System.Convert.ToDateTime(date_);
                rpt.SetParameterValue("@Period", d.ToString("yyyyMM"));
                rpt.DataDefinition.FormulaFields["Month"].Text = "\"" + d.ToString("MMMM") + " " + d.ToString("yyyy") + "\"";
            }
            else
            {
                DateTime d = DateTime.Today;
                rpt.SetParameterValue("@Period", d.ToString("yyyyMM"));
                rpt.DataDefinition.FormulaFields["Month"].Text = "\"" + d.ToString("MMMM") + " " + d.ToString("yyyy") + "\"";
            }

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
        private void FillCust()
        {
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";
            cboCust.DataBind();
        }


    }
}