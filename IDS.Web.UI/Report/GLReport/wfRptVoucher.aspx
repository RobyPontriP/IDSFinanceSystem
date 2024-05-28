<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptVoucher.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptVoucher" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h4>
        <asp:Label ID="lblTitle" runat="server" >Voucher</asp:Label>
    </h4>
    <asp:ScriptManager runat="server" ID="scr1"></asp:ScriptManager>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-sm-5">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row mb-0">
                        <asp:Label ID="Label3" runat="server" Text="From" CssClass="control-label col-sm-2 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDtpFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>

                        <asp:Label ID="Label4" runat="server" Text="To" CssClass="control-label col-sm-2 col-form-label text-right" AssociatedControlID="txtDtpTo" Font-Bold="true"></asp:Label>
                        <div class="col-sm-4">
                            <asp:TextBox ID="txtDtpTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-7">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Source Code" CssClass="control-label col-sm-3 col-form-label text-right" Font-Bold="true"></asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="cboSC" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                        <asp:Label ID="Label5" runat="server" Text="Voucher" CssClass="control-label col-sm-2 col-form-label text-right" Font-Bold="true"></asp:Label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="cboVoucher" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                   
                        <div class="col-sm-3" style="justify-content: flex-end">
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
    <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/Select2/js/select2.full.min.js") %>"></script>

    <script type="text/javascript">
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
        var curFromDate = "01-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();
        var curToDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
            //FillSC();
            $(".select2DDList").select2({
                width: "100%",
                theme:"classic"
            });
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
                    alert('To Date must be greater then From Date');
                    $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                    return;
                } else
                {
                    FillVoucher($("#<%=cboSC.ClientID %>").val(),$("#<%=cboBranch.ClientID %>").val(),$("#<%=txtDtpFrom.ClientID %>").val(),$("#<%=txtDtpTo.ClientID %>").val());
                }

                curFromDate = $("#<%=txtDtpFrom.ClientID %>").val();
            });

            $("#<%=txtDtpFrom.ClientID %>").change(function () {
                if ($("#<%=txtDtpTo.ClientID %>").val() != '') {
                    var dtpTo = new Date($("#<%=txtDtpTo.ClientID %>").val());
                    var dtpFrom = new Date($("#<%=txtDtpFrom.ClientID %>").val());
                    if (dtpTo < dtpFrom) {
                        alert('To Date must be greater then From Date');
                        $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                        return;
                    } else {
                        FillVoucher($("#<%=cboSC.ClientID %>").val(),$("#<%=cboBranch.ClientID %>").val(),$("#<%=txtDtpFrom.ClientID %>").val(),$("#<%=txtDtpTo.ClientID %>").val());
                    }
                }
            });

            $('#<%= cboSC.ClientID %>').change(function () {
                $("#<%=cboVoucher.ClientID %>").empty();
                FillVoucher($("#<%=cboSC.ClientID %>").val(),$("#<%=cboBranch.ClientID %>").val(),$("#<%=txtDtpFrom.ClientID %>").val(),$("#<%=txtDtpTo.ClientID %>").val());
            });

            $("#<%=btnPreview.ClientID %>").click(function () {
            });

            if ($("#<%=cboVoucher.ClientID %>").val() == null || $("#<%=cboVoucher.ClientID %>").val() == '') {
                FillVoucher($("#<%=cboSC.ClientID %>").val(),$("#<%=cboBranch.ClientID %>").val(),$("#<%=txtDtpFrom.ClientID %>").val(),$("#<%=txtDtpTo.ClientID %>").val());
            }
            
        });

        function FillVoucher(sc,branchCode,dtpFrom,dtpTo) {
                <%--var sc = $("#<%=cboSC.ClientID %>").val();
                var branchCode = $("#<%=cboBranch.ClientID %>").val();
                var dtpFrom = $("#<%=txtDtpFrom.ClientID %>").val();
                var dtpTo = $("#<%=txtDtpTo.ClientID %>").val();--%>

                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetVoucherForDataSourceWithAll","Voucher",new { Area="GLTransaction"}) %>",
                    contentType: "application/json; charset=utf-8",
                    //data: "{scode: '" + sc + "',branchCode: '" + branchCode + "',from: '" + dtpFrom + "',to: '" + dtpTo + "'}",
                    data: "{scode: '" + sc + "',branchCode: '" + branchCode + "',from: '" + dtpFrom + "',to: '" + dtpTo + "',withAll: '" + true + "'}",
                    dataType: "json",
                    success: function (respond) {
                        $("#<%=cboVoucher.ClientID %>").empty();
                  <%--      $("#<%=cboVoucher.ClientID %>").append('<option value = "' + "ALL" + '">' + "ALL" + '</option>');--%>
                        $.each(respond, function (i, item) {
                            $("#<%=cboVoucher.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                        });
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
                }
    </script>
</asp:Content>
