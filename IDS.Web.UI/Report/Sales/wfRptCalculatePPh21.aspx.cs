using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptCalculatePPh21 : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Fill_TaxObjectType();
                FillCust();
            }
            Fill_TaxObjectType();
            FillCust();
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
                    case "0305250000000000": // Report Forex Revaluation
                        string judul_ = "Calculate Payable Tax";
                        this.Page.Title = judul_;
                        this.txtJudul.InnerHtml = judul_;
                        //exec sp_ReportPPh21 @PERIODE='2011',@CUST='SDEASY',@PPH='PPH21',@JPENGHASILAN='1',@FROM=1,@TO=12
                        rpt.Load(Server.MapPath(@"~/Report/Sales/CR/rptPPh21.rpt"));
                        var dtFrom_ = Request.Params["ctl00$ContentPlaceHolder1$txtFrom"];
                        var dtTo_ = Request.Params["ctl00$ContentPlaceHolder1$txtTo"];
                        var suply_ = Request.Params["ctl00$ContentPlaceHolder1$cboCustomer"];
                        var objtype_ = Request.Params["ctl00$ContentPlaceHolder1$cboJenisPenghasilan"];
                        int year_ = 0;
                        DateTime dtFtomConvert;
                        if (!string.IsNullOrEmpty(dtFrom_) && IsvalidDatetime(dtFrom_))
                        {
                            dtFtomConvert = Convert.ToDateTime(dtFrom_);
                            year_ = dtFtomConvert.Year;
                        }
                        else
                        {
                            dtFtomConvert = DateTime.Today;
                            year_ = dtFtomConvert.Year;
                        }
                        year_ = dtFtomConvert.Year;
                        rpt.SetParameterValue("@PERIODE", year_);
                        rpt.SetParameterValue("@PPH", "PPh21");
                        rpt.SetParameterValue("@CUST", suply_);
                        rpt.SetParameterValue("@JPENGHASILAN", objtype_);

                        if (!string.IsNullOrEmpty(dtFrom_) && IsvalidDatetime(dtFrom_))
                        {
                            rpt.SetParameterValue("@FROM", dtFtomConvert.Month);
                        }
                        else
                        {
                            rpt.SetParameterValue("@FROM", System.DateTime.Today.Month);
                        }

                        DateTime dtToConvert;
                        if (!string.IsNullOrEmpty(dtTo_) && IsvalidDatetime(dtTo_))
                        {
                            dtToConvert = Convert.ToDateTime(dtTo_);
                            rpt.SetParameterValue("@TO", dtToConvert.Month);
                        }
                        else
                        {
                            dtToConvert = DateTime.Today;
                            rpt.SetParameterValue("@TO", dtToConvert.Month);
                        }

                        rpt.DataDefinition.FormulaFields["FROMMONTH"].Text = "\"" + dtFtomConvert.Month + "\"";
                        rpt.DataDefinition.FormulaFields["TOMONTH"].Text = "\"" + dtToConvert.Month + "\"";

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
        private void FillCust()
        {
            //cboCustomer.DataSource = IDS.GeneralTable.Customer.GetACFCUSTForDataSource();
            cboCustomer.DataSource = IDS.GeneralTable.Supplier.GetACFVENDForDataSource();
            cboCustomer.DataTextField = "Text";
            cboCustomer.DataValueField = "Value";
            cboCustomer.DataBind();
        }


        private void Fill_TaxObjectType()
        {
            cboJenisPenghasilan.DataSource = IDS.GeneralTable.tblJenisPenghasilan.Get_TaxObjectType();
            cboJenisPenghasilan.DataTextField = "Text";
            cboJenisPenghasilan.DataValueField = "Value";
            cboJenisPenghasilan.DataBind();
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}