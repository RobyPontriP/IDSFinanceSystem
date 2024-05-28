using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfSlsRptTandaTerima : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "Tanda Terima";
            if (!IsPostBack)
            {
                FillBranch();
                FillSuplayer();
                Refresh();
            }
            else
            {
                FillBranch();
                FillSuplayer();
                Refresh();
            }
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
            var branch_ = Request.Params["ctl00$ContentPlaceHolder1$cboBranch"];
            var cust_ = Request.Params["ctl00$ContentPlaceHolder1$cboCust"];
            var date_ = Request.Params["ctl00$ContentPlaceHolder1$txtDate"];
            rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptTandaTerima.rpt"));
            IDS.GeneralTable.Customer customer = IDS.GeneralTable.Customer.GetCustomer(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]);
            if (customer != null)
            {
                rpt.DataDefinition.FormulaFields["Kepada"].Text = "\"" + customer.CUSTName + "\"";
                rpt.DataDefinition.FormulaFields["UP"].Text = "\"" + customer.ContactPerson + "\"";
            }
            
            //if (!string.IsNullOrEmpty(cboCust.SelectedItem.Text))
            //{
            //    string[] cust = cboCust.SelectedItem.Text.Split('-');
            //    rpt.DataDefinition.FormulaFields["Kepada"].Text = "\"" + cust[1] + "\"";
            //    rpt.DataDefinition.FormulaFields["UP"].Text = "\"" + cust[1] + "\"";
            //}


            if (!string.IsNullOrEmpty(date_) && IsvalidDatetime(date_))
            {
                DateTime d = System.Convert.ToDateTime(date_);
                rpt.DataDefinition.FormulaFields["Tangal"].Text = "\"" + d.ToString("dd - MMMM - yyyy") + "\"";
            }
            else
            {
                DateTime d = DateTime.Today;
                rpt.DataDefinition.FormulaFields["Tangal"].Text = "\"" + Convert.ToDateTime(d).ToString("dd - MMMM - yyyy") + "\"";
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
        private void FillSuplayer()
        {
            //var suplier = Request.Params["ctl00$ContentPlaceHolder1$cboBranch"];
            //if (string.IsNullOrEmpty(suplier))
            //{
            //    suplier = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
            //}
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSource();
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


    }
}