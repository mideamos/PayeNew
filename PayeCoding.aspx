<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PayeCoding.aspx.cs" Inherits="PayeCoding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>


    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Run Tax Coding for the Employer Selected?")) {
                confirm_value.value = "";
                confirm_value.value = "Yes";
                document.getElementById('<%= hidden1.ClientID %>').value = "Yes";
            } else {
                confirm_value.value = "";
                confirm_value.value = "No";
                document.getElementById('<%= hidden1.ClientID %>').value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

        function Confirm_reverse() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want to Reverse Input File?")) {
                confirm_value.value = "";
                confirm_value.value = "Yes";
                document.getElementById('<%= hidden1.ClientID %>').value = "Yes";
            } else {
                confirm_value.value = "";
                confirm_value.value = "No";
                document.getElementById('<%= hidden1.ClientID %>').value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    Paye Coding
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Search
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div class="portlet-title">
                <div class="caption">Paye Coding (Corporate List)</div>
                <div align="right" class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                    <asp:Button ID="btn_file_selected" Text="Code Selected" runat="server" CssClass="btn btn-theme" OnClick="btn_file_selected_Click" OnClientClick="Confirm()" />

                </div>
            </div>

            <div>
                <table class="table borderless" style="width: 100% !important; border: none !important;">
                    <tr>
                        <td>Tax Year:</td>

                        <td>
                            <asp:DropDownList ID="txt_tax_year" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="btn_search_Click"></asp:DropDownList></td>
                        <td>Company RIN:</td>

                        <td>
                            <asp:TextBox ID="txt_cmp_RIN" runat="server" CssClass="form-control" placeholder="Company RIN"></asp:TextBox>

                        </td>
                        <td>Business RIN:</td>

                        <td>
                            <asp:TextBox ID="txt_employer_RIN" runat="server" CssClass="form-control" placeholder="Business RIN"></asp:TextBox>

                        </td>

                        <td colspan="3" style="text-align: right;">
                            <asp:Button ID="Button1" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click" Visible="true" />

                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>Employer Name:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_employer_name" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr style="display: none;">
                        <td>Employer TIN:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_employer_TIN" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr style="display: none;">
                        <td>Business RIN:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_business_RIN" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>




                </table>
            </div>
            <div class="portlet light">

                <div>
                    <asp:GridView ID="grd_Company" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" PagerSettings-PageButtonCount="5"
                        AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" ShowFooter="false" OnRowDataBound="grd_Company_RowDataBound">


                        <Columns>
                            <asp:BoundField DataField="CompanyRIN" HeaderText="Employer RIN" />
                            <asp:BoundField DataField="CompanyName" HeaderText="Employer Name" />
                            <%--  <asp:BoundField DataField="CompanyTIN" HeaderText="Employer TIN" />   --%>
                            <asp:BoundField DataField="Tax_Year" HeaderText="Tax Year" />
                            <asp:BoundField DataField="BusinessRIN" HeaderText="Business RIN" />
                            <asp:BoundField DataField="BusinessName" HeaderText="Business Name" Visible="false" />


                            <asp:BoundField DataField="EmployeeCount" HeaderText="Employee Count" />

                            <asp:BoundField DataField="Status" HeaderText="Status (Coded/Not Coded)" />

                            <asp:TemplateField HeaderText="CheckBox">
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkchkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            Action <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton PostBackUrl='<%#"~/ShowLegacyDataEmpInput.aspx?compRIN="+Eval("CompanyRIN")+"&year="+Eval("Tax_Year")+"&BusinessRIN="+Eval("BusinessRIN")+"&redirect=C&Employer="+Eval("CompanyName")+""%>' runat="server" ID="lnkDetails"> View Details </asp:LinkButton>
                                            </li>
                                            <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                                <asp:LinkButton runat="server" ID="lnksendtoinputfile" OnClick="btn_file_selected_Click" OnClientClick="Confirm()"> Code File </asp:LinkButton>
                                            </li>
                                            <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                                <asp:LinkButton runat="server" ID="lnk_reverse_Input" OnClick="btn_file_reverse_Click" OnClientClick="Confirm_reverse()" CommandName="Select"> Reverse to PAYE Input File </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>



                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                    </asp:GridView>
                    <input type="hidden" runat="server" value="" id="hidden1" />
                    <div style="margin-top: -60px; margin-left: 10px;" id="div_paging" runat="server">
                        Showing
                        <asp:Label runat="server" ID="lblpagefrom"></asp:Label>
                        -
                        <asp:Label runat="server" ID="lblpageto"></asp:Label>
                        entries of
                        <asp:Label runat="server" ID="lbltoal"></asp:Label>
                        entries
                    </div>
                </div>

            </div>

            <div class="modal fade" id="divTaxPayerModal" tabindex="-1" role="dialog" aria-labelledby="divTaxPayerModalLabel" style="display: none;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="divTaxPayerModalLabel">Assessment - Rules Check Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Tax Payer RIN </label>
                                        <div id="tax_payer_rin"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Tax Payer Name</label>
                                        <div id="tax_payer_name"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Assessment Rule</label>
                                        <div id="tax_rule"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Assessed</label>
                                        <div id="tax_assessed"></div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

