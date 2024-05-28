<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSlsRptClientAging.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfSlsRptClientAging" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>List of Bills</h4>
    </div>

   

    <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="cboXBranch" class="col-form-label col-form-label-sm col-sm-3">Branch</label>
                    <div class="col-sm-9">
                        <asp:DropDownList AutoPostBack="false"  ID="cboXBranch" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboDate" class="col-form-label col-form-label-sm col-sm-4">Date</label>
                    <div class="col-sm-8">
                        <asp:Textbox ID="cboDate" runat="server" CssClass="form-control form-control-sm"></asp:Textbox>
                    </div>
                </div>

                <div class="col-sm-6 pull-right">
                       <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm float-right" Width="100px" Text="Print Preview" OnClick="btnPreview_Click"></asp:Button>
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
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {

            if ($("#<%=cboDate.ClientID %>").val() != '') {
                curDate = $("#<%=cboDate.ClientID %>").val();
            }

            flatpickr("#<%=cboDate.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true,
            });
                      
        });
    </script>
</asp:Content>





<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSlsRptClientAging.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfSlsRptClientAging" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>List of Bills</h4>
    </div>

   

    <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="cboXBranch" class="col-form-label col-form-label-sm col-sm-3">Branch</label>
                    <div class="col-sm-9">
                        <asp:DropDownList AutoPostBack="false"  ID="cboXBranch" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboDate" class="col-form-label col-form-label-sm col-sm-4">Customer</label>
                    <div class="col-sm-8">
                        <asp:Textbox ID="cboDate" runat="server" CssClass="form-control form-control-sm"></asp:Textbox>
                    </div>
                </div>

                <div class="col-sm-6 pull-right">
                       <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm float-right" Width="100px" Text="Print Preview" OnClick="btnPreview_Click"></asp:Button>
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
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {

            if ($("#<%=cboDate.ClientID %>").val() != '') {
                curDate = $("#<%=cboDate.ClientID %>").val();
            }

            flatpickr("#<%=cboDate.ClientID %>", {
                dateFormat: "d/M/Y",
                defaultDate: [curDate],
                static: true,
            });
                      
        });
    </script>
</asp:Content>

--%>
