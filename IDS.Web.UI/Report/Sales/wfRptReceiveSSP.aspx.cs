using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptReceiveSSP : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "SSP Collection";
            if (!IsPostBack)
            {
                cboDate.Text = System.DateTime.Today.ToString("yy/MMM/yyyy");
                FillCust();
                FillBranch();
                Refresh();
            }
            else
            {
                FillCust();
                FillBranch();
                Refresh();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboDate.Text = System.DateTime.Today.ToString("yy/MMM/yyyy");
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
            var branch_ = Request.Params["ctl00$ContentPlaceHolder1$cboBranch"];
            var cust_ = Request.Params["ctl00$ContentPlaceHolder1$cboCustomer"];
            var date_ = Request.Params["ctl00$ContentPlaceHolder1$cboDate"];

            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReceiveSSP.rpt"));
            rpt.SetParameterValue("@branch", String.IsNullOrEmpty(branch_) ? Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString() : branch_);
            if (string.IsNullOrEmpty(cust_))
            {
                rpt.SetParameterValue("@cust", "");
            }
            else
            {
                rpt.SetParameterValue("@cust", cust_);
            }

            DateTime d = System.DateTime.Now;// System.Convert.ToDateTime(date_);
            if (string.IsNullOrEmpty(date_))
            {
                rpt.SetParameterValue("@Period", d.ToString("yyyyMM"));
            }
            else
            {
                rpt.SetParameterValue("@Period", System.Convert.ToDateTime(date_).ToString("yyyyMM"));
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
            cboCustomer.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCustomer.DataTextField = "Text";
            cboCustomer.DataValueField = "Value";
            cboCustomer.DataBind();
        }

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
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