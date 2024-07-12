<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BusinessLists.aspx.cs" Inherits="BusinessLists" %>

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
        $(function () {
            var gridId = "<%= grd_Business.ClientID %>";
            var rowClickEvent = "#" + gridId + " tr"
            var current = "";

            $("#" + gridId).on("click", "span.close", function () {
                //Remove the row when user click on X
                $(this).parent().parent().empty();
            });

            $(rowClickEvent).click(function () {
                //Add row containing aditional info when user click on a row inside the grid view
                var row = this;

                $("#business_name").text(row.children[1].innerText);
                $("#business_address").text(row.children[2].innerText);
                $("#business_type").text(row.children[6].innerText);
                $("#business_lga").text(row.children[7].innerText);
                $("#business_category").text(row.children[8].innerText);
                $("#business_sector").text(row.children[3].innerText);


                // current = name + surname;
                //alert(comp_name.html);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    Search Assets - Business
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Search Assets - Business
        
        </div>
    </div>

    <div>
        <table class="table borderless" style="width: 45% !important; border: none !important;">
            <tr>
                <td>Name:</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txt_name" runat="server" CssClass="form-control"></asp:TextBox></td>
            </tr>

            <tr>
                <td>RIN:</td>
                <td></td>
                <td>
                    <asp:TextBox ID="txt_RIN" runat="server" CssClass="form-control"></asp:TextBox></td>
            </tr>

            <tr>
                <td colspan="3" style="text-align: right;">
                    <asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click" /></td>
            </tr>

        </table>
    </div>
    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="portlet light">
                <div class="portlet-title">
                    <div class="caption">Business List</div>

                    <li >
                        <div class="actions">
                            <div class="btn-group">
                                <button type="button" class="btn btn-redtheme dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Add New <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li>
                                        <a href="#">Building</a>
                                    </li>
                                    <li>
                                        <a href="IndividualBusinessAdd.aspx">Business</a>
                                    </li>
                                    <li>
                                        <a href="#">Vechile</a>
                                    </li>
                                    <li>
                                        <a href="#">Land</a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </div>


                <div>
                    <asp:GridView ID="grd_Business" runat="server" AllowPaging="True" AllowSorting="True" PageSize="15"
                        AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" ShowFooter="false" PagerSettings-PageButtonCount="5">


                        <Columns>




                            <asp:BoundField DataField="BusinessRIN" HeaderText="Business RIN" />
                            <asp:BoundField DataField="BusinessName" HeaderText="Business Name" />
                            <asp:BoundField DataField="BusinessAddress" HeaderText="Business Address" />
                            <asp:BoundField DataField="BusinessSectorName" HeaderText="Business Sector Name" />
                            <asp:BoundField DataField="BusinessSubSectorName" HeaderText="BusinessSubSector Name" />
                            <asp:BoundField DataField="BusinessID" HeaderText="BusinessID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>

                                    <div class="btn-group">
                                        <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            Action <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                <asp:LinkButton data-toggle="modal" data-target="#divTaxPayerModal" runat="server" ID="lnkCustDetails" Text="Quick View" OnClick="lnkCustDetails_Click" />
                                            </li>

                                            <li>
                                                <asp:LinkButton PostBackUrl='<%#"~/BusinessDetails.aspx?BusinessDet="+Eval("BusinessRIN")+"|"+Eval("BusinessName")+"|"+Eval("BusinessAddress")+"|"+(Eval("BusinessSectorName")).ToString().Replace("&","*--*").ToString()+"|"+Eval("BusinessID")%>' runat="server" ID="lnkDetails"> View Details </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="BusinessTypeName" HeaderText="BusinessTypeName" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField DataField="LGAName" HeaderText="LGAName" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                            <asp:BoundField DataField="BusinessCategoryName" HeaderText="BusinessCategoryName" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                        </Columns>



                        <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                    </asp:GridView>
                    <div style="margin-left: 10px;" id="div_paging" runat="server">
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
                            <h4 class="modal-title" id="divTaxPayerModalLabel">Business Profile</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Asset Type</label>
                                        <div>Business</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business Name </label>
                                        <div id="business_name"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business Address</label>
                                        <div id="business_address"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business Type</label>
                                        <div id="business_type"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business LGA</label>
                                        <div id="business_lga"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business Category</label>
                                        <div id="business_category"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Business Sector </label>
                                        <div id="business_sector"></div>
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

