using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.FixedAsset
{
    public partial class WFRptFixedAssetList : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cboExpense.AppendDataBoundItems = false;
                cboExpense.Items.Add("Non Expense");
                cboExpense.Items.Add("Expense");

                FillBranch();
                loadAssGroup();
                Refresh();
            }
            else
            {
                FillBranch();
                loadAssGroup();
                Refresh();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Asset List";

          
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

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }
        private void loadAssGroup()
        {
            if (cboExpense.SelectedIndex == 1)
            {
                cboAssetGroup.DataSource = IDS.FixedAsset.FAAssetGroupExpense.FAAssetGroupExpenseForDatasource();
                cboAssetGroup.DataTextField = "Text";
                cboAssetGroup.DataValueField = "Value";
                cboAssetGroup.DataBind();
                cboAssetGroup.Items.Insert(0, new ListItem("", String.Empty));
            }
            else
            {
                cboAssetGroup.DataSource = IDS.FixedAsset.FAGroup.getFAGroupForDatasource();
                cboAssetGroup.DataTextField = "Text";
                cboAssetGroup.DataValueField = "Value";
                cboAssetGroup.DataBind();
                cboAssetGroup.Items.Insert(0, new ListItem("", String.Empty));
            }
        }

        protected void cboExpense_SelectedIndexChanged(object sender, EventArgs e)
        {
            Refresh();
            //ctl00$ContentPlaceHolder1$cboExpense
        }

        private void Refresh()
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
            rpt.Load(Server.MapPath(@"~/Report/FixedAsset/CR/rptAssetList.rpt"));
            rpt.SetParameterValue("@branch", cboBranch.Text);
            if (cboAssetGroup.Text.Trim() != "")
            {
                rpt.SetParameterValue("@ItemGrp", cboAssetGroup.Text);
            }
            else
            {
                rpt.SetParameterValue("@ItemGrp", "");
            }

            rpt.SetParameterValue("@Expense", cboExpense.SelectedIndex);
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
            CRViewer.EnableDrillDown = false;
        }
        protected void btnExe_Click(object sender, EventArgs e)
        {
             Refresh();
            //btnExe
            
        }

        


    }
}