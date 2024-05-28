<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfSlsRptDetailIncomeStatementwithVAT.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfSlsRptDetailIncomeStatementwithVAT"  MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6 mb-2">
        <h4>Additional Listing AR</h4>
    </div>
     <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;"><b>Filter</b></h3>
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
                        <asp:Label ID="Label7" runat="server" Text="Period" CssClass="control-label col-sm-5 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-7">
                            <asp:TextBox ID="cboPeriod" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label11" runat="server" Text="Customer" CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="cboCust" runat="server" CssClass="form-control form-control-sm select2DDList" Enabled="true"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label3" runat="server" Text="Invoice Rol" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList AutoPostBack="false" ID="cboInvoiceRol" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="form-group row mb-2">
                        <asp:Label ID="Label2" runat="server" Text="Invoice No" CssClass="control-label col-sm-4 col-form-label" Font-Bold="true"></asp:Label>
                        <div class="col-sm-8">
                            <asp:DropDownList AutoPostBack="false" ID="cboInvoiceNO" runat="server" CssClass="form-control form-control-sm mr-4"></asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 form-group text-right">
                    <asp:Button runat="server" ID="btnPreview" Text="Preview" CausesValidation="false" ToolTip="Preview Report" CssClass="col-sm-3 form-control form-control-sm btn btn-sm-4 btn-default" OnClick="btnPreview_Click" />
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
        const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        //var curDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();
        var curDate = monthNames[new Date().getMonth()] + " " + new Date().getFullYear();

        $(document).ready(function () {
            $(".select2DDList").select2();

         if ($("#<%=cboPeriod.ClientID %>").val() != '') {
                curDate = $("#<%=cboPeriod.ClientID %>").val();
            }

            flatpickr("#<%=cboPeriod.ClientID %>", {
                dateFormat: "M Y",
                defaultDate: [curDate],
                static: true,
                disableMobile: true,
                plugins: [new monthSelectPlugin({
                    noCalendar: false,
                    shorthand: true, //bila true january jadi jan
                    dateFormat: "M Y", //defaults to "F Y"
                    altFormat: "F Y"
                })],
            });

            $("#<%=cboPeriod.ClientID %>").change(function () {
                curDate = $("#<%=cboPeriod.ClientID %>").val();
                FillVoucher();
            });

            $("#<%=cboCust.ClientID %>").change(function () {
                $("#<%=cboInvoiceNO.ClientID %>").empty();
                curDate = $("#<%=cboCust.ClientID %>").val();
                FillVoucher();
            });

         
        });
        //FillCboRoll();
        //FillCust();
        //FillBranch();
        function FillVoucher() {
                var period = $("#<%=cboPeriod.ClientID %>").val();
                var cust = $("#<%=cboCust.ClientID %>").val();
            var branch = $("#<%=cboBranch.ClientID %>").val();
             $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetInvoiceNoForDataSource","SalesInvoice",new { Area="Sales"}) %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{Branch: '" + branch + "',Cust: '" + cust + "',period: '" + period + "',withAll:'"+true+"'}",
                    dataType: "json",
                    success: function (respond) {
                        $("#<%=cboInvoiceNO.ClientID %>").empty();
                        <%--$("#<%=cboInvoiceNO.ClientID %>").append('<option value = "">All</option>');--%>
                        if (cust != '') {
                            $.each(respond, function (i, item) {
                            $("#<%=cboInvoiceNO.ClientID %>").append('<option value = "' + item.Value + '">' + item.Text + '</option>');
                        });
                        } else {
                            $("#<%=cboInvoiceNO.ClientID %>").append('<option value = "">All</option>');
                        }
                        
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
           <%-- 
            var data = JSON.stringify({ "Branch": branch, "Cust": cust, "period": period })

                var uri= "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetInvoiceForDataSource","SalesInvoice",new { Area="Sales"}) %>"
                var wb_methode = 'POST';
                var return_ = httpJson(wb_methode, uri, data);
                $.each(return_, function (i, item) {
                    console.log(item);
                });--%>
        }//FillVoucher

        function httpJson(getOrpost, theUrl, param) {
            var xmlHttp = new XMLHttpRequest();
            xmlHttp.open(getOrpost, theUrl, false); // false for synchronous request
            xmlHttp.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
            xmlHttp.send(param);
            return xmlHttp.responseText;
        }

        
    </script>
</asp:Content>


