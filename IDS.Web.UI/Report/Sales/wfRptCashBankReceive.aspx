<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptCashBankReceive.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptCashBankReceive" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6 mb-2">
        <h4>Cash Bank Received Report</h4>
    </div>
     <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;"><b>Filter</b></h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <div class="row">
              <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="txtFrom" class="col-form-label col-form-label-sm col-sm-3">From</label>
                    <div class="col-sm-9">
                        <asp:Textbox AutoPostBack="false"  ID="txtFrom" runat="server" CssClass="form-control form-control-sm pl-15"></asp:Textbox>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboCustomer" class="col-form-label col-form-label-sm col-sm-4">To</label>
                    <div class="col-sm-8">
                        <asp:Textbox ID="txtTo" runat="server" CssClass="form-control form-control-sm"></asp:Textbox>
                    </div>
                </div>

                <div class="col-sm-6 pull-right">
                    <asp:Button ID="btnEXE" runat="server"  Text="Preview" CssClass="form-control form-control-sm float-right" Width="100px" OnClick="btnPreview_Click"></asp:Button>
                </div>
            </div>

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

<asp:Content runat="server" ContentPlaceHolderID="ScriptSection">
<script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/moment.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/id.min.js") %>"></script>
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/Index.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/Select2/js/select2.full.min.js") %>"></script>

    <script type="text/javascript">
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
           
         if ($("#<%=txtTo.ClientID %>").val() != '') {
                curDate = $("#<%=txtTo.ClientID %>").val();
            }

            flatpickr("#<%=txtTo.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true
            });

           if ($("#<%=txtFrom.ClientID %>").val() != '') {
                curDate = $("#<%=txtFrom.ClientID %>").val();
            }

            flatpickr("#<%=txtFrom.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true
            });
         
            $("#<%=txtFrom.ClientID %>").change(function () {
                if ($("#<%=txtTo.ClientID %>").val() != '') {
                    var dtpTo = new Date($("#<%=txtTo.ClientID %>").val());
                    var dtpFrom = new Date($("#<%=txtFrom.ClientID %>").val());
                    if (dtpTo < dtpFrom) {
                        alert('From Date cannot greather then To Date');
                        $("#<%=txtTo.ClientID %>").val($("#<%=txtFrom.ClientID %>").val());
                        return;
                    }
                }
            });

            $("#<%=txtTo.ClientID %>").change(function () {
                var dtpTo = new Date($("#<%=txtTo.ClientID %>").val());
                var dtpFrom = new Date($("#<%=txtFrom.ClientID %>").val());
                if (dtpTo < dtpFrom) {
                    alert('From Date cannot greather then To Date');
                    $("#<%=txtTo.ClientID %>").val($("#<%=txtFrom.ClientID %>").val());
                    return;
                }

                curFromDate = $("#<%=txtFrom.ClientID %>").val();
            });
        });// DOCUMENT rEADY
        
    </script>
</asp:Content>



