using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class rptYear : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDtpPeriod.Text = DateTime.Now.Year.ToString();
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
                    case "0305230400000000": // Report Forex Revaluation
                        string judul_ = "Summary PPh 23";
                        this.Page.Title = judul_;
                        this.txtJudul.InnerHtml = judul_;
                        var year_ = Request.Params["ctl00$ContentPlaceHolder1$txtDtpPeriod"];
                        rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptRekapPPH23.rpt"));
                        if (string.IsNullOrEmpty(year_))
                        {
                            rpt.SetParameterValue("@YEAR", DateTime.Today.ToString("yyyy"));
                        }
                        else
                        {
                            rpt.SetParameterValue("@YEAR", year_);
                            txtDtpPeriod.Text = year_;
                        }
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
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }


    }
}