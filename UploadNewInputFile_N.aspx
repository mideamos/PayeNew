<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UploadNewInputFile_N.aspx.cs" Inherits="UploadNewInputFile_N" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    Upload PAYE Input File
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server"
    <ContentTemplate> --%>




    <div class="portlet light">
        <div class="portlet-title">
            <div class="caption">Upload Paye Input File</div>
        </div>

        <div>

            <script src="js/Extension.min.js" type="text/javascript"></script>
            <div id="divmsg" class="" runat="server" style="display: none"></div>
            <table class="table borderless" style="width: 100% !important; border: none !important;">
                <tr>
                    <td align="right">Select Company:</td>
                    <td width="30"></td>
                    <td>
                        <asp:DropDownList ID="drpfupcomapny" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false"></asp:DropDownList></td>
                </tr>

                <tr>
                    <td align="right">Select Business:</td>
                    <td width="30"></td>
                    <td align="left">
                        <asp:DropDownList ID="drpbusiness" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList><asp:Label Text="NA" runat="server" ID="lblnabusiness" Visible="false"></asp:Label></td>


                </tr>

                <tr>
                    <td align="right">Select File to upload:</td>
                    <td width="30"></td>
                    <td>
                        <asp:FileUpload runat="server" ID="fpemp" Style="float: left;" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4"><a href="EmployeeTemplate.xls" style="font-size: small; text-decoration: underline; color: blue;">Download Employee Template here</a></td>
                </tr>

                <tr class="tblrw">
                    <td colspan="4" align="center">
                        <asp:Button ID="btnupload" CssClass="btn btn-redtheme" Text="Upload" OnClick="btnupload_Click"  runat="server" /></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <br />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%-- </ContentTemplate></asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

