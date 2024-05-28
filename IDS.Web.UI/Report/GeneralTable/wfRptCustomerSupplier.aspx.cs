using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.GeneralTable
{
    public partial class wfRptCustomerSupplier : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillType();
            }
            else
            {
                
            }

            rpt.Load(Server.MapPath(@"~/Report/GeneralTable/CR/RptCustomerSupplier.rpt"));
            rpt.SetParameterValue("@type", "1");
            //rpt.SetParameterValue("@pCust", DBNull.Value);
            rptHelper.SetDefaultFormulaField(rpt);
            //rpt.SetDataSource(rpt);
            rptHelper.SetLogOn(rpt);

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Customer Report";

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

        private void FillType()
        {
            List<System.Web.Mvc.SelectListItem> lstType = new List<System.Web.Mvc.SelectListItem>();

            lstType.Add(new System.Web.Mvc.SelectListItem() { Text = "Both", Value = "0" });
            lstType.Add(new System.Web.Mvc.SelectListItem(){ Text = "Customer",Value = "1"});
            lstType.Add(new System.Web.Mvc.SelectListItem() { Text = "Supplier", Value = "2" });

            //cboType.DataSource = lstType;
            //cboType.DataTextField = "Text";
            //cboType.DataValueField = "Value";
            //cboType.DataBind();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            CRViewer.ReportSource = rpt;
        }
    }
}