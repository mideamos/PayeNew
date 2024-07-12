<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EmployeeContributionOutput.aspx.cs" Inherits="EmployeeContributionOutput" %>

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
    Employee Contribution Output File
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="portlet-title">
        <div class="caption">
            Search Employee Data
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
      
                <div id="div_loading" runat="server" style="display: none; margin-top: 25%; margin-left: 35%; align-items: center; position: fixed;">
                    <img id="img_load" runat="server" src="~/images/Pulsating circle.gif" />
                    <p id="sync_data" runat="server">please wait processing  your request</p>
                </div>
                <div>
                    <table class="table borderless" style="width: 100% !important; border: none !important;">

                        <tr>
                            <td>Tax Year:</td>
                            <td></td>
                            <td>
                                <asp:DropDownList ID="txt_tax_year" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="btn_search_Click"></asp:DropDownList></td>
                            <%--</tr>
                <tr>--%>
                            <td>Search:</td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_employer_RIN" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="btn_search_Click"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPDF" Text="Save AS PDF" CssClass="btn btn-theme" runat="server" OnClick="btnPDF_Click" />

                            </td>
                            <td>
                                <asp:Button ID="btnExcel" Text="Save AS Excel" CssClass="btn btn-theme" runat="server" OnClick="btnExcel_Click" />

                            </td>

                        </tr>
                        <tr style="display: none;">
                            <td>Employee RIN:</td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_tax_payer_RIN" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>

                        <tr style="display: none;">
                            <td>Employee Name:</td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_tax_payer_name" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr style="display: none;">
                            <td>Rule Code:</td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_rule_code" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr style="display: none;">
                            <td>Rule Name:</td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_rule_name" runat="server" CssClass="form-control"></asp:TextBox></td>
                        </tr>

                        <tr style="display: none;">
                            <td colspan="3" style="text-align: right;">
                                <asp:Button ID="btn_search" Text="Search" runat="server" CssClass="btn btn-theme" OnClick="btn_search_Click" /></td>
                        </tr>

                    </table>
                </div>



                <div class="portlet light">
                    <div class="portlet-title">
                        <div class="caption">Output File</div>

                    </div>
                    <div style="overflow: scroll;">
                        <asp:GridView ID="grd_empoyee_contribution" runat="server" AllowPaging="True" AllowSorting="True" PageSize="10" PagerSettings-PageButtonCount="5"
                            AutoGenerateColumns="False" OnPageIndexChanging="GridView1_PageIndexChanging" CssClass="table table-striped table-bordered table-hover" HeaderStyle-CssClass="GridHeader" ShowFooter="false">
                            <Columns>
                                <asp:BoundField DataField="EmployeeRIN" HeaderText="Employee RIN" />
                                <asp:BoundField DataField="Employeename" HeaderText="Employee Name" />
                                <asp:BoundField DataField="Assessment_year" HeaderText="Assessment Year" />
                                <asp:BoundField DataField="EmployerName" HeaderText="Employer Name" />
                                <asp:BoundField DataField="EmployerRIN" HeaderText="Employer RIN" />
                                <asp:BoundField DataField="AssetRin" HeaderText="Business RIN" />
                                <asp:BoundField DataField="Jan" HeaderText="Jan Contribution" />
                                <asp:BoundField DataField="Feb" HeaderText="Feb Contribution" />
                                <asp:BoundField DataField="Mar" HeaderText="Mar Contribution" />
                                <asp:BoundField DataField="Apr" HeaderText="Apr Contribution" />
                                <asp:BoundField DataField="May" HeaderText="May Contribution" />
                                <asp:BoundField DataField="Jun" HeaderText="Jun Contribution" />
                                <asp:BoundField DataField="Jull" HeaderText="Jul Contribution" />
                                <asp:BoundField DataField="Aug" HeaderText="Aug Contribution" />
                                <asp:BoundField DataField="Sep" HeaderText="Sep Contribution" />
                                <asp:BoundField DataField="Oct" HeaderText="Oct Contribution" />
                                <asp:BoundField DataField="Nov" HeaderText="Nov Contribution" />
                                <asp:BoundField DataField="Dec" HeaderText="Dec Contribution" />
                                <%--                <asp:ButtonField ButtonType="Button" Text="Download" CommandName="DownloadPDF" />--%>
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

                </div>

                <div class="modal fade" id="divTaxPayerModal" tabindex="-1" role="dialog" aria-labelledby="divTaxPayerModalLabel" style="display: none;">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                                <h4 class="modal-title" id="divTaxPayerModalLabel">Assessment - Rules Check Details</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label control-label-static bold">Tax Payer RIN </label>
                                            <div id="tax_payer_rin"></div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label control-label-static bold">Tax Payer Name</label>
                                            <div id="tax_payer_name"></div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label control-label-static bold">Assessment Rule</label>
                                            <div id="tax_rule"></div>
                                        </div>
                                    </div>

                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label control-label-static bold">Assessed</label>
                                            <div id="tax_assessed"></div>
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--        <script>
                $(document).ready(function () {
                    $('#<%= grd_empoyee_contribution.ClientID %> tbody').on('click', 'tr button[data-commandname="DownloadPDF"]', function () {
                        var rowIndex = $(this).closest('tr').index();

                        // Get the PDF URL from the GridView row
                        var pdfUrl = $('#<%= grd_empoyee_contribution.ClientID %> tr:eq(' + (rowIndex + 1) + ') td:eq(1)').text();

                        // Trigger the download by redirecting to the PDF URL
                        window.location.href = pdfUrl;
                    });
                });

            </script>--%>
            
        <script type="text/javascript">
            $(window).bind('beforeunload', function () {
                $(".loader").fadeIn("slow");
            });

            function codeAddress() {
                $(".loader").fadeOut("slow");
            }

            window.onload = codeAddress;

            var currentPath = "<%= Request.Url.AbsolutePath %>";
            var targetPaths = ["/EmployeeContributionOutput.aspx"];
            if (targetPaths.map(function (path) { return path.toLowerCase(); }).includes(currentPath.toLowerCase())) {
                $('style').remove();
            }

            setTimeout(function () {
                if (!hasReloaded) {
                    location.reload();
                    hasReloaded = true;
                    return result; // Return the result after the page reloads
                }
            }, 60000);

            // Bind the reloadPage function to the btnExcel button click event
            $(".ContentPlaceHolder1_btnExcel").click(function () {
                if (targetPaths.includes(currentPath.toLowerCase())) {
                    setTimeout(reloadPage, 10000); // Reload the page after 10 seconds
                }
            });
        </script>

      
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modelpops" runat="Server">
</asp:Content>

