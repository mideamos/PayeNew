<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Corporates.aspx.cs" Inherits="Corporates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        function gf() {
            $(function () {
                var gridId = "<%= grd_Corporate.ClientID %>";
                var rowClickEvent = "#" + gridId + " tr"
                var current = "";

                $("#" + gridId).on("click", "span.close", function () {
                    //Remove the row when user click on X
                    $(this).parent().parent().empty();
                });

                $(rowClickEvent).click(function () {
                    //Add row containing aditional info when user click on a row inside the grid view
                    var row = this;
                    var name = row.children[0].innerText;
                    var surname = row.children[1].innerText;


                    //if ((name + surname) != current) {
                    //    $("<tr style='background-color:yellow;'><td style='height:200px;vertical-align:top;'>Dear " + name + " " + surname + "<br />Your Account Was Created Successfully<td><td style='vertical-align:top;'><span class='close'>X</span></td></tr>").insertAfter(row);
                    //}
                    // $comp_name.html(name);
                    $("#comp_name").text(surname);
                    $("#tin").text(row.children[2].innerText);
                    $("#Phone1").text(row.children[3].innerText);

                    $("#email1").text(row.children[4].innerText);


                    $("#tin").text(row.children[2].innerText);

                    $("#taxoffice").text(row.children[7].innerText);


                    $("#address11").text(row.children[8].innerText);


                    current = name + surname;

                });
            });
        }
    </script>
</asp:Content>
<asp:Content ID="content3" runat="server" ContentPlaceHolderID="contentheading">
    Search tax payer - Corporate
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Search tax payer - Corporate
        
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <table class="table borderless" style="width: 45% !important; border: none !important;">
                    <tr>
                        <td>Name:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_name" runat="server" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>TIN:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_tin" runat="server" CssClass="form-control"></asp:TextBox></td>
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
            <div class="portlet light">
                <div class="portlet-title">
                    <div class="caption">List of Corporate Tax Payer</div>
                    <div class="actions">
                       
                        <li>
                            <div class="btn-group">
                                <button type="button" class="btn btn-redtheme dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Add New <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                        <a href="IndividualAdd.aspx">Individual</a>
                                    </li>
                                    <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                        <a href="CorporateAdd.aspx">Corporates</a>
                                    </li>
                                    <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                        <a href="GovernmentAdd.aspx">Government</a>
                                    </li>
                                    <li class='<%= Session["roleId"].ToString() == "3" ? "hide" : "show" %>'>
                                        <a href="SpecialAdd.aspx">Special</a>
                                    </li>
                                </ul>
                            </div>
                        </li>
                    </div>
                </div>
            </div>

            <div>

                <asp:GridView ID="grd_Corporate" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                    AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" PagerSettings-PageButtonCount="5"
                    CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" OnRowDataBound="grd_Corporate_RowDataBound">


                    <Columns>

                        <asp:BoundField DataField="TaxPayerRIN" HeaderText="Company RIN" />
                        <asp:BoundField DataField="TaxPayerName" HeaderText="Company Name" />
                        <asp:BoundField DataField="TIN" HeaderText="Company TIN" />
                        <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" />
                        <asp:BoundField DataField="EmailAddress" HeaderText="Email" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <%-- <asp:BoundField DataField="EconomicActivitiesName" HeaderText="Economic Activity" />--%>
                        <%--<asp:BoundField DataField="ActiveText" HeaderText="Tax Payer Status" />--%>

                        <asp:BoundField DataField="MobileNumber" HeaderText="Mobile" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                        <asp:BoundField DataField="EmailAddress" HeaderText="Email" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                        <asp:BoundField DataField="TaxOffice" HeaderText="TO" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                        <asp:BoundField DataField="ContactAddress" HeaderText="Address" />



                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>

                                <div class="btn-group">
                                    <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        Action <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li>
                                            <a data-toggle="modal" data-target="#divTaxPayerModal" onclick="gf();" runat="server">Quick View</a>


                                        </li>
                                        <li>
                                            <asp:LinkButton PostBackUrl='<%#"~/TaxPayerDetails.aspx?name="+Eval("TaxPayerName")+"|"+"Cmp"+"|"+Eval("ContactAddress")+"|"+Eval("Tin")+"|"+Eval("TaxPayerRIN")+"|"+Eval("MobileNumber")%>' runat="server" ID="lnkDetails"> Tax Payer Details </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                    <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                </asp:GridView>

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

            <div class="modal fade" id="divTaxPayerModal" tabindex="-1" role="dialog" aria-labelledby="divTaxPayerModalLabel" style="display: none;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="divTaxPayerModalLabel">Corporate Tax Payer Details</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Tax Payer Type</label>
                                        <div>Corporate</div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Company Name </label>
                                        <div id="comp_name"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">TIN </label>
                                        <div id="tin"></div>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Phone No.</label>
                                        <div id="Phone1"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Email Address</label>
                                        <div id="email1"></div>
                                    </div>
                                </div>

                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Tax Office</label>
                                        <div id="taxoffice"></div>
                                    </div>
                                </div>


                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <label class="control-label control-label-static bold">Contact Address </label>
                                        <div id="address11"></div>
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
