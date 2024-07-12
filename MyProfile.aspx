<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyProfile.aspx.cs" Inherits="MyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="/../code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />

    <style>
        #ContentPlaceHolder1_RegularExpressionValidator1 {
            color: red;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    User Management
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="col-md-5">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Edit User</h3>
            </div>

            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>

            <div class="box-body">
                <div class="form-group">

                    <asp:Label ID="lbl_fname" runat="server" Text="First Name"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_fname" placeholder="Enter First Name" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_fname" runat="server" ControlToValidate="txt_fname" ErrorMessage="First Name is required." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                    <asp:Label ID="lbl_lname" runat="server" Text="Last Name"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_lname" placeholder="Enter Last Name" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_lname" runat="server" ControlToValidate="txt_lname" ErrorMessage="Last Name is required." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                    <asp:Label ID="lbl_phn" runat="server" Text="Phone Number"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_phn" placeholder="Enter Phone Number" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_phn" runat="server" ControlToValidate="txt_phn" ErrorMessage="Phone Number is required." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                    <asp:Label ID="lbl_email" runat="server" Text="Email"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_email" placeholder="Enter Email" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txt_email" ErrorMessage="Email is required." Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>

                    <asp:Label ID="lbl_password" runat="server" Text="Present Password"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_password" Text="password" placeholder="Enter Present Password" CssClass="form-control" ReadOnly="true"></asp:TextBox>


                    <asp:Label ID="lbl_new_password" runat="server" Text="New Password"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_new_password" placeholder="Enter New Password" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Password cannot be blank" ControlToValidate="txt_new_password" ForeColor="Red"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_new_password" ControlToValidate="txt_con_password" ErrorMessage="Password and Confirm password must be same" ForeColor="Red"></asp:CompareValidator>


                    <asp:Label ID="lbl_con_password" runat="server" Text="Confirm Password"></asp:Label>
                    <asp:TextBox runat="server" ID="txt_con_password" placeholder="Enter Confirm Password" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Password cannot be blank" ControlToValidate="txt_con_password" ForeColor="Red"></asp:RequiredFieldValidator>


                </div>
            </div>
        </div>
    </div>

    <div class="col-md-10" align="center">
        <asp:Button runat="server" CssClass="btn btn-primary" Text="Update" ID="btnUpdate" OnClick="btnUpdate_Click" />
    </div>
</asp:Content>

