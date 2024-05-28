using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfSlsRptDetailIncomeStatement : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Detail of Revenue";
            if (!IsPostBack)
            {
                FillCboRoll();
                FillCust();
                FillBranch();
                cboPeriod.Text = DateTime.Today.ToString("MMM yyyy");
            }
            else
            {
            }

            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptDetailIncomeStatement.rpt"));
            rpt.SetParameterValue("@Branch", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
            if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]))
            {
                rpt.SetParameterValue("@Cust", "");
            }
            else
            {
                rpt.SetParameterValue("@Cust", IDS.Tool.GeneralHelper.StringToDBNull(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]));
            }
            
            rpt.SetParameterValue("@Period", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]) ? DateTime.Now.ToString("yyyyMM") : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]).ToString("yyyyMM"));
            rpt.SetParameterValue("@InvNo", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"])? "" : Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"]);
            rpt.SetParameterValue("@InvRole", IDS.Tool.GeneralHelper.StringToDBNull(Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceRol"]));
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboPeriod.Text = DateTime.Today.ToString("MMM yyyy");
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
            else
            {
                FillInvoiceNo(Request.Params["ctl00$ContentPlaceHolder1$cboBranch"], Request.Params["ctl00$ContentPlaceHolder1$cboCust"], Request.Params["ctl00$ContentPlaceHolder1$cboPeriod"]);
                cboInvoiceNO.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"];
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
            CRViewer.ReportSource = rpt;
        }

        private void FillCboRoll()
        {
            List<System.Web.Mvc.SelectListItem> RP = new List<System.Web.Mvc.SelectListItem>();
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "ALL", Value = "-1" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "SMI", Value = "0" });
            RP.Add(new System.Web.Mvc.SelectListItem() { Text = "CASI", Value = "1" });
            cboInvoiceRol.DataSource = RP;
            cboInvoiceRol.DataTextField = "Text";
            cboInvoiceRol.DataValueField = "Value";
            cboInvoiceRol.DataBind();
        }

        private void FillCust()
        {
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";
            cboCust.DataBind();
        }
        private void FillBranch()
        {

            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        private void FillInvoiceNo(string branch, string cust, string period)
        {
            if (string.IsNullOrEmpty(period))
                period = DateTime.Now.ToString("yyyyMM");
            else
            {
                DateTime datePeriod = Convert.ToDateTime(period);
                period = datePeriod.ToString("yyyyMM");
            }

            if (string.IsNullOrEmpty(cust))
                cust = "All";

            cboInvoiceNO.DataSource = IDS.Sales.Invoice.GetInvoiceNoForDataSource(branch, cust, period,true);
            cboInvoiceNO.DataTextField = "Text";
            cboInvoiceNO.DataValueField = "Value";
            cboInvoiceNO.DataBind();
            cboInvoiceNO.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboInvoiceNO"];
        }
        
    }
}