<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wfRptInvoice.aspx.cs" Inherits="IDS.Web.UI.Report.Sales.wfRptInvoice" MasterPageFile="~/Report/Template/ReportMaster.Master" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager runat="server" ID="scr1"></asp:ScriptManager>

    <div class="card">
        <div class="card-header">
            <h3 class="card-title" style="font-size: 12px;">Filter</h3>
            <div class="card-tools">
                <button type="button" class="btn btn-tool" data-card-widget="collapse"><i class="fas fa-minus"></i></button>
            </div>
        </div>

        <div class="card-body">
            <asp:HiddenField ID="invNo" runat="server" />
            <div class="row">

                <div class="col-sm-3">
                    <div class="form-group row mb-2">

                        <div class="col-sm-5">
                            <asp:Label ID="Label3" runat="server" Text="Show Tax" CssClass="control-label col-sm-12 col-form-label" Font-Bold="true"></asp:Label>
                            <asp:CheckBox ID="chkShowTax" runat="server" CssClass="form-check-inline" Checked="true" />
                        </div>

                        <div class="col-sm-7">
                            <asp:Label ID="Label2" runat="server" Text="Show Grand Total" CssClass="control-label col-sm-12 col-form-label" Font-Bold="true"></asp:Label>
                            <asp:CheckBox ID="chkShowGrandTotal" runat="server" CssClass="form-check-inline" Checked="true" />
                        </div>
                    </div>
                </div>

                <%--    <div class="form-group row col-sm-3">
                        <label for="cboSignBy" class="col-form-label col-form-label-sm col-sm-3 mb-3">Sign By</label>
                        <div class="col-sm-8">
                            <asp:DropDownList AutoPostBack="false"  ID="cboSignBy" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                        </div>
                    </div>--%>

                <%--      <div class="form-group row col-sm-3">
                    <label for="cboOccupation" class="col-form-label col-form-label-sm col-sm-3 mb-3">Occupation</label>
                    <div class="col-sm-8">
                        <asp:DropDownList AutoPostBack="false"  ID="cboOccupation" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                    </div>
                </div>--%>

                <div class="form-group row col-sm-2">
                    <label for="cboBank" class="col-form-label col-form-label-sm col-sm-3 mb-3">Bank</label>
                    <div class="col-sm-8">
                        <asp:DropDownList AutoPostBack="false" ID="cboBank" runat="server" CssClass="form-control form-control-sm pl-15"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-4">
                    <%--<input runat="server" type="file" class="form-control" id="FileDialog">--%>
                </div>

                <div class="col-sm-3">
                    <div class="form-group row mb-2" style="justify-content: flex-end">
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
        $(document).ready(function () {
            <%--  $("#<%=btnPreview.ClientID %>").click(function () {
            });--%>
        });
    </script>
</asp:Content>



