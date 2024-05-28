using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptSlsOutstandingByInvoice : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Page.Title = "Outstanding A/R by Invoice";
            if (!IsPostBack)
            {
                
            }
            else
            {
            }
            FillBranch();
            FillCust();
            Refresh();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Today.ToString("dd/MMM/yyyy");
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
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptSlsOutstandingARByInvoice.rpt"));
            var date_ = Request.Params["ctl00$ContentPlaceHolder1$txtDate"];
            var cust_ = Request.Params["ctl00$ContentPlaceHolder1$cboCust"];
            var branch_ = Request.Params["ctl00$ContentPlaceHolder1$cboBranch"];
            if (string.IsNullOrEmpty(branch_))
            {
                branch_ = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            }
            rpt.SetParameterValue("@Branch", branch_);
            if (string.IsNullOrEmpty(cust_))
            {
                //rpt.SetParameterValue("@Cust", DBNull.Value);
                rpt.SetParameterValue("@Cust", "");
            }
            else
            {
                rpt.SetParameterValue("@Cust", cust_);
            }
            if (!string.IsNullOrEmpty(date_) && IsvalidDatetime(date_))
            {
                DateTime d = System.Convert.ToDateTime(date_);
                rpt.SetParameterValue("@vDate", d);
            }
            else
            {
                rpt.SetParameterValue("@vDate", DBNull.Value);
            }
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;

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
        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
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