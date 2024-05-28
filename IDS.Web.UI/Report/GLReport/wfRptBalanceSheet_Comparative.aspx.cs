using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptBalanceSheet_Comparative : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillBranch();
                Code_Afcgen();
            }
            else
            {
                FillBranch();
                //Code_Afcgen();
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
                    case "0301022600000000":
                        string judul_ = "Balance Sheet Comparative";
                        this.Page.Title = judul_;
                        this.txtJudul.InnerHtml = judul_;
                        var code_ = Request.Params["ctl00$ContentPlaceHolder1$cboXCode"];
                        var from_ = Request.Params["ctl00$ContentPlaceHolder1$cboXYearFrom"];
                        var branch_ = Request.Params["ctl00$ContentPlaceHolder1$cbobranchcode"];

                        rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/GLRepBalSComparative.rpt"));

                        // CODE
                        if (string.IsNullOrEmpty(code_))
                        {
                            rpt.SetParameterValue("@Code", "P1");
                        }
                        else
                        {
                            rpt.SetParameterValue("@Code", code_.ToString());
                        }

                        // FROM
                        if (string.IsNullOrEmpty(from_))
                        {
                            rpt.SetParameterValue("@PeriodToProforma", System.DateTime.Now.ToString("yyyyMM"));
                            rpt.DataDefinition.FormulaFields["period"].Text = "\"" + System.DateTime.Today.ToString("MMM yyyy") + "\"";
                        }
                        else
                        {
                            rpt.SetParameterValue("@PeriodToProforma", System.Convert.ToDateTime(from_).ToString("yyyyMM"));
                            rpt.DataDefinition.FormulaFields["period"].Text = "\"" + System.DateTime.Today.ToString("MMM yyyy") + "\"";
                        }

                        //Branch
                        if (string.IsNullOrEmpty(branch_))
                        {
                            rpt.SetParameterValue("@BranchCode", Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
                        }
                        else
                        {
                            rpt.SetParameterValue("@BranchCode", branch_.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
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

                cboXYearFrom.Text = System.DateTime.Now.ToString("MMM yyyy");
            }
            else
            {
                //FillBranch();
                //Code_Afcgen();
                //Branchhhh = Request.Params["ctl00$ContentPlaceHolder1$cbobranchcode"].ToString();
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
            cbobranchcode.DataSource = Convert.ToBoolean(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_HO_STATUS]) == true ? IDS.GeneralTable.Branch.GetBranchForDatasource() : IDS.GeneralTable.Branch.GetBranchForDatasource(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            cbobranchcode.DataTextField = "Text";
            cbobranchcode.DataValueField = "Value";
            cbobranchcode.DataBind();
            cbobranchcode.SelectedValue = Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString();
        }
        private void Code_Afcgen()
        {
            cboXCode.DataSource = IDS.GLTable.RptGen.GetRptGenForDatasource().Where(x => x.Value.Substring(0,1) == "B");
            cboXCode.DataTextField = "Text";
            cboXCode.DataValueField = "Value";
            cboXCode.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {

            CRViewer.ReportSource = rpt;
        }
    }
}