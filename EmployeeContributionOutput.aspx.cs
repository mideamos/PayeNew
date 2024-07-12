using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net.Security;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Web.UI.HtmlControls;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Xml;

public partial class EmployeeContributionOutput : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.btnExcel);
        string val = "";
        if (!IsPostBack)
        {
            txt_tax_year.Items.Add("--Select Year--");
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                txt_tax_year.Items.Add(i.ToString());
            }

            DataTable dt_list = new DataTable();
            //string query1 = "select distinct *, (FirstName+' '+middlename+' '+Surname) as EmployeeName from vw_employeecontributionoutputfile where EmployeRIN is not null and EmployerRIN in (select EmployerRIN from PayeOuputFile where status=3)";
            string query2 = "Select StartMonth, MonthlyTax, EmployerName, EmployerRIN, EmployeeRIN,AssetRin, Assessment_Year, FirstName + ' ' + MiddleName + ' ' + Surname EmployeeName  from [PayeOuputFile] where Status = 3 ";
            SqlDataAdapter Adp = new SqlDataAdapter(query2, con);
            Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            Adp.Fill(dt_list);
            var counte = dt_list.Rows.Count;

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

            MonthListValue result;
            foreach (DataRow dr in dt_list.Rows)
            {
                string startMonth = dr["StartMonth"].ToString().ToLower();
                double monthlyTax = Convert.ToDouble(dr["MonthlyTax"]);
                 result =  CalculateTaxMonths(startMonth,  monthlyTax);

                dr.SetField("Jan", result.Jan);
                dr.SetField("Feb", result.Feb);
                dr.SetField("Mar", result.Mar);
                dr.SetField("Apr", result.Apr);
                dr.SetField("May", result.May);
                dr.SetField("Jun", result.Jun);
                dr.SetField("Jull", result.Jull);
                dr.SetField("Aug", result.Aug);
                dr.SetField("Sep", result.Sep);
                dr.SetField("Oct", result.Oct);
                dr.SetField("Nov", result.Nov);
                dr.SetField("Dec", result.Dec);
            }

            Session["dt_l"] = dt_list;
            grd_empoyee_contribution.DataSource = dt_list;
            grd_empoyee_contribution.DataBind();

            int pagesize = grd_empoyee_contribution.Rows.Count;
            int from_pg = 1;
            int to = grd_empoyee_contribution.Rows.Count;
            int totalcount = dt_list.Rows.Count;
            lblpagefrom.Text = from_pg.ToString();
            lblpageto.Text = (from_pg + pagesize - 1).ToString();
            lbltoal.Text = totalcount.ToString();

            if (totalcount < grd_empoyee_contribution.PageSize)
                div_paging.Style.Add("margin-top", "0px");
            else
                div_paging.Style.Add("margin-top", "-60px");
        }
        //BindGridView();
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //HtmlContainerControl modalinfo = (HtmlContainerControl)this.Master.FindControl("modalinfo");
        //Label lblmodalbody = (Label)this.Master.FindControl("lblmodalbody");

        //if (grd_empoyee_contribution.Rows.Count > 0)
        //{
        //    string attachment = "attachment; filename=EmployerContribution.xls";
        //    Response.ClearContent();
        //    Response.AddHeader("content-disposition", attachment);
        //    string tab = "";
        //    if (grd_empoyee_contribution.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < grd_empoyee_contribution.Columns.Count; i++)
        //        {
        //            Response.Write(tab + grd_empoyee_contribution.Columns[i].HeaderText);
        //            tab = "\t";
        //        }

        //        Response.Write("\n");

        //        foreach (GridViewRow dr in grd_empoyee_contribution.Rows)
        //        {
        //            tab = "";
        //            for (int i = 0; i < grd_empoyee_contribution.Columns.Count; i++)
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

        //    if (grd_empoyee_contribution.Rows.Count < 0)
        //    {
        //        modalinfo.Attributes.Add("class", "modal show");
        //        lblmodalbody.Text = "You have Nothing to Download";
        //        return;
        //    }
        //}


        try
        {
            ServicePointManager.ServerCertificateValidationCallback = new
            RemoteCertificateValidationCallback
            (
              delegate { return true; }
            );

            div_loading.Attributes.Add("display", "block");
            DataTable dt_list = (DataTable)(Session["dt_l"]);

            grd_empoyee_contribution.Visible = false;
            grd_empoyee_contribution.AllowPaging = false;

            DataTable dt_filtered = new DataTable();
            DataView dt_v = dt_list.DefaultView;
            if (txt_employer_RIN.Text != "")
            {
                dt_v.RowFilter = "employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%'";

                if (txt_tax_year.SelectedItem.Text != "--Select Year--")
                    dt_v.RowFilter = "(employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%') and (Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%')";


            }
            if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
                dt_v.RowFilter = "Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%'";

            grd_empoyee_contribution.DataSource = dt_v;

            grd_empoyee_contribution.DataBind();



            DataTable dt = new DataTable();
            for (int i = 0; i < grd_empoyee_contribution.Columns.Count; i++)
            {
                dt.Columns.Add(grd_empoyee_contribution.HeaderRow.Cells[i].Text + "");
            }

            foreach (GridViewRow row in grd_empoyee_contribution.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < grd_empoyee_contribution.Columns.Count; j++)
                {
                    if (row.Cells[j].Text == "&nbsp;")
                    {
                        row.Cells[j].Text = "";
                    }
                    dr[grd_empoyee_contribution.HeaderRow.Cells[j].Text] = row.Cells[j].Text;
                }
                dt.Rows.Add(dr);
            }

            MemoryStream memory = PAYEClass.DataTableToExcelXlsx(dt, "EmpoyeeContribution");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment;filename=EmpoyeeContribution.xlsx");
            memory.WriteTo(Response.OutputStream);
            Response.StatusCode = 200;
            Response.End();

            grd_empoyee_contribution.AllowPaging = true;
            grd_empoyee_contribution.DataSource = (DataTable)(Session["dt_l"]);

            grd_empoyee_contribution.DataBind();
        }
        catch (Exception exc)
        {

        }
        div_loading.Attributes.Add("display", "none");
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

        //    div_loading.Attributes.Add("display", "block");
        //    DataTable dt_list = (DataTable)(Session["dt_l"]);

        //    grd_empoyee_contribution.Visible = false;
        //    grd_empoyee_contribution.AllowPaging = false;

        //    DataTable dt_filtered = new DataTable();
        //    DataView dt_v = dt_list.DefaultView;
        //    if (txt_employer_RIN.Text != "")
        //    {
        //        dt_v.RowFilter = "employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%'";

        //        if (txt_tax_year.SelectedItem.Text != "--Select Year--")
        //            dt_v.RowFilter = "(employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%') and (Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%')";
        //    }

        //    if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
        //        dt_v.RowFilter = "Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%'";

        //    grd_empoyee_contribution.DataSource = dt_v;
        //    grd_empoyee_contribution.DataBind();
        //    grd_empoyee_contribution.Style.Add("font-weight", "200");

        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=EmployeeContribution.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);

        //    using (StringWriter stringWriter = new StringWriter())
        //    {
        //        iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(19);
        //        iTextSharp.text.Font brown = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9f, iTextSharp.text.Font.NORMAL);

        //        // Set the column widths
        //        int[] widths = { 5, 15, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12, 12 };
        //        PdfPCell theCel = new PdfPCell(new Paragraph("S/N", brown));
        //        table.AddCell(theCel);

        //        for (int x = 0; x < grd_empoyee_contribution.Columns.Count; x++)
        //        {
        //            try
        //            {
        //                string cellText = grd_empoyee_contribution.HeaderRow.Cells[x].Text;
        //                PdfPCell theCell = new PdfPCell(new Paragraph(cellText, brown));
        //                table.AddCell(theCell);
        //            }
        //            catch (Exception ext)
        //            {
        //                // Handle the exception or log it, if necessary
        //            }
        //        }

        //        table.CompleteRow();
        //        table.SetWidths(widths);

        //        // Transfer rows from GridView to table
        //        for (int i = 0; i < grd_empoyee_contribution.Rows.Count; i++)
        //        {
        //            string cellText2 = (i + 1).ToString();
        //            PdfPCell theCell2 = new PdfPCell(new Paragraph(cellText2, brown));
        //            table.AddCell(theCell2);

        //            for (int j = 0; j < grd_empoyee_contribution.Columns.Count; j++)
        //            {
        //                string cellText = grd_empoyee_contribution.Rows[i].Cells[j].Text;
        //                if (cellText == "&nbsp;")
        //                {
        //                    cellText = "";
        //                }
        //                PdfPCell theCell = new PdfPCell(new Paragraph(cellText, brown));
        //                table.AddCell(theCell);
        //            }

        //            table.CompleteRow();
        //        }

        //        var style = new StyleSheet();
        //        style.LoadTagStyle("body", "size", "5px");
        //        table.WidthPercentage = 100;

        //        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        //        var style1 = new StyleSheet();
        //        style1.LoadTagStyle("body", "size", "5px");
        //        table.WidthPercentage = 100;
        //        Main.RenderControl(htmlTextWriter);
        //        StringReader stringReader = new StringReader(stringWriter.ToString());
        //        Document Doc = new Document(PageSize.A4, 20, 13, 20, 0);
        //        Doc.SetPageSize(PageSize.A4.Rotate());
        //        HTMLWorker htmlparser = new HTMLWorker(Doc);


        //        using (Document doc = new Document(PageSize.A4, 20, 13, 20, 0))
        //        {
        //            doc.SetPageSize(PageSize.A4.Rotate());
        //            PdfWriter.GetInstance(doc, Response.OutputStream);
        //            doc.Open();
        //            htmlparser.SetStyleSheet(style);

        //            htmlparser.Parse(stringReader);
        //            doc.Add(table);
        //            doc.Close();
        //        }

        //        Response.Write(Doc);
        //        HttpContext.Current.ApplicationInstance.CompleteRequest(); 
        //    }

        //    grd_empoyee_contribution.AllowPaging = true;
        //    grd_empoyee_contribution.DataSource = (DataTable)(Session["dt_l"]);
        //    grd_empoyee_contribution.DataBind();
        //}
        //catch (Exception e1)
        //{
        //    // Handle the exception or log it, if necessary
        //}
        div_loading.Attributes.Add("display", "none");
    }



    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grd_empoyee_contribution.PageIndex = e.NewPageIndex;
        grd_empoyee_contribution.DataSource = Session["dt_l"];

        grd_empoyee_contribution.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom.Text = "1";
        }
        else
        {
            lblpagefrom.Text = ((grd_empoyee_contribution.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto.Text = ((e.NewPageIndex + 1) * grd_empoyee_contribution.Rows.Count).ToString();

    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        dt_list_s = (DataTable)Session["dt_l"];
        // DataRow[] filteredRows = dt_list_s.Select("TaxPayerRIN LIKE '" + txt_RIN.Text + "'");
        DataTable dt_filtered = new DataTable();
        DataView dt_v = dt_list_s.DefaultView;
        if (txt_employer_RIN.Text != "")
        {
            dt_v.RowFilter = "employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%'";

            if (txt_tax_year.SelectedItem.Text != "--Select Year--")
                dt_v.RowFilter = "(employeeRIN like '%" + txt_employer_RIN.Text + "%' or Employeename like '%" + txt_employer_RIN.Text + "%' or EmployerName like '%" + txt_employer_RIN.Text + "%' or Assessment_year like '%" + txt_employer_RIN.Text + "%' or EmployerRIN like '%" + txt_employer_RIN.Text + "%') and (Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%')";

            if (dt_v.Count == 0)
            {
                string script = "alert('No Record Found');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
                return;
            }
        }
        if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
            dt_v.RowFilter = "Assessment_year like '%" + txt_tax_year.SelectedItem.Text + "%'";
        if (dt_v.Count == 0)
        {
            string script = "alert('No Record Found');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }
        grd_empoyee_contribution.DataSource = dt_v;
        grd_empoyee_contribution.DataBind();

        int pagesize = grd_empoyee_contribution.Rows.Count;
        int from_pg = 1;
        int to = grd_empoyee_contribution.Rows.Count;
        int totalcount = dt_v.Count;
        lblpagefrom.Text = from_pg.ToString();
        lblpageto.Text = (from_pg + pagesize - 1).ToString();
        lbltoal.Text = totalcount.ToString();

        if (totalcount < grd_empoyee_contribution.PageSize)
            div_paging.Style.Add("margin-top", "0px");
        else
            div_paging.Style.Add("margin-top", "-60px");
    }


    //protected void BindGridView()
    //{
     
    //    foreach (GridViewRow row in grd_empoyee_contribution.Rows)
    //    {
    //        if (row.RowType == DataControlRowType.DataRow)
    //        {
    //            string pdfUrl = Server.MapPath("~/EmployerContributionOutput.aspx");
    //            //string pdfUrl = GetPDFUrlForRow(row); // Replace this with your logic to get the PDF URL

    //            Button btnDownload = (Button)row.FindControl("btnDownload");
    //            btnDownload.Attributes["data-pdfurl"] = pdfUrl;
    //        }
    //    }
    //}


    public static MonthListValue CalculateTaxMonths(string startMonth, double monthlyTax)
    {
        startMonth = startMonth.ToLower();
        if (startMonth == "january")
            return new MonthListValue { Jan = monthlyTax, Feb = monthlyTax, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "february")
            return new MonthListValue { Jan = 0, Feb = monthlyTax, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "march")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = monthlyTax, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "april")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = monthlyTax, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "may")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = monthlyTax, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "june")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = monthlyTax, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "july")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = monthlyTax, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "august")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = monthlyTax, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "september")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = monthlyTax, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "october")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = monthlyTax, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "november")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = monthlyTax, Dec = monthlyTax };
        else if (startMonth == "december")
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = 0, Dec = monthlyTax };
        else
            return new MonthListValue { Jan = 0, Feb = 0, Mar = 0, Apr = 0, May = 0, Jun = 0, Jull = 0, Aug = 0, Sep = 0, Oct = 0, Nov = 0, Dec = 0 };
    }

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