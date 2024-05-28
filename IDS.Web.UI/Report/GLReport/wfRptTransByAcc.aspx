<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptTransByAcc.aspx.cs" Inherits="IDS.Web.UI.Report.GLReport.wfRptTransByAcc" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="StyleSection">
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/Select2/css/select2.min.css")%>" />
    <link rel="stylesheet" type="text/css" href="<%=ResolveClientUrl("~/Content/flatpickr/flatpickr.min.css")%>" />
</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <h4>
        <asp:Label ID="lblTitle" runat="server" >Transaction Listing By Account</asp:Label>
    </h4>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>
        <div class="card-body">
             <div class="row mb-1">
                 <div class="col-sm-4">
                     <div class="form-group row mb-1">
                         <asp:Label ID="Label1" runat="server" Text="Branch" CssClass="control-label col-sm-3 col-form-label text-right" Font-Bold="true"></asp:Label>
                         <div class="col-sm-9">
                             <asp:DropDownList ID="cboBranch" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                         </div>
                     </div>

                     <div class="form-group row mb-1">
                         <asp:Label ID="Label6" runat="server" Text="Ccy" CssClass="control-label col-sm-3 col-form-label text-right" Font-Bold="true"></asp:Label>
                         <div class="col-sm-9">
                             <asp:DropDownList ID="cbocurrency" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                         </div>
                     </div>

                 </div>
                 <div class="col-sm-4">
                     <div class="form-group row mb-1">
                         <asp:Label ID="Label3" runat="server" Text="From" CssClass="control-label col-sm-4 col-form-label text-right" Font-Bold="true"></asp:Label>
                         <div class="col-sm-7">
                             <asp:TextBox ID="txtDtpFrom" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                         </div>
                     </div>
                     <div class="form-group row mb-1">
                         <asp:Label ID="Label4" runat="server" Text="To" CssClass="control-label col-sm-4 col-form-label text-right" AssociatedControlID="txtDtpTo" Font-Bold="true"></asp:Label>
                         <div class="col-sm-7">
                             <asp:TextBox ID="txtDtpTo" runat="server" CssClass="form-control form-control-sm bg-white"></asp:TextBox>
                         </div>
                     </div>
                 </div>
                  <div class="col-sm-4">
                      <div class="form-group row mb-1">
                          <asp:Label ID="Label2" runat="server" Text="Acc From" CssClass="control-label col-sm-4 col-form-label text-right" Font-Bold="true"></asp:Label>
                          <div class="col-sm-8">
                              <asp:DropDownList ID="cboAccFrom" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                          </div>
                      </div>
                      <div class="form-group row mb-1">
                          <asp:Label ID="Label5" runat="server" Text="Acc To" CssClass="control-label col-sm-4 col-form-label text-right" Font-Bold="true"></asp:Label>
                          <div class="col-sm-8">
                              <asp:DropDownList ID="cboAccTo" runat="server" CssClass="form-control select2DDList"></asp:DropDownList>
                          </div>
                      </div>
                  </div>
                
             </div>

             <div class="row mb-1">
                  <div class="col-sm-4"></div>
                  <div class="col-sm-4"></div>
                  <div class="col-sm-4">
                       <div class="form-group row mb-1 float-right">
                           <div class="col-sm-12">
                               <asp:Button runat="server" ID="btnPreview" Text="Preview" ToolTip="Preview Report"
                                     CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnPreview_Click"/>
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
  //      const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
  //"Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
  //      ];
        var curFromDate = "01-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();
        var curToDate = new Date().getDate() + "-" + monthNames[new Date().getMonth()] + "-" + new Date().getFullYear();

        $(document).ready(function () {
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
                    alert('From cannot less then To');
                    $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                    return;
                }

                curFromDate = $("#<%=txtDtpFrom.ClientID %>").val();
            });

            $("#<%=txtDtpFrom.ClientID %>").change(function () {
                if ($("#<%=txtDtpTo.ClientID %>").val() != '') {
                    var dtpTo = new Date($("#<%=txtDtpTo.ClientID %>").val());
                    var dtpFrom = new Date($("#<%=txtDtpFrom.ClientID %>").val());
                    if (dtpTo < dtpFrom) {
                        alert('From cannot greather then To');
                        $("#<%=txtDtpTo.ClientID %>").val($("#<%=txtDtpFrom.ClientID %>").val());
                        return;
                    }
                }
            });

             $("#<%=cbocurrency.ClientID %>").change(function () {
                var ccy = $("#<%=cbocurrency.ClientID %>").val();
                  $.ajax({
                    type: "POST",
                      //url: "wfRptTransByAcc.aspx/GetAccFromTo",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("GetCOAFromCCY","ChartOfAccount",new { Area="GLTable"}) %>",
                    contentType: "application/json; charset=utf-8",
                      //data: "{ccy: '" + ccy + "'}",
                    data: "{currencyCode: '" + ccy + "'}",
                    dataType: "json",
                    success: function (d) {
                        $("#<%=cboAccFrom.ClientID %>,#<%=cboAccTo.ClientID %>").empty();
                        //var data = JSON.parse(d.d);
                        $.each(d, function (i, item) {
                            $("#<%=cboAccFrom.ClientID %>,#<%=cboAccTo.ClientID %>").append('<option value = "' + item["Value"] + '">' + item["Text"] + '</option>');
                        });
                       
                    },
                    error: function (xhr, test, response, aas) {
                   }
                });
            });


        });
    </script>
</asp:Content>
