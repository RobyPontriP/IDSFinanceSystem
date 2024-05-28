<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptDailyCashRegister.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptDailyCashRegister" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
        <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    </asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Daily Cash Register</asp:Label>
    </h4>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
           <!-- Mulai -->
            <div class="row">
                <div class="col-sm-4 mb-2">
                    <div class="form-group row mb-0">
                        <label for="cboBranch" class="col-sm-3 col-form-label col-form-label-sm">Branch</label>
                        <div class="col-sm-7 mb-2">
                         <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
               </div>

                 <div class="col-sm-3 mb-2">
                    <div class="form-group row mb-0">
                        <label for="txtDtpPeriod" class="col-sm-4 col-form-label col-form-label-sm">Period</label>
                        <div class="col-sm-7 mb-2">
                            <asp:TextBox ID="txtDtpPeriod" runat="server" CssClass="form-control form-control-sm col-sm-12 bg-white"></asp:TextBox>
                        </div>
                    </div>
               </div>

                <div class="col-sm-3 mb-2">
                    <div class="form-group row mb-0">
                        <label for="cboCcy" class="col-sm-3 col-form-label col-form-label-sm">Ccy</label>
                        <div class="col-sm-7 mb-2">
                            <asp:DropDownList ID="cboCcy" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
               </div>

                <div class="col-sm-2 mb-2">
                   <asp:Button runat="server" ID="Button1" Text="Preview" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click" />
               </div>

                 

            </div>
           <!-- Selesai -->
          
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

        var curDate = "01-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2({
                width:"100%",
                theme:"classic"
            });
            if ($("#<%=txtDtpPeriod.ClientID %>").val() != '') {
                curDate = $("#<%=txtDtpPeriod.ClientID %>").val();
            }

            flatpickr("#<%=txtDtpPeriod.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true,
                disableMobile: true,
            });

            $("#<%=txtDtpPeriod.ClientID %>").change(function () {
                curDate = $("#<%=txtDtpPeriod.ClientID %>").val();
            });
        });
    </script>
</asp:Content>



