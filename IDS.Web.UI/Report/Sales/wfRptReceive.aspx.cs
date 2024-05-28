using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptReceive : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Acfcus();
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
                    case "0305150000000000":
                        string judul_ = "LISTING OF PAYMENT PER NO‎. ‎BUKTI ‎- ‎DETAIL‎";//201109-CCMP
                        this.Page.Title = judul_;

                        var period_ = Request.Params["ctl00$ContentPlaceHolder1$cboMontYear"];
                        var supl_ = Request.Params["ctl00$ContentPlaceHolder1$cboSuplier"];

                        rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptReceivever2.rpt"));

                        string period = "";
                        if (string.IsNullOrEmpty(period_))
                        {
                            period = System.DateTime.Now.ToString("yyyyMM");
                            rpt.SetParameterValue("@PERIOD", period);
                            rpt.SetParameterValue("Period", System.DateTime.Now.ToString("MM") + " " + System.DateTime.Now.ToString("yyyy"));
                        }
                        else
                        {
                            period = System.Convert.ToDateTime(period_).ToString("yyyyMM");
                            rpt.SetParameterValue("@PERIOD", period);
                            rpt.SetParameterValue("Period", System.Convert.ToDateTime(period_).ToString("MM") + " " + System.Convert.ToDateTime(period_).ToString("yyyy"));
                        }

                        if (string.IsNullOrEmpty(supl_))
                        {
                            rpt.SetParameterValue("@CUST", "");
                        }
                        else
                        {
                            rpt.SetParameterValue("@CUST", supl_);
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
                cboMontYear.Text = System.DateTime.Now.ToString("MMM yyyy");
                CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
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



        private void Acfcus()
        {
            cboSuplier.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSourceWithAll();
            cboSuplier.DataTextField = "Text";
            cboSuplier.DataValueField = "Value";
            cboSuplier.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}