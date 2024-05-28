using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptVoucher : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            FillBranch();
            FillSC();
            //FillVoucher(Request.Params["ctl00$ContentPlaceHolder1$cboSC"], Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]), string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
            System.Drawing.Printing.PrintDocument PrintDoc = new System.Drawing.Printing.PrintDocument();
            if (!IsPostBack)
            {
                FillVoucher(Request.Params["ctl00$ContentPlaceHolder1$cboSC"], Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]), string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptGeneralLedger.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
                
                rpt.SetParameterValue("@pFromDate",Convert.ToDateTime(DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.ToString("MM") + "-" + "01"));//string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date :Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pBranchcode", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]) ? Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE] : Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@pSCode", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboSC"]) ? "ALL" : Request.Params["ctl00$ContentPlaceHolder1$cboSC"]);
                rpt.SetParameterValue("@pVoucher", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"]) ? "ALL" : Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
            else
            {
                FillVoucher(Request.Params["ctl00$ContentPlaceHolder1$cboSC"], Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]), string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptGeneralLedger.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptGeneralLedger.rpt"));
                //rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-01" : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]).ToString("yyyy-MM-dd"));
                //rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]).ToString("yyyy-MM-dd"));
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date  : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@pBranchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                rpt.SetParameterValue("@pSCode", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboSC"]) ? "ALL" : Request.Params["ctl00$ContentPlaceHolder1$cboSC"]);
                rpt.SetParameterValue("@pVoucher", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"]) ? "ALL" : Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"]);

                //System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
                //doctoprint.PrinterSettings.PrinterName = PrintDoc.PrinterSettings.PrinterName;//'(ex. "Epson SQ-1170 ESC/P 2")

                //int rawKind1;
                //for (int i = 0; i <= Convert.ToDecimal(doctoprint.PrinterSettings.PaperSizes.Count) - 1; i++)
                //{
                //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName == "Letter")
                //    {
                //        rawKind1 = Convert.ToInt32(doctoprint.PrinterSettings.PaperSizes[i].GetType().GetField("kind", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(doctoprint.PrinterSettings.PaperSizes[i]));
                //        rpt.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind1;
                //        break;
                //    }
                //}

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();

            }
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Voucher Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                cboSC.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboSC"];
                FillVoucher(Request.Params["ctl00$ContentPlaceHolder1$cboSC"], Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]), string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                cboVoucher.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"];
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

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        private void FillSC()
        {
            List<System.Web.Mvc.SelectListItem> list = IDS.GLTable.SourceCode.GetSourceCodeForDataSource();
            list.Insert(0, new System.Web.Mvc.SelectListItem() { Text = "ALL", Value = "ALL" });

            cboSC.DataSource = list;
            cboSC.DataTextField = "Value";
            cboSC.DataValueField = "Value";
            cboSC.SelectedValue = "ALL";
            cboSC.DataBind();
            //cboSC.DataSource = IDS.GLTable.SourceCode.GetSourceCodeForDataSource();
            //cboSC.DataTextField = "Value";
            //cboSC.DataValueField = "Value";
            //cboSC.DataBind();

            //cboSC.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboSC"];
        }

        private void FillVoucher(string scode, string branchcode, DateTime from, DateTime to)
        {
            //cboVoucher.DataSource = IDS.GLTransaction.GLVoucherH.GetVoucherForDataSource(scode, branchcode, from, to);
            cboVoucher.DataSource = IDS.GLTransaction.GLVoucherH.GetVoucherForDataSource(scode, branchcode, from, to,true);
            cboVoucher.DataTextField = "Value";
            cboVoucher.DataValueField = "Value";
            cboVoucher.DataBind();
            //cboVoucher.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboVoucher"];
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

        //[WebMethod]
        //public static List<System.Web.Mvc.SelectListItem> FillSCForJquery()
        //{
        //    return IDS.GeneralTable.Currency.GetCurrencyForDatasource();
        //}

        //[WebMethod]
        //public static List<System.Web.Mvc.SelectListItem> FillVoucherForJquery(string scode, string branchcode, string from, string to)
        //{
        //    return IDS.GLTransaction.GLVoucherH.GetVoucherForDatasource(scode, branchcode,Convert.ToDateTime(from),Convert.ToDateTime(to));
        //}
    }
}