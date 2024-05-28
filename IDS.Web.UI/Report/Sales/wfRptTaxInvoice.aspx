<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptTaxInvoice.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptTaxInvoice" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager runat="server" ID="scr1"></asp:ScriptManager>
    <cc1:MaskedEditExtender ID="MaskEdExNPWP" runat="server" TargetControlID="txtTaxNo" Mask="999.999-99.99999999" MaskType="Number" MessageValidatorTip="true" 
        ClearMaskOnLostFocus="false"/>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <asp:HiddenField ID="InvNo" runat="server" />
            <asp:HiddenField ID="MsgUpdTaxNo" runat="server" />

            <div class="row">
                <div class="col-sm-3">
                    <div class="form-group row mb-0">
                        <div class="col-sm-12">
                            <div class="form-group row mb-2">
                                <asp:Label ID="Label1" runat="server" Text="Tax No : " CssClass="control-label col-sm-3 col-form-label" Font-Bold="true"></asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtTaxNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2">
                    <div class="form-group row mb-0">
                        <div class="col-sm-10">
                            <asp:Button runat="server" ID="btnSetTaxNo" Text="Set Tax Number" CausesValidation="false" ToolTip="Preview Report"
                                CssClass="form-control form-control-sm btn btn-sm btn-default" OnClick="btnSetTaxNo_Click" />
                        </div>
                    </div>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-0">

                        <div class="col-sm-12">
                            <asp:Label ID="Label3" runat="server" Text="Show Customer NPPKP" CssClass="control-label col-sm-12 col-form-label" Font-Bold="true"></asp:Label>
                            <asp:CheckBox ID="chkShowNPPKP" runat="server" CssClass="form-check-inline" />
                        </div>
                    </div>
                </div>

                <div class="col-sm-4">
                    <div class="form-group row mb-0" style="justify-content: flex-end">
                        <div class="col-sm-4">
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

    <script type="text/javascript">
        var MsgResultUpd = '';
        $(document).ready(function () {
            $("#<%=btnSetTaxNo.ClientID %>").click(function () {
                $.ajax({
                    type: "POST",
                    url: "<%=new System.Web.Mvc.UrlHelper(this.Request.RequestContext).Action("RedirectToRptTaxInvoiceUpdate","Invoice") %>",
                    contentType: "application/json; charset=utf-8",
                    data: "{invNo: '" + $("#<%=InvNo.ClientID %>").val() + "', ppnNo: '" + $("#<%=txtTaxNo.ClientID %>").val() + "'}",
                    dataType: "json",
                    success: function (respond) {
                        if (respond != '') {
                            alert(respond);
                            return;
                        }
                        $("#<%=MsgUpdTaxNo.ClientID %>").val(respond);
                    },
                    error: function (xhr, test, response, aas) {
                    }
                });
            });
        });
    </script>
</asp:Content>



