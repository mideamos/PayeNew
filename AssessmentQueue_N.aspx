<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="AssessmentQueue_N.aspx.cs" Inherits="AssessmentQueue_N" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    Assessment Queue
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Assessment Queue
        </div>


    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div>
                <table class="table borderless" style="width: 100% !important; border: none !important;">
                    <tr>
                        <header>Search</header>
                    </tr>
                    <tr>
                        <td>Tax Year:</td>
                        <td>
                            <asp:DropDownList ID="txt_tax_year" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="btn_search_Click"></asp:DropDownList></td>
                        <td>
                            <asp:TextBox ID="txt_employer_RIN" runat="server" CssClass="form-control" placeholder="Search EmployerRin" AutoPostBack="true" ></asp:TextBox>
                        
                        </td>
                        <td>
                            <asp:TextBox ID="txt_assetRin" runat="server" CssClass="form-control" placeholder="Search AssetRin" AutoPostBack="true"></asp:TextBox>
                                    <td><asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="txt_employer_RIN_TextChanged" Visible="true" /></td>
                        </td>
                      <%--  <td colspan="3" style="text-align: right;">
                            <asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click" Visible="true" /></td>--%>
                    </tr>
                </table>
            </div>

            <div>
                <table>

                    <tr>
                        <td>
                            <asp:GridView ID="grdAssessmentQueue" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                                AutoGenerateColumns="false" PagerSettings-PageButtonCount="5" ShowHeaderWhenEmpty="true"
                                CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" OnPageIndexChanging="grdAssessmentQueue_PageIndexChanging">

                                <Columns>
                                    <asp:BoundField DataField="TaxPayerRINNumber" HeaderText="Employer RIN" />
                                    <asp:BoundField DataField="TaxPayerName" HeaderText="Employer Name" />
                                    <asp:BoundField DataField="AssetName" HeaderText="Business Name" />
                                    <asp:BoundField DataField="AssetRIN" HeaderText="Asset (Business)" />
                                    <asp:BoundField DataField="AssessmentRuleID" HeaderText="RuleID" Visible="false" />
                                    <asp:BoundField DataField="assessmentrulename" HeaderText="Rule" />
                                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Month/Year" />
                                    <asp:BoundField DataField="TaxBaseAmount" HeaderText="Assessed Amount(₦)"  />
                                    <%--  <asp:BoundField DataField="AssessmentNotes" HeaderText="Assessment Notes"/>--%>
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Assessment_RefNo" HeaderText="Assessment Ref." />
                                </Columns>

                                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

