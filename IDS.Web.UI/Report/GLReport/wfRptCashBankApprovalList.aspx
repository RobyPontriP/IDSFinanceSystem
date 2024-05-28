<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptCashBankApprovalList.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptCashBankApprovalList" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Cash Bank Approval List Report</asp:Label>
    </h4>

    <asp:ScriptManager runat="server" ID="scr1"></asp:ScriptManager>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <div class="row">
                <div class="col-sm-4">
                     <div class="form-group row mb-1">
                         <asp:Label ID="Label3" runat="server" Text="From" CssClass="control-label col-sm-4 col-form-label text-right" Font-Bold="true"></asp:Label>
                         <div class="col-sm-7">
                             <asp:TextBox ID="txtDtpFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                         </div>
                     </div>
                     <div class="form-group row mb-1">
                         <asp:Label ID="Label4" runat="server" Text="To" CssClass="control-label col-sm-4 col-form-label text-right" AssociatedControlID="txtDtpTo" Font-Bold="true"></asp:Label>
                         <div class="col-sm-7">
                             <asp:TextBox ID="txtDtpTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                         </div>
                     </div>
                 </div>

                <div class="col-sm-8">
                    <div class="form-group row mb-2" style=" justify-content: flex-end">
                        <div class="col-sm-2">
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report" 
                                CssClass="form-control form-control-sm btn btn-sm btn-default"
                                OnClick="btnPreview_Click" />
                        </div>
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
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/Index.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/Select2/js/select2.full.min.js") %>"></script>

    <script type="text/javascript">
              const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
        "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
              ];
        var curFromDate = "01-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();
        var curToDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
           if ($("#<%=txtDtpFrom.ClientID %>").val() != '') {
                curFromDate = $("#<%=txtDtpFrom.ClientID %>").val();
            }

            if ($("#<%=txtDtpTo.ClientID %>").val() != '') {
                curToDate = $("#<%=txtDtpTo.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpFrom.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curFromDate],
                static: true,
                disableMobile: true,
            });

            flatpickr("#<%=txtDtpTo.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curToDate],
                static: true,
                disableMobile: true,
            });

            $("#<%=txtDtpTo.ClientID %>").change(function () {
                var dtpTo = new Date($("#<%=txtDtpTo.ClientID %>").val());
                var dtpFrom = new Date($("#<%=txtDtpFrom.ClientID %>").val());
                if (dtpTo < dtpFrom) {
                    alert('From cannot less then To');
                    $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                    return;
                }

                curFromDate = $("#<%=txtDtpFrom.ClientID %>").val();
            });

            $("#<%=txtDtpFrom.ClientID %>").change(function () {
                if ($("#<%=txtDtpTo.ClientID %>").val() != '') {
                    var dtpTo = new Date($("#<%=txtDtpTo.ClientID %>").val());
                    var dtpFrom = new Date($("#<%=txtDtpFrom.ClientID %>").val());
                    if (dtpTo < dtpFrom) {
                        alert('From cannot greather then To');
                        $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                        return;
                    }
                }
            });
        });
    </script>
</asp:Content>



