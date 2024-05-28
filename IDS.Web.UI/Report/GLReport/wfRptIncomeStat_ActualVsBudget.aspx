<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptIncomeStat_ActualVsBudget.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptIncomeStat_ActualVsBudget" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>


<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div class="col-sm-7 col-md-7 col-6 mb-2">
        <h4>
            <p id="txtJudul" contenteditable="true" runat="server">Title</p>
        </h4>
    </div>
    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>
        <div class="card-body">
            <div class="row">

                <div class="col-sm-5 mb-2">
                    <div class="form-group row mb-0">
                        <label for="cboBranchCode" class="col-sm-4 col-form-label col-form-label-sm">Branch Code</label>
                        <div class="col-sm-8 mb-2">
                         <asp:DropDownList AutoPostBack="false" ID="cbobranchcode" runat="server" CssClass="form-control form-control-sm select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row mb-0">
                        <label for="cboXCode" class="col-sm-4 col-form-label col-form-label-sm">Code</label>
                        <div class="col-sm-8 mb-2">
                           <asp:DropDownList AutoPostBack="false" ID="cboXCode" runat="server" CssClass="form-control form-control-sm select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="col-sm-3 mb-2">
                    <div class="form-group row mb-2">
                        <label for="cboXYearFrom" class="col-sm-3 col-form-label col-form-label-sm">From</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="cboXYearFrom" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group row mb-0">
                        <label for="cboXYear" class="col-sm-3 col-form-label col-form-label-sm">To</label>
                        <div class="col-sm-8">
                            <asp:TextBox ID="cboXYear" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>
              
                <div class="col-sm-4 text-right">
                    <asp:Button ID="btnExe" runat="server" CssClass="btn-primary btn btn-sm" Text="Preview" OnClick="btnPreview_Click"></asp:Button>
                </div>

            </div>

        </div>
    </div>
    <!-- /.content -->
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
      const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();
    <script type="text/javascript">
          $(document).ready(function () {
              $(".select2DDList").select2({
                  width:"100%",
                  theme:"classic"
              });
           
            flatpickr("#<%=cboXYearFrom.ClientID %>,#<%=cboXYear.ClientID %>", {
                dateFormat: "M Y",
                disableMobile:true,
                static: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });


           <%-- $("#<%=cboMontYear.ClientID %>").change(function () {
                curDate = $("#<%=cboMontYear.ClientID %>").val();
            });--%>
        });
    </script>
</asp:Content>
