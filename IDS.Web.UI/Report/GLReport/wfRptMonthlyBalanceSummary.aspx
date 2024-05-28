<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptMonthlyBalanceSummary.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptMonthlyBalanceSummary" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
        <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Monthly Balance Summary Report</asp:Label>
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

            <div class="row mb-1">

                 <div class="col-sm-4">
                      <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                 <div class="col-sm-4">
                      <div class="form-group row mb-2">
                        <asp:Label ID="Label9" runat="server" Text="Divide" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboDivide" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                 <div class="col-sm-2">
                     <div class="form-group row mb-2">
                          <div class="form-check pl-1">
                             <asp:CheckBox ID="chkPeriod" runat="server" CssClass="checkbox"></asp:CheckBox>
                            <label class="form-check-label" for="chkPeriod">
                                Enable Period
                            </label>
                        </div>
                     </div>
                 </div>

                 <div class="col-sm-2">
                     <div class="form-group row mb-2">
                           <div class="form-check pl-1">
                            <asp:CheckBox ID="chkAddFilter" runat="server" CssClass="checkbox"></asp:CheckBox>
                            <label class="form-check-label" for="chkAddFilter">
                                Enable Filter
                            </label>
                        </div>
                     </div>
                 </div>
                
            </div>

            <div class="row mb-1">
                 <div class="col-sm-4">
                      <div class="form-group row mb-2">
                        <asp:Label ID="Label4" runat="server" Text="CCY From" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="cboCcyFrom" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                      <div class="form-group row mb-2">
                        <asp:Label ID="Label5" runat="server" Text="CCY To" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="cboCcyTo" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                 </div>
                 <div class="col-sm-4">
                      <div class="form-group row mb-2">
                        <asp:Label ID="Label6" runat="server" Text="ACC From" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboAccFrom" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                     <div class="form-group row mb-2">
                        <asp:Label ID="Label7" runat="server" Text="ACC To" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboAccTo" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                 </div>

                 <div class="col-sm-4">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Period From" CssClass="control-label col-sm-6 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriodFrom" runat="server" CssClass="form-control form-control-sm bg-white" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                     <div class="form-group row mb-2">
                        <asp:Label ID="Label3" runat="server" Text="Period To" CssClass="control-label col-sm-6 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriodTo" runat="server" CssClass="form-control form-control-sm bg-white" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                 </div>

            </div>

            <div class="row mb-1">
                <div class="col-sm-2"></div>
                <div class="col-sm-2"></div>
                <div class="col-sm-2"></div>
                 <div class="col-sm-2"></div>
                  <div class="col-sm-2"></div>
                 <div class="col-sm-2">
                      <div class="form-group row mb-2" style="justify-content: flex-end">
                        <div class="col-sm-12">
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click" />
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
        var curDateFrom = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();
        var curDateTo = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2({
                width:"100%",
                theme:"classic"
            });

            var cboCcyFrom = $('cboCcyFrom');
            var cboCcyTo = $('cboCcyTo');
            var cboAccFrom = $('cboAccFrom');
            var cboAccTo = $('cboAccTo');

            if ($('#<%= chkPeriod.ClientID %>').is(':checked')) {
                $('#<%= txtDtpPeriodFrom.ClientID %>').prop('disabled', false);
                $('#<%= txtDtpPeriodTo.ClientID %>').prop('disabled', false);
            } else {
                $('#<%= txtDtpPeriodFrom.ClientID %>').prop('disabled', true);
                $('#<%= txtDtpPeriodTo.ClientID %>').prop('disabled', true);
            }

            if ($("#<%=txtDtpPeriodFrom.ClientID %>").val() != '') {
                curDateFrom = $("#<%=txtDtpPeriodFrom.ClientID %>").val();
            }

            if ($("#<%=txtDtpPeriodTo.ClientID %>").val() != '') {
                curDateTo = $("#<%=txtDtpPeriodTo.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpPeriodFrom.ClientID %>", {
                //shorthand: true,
                dateFormat: "M/Y",
                altFormat: "F Y",
                defaultDate: [curDateFrom],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

            flatpickr("#<%=txtDtpPeriodTo.ClientID %>", {
                //shorthand: true,
                dateFormat: "M/Y",
                altFormat: "F Y",
                defaultDate: [curDateTo],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
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

            $("#<%=chkAddFilter.ClientID %>").change(function () {
                if ($('#<%= chkAddFilter.ClientID %>').is(':checked')) {
                    $('#<%= cboAccFrom.ClientID %>').prop('disabled', false);
                    $('#<%= cboAccTo.ClientID %>').prop('disabled', false);
                    $('#<%= cboCcyFrom.ClientID %>').prop('disabled', false);
                    $('#<%= cboCcyTo.ClientID %>').prop('disabled', false);

                    FillCcyFrom();
                } else {
                    $('#<%= cboAccFrom.ClientID %>').prop('disabled', true);
                    $('#<%= cboAccTo.ClientID %>').prop('disabled', true);
                    $('#<%= cboCcyFrom.ClientID %>').prop('disabled', true);
                    $('#<%= cboCcyTo.ClientID %>').prop('disabled', true);

                    $("#<%=cboCcyFrom.ClientID %>").empty();
                    $("#<%=cboCcyTo.ClientID %>").empty();
                    $("#<%=cboAccFrom.ClientID %>").empty();
                    $("#<%=cboAccTo.ClientID %>").empty();
                }
            });

            $("#<%=chkPeriod.ClientID %>").change(function () {
                if ($('#<%= chkPeriod.ClientID %>').is(':checked')) {
                    $('#<%= txtDtpPeriodFrom.ClientID %>').prop('disabled', false);
                    $('#<%= txtDtpPeriodTo.ClientID %>').prop('disabled', false);

                } else {
                    $('#<%= txtDtpPeriodFrom.ClientID %>').prop('disabled', true);
                    $('#<%= txtDtpPeriodTo.ClientID %>').prop('disabled', true);
                }
            });

            $('#<%= cboCcyFrom.ClientID %>').change(function () {
                $("#<%=cboAccFrom.ClientID %>").empty();
                FillAccFrom();
            });

            $('#<%= cboCcyTo.ClientID %>').change(function () {
                $("#<%=cboAccTo.ClientID %>").empty();
                FillAccTo();
            });

            function FillCcyFrom() {
                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetCurrencyList","Currency",new { Area="GeneralTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: '{}',
                    dataType: "json",
                    success: function (respond) {
                        $.each(respond, function (i, item) {
                            $("#<%=cboCcyFrom.ClientID %>").append('<option value = "' + item.Value + '">' + item.Value + '</option>');
                            $("#<%=cboCcyTo.ClientID %>").append('<option value = "' + item.Value + '">' + item.Value + '</option>');
                        });
                        FillAccFrom();
                        FillAccTo();
                    },
                    error: function (xhr, test, response) {
                    }
                });
            }

            function FillAccFrom() {
                var ccyFrom = $("#<%=cboCcyFrom.ClientID %>").val();
                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetCOAFromCCY","ChartOfAccount",new { Area="GLTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{currencyCode: '" + ccyFrom + "'}",
                    dataType: "json",
                    success: function (respond) {
                        $.each(respond, function (i, item) {
                            $("#<%=cboAccFrom.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                        });
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
                }

            function FillAccTo() {
                var ccyTo = $("#<%=cboCcyTo.ClientID %>").val();

                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetCOAFromCCY","ChartOfAccount",new { Area="GLTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{currencyCode: '" + ccyTo + "'}",
                    dataType: "json",
                    success: function (respond) {
                        $.each(respond, function (i, item) {
                            $("#<%=cboAccTo.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                        });
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
                }


            $("#<%=btnPreview.ClientID %>").click(function () {
                if ($("#<%=cboAccFrom.ClientID %>").val() == null && $('#<%= chkAddFilter.ClientID %>').is(':checked')) {
                    alert('please select Acc From');
                    return;
                }
            });
        });
    </script>
</asp:Content>



