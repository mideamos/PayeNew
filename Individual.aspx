<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Individual.aspx.cs" Inherits="Individual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
.modalBackground
{
background-color: Gray;
filter: alpha(opacity=80);
opacity: 0.8;
z-index: 10000;
}
</style>
    <style type="text/css">
  .hiddencol
  {
    display: none;
  }
</style>
           <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
    
    <script type="text/javascript">



        function gf() {
            $(document).ready(function () {

                var gridId = "<%= grd_ind.ClientID %>";
                var rowClickEvent = "#" + gridId + " tr"
                var current = "";

                $('#<%=grd_ind.ClientID %> tr').click(function () {

                    var row = this;
                    var name = row.children[0].innerText;
                    var surname = row.children[1].innerText;




                    $("#Ind_name").text(surname);
                    $("#tin").text(row.children[6].innerText);
                    $("#Phone1").text(row.children[3].innerText);

                    $("#email1").text(row.children[4].innerText);

                    $("#taxoffice").text(row.children[5].innerText);


                    $("#address").text(row.children[7].innerText);


                    current = name + surname;

                });
            });

        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" Runat="Server">
    Search tax payer- Individual
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="portlet-title">
    <div class="caption">
       Search tax payer- Individual
        
    </div>
        </div>
    
    <div>
            <table class="table borderless" style="width:45% !important; border:none !important;">
                <tr>
                    <td>Name:</td> <td></td><td><asp:TextBox ID="txt_name" runat="server" CssClass="form-control"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Mobile:</td> <td></td><td><asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>RIN:</td> <td></td><td><asp:TextBox ID="txt_RIN" runat="server" CssClass="form-control"></asp:TextBox></td>
                </tr>

                <tr>
                    <td colspan="3" style="text-align: right;"><asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click"/></td>
                </tr>

            </table>
        </div>
        <br />

       <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
          
    <ContentTemplate> 
        
        <div class="portlet light">  
      <div class="portlet-title">
                 <div class="caption">Individual List</div>
          <div class="actions">
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
                            </div>
         </div>
               
       <div>
    
        <asp:GridView ID="grd_ind" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" 
        AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" 
        CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" PagerSettings-PageButtonCount="5">
       

         <Columns>

         <asp:BoundField DataField="TaxPayerId" HeaderText="Tax Payer ID"  />
         <asp:BoundField DataField="TaxPayerName" HeaderText="Tax Payer Name"/>
        <asp:BoundField DataField="TaxPayerRIN" HeaderText="Tax Payer RIN" />
        <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
        <asp:BoundField DataField="EmailAddress" HeaderText="E-Mail Address" />
        <asp:BoundField DataField="TaxOffice" HeaderText="Tax Office" />

  <asp:BoundField DataField="Tin" HeaderText="TIN" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

               <asp:BoundField DataField="ContactAddress" HeaderText="Address" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

             <asp:TemplateField HeaderText = "Actions">
             <ItemTemplate>
               
              <div class="btn-group">
                                                <button type="button" class="btn btn-theme btn-xs md-skip dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                    Action <span class="caret"></span>
                                                </button>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <%--<a data-toggle="modal" data-target="#divTaxPayerModal" runat="server">Quick View</a>--%>

                                                        <asp:LinkButton data-toggle="modal" data-target="#divTaxPayerModal" runat="server" ID="lnkCustDetails" OnClientClick="gf();" Text="Quick View"/>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton PostBackUrl= '<%#"~/TaxPayerDetails.aspx?name="+Eval("TaxPayerName")+"|"+"Ind"+"|"+Eval("ContactAddress")+"|"+Eval("Tin")+"|"+Eval("TaxPayerRIN")%>' runat="server" ID="lnkDetails"> Tax Payer Details </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
        </ItemTemplate>
        </asp:TemplateField>
         </Columns>
       
         <HeaderStyle CssClass="GridHeader" />
         <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />
        </asp:GridView>
       
            <div style="margin-top:-60px;margin-left:10px;" id="div_paging" runat="server">Showing <asp:Label runat="server" ID="lblpagefrom"></asp:Label> - <asp:Label runat="server" ID="lblpageto"></asp:Label> entries of <asp:Label runat="server" ID="lbltoal"></asp:Label> entries</div>
       </div>     
       </div>
       

        <div class="modal fade" id="divTaxPayerModal" tabindex="-1" role="dialog" aria-labelledby="divTaxPayerModalLabel" style="display: none;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title" id="divTaxPayerModalLabel">Individual Tax Payer Details</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="control-label control-label-static bold">Tax Payer Type</label>
                                <div>Individual</div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="control-label control-label-static bold">Tax Payer Name </label>
                                <div id="Ind_name"></div>
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
                                <div id="Phone1" ></div>
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
                                <div id="address"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
     </ContentTemplate></asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" Runat="Server">
</asp:Content>

