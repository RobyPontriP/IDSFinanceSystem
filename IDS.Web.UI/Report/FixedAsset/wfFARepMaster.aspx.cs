using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.FixedAsset
{
    public partial class wfFARepMaster : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        string h = System.DateTime.Now.ToString("yyyyMM");

        protected void Page_Init(object sender, EventArgs e)
        {
          
            this.Page.Title = "Asset List";
            if (!IsPostBack)
            {
                FillBranch();
                Refresh();
            }
            else
            {
                FillBranch();
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
            h = Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$cboMontYear"]).ToString("yyyyMM");
            if (h.Contains("0001"))
            {
                h = System.DateTime.Now.ToString("yyyyMM");
            }
            System.DateTime expenddt1 = System.DateTime.ParseExact(h, "yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
          
            rpt.Load(Server.MapPath(@"~/Report/FixedAsset/CR/rptMasterItem.rpt"));
            rpt.DataDefinition.FormulaFields["period"].Text = "\"" + expenddt1.ToString("MMMM") + " " + expenddt1.ToString("yyyy") + "\"";
            rpt.DataDefinition.FormulaFields["Branch"].Text = "\""+ cboBranch.Text + "\"";
            rpt.SetParameterValue("@Locate", "NULL");
            rpt.SetParameterValue("@Dept", "NULL");
            rpt.SetParameterValue("@init", 0);
            rpt.SetParameterValue("@Expense", 0);
            rpt.SetParameterValue("@Branch", cboBranch.Text);
            rpt.SetParameterValue("@Period", h);
            rptHelper.SetDefaultFormulaField(rpt);
            rptHelper.SetLogOn(rpt);
            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
          
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