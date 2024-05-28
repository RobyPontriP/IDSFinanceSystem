<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfFARepGroupTable.aspx.cs" Inherits="IDS.Web.UI.Report.FixedAsset.wfFARepGroupTable" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>Assets Group List</h4>
    </div>
    <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="cboExpense" class="col-form-label col-form-label-sm col-sm-3">Branch</label>
                    <div class="col-sm-9">
                        <asp:DropDownList AutoPostBack="false"  ID="cboExpense" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>
         
                <div class="form-group row col-sm-3">
                    <div class="col-sm-9 text-right">
                          <asp:Button ID="btnPreview" runat="server" CssClass="form-control form-control-sm" Width="100px" Text="Print Preview" OnClick="btnPreview_Click"></asp:Button>
                    </div>
                </div>


      </div>

    <div class="CRReportViewer">
        <CR:CrystalReportViewer runat="server" ID="CRViewer" 
            AutoDataBind="true"
            EnableDatabaseLogonPrompt="false"
            DisplayToolbar="True" 
            HasToggleGroupTreeButton="False" 
            BestFitPage="False"
            ToolPanelView="None"
            ReuseParameterValuesOnRefresh="True" 
            HasCrystalLogo="True" 
            Width="100%" />
    </div>
</asp:Content>