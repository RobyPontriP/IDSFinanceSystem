using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptCashBankReceive : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Cash Bank Received Report";
            if (!IsPostBack)
            {
                Refresh_();
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
            var datetime_from = Request.Params["ctl00$ContentPlaceHolder1$txtFrom"];
            var datetime_to = Request.Params["ctl00$ContentPlaceHolder1$txtTo"];
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptCashBankReceived.rpt"));
            if (IsValidDateTime(datetime_from))
            {
                rpt.SetParameterValue("@FROMDATE", Convert.ToDateTime(datetime_from));
            }
            else
            {
                rpt.SetParameterValue("@FROMDATE", DBNull.Value);
            }
            if (IsValidDateTime(datetime_to))
            {
                rpt.SetParameterValue("@TODATE", Convert.ToDateTime(datetime_to));
            }
            else
            {
                rpt.SetParameterValue("@TODATE", DBNull.Value);
            }


            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
        }

        private void Refresh_()
        {
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptCashBankReceived.rpt"));
            rpt.SetParameterValue("@FROMDATE", DBNull.Value);
            rpt.SetParameterValue("@TODATE", DBNull.Value);
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
        }

        private static bool IsValidDateTime(string datetime_)
        {
            bool valid_ = false;
            try
            {
                DateTime x = System.Convert.ToDateTime(datetime_);
                valid_ = true;
            }
            catch
            {
                valid_ = false;
            }
            DateTime datePeriod = System.Convert.ToDateTime(datetime_);
            return valid_;
        }


    }
}