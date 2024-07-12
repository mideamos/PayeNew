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
using System.Linq;
using System.Globalization;

public partial class AssessmentQueue_N : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txt_tax_year.Items.Add("--Select Year--");
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                txt_tax_year.Items.Add(i.ToString());
            }

            bindgrid();
        }
        var EmpRin = txt_employer_RIN.Text;
        var AssRin = txt_assetRin.Text;
    }
    protected void grdAssessmentQueue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAssessmentQueue.PageIndex = e.NewPageIndex;
        grdAssessmentQueue.DataSource = Session["assQue"];

        grdAssessmentQueue.DataBind();
    }

    public void bindgrid()
    {
        DateTime dt = DateTime.Now;
        int year = dt.Year;

        //txtAmt.Text = StrAmt; Output:= 1,34,600.00
        try
        {
            //DataTable dtempcollection = new DataTable();
            //DataTable dtempcollection1 = new DataTable();
            //dtempcollection.Columns.Add("EmployerRIN", typeof(string));
            //dtempcollection.Columns.Add("EmployerName", typeof(string));
            //dtempcollection.Columns.Add("Asset", typeof(string));
            //dtempcollection.Columns.Add("Rule", typeof(string));
            //dtempcollection.Columns.Add("TaxMonthYear", typeof(string));
            //dtempcollection.Columns.Add("TaxBaseAmount", typeof(string));
            //dtempcollection.Columns.Add("AssessmentNotes", typeof(string));
            //dtempcollection.Columns.Add("Status", typeof(string));
            //dtempcollection.Columns.Add("AssessmentRef", typeof(string));

            //  dtempcollection1.Columns.Add("EmployerRIN", typeof(string));
            //dtempcollection1.Columns.Add("EmployerName", typeof(string));
            //dtempcollection1.Columns.Add("Asset", typeof(string));
            //dtempcollection1.Columns.Add("Rule", typeof(string));
            //dtempcollection1.Columns.Add("TaxMonthYear", typeof(string));
            //dtempcollection1.Columns.Add("TaxBaseAmount", typeof(string));
            //dtempcollection1.Columns.Add("AssessmentNotes", typeof(string));
            //dtempcollection1.Columns.Add("Status", typeof(string));
            //dtempcollection1.Columns.Add("AssessmentRef", typeof(string));

            //DataTable dt_list = new DataTable();
            //DataTable dt_list1=new DataTable();
            //SqlDataAdapter Adp = new SqlDataAdapter("SELECT  A.AssetID,B.AssetName,A.Assessment_RefNo, A.ProfileID,A.AssessmentRuleID,A.TaxYear,A.AssessmentItemID,A.Status,A.TaxPayerID,A.TaxPayerTypeID,A.AssetID,A.AssetTypeID,AssessmentRuleID,A.TaxBaseAmount,A.AssessmentItemName,A.AssessmentRuleName,A.AssetRin,A.TaxPayerRin FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null AND A.TaxYear = '" + year + "'  ORDER BY A.AssessmentRDM_Id DESC", con);
            //SqlDataAdapter Adp2 = new SqlDataAdapter("SELECT TOP(15)  A.AssetID,B.AssetName,A.Assessment_RefNo, A.ProfileID,A.AssessmentRuleID,A.TaxYear,A.AssessmentItemID,A.Status,A.TaxPayerID,A.TaxPayerTypeID,A.AssetID,A.AssetTypeID,AssessmentRuleID,A.TaxBaseAmount,A.AssessmentItemName,A.AssessmentRuleName,A.AssetRin,A.TaxPayerRin FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null AND A.TaxYear = '" + year + "'  ORDER BY A.AssessmentRDM_Id DESC", con);
            //Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            //Adp.Fill(dt_list);
            //for (int i = 0; i < dt_list.Rows.Count; i++)
            //{
            //    decimal moneyvalue = Convert.ToDecimal(dt_list.Rows[i]["TaxBaseAmount"]);
            //    string html = String.Format("{0:n2}", moneyvalue);
            //    dtempcollection.Rows.Add(dt_list.Rows[i]["TaxPayerRIN"].ToString(), dt_list.Rows[i]["AssetName"].ToString(), dt_list.Rows[i]["AssetRIN"].ToString(), dt_list.Rows[i]["AssessmentRuleName"].ToString(), dt_list.Rows[i]["TaxYear"].ToString(), html, dt_list.Rows[i]["AssessmentRuleName"].ToString() + "-" + dt_list.Rows[i]["TaxYear"].ToString() + "(₦" + html + ")", dt_list.Rows[i]["Status"].ToString(), dt_list.Rows[i]["Assessment_RefNo"].ToString());
            //}


            ////will refactor later
            //Adp2.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            //Adp2.Fill(dt_list1);
            //for (int i = 0; i < dt_list1.Rows.Count; i++)
            //{
            //    decimal moneyvalue = Convert.ToDecimal(dt_list1.Rows[i]["TaxBaseAmount"]);
            //    string html = String.Format("{0:n2}", moneyvalue);
            //    dtempcollection1.Rows.Add(dt_list1.Rows[i]["TaxPayerRIN"].ToString(), dt_list1.Rows[i]["AssetName"].ToString(), dt_list1.Rows[i]["AssetRIN"].ToString(), dt_list1.Rows[i]["AssessmentRuleName"].ToString(), dt_list1.Rows[i]["TaxYear"].ToString(), html, dt_list1.Rows[i]["AssessmentRuleName"].ToString() + "-" + dt_list1.Rows[i]["TaxYear"].ToString() + "(₦" + html + ")", dt_list1.Rows[i]["Status"].ToString(), dt_list1.Rows[i]["Assessment_RefNo"].ToString());
            //}


            string query = "";
            query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID and A.TaxPayerID=B.TaxPayerID WHERE  A.Assessment_RefNo is not null ORDER BY A.AssessmentRDM_Id DESC ";

            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt1 = new DataTable();
            con.Open();
            dt1.Clear();
            adp.Fill(dt1);
            if (dt1.Rows.Count == 0)
            {
                string script = "alert('No Record Found');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
                return;
            }
            con.Close();

            Session["assQue"] = dt1;
            grdAssessmentQueue.DataSource = dt1;
            grdAssessmentQueue.DataBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        //dt_list_s = (DataTable)Session["dtempcollection"];
        //// DataRow[] filteredRows = dt_list_s.Select("TaxPayerRIN LIKE '" + txt_RIN.Text + "'");
        //DataTable dt_filtered = new DataTable();
        //DataView dt_v = dt_list_s.DefaultView;
        //if (txt_employer_RIN.Text != "")
        //{
        //    dt_v.RowFilter = "TaxPayerRIN like '%" + txt_employer_RIN.Text + "%' or Asset like '%" + txt_employer_RIN.Text + "%' or Rule like '%" + txt_employer_RIN.Text + "%' or AssessmentRef like '%" + txt_employer_RIN.Text + "%' or Status like '" + txt_employer_RIN.Text + "%'";

        //    if (txt_tax_year.SelectedItem.Text != "--Select Year--")
        //        dt_v.RowFilter = "(TaxPayerRIN like '%" + txt_employer_RIN.Text + "%' or Asset like '%" + txt_employer_RIN.Text + "%' or Rule like '%" + txt_employer_RIN.Text + "%' or AssessmentRef like '%" + txt_employer_RIN.Text + "%' or Status like '" + txt_employer_RIN.Text + "%') and (TaxMonthYear like '%" + txt_tax_year.SelectedItem.Text + "%')";

        //}
        //if (txt_tax_year.SelectedItem.Text != "--Select Year--" && txt_employer_RIN.Text == "")
        //    dt_v.RowFilter = "TaxMonthYear like '%" + txt_tax_year.SelectedItem.Text + "%'";
        string query = "";
        if (txt_tax_year.SelectedItem.Text != "--Select Year--")
        {
            var Year = txt_tax_year.Text;
            query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and TaxYear = '" + Year + "' ORDER BY A.AssessmentRDM_Id DESC ";

        }
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        con.Open();
        dt1.Clear();
        adp.Fill(dt1);
        if (dt1.Rows.Count == 0)
        {
            string script = "alert('No Record Found For Year Selected');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }

        grdAssessmentQueue.DataSource = dt1;
        grdAssessmentQueue.DataBind();

        //int pagesize = grdAssessmentQueue.Rows.Count;
        //int from_pg = 1;
        //int to = grdAssessmentQueue.Rows.Count;
        //int totalcount = dt_v.Count;
        //lblpagefrom.Text = from_pg.ToString();
        //lblpageto.Text = (from_pg + pagesize - 1).ToString();
        //lbltoal.Text = totalcount.ToString();

        //if (totalcount < grd_Company.PageSize)
        //    div_paging.Style.Add("margin-top", "0px");
        //else
        //    div_paging.Style.Add("margin-top", "-60px");
    }


    protected void txt_employer_RIN_TextChanged(object sender, EventArgs e)
    {
        var EmpRin = txt_employer_RIN.Text;
        var AssRin = txt_assetRin.Text;

        string query = "";
        if (EmpRin != null)
        {
       
            if (EmpRin.Contains("CMP") == true || EmpRin.Contains("cmp") == true)
            {
                query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and b.TaxPayerRINNumber = '" + EmpRin + "' ORDER BY A.AssessmentRDM_Id DESC ";

            }
            else if (EmpRin.Contains("for") == true || EmpRin.Contains("FOR") == true)
            {
                query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and b.AssetRIN = '" + EmpRin + "' ORDER BY A.AssessmentRDM_Id DESC ";

            }
        }

        if (AssRin != null)
        {
            if (AssRin.Contains("CMP") == true || AssRin.Contains("cmp") == true)
            {
                query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and b.TaxPayerRINNumber = '" + AssRin + "' ORDER BY A.AssessmentRDM_Id DESC ";

            }
           else if (EmpRin.Length > 0 && AssRin.Length > 0)
            {
                query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and b.TaxPayerRINNumber = '" + EmpRin + "' and b.AssetRIN = '" + AssRin + "' ORDER BY A.AssessmentRDM_Id DESC ";
            }
            else if (AssRin.Contains("for") == true || AssRin.Contains("FOR") == true )
            {
                query = "SELECT a.Assessment_RefNo, b.TaxPayerRINNumber, b.TaxPayerName, b.AssetRIN, a.AssessmentRuleID, A.assessmentrulename, A.TaxYear, A.TaxBaseAmount, A.Status, AssetName FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE  A.Assessment_RefNo is not null and b.AssetRIN = '" + AssRin + "' ORDER BY A.AssessmentRDM_Id DESC ";

            }
        }



        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        con.Open();
        dt1.Clear();
        adp.Fill(dt1);
        con.Close();

        if (dt1.Rows.Count == 0)
        {
            string script = "alert('No Record Found');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }
        

        Session["assQue"] = dt1;
        grdAssessmentQueue.DataSource = dt1;
        grdAssessmentQueue.DataBind();
    }

    protected void txt_assetRin_TextChanged(object sender, EventArgs e)
    {

        string query = "";

        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        DataTable dt1 = new DataTable();
        con.Open();
        dt1.Clear();
        adp.Fill(dt1);
        if (dt1.Rows.Count == 0)
        {
            string script = "alert('No Record Found');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "successAlert", script, true);
            return;
        }
        con.Close();

        Session["assQue"] = dt1;
        grdAssessmentQueue.DataSource = dt1;
        grdAssessmentQueue.DataBind();
    }
}