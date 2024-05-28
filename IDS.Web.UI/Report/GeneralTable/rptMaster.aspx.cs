using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GeneralTable
{
    public partial class wfRptMaster : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string menuCodeEncrypted = Request.QueryString["rpt"];

                if (string.IsNullOrWhiteSpace(menuCodeEncrypted))
                    Response.Redirect("~/Error/Error403");

                string menuCodeDecrypted = IDS.Tool.UrlEncryption.DecryptParam(menuCodeEncrypted);

                int groupAccess = IDS.Web.UI.Models.GroupAccessLevel.GetGroupAccessLevelByMenuCode(Session[Tool.GlobalVariable.SESSION_USER_GROUP_CODE] as string, menuCodeDecrypted);

                if (groupAccess <= 0)
                    Response.Redirect("~/Error/Error403");

                switch (menuCodeDecrypted)
                {
                    case "0301010100000000": // Report Charts Of Account
                        this.Page.Title = "Charts Of Account Report";
                        lblTitle.Text = "Charts Of Account Report";
                        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptChartsOfAcc.rpt"));
                        break;
                    case "0304030000000000": // Report Bank List
                        this.Page.Title = "Bank List";
                        lblTitle.Text = "Bank List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptBank.rpt"));
                        break;
                    case "0304010000000000": // Report Branch List
                        this.Page.Title = "Branch List";
                        lblTitle.Text = "Branch List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptBranch.rpt"));
                        break;
                    case "0304040000000000": // Report Brand List
                        this.Page.Title = "Brand List";
                        lblTitle.Text = "Brand List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptBrand.rpt"));
                        break;
                    case "0304050000000000": // Report City List
                        this.Page.Title = "City List";
                        lblTitle.Text = "City List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptCity.rpt"));
                        break;
                    case "0304020000000000": // Report Currency List
                        this.Page.Title = "Currency List";
                        lblTitle.Text = "Currency List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptCurrency.rpt"));
                        break;
                    case "0304080000000000": // Report Division List
                        this.Page.Title = "Division List";
                        lblTitle.Text = "Division List";
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptDivision.rpt"));
                        break;
                    case "0301010200000000": // Report Cash Basis Account
                        this.Page.Title = "Cash Basis Account List";
                        lblTitle.Text = "Cash Basis Account List";
                        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptCashBasis.rpt"));
                        break;
                    case "0304070000000000": // Report Department List
                        this.Page.Title = "Department List";
                        lblTitle.Text = "Department List";
                        FillBranch();
                        cboBranchASP.Style.Add("display", "true");
                        rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptDepartment.rpt"));
                        rpt.SetParameterValue("@branchcode", cboBranch.SelectedValue);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                Response.Redirect("~/Error/Error403");
            }

            rptHelper.SetDefaultFormulaField(rpt);
            //rpt.SetDataSource(rpt);
            rptHelper.SetLogOn(rpt);

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
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
        }

        private void FillBranch()
        {
            cboBranch.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cboBranch.DataTextField = "Text";
            cboBranch.DataValueField = "Value";
            cboBranch.DataBind();
            cboBranch.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}