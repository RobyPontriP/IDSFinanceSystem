<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptTransaction.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptTransaction"  MasterPageFile="~/Report/Template/ReportMaster.Master"  %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>Invoice Transaction List - Complete</h4>
    </div>

    <div class="row">
            <div class="col-md-12 bg-light">
                <label for="cbofrom">From</label>
                <asp:TextBox ID="cbofrom" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                <label for="cboto">to</label>
                <asp:TextBox ID="cboto" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm float-right" Width="100px" Text="Print Preview" OnClick="btnPreview_Click"></asp:Button>
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

<asp:Content runat="server" ContentPlaceHolderID="ScriptSection">
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/id.min.js") %>"></script>
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/Index.js") %>"></script>

    <script type="text/javascript">
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {

            if ($("#<%=cbofrom.ClientID %>").val() != '') {
                curDate = $("#<%=cbofrom.ClientID %>").val();
            }

            flatpickr("#<%=cbofrom.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true,
            });

            $("#<%=cbofrom.ClientID %>").change(function () {
                curDate = $("#<%=cbofrom.ClientID %>").val();
            });
            //----------------------------------------------------

             if ($("#<%=cboto.ClientID %>").val() != '') {
                curDate = $("#<%=cboto.ClientID %>").val();
            }

            flatpickr("#<%=cboto.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true,
            });

            $("#<%=cboto.ClientID %>").change(function () {
                curDate = $("#<%=cboto.ClientID %>").val();
            });
        });
    </script>
</asp:Content>
