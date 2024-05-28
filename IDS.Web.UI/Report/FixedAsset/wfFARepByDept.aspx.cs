using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.FixedAsset
{
    public partial class wfFARepByDept : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
       
        protected void Page_Init(object sender, EventArgs e)
        {

            this.Page.Title = "FA Report by Dept";
            if (!IsPostBack)
            {
                //FillYear();
                FillBranch();
                FillDepartMent();
                if (IDS.FixedAsset.FASchedule.IssNoEmpty(cboXDept.Text, cboBranch.Text, cboMontYear.Text, 0))
                {
                    Refresh(true);
                }
                else
                {
                    Refresh(false);
                }
            }
            else
            {
                //FillYear();
                FillBranch();
                FillDepartMent();
                if (IDS.FixedAsset.FASchedule.IssNoEmpty(cboXDept.Text, cboBranch.Text, cboMontYear.Text, 0))
                {
                    Refresh(true);
                }
                else
                {
                    Refresh(false);
                }
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
            //Refresh();
            if (IDS.FixedAsset.FASchedule.IssNoEmpty(cboXDept.Text, cboBranch.Text, cboMontYear.Text, 0))
            {
                Refresh(true);
            }
            else
            {
                Refresh(false);
            }
        }

        private void Refresh(bool NotEmpty)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
            string oYear = cboMontYear.Text;

            string bef_ToYear = "";
            if (string.IsNullOrEmpty(cboMontYear.Text))
            {
                bef_ToYear = System.DateTime.Now.ToString("yyyyMM");
            }
            else
            {
                DateTime datePeriod = System.Convert.ToDateTime(cboMontYear.Text);
                bef_ToYear = datePeriod.ToString("yyyyMM");
            }
            System.DateTime expenddt1 = System.DateTime.ParseExact(bef_ToYear, "yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string To_month = expenddt1.ToString("MM"); //+ "/01/" + expenddt.Year;

            rpt.Load(Server.MapPath(@"~/Report/FixedAsset/CR/rptMasterItemDept.rpt"));
            rpt.DataDefinition.FormulaFields["period"].Text = "\"" + expenddt1.ToString("MMMM") + " " + expenddt1.ToString("yyyy") + "\"";
            rpt.DataDefinition.FormulaFields["Branch"].Text = "\"" + cboBranch.Text + "\"";
            rpt.SetParameterValue("@Locate", "NULL");
            rpt.SetParameterValue("@Dept", cboXDept.Text);
            rpt.SetParameterValue("@init", 0);
            rpt.SetParameterValue("@Expense", 0);
            rpt.SetParameterValue("@Branch",cboBranch.Text);
            if (NotEmpty) {
                rpt.SetParameterValue("@Period", expenddt1.ToString("yyyy"));
            } else
            {
                rpt.SetParameterValue("@Period", DBNull.Value);
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

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        private void FillDepartMent()
        {
            cboXDept.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Department.GetDepartmentForDataSource() : IDS.GeneralTable.Department.GetDepartmentForDataSource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboXDept.DataTextField = "Text";
            cboXDept.DataValueField = "Value";
            cboXDept.DataBind();
            cboXDept.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        //private void FillYear()
        //{
        //    cboMontYear.DataSource = IDS.FixedAsset.FASchedule.GetYearFromFASchedule();// Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Department.GetDepartmentForDataSource() : IDS.GeneralTable.Department.GetDepartmentForDataSource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
        //    cboMontYear.DataTextField = "Text";
        //    cboMontYear.DataValueField = "Value";
        //    cboMontYear.DataBind();
        //}
        
       


    }
}