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
using DocumentFormat.OpenXml.Wordprocessing;
using System.Activities.Statements;


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
        //string query3 = "Select Distinct b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, AssessmentRuleName from PreAssessmentRDM a join PayeOuputFile b on a.TaxPayerRin = b.EmployerRIN order by a.TaxYear";
        //string query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN ) AS subquery WHERE rn = 1 ORDER BY TaxYear";

        //string query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";
        string query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";

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



        dt_list.Columns.Add("Jan", typeof(System.String));
        dt_list.Columns.Add("Feb", typeof(System.String));
        dt_list.Columns.Add("Mar", typeof(System.String));
        dt_list.Columns.Add("Apr", typeof(System.String));
        dt_list.Columns.Add("May", typeof(System.String));
        dt_list.Columns.Add("Jun", typeof(System.String));
        dt_list.Columns.Add("Jull", typeof(System.String));
        dt_list.Columns.Add("Aug", typeof(System.String));
        dt_list.Columns.Add("Sep", typeof(System.String));
        dt_list.Columns.Add("Oct", typeof(System.String));
        dt_list.Columns.Add("Nov", typeof(System.String));
        dt_list.Columns.Add("Dec", typeof(System.String));

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

                dr.SetField("Jan", sumTaxBaseAmtJan.ToString());
                dr.SetField("Feb", sumTaxBaseAmtFeb.ToString());
                dr.SetField("Mar", sumTaxBaseAmtMar.ToString());
                dr.SetField("Apr", sumTaxBaseAmtApr.ToString());
                dr.SetField("May", sumTaxBaseAmtMay.ToString());
                dr.SetField("Jun", sumTaxBaseAmtJun.ToString());
                dr.SetField("Jull", sumTaxBaseAmtJull.ToString());
                dr.SetField("Aug", sumTaxBaseAmtAug.ToString());
                dr.SetField("Sep", sumTaxBaseAmtSep.ToString());
                dr.SetField("Oct", sumTaxBaseAmtOct.ToString());
                dr.SetField("Nov", sumTaxBaseAmtNov.ToString());
                dr.SetField("Dec", sumTaxBaseAmtDec.ToString());

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


        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
                  delegate { return true; }
            );
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showImage()", true);
            System.Data.DataTable dt_list = (System.Data.DataTable)(Session["dt_l"]);

            grd_empoyer_contribution.Visible = false;
            grd_empoyer_contribution.AllowPaging = false;

            System.Data.DataTable dt_filtered = new System.Data.DataTable();
            DataView dt_v = dt_list.DefaultView;
            if (txt_employer_RIN.Text != "")
            {
                var EmpRin = txt_employer_RIN.Text;
                if(EmpRin.Contains("cmp") == true || EmpRin.Contains("CMP") == true || EmpRin.Contains("Cmp") == true)
                {
                    dt_v.RowFilter = "EmployerRIN like '%" + txt_employer_RIN.Text + "%'";

                }
                else
                {
                    dt_v.RowFilter = "EmployerName like '%" + txt_employer_RIN.Text + "%' or TaxYear like '%" + txt_employer_RIN.Text + "%' or AssetRin like '%" + txt_employer_RIN.Text + "%'";

                }


                if (txt_tax_year.SelectedItem.Text != "--Select Year--")
                    dt_v.RowFilter = " (EmployerRIN like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or TaxYear like '%" + txt_employer_RIN.Text + "%') and (AssetRin like '%" + txt_tax_year.SelectedItem.Text + "%')";


            }
            if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
                dt_v.RowFilter = "TaxYear like '%" + txt_tax_year.SelectedItem.Text + "%'";

            grd_empoyer_contribution.DataSource = dt_v;

            grd_empoyer_contribution.DataBind();



            System.Data.DataTable dt = new System.Data.DataTable();
            for (int i = 0; i < grd_empoyer_contribution.Columns.Count; i++)
            {
                dt.Columns.Add(grd_empoyer_contribution.HeaderRow.Cells[i].Text + "");
            }

            foreach (GridViewRow row in grd_empoyer_contribution.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < grd_empoyer_contribution.Columns.Count; j++)
                {
                    dr[grd_empoyer_contribution.HeaderRow.Cells[j].Text] = row.Cells[j].Text;
                }
                dt.Rows.Add(dr);
            }
            MemoryStream memory = PAYEClass.DataTableToExcelXlsx(dt, "EmployerCollection");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=EmployerCollection.xlsx");
            memory.WriteTo(Response.OutputStream);
            Response.StatusCode = 200;
            Response.Flush();
            Response.End();

            grd_empoyer_contribution.AllowPaging = true;
            grd_empoyer_contribution.DataSource = (System.Data.DataTable)(Session["dt_l"]);

            grd_empoyer_contribution.DataBind();
        }
        catch (Exception exc)
        {

        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);

    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        //Ser

        //string filePath = Server.MapPath("~/EmployerContributionOutput.aspx"); // Provide the physical file path
        //string fileName = "file.pdf"; // Provide the desired file name

        //if (File.Exists(filePath))
        //{
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        //    Response.TransmitFile(filePath);
        //    Response.End();
        //}
        //else
        //{
        //    // Handle file not found or other errors
        //    Response.Write("File not found");
        //}



        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
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

            // Create the PDF document
            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 13, 20, 0);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();
            StringWriter stringWriter = new StringWriter();

            iTextSharp.text.Font brown = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9f, iTextSharp.text.Font.NORMAL);

            // Create the PDF table
            int[] widths = { 5, 15, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
            float[] columnWidths = Array.ConvertAll(widths, item => (float)item);
            PdfPTable table = new PdfPTable(columnWidths);
            table.WidthPercentage = 100;

            // Add table headers
            for (int x = 0; x < grd_empoyer_contribution.Columns.Count; x++)
            {
                string cellText = grd_empoyer_contribution.HeaderRow.Cells[x].Text;
                PdfPCell headerCell = new PdfPCell(new Phrase(cellText, brown));
                table.AddCell(headerCell);
            }

            // Add table rows
            for (int i = 0; i < grd_empoyer_contribution.Rows.Count; i++)
            {
                string cellText2 = (i + 1).ToString();
                PdfPCell numberCell = new PdfPCell(new Phrase(cellText2, brown));
                table.AddCell(numberCell);

                for (int j = 1; j < grd_empoyer_contribution.Columns.Count; j++)
                {
                    string cellText = grd_empoyer_contribution.Rows[i].Cells[j].Text;
                    PdfPCell dataCell = new PdfPCell(new Phrase(cellText, brown));
                    table.AddCell(dataCell);
                }
            }
            HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

            var style = new StyleSheet();
            style.LoadTagStyle("body", "size", "5px");
            table.WidthPercentage = 100;
            // main.RenderControl(htmlTextWriter);
            StringReader stringReader = new StringReader(stringWriter.ToString());
            iTextSharp.text.Document Doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 20, 13, 20, 0);
            Doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            HTMLWorker htmlparser = new HTMLWorker(Doc);
            // Add the table to the document
            doc.Add(table);

            htmlparser.SetStyleSheet(style);

            htmlparser.Parse(stringReader);
            doc.Close();

            // Set the response headers for downloading the PDF file
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=EmployerCollection.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Write the PDF file to the response stream
            Response.BinaryWrite(memoryStream.ToArray());
            HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();
            ////Response.End();

            Response.Write(doc);
            // Restore the gridview settings
            grd_empoyer_contribution.AllowPaging = true;
            grd_empoyer_contribution.DataSource = (System.Data.DataTable)(Session["dt_l"]);
            grd_empoyer_contribution.DataBind();


        }
        catch (Exception e1)
        {
            //string script = "alert('Something Went Wrong With the download!!', '" + e1 + "');";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            //return;
        }
        string script = string.Empty;
        if (grd_empoyer_contribution.Rows.Count > 0)
        {
            script = "alert('Download Completed!');";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);
            return; ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; }); ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "showImage()", true);
        }
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

                //Response.End();

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






    protected void Button_Search(object sender, EventArgs e)
    {
        string val = "";

        txt_tax_year.Items.Add("--Select Year--");
        for (int i = DateTime.Now.Year; i >= 2014; i--)
        {
            txt_tax_year.Items.Add(i.ToString());
        }
        System.Data.DataTable dt_list = new System.Data.DataTable();
        var CompanyRin = txt_employer_RIN.Text;
        var BusinessRin = txt_asset_RIN.Text;
        string query3 = "";
        if (CompanyRin != null)
        {

            if (CompanyRin.Contains("CMP") == true || CompanyRin.Contains("cmp") == true)
            {
                //query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN  ) AS subquery WHERE rn = 1 and EmployerRIN =  '" + CompanyRin + "' ORDER BY TaxYear";
                query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID WHERE  b.EmployerRIN = '" + CompanyRin + "' GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";

            }
            else if (CompanyRin.Contains("for") == true || CompanyRin.Contains("FOR") == true)
            {
                //query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN  ) AS subquery WHERE rn = 1 and AssetRin = '"+ CompanyRin + "' ORDER BY TaxYear";
                query3 ="SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID WHERE a.AssetRin = '" + CompanyRin + "' GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";

            }
        }

        if (BusinessRin != null)
        {
            if (BusinessRin.Contains("CMP") == true || BusinessRin.Contains("cmp") == true)
            {
                //query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN  ) AS subquery WHERE rn = 1 and EmployerRIN =  '" + BusinessRin + "' ORDER BY TaxYear";
                query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID WHERE  b.EmployerRIN = '" + BusinessRin + "' GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";

            }
            else if (BusinessRin.Length > 0 && CompanyRin.Length > 0)
            {
                query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID WHERE a.AssetRin = '" + BusinessRin + "' and b.EmployerRIN = '" + CompanyRin + "' GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";
                //query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN  ) AS subquery WHERE rn = 1 and AssetRin = '"+ BusinessRin + "' and EmployerRIN = '"+ CompanyRin + "' ORDER BY TaxYear";
            }
            else if (BusinessRin.Contains("for") == true || BusinessRin.Contains("FOR") == true)
            {
                //query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN  ) AS subquery WHERE rn = 1 and AssetRin = '"+ BusinessRin + "' ORDER BY TaxYear";
                query3 = "SELECT EmployerName, EmployerRIN, TaxYear, TaxBaseAmount, AssetRin, AssessmentRuleName, CountEmployeeRin, Taxoffice FROM (SELECT b.EmployerName, b.EmployerRIN, a.TaxYear, COUNT(b.EmployeeRIN) AS CountEmployeeRin, a.TaxBaseAmount, c.TaxOffice, a.AssetRin, a.AssessmentRuleName, ROW_NUMBER() OVER (PARTITION BY b.EmployerName, a.TaxYear ORDER BY a.TaxYear) AS rn FROM PreAssessmentRDM a JOIN PayeOuputFile b ON a.TaxPayerRin = b.EmployerRIN left join companylist_api c on a.TaxPayerID = c.TaxPayerID and c.TaxPayerTypeID = a.TaxPayerTypeID WHERE a.AssetRin = '" + BusinessRin + "' GROUP BY b.EmployerName, b.EmployerRIN, a.TaxYear, a.TaxBaseAmount, a.AssetRin, c.TaxOffice, a.AssessmentRuleName) AS subquery WHERE rn = 1 ORDER BY TaxYear";

            }
        }



        SqlDataAdapter Adp = new SqlDataAdapter(query3, con);
        Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp.Fill(dt_list);
        if (dt_list.Rows.Count == 0)
        {
            string script = "alert('No Record Found');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }



        dt_list.Columns.Add("Jan", typeof(System.String));
        dt_list.Columns.Add("Feb", typeof(System.String));
        dt_list.Columns.Add("Mar", typeof(System.String));
        dt_list.Columns.Add("Apr", typeof(System.String));
        dt_list.Columns.Add("May", typeof(System.String));
        dt_list.Columns.Add("Jun", typeof(System.String));
        dt_list.Columns.Add("Jull", typeof(System.String));
        dt_list.Columns.Add("Aug", typeof(System.String));
        dt_list.Columns.Add("Sep", typeof(System.String));
        dt_list.Columns.Add("Oct", typeof(System.String));
        dt_list.Columns.Add("Nov", typeof(System.String));
        dt_list.Columns.Add("Dec", typeof(System.String));

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

                dr.SetField("Jan", sumTaxBaseAmtJan.ToString());
                dr.SetField("Feb", sumTaxBaseAmtFeb.ToString());
                dr.SetField("Mar", sumTaxBaseAmtMar.ToString());
                dr.SetField("Apr", sumTaxBaseAmtApr.ToString());
                dr.SetField("May", sumTaxBaseAmtMay.ToString());
                dr.SetField("Jun", sumTaxBaseAmtJun.ToString());
                dr.SetField("Jull", sumTaxBaseAmtJull.ToString());
                dr.SetField("Aug", sumTaxBaseAmtAug.ToString());
                dr.SetField("Sep", sumTaxBaseAmtSep.ToString());
                dr.SetField("Oct", sumTaxBaseAmtOct.ToString());
                dr.SetField("Nov", sumTaxBaseAmtNov.ToString());
                dr.SetField("Dec", sumTaxBaseAmtDec.ToString());

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
}