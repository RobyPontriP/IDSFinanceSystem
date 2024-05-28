<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptSumTBDept.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptSumTBDept" MasterPageFile="~/Report/Template/ReportMaster.Master" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label9" runat="server" Text="Dept" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:DropDownList ID="cboDept" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Period" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriod" runat="server" CssClass="form-control form-control-sm col-sm-12"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <asp:Label ID="Label3" runat="server" Text="Print Zero Amount" CssClass="control-label col-sm-8 col-form-label" Font-Bold="true"></asp:Label>
                    <asp:CheckBox ID="chkPrintZeroAmt" runat="server" CssClass="form-check-inline" />
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
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
        var curDate = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {

            if ($("#<%=txtDtpPeriod.ClientID %>").val() != '') {
                curDate = $("#<%=txtDtpPeriod.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpPeriod.ClientID %>", {
                //shorthand: true,
                dateFormat: "M/Y",
                altFormat: "F Y",
                defaultDate: [curDate],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

            $("#<%=txtDtpPeriod.ClientID %>").change(function () {
                curDate = $("#<%=txtDtpPeriod.ClientID %>").val();
            });

            function FillKarep() {
                var branchCode = $("#<%=cboBranch.ClientID %>").val();
                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetDeptForDataSource","Department",new { Area="GeneralTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{branchCode: '" + branchCode + "'}",
                    dataType: "json",
                    success: function (respond) {
                        $.each(respond, function (i, item) {
                            $("#<%=cboDept.ClientID %>").append('<option value = "' + item.Value + '">' + item.Value + '</option>');
                        });
                    },
                    error: function (xhr, test, response,aas) {
                    }
                });
            }

            $('#<%= cboBranch.ClientID %>').change(function () {
                $("#<%=cboDept.ClientID %>").empty();
                FillKarep();
            });
        });
    </script>
</asp:Content>



