using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfSlsRptListBelumAdaBuktiPotong : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                FillCust();
            }

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
                    case "0305230300000000": // Report Forex Revaluation
                        string judul_ = "Prepaid PPh 23 Report";
                        this.Page.Title = judul_;
                        this.txtJudul.InnerHtml = judul_;
                        rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptBuktiPotong.rpt"));
                        if (string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboCust"]) || Request.Params["ctl00$ContentPlaceHolder1$cboCust"].ToString() == "ALL")
                        {
                            rpt.SetParameterValue("@CustCode", "ALL");
                        }
                        else
                        {
                            rpt.SetParameterValue("@CustCode", Request.Params["ctl00$ContentPlaceHolder1$cboCust"]);
                        }
                        var chck = Request.Params["ctl00$ContentPlaceHolder1$chkBP"];
                        rpt.SetParameterValue("@BUKTIPOTONG", !string.IsNullOrEmpty(chck) ? 1 : 0);
                        rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);
                        rpt.DataDefinition.FormulaFields["vTanggal"].Text = "\"" + DateTime.Today.ToString("dd MMMM yyyy") + "\"";
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

        private void FillCust()
        {
            cboCust.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboCust.DataTextField = "Text";
            cboCust.DataValueField = "Value";
            cboCust.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }

    }
}