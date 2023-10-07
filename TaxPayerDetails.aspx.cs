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

public partial class TaxPayerDetails : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            string val = "";
            SqlConnection con = new SqlConnection(PAYEClass.connection);
            if (Request.QueryString["name"] != null)
            {
                val = Request.QueryString["name"].ToString();
                Session["val"] = val;
                Response.Redirect("TaxPayerDetails.aspx");
            }

            if (Session["val"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            string[] val1 = Session["val"].ToString().Split('|');
            txt_name.Text = val1[0].ToString().Trim();
            txt_RIN.Text = val1[4].ToString().Trim();
            txt_tin.Text = val1[3].ToString().Trim();
            txt_Mobileno.Text = val1[5].ToString().Trim();
            txt_address.Text = val1[2].ToString().Trim();
            DataView dataView_rules = new DataView();
            DataView dataView_asset = new DataView();
            div_assets.Style.Add("display", "");

            if (val1[1].ToString().Trim() == "Ind" || val1[1].ToString().Trim() == "Individual")
            {
                div_assets.Style.Add("display", "none");
                txt_type.Text = "Individual";
                lbl_main_head.Text = "Individual Tax Payer";
                lbl_head1.Text = "Individual Tax Payer Information";
                dataView_rules = null;
                Session["dt_l"] = dataView_rules;

                div_MDA_Services.Style.Add("display", "");
                div_rules.Style.Add("display", "none");


                grd_AssessmentRules.DataSource = dataView_rules;
                grd_AssessmentRules.DataBind();


                DataTable dt_MDAServices = new DataTable();
                dt_MDAServices = null;
                grd_MDA_Services.DataSource = dt_MDAServices;
                grd_MDA_Services.DataBind();

                int from_pg1 = 0;
                int totalcount1 = 0;
                int to1 = 0;

                if (grd_MDA_Services.Rows.Count > 0)
                {

                    from_pg1 = 1;
                    to1 = grd_AssessmentRules.Rows.Count;
                    if (grd_AssessmentRules.Rows.Count > 0)
                        totalcount1 = dataView_rules.Count;
                    else
                        totalcount1 = grd_AssessmentRules.Rows.Count;
                    totalcount1 = dataView_rules.Count;
                    lblpagefrom_MDA.Text = from_pg1.ToString();
                    lblpageto_MDA.Text = (from_pg1 + to1 - 1).ToString();
                    lblpageAll_MDA.Text = totalcount1.ToString();
                }
                else
                {
                    from_pg1 = 0;
                    to1 = 0;
                    totalcount1 = 0;
                    lblpagefrom_MDA.Text = "0";
                    lblpageto_MDA.Text = "0";
                    lblpageAll_MDA.Text = "0";

                }
                /********************************************************************/

                WebClient request = new WebClient();

                string url = PAYEClass.uploadurltxtfile + "Business.txt";
                string version = "";
                string fileString = "";
                request.Credentials = new NetworkCredential(PAYEClass.ftpusername, PAYEClass.ftppassword);

                //byte[] newFileData = request.DownloadData(new Uri(url));
                //fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                //string InsCompRes = fileString;
                //DataTable dt_list = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));
                DataTable dt_list = new DataTable();
                SqlDataAdapter Adp = new SqlDataAdapter("select * from Businesses_Full_API", con);
                Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
                Adp.Fill(dt_list);

                dataView_asset = dt_list.DefaultView;
                dataView_asset.RowFilter = "TaxPayerRIN = '" + val1[4].ToString().Trim() + "'";
                if (dataView_asset.Count > 0)
                {
                    grd_asset.DataSource = dataView_asset;
                    grd_asset.DataBind();
                }
                else
                {
                    dataView_asset = null;
                    grd_asset.DataSource = dataView_asset;
                    grd_asset.DataBind();
                }
                Session["dt_l_asset"] = dataView_asset;


            }
            else
            {
                lbl_main_head.Text = "Corporate Tax Payer";
                txt_type.Text = "Corporate";
                lbl_head1.Text = "Corporate Tax Payer Information";

                div_MDA_Services.Style.Add("display", "none");
                div_rules.Style.Add("display", "");

                //WebClient request = new WebClient();

                //string url = PAYEClass.uploadurltxtfile + "AssessmentRules.txt";
                //string version = "";
                //string fileString = "";
                //request.Credentials = new NetworkCredential(PAYEClass.ftpusername, PAYEClass.ftppassword);

                //byte[] newFileData = request.DownloadData(new Uri(url));
                //fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                //string InsCompRes = fileString;
                //DataTable dt_list = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));



                DataTable dt_CorporatsAssets = new DataTable();
                SqlDataAdapter Adp_Assets = new SqlDataAdapter();
               
                {
                    Adp_Assets = new SqlDataAdapter("select * from vw_Corporates_Assets where TaxPayerRIN='" + val1[4].ToString().Trim() + "'", con);
                }
                Adp_Assets.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;

                Adp_Assets.Fill(dt_CorporatsAssets);
                dataView_asset = dt_CorporatsAssets.DefaultView;
                grd_asset.DataSource = dt_CorporatsAssets;
                grd_asset.DataBind();
                Session["dt_l_asset"] = dt_CorporatsAssets;
            }

            //asset paging starts here
            int from_pg_ar = 0;
            int to_pg_ar = 0;
            int totalcount_pg_ar = 0;
            int from_pg = 0;
            int totalcount = 0;
            int to = 0;


            if (dataView_asset == null || dataView_asset.Count == 0)
            {
                lbl_page_from1.Text = "0";
                lbl_page_to1.Text = "0";
                lbl_page_all1.Text = "0";

            }
            else
            {
                from_pg_ar = 1;
                to_pg_ar = dataView_asset.Count;
                totalcount_pg_ar = dataView_asset.Count;

                lbl_page_from1.Text = from_pg_ar.ToString();
                lbl_page_to1.Text = (from_pg_ar + to_pg_ar - 1).ToString();
                lbl_page_all1.Text = totalcount_pg_ar.ToString();


            }
            // asset paging ends here



            DataTable dt_list_rules = new DataTable();
            SqlDataAdapter Adp_rules = new SqlDataAdapter("select * from Assessment_Rules_API order by AssessmentRuleCode", con);
            Adp_rules.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            // DataTable Dt_database = new DataTable();
            Adp_rules.Fill(dt_list_rules);

            dataView_rules = dt_list_rules.DefaultView;
            dataView_rules.RowFilter = "TaxPayerRIN = '" + val1[4].ToString().Trim() + "'";
            grd_AssessmentRules.DataSource = dataView_rules;
            grd_AssessmentRules.DataBind();

            Session["dt_l"] = dataView_rules;
            //assessment rule paging starts here

            if (grd_AssessmentRules.Rows.Count > 0)
            {

                from_pg = 1;
                to = grd_AssessmentRules.Rows.Count;
                if (grd_AssessmentRules.Rows.Count > 0)
                    totalcount = dataView_rules.Count;
                else
                    totalcount = grd_AssessmentRules.Rows.Count;
                totalcount = dataView_rules.Count;
                lblpagefrom.Text = from_pg.ToString();
                lblpageto.Text = (from_pg + to - 1).ToString();
                lbltoal.Text = totalcount.ToString();
            }
            else
            {
                from_pg = 0;
                to = 0;
                totalcount = 0;
                lblpagefrom.Text = "0";
                lblpageto.Text = "0";
                lbltoal.Text = "0";

            }
            //assessment rule ends here 

            //margin for assessment rule
            if (totalcount < grd_AssessmentRules.PageSize)
                div_paging.Style.Add("margin-top", "0px");
            else
                div_paging.Style.Add("margin-top", "-60px");
            //margin assessment rule ends here

            //margin for asset pagining
            if (totalcount_pg_ar < grd_asset.PageSize)
            {
                divpagingasset.Style.Add("margin-top", "0px");
            }
            else
            {
                divpagingasset.Style.Add("margin-top", "-60px");
            }

            //margin assets ends here



            //Profile Gridview Bind

            DataTable dt_profile_list = new DataTable();
            SqlDataAdapter Adp_Profile = new SqlDataAdapter("select * from Profiles_API", con);
            Adp_Profile.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            // DataTable Dt_database = new DataTable();
            Adp_Profile.Fill(dt_profile_list);


            grd_Profiles.DataSource = dt_profile_list;
            grd_Profiles.DataBind();

            Session["dt_l_profile"] = dt_profile_list;

            if (grd_Profiles.Rows.Count > 0)
            {

                from_pg = 1;
                to = grd_Profiles.Rows.Count;
                if (grd_Profiles.Rows.Count > 0)
                    totalcount = dt_profile_list.Rows.Count;
                else
                    totalcount = grd_Profiles.Rows.Count;
                totalcount = dt_profile_list.Rows.Count;
                lbl_page_from2.Text = from_pg.ToString();
                lbl_page_to2.Text = (from_pg + to - 1).ToString();
                lbl_page_all2.Text = totalcount.ToString();
            }
            else
            {
                from_pg = 0;
                to = 0;
                totalcount = 0;
                lbl_page_from2.Text = "0";
                lbl_page_to2.Text = "0";
                lbl_page_all2.Text = "0";

            }

            // Profile Binding END




            //Bills Gridview Bind

            DataTable dt_bills_list = new DataTable();
            SqlDataAdapter Adp_Bills = new SqlDataAdapter("select * from (select Assessment_RefNo as BillID, 'Assessment' as BillType, Status as BillStatus, TaxBaseAmount as BillAmount, TaxPayerID, convert(varchar, create_at, 106) as BillDate from PreAssessmentRDM) a, (select TaxPayerID,TaxPayerRIN from CompanyList_API) b where a.TaxPayerID=b.TaxPayerID and TaxPayerRIN='" + val1[4].ToString().Trim() + "'", con);
            Adp_Bills.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            // DataTable Dt_database = new DataTable();
            Adp_Bills.Fill(dt_bills_list);


            grd_Associated_Bills.DataSource = dt_bills_list;
            grd_Associated_Bills.DataBind();

            Session["dt_l_bills"] = dt_bills_list;

            if (grd_Associated_Bills.Rows.Count > 0)
            {

                from_pg = 1;
                to = grd_Associated_Bills.Rows.Count;
                if (grd_Associated_Bills.Rows.Count > 0)
                    totalcount = dt_bills_list.Rows.Count;
                else
                    totalcount = grd_Associated_Bills.Rows.Count;
                totalcount = dt_bills_list.Rows.Count;
                lblpagefrom3.Text = from_pg.ToString();
                lblpageto3.Text = (from_pg + to - 1).ToString();
                lblpageTotal3.Text = totalcount.ToString();
            }
            else
            {
                from_pg = 0;
                to = 0;
                totalcount = 0;
                lblpagefrom3.Text = "0";
                lblpageto3.Text = "0";
                lblpageTotal3.Text = "0";

            }
        }
        if (Session["roleId"] != null)
        {
            string roleId = Session["roleId"].ToString();
            if (roleId == "1")
            {
                AddAssId.Visible = false;
            }
            else if (roleId == "2" || roleId == "3")
            {
                AddAssId.Visible = true;
            }
        }

        // Bills Binding END
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_AssessmentRules.PageIndex = e.NewPageIndex;
        grd_AssessmentRules.DataSource = Session["dt_l"];
        grd_AssessmentRules.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom.Text = "1";
        }
        else
        {
            lblpagefrom.Text = ((grd_AssessmentRules.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto.Text = ((e.NewPageIndex + 1) * grd_AssessmentRules.Rows.Count).ToString();



    }

    protected void grd_AssessmentRules_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void grd_asset_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_asset.PageIndex = e.NewPageIndex;
        grd_asset.DataSource = Session["dt_l_asset"];
        grd_asset.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lbl_page_from1.Text = "1";
        }
        else
        {
            lbl_page_from1.Text = ((grd_asset.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lbl_page_to1.Text = ((e.NewPageIndex + 1) * grd_asset.Rows.Count).ToString();
    }

    protected void grd_Associated_Bills_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd_Associated_Bills.PageIndex = e.NewPageIndex;
        grd_Associated_Bills.DataSource = Session["dt_l_bills"];
        grd_Associated_Bills.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom3.Text = "1";
        }
        else
        {
            lblpagefrom3.Text = ((grd_Associated_Bills.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto3.Text = ((e.NewPageIndex + 1) * grd_Associated_Bills.Rows.Count).ToString();

    }

    protected void txtsearchasset_TextChanged(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        dt_list_s = (DataTable)Session["dt_l_asset"];
        DataTable dt_filtered = new DataTable();
        DataView dt_v = dt_list_s.DefaultView;
        if (txtsearchasset.Text != "")
        {
            dt_v.RowFilter = "BusinessName like '%" + txtsearchasset.Text + "%'";
        }
        if (dt_v.Count > 0)
        {
            grd_asset.DataSource = dt_v;
            grd_asset.DataBind();
        }
        else
        {
            grd_asset.DataSource = null;
            grd_asset.DataBind();
        }
    }
    protected void txtsearchprofile_TextChanged(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        dt_list_s = (DataTable)Session["dt_l_profile"];
        DataTable dt_filtered = new DataTable();
        DataView dt_v = dt_list_s.DefaultView;
        if (txtsearchprofile.Text != "")
        {
            dt_v.RowFilter = "ProfileDescription like '%" + txtsearchprofile.Text + "%' or ProfileReferenceNo like '%" + txtsearchprofile.Text + "%'";
        }
        if (dt_v.Count > 0)
        {
            grd_Profiles.DataSource = dt_v;
            grd_Profiles.DataBind();
        }
        else
        {
            grd_Profiles.DataSource = null;
            grd_Profiles.DataBind();
        }
    }
    protected void txtsearchbills_TextChanged(object sender, EventArgs e)
    {
        DataTable dt_list_s = new DataTable();
        dt_list_s = (DataTable)Session["dt_l_bils"];
        DataTable dt_filtered = new DataTable();
        DataView dt_v = dt_list_s.DefaultView;
        if (txtsearchbills.Text != "")
        {
            dt_v.RowFilter = "BillID like '%" + txtsearchbills.Text + "%'";
        }
        if (dt_v.Count > 0)
        {
            grd_Associated_Bills.DataSource = dt_v;
            grd_Associated_Bills.DataBind();
        }
        else
        {
            grd_Associated_Bills.DataSource = null;
            grd_Associated_Bills.DataBind();
        }
    }
    protected void txtsearchrule_TextChanged(object sender, EventArgs e)
    {
        DataView dt_v = (DataView)Session["dt_l"];
        if (txtsearchrule.Text != "")
        {
            dt_v.RowFilter = "AssessmentRuleCode like '%" + txtsearchrule.Text + "%' or AssessmentRuleName like '%" + txtsearchrule.Text + "%'";
        }
        if (dt_v.Count > 0)
        {
            grd_AssessmentRules.DataSource = dt_v;
            grd_AssessmentRules.DataBind();
        }
        else
        {
            grd_AssessmentRules.DataSource = null;
            grd_AssessmentRules.DataBind();
        }
    }
}