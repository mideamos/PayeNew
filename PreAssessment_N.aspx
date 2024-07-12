<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PreAssessment_N.aspx.cs" Inherits="PreAssessment_N" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>

    <script type="src= http://code.jquery.com/jquery-1.9.1.js"></script>
    <script
        src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@8"></script>



    <script type="text/javascript">


        function startUpdate() {
            Swal.fire({
                title: '<strong>Do not Close this window</strong>',
                type: 'info',
                html:
                    'Closing this window may cause loss in the data',
                showCloseButton: true,
                showCancelButton: true,
                focusConfirm: false,
                confirmButtonText:
                    'Start',
                confirmButtonAriaLabel: 'Start!',
                cancelButtonText:
                    'Cancel',
                cancelButtonAriaLabel: 'Thumbs down',
            }).then((result) => {
                if (result.value) {
                    //truncateData_start_syncing();

                    Swal.fire({

                        showCancelButton: false,
                        showConfirmButton: false,
                        title: 'Syncing',
                        html: '<img width=40 src="https://gifimage.net/wp-content/uploads/2018/04/loader-gif-transparent-background-11.gif"/><div class="progress">' +

                            '<div class="progress-meter" style="width:0%"></div>' +
                            '</div >' +
                            '<span class="progress-val">0%</span> complete ',
                        allowOutsideClick: false


                    });
                    callUpdate();
                }
            })
        }


        function callUpdate() {

            $.ajax({
                type: "POST",
                url: "PreAssessment_N.aspx/update",
                contentType: "application/json; charset=utf-8",
                data: '',
                success: function (msg) {

                    Swal.fire({
                        title: 'Updated Successfully',
                        text: "",
                        icon: 'success',
                        showCancelButton: false,
                        confirmButtonText: 'OK!'
                    }).then((result) => {
                        window.location.reload();
                    });



                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest);
                    var json = $.parseJSON(XMLHttpRequest.responseText);
                    console.log(json);
                    Swal.fire(
                        '(' + XMLHttpRequest.status + ' )' + errorThrown,
                        json.Message,
                        'error'
                    )
                }

            });
            var percentage = 99;
            var $el = $('.progress-val')
            $('.progress-meter').stop(true).animate({
                width: percentage + '%'
            }, {
                duration: 95000,
                step: function (now, fx) {
                    if (fx.prop == 'width') { //If you animate more than 1 property
                        var data = Math.round(now);
                        $el.text(data + ' % ');

                    }
                }
            });

        }



        function CheckOne(obj) {
            var grid = obj.parentNode.parentNode.parentNode;
            var inputs = grid.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    if (obj.checked && inputs[i] != obj && inputs[i].checked) {
                        inputs[i].checked = false;
                    }
                }
            }
        }          </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentheading" runat="Server">
    Pre Assessment
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Pre Assessment
        </div>
        <div class="actions">
            .
            <div class="btn-group">

                <li class='<%= Session["roleId"].ToString() != "1" ? "show" : "hide" %>'>
                    <asp:LinkButton runat="server" ID="lnk_Pull_Assets" CssClass="btn btn-theme" OnClientClick="startUpdate();">Update </asp:LinkButton>
                </li>
            </div>
            <div class="btn-group">

                <li >
                    <asp:Button runat="server" ID="btnAdd" CssClass="btn btn-redtheme" Text="Send to RDM" OnClick="btnAdd_Click" />
                </li>
            </div>
        </div>
        <br />
        <p>Click on Update button to get the latest data *</p>

    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="divmsg" class="" runat="server" style="display: none"></div>

            <div>
                <table class="table borderless" style="width: 100% !important; border: none !important;">
                    <tr>
                        <td>Tax Year:</td>

                        <td>
                            <asp:DropDownList ID="txt_tax_year" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="btn_search_Click"></asp:DropDownList>
                        </td>
                        <td>Company RIN:</td>

                        <td>
                            <asp:TextBox ID="txt_cmp_RIN" runat="server" CssClass="form-control" placeholder="Company RIN"></asp:TextBox>

                        </td>
                        <td>Business RIN:</td>

                        <td>
                            <asp:TextBox ID="txt_employer_RIN" runat="server" CssClass="form-control" placeholder="Business RIN"></asp:TextBox>

                        </td>

                        <td colspan="3" style="text-align: right;">
                            <asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click" Visible="true" />

                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>Employer Name:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_employer_name" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>Employer TIN:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_employer_TIN" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td>Business RIN:</td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txt_business_RIN" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>




                </table>
            </div>
            <div>
                <table>

                    <tr>
                        <td>
                            <asp:GridView ID="grdempcollection" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10"
                                AutoGenerateColumns="False" PagerSettings-PageButtonCount="5"
                                CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" OnPageIndexChanging="grdempcollection_PageIndexChanging" OnRowDataBound="grdempcollection_RowDataBound">

                                <Columns>
                                    <asp:BoundField DataField="TaxPayerTypeId" HeaderText="TaxPayerTypeId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="TaxPayerID" HeaderText="TaxPayerId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="AssetTypeId" HeaderText="AssetTypeId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="AssetId" HeaderText="AssetId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="ProfileID" HeaderText="ProfileId" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />

                                    <asp:BoundField DataField="AssessmentItemID" HeaderText="Assessment ItemID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:BoundField DataField="AssessmentRuleID" HeaderText="Assessment RuleID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />



                                    <asp:BoundField DataField="TaxPayerRin" HeaderText="Employer RIN" />
                                    <asp:BoundField DataField="AssetRin" HeaderText="Asset RIN" />
                                    <asp:BoundField DataField="AssetName" HeaderText="Employer Name" />
                                    <%-- <asp:BoundField DataField="AssessmentItemName" HeaderText="Asset (Business)" />--%>

                                    <asp:BoundField DataField="AssessmentRuleName" HeaderText="Rule Name" />
                                    <asp:BoundField DataField="TaxYear" HeaderText="Tax Month/Year" />

                                    <asp:BoundField DataField="TaxBaseAmount" HeaderText="Assessed Amount" />
                                    <asp:TemplateField HeaderText="Send to RDM">
                                        <ItemTemplate>
                                            <%--<asp:CheckBox runat="server" ID="chkrdm" onclick="CheckOne(this)"/>--%>
                                            
                                            <li >
                                            <asp:CheckBox runat="server" ID="chkrdm" />
                                            <asp:Label ID="lbl_status" runat="server"></asp:Label>
                                                </li>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerStyle CssClass="pagination-ys" HorizontalAlign="Right" />

                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

