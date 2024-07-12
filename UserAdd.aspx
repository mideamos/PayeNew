<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserAdd.aspx.cs" Inherits="UserAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">  
        $(document).ready(function () {
            $('#show_password').hover(
                function () {
                    // Change the attribute to text  
                    $('#<%= txtpassword.ClientID %>').attr('type', 'text');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                },
                function () {
                    // Change the attribute back to password 
                    $('#<%= txtpassword.ClientID %>').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
            );

            $('#show_password2').hover(
                function () {
                    // Change the attribute to text  
                    $('#<%= txtpassword2.ClientID %>').attr('type', 'text');
                    $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                },
                function () {
                    // Change the attribute back to password 
                    $('#<%= txtpassword2.ClientID %>').attr('type', 'password');
                    $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                }
            );
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    User Management
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Add User
        </div>
        <div class="actions">
            <a href="UserManagement.aspx" class="btn btn-redtheme">Cancel </a>
        </div>
    </div>
    <div id="divIndividualForm">

        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label required-star">First Name</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtfirstname" placeholder="Enter First Name"></asp:TextBox>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label required-star">Last Name</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtlastname" placeholder="Enter Last Name"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label">Middle Name</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtmiddlename" placeholder="Enter Middle Name"></asp:TextBox>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label">Email Address</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtemail1" placeholder="Enter Email Address 1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtemail1" ErrorMessage="Email is required." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label required-star">UserName</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtuserName" placeholder="Enter User Name"></asp:TextBox>
                </div>
            </div>

            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label">Designation</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtdesign" placeholder="Enter Designation"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label required-star">Phone 1</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtphone1" placeholder="Enter Phone 1"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="form-group">
                    <label class="control-label required-star">Role </label>
                    <asp:DropDownList ID="drptitle" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="drptitle" InitialValue="Select" ErrorMessage="Please select a role." Display="Dynamic" CssClass="text-danger"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <div class="form-group">
                <label class="control-label">Password</label>
                <asp:TextBox runat="server" CssClass="form-control" ID="txtpassword" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                <div class="input-group-append">
                    <button id="show_password" class="btn btn-primary" type="button">
                        <span class="fa fa-eye-slash icon"></span>
                    </button>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="form-group">
                <label class="control-label">Confirm Password</label>
                <asp:TextBox runat="server" CssClass="form-control" ID="txtpassword2" TextMode="Password" placeholder="Confirm Password"></asp:TextBox>
                <div class="input-group-append">
                    <button id="show_password2" class="btn btn-primary" type="button">
                        <span class="fa fa-eye-slash icon"></span>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="text-right col-sm-6 text-right">
            <div class="form-group">
                <asp:Button ID="btnsaveindividual" Style="color: darkgreen;" CssClass="button" runat="server" Text="Save" OnClick="btn_add_user_Click" />
                <%--             <asp:Button runat="server" ID="btnsaveindividual"OnClick="btn_add_user_Click" CssClass="btn-theme btn" Text="Save" />--%>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

