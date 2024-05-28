<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSlsRptListBelumAdaBuktiPotong.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfSlsRptListBelumAdaBuktiPotong" MasterPageFile="~/Report/Template/ReportGeneralTableWithCode.Master" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <div class="col-sm-6 col-md-6 col-6 mb-2">
        <h4>
            <p id="txtJudul" contenteditable="true" runat="server">Title</p>
        </h4>
    </div>


    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
          <div class="row">
                <div class="col-sm-2" style="display:none" id="father_txtperiod" contenteditable="true" runat="server">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Period" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-6">
                            <asp:TextBox ID="txtDtpPeriod" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-9">
                            <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label3" runat="server" Text="Customer" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboCust" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <div class="col-sm-1">
                           <asp:CheckBox ID="chkBP" runat="server" CssClass="form-check-inline" />
                        </div>
                         <span class="control-label col-sm-7 col-form-label">Sudah ada bukti potong</span>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2" style="justify-content: flex-end">
                        <div class="col-sm-7">
                           <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report" CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click" />
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
        var curDate = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2();
            flatpickr("#<%=txtDtpPeriod.ClientID %>", {
                dateFormat: "Y",
                defaultDate: [curDate],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    noCalendar: false,
                    shorthand: true,
                    dateFormat: "Y",
                    altFormat: "F Y"
                })]
            });
        });

    </script>
</asp:Content>



