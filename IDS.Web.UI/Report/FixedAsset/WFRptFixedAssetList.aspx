<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFRptFixedAssetList.aspx.cs" Inherits="IDS.Web.UI.Report.FixedAsset.WFRptFixedAssetList"  MasterPageFile="~/Report/Template/ReportMaster.Master"%>
<%@Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    
    <div class="col-sm-6 col-md-6 col-6">
        <h4>Asset List</h4>
    </div>

            <div class="row mb-0">
                <div class="form-group row col-sm-3">
                    <label for="cboBranch" class="col-form-label col-form-label-sm col-sm-3">Branch</label>
                    <div class="col-sm-9">
                        <asp:DropDownList AutoPostBack="false"  ID="cboBranch" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboAssetGroup" class="col-form-label col-form-label-sm col-sm-4">Asset_Group</label>
                    <div class="col-sm-8">
                        <asp:DropDownList AutoPostBack="false"  ID="cboAssetGroup" onselectedindexchanged="cboExpense_SelectedIndexChanged"  runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <label for="cboExpense" class="col-form-label col-form-label-sm col-sm-4">Group</label>
                    <div class="col-sm-8">
                         <asp:DropDownList AutoPostBack="false" onselectedindexchanged="cboExpense_SelectedIndexChanged" ID="cboExpense" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                    </div>
                </div>

                <div class="form-group row col-sm-3">
                    <div class="col-sm-9 text-right">
                          <asp:Button ID="btnExe" runat="server" CssClass="form-control form-control-sm" Width="100px" Text="Print Preview" OnClick="btnExe_Click" ></asp:Button>
                            <asp:PlaceHolder ID="phButtonToLabelsAdminBox" runat="server"></asp:PlaceHolder>
                        <!--
                        <p>
                            <a onclick="goPreview()" class="btn btn-sm btn-primary" href="#">
                                <i class="fas fa-search"></i>
                            </a>
                        </p>
                        -->
                       

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

<asp:Content ID="Content1" runat="server" contentplaceholderid="StyleSection">
    <style type="text/css">
        .btn-default {}
    </style>
</asp:Content>

<script runat="server">
    
</script>