<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptCashBankReport.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptCashBankReport" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Cash Bank Report</asp:Label>
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
                    <div class="form-group row mb-1">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                 </div>
                 <div class="col-sm-4">
                     <div class="form-group row mb-1">
                        <asp:Label ID="Label2" runat="server" Text="Report of" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboRptOf" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                 </div>
                <div class="col-sm-4">
                    <div class="form-group row mb-1 float-right">
                        <div class="col-sm-12">
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mb-1">

                <div class="col-sm-4">
                    <div class="form-group row mb-1">
                        <asp:Label ID="Label7" runat="server" Text="Date From" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtDtpPeriodFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row mb-1">
                        <asp:Label ID="Label12" runat="server" Text="Date To" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtDtpPeriodTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="form-group row mb-1">
                          <asp:Label ID="Label11" runat="server" Text="ACC From" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboAccFrom" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row mb-1">
                           <asp:Label ID="Label3" runat="server" Text="ACC To" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboAccTo" runat="server" CssClass="form-control select2DDList" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                     <div class="form-group row mb-1">
                         <div class="form-check">
                            <asp:CheckBox ID="chkFilterAcc" runat="server" CssClass="form-check-inline" />
                            <label class="form-check-label" for="chkAddFilter">
                               Enable Acc
                            </label>
                        </div>
                     </div>

                     <div class="form-group row mb-1">
                          <div class="form-check">
                               <asp:RadioButton ID="rbSummary" runat="server" CssClass="form-check-inline" GroupName="rbRptType" />
                              <label class="form-check-label" for="rbSummary">
                               Summary
                            </label>
                          </div>
                     </div>

                     <div class="form-group row mb-1">
                          <div class="form-check">
                               <asp:RadioButton ID="rbDetail" runat="server" CssClass="form-check-inline" GroupName="rbRptType" Checked="true" />
                               <label class="form-check-label" for="rbDetail">
                               Detail
                            </label>
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
        var curDateFrom = "01" + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();
        var curDateTo = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2({
                width:"100%", theme:"classic"
            });

            var cboAccFrom = $('cboAccFrom');
            var cboAccTo = $('cboAccTo');

            if ($("#<%=txtDtpPeriodFrom.ClientID %>").val() != '') {
                curDateFrom = $("#<%=txtDtpPeriodFrom.ClientID %>").val();
            }

            if ($("#<%=txtDtpPeriodTo.ClientID %>").val() != '') {
                curDateTo = $("#<%=txtDtpPeriodTo.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpPeriodFrom.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDateFrom],
                disableMobile: "true",
                static: true,
            });

            flatpickr("#<%=txtDtpPeriodTo.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDateTo],
                disableMobile: "true",
                static: true,
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

            $("#<%=chkFilterAcc.ClientID %>").change(function () {
                if ($('#<%= chkFilterAcc.ClientID %>').is(':checked')) {
                    $('#<%= cboAccFrom.ClientID %>').prop('disabled', false);
                    $('#<%= cboAccTo.ClientID %>').prop('disabled', false);

                    FillAccFrom();
                } else {
                    $('#<%= cboAccFrom.ClientID %>').prop('disabled', true);
                    $('#<%= cboAccTo.ClientID %>').prop('disabled', true);

                    $("#<%=cboAccFrom.ClientID %>").empty();
                    $("#<%=cboAccTo.ClientID %>").empty();
                }
            });

            $('#<%= cboRptOf.ClientID %>').change(function () {
                if ($('#<%= chkFilterAcc.ClientID %>').is(':checked')) {
                   <%-- $('#<%= cboAccFrom.ClientID %>').prop('disabled', false);
                    $('#<%= cboAccTo.ClientID %>').prop('disabled', false);--%>

                    FillAccFrom();
                } 
            });

            function FillAccFrom() {
                //var ccyFrom = 'IDR';
                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetSPACCForDataSource","SpecialAccount",new { Area="GLTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{rptOf: '" + $("#<%=cboRptOf.ClientID %>").val() + "'}",
                    dataType: "json",
                    success: function (respond) {
                        $("#<%=cboAccFrom.ClientID %>").empty();
                        $("#<%=cboAccTo.ClientID %>").empty();
                        $.each(respond, function (i, item) {
                            $("#<%=cboAccFrom.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                            $("#<%=cboAccTo.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                        });
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
            }


            $("#<%=btnPreview.ClientID %>").click(function () {
            });
        });
    </script>
</asp:Content>



