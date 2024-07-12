<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserManagement.aspx.cs" Inherits="UserManagement" %>

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

    <script type="text/javascript">
        function Confirm_drop_emps() {

            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Are You Sure You Want to This User InActive?")) {
                confirm_value.value = "";
                confirm_value.value = "Yes";
                document.getElementById('<%= hidden1.ClientID %>').value = "Yes";
                showImage();
            } else {
                confirm_value.value = "";
                confirm_value.value = "No";
                document.getElementById('<%= hidden1.ClientID %>').value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
    <%-- <script type="text/javascript">
        function gf() {
            $(function () {
                var gridId = "<%= grd_user_management.ClientID %>";
                var rowClickEvent = "#" + gridId + " tr"
                var current = "";

                $("#" + gridId).on("click", "span.close", function () {
                    //Remove the row when user click on X
                    $(this).parent().parent().empty();
                });

                $(rowClickEvent).click(function () {
                    //Add row containing aditional info when user click on a row inside the grid view
                    var row = this;

                    $("#tax_payer_rin").text(row.children[0].innerText);
                    $("#tax_payer_name").text(row.children[1].innerText);
                    $("#tax_rule").text(row.children[3].innerText);

                    var assessed = "";
                    if (row.children[6].innerText == "No")
                        assessed = "Assessment Not Generated Yet";
                    else
                        assessed = "Assessment has been generated";
                    $("#tax_assessed").text(assessed);


                });
            });
        }
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    User Management
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="portlet light">
        <div class="portlet-title">
            <div class="caption">Users</div>
            <div class="actions">
                <div class="btn-group">
                    <button type="button" class="btn btn-redtheme dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Add New <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu">
                        <li class='<%= Session["roleId"].ToString() != "1" ? "hide" : "show" %>'>
                            <a href="UserAdd.aspx">User</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div style="overflow: scroll;">
            <%--<asp:GridView ID="grd_user_management" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" PagerSettings-PageButtonCount="5"
                AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" ShowFooter="false">
            --%>
            <asp:GridView ID="grd_user_management" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" PagerSettings-PageButtonCount="5"
                AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" ShowFooter="false">

                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Username" HeaderText="User Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email Address" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone Number" />
                    <asp:BoundField DataField="Role" HeaderText="Role" />
                    <asp:BoundField DataField="IsActive" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <div class="btn-group">
                                <button type="button"  class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Action <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li class='<%= Session["roleId"].ToString() != "1" ? "hide" : "show" %>'>
                                        <asp:LinkButton runat="server" ID="make_user_inactive" OnClick="btn_make_user_inactive_Click" OnClientClick="Confirm_drop_emp()"> Make User InActive/Active </asp:LinkButton>
                                        <%-- <asp:LinkButton PostBackUrl='<%#"~/ShowLegacyDataEmpInput.aspx?compRIN="+Eval("CompanyRIN")+"&year="+Eval("Tax_Year")+"&redirect=I&Employer="+Eval("CompanyName")+"&BusinessRIN="+Eval("BusinessRIN")+"&FiledStatus="+Eval("Status")+""%>' runat="server" ID="lnkDetails"> Manage Employees </asp:LinkButton>--%>
                                    </li>
                                    <li class='<%= Session["roleId"].ToString() != "1" ? "hide" : "show" %>'>
                                        <asp:LinkButton PostBackUrl='<%#"~/MyProfile.aspx?emailEMP="+Eval("Email")+""%>' runat="server" ID="lnkEdit"> Edit User </asp:LinkButton>
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
</asp:Content>
