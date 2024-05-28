<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptReceive.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptReceive" MasterPageFile="~/Report/Template/ReportMaster.Master"  %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
      <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/plugins/monthSelect/style.css")%>" />
      <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <section class="content">
    <div class="container-fluid">
        <div class="row">
            <section class="col-lg-12 p-0 connectedSortable">
                <div class="card">
                    <div class="card-header">
                        <h3 class="card-title">
                          <b> LISTING OF PAYMENT PER NO‎. ‎BUKTI ‎- ‎DETAIL</b>
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
                                                <div class="col-sm-4">
                                                    <div class="form-group row mb-2">
                                                        <label for="cboMontYear" class="col-form-label col-form-label-sm col-sm-3">Period</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="cboMontYear" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-5">
                                                    <div class="form-group row mb-2">
                                                        <label for="cboSuplier" class="col-form-label col-form-label-sm col-sm-3 text-right">Supplier</label>
                                                        <div class="col-sm-9">
                                                            <asp:DropDownList AutoPostBack="false" ID="cboSuplier" runat="server" CssClass="form-control form-control-sm select2DDList"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                 <div class="col-sm-3">
                                                    <div class="form-group row mb-2">
                                                        <label for="cboSuplier" class="col-form-label col-form-label-sm col-sm-3 text-right"></label>
                                                        <div class="col-sm-9">
                                                            <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm" Text="Preview" OnClick="btnPreview_Click"></asp:Button>
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
                width: "100%",
                theme: "classic"
            });


           
																 
			 

            flatpickr("#<%=cboMontYear.ClientID %>", {
                //shorthand: true,
                dateFormat: "M/Y",
                altFormat: "F Y",
									   
                static: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

          
																 
			   

        });
    </script>
</asp:Content>


<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptReceive.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptReceive" MasterPageFile="~/Report/Template/ReportMaster.Master"  %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-12 col-md-12 col-12">
        <h4>LISTING OF PAYMENT PER NO‎. ‎BUKTI ‎- ‎DETAIL‎</h4>
    </div>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;"><b>Filter</b></h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <div class="row mb-1">

                 <div class="col-sm-4">
                    <div class="form-group row mb-2">
                        <label for="cboMontYear" class="col-form-label col-form-label-sm col-sm-3">Period</label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="cboMontYear" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-6">
                    <div class="form-group row mb-2">
                        <label for="cboSuplier" class="col-form-label col-form-label-sm col-sm-3 text-right">Supplier</label>
                        <div class="col-sm-9">
                            <asp:DropDownList AutoPostBack="false" ID="cboSuplier" runat="server" CssClass="form-control form-control-sm select2DDList"></asp:DropDownList>
                        </div>
                    </div>
                </div>

            </div>

            <div class="row mb-1">
                  <div class="col-sm-9"></div>
                   <div class="col-sm-3">
                    <div class="form-group row mb-2 float-right">
					<div class="col-sm-12">
                        <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm" Text="Preview" OnClick="btnPreview_Click"></asp:Button>
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
     <script type="text/javascript" src="<%=ResolveClientUrl("~/Content/Select2/js/select2.full.min.js") %>"></script>
    <script type="text/javascript">
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
        ];
        var curDate = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {

            $(".select2DDList").select2({
                width: "100%",
                theme: "classic"
            });


             if ($("#<%=cboMontYear.ClientID %>").val() != '') {
                curDate = $("#<%=cboMontYear.ClientID %>").val();
            }

            flatpickr("#<%=cboMontYear.ClientID %>", {
                //shorthand: true,
                dateFormat: "M/Y",
                altFormat: "F Y",
                defaultDate: [curDate],
                static: true,
                plugins: [new monthSelectPlugin({
                    shorthand: true, //defaults to false
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

            $("#<%=cboMontYear.ClientID %>").change(function () {
                curDate = $("#<%=cboMontYear.ClientID %>").val();
            });

        });
    </script>
</asp:Content>--%>
