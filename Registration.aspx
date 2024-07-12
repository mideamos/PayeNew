<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="http://code.jquery.com/ui/1.11.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>

    <script type="text/javascript">
        function HideLabel() {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=divmsg.ClientID %>").style.display = "none";
            }, seconds * 2000);
        };
    </script>



    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="WaterMark.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("[id*=ctl00_ContentPlaceHolder1_txt_house_no]").WaterMark();


        });
    </script>
      <script>
          document.addEventListener('DOMContentLoaded', function () {
              var btnUpdate = document.getElementById('btnUpdate');
              var btnsave = document.getElementById('btnUpdateId');

              btnsave.addEventListener('click', function (e) {
                  if (!btnUpdateId.style.display || btnUpdateId.style.display === 'block') {
                      e.preventDefault(); // Prevent navigation if visibility is not set to 'none
                  }
              });

          });
      </script>

</asp:Content>
<asp:Content ID="content3" ContentPlaceHolderID="contentheading" runat="server">
    Registration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(HideLabel);
            </script>
            <div>

                <div id="divmsg" class="alert alert-warning" runat="server" style="display: none"></div>

                <table style="text-align: center; width: 98%;" class="table borderless">

                    <tr class="tblrw">
                        <td colspan="6">
                            <asp:RadioButton ID="rd_search" runat="server" Text="Search Via RIN No." GroupName="g" AutoPostBack="true" Checked="true" OnCheckedChanged="rd_search_CheckedChanged" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:RadioButton ID="rd_Ind" runat="server" Text="Add Company Without RIN No." AutoPostBack="true" GroupName="g" OnCheckedChanged="rd_Ind_CheckedChanged" />

                        </td>

                    </tr>

                    <tr class="tblrw" id="tr_enter_TIN" runat="server">
                        <td colspan="6" align="center">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_enter_TIN" runat="server" Text="Enter RIN:" CssClass="control-label" ForeColor="Black" BackColor="White" BorderStyle="None"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txt_enter_TIN" runat="server" CssClass="form-control"></asp:TextBox></td>
                                    <td>&nbsp;&nbsp;&nbsp;<asp:Button ID="btn_search" runat="server" CssClass="btn btn-redtheme" OnClick="btn_search_Click" Text="Search" /></td>
                                </tr>

                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>

                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_RIN" runat="server" Text="RIN:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_RIN" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                        </td>

                        <td style="text-align: right">
                            <asp:Label ID="lbl_email1" runat="server" Text="E-Mail Add1:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_Email1" runat="server" CssClass="form-control"></asp:TextBox>


                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_company" runat="server" Text="Company Name:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_company" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                        </td>

                        <td style="text-align: right">
                            <asp:Label ID="lbl_email2" runat="server" Text="E-Mail Add2:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_Email2" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_Company_TIN" runat="server" Text="Company TIN:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_Company_TIN" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </td>

                        <td style="text-align: right">
                            <asp:Label ID="lbl_tax_ofc" runat="server" Text="Tax Office:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="txt_tax_ofc" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_mob1" runat="server" Text="Mobile1:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_mob1" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>

                        <td style="text-align: right">
                            <asp:Label ID="lbl_tax_payer_type" runat="server" Text="Tax Payer Type:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="txt_tax_payer_type" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_mob2" runat="server" Text="Mobile2:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_mob2" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>

                        <td style="text-align: right">
                            <asp:Label ID="lbl_economic_Activity" runat="server" Text="Economic Activity:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="txt_economic_activity" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_tax_payer_status" runat="server" Text="Tax Payer Status:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="txt_tax_payer_status" runat="server" CssClass="form-control">
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>InActive</asp:ListItem>
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_preferred_Notification" runat="server" Text="Preferred Notification:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="txt_preferred_notification" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw" style="display: none;">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_Acct_Bal" runat="server" Text="Account Balance:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_Acct_Bal" runat="server" CssClass="form-control" Enabled="False" Text="0"></asp:TextBox>
                        </td>

                        <td style="text-align: right; display: none;">
                            <asp:Label ID="lbl_Company_Created_By" runat="server" Text="Company Created By:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td style="display: none;">
                            <asp:TextBox ID="txt_Company_Created_By" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>

                    <tr class="tblrw" style="display: none;">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_business_type" runat="server" Text="Business Type:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="dpd_business_type" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="dpd_business_type_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_business_category" runat="server" Text="Business Category:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td style="">
                            <asp:DropDownList ID="dpd_business_category" AutoPostBack="true" runat="server" CssClass="form-controlform-control" OnSelectedIndexChanged="dpd_business_category_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw" style="display: none;">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_business_structure" runat="server" Text="Business Structure:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="dpd_business_structure" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_business_sector" runat="server" Text="Business Sector:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td style="">
                            <asp:DropDownList ID="dpd_business_sector" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="dpd_business_sector_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw" style="display: none;">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_business_sub_sector" runat="server" Text="Business Sub Sector:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="dpd_business_sub_sector" runat="server" CssClass="form-control" OnSelectedIndexChanged="dpd_business_sub_sector_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_business_operations" runat="server" Text="Business Operations:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td style="">
                            <asp:DropDownList ID="dpd_business_operations" runat="server" CssClass="form-control">
                            </asp:DropDownList>

                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">
                            <asp:Label ID="lbl_local_govt_areas" runat="server" Text="Local Goverment Areas:"></asp:Label>
                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="dpd_lga" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dpd_lga_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_contact_person" runat="server" Text="Contact Person:"></asp:Label>

                        </td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_contact_person" runat="server" CssClass="form-control"></asp:TextBox>


                        </td>
                    </tr>

                    <tr class="tblrw" style="display: none;">


                        <td style="text-align: right;">
                            <asp:Label ID="lbl_profile" runat="server" Text="Profile:"></asp:Label>
                        </td>
                        <td style="">
                            <asp:DropDownList ID="dpd_profile" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>

                        <td></td>
                        <td style="text-align: right">
                            <asp:Label ID="lbl_asset_type" runat="server" Text="Asset Type:" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="dpd_asset_type" runat="server" Visible="false" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="dpd_asset_type_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right;">

                            <asp:Label ID="lbl_address1" runat="server" Text="Address1:"></asp:Label></td>
                        <td width="30"></td>
                        <td>

                            <asp:TextBox ID="txt_house_no" runat="server" CssClass="form-control" placeholder="House No." TextMode="MultiLine"></asp:TextBox>


                        </td>

                        <td style="text-align: right;">
                            <asp:Label ID="lbl_address2" runat="server" Text="Address2:"></asp:Label></td>
                        <td width="30"></td>
                        <td>
                            <asp:TextBox ID="txt_street" runat="server" CssClass="form-control" placeholder="Street" TextMode="MultiLine"></asp:TextBox></td>

                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right;">



                            <asp:Label ID="lbl_address3" runat="server" Text="Address3:"></asp:Label></td>
                        <td width="30"></td>
                        <td>

                            <asp:TextBox ID="txt_off_street_town" runat="server" CssClass="form-control" placeholder="Off Street & Town" TextMode="MultiLine"></asp:TextBox>
                        </td>



                        <td style="text-align: right">
                            <asp:Label ID="lbl_state" runat="server" Text="Select State:"></asp:Label></td>
                        <td width="30"></td>
                        <td>
                            <asp:DropDownList ID="dpd_state" runat="server" CssClass="form-control">
                            </asp:DropDownList></td>
                    </tr>

                    <tr class="tblrw">
                        <td style="text-align: right">

                            <asp:Label ID="lbl_town" runat="server" Text="Select Town:"></asp:Label></td>
                        <td width="30"></td>
                        <td>

                            <asp:DropDownList ID="dpd_town" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td align="right">Ward:</td>

                        <td></td>


                        <td>
                            <asp:DropDownList ID="dpd_ward" runat="server" CssClass="form-control" Width="200px">
                            </asp:DropDownList>

                        </td>


                    </tr>


                    <li id="btnUpdateId" runat="server">
                        <tr class="tblrw">
                            <td align="center" colspan="6">
                                <asp:Button ID="btnsave" runat="server" CssClass="btn btn-redtheme" OnClick="btnsave_Click" Text="Update & Proceed" />
                            </td>
                        </tr>
                    </li>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>


                </table>

            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
