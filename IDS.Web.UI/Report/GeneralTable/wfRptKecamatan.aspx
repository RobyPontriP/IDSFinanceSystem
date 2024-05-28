<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptKecamatan.aspx.cs" Inherits="IDS.Web.UI.Report.GeneralTable.wfRptKecamatan" MasterPageFile="~/Report/Template/ReportMaster.Master" %>

<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="text-right mb-2">
        <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report" />
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
    
