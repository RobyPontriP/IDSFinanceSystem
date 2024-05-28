using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.FixedAsset
{
    public partial class wfFARepGroupTable : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Page.Title = "Assets Group List";
            if (!IsPostBack)
            {
                cboExpense.AppendDataBoundItems = false;
                cboExpense.Items.Add("Non Expense");
                cboExpense.Items.Add("Expense");
                Refresh();
            }
            else
            {
                //FillBranch();
                //loadAssGroup();
                //Refresh();
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

            CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
            rpt.Load(Server.MapPath(@"~/Report/FixedAsset/CR/rptMasterGroup.rpt"));
            //rpt.SetParameterValue("@branch", cboBranch.Text);
            //if (cboAssetGroup.Text.Trim() != "")
            //{
            //    rpt.SetParameterValue("@ItemGrp", cboAssetGroup.Text);
            //}
            //else
            //{
            //    rpt.SetParameterValue("@ItemGrp", DBNull.Value);
            //}

            rpt.SetParameterValue("@Expense", cboExpense.SelectedIndex);
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
            CRViewer.EnableDrillDown = false;
        }
    }
}