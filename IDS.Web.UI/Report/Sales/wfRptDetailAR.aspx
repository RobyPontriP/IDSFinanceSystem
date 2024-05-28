<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptDetailAR.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptDetailAR" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <section class="content">
											 
		  

    <div class="container-fluid">
        <div class="row">
            <section class="col-lg-12 p-0 connectedSortable">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">
                          <b> Detail of Account Receivable</b>
                        </h3>
                    </div>
                    <div class="card-body">
                        <div class="tab-content p-0">
                            <div class="row mb-1">
                                <section class="col-lg-12 connectedSortable">
                                    <div class="card">
                                        <div class="card-header ui-sortable-handle" style="cursor: move;">
                                            <h3 class="card-title">
                                               Filter
                                            </h3>
                                        </div>
                                        <div class="card-body">
                                             <div class="row mb-1">
                                                <div class="form-group row mb-1 col-sm-4">
                                                    <label for="dtxPeriod" class="col-form-label-sm col-sm-4">Period</label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="cboPeriod" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group row mb-1 col-sm-5">
                                                    <label for="dtxPeriod" class="col-form-label-sm col-sm-5">Customer</label>
                                                    <div class="col-sm-7">
                                                         <asp:DropDownList AutoPostBack="false"  ID="cboCust" runat="server" CssClass="form-control form-control-sm select2DDList"></asp:DropDownList>
                                                    </div>
                                                </div>

                                                 <div class="form-group row mb-1 col-sm-3">
                                                     <div class="row">
                                                        <div class="col-12"> 
                                                            <asp:Button ID="btnExe" runat="server" CssClass="btn btn-primary float-right" Text="Print Preview" OnClick="btnPreview_Click"></asp:Button>
                                                        </div>
                                                     </div>
                                                     
                                                 </div>
                                             </div>
                                        </div>
                                    </div>
                                </section>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
				  

   

        </div>
    </div>
</section>
    
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
      
        $(document).ready(function () {

            $(".select2DDList").select2({
                theme: "classic",
                width:"100%"
            });

            flatpickr("#<%=cboPeriod.ClientID %>", {
                dateFormat: "M Y",
                static: true,
                plugins: [new monthSelectPlugin({
                    noCalendar: false,
                    shorthand: true, //bila true january jadi jan
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });
            
        });
    </script>
</asp:Content>












<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptDetailAR.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptDetailAR" MasterPageFile="~/Report/Template/ReportMaster.Master" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>Detail of Account Receivable</h4>
    </div>

    <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="cboPeriod" class="col-form-label col-form-label-sm col-sm-4">Period</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="cboPeriod" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboCust" class="col-form-label col-form-label-sm col-sm-3">Customer</label>
                    <div class="col-sm-8">
                        <asp:DropDownList AutoPostBack="false"  ID="cboCust" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-6 text_right">
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
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {

            if ($("#<%=cboPeriod.ClientID %>").val() != '') {
                curDate = $("#<%=cboPeriod.ClientID %>").val();
            }

            flatpickr("#<%=cboPeriod.ClientID %>", {
                dateFormat: "M Y",
                defaultDate: [curDate],
                static: true,
                plugins: [new monthSelectPlugin({
                    noCalendar: false,
                    shorthand: true, //bila true january jadi jan
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

            $("#<%=cboPeriod.ClientID %>").change(function () {
                curDate = $("#<%=cboPeriod.ClientID %>").val();
            });
            
        });
    </script>
</asp:Content>

--%>
