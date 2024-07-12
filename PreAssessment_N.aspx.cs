using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Spire.Xls;
using System.Linq;

public partial class PreAssessment_N : System.Web.UI.Page
{
    private static string connection = System.Configuration.ConfigurationManager.ConnectionStrings["constring"].ToString();
    SqlConnection con = new SqlConnection(connection);
    //SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        lnk_Pull_Assets.Attributes.Add("onClick", "return false;");
        if (!IsPostBack)
        {
            txt_tax_year.Items.Add("--Select Year--");
            for (int i = DateTime.Now.Year; i >= 2014; i--)
            {
                txt_tax_year.Items.Add(i.ToString());
            }

            //bindgrid();
        }

    }

    public class AssessmentAmt
    {
        public int AssessmentItemID { get; set; }
        public double TaxBaseAmount { get; set; }
    }

    public class AssessmetDet
    {
        public int TaxPayerTypeID { get; set; }
        public int TaxPayerID { get; set; }
        public string Notes { get; set; }
        public int AssetTypeID { get; set; }
        public int AssetID { get; set; }
        public int ProfileID { get; set; }
        public int AssessmentRuleID { get; set; }
        public int TaxYear { get; set; }
        public List<AssessmentAmt> LstAssessmentItem { get; set; }

    }

    [System.Web.Services.WebMethod]
    public static Object update()
    {
        return PAYEClass.returnDataTable();
    }
    public void bindgrid(string assetRIN,string companyRIN)
    {
        DataTable dtempcollection = new DataTable();
        dtempcollection.Columns.Add("AssetID", typeof(string));
        dtempcollection.Columns.Add("AssetName", typeof(string));
        dtempcollection.Columns.Add("ProfileID", typeof(string));
        dtempcollection.Columns.Add("AssetTypeID", typeof(string));
        dtempcollection.Columns.Add("AssessmentRuleID", typeof(string));
        dtempcollection.Columns.Add("TaxPayerID", typeof(string));
        dtempcollection.Columns.Add("TaxPayerTypeID", typeof(string));
        dtempcollection.Columns.Add("TaxYear", typeof(string));
        dtempcollection.Columns.Add("AssessmentItemID", typeof(string));
        dtempcollection.Columns.Add("TaxBaseAmount", typeof(string));
        dtempcollection.Columns.Add("AssessmentItemName", typeof(string));
        dtempcollection.Columns.Add("AssessmentRuleName", typeof(string));

        dtempcollection.Columns.Add("AssetRin", typeof(string));
        dtempcollection.Columns.Add("Status", typeof(string));
        dtempcollection.Columns.Add("TaxPayerRin", typeof(string));

        DataTable dt = new DataTable();
        dt =  GetPreAssessments(assetRIN, companyRIN);
        //dt = PAYEClass.fetchdata(qryempcollection);
        //dt = PAYEClass.returnDataTable();

        dt.Columns.Add("Jan", typeof(System.Double));

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string resString = dt.Rows[i]["AssessmentRuleName"].ToString();
                string monthNameOld = resString.Remove(0,18); 
                string monthName = monthNameOld.Replace("Collections", ""); 
                {
                    dtempcollection.Rows.Add(dt.Rows[i]["AssetID"].ToString(),
                    dt.Rows[i]["AssetName"].ToString(),
                    dt.Rows[i]["ProfileID"].ToString(),
                    dt.Rows[i]["AssetTypeID"].ToString(),
                    dt.Rows[i]["AssessmentRuleID"].ToString(),
                    dt.Rows[i]["TaxPayerID"].ToString(),
                    dt.Rows[i]["TaxPayerTypeID"].ToString(),
                    monthName + '-' + " "+dt.Rows[i]["TaxYear"].ToString(),
                    dt.Rows[i]["AssessmentItemID"].ToString(),
                    dt.Rows[i]["TaxBaseAmount"].ToString(),
                    dt.Rows[i]["AssessmentItemName"].ToString(),
                    dt.Rows[i]["AssessmentRuleName"].ToString(),
                    dt.Rows[i]["AssetRin"].ToString(),
                    dt.Rows[i]["Status"].ToString(),
                    dt.Rows[i]["TaxPayerRin"].ToString());

                }

            }
        }


        DataView dv = dtempcollection.DefaultView;
        dv.Sort = "AssetRin";

        Session["dtempcollection"] = dv;
        grdempcollection.DataSource = dv;
        grdempcollection.DataBind();
    }

    private DataTable GetPreAssessments(string assetRIN,string companyRIN)
    {
        DataTable responseDt = new DataTable();
        var query = "SELECT  A.AssetID,B.AssetName, A.ProfileID,A.AssessmentRuleID,A.TaxYear,A.AssessmentItemID,A.Status,A.TaxPayerID,A.TaxPayerTypeID,A.AssetID,A.AssetTypeID,AssessmentRuleID,A.TaxBaseAmount,A.AssessmentItemName,A.AssessmentRuleName,A.AssetRin,A.TaxPayerRin FROM PreAssessmentRDM A left join AssetTaxPayerDetails_API B on A.AssetID = B.AssetID WHERE A.AssetRin = '" + assetRIN + "' AND B.TaxPayerRINNumber = '"+ companyRIN + "' And Status is null order by a.TaxYear desc, a.AssessmentItemID";

        SqlCommand cmd = new SqlCommand(query, con);

        //cmd.CommandTimeout = 30 * 1000;
        con.Open();

        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandTimeout = PAYEClass.defaultTimeout;
            adapter.Fill(responseDt);
        }

        con.Close();
        con.Dispose();

        return responseDt;
    }


    protected void grdempcollection_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdempcollection.PageIndex = e.NewPageIndex;
        grdempcollection.DataSource = (DataView)Session["dtempcollection"];
      

        grdempcollection.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string token = "";
        token = PAYEClass.getToken();
        try
        {
            int check = 0;

            int taxpayerTypeID1 = 1;
            int TaxPayerID1 = 1;
            int AssetTypeID1 = 1;
            int AssetID1 = 1;

            string Notes1 = "sss";

            int ProfileId1 = 1;
            int AssesmentRuleId1 = 1;
            int TaxYear1 = 2018;
            int AssessmentItemId1 = 1;
            double taxBaseAmt1 = 10;

            string msg = "";
            foreach (GridViewRow gvrow in grdempcollection.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkrdm");
                Label lbl_status = (Label)gvrow.FindControl("lbl_status");
                if (chk != null & chk.Checked)
                {
                    check = 1;

                    
                    AssessmentAmt amt = new AssessmentAmt
                    {
                        AssessmentItemID = Convert.ToInt32(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "")),
                        TaxBaseAmount = Convert.ToDouble(gvrow.Cells[12].Text.Trim().Replace("&nbsp;", "")),
                    };

                    AssessmetDet Assessment = new AssessmetDet
                    {
                        TaxPayerTypeID = Convert.ToInt32(gvrow.Cells[0].Text),

                        TaxPayerID = Convert.ToInt32(gvrow.Cells[1].Text),

                        AssetTypeID = Convert.ToInt32(gvrow.Cells[2].Text),

                        AssetID = Convert.ToInt32(gvrow.Cells[3].Text),

                        ProfileID = Convert.ToInt32(gvrow.Cells[4].Text),

                        AssessmentRuleID = Convert.ToInt32(gvrow.Cells[6].Text),

                        Notes = "-",
                        TaxYear = Convert.ToInt32(gvrow.Cells[11].Text.Split('-')[1]),

                        LstAssessmentItem = new List<AssessmentAmt>()
                    };

                    Assessment.LstAssessmentItem.Add(amt);
                    taxpayerTypeID1 = Convert.ToInt32(gvrow.Cells[0].Text);

                    TaxPayerID1 = Convert.ToInt32(gvrow.Cells[1].Text);

                    AssetTypeID1 = Convert.ToInt32(gvrow.Cells[2].Text);

                    AssetID1 = Convert.ToInt32(gvrow.Cells[3].Text);

                    ProfileId1 = Convert.ToInt32(gvrow.Cells[4].Text);

                    TaxYear1 = Convert.ToInt32(gvrow.Cells[11].Text.Split('-')[1]);

                    AssessmentItemId1 = Convert.ToInt32(gvrow.Cells[5].Text.Trim().Replace("&nbsp;", "") == "" ? "0" : gvrow.Cells[5].Text.Trim().Replace("&nbsp;", ""));

                    AssesmentRuleId1 = Convert.ToInt32(gvrow.Cells[6].Text);

                    taxBaseAmt1 = Convert.ToDouble(gvrow.Cells[12].Text.Trim().Replace("&nbsp;", ""));

                    string[] res;
                    string URI = PAYEClass.URL_API + "RevenueData/Assessment/Insert";
                   string myParameters = "{ \"TaxPayerTypeID\": " + taxpayerTypeID1 + ", \"TaxPayerID\": " + TaxPayerID1 + ", \"Notes\": \"" + Notes1 + "\", \"AssetTypeID\": " + AssetTypeID1 + ", \"AssetID\": " + AssetID1 + ", \"ProfileID\": " + ProfileId1 + ", \"AssessmentRuleID\": " + AssesmentRuleId1 + ", \"TaxYear\": " + TaxYear1 + ", \"LstAssessmentItem\": [ { \"AssessmentItemID\": " + AssessmentItemId1 + ", \"TaxBaseAmount\": " + taxBaseAmt1 + " } ] }";
                    Session["AssetId"] = "0";
                    string InsCompRes = "";
                    string tok = Session["token"].ToString();
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                        wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();
                        string json = JsonConvert.SerializeObject(Assessment);
                        InsCompRes = wc.UploadString(new Uri(URI), "POST", json);
                        res = InsCompRes.Split('"');

                    }

                    if (res[2].ToString().Contains("true"))
                    {
                       
                        SqlCommand delete = new SqlCommand("Update PreAssessmentRDM set [Assessment_RefNo]='" + res[9].ToString() + "',status='Success' where AssetRIn='" + gvrow.Cells[8].Text + "' and TaxPayerRin ='" + gvrow.Cells[7].Text + "' and AssessmentRuleID='" + Assessment.AssessmentRuleID + "'", con);
                        con.Open();
                        delete.ExecuteNonQuery();
                        con.Close();
                        msg = msg + "Assessment Added Successfully. Ref. No. " + res[9].ToString() + " For TaxPayer " + gvrow.Cells[7].Text + "(" + gvrow.Cells[11].Text + ") " + System.Environment.NewLine;
                        chk.Visible = false;
                        lbl_status.Visible = true;
                        lbl_status.Text = "Done";
                    }

                    if (res[2].ToString().Contains("false"))
                    {
                        msg = msg + res[5].ToString() + " For TaxPayer" + gvrow.Cells[7].Text + "(" + gvrow.Cells[11].Text + ")  " + System.Environment.NewLine;
                    }

                }

            }

            showmsg(1, "" + msg);
        }
        catch (Exception ex)
        {
            showmsg(2, "Error Occured. Contact to Administrator.");
            Console.WriteLine(ex.Message);
        }
    }

    public void showmsg(int id, string msg)
    {
        if (id == 1)
        {
            divmsg.Style.Add("display", "");
            divmsg.InnerHtml = "<i class='menu-icon fa fa-check-circle' style='font-size:20px !important;'></i>&nbsp;" + msg + "";
            divmsg.Attributes.Add("class", "alert alert-success");
        }
        else if (id == 2)
        {
            divmsg.Style.Add("display", "");
            divmsg.InnerHtml = "<i class='menu-icon fa fa-warning (alias)' style='font-size:20px !important;'></i>&nbsp;" + msg + "";
            divmsg.Attributes.Add("class", "alert alert-warning");
        }
        else
        {
            divmsg.Style.Add("display", "none");
        }
    }
    protected void grdempcollection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[0].Text != "&nbsp;")
                {
                    DataTable dt_list = new DataTable();
                    SqlDataAdapter Adp = new SqlDataAdapter("select * from PreAssessmentRDM where taxpayertypeid=" + e.Row.Cells[0].Text + " and TaxPayerID=" + e.Row.Cells[1].Text + " and AssetId=" + e.Row.Cells[3].Text + " and ProfileID=" + e.Row.Cells[4].Text + " and AssessmentRuleID=" + e.Row.Cells[6].Text + " and AssessmentItemID=" + e.Row.Cells[5].Text + " and TaxYear=" + e.Row.Cells[11].Text.Split('-')[1], con);
                    Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
                    Adp.Fill(dt_list);

                    CheckBox chkbox = (CheckBox)e.Row.FindControl("chkrdm");
                    Label lbl_status = (Label)e.Row.FindControl("lbl_status");

                    //if (dt_list.Rows.Count > 0)
                    //{
                    //    chkbox.Visible = false;
                    //    lbl_status.Visible = true;
                    //    lbl_status.Text = "Done";
                    //}

                    //else
                    //{

                    //    chkbox.Visible = true;
                    //    lbl_status.Visible = false;
                    //    lbl_status.Text = "";
                    //}
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        DataView dt_list_s = new DataView();
        dt_list_s = (DataView)Session["dtempcollection"];
        DataView dt_v = dt_list_s;

        if (txt_employer_RIN.Text != "")
        {
            var assetRIN = txt_employer_RIN.Text;
            var companyRIN = txt_cmp_RIN.Text;
            bindgrid(assetRIN, companyRIN);
         }

        int pagesize = grdempcollection.Rows.Count;
        int from_pg = 1;
        int to = grdempcollection.Rows.Count;
        int totalcount = 12;
    }
}