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
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

public partial class PayeOutputFile_N : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt_list = new DataTable();
            if (Session["redirectedRIN"] != null)
            {
                string comRin = Session["searched_company_RIN"].ToString();
                string assetRin = Session["redirectedRIN"].ToString();
                Session.Remove("redirectedRIN");
                dt_list = GetEmployeeCompanies(assetRin,comRin);
            }
            Session["EditEMPFlag"] = "0";

            txt_tax_year.Items.Add("--Select Year--");
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                txt_tax_year.Items.Add(i.ToString());
            }

            Session["dt_l"] = dt_list;
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
    public class MyClass
    {
        public string Tax_Year { get; set; }
        public decimal TotalMonthlyTax { get; set; }
        public string AssetRin { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string EmployeeCount { get; set; }
        public int Id { get; set; }
        public string CompanyRIN { get; set; }
        public string CompanyName { get; set; }
        public string BusinessRIN { get; set; }
        public int TaxPayerTypeId { get; set; }
        public string TaxPayerTypeName { get; set; }
        public int AssetId { get; set; }
        public int TaxPayerId { get; set; }
        public string TaxPayerName { get; set; }
        public int AssetTypeId { get; set; }
        public int AssessmentRuleId { get; set; }
        public int ProfileId { get; set; }
        public int AssessmentItemID { get; set; }
        public string AssessmentItemName { get; set; }
        public string AssessmentRuleName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
    public class Query1
    {
        public int Id { get; set; }
        public string Tax_Year { get; set; }
        public string AssetRin { get; set; }
        public string CompanyRIN { get; set; }
        public string CompanyName { get; set; }
        public string BusinessRIN { get; set; }
        public string SuccessfulStatus { get; set; }


        public int Tpaid { get; set; }
        public int TaxPayerTypeId { get; set; }
        public string TaxPayerTypeName { get; set; }
        public int AssetId { get; set; }
        public int TaxPayerId { get; set; }
        public string TaxPayerName { get; set; }
        public int AssetTypeId { get; set; }
        public string TaxPayerRinnumber { get; set; }
        public string TaxPayerEmailAddress { get; set; }
        public string TaxPayerMobileNumber { get; set; }
        public string AssetTypeName { get; set; }
        public int TaxPayerRoleId { get; set; }
        public string TaxPayerRoleName { get; set; }
        public string AssetLga { get; set; }
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
        public int Id { get; set; }
        public string Tax_Year { get; set; }
        public string AssessmentRuleId { get; set; }
        public string ProfileId { get; set; }
        public string StatusId { get; set; }
        public string Status { get; set; }
        public string EmployeeCount { get; set; }
        public string AssessmentRuleName { get; set; }
    }
    public class Query2b
    {
        public Decimal TotalMonthlyTax { get; set; }
    }
    public class Query3
    {
        public int Id { get; set; }
        public int AssessmentItemID { get; set; }
        public string AssessmentItemName { get; set; }
        public string AssessmentRuleName { get; set; }
    }
    private DataTable GetEmployeeCompanies(string assetRin,string comRin)
    {
        DataTable responseDt = new DataTable();
        DataTable responseDt2 = new DataTable();
        DataTable responseDt3 = new DataTable();
     var q1=   "SELECT distinct o.EmployerRIN as CompanyRIN, t.TaxPayerName as CompanyName, o.Assessment_Year as Tax_Year, o.AssetRin, ISNULL(o.STATUS, 0) " +
                 "AS STATUS, CASE WHEN ISNULL(o.STATUS, 0) = 0 THEN 'UnFiled' ELSE 'Filed' END AS FiledStatus,  (CASE WHEN o.STATUS = 3 THEN 'Processed' " +
                    "ELSE '' END) AS SuccessfulStatus from AssetTaxPayerDetails_API t left outer join PayeOuputFile o on t.TaxPayerRINNumber = o.employerRIN where o.AssetRin ='" + assetRin + "'and o.EmployerRin='" + comRin + "' and o.Status >= 2";
        //var q1 = "SELECT distinct EmployerRIN as CompanyRIN,EmployerName as CompanyName,Assessment_Year as Tax_Year,AssetRin, ISNULL(PayeOuputFile.STATUS, 0) " +
        //         "AS STATUS, CASE WHEN ISNULL(PayeOuputFile.STATUS, 0) = 0 THEN 'UnFiled' ELSE 'Filed' END AS FiledStatus,  (CASE WHEN PayeOuputFile.STATUS = 3 THEN 'Processed' " +
        //            "ELSE '' END) AS SuccessfulStatus" +
        //    " from PayeOuputFile cross join TaxNewYears where AssetRin ='" + assetRin + "'and EmployerRin='"+ comRin + "' and Status >= 2";

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
            student.SuccessfulStatus = responseDt2.Rows[i]["SuccessfulStatus"].ToString();
            student.CompanyName = responseDt2.Rows[i]["CompanyName"].ToString();
            student.CompanyRIN = responseDt2.Rows[i]["CompanyRIN"].ToString();
            student.AssetRin = responseDt2.Rows[i]["AssetRin"].ToString();
            que1.Add(student);
        }
        string CompanyRIN = "0";
        var ret = que1.FindLast(o => o.Id == 1);
        if (ret != null)
        {
            CompanyRIN = ret.CompanyRIN;
        }
        var q2 = "select count(EmployeeRIN) TotalCount,Status,Assessment_Year from PayeOuputFile where AssetRin='" + assetRin + "'and  EmployerRIN ='" + CompanyRIN + "' and Status >= 2 group by Assessment_Year,EmployerRIN,Status";

        SqlCommand cmd2 = new SqlCommand(q2, con);
        using (SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2))
        {
            cmd2.CommandTimeout = PAYEClass.defaultTimeout;
            adapter2.Fill(responseDt3);
        }

        for (int i = 0; i < responseDt3.Rows.Count; i++)
        {
            Query2 student = new Query2();
            student.Tax_Year = responseDt3.Rows[i]["Assessment_Year"].ToString();
            student.StatusId = responseDt3.Rows[i]["Status"].ToString();
            student.EmployeeCount = responseDt3.Rows[i]["TotalCount"].ToString();
            que2.Add(student);
        }

        var viewModels = (from m in que1
                          join r in que2 on m.Tax_Year equals r.Tax_Year
                          select new MyClass()
                          {
                              AssetRin = m.AssetRin,
                              CompanyName = m.CompanyName,
                              CompanyRIN = m.CompanyRIN,
                              Tax_Year = m.Tax_Year,
                              StatusId = r.StatusId,
                              Status = m.SuccessfulStatus,
                              EmployeeCount = r.EmployeeCount
                          }).ToList();

        con.Close();
        con.Dispose();

        Session["responseDt"] = responseDt;
        responseDt = ListToDataTable(viewModels);
        return responseDt;

    }
    public static DataTable ListToDataTable<T>(IList<T> data)
    {
        DataTable table = new DataTable();

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
        DataView dt_v = dt_list_s.DefaultView;

        if (txt_employer_RIN.Text != "")
        {
            var assetRin = txt_employer_RIN.Text;
            var comRin = txt_cmp_RIN.Text;
            Session["redirectedRIN"] = assetRin;
            Session["searched_company_RIN"] = txt_cmp_RIN.Text;
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

            stat = stat.ToLower();
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkchkbox");
            if (chk != null & chk.Checked)
            {
                if (stat == "processed")
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Company Already Processed.');</script>", false);
                    return;
                }

                check = 1;
            }

        }

        if (check == 0)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Please Select a Company');</script>", false);
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

                    SqlCommand Update = new SqlCommand("Update PayeOuputFile set Status=3 where EmployerRIN='" + gvrow.Cells[0].Text + "' and AssetRin ='" + gvrow.Cells[3].Text + "' and Assessment_Year='" + gvrow.Cells[2].Text + "'", con);
                    con.Open();
                    Update.ExecuteNonQuery();
                    con.Close();
                    GetRecord(gvrow.Cells[3].Text, gvrow.Cells[0].Text, gvrow.Cells[2].Text);
                    Session["redirectedRIN"] = gvrow.Cells[3].Text;
                }
            }
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Status Changed Successfully!!!');</script>", false);
            Response.Redirect("PayeOutputFile_N.aspx");

            return;

        }

    }
    private DataTable GetRecord(string assetRin, string comRin, string year)
    {
        DataTable responseDt = new DataTable();
        DataTable responseDt2 = new DataTable();
        DataTable responseDt3 = new DataTable();
        DataTable responseDt30 = new DataTable();
        DataTable responseDt4 = new DataTable();
        DataTable responseDt5 = new DataTable();

        var q1 = "SELECT * FROM AssetTaxPayerDetails_API where AssetRin ='" + assetRin + "'  and TaxPayerRINNumber ='" + comRin + "'";

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
        List<Query2b> que2b = new List<Query2b>();
        List<Query3> que4 = new List<Query3>();
        for (int i = 0; i < responseDt2.Rows.Count; i++)
        {
            Query1 student = new Query1();
            student.Id = i + 1;
            student.TaxPayerId = Convert.ToInt32(responseDt2.Rows[i]["TaxPayerID"]);
            student.TaxPayerTypeId = Convert.ToInt32(responseDt2.Rows[i]["TaxPayerTypeId"]);
            student.AssetId = Convert.ToInt32(responseDt2.Rows[i]["AssetId"]);
            student.AssetTypeId = Convert.ToInt32(responseDt2.Rows[i]["AssetTypeId"]);
            student.AssetRin = responseDt2.Rows[i]["AssetRin"].ToString();
            student.TaxPayerName = responseDt2.Rows[i]["TaxPayerName"].ToString();
            que1.Add(student);
        }
        var q2 = "select * from Assessment_Rules where tax_year='" + year + "'";
        var q3 = "SELECT Distinct [AssessmentRuleName],[AssessmentItemID],[AssessmentItemName] FROM [Assessment_Item_API]";
        var q4 = "select sum(monthlytax)TotalMonthlyTax from PayeOuputFile where EmployerRIN = '" + comRin + "' and AssetRin='" + assetRin + "' and Assessment_Year='" + year + "'";

        SqlCommand cmd2 = new SqlCommand(q2, con);
        using (SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2))
        {
            cmd2.CommandTimeout = PAYEClass.defaultTimeout;
            adapter2.Fill(responseDt3);
        }
        SqlCommand cmd4 = new SqlCommand(q4, con);
        using (SqlDataAdapter adapter4 = new SqlDataAdapter(cmd4))
        {
            cmd4.CommandTimeout = PAYEClass.defaultTimeout;
            adapter4.Fill(responseDt5);
        }
        SqlCommand cmd3 = new SqlCommand(q3, con);
        using (SqlDataAdapter adapter3 = new SqlDataAdapter(cmd3))
        {
            cmd3.CommandTimeout = PAYEClass.defaultTimeout;
            adapter3.Fill(responseDt4);
        }

        for (int i = 0; i < responseDt3.Rows.Count; i++)
        {
            Query2 student = new Query2();
            student.Id = i + 1;
            student.ProfileId = responseDt3.Rows[i]["profile"].ToString();
            student.AssessmentRuleId = responseDt3.Rows[i]["AssessmentRuleId"].ToString();
            student.AssessmentRuleName = responseDt3.Rows[i]["assessment_rule_name"].ToString();

            que2.Add(student);
        }
        for (int i = 0; i < responseDt5.Rows.Count; i++)
        {
            Query2b student = new Query2b();
            student.TotalMonthlyTax = Convert.ToDecimal(responseDt5.Rows[i]["TotalMonthlyTax"]);

            que2b.Add(student);
        }
        for (int i = 0; i < responseDt4.Rows.Count; i++)
        {
            Query3 student = new Query3();
            student.Id = i + 1;
            student.AssessmentItemID = Convert.ToInt32(responseDt4.Rows[i]["AssessmentItemID"]);
            student.AssessmentItemName = responseDt4.Rows[i]["AssessmentItemName"].ToString();
            student.AssessmentRuleName = responseDt4.Rows[i]["AssessmentRuleName"].ToString();

            que4.Add(student);
        }
        string ruleName = "";
        string itemName = "";
        int itemId = 0;

        foreach (var ret in que2)
        {

            MyClass myClass = new MyClass();
            var retVal = que1.FirstOrDefault();
            var retValSum = que2b.FirstOrDefault();
            var retVal2 = que4.FirstOrDefault(o => o.Id == ret.Id);
            if (retVal2 != null)
            {
                ruleName = retVal2.AssessmentRuleName;
                itemName = retVal2.AssessmentItemName;
                itemId = retVal2.AssessmentItemID;
            }

            if (ret != null)
            {
                myClass.AssessmentItemName = itemName;
                myClass.AssessmentRuleName = ruleName;
                myClass.AssessmentItemID = itemId;
                myClass.TaxPayerName = retVal.TaxPayerName;
                myClass.TaxPayerId = retVal.TaxPayerId;
                myClass.TaxPayerTypeId = retVal.TaxPayerTypeId;
                myClass.AssetId = retVal.AssetId;
                myClass.AssetTypeId = retVal.AssetTypeId;
                myClass.AssetRin = retVal.AssetRin;
                myClass.Tax_Year = year;
                myClass.ProfileId = Convert.ToInt32(ret.ProfileId);
                myClass.AssessmentRuleId = Convert.ToInt32(ret.AssessmentRuleId);
                myClass.TotalMonthlyTax = retValSum.TotalMonthlyTax;
                que3.Add(myClass);
            }
        }

        con.Close();
        con.Dispose();

        responseDt = ListToDataTable(que3);
        string userId = Session["user_id"].ToString();
        SqlConnection con1 = new SqlConnection(PAYEClass.connection);
        SqlConnection con2 = new SqlConnection(PAYEClass.connection);
        con1.Open();

        var checker = "select * from PreAssessmentRDM where taxyear='" + year + "' and AssetRin='" + assetRin + "' and TaxPayerRin='" + comRin + "' and Assessment_RefNo is not null";
        SqlCommand cmd20 = new SqlCommand(checker, con1);
        using (SqlDataAdapter adapter20 = new SqlDataAdapter(cmd20))
        {
            cmd20.CommandTimeout = PAYEClass.defaultTimeout;
            adapter20.Fill(responseDt30);
        }

        foreach (DataRow row in responseDt.Rows)
        {

            var rows = responseDt30.AsEnumerable().Where(r => r.Field<int>("AssessmentItemID") == row.Field<int>("AssessmentItemID"));
            if (rows.Count() > 0)
            {
                SqlCommand delete = new SqlCommand("Update PreAssessmentRDM set TaxBaseAmount='" + row.Field<decimal>("TotalMonthlyTax") + "' where AssessmentItemID='" + row.Field<int>("AssessmentItemID") + "' and taxyear='" + year + "'", con2);
                con2.Open();
                delete.ExecuteNonQuery();
                con2.Close();
            }
            else
            {
                var kk = "INSERT INTO PreAssessmentRDM ([TaxPayerTypeID],[TaxPayerID],[AssetTypeID],[AssetID],[ProfileID],[AssessmentRuleID],[TaxYear],[AssessmentItemID],[TaxBaseAmount],[AssessmentRuleName],[AssessmentItemName],[AssetRin],[TaxPayerRin],[create_by]) values (" +
                 "'" + row.Field<int>("TaxPayerTypeID") + "'," +
                 "'" + row.Field<int>("TaxPayerID") + "'," +
                 "'" + row.Field<int>("AssetTypeID") + "'," +
                 "'" + row.Field<int>("AssetID") + "'," +
                 "'" + row.Field<int>("ProfileID") + "'," +
                 "'" + row.Field<int>("AssessmentRuleID") + "'," +
                 "'" + row.Field<string>("Tax_Year") + "'," +
                 "'" + row.Field<int>("AssessmentItemID") + "'," +
                 "'" + row.Field<Decimal>("TotalMonthlyTax") + "'," +
                 "'" + row.Field<string>("AssessmentRuleName") + "'," +
                 "'" + row.Field<string>("AssessmentItemName") + "'," +
                 "'" + assetRin + "'," +
                 "'" + comRin + "'," +
                 "'" + userId + "'" +
                 ")";
                SqlCommand cmd8 = new SqlCommand(kk, con1);
                cmd8.ExecuteNonQuery();
            }
        }


        return responseDt;

    }
    public void checkbox()
    {
    }
    protected void grd_Company_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }
    }
    protected void btn_file_reverse_Click(object sender, EventArgs e)
    {
        int check = 0;
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        string rin = grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString();

        string confirmValue = Request.Form["confirm_value"];

        confirmValue = hidden1.Value;
        if (confirmValue == "Yes")
        {
            //firstly delete from payeouputfile table --- done
            //then go to  preassessmentrdm table where employerrin and year = what you have then check if the assementrefno is ! null
            string query = "DELETE FROM PayeOuputFile where EmployerRIN ='" + grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString() + "' and  Assessment_Year ='" + grd_Company.Rows[clickedRow.RowIndex].Cells[2].Text.ToString() + "'";
            string queryPreass = "DELETE FROM PreAssessmentRDM where TaxPayerRIN='" + grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString() + "' and AssetRin='" + grd_Company.Rows[clickedRow.RowIndex].Cells[3].Text.ToString() + "' and TaxYear ='" + grd_Company.Rows[clickedRow.RowIndex].Cells[2].Text.ToString() + "' and Assessment_RefNo is null";

            SqlCommand truncate = new SqlCommand(query, con);
            SqlCommand truncate2 = new SqlCommand(queryPreass, con);
            con.Open();
            truncate.ExecuteNonQuery();
            truncate2.ExecuteNonQuery();
            SqlCommand update_reverse = new SqlCommand("Update PayeInputFile set StatusId=1 where EmployerRIN='" + grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString() + "' and Assessment_Year='" + grd_Company.Rows[clickedRow.RowIndex].Cells[2].Text.ToString() + "'", con);

            update_reverse.ExecuteNonQuery();
            con.Close();
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

    }
    protected void btn_Save_Input_Ouput_Click(object sender, EventArgs e)
    {
        int check = 0;
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        string rin = grd_Company.Rows[clickedRow.RowIndex].Cells[0].Text.ToString();
        string tax_year = grd_Company.Rows[clickedRow.RowIndex].Cells[3].Text.ToString();


        DataTable dt_list_SaveInputOutput = new DataTable();

        SqlDataAdapter Adp = new SqlDataAdapter("select * from vw_SaveInputOutput_API where EmployerRIN='" + rin + "' and Assessment_Year='" + tax_year + "'", con);
        Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp.Fill(dt_list_SaveInputOutput);


        for (int i = 0; i < dt_list_SaveInputOutput.Rows.Count; i++)
        {
            /************************ADD Input API***********************/

            //DateTime.ParseExact(row1["start_month"].ToString(), "MMM", CultureInfo.InvariantCulture).Month
            string[] res_Input_API;
            string URI_Input_API = "https://stage-api.eirsautomation.xyz/DataWarehouse/PAYEInput/Insert";
            URI_Input_API = PAYEClass.URL_API + "DataWarehouse/PAYEInput/Insert";

            // string myParameters_Input_API = "{\n    \"TranscationDate\": \"" + DateTime.Now.Date.ToString() + "\",\n    \"Employer_RIN\": \"" + drpfupcomapny.SelectedValue.ToString() + "\",\n    \"Employee_RIN\": \"" + dt_list_api.Rows[0]["TaxPayerRIN"].ToString().Trim() + "\",\n    \"Assessment_Year\": " + row1["assessment_year"].ToString().Trim() + ",\n    \"Start_Month\": " + DateTime.ParseExact(row1["start_month"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"End_Month\": " + DateTime.ParseExact(row1["end_month"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"Annual_Basic\": " + row1["annual_basic"].ToString() + ",\n    \"Annual_Rent\": " + row1["annual_rent"].ToString() + ",\n    \"Annual_Transport\": " + row1["annual_transport"].ToString() + ",\n    \"Annual_Utility\": " + row1["annual_utility"].ToString() + ",\n    \"Annual_Meal\": " + row1["annual_meal"].ToString() + ",\n    \"Other_Allowances_Annual\": " + row1["other_allowances_annual"].ToString().Trim() + ",\n    \"Leave_Transport_Grant_Annual\": " + row1["leave_transport_grant_annual"].ToString().Trim() + ",\n    \"pension_contribution_declared\": " + row1["pension_contribution_declared"].ToString().Trim() + ",\n    \"nhf_contribution_declared\": " + row1["nhf_contribution_declared"].ToString().Trim() + ",\n    \"nhis_contribution_declared\": " + row1["nhis_contribution_declared"].ToString().Trim() + "\n}";

            string tax_of = "For Review";
            //  tax_of = Session["TaxOfc"].ToString();
            string myParameters_Input_API = "{\n    \"TranscationDate\": \"" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "\",\n    \"Employer_RIN\": \"" + dt_list_SaveInputOutput.Rows[i]["EmployerRIN"].ToString() + "\",\n    \"Employee_RIN\": \"" + dt_list_SaveInputOutput.Rows[i]["EmployeeRIN"].ToString() + "\",\n    \"Assessment_Year\": " + dt_list_SaveInputOutput.Rows[i]["Assessment_Year"].ToString() + ",\n    \"Start_Month\": " + DateTime.ParseExact(dt_list_SaveInputOutput.Rows[i]["StartMonth"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"End_Month\": " + DateTime.ParseExact(dt_list_SaveInputOutput.Rows[i]["EndMonth"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"Annual_Basic\": " + dt_list_SaveInputOutput.Rows[i]["AnnualBasic"].ToString() + ",\n    \"Annual_Rent\": " + dt_list_SaveInputOutput.Rows[i]["AnnualRent"].ToString() + ",\n    \"Annual_Transport\": " + dt_list_SaveInputOutput.Rows[i]["AnnualTransport"].ToString() + ",\n    \"Annual_Utility\": " + dt_list_SaveInputOutput.Rows[i]["AnnualUtility"].ToString() + ",\n    \"Annual_Meal\": " + dt_list_SaveInputOutput.Rows[i]["AnnualMeal"].ToString() + ",\n    \"Other_Allowances_Annual\": " + dt_list_SaveInputOutput.Rows[i]["OtherAllowances_Annual"].ToString() + ",\n    \"Leave_Transport_Grant_Annual\": " + dt_list_SaveInputOutput.Rows[i]["LeaveTransport_Annual"].ToString().Trim() + ",\n    \"pension_contribution_declared\": " + dt_list_SaveInputOutput.Rows[i]["Pension"].ToString().Trim() + ",\n    \"nhf_contribution_declared\": " + dt_list_SaveInputOutput.Rows[i]["NHF"].ToString().Trim() + ",\n    \"nhis_contribution_declared\": " + dt_list_SaveInputOutput.Rows[i]["NHIS"].ToString().Trim() + ",\n    \"Tax_Office\":\"" + dt_list_SaveInputOutput.Rows[i]["TaxOffice"].ToString().Trim() + "\"\n}";

            string InsCompRes_InputAPI = "";
            using (WebClient wc_Input_API = new WebClient())
            {
                wc_Input_API.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc_Input_API.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();
                // string json = JsonConvert.SerializeObject(Assessment);
                InsCompRes_InputAPI = wc_Input_API.UploadString(URI_Input_API, myParameters_Input_API);

                res_Input_API = InsCompRes_InputAPI.Split('"');

            }
            /****************************END***************************/

            /************************ADD Ouput API***********************/
            //  string tax_of = "For Review";
            //DateTime.ParseExact(row1["start_month"].ToString(), "MMM", CultureInfo.InvariantCulture).Month
            string[] res_Ouput_API;
            string URI_Ouput_API = "https://stage-api.eirsautomation.xyz/DataWarehouse/PAYEOutput/Insert";
            URI_Ouput_API = PAYEClass.URL_API + "DataWarehouse/PAYEOutput/Insert";
            // string myParameters_Ouput_API = "{\n    \"Transaction_Date\": \"" + DateTime.Now.ToString() + "\",\n    \"Employee_Rin\": \"" + dt.Rows[i]["employee_rin"].ToString().Replace("'", "''") + "\",\n    \"Employer_Rin\": \"" + drpfupcomapny.SelectedValue.ToString() + "\",\n    \"AssessmentYear\": " + Convert.ToInt32(Session["Tax_Year"].ToString()) + ",\n    \"Assessment_Month\": 1,\n    \"Monthly_CRA\": " + sal_brkup[0] + ",\n    \"Monthly_Gross\": 0,\n    \"Monthly_ValidatedNHF\": " + sal_brkup[2] + ",\n    \"Monthly_ValidatedNHIS\": " + sal_brkup[3] + ",\n    \"Monthly_ValidatedPension\": " + sal_brkup[1] + ",\n    \"Monthly_TaxFreePay\": " + sal_brkup[4] + ",\n    \"Monthly_ChargeableIncome\": " + sal_brkup[5] + ",\n    \"Monthly_Tax\": " + sal_brkup[7] + "\n}";
            string myParameters_Ouput_API = "{\n    \"Transaction_Date\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",\n    \"Employee_Rin\": \"" + dt_list_SaveInputOutput.Rows[i]["employeerin"].ToString().Replace("'", "''") + "\",\n    \"Employer_Rin\": \"" + dt_list_SaveInputOutput.Rows[i]["EmployerRIN"].ToString() + "\",\n    \"AssessmentYear\": " + Convert.ToInt32(dt_list_SaveInputOutput.Rows[i]["Assessment_Year"].ToString()) + ",\n    \"Assessment_Month\": 1,\n    \"Monthly_CRA\": " + dt_list_SaveInputOutput.Rows[i]["CRA"].ToString().Trim() + ",\n    \"Monthly_Gross\": 0,\n    \"Monthly_ValidatedNHF\": " + dt_list_SaveInputOutput.Rows[i]["ValidatedNHF"].ToString().Trim() + ",\n    \"Monthly_ValidatedNHIS\": " + dt_list_SaveInputOutput.Rows[i]["ValidatedNHIS"].ToString().Trim() + ",\n    \"Monthly_ValidatedPension\": " + dt_list_SaveInputOutput.Rows[i]["ValidatedPension"].ToString().Trim() + ",\n    \"Monthly_TaxFreePay\": " + dt_list_SaveInputOutput.Rows[i]["TaxFreePay"].ToString().Trim() + ",\n    \"Monthly_ChargeableIncome\": " + dt_list_SaveInputOutput.Rows[i]["ChargeableIncome"].ToString().Trim() + ",\n    \"Monthly_Tax\": " + dt_list_SaveInputOutput.Rows[i]["MonthlyTax"].ToString().Trim() + ",\n    \"Tax_Office\":\"" + dt_list_SaveInputOutput.Rows[i]["TaxOffice"].ToString().Trim() + "\"\n}";
            string InsCompRes_OuputAPI = "";
            using (WebClient wc_Ouput_API = new WebClient())
            {
                wc_Ouput_API.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc_Ouput_API.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();
                // string json = JsonConvert.SerializeObject(Assessment);
                InsCompRes_OuputAPI = wc_Ouput_API.UploadString(URI_Ouput_API, myParameters_Ouput_API);

                res_Ouput_API = InsCompRes_OuputAPI.Split('"');

            }
            /****************************END***************************/
        }

        ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Process Completed.');</script>", false);
        return;
    }
}