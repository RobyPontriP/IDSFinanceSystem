<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptExchangeRate.aspx.cs" Inherits="IDS.Web.UI.Report.GeneralTable.wfRptExchangeRate" MasterPageFile="~/Report/Template/ReportMaster.Master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">


    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <div class="row">
                <div class="col-sm-2">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="From" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtDtpFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                            <%--<asp:DropDownList ID="cboType" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>--%>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="To" CssClass="control-label col-sm-2 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtDtpTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                            <%--<asp:DropDownList ID="cboType" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>--%>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="form-group row mb-6">
                        <asp:Label ID="Label4" runat="server" Text="Currency" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6 mr-1">
                            <asp:DropDownList ID="cboCcy1" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label3" runat="server" Text="Print Only Last Day Of Month" CssClass="control-label col-sm-8 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-2" style="display: inline-block">
                            <asp:CheckBox ID="chkLastDay" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2" style="justify-content: flex-end">
                        <div class="col-sm-5">
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <%--<div class="row">
                <div class="col-sm-6">
                    <div class="form-group row mb-6">
                        <asp:Label ID="Label2" runat="server" Text="To" CssClass="control-label col-sm-2 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-5">
                            <asp:TextBox ID="txtDtpTo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>--%>
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
            $(".select2DDList").select2();

             if ($("#<%=txtDtpFrom.ClientID %>").val() != '') {
                curFromDate = $("#<%=txtDtpFrom.ClientID %>").val();
            }

            if ($("#<%=txtDtpTo.ClientID %>").val() != '') {
                curToDate = $("#<%=txtDtpTo.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpFrom.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curFromDate],               
                disableMobile:true,
                static: true,
            });

            flatpickr("#<%=txtDtpTo.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curToDate],
                disableMobile: true,
                static: true,
            });

            $("#<%=txtDtpTo.ClientID %>").change(function () {
                var dtpTo = new Date($("#<%=txtDtpTo.ClientID %>").val());
                var dtpFrom = new Date($("#<%=txtDtpFrom.ClientID %>").val());
                if (dtpTo < dtpFrom) {
                    alert('From cannot less then To');
                    $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                    return;
                }
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



