using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GLReport
{
    public partial class wfRptTransByAcc : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            FillBranch();
            // FillAcc();
            Fillccy();

            if (!IsPostBack)
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransByACC.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@pFacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@pTAcc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
            else
            {
                rpt.Load(Server.MapPath(@"~/Report/GLReport/CR/RptTransByACC.rpt"));
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);

                rpt.SetParameterValue("@pFacc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"]);
                rpt.SetParameterValue("@pTAcc", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]) ? "" : Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"]);
                rpt.SetParameterValue("@pFromDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpFrom"]));
                rpt.SetParameterValue("@pToDate", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]) ? DateTime.Now.Date : Convert.ToDateTime(Request.Params["ctl00$ContentPlaceHolder1$txtDtpTo"]));
                rpt.SetParameterValue("@branchcode", Request.Params["ctl00$ContentPlaceHolder1$cboBranch"]);

                CRViewer.EnableDatabaseLogonPrompt = true;
                CRViewer.ReportSource = rpt;
                CRViewer.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Transaction By Account Report";

            if (!IsPostBack)
            {
                //CRViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                FillAcc(cbocurrency.Text);
            }
            else
            {
                FillAcc(cbocurrency.Text);
                try
                {
                    cboAccFrom.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccFrom"].ToString();
                    cboAccTo.SelectedValue = Request.Params["ctl00$ContentPlaceHolder1$cboAccTo"].ToString();
                }
                catch (Exception msg)
                {
                    System.Diagnostics.Debug.WriteLine(msg.Message);
                }

                // CRViewer.ReportSource = rpt;

                //cboAccFrom.SelectedValue = cboAccFrom.Text;
                //cboAccTo.SelectedValue = cboAccTo.Text;
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

        private void FillAcc(string ccy)
        {
            cboAccFrom.DataSource = cboAccTo.DataSource = IDS.GLTable.ChartOfAccount.GetCOAForDataSource(ccy);
            cboAccFrom.DataTextField = cboAccTo.DataTextField = "Text";
            cboAccFrom.DataValueField = cboAccTo.DataValueField = "Value";
            cboAccFrom.DataBind();
            cboAccTo.DataBind();
        }

        private void Fillccy()
        {
            cbocurrency.DataSource = IDS.GeneralTable.Currency.GetCurrencyForDataSource();
            cbocurrency.DataTextField = "Text";
            cbocurrency.DataValueField = "Value";
            cbocurrency.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string GetAccFromTo(string ccy)
        {
            return ToJSON(IDS.GLTable.ChartOfAccount.GetCOAForDataSource(ccy));
        }

        //private static string GetCCY(string ccy)
        //{
        //    List<System.Web.Mvc.SelectListItem> list = new List<System.Web.Mvc.SelectListItem>();
        //    list = IDS.GLTable.ChartOfAccount.GetCOAForDatasourceWithCCy(ccy);
        //    return ToJSON(list);
        //}
        static string ToJSON(object obj)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}