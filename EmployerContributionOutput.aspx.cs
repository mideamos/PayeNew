using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Security;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using ClosedXML.Excel;
using Spire.Xls;
using Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

using Spire.Pdf.Graphics;
using System.Drawing;
using System.Text;
using Label = Microsoft.Office.Interop.Excel.Label;

using System.Web.UI.HtmlControls;

public partial class EmployerContributionOutput : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnExcel);
        if (!IsPostBack)
        {
            TheMethod();
        }
    }
    protected void TheMethod()
    {
        string val = "";
    
            txt_tax_year.Items.Add("--Select Year--");
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                txt_tax_year.Items.Add(i.ToString());
            }
            System.Data.DataTable dt_list = new System.Data.DataTable();

            //string query1 = "select * from VW_Employer_Contribution where EmployerRIN is not null";
            //string query2 = "select * from PreAssessmentRDM where TaxPayerID is not null";
            //string query3 = "Select Distinct EmployerName, EmployerRIN, Assessment_Year, sum(TaxBaseAmount)amount, TaxPayerRin, AssessmentRuleName from PreAssessmentRDM a join [PayeOuputFile] b on a.TaxPayerRin = b.EmployerRIN group by EmployerName, EmployerRIN, Assessment_Year, TaxPayerRin, AssessmentRuleName order by Assessment_Year";
            string query3 = "Select Distinct b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, AssessmentRuleName from PreAssessmentRDM a join PayeOuputFile b on a.TaxPayerRin = b.EmployerRIN order by a.TaxYear";
            //string query3 = "Select Distinct EmployerName, EmployerRIN, Assessment_Year, TaxBaseAmount, TaxPayerRin, AssessmentRuleName from PreAssessmentRDM a join [PayeOuputFile] b on a.TaxPayerRin = b.EmployerRIN order by Assessment_Year";
            SqlDataAdapter Adp = new SqlDataAdapter(query3, con);
            Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            Adp.Fill(dt_list);
            if (dt_list.Rows.Count == 0)
            {
                string script = "alert('No Record Found');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
                return;
            }



            dt_list.Columns.Add("Jan", typeof(System.Double));
            dt_list.Columns.Add("Feb", typeof(System.Double));
            dt_list.Columns.Add("Mar", typeof(System.Double));
            dt_list.Columns.Add("Apr", typeof(System.Double));
            dt_list.Columns.Add("May", typeof(System.Double));
            dt_list.Columns.Add("Jun", typeof(System.Double));
            dt_list.Columns.Add("Jull", typeof(System.Double));
            dt_list.Columns.Add("Aug", typeof(System.Double));
            dt_list.Columns.Add("Sep", typeof(System.Double));
            dt_list.Columns.Add("Oct", typeof(System.Double));
            dt_list.Columns.Add("Nov", typeof(System.Double));
            dt_list.Columns.Add("Dec", typeof(System.Double));

            //dt_list.Columns.Add("SumMonthlyTax", typeof(System.Double));
            int c = 0;
            if (dt_list.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_list.Rows)
                {
                    string resString = dr["AssessmentRuleName"].ToString().ToLower();
                    double taxBaxAmount = Convert.ToDouble(dr["TaxBaseAmount"]);


                    var taxBaseAmt = CalculateTaxBaseAmount(resString, taxBaxAmount);

                    var sumTaxBaseAmtJan = taxBaseAmt.Jan;
                    var sumTaxBaseAmtFeb = taxBaseAmt.Feb;
                    var sumTaxBaseAmtMar = taxBaseAmt.Mar;
                    var sumTaxBaseAmtApr = taxBaseAmt.Apr;
                    var sumTaxBaseAmtMay = taxBaseAmt.May;
                    var sumTaxBaseAmtJun = taxBaseAmt.Jun;
                    var sumTaxBaseAmtJull = taxBaseAmt.Jull;
                    var sumTaxBaseAmtAug = taxBaseAmt.Aug;
                    var sumTaxBaseAmtSep = taxBaseAmt.Sep;
                    var sumTaxBaseAmtOct = taxBaseAmt.Oct;
                    var sumTaxBaseAmtNov = taxBaseAmt.Nov;
                    var sumTaxBaseAmtDec = taxBaseAmt.Dec;

                    //+ taxBaseAmt.Feb + taxBaseAmt.Mar + taxBaseAmt.Apr + taxBaseAmt.May + taxBaseAmt.May
                    //+ taxBaseAmt.Jun + taxBaseAmt.Jull + taxBaseAmt.Aug + taxBaseAmt.Sep + taxBaseAmt.Oct + taxBaseAmt.Nov + taxBaseAmt.Dec;

                    dr.SetField("Jan", sumTaxBaseAmtJan);
                    dr.SetField("Feb", sumTaxBaseAmtFeb);
                    dr.SetField("Mar", sumTaxBaseAmtMar);
                    dr.SetField("Apr", sumTaxBaseAmtApr);
                    dr.SetField("May", sumTaxBaseAmtMay);
                    dr.SetField("Jun", sumTaxBaseAmtJun);
                    dr.SetField("Jull", sumTaxBaseAmtJull);
                    dr.SetField("Aug", sumTaxBaseAmtAug);
                    dr.SetField("Sep", sumTaxBaseAmtSep);
                    dr.SetField("Oct", sumTaxBaseAmtOct);
                    dr.SetField("Nov", sumTaxBaseAmtNov);
                    dr.SetField("Dec", sumTaxBaseAmtDec);

                    foreach (GridViewRow row in grd_empoyer_contribution.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {


                            string Jan = string.Format("&#8358;{0:N0}", sumTaxBaseAmtJan);
                            row.Cells[4].Text = Jan;


                        }
                    }

                }
            }



            Session["dt_l"] = dt_list;
            grd_empoyer_contribution.DataSource = dt_list;
            grd_empoyer_contribution.DataBind();

            int pagesize = grd_empoyer_contribution.Rows.Count;
            int from_pg = 1;
            int to = grd_empoyer_contribution.Rows.Count;
            int totalcount = dt_list.Rows.Count;
            lblpagefrom.Text = from_pg.ToString();
            lblpageto.Text = (from_pg + pagesize - 1).ToString();
            lbltoal.Text = totalcount.ToString();

            if (totalcount < grd_empoyer_contribution.PageSize)
                div_paging.Style.Add("margin-top", "0px");
            else
                div_paging.Style.Add("margin-top", "-60px");
        
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grd_empoyer_contribution.PageIndex = e.NewPageIndex;
        grd_empoyer_contribution.DataSource = Session["dt_l"];

        grd_empoyer_contribution.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom.Text = "1";
        }
        else
        {
            lblpagefrom.Text = ((grd_empoyer_contribution.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto.Text = ((e.NewPageIndex + 1) * grd_empoyer_contribution.Rows.Count).ToString();

    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        ExportToPDF2();
        

        //HtmlContainerControl modalinfo = (HtmlContainerControl)this.Master.FindControl("modalinfo");
        //Label lblmodalbody = (Label)this.Master.FindControl("lblmodalbody");

        //if (grd_empoyer_contribution.Rows.Count > 0)
        //{
        //    string attachment = "attachment; filename=EmployerCollection.xls";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    string tab = "";
        //    if (grd_empoyer_contribution.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
        //        {
        //            Response.Write(tab + grd_empoyer_contribution.Columns[i].HeaderText);
        //            tab = "\t";
        //        }

        //        Response.Write("\n");

        //        foreach (GridViewRow dr in grd_empoyer_contribution.Rows)
        //        {
        //            tab = "";
        //            for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
        //            {
        //                Response.Write(tab + dr.Cells[i].Text);
        //                tab = "\t";
        //            }
        //            Response.Write("\n");
        //        }
        //        Response.End();
        //    }
        //}
        //else
        //{

        //    if (grd_empoyer_contribution.Rows.Count < 0)
        //    {
        //        modalinfo.Attributes.Add("class", "modal show");
        //        lblmodalbody.Text = "You have Nothing to Download";
        //        return;
        //    }
        //}




        //try
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = new
        //    RemoteCertificateValidationCallback
        //    (
        //          delegate { return true; }
        //    );
        //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showImage()", true);
        //    System.Data.DataTable dt_list = (System.Data.DataTable)(Session["dt_l"]);

        //    grd_empoyer_contribution.Visible = false;
        //    grd_empoyer_contribution.AllowPaging = false;

        //    System.Data.DataTable dt_filtered = new System.Data.DataTable();
        //    DataView dt_v = dt_list.DefaultView;
        //    if (txt_employer_RIN.Text != "")
        //    {
        //        dt_v.RowFilter = "EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%'";

        //        if (txt_tax_year.SelectedItem.Text != "--Select Year--")
        //            dt_v.RowFilter = "(EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%') and (Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%')";


        //    }
        //    if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
        //        dt_v.RowFilter = "Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%'";

        //    grd_empoyer_contribution.DataSource = dt_v;

        //    grd_empoyer_contribution.DataBind();



        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
        //    {
        //        dt.Columns.Add(grd_empoyer_contribution.HeaderRow.Cells[i].Text + "");
        //    }

        //    foreach (GridViewRow row in grd_empoyer_contribution.Rows)
        //    {
        //        DataRow dr = dt.NewRow();
        //        for (int j = 0; j < grd_empoyer_contribution.Columns.Count; j++)
        //        {
        //            dr[grd_empoyer_contribution.HeaderRow.Cells[j].Text] = row.Cells[j].Text;
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    MemoryStream memory = PAYEClass.DataTableToExcelXlsx(dt, "EmpoyerContribution");
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=EmpoyerContribution.xlsx");
        //    memory.WriteTo(Response.OutputStream);
        //    Response.StatusCode = 200;
        //    Response.End();

        //    grd_empoyer_contribution.AllowPaging = true;
        //    grd_empoyer_contribution.DataSource = (System.Data.DataTable)(Session["dt_l"]);

        //    grd_empoyer_contribution.DataBind();
        //}
        //catch (Exception exc)
        //{

        //}
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);

    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {


        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new
RemoteCertificateValidationCallback
(
  delegate { return true; }
); ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showImage()", true);
            System.Data.DataTable dt_list = (System.Data.DataTable)(Session["dt_l"]);

            grd_empoyer_contribution.Visible = false;
            grd_empoyer_contribution.AllowPaging = false;

            System.Data.DataTable dt_filtered = new System.Data.DataTable();
            DataView dt_v = dt_list.DefaultView;
            if (txt_employer_RIN.Text != "")
            {
                dt_v.RowFilter = "EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%'";

                if (txt_tax_year.SelectedItem.Text != "--Select Year--")
                    dt_v.RowFilter = "(EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%') and (Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%')";


            }
            if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
                dt_v.RowFilter = "Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%'";

            grd_empoyer_contribution.DataSource = dt_v;

            grd_empoyer_contribution.DataBind();
            grd_empoyer_contribution.Style.Add("font-weight", "200");
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=EmployerCollection.pdf");

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter stringWriter = new StringWriter();
            iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(19);

            iTextSharp.text.Font brown = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9f, iTextSharp.text.Font.NORMAL);
            //Set the column widths 
            int[] widths = { 5, 15, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
            for (int x = 0; x < grd_empoyer_contribution.Columns.Count; x++)
            {
                try
                {

                    string cellText = grd_empoyer_contribution.HeaderRow.Cells[x].Text;
                    PdfPCell theCell = new PdfPCell(new Paragraph(cellText, brown));
                    table.AddCell(theCell);
                }
                catch (Exception ext)
                {
                    string script = "alert('Something Went Wrong With the download!!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
                    return;

                }

            }
            table.CompleteRow();
            table.SetWidths(widths);



            //Transfer rows from GridView to table
            for (int i = 0; i < grd_empoyer_contribution.Rows.Count; i++)
            {
                string cellText2 = (i + 1) + "";


                PdfPCell theCell2 = new PdfPCell(new Paragraph(cellText2, brown));
                table.AddCell(theCell2);
                for (int j = 1; j < grd_empoyer_contribution.Columns.Count; j++)
                {

                    string cellText = grd_empoyer_contribution.Rows[i].Cells[j].Text;


                    PdfPCell theCell = new PdfPCell(new Paragraph(cellText, brown));
                    table.AddCell(theCell);
                }
                table.CompleteRow();

            }



            var style = new StyleSheet();
            style.LoadTagStyle("body", "size", "5px");
            table.WidthPercentage = 100;
            Document Doc = new Document(PageSize.A4, 20, 13, 20, 0);
            Doc.SetPageSize(PageSize.A4.Rotate());
            //Document Doc = new Document(new Rectangle(1000f, 1000f));
            PdfWriter.GetInstance(Doc, Response.OutputStream);
            Doc.Open();

            Doc.Add(table);
            Doc.Close();

            Response.Write(Doc);
         
            grd_empoyer_contribution.AllowPaging = true;
            grd_empoyer_contribution.DataSource = (System.Data.DataTable)(Session["dt_l"]);

            grd_empoyer_contribution.DataBind();
        }
        catch (Exception e1)
        {
            string script = "alert('Something Went Wrong With the download!!', '"+ e1 + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);
    }

    private int ExportToPDF2()
    {
        if (grd_empoyer_contribution.Rows.Count > 0)
        {
            string attachment = "attachment; filename=EmployerCollection.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            string tab = "";
            if (grd_empoyer_contribution.Rows.Count > 0)
            {

                for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
                {
                    Response.Write(tab + grd_empoyer_contribution.Columns[i].HeaderText);
                    tab = "\t";
                }

                Response.Write("\n");

                foreach (GridViewRow dr in grd_empoyer_contribution.Rows)
                {
                    tab = "";
                    for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
                    {
                        Response.Write(tab + dr.Cells[i].Text);
                        tab = "\t";
                    }
                    Response.Write("\n");
                }
            
                Response.End();
                TheMethod();

            }
        }
        return 0;
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        System.Data.DataTable dt_list_s = new System.Data.DataTable();
        dt_list_s = (System.Data.DataTable)Session["dt_l"];
        // DataRow[] filteredRows = dt_list_s.Select("TaxPayerRIN LIKE '" + txt_RIN.Text + "'");
        System.Data.DataTable dt_filtered = new System.Data.DataTable();
        DataView dt_v = dt_list_s.DefaultView;
        if (txt_employer_RIN.Text != "")
        {
            dt_v.RowFilter = "EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or TaxYear like '%" + txt_employer_RIN.Text + "%'";

            if (txt_tax_year.SelectedItem.Text != "--Select Year--")
                dt_v.RowFilter = "(EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or TaxYear like '%" + txt_employer_RIN.Text + "%') and (TaxYear like '%" + txt_tax_year.SelectedItem.Text + "%')";


        }

        if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
        {
            dt_v.RowFilter = "TaxYear like '%" + txt_tax_year.SelectedItem.Text + "%'";
            if (dt_v.RowFilter.Length == 0)
            {
                string script = "alert('No Record Found');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
                return;
            }
        }
 

        grd_empoyer_contribution.DataSource = dt_v;
        grd_empoyer_contribution.DataBind();

        int pagesize = grd_empoyer_contribution.Rows.Count;
        int from_pg = 1;
        int to = grd_empoyer_contribution.Rows.Count;
        int totalcount = dt_v.Count;
        lblpagefrom.Text = from_pg.ToString();
        lblpageto.Text = (from_pg + pagesize - 1).ToString();
        lbltoal.Text = totalcount.ToString();

        if (totalcount < grd_empoyer_contribution.PageSize)
            div_paging.Style.Add("margin-top", "0px");
        else
            div_paging.Style.Add("margin-top", "-60px");
    }

    public static MonthListValue CalculateTaxBaseAmount(string taxBaseAm, double monthlyTax)
    {
        taxBaseAm = taxBaseAm.ToLower();
        if (taxBaseAm.Contains("january"))
            return new MonthListValue { Jan = monthlyTax, Feb = monthlyTax, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("february"))
            return new MonthListValue { Jan = 0, Feb = monthlyTax, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("march"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("april"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("may"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("june"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("july"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("august"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("september"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("october"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("november"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = monthlyTax, Dec = monthlyTax };
        else if (taxBaseAm.Contains("december"))
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = 0, Dec = monthlyTax };
        else
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = 0, Dec = 0 };
    }

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    if (fileUpload.HasFile)
    //    {
    //        // Get the uploaded file
    //        HttpPostedFile uploadedFile = fileUpload.PostedFile;

    //        // Generate the PDF
    //        byte[] pdfBytes = GeneratePdfFromPage(uploadedFile);

    //        // Return the PDF as a file in the web form
    //        Response.Clear();
    //        Response.ContentType = "application/pdf";
    //        Response.AppendHeader("Content-Disposition", "attachment; filename=generated.pdf");
    //        Response.BinaryWrite(pdfBytes);
    //        Response.End();
    //    }
    //}

    //private byte[] GeneratePdfFromPage(HttpPostedFile uploadedFile)
    //{
    //    // Read the uploaded file data
    //    byte[] fileData;
    //    using (BinaryReader reader = new BinaryReader(uploadedFile.InputStream))
    //    {
    //        fileData = reader.ReadBytes(uploadedFile.ContentLength);
    //    }

    //    // Create a new PDF document
    //    Document document = new Document();

    //    // Create a new MemoryStream to write the PDF content
    //    MemoryStream memoryStream = new MemoryStream();

    //    // Create a new PdfWriter to write the PDF document to the MemoryStream
    //    PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

    //    // Open the PDF document
    //    document.Open();

    //    // Add the content from the uploaded file to the PDF document
    //    PdfContentByte pdfContent = writer.DirectContent;
    //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(fileData);
    //    image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
    //    pdfContent.AddImage(image);

    //    // Close the PDF document
    //    document.Close();

    //    // Get the PDF bytes from the MemoryStream
    //    byte[] pdfBytes = memoryStream.ToArray();

    //    // Clean up resources
    //    memoryStream.Dispose();
    //    writer.Dispose();
    //    document.Dispose();

    //    return pdfBytes;
    //}

    public class MonthListValue
    {
        public double Jan { get; set; }
        public double Feb { get; set; }
        public double Mar { get; set; }
        public double Apr { get; set; }
        public double May { get; set; }
        public double Jun { get; set; }
        public double Jull { get; set; }
        public double Aug { get; set; }
        public double Sep { get; set; }
        public double Oct { get; set; }
        public double Nov { get; set; }
        public double Dec { get; set; }
    }





}