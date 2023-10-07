<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TaxPayerDetails.aspx.cs" Inherits="TaxPayerDetails" %>

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

        $(document).ready(function () {

            var gridId = "<%= grd_AssessmentRules.ClientID %>";
            var rowClickEvent = "#" + gridId + " tr"
            var current = "";
            $("#" + gridId).on("click", "span.close", function () {
                $(this).parent().parent().empty();
            });
            $('#<%=grd_AssessmentRules.ClientID %> tr').click(function () {

                var row = this;

                $("#rulename").text(row.children[2].innerHTML);
                $("#ruleyear").text(row.children[0].innerHTML);

            });
        });

        $(document).ready(function () {

            var gridId_Asset = "<%= grd_asset.ClientID %>";
            var rowClickEvent = "#" + gridId_Asset + " tr"
            var current = "";
            $("#" + gridId_Asset).on("click", "span.close", function () {
                $(this).parent().parent().empty();
            });
            $('#<%=grd_asset.ClientID %> tr').click(function () {

                var row = this;

                $("#div_assettype").text(row.children[0].innerHTML);
                $("#div_assetname").text(row.children[1].innerHTML);
                $("#div_role").text(row.children[2].innerHTML);


            });
        });


        $(document).ready(function () {

            var gridId_Profile = "<%= grd_Profiles.ClientID %>";
            var rowClickEvent = "#" + gridId_Profile + " tr"
            var current = "";
            $("#" + gridId_Profile).on("click", "span.close", function () {
                $(this).parent().parent().empty();
            });
            $('#<%=grd_Profiles.ClientID %> tr').click(function () {

                var row = this;

                $("#div_profile").text(row.children[0].innerHTML);
                $("#div_profile_des").text(row.children[1].innerHTML);


            });
        });


        $(document).ready(function () {

            var gridId_Bills = "<%= grd_Associated_Bills.ClientID %>";
            var rowClickEvent = "#" + gridId_Bills + " tr"
            var current = "";
            $("#" + gridId_Bills).on("click", "span.close", function () {
                $(this).parent().parent().empty();
            });
            $('#<%=grd_Associated_Bills.ClientID %> tr').click(function () {

                var row = this;

                $("#div_bill_date").text(row.children[0].innerHTML);
                $("#div_bill_type").text(row.children[1].innerHTML);

                $("#div_bill_ID").text(row.children[2].innerHTML);
                $("#div_bill_amount").text(row.children[3].innerHTML);
                $("#div_bill_status").text(row.children[4].innerHTML);

            });
        });
    </script>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var AddAss = document.getElementById('AddAss');
            var AddAssId = document.getElementById('AddAssId');

            AddAss.addEventListener('click', function (e) {
                if (!AddAssId.style.display || AddAssId.style.display === 'block') {
                    e.preventDefault(); // Prevent navigation if visibility is not set to 'none'
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">

    <asp:Label ID="lbl_main_head" runat="server">Tax Payer Information</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="portlet light">
                <div class="portlet-title">
                    <div class="caption">

                        <asp:Label ID="lbl_head1" runat="server">Tax Payer Information</asp:Label>
                    </div>
                </div>

                <div>
                    <table class="table borderless" style="width: 100% !important; border: none !important;">
                        <tr>
                            <td><b>Tax Payer Type:</b></td>
                            <td>
                                <asp:Label ID="txt_type" runat="server"></asp:Label></td>
                            <td><b>Tax Payer Name:</b></td>
                            <td>
                                <asp:Label ID="txt_name" runat="server"></asp:Label></td>
                            <td></td>
                        </tr>

                        <tr>
                            <td><b>RIN:</b></td>
                            <td>
                                <asp:Label ID="txt_RIN" runat="server"></asp:Label></td>
                            <td><b>Tin:</b></td>
                            <td>
                                <asp:Label ID="txt_tin" runat="server"></asp:Label></td>
                            <td></td>
                        </tr>

                        <tr>
                            <td><b>Mobile Number:</b></td>
                            <td>
                                <asp:Label ID="txt_Mobileno" runat="server"></asp:Label></td>
                            <td><b>Contact Address:</b></td>
                            <td>
                                <asp:Label ID="txt_address" runat="server"></asp:Label></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>

            <div class="portlet light" id="div_assets" runat="server">
                <div class="portlet-title">
                    <div class="caption">
                        Associated Assets
        
                    </div>
                    <div class="actions">
                        <div class="btn-group">
                            <button type="button" class="btn btn-redtheme dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                Add New <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu">
                                <li id="AddAssId" runat="server">
                                    <a href="AddAsset.aspx" id="AddAss">Asset</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div>
                    <div class="actions" style="float: right;">
                        <table>
                            <tr>
                                <td>Search:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtsearchasset" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtsearchasset_TextChanged" Width="160px" Height="30px"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>

                        <asp:GridView ID="grd_asset" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                            AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5" OnRowDataBound="grd_AssessmentRules_RowDataBound" OnPageIndexChanging="grd_asset_PageIndexChanging">


                            <Columns>

                                <asp:BoundField DataField="AssetTypeName" HeaderText="Asset Type" />
                                <asp:BoundField DataField="BusinessName" HeaderText="Asset Name" />
                                <asp:BoundField DataField="BusinessTypeName" HeaderText="Tax Payer Role" />

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                    <asp:LinkButton data-toggle="modal" data-target="#divAssetModal" runat="server" ID="lnkCustDetails" Text="Quick View" />
                                                </li>

                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table table-striped table-bordered table-hover" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_grd_AssessmentRules" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="GridHeader">
                                            <th scope="col">Asset Type</th>
                                            <th scope="col">Asset Name</th>
                                            <th scope="col">Tax Payer Role </th>
                                            <th scope="col">Actions</th>
                                        </tr>
                                    </tbody>
                                </table>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                        </asp:GridView>

                        <div style="margin-top: 0px; margin-left: 10px;" id="divpagingasset" runat="server">Showing
                            <asp:Label runat="server" ID="lbl_page_from1" Text="0"></asp:Label>
                            -
                            <asp:Label runat="server" ID="lbl_page_to1" Text="0"></asp:Label>
                            entries of
                            <asp:Label runat="server" ID="lbl_page_all1" Text="0"></asp:Label>
                            entries</div>
                    </div>
                </div>

            </div>




            <div class="portlet light">
                <div class="portlet-title">
                    <div class="caption">
                        Associated Profiles
        
                    </div>
                </div>

                <div>
                    <div class="actions" style="float: right;">
                        <table>
                            <tr>
                                <td>Search:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtsearchprofile" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtsearchprofile_TextChanged" Width="160px" Height="30px"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>

                        <asp:GridView ID="grd_Profiles" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                            AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5">


                            <Columns>

                                <asp:BoundField DataField="ProfileReferenceNo" HeaderText="Profile Ref. No." />
                                <asp:BoundField DataField="ProfileDescription" HeaderText="Profile Description" />

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                    <asp:LinkButton data-toggle="modal" data-target="#divProfileModel" runat="server" ID="lnkCustDetails" Text="Quick View" />
                                                </li>

                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table table-striped table-bordered table-hover" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_grd_AssessmentRules" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="GridHeader">
                                            <th scope="col">Profile Ref. No.</th>
                                            <th scope="col">Profile Description</th>
                                            <th scope="col">Actions</th>
                                        </tr>
                                    </tbody>
                                </table>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                        </asp:GridView>

                        <div style="margin-top: 0px; margin-left: 10px;" id="div1" runat="server">Showing
                            <asp:Label runat="server" ID="lbl_page_from2" Text="0"></asp:Label>
                            -
                            <asp:Label runat="server" ID="lbl_page_to2" Text="0"></asp:Label>
                            entries of
                            <asp:Label runat="server" ID="lbl_page_all2" Text="0"></asp:Label>
                            entries</div>
                    </div>
                </div>

            </div>







            <div class="portlet light" id="div_grd_bills" runat="server">
                <div class="portlet-title">
                    <div class="caption">
                        Associated Bills
        
                    </div>
                </div>

                <div>
                    <div class="actions" style="float: right;">
                        <table>
                            <tr>
                                <td>Search:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtsearchbills" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtsearchbills_TextChanged" Width="160px" Height="30px"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>

                        <asp:GridView ID="grd_Associated_Bills" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                            AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5" OnRowDataBound="grd_AssessmentRules_RowDataBound" OnPageIndexChanging="grd_Associated_Bills_PageIndexChanging">


                            <Columns>

                                <asp:BoundField DataField="BillDate" HeaderText="Date" />
                                <asp:BoundField DataField="BillType" HeaderText="Bill Type" />
                                <asp:BoundField DataField="BillID" HeaderText="Bill ID" />
                                <asp:BoundField DataField="BillAmount" HeaderText="Bill Amount" />
                                <asp:BoundField DataField="BillStatus" HeaderText="Bill Status" />

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                    <asp:LinkButton data-toggle="modal" data-target="#divBillModel" runat="server" ID="lnkCustDetails" Text="Quick View" />
                                                </li>
                                                <li>
                                                    <asp:LinkButton runat="server" ID="lnkDetails" Text="View Items"></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table table-striped table-bordered table-hover" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_grd_AssessmentRules" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="GridHeader">
                                            <th scope="col">Date</th>
                                            <th scope="col">Bill Type</th>
                                            <th scope="col">Bill ID</th>
                                            <th scope="col">Bill Amount</th>
                                            <th scope="col">Bill Status</th>
                                            <th scope="col">Actions</th>
                                        </tr>
                                    </tbody>
                                </table>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                        </asp:GridView>

                        <div style="margin-top: -10px; margin-left: 10px;" id="div2" runat="server">Showing
                            <asp:Label runat="server" ID="lblpagefrom3"></asp:Label>
                            -
                            <asp:Label runat="server" ID="lblpageto3"></asp:Label>
                            entries of
                            <asp:Label runat="server" ID="lblpageTotal3"></asp:Label>
                            entries</div>
                    </div>
                </div>

            </div>







            <div class="portlet light" id="div_rules" runat="server">
                <div class="portlet-title">
                    <div class="caption">
                        Associated Rules
        
                    </div>
                </div>

                <div>
                    <div class="actions" style="float: right;">
                        <table>
                            <tr>
                                <td>Search:</td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtsearchrule" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtsearchrule_TextChanged" Width="160px" Height="30px"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <div>

                        <asp:GridView ID="grd_AssessmentRules" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                            AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5" OnRowDataBound="grd_AssessmentRules_RowDataBound">


                            <Columns>

                                <asp:BoundField DataField="TaxYear" HeaderText="Rule Year" />
                                <asp:BoundField DataField="AssessmentRuleCode" HeaderText="Rule Code" />
                                <asp:BoundField DataField="AssessmentRuleName" HeaderText="Rule Name" />
                                <asp:BoundField DataField="RuleRunName" HeaderText="Rule Run Name" />

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                    <asp:LinkButton data-toggle="modal" data-target="#divAssessmentRulesModal" runat="server" ID="lnkCustDetails" Text="Quick View" />
                                                </li>
                                                <li>
                                                    <asp:LinkButton runat="server" ID="lnkDetails" Text="View Items"></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table table-striped table-bordered table-hover" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_grd_AssessmentRules" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="GridHeader">
                                            <th scope="col">Rule Year</th>
                                            <th scope="col">Rule Code</th>
                                            <th scope="col">Rule Name</th>
                                            <th scope="col">Rule Run Name</th>
                                            <th scope="col">Actions</th>
                                        </tr>
                                    </tbody>
                                </table>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                        </asp:GridView>

                        <div style="margin-top: -60px; margin-left: 10px;" id="div_paging" runat="server">Showing
                            <asp:Label runat="server" ID="lblpagefrom"></asp:Label>
                            -
                            <asp:Label runat="server" ID="lblpageto"></asp:Label>
                            entries of
                            <asp:Label runat="server" ID="lbltoal"></asp:Label>
                            entries</div>
                    </div>
                </div>

            </div>





            <div class="portlet light" id="div_MDA_Services" runat="server">
                <div class="portlet-title">
                    <div class="caption">
                        Associated MDA Services
        
                    </div>
                </div>

                <div>
                    <div>

                        <asp:GridView ID="grd_MDA_Services" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                            AutoGenerateColumns="False"
                            CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5" OnRowDataBound="grd_AssessmentRules_RowDataBound" OnPageIndexChanging="grd_Associated_Bills_PageIndexChanging">


                            <Columns>

                                <asp:BoundField DataField="ServiceYear" HeaderText="Service Year" />
                                <asp:BoundField DataField="ServiceName" HeaderText="Service Name" />
                                <asp:BoundField DataField="ServiceAmt" HeaderText="Service Amount" />
                                <asp:BoundField DataField="BillAmount" HeaderText="Billed Amount" />

                                <asp:TemplateField HeaderText="Actions">
                                    <ItemTemplate>

                                        <div class="btn-group">
                                            <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                Action <span class="caret"></span>
                                            </button>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                    <asp:LinkButton data-toggle="modal" data-target="#divBillModel" runat="server" ID="lnkCustDetails" Text="Quick View" />
                                                </li>
                                                <li>
                                                    <asp:LinkButton runat="server" ID="lnkDetails" Text="View Items"></asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table class="table table-striped table-bordered table-hover" cellspacing="0" rules="all" border="1" id="ctl00_ContentPlaceHolder1_grd_AssessmentRules" style="border-collapse: collapse;">
                                    <tbody>
                                        <tr class="GridHeader">
                                            <th scope="col">Service Year</th>
                                            <th scope="col">Service Name</th>
                                            <th scope="col">Service Amount</th>
                                            <th scope="col">Billed Amount</th>
                                            <th scope="col">Actions</th>
                                        </tr>
                                    </tbody>
                                </table>
                            </EmptyDataTemplate>
                            <HeaderStyle CssClass="GridHeader" />
                            <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
                        </asp:GridView>

                        <div style="margin-top: -10px; margin-left: 10px;" id="div4" runat="server">Showing
                            <asp:Label runat="server" ID="lblpagefrom_MDA"></asp:Label>
                            -
                            <asp:Label runat="server" ID="lblpageto_MDA"></asp:Label>
                            entries of
                            <asp:Label runat="server" ID="lblpageAll_MDA"></asp:Label>
                            entries</div>
                    </div>
                </div>

            </div>








            <div class="modal fade in" id="divAssessmentRulesModal" tabindex="-1" role="dialog" aria-labelledby="divAssessmentRulesModalLabel" style="display: none; padding-right: 17px;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="divAssessmentRulesModalLabel">Assessment Rules</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Rule Name</label>
                                        <div id="rulename">Name</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Year</label>
                                        <div id="ruleyear">2017</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="modal fade in" id="divAssetModal" tabindex="-1" role="dialog" aria-labelledby="divAssetModalLabel" style="display: none; padding-right: 17px;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="divAssetModalLabel">Asset Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Asset Type</label>
                                        <div id="div_assettype">Building</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Asset Name</label>
                                        <div id="div_assetname">Name</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Tax Payer Role</label>
                                        <div id="div_role">Role</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="modal fade in" id="divProfileModel" tabindex="-1" role="dialog" aria-labelledby="divProfileModalLabel" style="display: none; padding-right: 17px;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="H1">Profile Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Profile Ref. No.</label>
                                        <div id="div_profile">Profile</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Profile Description</label>
                                        <div id="div_profile_des">Description</div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>





            <div class="modal fade in" id="divBillModel" tabindex="-1" role="dialog" aria-labelledby="divBillModalLabel" style="display: none; padding-right: 17px;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="H2">Bill Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Bill Date</label>
                                        <div id="div_bill_date">date</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Bill Type</label>
                                        <div id="div_bill_type">BillType</div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Bill ID</label>
                                        <div id="div_bill_ID">BillID</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Bill Amount</label>
                                        <div id="div_bill_amount">BillAmount</div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Bill Status</label>
                                        <div id="div_bill_status">BillStatus</div>
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

