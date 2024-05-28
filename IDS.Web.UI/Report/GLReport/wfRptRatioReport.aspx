<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptRatioReport.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptRatioReport" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
        <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Financial Ratio Report</asp:Label>
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
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label5" runat="server" Text="Formula" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="cboFormula" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-5">
                    <div class="form-group row mb-2" style="justify-content: flex-end">
                        <div class="col-sm-4">
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click"/>
                        </div>
                    </div>
                </div>

            </div>


            <div class="row">
                <div class="col-sm-4">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Period From" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriodFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="col-sm-4">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label3" runat="server" Text="Period To" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriodTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
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
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/Select2/js/select2.full.min.js") %>"></script>
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/Index.js") %>"></script>

    <script type="text/javascript">
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
        var curDateFrom = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();
        var curDateTo = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2({
                width:"100%",theme:"classic"
            });

            if ($("#<%=txtDtpPeriodFrom.ClientID %>").val() != '') {
                curDateFrom = $("#<%=txtDtpPeriodFrom.ClientID %>").val();
            }

            if ($("#<%=txtDtpPeriodTo.ClientID %>").val() != '') {
                curDateTo = $("#<%=txtDtpPeriodTo.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpPeriodFrom.ClientID %>", {
                ////shorthand: true,
                //dateFormat: "M/Y",
                //altFormat: "F Y",
                //defaultDate: [curDateFrom],
                //static: true,
                //plugins: [new monthSelectPlugin({
                //    shorthand: true,
                //    dateFormat: "M Y",
                //    altFormat: "F Y"
                //})],
                
                dateFormat: "Y",
                altFormat: "Y",
                defaultDate: [curDateFrom],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true,
                    dateFormat: "Y",
                    altFormat: "Y"
                })],
            });

            flatpickr("#<%=txtDtpPeriodTo.ClientID %>", {
                ////shorthand: true,
                //dateFormat: "M/Y",
                //altFormat: "F Y",
                //defaultDate: [curDateTo],
                //static: true,
                //plugins: [new monthSelectPlugin({
                //    shorthand: true,
                //    dateFormat: "M Y",
                //    altFormat: "F Y"
                //})],
                dateFormat: "Y",
                altFormat: "Y",
                defaultDate: [curDateTo],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true,
                    dateFormat: "Y",
                    altFormat: "Y"
                })],
            });

            $("#<%=txtDtpPeriodFrom.ClientID %>").change(function () {
                var dtpTo = new Date($("#<%=txtDtpPeriodTo.ClientID %>").val());
                var dtpFrom = new Date($("#<%=txtDtpPeriodFrom.ClientID %>").val());
                if (dtpTo < dtpFrom) {
                    alert('From Period cannot less then To Period');
                    $("#<%=txtDtpPeriodFrom.ClientID %>").val($("#<%=txtDtpPeriodTo.ClientID %>").val());
                    return;
                }

                curDateFrom = $("#<%=txtDtpPeriodTo.ClientID %>").val();
            });

            $("#<%=txtDtpPeriodTo.ClientID %>").change(function () {
                var dtpTo = new Date($("#<%=txtDtpPeriodTo.ClientID %>").val());
                var dtpFrom = new Date($("#<%=txtDtpPeriodFrom.ClientID %>").val());
                if (dtpTo < dtpFrom) {
                    alert('From Period cannot less then To Period');
                    $("#<%=txtDtpPeriodTo.ClientID %>").val($("#<%=txtDtpPeriodFrom.ClientID %>").val());
                    return;
                }

                curDateTo = $("#<%=txtDtpPeriodTo.ClientID %>").val();
            });


            $("#<%=btnPreview.ClientID %>").click(function () {
            });
        });
    </script>
</asp:Content>



