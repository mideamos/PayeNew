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

public partial class Registration : System.Web.UI.Page
{
    public class CompanyList_RDM
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string Result { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user_id"] == null)
        {
            Response.Redirect("Login.aspx");

        }
        if (!IsPostBack)
            binddropdowns();
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "HideLabel();", true);


        if (Session["roleId"] != null)
        {
            string roleId = Session["roleId"].ToString();

            if (roleId == "1" || roleId == "2" || roleId == "3")
            {
                btnUpdateId.Visible = true;
            }
        }



    }
    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            divmsg.Style.Add("display", "none");

            if (txt_enter_TIN.Text == "")
            {
                showmsg(11, "Please Fill RIN No.");
                return;
            }


            /***************************************************************/
            string[] res;
            //string URI = "https://stage-api.eirsautomation.xyz/TaxPayer/Company/List";
            string URI = "https://stage-api.eirsautomation.xyz/TaxPayer/Company/Search";
            URI = PAYEClass.URL_API + "TaxPayer/Company/Search";
            string myParameters = "CompanyRIN=" + txt_enter_TIN.Text;

            string InsCompRes = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();

                InsCompRes = wc.UploadString(URI, myParameters);

                res = InsCompRes.Split('"');

            }


            DataTable dt_list = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));

            DataRow[] filteredRows = dt_list.Select("CompanyRIN LIKE '" + txt_enter_TIN.Text + "'");
            DataTable dt_filtered = new DataTable();


            /**************************************************************/


            string dataqry = "select * from vw_companies where company_rin='" + txt_enter_TIN.Text + "'";
            DataTable dt = new DataTable();
            dt = PAYEClass.fetchdata(dataqry);
            Session["CompanyId"] = "0";
            if (dt.Rows.Count > 0)
            {
                Session["CompanyId"] = dt.Rows[0]["company_id"].ToString();
                txt_Company_Created_By.Text = dt.Rows[0]["user_name"].ToString();
                txt_Email1.Text = dt.Rows[0]["email_address_1"].ToString();
                txt_Email2.Text = dt.Rows[0]["email_address_2"].ToString();
                txt_company.Text = dt.Rows[0]["company_name"].ToString();
                txt_RIN.Text = dt.Rows[0]["company_rin"].ToString();

                txt_tax_ofc.SelectedValue = dt.Rows[0]["tax_office"].ToString();
                txt_Company_TIN.Text = dt.Rows[0]["company_Tin"].ToString();
                Session["Rin"] = dt.Rows[0]["company_rin"].ToString();
                txt_tax_payer_type.SelectedValue = dt.Rows[0]["tax_payer_type"].ToString();
                txt_mob1.Text = dt.Rows[0]["mobile_number_1"].ToString();
                txt_mob2.Text = dt.Rows[0]["mobile_number_2"].ToString();
                txt_economic_activity.SelectedValue = dt.Rows[0]["economic_activity"].ToString();
                txt_preferred_notification.SelectedValue = dt.Rows[0]["preferred_notification_method"].ToString();
                txt_tax_payer_status.Text = dt.Rows[0]["tax_payer_status"].ToString();
                txt_Acct_Bal.Text = dt.Rows[0]["account_balance"].ToString();

                txt_house_no.Text = dt.Rows[0]["Add1_hno"].ToString();
                txt_street.Text = dt.Rows[0]["Add2_street"].ToString();
                txt_off_street_town.Text = dt.Rows[0]["Add3_offstreet_town"].ToString();

                dpd_state.SelectedValue = dt.Rows[0]["state"].ToString();

                /*********************************************************************************************************/

                //  dpd_asset_type.SelectedValue = dt.Rows[0]["asset_type"].ToString();
                //  dpd_business_type.SelectedValue = dt.Rows[0]["business_type"].ToString();
                //  dpd_lga.SelectedValue = dt.Rows[0]["business_lga"].ToString();

                dpd_town.SelectedValue = dt.Rows[0]["town"].ToString();
                txt_contact_person.Text = dt.Rows[0]["contact_person"].ToString();

                string qry = "";
                DataTable dt_dpd = new DataTable();

                //qry = "Select * from Business_Structure where business_type=" + dpd_business_type.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_business_structure.DataSource = dt_dpd;
                //dpd_business_structure.DataTextField = "business_structure";
                //dpd_business_structure.DataValueField = "bs_st_id";
                //dpd_business_structure.DataBind();

                //qry = "Select * from Business_Category where business_type=" + dpd_business_type.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_business_category.DataSource = dt_dpd;
                //dpd_business_category.DataTextField = "business_category";
                //dpd_business_category.DataValueField = "bs_ct_id";
                //dpd_business_category.DataBind();

                //qry = "Select * from Business_Operations where business_type=" + dpd_business_type.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_business_operations.DataSource = dt_dpd;
                //dpd_business_operations.DataTextField = "business_operations";
                //dpd_business_operations.DataValueField = "bs_op_id";
                //dpd_business_operations.DataBind();

                //qry = "Select * from Profiles where asset_type=" + dpd_asset_type.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_profile.DataSource = dt_dpd;
                //dpd_profile.DataTextField = "profile_group";
                //dpd_profile.DataValueField = "profile_id";
                //dpd_profile.DataBind();

                //qry = "Select * from Business_Sectors where business_category=" + dpd_business_category.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_business_sector.DataSource = dt_dpd;
                //dpd_business_sector.DataTextField = "business_sector";
                //dpd_business_sector.DataValueField = "bs_sc_id";
                //dpd_business_sector.DataBind();

                //qry = "Select * from Business_Sub_Sectors where  business_sector=" + dpd_business_sector.SelectedValue;
                //dt_dpd = new DataTable();
                //dt_dpd = PAYEClass.fetchdata(qry);
                //dpd_business_sub_sector.DataSource = dt_dpd;
                //dpd_business_sub_sector.DataTextField = "business_sub_sector";
                //dpd_business_sub_sector.DataValueField = "bs_sb_id";
                //dpd_business_sub_sector.DataBind();

                //dpd_business_structure.SelectedValue = dt.Rows[0]["business_structure"].ToString();
                //dpd_business_category.SelectedValue = dt.Rows[0]["business_category"].ToString();
                //dpd_business_operations.SelectedValue = dt.Rows[0]["business_operations"].ToString();
                //dpd_profile.SelectedValue = dt.Rows[0]["profile"].ToString();
                //dpd_business_sector.SelectedValue = dt.Rows[0]["business_sector"].ToString();
                //dpd_business_sub_sector.SelectedValue = dt.Rows[0]["business_sub_sector"].ToString();
                if (filteredRows.Length > 0)
                {
                    dt_filtered = filteredRows.CopyToDataTable();
                    Session["CompanyId"] = dt_filtered.Rows[0]["CompanyID"].ToString();
                }


            }

            else if (filteredRows.Length > 0)
            {
                dt_filtered = filteredRows.CopyToDataTable();
                txt_RIN.Text = dt_filtered.Rows[0]["CompanyRIN"].ToString();
                txt_company.Text = dt_filtered.Rows[0]["CompanyName"].ToString();
                txt_Company_TIN.Text = dt_filtered.Rows[0]["TIN"].ToString();
                txt_mob1.Text = dt_filtered.Rows[0]["MobileNumber1"].ToString();
                txt_mob2.Text = dt_filtered.Rows[0]["MobileNumber2"].ToString();

                txt_Email1.Text = dt_filtered.Rows[0]["EmailAddress1"].ToString();
                txt_Email2.Text = dt_filtered.Rows[0]["EmailAddress2"].ToString();
                txt_tax_ofc.SelectedValue = dt_filtered.Rows[0]["TaxOfficeID"].ToString();

                txt_tax_payer_type.SelectedValue = dt_filtered.Rows[0]["TaxPayerTypeID"].ToString();
                txt_economic_activity.SelectedValue = dt_filtered.Rows[0]["EconomicActivitiesID"].ToString();

                txt_preferred_notification.SelectedValue = dt_filtered.Rows[0]["NotificationMethodID"].ToString();

                txt_house_no.Text = dt_filtered.Rows[0]["ContactAddress"].ToString();

                txt_tax_payer_status.SelectedItem.Text = dt_filtered.Rows[0]["ActiveText"].ToString();
                txt_house_no.Text = dt_filtered.Rows[0]["ContactAddress"].ToString();
                Session["CompanyId"] = dt_filtered.Rows[0]["CompanyID"].ToString();

            }
            else
            {

                // showmsg(11, "Company Not Exist.");
                showmsg(11, "TIN Does Not Exist.");
                txt_company.Text = "";
                txt_Company_TIN.Text = "";
                txt_mob1.Text = "";
                txt_mob2.Text = "";
                txt_Email1.Text = "";
                txt_Email2.Text = "";
                txt_Acct_Bal.Text = "";
                txt_RIN.Text = "";
                txt_Company_Created_By.Text = "";
            }
        }
        catch (Exception ex)
        {
            showmsg(111, "Connection Problem.");
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
        else
        {
            divmsg.Style.Add("display", "");
            divmsg.InnerHtml = "<i class='menu-icon fa fa-warning (alias)' style='font-size:20px !important;'></i>&nbsp;" + msg + "";
            divmsg.Attributes.Add("class", "alert alert-warning");
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        Session["NewSearched"] = "A";
        if (txt_company.Text == "" && rd_search.Checked == true)
        {
            showmsg(11, "Please Search Company.");
            // divmsg.Style.Add("display", "");
            // divmsg.InnerText = "Please Search Company.";
            // divmsg.Attributes.Add("class", "msg-error");
            return;
        }

        if (txt_company.Text == "" && rd_Ind.Checked == true)
        {
            showmsg(11, "Please Enter Company.");
            // divmsg.Style.Add("display", "");
            // divmsg.InnerText = "Please Search Company.";
            // divmsg.Attributes.Add("class", "msg-error");
            return;
        }


        if (txt_RIN.Text == "")
        {
            showmsg(11, "Please Enter RIN No.");

            return;
        }

        if (Regex.Match(txt_mob1.Text, @"^[0-9]{10}$").Success)
        {
            //   Console.WriteLine("correctly entered");
        }
        else
        {
            showmsg(11, "Please Enter 10 Digits Mobile Number.");

            return;
        }
        if (Regex.Match(txt_mob2.Text, @"^[0-9]{10}$").Success)
        {
            //   Console.WriteLine("correctly entered");
        }
        else
        {
            showmsg(11, "Please Enter 10 Digits Mobile Number.");

            return;
        }


        string pattern = null;
        pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

        // if (txt_Email1.Text == "" || txt_Email2.Text == "")
        if (txt_Email1.Text == "")
        {
            if (txt_Email1.Text == "")
            {
                showmsg(11, "Please Enter Email Address 1.");
                return;
            }
            //if (txt_Email2.Text == "")
            //{
            //    showmsg(11, "Please Enter Email Address 2.");
            //    return;
            //}

        }

        if (Regex.IsMatch(txt_Email1.Text, pattern) || Regex.IsMatch(txt_Email2.Text, pattern))
        {
            // showmsg(11, "Please Enter Valid Email Address.");
        }
        else
        {
            showmsg(11, "Please Enter Valid Email Address.");
            return;
        }

        /***************************************************************/
        string RIN = "0";
        if (rd_Ind.Checked == true)
        {
            Session["NewSearched"] = "N";
            string[] res;
            string URI = "https://api.eirsautomation.xyz/TaxPayer/Company/Insert";
            // string myParameters = "UserName=Contec&Password=3Uhf7j~4&grant_type=password";

            string myParameters = "CompanyName=" + txt_company.Text + "&TIN=" + txt_Company_TIN.Text + "&MobileNumber1=" + txt_mob1.Text + "&MobileNumber2=" + txt_mob2.Text + "&EmailAddress1=" + txt_Email1.Text + "&EmailAddress2=" + txt_Email2.Text + "&TaxOfficeID=" + txt_tax_ofc.SelectedValue + "&EconomicActivitiesID=" + txt_economic_activity.SelectedValue + "&NotificationMethodID=" + txt_preferred_notification.SelectedValue + "&ContactAddress=" + txt_house_no.Text + ";" + txt_street.Text + ";" + "&TaxPayerTypeID=" + txt_tax_payer_type.SelectedValue + "";
            string InsCompRes = "";
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                // wc.Headers[HttpRequestHeader.Authorization] = Session["token"].ToString();
                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();

                InsCompRes = wc.UploadString(URI, myParameters);

                res = InsCompRes.Split('"');
                RIN = res[13].ToString();
            }

            if (res[2] == ":false,")
            {
                showmsg(11, res[5].ToString());
                return;
            }
            // InsCompRes = InsCompRes.Replace("\"", "'");
            // Token[] jsonObject = JsonReader.Deserialize<Token[]>(BearerToken);
        }

        /**************************************************************/
        int result = 0;
        if (rd_Ind.Checked == true)
            result = insertcompany(RIN);
        else
            result = insertcompany(txt_RIN.Text);
        if (result == 1)
        {
            txt_company.Text = "";
            txt_Company_TIN.Text = "";
            txt_mob1.Text = "";
            txt_mob2.Text = "";
            txt_Email1.Text = "";
            txt_Email2.Text = "";
            txt_Acct_Bal.Text = "";
            txt_RIN.Text = "";
            txt_Company_Created_By.Text = "";
            // divmsg.Style.Add("display", "");
            // divmsg.InnerText = "Company Created Successfully";
            // divmsg.Attributes.Add("class", "msg");

            showmsg(1, "Company Created Successfully");
            Session["comp_rin"] = "Company Created Successfully," + Session["Rin"] + "";
            // Response.Redirect("AddEmployee.aspx");
            Response.Redirect("AddAsset.aspx");
        }
        else if (result == 11)
        {
            Session["NewSearched"] = "S";
            txt_company.Text = "";
            txt_Company_TIN.Text = "";
            txt_mob1.Text = "";
            txt_mob2.Text = "";
            txt_Email1.Text = "";
            txt_Email2.Text = "";
            txt_Acct_Bal.Text = "";
            txt_RIN.Text = "";
            txt_Company_Created_By.Text = "";
            // divmsg.Style.Add("display", "");
            // divmsg.InnerText = "Company Updated Successfully";
            //  divmsg.Attributes.Add("class", "msg");

            showmsg(1, "Company Updated Successfully");
            Session["comp_rin"] = "Company Updated Successfully," + Session["Rin"] + "";
            //    Response.Redirect("AddEmployee.aspx");
            Response.Redirect("AddAsset.aspx");

        }

        else
        {
            divmsg.Style.Add("display", "");
            divmsg.InnerText = "Error Occured";
            divmsg.Attributes.Add("class", "msg-error");
        }
    }

    public void binddropdowns()
    {


        /***************************************************************/
        string[] res;

        string URI = "https://stage-api.eirsautomation.xyz/ReferenceData/TaxOffice/List";
        URI = PAYEClass.URL_API + "ReferenceData/TaxOffice/List";
        string myParameters = "";

        string InsCompRes = "";
        using (WebClient wc = new WebClient())
        {
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            // wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + Session["token"].ToString();

            InsCompRes = wc.DownloadString(URI);

            res = InsCompRes.Split('"');

        }


        DataTable dt_tax_ofc_list = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));


        /**************************************************************/



        string qry = "Select * from Tax_Offices";
        DataTable dt = new DataTable();
        // dt = PAYEClass.fetchdata(qry);
        txt_tax_ofc.DataSource = dt_tax_ofc_list;
        //txt_tax_ofc.DataTextField = "tax_office";
        //txt_tax_ofc.DataValueField = "to_id";

        txt_tax_ofc.DataTextField = "TaxOfficeName";
        txt_tax_ofc.DataValueField = "TaxOfficeID";

        txt_tax_ofc.DataBind();
        // txt_tax_ofc.Items.Insert(0, new ListItem("--Select--", "0", true));


        qry = "Select * from Tax_Payer_types";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        txt_tax_payer_type.DataSource = dt;
        txt_tax_payer_type.DataTextField = "tax_payer_type";
        txt_tax_payer_type.DataValueField = "tptype_id";

        txt_tax_payer_type.DataBind();
        // txt_tax_payer_type.Items.Insert(0, new ListItem("--Select--", "0", true));



        qry = "Select * from Economic_Activities";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        txt_economic_activity.DataSource = dt;
        txt_economic_activity.DataTextField = "activity";
        txt_economic_activity.DataValueField = "Id";
        txt_economic_activity.DataBind();

        qry = "Select * from Notification_Types";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        txt_preferred_notification.DataSource = dt;
        txt_preferred_notification.DataTextField = "notification_types";
        txt_preferred_notification.DataValueField = "nott_create_by";
        txt_preferred_notification.DataBind();

        //qry = "Select * from Tax_Payer_Roles";
        //dt = new DataTable();
        //dt = PAYEClass.fetchdata(qry);
        //txt_tax_payer_status.DataSource = dt;
        //txt_tax_payer_status.DataTextField = "tpt_status";
        //txt_tax_payer_status.DataValueField = "tpt_id";
        //txt_tax_payer_status.DataBind();

        qry = "Select * from Asset_type";

        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_asset_type.DataSource = dt;
        dpd_asset_type.DataTextField = "asset_type";
        dpd_asset_type.DataValueField = "asset_id";
        dpd_asset_type.DataBind();
        //        dpd_asset_type.Items.Add(new ListItem("--Select--", "0", true));
        dpd_asset_type.Items.Insert(0, new ListItem("--Select--", "0", true));

        qry = "Select * from Business_Type";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_type.DataSource = dt;
        dpd_business_type.DataTextField = "Business_Type";
        dpd_business_type.DataValueField = "Business_Type_id";
        dpd_business_type.DataBind();
        //  dpd_business_type.Items.Insert(0, new ListItem("--Select--", "0", true));

        qry = "Select * from Local_Government_Areas";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_lga.DataSource = dt;
        dpd_lga.DataTextField = "lga";
        dpd_lga.DataValueField = "lga_id";
        dpd_lga.DataBind();

        qry = "Select * from StateMaster";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_state.DataSource = dt;
        dpd_state.DataTextField = "state_name";
        dpd_state.DataValueField = "state_id";
        dpd_state.DataBind();


        qry = "Select * from Towns";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_town.DataSource = dt;
        dpd_town.DataTextField = "towns";
        dpd_town.DataValueField = "town_id";
        dpd_town.DataBind();



        qry = "Select * from Wards";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_ward.DataSource = dt;
        dpd_ward.DataTextField = "wards";
        dpd_ward.DataValueField = "wards_id";
        dpd_ward.DataBind();


        dpd_business_type_SelectedIndexChanged(this, EventArgs.Empty);
    }


    public int insertcompany(string rin)
    {
        float acctbal = 0;
        if (txt_Acct_Bal.Text.Trim() != "")
            acctbal = float.Parse(txt_Acct_Bal.Text.Trim());

        Session["Rin"] = rin;
        if (txt_Company_TIN.Text == "Autogenerated")
            txt_Company_TIN.Text = rin;
        SqlParameter[] pram = new SqlParameter[31];
        pram[0] = new SqlParameter("@company_create_by", 1);
        pram[1] = new SqlParameter("@company_name", txt_company.Text.ToString().Trim());
        pram[2] = new SqlParameter("@company_tin", txt_Company_TIN.Text.Trim());
        pram[3] = new SqlParameter("@mobile_number_1", txt_mob1.Text.Trim());
        pram[4] = new SqlParameter("@mobile_number_2", txt_mob2.Text.Trim());
        pram[5] = new SqlParameter("@email_address_1", txt_Email1.Text.Trim());
        pram[6] = new SqlParameter("@email_address_2", txt_Email2.Text.Trim());
        pram[7] = new SqlParameter("@tax_office", txt_tax_ofc.SelectedValue.Trim());
        pram[8] = new SqlParameter("@tax_payer_type", txt_tax_payer_type.SelectedValue.Trim());
        pram[9] = new SqlParameter("@economic_activity", txt_economic_activity.SelectedValue.Trim());
        pram[10] = new SqlParameter("@preffered_notification_method", txt_preferred_notification.SelectedValue.Trim());
        pram[11] = new SqlParameter("@tax_payer_status", txt_tax_payer_status.SelectedValue.Trim());
        pram[12] = new SqlParameter("@acct_bal", acctbal);
        //  pram[13] = new SqlParameter("@company_rin", txt_RIN.Text.Trim());
        pram[13] = new SqlParameter("@company_rin", rin);
        /***************************************************************************/

        pram[14] = new SqlParameter("@Asset_Type", dpd_asset_type.SelectedValue.Trim());
        pram[15] = new SqlParameter("@Profile", dpd_profile.SelectedValue.Trim());
        pram[16] = new SqlParameter("@Business_Type", dpd_business_type.SelectedValue.Trim());
        pram[17] = new SqlParameter("@Business_Category", dpd_business_category.SelectedValue.Trim());
        pram[18] = new SqlParameter("@Business_Structure", dpd_business_structure.SelectedValue.Trim());
        pram[19] = new SqlParameter("@Business_Sector", dpd_business_sector.SelectedValue.Trim());
        pram[20] = new SqlParameter("@Business_Sub_sector", dpd_business_sub_sector.SelectedValue.Trim());
        pram[21] = new SqlParameter("@Business_Operations", dpd_business_operations.SelectedValue.Trim());
        pram[22] = new SqlParameter("@LGA", dpd_lga.SelectedValue.Trim());

        pram[23] = new SqlParameter("@Add1_hno", txt_house_no.Text.Trim());
        pram[24] = new SqlParameter("@Add2_street", txt_street.Text.Trim());
        pram[25] = new SqlParameter("@Add3_offstreet_town", txt_off_street_town.Text.Trim());
        pram[26] = new SqlParameter("@state", dpd_state.SelectedValue.Trim());

        pram[27] = new SqlParameter("@town", dpd_town.SelectedValue.Trim());

        pram[28] = new SqlParameter("@contact_person", txt_contact_person.Text.Trim());

        pram[29] = new SqlParameter("@ward", dpd_ward.SelectedValue.Trim());

        /***************************************************************************/

        pram[30] = new SqlParameter("@SucessID", 1);
        pram[30].Direction = System.Data.ParameterDirection.Output;
        SqlHelper.ExecuteNonQuery(PAYEClass.connection, CommandType.StoredProcedure, "ADM_INS_CompanyRegistration", pram);
        return int.Parse(pram[30].Value.ToString());
    }
    protected void dpd_business_sector_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = "";
        DataTable dt = new DataTable();

        qry = "Select * from Business_Sub_Sectors where  business_sector=" + dpd_business_sector.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_sub_sector.DataSource = dt;
        dpd_business_sub_sector.DataTextField = "business_sub_sector";
        dpd_business_sub_sector.DataValueField = "bs_sb_id";
        dpd_business_sub_sector.DataBind();
    }
    protected void dpd_business_category_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = "";
        DataTable dt = new DataTable();

        qry = "Select * from Business_Sectors where business_category=" + dpd_business_category.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_sector.DataSource = dt;
        dpd_business_sector.DataTextField = "business_sector";
        dpd_business_sector.DataValueField = "bs_sc_id";
        dpd_business_sector.DataBind();
    }
    protected void dpd_asset_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = "";
        DataTable dt = new DataTable();

        qry = "Select * from Profiles where asset_type=" + dpd_asset_type.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_profile.DataSource = dt;
        dpd_profile.DataTextField = "profile_group";
        dpd_profile.DataValueField = "profile_id";
        dpd_profile.DataBind();
    }
    protected void dpd_business_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = "";
        DataTable dt = new DataTable();

        qry = "Select * from Business_Structure where business_type=" + dpd_business_type.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_structure.DataSource = dt;
        dpd_business_structure.DataTextField = "business_structure";
        dpd_business_structure.DataValueField = "bs_st_id";
        dpd_business_structure.DataBind();

        qry = "Select * from Business_Category where business_type=" + dpd_business_type.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_category.DataSource = dt;
        dpd_business_category.DataTextField = "business_category";
        dpd_business_category.DataValueField = "bs_ct_id";
        dpd_business_category.DataBind();

        qry = "Select * from Business_Operations where business_type=" + dpd_business_type.SelectedValue;
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_operations.DataSource = dt;
        dpd_business_operations.DataTextField = "business_operations";
        dpd_business_operations.DataValueField = "bs_op_id";
        dpd_business_operations.DataBind();



        qry = "Select * from Business_Sectors where business_category=" + dpd_business_category.SelectedValue;
        if (dpd_business_category.SelectedValue == "")
            qry = "Select * from Business_Sectors where business_category=0";


        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_sector.DataSource = dt;
        dpd_business_sector.DataTextField = "business_sector";
        dpd_business_sector.DataValueField = "bs_sc_id";
        dpd_business_sector.DataBind();


        qry = "Select * from Business_Sub_Sectors where  business_sector=" + dpd_business_sector.SelectedValue;

        if (dpd_business_sector.SelectedValue == "")
            qry = "Select * from Business_Sub_Sectors where  business_sector=0";


        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_business_sub_sector.DataSource = dt;
        dpd_business_sub_sector.DataTextField = "business_sub_sector";
        dpd_business_sub_sector.DataValueField = "bs_sb_id";
        dpd_business_sub_sector.DataBind();

        qry = "Select * from Profiles";
        dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_profile.DataSource = dt;
        dpd_profile.DataTextField = "profile_group";
        dpd_profile.DataValueField = "profile_id";
        dpd_profile.DataBind();
    }
    protected void dpd_business_sub_sector_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void rd_Ind_CheckedChanged(object sender, EventArgs e)
    {
        tr_enter_TIN.Style.Add("display", "none");

        txt_company.Enabled = true;
        txt_Company_TIN.Text = "Autogenerated";

        txt_company.Text = "";

        txt_mob1.Text = "";
        txt_mob2.Text = "";
        txt_Email1.Text = "";
        txt_Email2.Text = "";
        txt_Acct_Bal.Text = "";
        txt_RIN.Text = "";
        txt_Company_Created_By.Text = "";
        txt_house_no.Text = "";
        txt_off_street_town.Text = "";

        txt_RIN.Text = "System Generated";
        txt_RIN.Enabled = false;
    }
    protected void rd_search_CheckedChanged(object sender, EventArgs e)
    {
        tr_enter_TIN.Style.Add("display", "");

        txt_RIN.Enabled = false;
        txt_company.Enabled = false;
        txt_Company_TIN.Text = "";
    }
    protected void dpd_lga_SelectedIndexChanged(object sender, EventArgs e)
    {
        string qry = "";
        qry = "Select * from Wards where ";
        DataTable dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        dpd_ward.DataSource = dt;
        dpd_ward.DataTextField = "wards";
        dpd_ward.DataValueField = "wards_id";
        dpd_ward.DataBind();
    }
}