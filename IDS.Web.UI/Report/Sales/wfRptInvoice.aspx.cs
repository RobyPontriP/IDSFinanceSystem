using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IDS.Web.UI.Report.Sales
{
    public partial class wfRptInvoice : System.Web.UI.Page
    {
        CrystalDecisions.CrystalReports.Engine.ReportDocument rpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        IDS.ReportHelper.CrystalHelper rptHelper = new IDS.ReportHelper.CrystalHelper();

        protected void Page_Init(object sender, EventArgs e)
        {
            //FillSignBy();
            //FillOccupation();
            FillBank();
            Tool.clsNumberToWord cls = new Tool.clsNumberToWord();
            IDS.GeneralTable.Branch branch = IDS.GeneralTable.Branch.GetBranch(Session[IDS.Tool.GlobalVariable.SESSION_USER_BRANCH_CODE].ToString());
            IDS.Maintenance.User user = IDS.Maintenance.User.GetUser(branch.InvSignBy);

            IDS.Sales.Invoice invt = IDS.Sales.Invoice.GetSalesInvoiceWithDetail(Request.QueryString["invNo"]);

            IDS.Sales.InvoiceQR invQR = new IDS.Sales.InvoiceQR();

            invQR.InvoiceNo = invt.InvoiceNumber;
            invQR.InvDate = invt.InvoiceDate;
            invQR.Name = invt.Cust.CUSTName;
            invQR.Amount = invt.InvoiceAmount.ToString("0.00");

            IDS.Tool.GenerateQR.GenerateQRCode(Newtonsoft.Json.JsonConvert.SerializeObject(invQR, Newtonsoft.Json.Formatting.Indented), Server.MapPath("~/Images/qrCode.png"));

            if (!IsPostBack)
            {
                IDS.Sales.Invoice inv = IDS.Sales.Invoice.GetTotal(Request.QueryString["invNo"], true);
                

                rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptProjectInvoiceDetail.rpt"));
                rpt.SetParameterValue("@invcode", Request.QueryString["invNo"]);
                //rpt.SetParameterValue("@VATRate", IDS.GeneralTable.Syspar.GetInstance().VAT.ToString());
                rpt.SetParameterValue("NumToWord", "( " + cls.NumToWordInd(inv.GrandTotal, 1) + ")");
                rpt.SetParameterValue("ServerQRPath", Server.MapPath("~/Images/qrCode.png"));
                //rpt.SetParameterValue("NumToWord", "( " + cls.NumToWordInd(120000,1) + ")");
                rpt.SetParameterValue("PrintTax", true);
                rpt.SetParameterValue("PrintTotal", true);
                rpt.SetParameterValue("SignBy", user.UserName + " - " + branch.InvOccupation);
                //rpt.SetParameterValue("SignBy", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboSignBy"]));
                //rpt.SetParameterValue("Occupation", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboOccupation"]));
                //rpt.SetParameterValue("TaxRate", IDS.GeneralTable.Syspar.GetInstance().VAT);
                if (!string.IsNullOrEmpty(cboBank.SelectedValue))
                {
                    IDS.GeneralTable.Bank bank = IDS.GeneralTable.Bank.GetBank(cboBank.SelectedValue);

                    rpt.SetParameterValue("BankInfo", bank.BankName + "\n" + bank.Address + "\n");
                }
                else
                {
                    rpt.SetParameterValue("BankInfo", "");
                }


                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }
            else
            {
                IDS.Sales.Invoice inv = IDS.Sales.Invoice.GetTotal(Request.QueryString["invNo"], string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkShowTax"]) ? false : true);
                rpt.Load(Server.MapPath(@"~/Report/Sales/CR/RptProjectInvoiceDetail.rpt"));
                rpt.SetParameterValue("@invcode", Request.QueryString["invNo"]);
                //rpt.SetParameterValue("@VATRate", IDS.GeneralTable.Syspar.GetInstance().VAT.ToString());
                rpt.SetParameterValue("NumToWord", "( " + cls.NumToWordInd(inv.GrandTotal, 1) + ")");
                //rpt.SetParameterValue("NumToWord", "( " + cls.NumToWordInd(120000, 1) + ")");
                rpt.SetParameterValue("ServerQRPath", Server.MapPath("~/Images/qrCode.png"));
                rpt.SetParameterValue("PrintTax", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkShowTax"]) ? false : true);
                rpt.SetParameterValue("PrintTotal", string.IsNullOrEmpty(Request.Params["ctl00$ContentPlaceHolder1$chkShowGrandTotal"]) ? false : true);
                rpt.SetParameterValue("SignBy", user.UserName + " - " + branch.InvOccupation);
                //rpt.SetParameterValue("SignBy", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboSignBy"], ""));
                //rpt.SetParameterValue("Occupation", IDS.Tool.GeneralHelper.NullToString(Request.Params["ctl00$ContentPlaceHolder1$cboOccupation"]));
                if (!string.IsNullOrEmpty(cboBank.SelectedValue))
                {
                    IDS.GeneralTable.Bank bank = IDS.GeneralTable.Bank.GetBank(Request.Params["ctl00$ContentPlaceHolder1$cboBank"]);

                    rpt.SetParameterValue("BankInfo", bank.BankName + "\n" + bank.Address + "\n" + bank.NoRek + "\n" + bank.Beneficiary + "\n");
                }
                //rpt.SetParameterValue("TaxRate", IDS.GeneralTable.Syspar.GetInstance().VAT);
                rptHelper.SetDefaultFormulaField(rpt);
                rptHelper.SetLogOn(rpt);
            }

            CRViewer.EnableDatabaseLogonPrompt = true;
            CRViewer.ReportSource = rpt;
            CRViewer.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "Invoice Report";

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

        //private void FillSignBy()
        //{
        //    cboSignBy.DataSource = IDS.GeneralTable.Branch.GetSignByForDataSource();
        //    cboSignBy.DataTextField = "Text";
        //    cboSignBy.DataValueField = "Value";
        //    cboSignBy.DataBind();
        //}

        //private void FillOccupation()
        //{
        //    cboOccupation.DataSource = IDS.GeneralTable.Syspar.GetOccupationForDataSource();
        //    cboOccupation.DataTextField = "Text";
        //    cboOccupation.DataValueField = "Value";
        //    cboOccupation.DataBind();
        //}

        private void FillBank()
        {
            cboBank.DataSource = IDS.GeneralTable.Bank.GetBankForDataSource();
            cboBank.DataTextField = "Text";
            cboBank.DataValueField = "Value";
            cboBank.DataBind();
        }
    }
}