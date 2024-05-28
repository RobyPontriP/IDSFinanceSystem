using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GeneralTable
{
    public partial class wfRptExchangeRate : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillCcy1();

                rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptExchangeRate.rpt"));
                rpt.SetParameterValue("@FromDate", DateTime.Now.ToString("yyyy")+"-" + DateTime.Now.ToString("MM") + "-" + "01");//string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.ToString("yyyy") + DateTime.Now.ToString("MM") + "01" : Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]);
                rpt.SetParameterValue("@ToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.ToString() : Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]);
                rpt.SetParameterValue("@Ccy1", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcy1"]) ? "IDR" : Request.Params["ctl00$ContentPlaceHolder1$cboCcy1"]);
                rpt.SetParameterValue("@IsLastDayOfMonth", Request.Params["ctl00$ContentPlaceHolder1$chkLastDay"] == null ? 0 : 1);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptExchangeRate.rpt"));
                rpt.SetParameterValue("@FromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"])? DateTime.Now.ToString(): Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]).ToString("yyyy-MM-dd"));
                rpt.SetParameterValue("@ToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.ToString() : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]).ToString("yyyy-MM-dd"));
                rpt.SetParameterValue("@Ccy1", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCcy1"]) ? "ALL" : Request.Params["ctl00$ContentPlaceHolder1$cboCcy1"]);
                rpt.SetParameterValue("@IsLastDayOfMonth", Request.Params["ctl00$ContentPlaceHolder1$chkLastDay"] == null? 0:1);
                rptHelper.SetDefaultFormulaField(rpt);
                //rpt.SetDataSource(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Exchange Rate Report";

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
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

        private void FillCcy1()
        {
            cboCcy1.DataSource = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            cboCcy1.DataTextField = "Value";
            cboCcy1.DataValueField = "Value";
            cboCcy1.DataBind();
        }
    }
}