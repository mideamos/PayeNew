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
using System.ComponentModel;
using System.Linq;

public partial class PayeInputFile_N : System.Web.UI.Page
{
    private string companyRIN = "";
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["user_id"] == null)
                {
                    Response.Redirect("Login.aspx");
                }


                    Session["EditEMPFlag"] = "1";
                DataTable dt_list = new DataTable();

                txt_tax_year.Items.Add("--Select Year--");
                for (int i = DateTime.Now.Year; i >= 2014; i--)
                {
                    txt_tax_year.Items.Add(i.ToString());
                }
                //if (Session["redirectedAss"] != null)
                //{
                //    string comprin = Session["redirectedAss"].ToString();
                //    Session.Remove("redirectedRIN");
                //    dt_list = GetEmployeeCompanies(comprin);
                //}
                //else {
                Session["dt_l"] = dt_list;

                //}
                grd_Company.DataSource = dt_list;
                grd_Company.DataBind();

                int pagesize = grd_Company.Rows.Count;
                int from_pg = 1;
                int to = grd_Company.Rows.Count;
                int totalcount = dt_list.Rows.Count;
                lblpagefrom.Text = from_pg.ToString();
                lblpageto.Text = (from_pg + pagesize - 1).ToString();
                lbltoal.Text = totalcount.ToString();

                if (totalcount < grd_Company.PageSize)
                    div_paging.Style.Add("margin-top", "0px");
                else
                    div_paging.Style.Add("margin-top", "-60px");

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Connection Problem.');</script>", false);
            return;
        }
    }

    public class MyClass
    {
        public string Tax_Year { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string EmployeeCount { get; set; }
        public int Id { get; set; }
        public string CompanyRIN { get; set; }
        public string CompanyName { get; set; }
        public string BusinessRIN { get; set; }
    }
    public class Query1
    {
        public int Id { get; set; }
        public string Tax_Year { get; set; }
        public string CompanyRIN { get; set; }
        public string CompanyName { get; set; }
        public string BusinessRIN { get; set; }


        public int Tpaid { get; set; }
        public int TaxPayerTypeId { get; set; }
        public string TaxPayerTypeName { get; set; }
        public int TaxPayerId { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerRinnumber { get; set; }
        public string TaxPayerEmailAddress { get; set; }
        public string TaxPayerMobileNumber { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public int TaxPayerRoleId { get; set; }
        public string TaxPayerRoleName { get; set; }
        public int AssetId { get; set; }
        public string AssetLga { get; set; }
        public string AssetRin { get; set; }
        public string AssetName { get; set; }
        public string BuildingUnitId { get; set; }
        public string UnitNumber { get; set; }
        public string Active { get; set; }
        public string ActiveText { get; set; }
        public DateTime DateCreated { get; set; }
        public int ApiId { get; set; }

    }
    public class YearCounter
    {
        public int Year { get; set; }

    }
    public class Query2
    {
        public string Tax_Year { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string EmployeeCount { get; set; }
    }
    private DataTable GetEmployeeCompanies(string assetRin, string comRin)
    {
        DataTable responseDt = new DataTable();
        DataTable responseDt2 = new DataTable();
        DataTable responseDt3 = new DataTable();
        var q1 = "SELECT distinct t.TaxPayerRINNumber as CompanyRIN,t.TaxPayerName as CompanyName, t.AssetName as BusinessName, i.Assessment_Year as Tax_Year,t.AssetRIN as BusinessRIN from AssetTaxPayerDetails_API t left outer join PayeInputFile i on t.TaxPayerRINNumber = i.EmployerRIN where t.AssetRIN ='" + assetRin + "' and t.TaxPayerRINNumber = '" + comRin + "' order by  i.Assessment_Year desc";
        //var q1 = "SELECT TaxPayerRINNumber as CompanyRIN,TaxPayerName as CompanyName,Year as Tax_Year,AssetRIN as BusinessRIN,AssetName as BusinessName from AssetTaxPayerDetails_API cross join TaxNewYears where AssetRIN ='" + assetRin + "' and TaxpayerRinNumber = '" + comRin + "'";

        SqlCommand cmd = new SqlCommand(q1, con);

        con.Open();

        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandTimeout = PAYEClass.defaultTimeout;
            adapter.Fill(responseDt2);
        }

        List<Query1> que1 = new List<Query1>();
        List<MyClass> que3 = new List<MyClass>();
        List<Query2> que2 = new List<Query2>();
        List<YearCounter> que4 = new List<YearCounter>();
        for (int i = 0; i < responseDt2.Rows.Count; i++)
        {
            Query1 student = new Query1();
            student.Id = i + 1;
            student.Tax_Year = responseDt2.Rows[i]["Tax_Year"].ToString();
            student.BusinessRIN = responseDt2.Rows[i]["BusinessRIN"].ToString();
            student.CompanyName = responseDt2.Rows[i]["CompanyName"].ToString();
            student.CompanyRIN = responseDt2.Rows[i]["CompanyRIN"].ToString();
            que1.Add(student);
        }
        var dateYear = DateTime.Now.Year;
        for (int i = 2019; i <= dateYear; i++)
        {
            YearCounter student = new YearCounter();
            student.Year = i;
            que4.Add(student);
        }

        string CompanyRIN = "0";
        var ret = que1.FindLast(o => o.Id == 1);
        if (ret != null)
        {

            CompanyRIN = ret.CompanyRIN;
        }
        var q2 = "select count(EmployeeRIN) TotalCount,StatusId,Assessment_Year from PayeInputFile where AssetRIN='" + assetRin + "' and  EmployerRIN ='" + CompanyRIN + "'  group by Assessment_Year,AssetRin,StatusId";

        SqlCommand cmd2 = new SqlCommand(q2, con);
        using (SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2))
        {
            cmd2.CommandTimeout = PAYEClass.defaultTimeout;
            adapter2.Fill(responseDt3);
        }



        if (responseDt3.Rows.Count <= 0)
        {
            foreach (var student in que4)
            {
                Query2 studentQ = new Query2();
                studentQ.Tax_Year = student.Year.ToString();
                studentQ.StatusId = "1";
                studentQ.EmployeeCount = "0";
                que2.Add(studentQ);
            }
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Taxpayer with ASSETRIN " + assetRin + " cannot be found in the System.');</script>", false);
            //return responseDt;
        }
        else
        {
            for (int i = 0; i < responseDt3.Rows.Count; i++)
            {
                Query2 student = new Query2();
                student.Tax_Year = responseDt3.Rows[i]["Assessment_Year"].ToString();
                student.StatusId = responseDt3.Rows[i]["StatusId"].ToString();

                student.EmployeeCount = responseDt3.Rows[i]["TotalCount"].ToString();
                que2.Add(student);
            }
            if (que4.Count != que2.Count)
            {
                foreach (var student in que4)
                {
                    Query2 studentQ = new Query2();
                    var isValid = que2.FirstOrDefault(o => o.Tax_Year == student.Year.ToString());
                    if (isValid == null)
                    {
                        studentQ.Tax_Year = student.Year.ToString();
                        studentQ.StatusId = "1";
                        studentQ.EmployeeCount = "0";
                        que2.Add(studentQ);
                    }
                }
            }
        }
        foreach (var student in que2)
        {
            if (student.StatusId == "1")
                student.Status = "Unfiled";
            else
                student.Status = "Filed";
        }

        List<MyClass> viewModels = new List<MyClass>();
        if (que1.Count != que2.Count)
        {
            foreach (var item in que2)
            {
                var resQue1 = que1.FirstOrDefault();
                MyClass myClass = new MyClass()
                {
                    BusinessRIN = resQue1.BusinessRIN,
                    CompanyName = resQue1.CompanyName,
                    CompanyRIN = resQue1.CompanyRIN,
                    Tax_Year = item.Tax_Year,
                    StatusId = item.StatusId,
                    Status = item.Status,
                    EmployeeCount = item.EmployeeCount
                };
                viewModels.Add(myClass);
            }
        }
        else
        {
            viewModels = (from m in que1
                          join r in que2
                          on m.Tax_Year equals r.Tax_Year

                          select new MyClass()
                          {
                              BusinessRIN = m.BusinessRIN,
                              CompanyName = m.CompanyName,
                              CompanyRIN = m.CompanyRIN,
                              Tax_Year = m.Tax_Year,
                              StatusId = r.StatusId,
                              Status = r.Status,
                              EmployeeCount = r.EmployeeCount
                          }).ToList();
        }


        con.Close();
        con.Dispose();
        responseDt = ListToDataTable(viewModels);
        return responseDt;
    }

    public static DataTable ListToDataTable<T>(IList<T> data)
    {
        DataTable table = new DataTable();

        //special handling for value types and string
        if (typeof(T).IsValueType || typeof(T).Equals(typeof(string)))
        {

            DataColumn dc = new DataColumn("Value", typeof(T));
            table.Columns.Add(dc);
            foreach (T item in data)
            {
                DataRow dr = table.NewRow();
                dr[0] = item;
                table.Rows.Add(dr);
            }
        }
        else
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch (Exception ex)
                    {
                        row[prop.Name] = DBNull.Value;
                    }
                }
                table.Rows.Add(row);
            }
        }
        return table;
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grd_Company.PageIndex = e.NewPageIndex;
        grd_Company.DataSource = Session["dt_l"];

        grd_Company.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom.Text = "1";
        }
        else
        {
            lblpagefrom.Text = ((grd_Company.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto.Text = ((e.NewPageIndex + 1) * grd_Company.Rows.Count).ToString();

    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        dt_list_s = (DataTable)Session["dt_l"];
        DataTable dt_filtered = new DataTable();
        DataTable dt_filtered2 = new DataTable();
        DataView dt_v = dt_list_s.DefaultView;

        if (txt_employer_RIN.Text != "")
        {
            Session["searched_Asset_RIN"] = txt_employer_RIN.Text;
            Session["searched_company_RIN"] = txt_cmp_RIN.Text;
            var assetRin = txt_employer_RIN.Text;
            var comRin = txt_cmp_RIN.Text;
            dt_filtered = GetEmployeeCompanies(assetRin, comRin);
           
        }

        grd_Company.DataSource = dt_filtered;
        grd_Company.DataBind();

        int pagesize = grd_Company.Rows.Count;
        int from_pg = 1;
        int to = grd_Company.Rows.Count;
        int totalcount = dt_v.Count;
        lblpagefrom.Text = from_pg.ToString();
        lblpageto.Text = (from_pg + pagesize - 1).ToString();
        lbltoal.Text = totalcount.ToString();

        if (totalcount < grd_Company.PageSize)
            div_paging.Style.Add("margin-top", "0px");
        else
            div_paging.Style.Add("margin-top", "-60px");
    }
    protected void btn_file_selected_Click(object sender, EventArgs e)
    {
        int check = 0;
        foreach (GridViewRow gvrow in grd_Company.Rows)
        {
            string stat = gvrow.Cells[6].Text;
            stat=stat.ToLower();
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkchkbox");
            if (chk != null & chk.Checked)
            {
                if(stat == "filed")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Company Already Filed.');</script>", false);
                    return;
                }

                check = 1;
            }
        }

        if (check == 0)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Please Select a Company For Filed.');</script>", false);
            return;
        }

        string confirmValue = Request.Form["confirm_value"];

        confirmValue = hidden1.Value;
        if (confirmValue == "Yes")
        {
            foreach (GridViewRow gvrow in grd_Company.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkchkbox");
                if (chk != null & chk.Checked)
                {
                    string countEMP = gvrow.Cells[6].Text;
                    int countEMP2 = Convert.ToInt32(gvrow.Cells[5].Text);
                    string CompRIN = "";
                    DataTable dt_count_EMP_10000 = new DataTable();
                    SqlDataAdapter Adp_10000 = new SqlDataAdapter("select count(*) as c, employerrin from PayeInputFile where Annualgross>=10000 and employerrin='" + gvrow.Cells[0].Text + "' and Assessment_Year='" + gvrow.Cells[2].Text + "' group by EmployerRIN", con);
                    Adp_10000.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
                    Adp_10000.Fill(dt_count_EMP_10000);
                    if (countEMP2 == 0)
                    {
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert(ERROR:'number of employees cannot be 0.');</script>", true);

                    }
                    if (dt_count_EMP_10000.Rows.Count > 0)
                    {
                        SqlCommand delete = new SqlCommand("Update PayeInputFile set StatusId=2 where EmployerRIN='" + gvrow.Cells[0].Text + "' and AssetRin = '" + gvrow.Cells[3].Text + "' and  Assessment_Year='" + gvrow.Cells[2].Text + "'", con);
                        con.Open();
                        delete.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        CompRIN = CompRIN + "," + gvrow.Cells[0].Text;
                        CompRIN = CompRIN.Trim(',');
                         ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Can not Filed these Companies " + CompRIN + " because AnnualGross is less than 10000 of some employees.');</script>", false);
                    }
                }
            }

            var AssetRIN = Session["searched_Asset_RIN"].ToString();
            var companyRIN = Session["searched_company_RIN"].ToString();
            var dt_filtered = GetEmployeeCompanies(AssetRIN, companyRIN);

            Session["dt_l"] = dt_filtered;
            grd_Company.DataSource = dt_filtered;
            grd_Company.DataBind();

        }

    }
    protected void btn_add_new_inputfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("AddPayeInputFile_N.aspx");
    }

    public void checkbox()
    {
    }
    protected void grd_Company_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbtn_drop_emps = (LinkButton)e.Row.FindControl("lnk_drop_employees");
            CheckBox chkbox = (CheckBox)e.Row.FindControl("chkchkbox");
            if (e.Row.Cells[6].Text.ToLower() == "filed")
            {
                chkbox.Enabled = false;
                lbtn_drop_emps.Visible = false;
            }
            LinkButton lnk_SubmitFiling = (LinkButton)e.Row.FindControl("lnksendtoinputfile");
            

        }

    }

    protected void OnDataBound(object sender, EventArgs e)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        for (int i = 0; i < grd_Company.Columns.Count; i++)
        {
            TableHeaderCell cell = new TableHeaderCell();
            TextBox txtSearch = new TextBox();
            txtSearch.Attributes["placeholder"] = grd_Company.Columns[i].HeaderText;
            txtSearch.CssClass = "search_textbox";
            cell.Controls.Add(txtSearch);
            row.Controls.Add(cell);
        }
        grd_Company.HeaderRow.Parent.Controls.AddAt(1, row);
    }

    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }

        //  public string Content-type { get; set; }
    }

    public class Receiver
    {
        public string Success { get; set; }
        //public string Message { get; set; }
        //public string Result { get; set; }

    }

    public void getIndData(string CompRIN)
    {
        DataTable dt_list = new DataTable();
        SqlDataAdapter Adp = new SqlDataAdapter("select  distinct TaxPayerRIN, TaxPayerID, BusinessID, BusinessRIN from Businesses_Full_API where TaxPayerRIN not in (select EmployerRIN from PayeOuputFile where EmployerRIN is not null) and TaxPayerRIN='" + CompRIN + "' order by BusinessID desc", con);
        Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp.Fill(dt_list);

        /***************************************************************/
        string token = PAYEClass.getToken();
        /**************************************************************/
        string URI1 = "";



        for (int i = 0; i < dt_list.Rows.Count; i++)
        {

            URI1 = PAYEClass.URL_API + "TaxPayer/Company/GetAssetTaxPayer?AssetTypeID=3 &AssetID=" + dt_list.Rows[i]["BusinessID"].ToString() + "";
            string InsCompRes = "";
            string headers = "";
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;

                InsCompRes = wc.DownloadString(URI1);

                var des = (Receiver)JsonConvert.DeserializeObject(InsCompRes, typeof(Receiver));



                DataTable dt_list_ins = new DataTable();

                if (des != null && (des.Success).ToLower().ToString() == "true")
                {
                    if (InsCompRes != "" && InsCompRes.Split('[')[1].ToString() != "]}")
                    {
                        dt_list_ins = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));
                        // dt_list_ins = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + InsCompRes.Split('[')[2].Replace("}]", "") + "]", (typeof(DataTable)));

                        DataRow[] dr_comp = dt_list_ins.Select("TaxPayerTypeID = 2");
                        // Array.ForEach<DataRow>(dt_list_ins.Select("TaxPayerTypeID =2"), row => row.Delete());

                        if (dt_list_ins.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt_list_ins.Rows.Count; j++)
                            {
                                if (dt_list_ins.Rows[j]["TaxPayerTypeID"].ToString() != "2")
                                {
                                    DataTable dt_list_select = new DataTable();
                                    SqlDataAdapter Adp_select = new SqlDataAdapter("select  * from Payeouputfile where EmployeeRIN='" + dt_list_ins.Rows[j]["TaxPayerRINNumber"].ToString() + "'", con);
                                    Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
                                    Adp_select.Fill(dt_list_select);

                                    if (dt_list_select.Rows.Count == 0)
                                    {
                                        con.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    protected void btn_drop_employees_Click(object sender, EventArgs e)
    {
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        string rin = grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString();
        string tax_year = grd_Company.Rows[clickedRow.RowIndex].Cells[2].Text.ToString();
        string assetRin = grd_Company.Rows[clickedRow.RowIndex].Cells[3].Text.ToString();
        string stat = grd_Company.Rows[clickedRow.RowIndex].Cells[6].Text.ToString();
        Session["redirectedAss"] = assetRin;
        string confirmValue = Request.Form["confirm_value"];

        confirmValue = hidden1.Value;
        if (confirmValue == "Yes")
        {
            string a;
            string main_ret = "";

            SqlConnection con = new SqlConnection(PAYEClass.connection.ToString());

            try
            {
                if (stat == "Filed")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Cannot Drop Filed Record!!');</script>", false);
                }
                else
                {
                    SqlCommand q1 = new SqlCommand("delete from PayeInputFile where EmployerRIN ='" + rin + "' and Assessment_Year= '" + tax_year + "' and  AssetRIN ='" + assetRin + "'", con);
                    con.Open();
                    q1.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Employee Dropped Successfull');</script>", false);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);
                con.Close();
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Connection Problem.');</script>", false);
                return;
            }



        }
    }
}