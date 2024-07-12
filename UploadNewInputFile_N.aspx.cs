using Newtonsoft.Json;
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
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

public partial class UploadNewInputFile_N : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    public class Receiver
    {
        public string Success { get; set; }
        // public string Message { get; set; }
        //public string Result { get; set; }

        public List<TaxPayerClassResult> Result { get; set; }

    }
    public class TaxPayerClassResult
    {
        public long TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerRIN { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerMobileNumber { get; set; }
        public string TaxPayerAddress { get; set; }
        public string TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerTypeName { get; set; }
    }
    public class Result
    {
        public string Success { get; set; }
        public string Message { get; set; }
        public string[] Result1 { get; set; }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                binddrop(Session["BusinessRIN"].ToString(), Session["compRIN"].ToString());
            }

            string compRin = Session["compRIN"].ToString();

            drpfupcomapny.SelectedValue = compRin;
            DataTable dt_Business_det = new DataTable();
            SqlDataAdapter Adp = new SqlDataAdapter("Select * from Businesses_API_Main where BusinessRIN='" + Session["BusinessRIN"].ToString() + "'", con);
            Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            Adp.Fill(dt_Business_det);

            Session["BusinessID"] = dt_Business_det.Rows[0]["BusinessID"].ToString();

            drpbusiness.SelectedValue = dt_Business_det.Rows[0]["BusinessRIN"].ToString();
            string qry = "Select * from CompanyList_API where TaxPayerRIN='" + compRin + "'";
            DataTable dt_company = new DataTable();
            dt_company = PAYEClass.fetchdata(qry);

            if (dt_company.Rows.Count > 0)
                Session["TaxOfc"] = dt_company.Rows[0]["TaxOffice"].ToString();
            else
                Session["TaxOfc"] = "FOR REVIEW";


        }
        catch (Exception ex)
        {
            showmsg(2, "Something Went Wrong.");

            // showmsg(2, "1");
        }
    }
    //protected void btnupload_Click(object sender, EventArgs e)
    //{
    //    /* try
    //     {*/
    //    showmsg(3, "");
    //    if (fpemp.HasFile)
    //    {
    //        if (fpemp.PostedFile.FileName.Contains(".xlsx"))
    //        {
    //            string uploadUrl = PAYEClass.uploadurl;
    //            string filePath = uploadUrl + "/docs/" + drpfupcomapny.SelectedValue.Trim() + "_1.xlsx";
    //            HttpPostedFile fileToUpload = fpemp.PostedFile;

    //            if (File.Exists(filePath))
    //            {
    //                File.Delete(filePath);
    //            }

    //            fileToUpload.SaveAs(filePath);
    //            uploademployee();

    //        }
    //        else
    //        {
    //            showmsg(2, "Please upload .xlsx file");
    //        }
    //    }
    //    else
    //    {
    //        showmsg(2, "Please upload a file");
    //    }

    //}

    protected void btnupload_Click(object sender, EventArgs e)
    {
        /* try
         {*/
        showmsg(3, "");
        if (fpemp.HasFile)
        {

            if (fpemp.PostedFile.FileName.Contains(".xlsx"))
            { //for stage since it RDC
                string uploadUrl = PAYEClass.uploadurl;
                string ftpUsername = PAYEClass.ftpusername;
                string ftpPassword = PAYEClass.ftppassword;
                //string filePath = drpfupcomapny.SelectedValue.Trim() + "_1.xlsx";
                string fileName = drpfupcomapny.SelectedValue.Trim() + "_1.xlsx";
                string filePath = Path.Combine(uploadUrl,  fileName);

                HttpPostedFile fileToUpload = fpemp.PostedFile;

                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                fileToUpload.SaveAs(filePath);



                //for live since it FTP

                //using (var client = new WebClient())
                //{
                //    client.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                //    client.UploadFile(uploadUrl, WebRequestMethods.Ftp.UploadFile, filePath);
                //}
                uploademployee();

            }
            else
            {
                showmsg(2, "Please upload .xlsx file");
            }
        }
        else
        {
            showmsg(2, "Please upload a file");
        }

    }

    public void uploademployee()
    {
        string token = "";
        token = PAYEClass.getToken();

        string uploadUrl = PAYEClass.uploadurl;
        //string filepath = uploadUrl + drpfupcomapny.SelectedValue.Trim() + "_1.xlsx";
        string fileName = drpfupcomapny.SelectedValue.Trim() + "_1.xlsx";
        string filePath = Path.Combine(uploadUrl, fileName);

        DataTable dt = new DataTable();
        dt = test22(filePath);
        int st = 0;

        int status = 0;
        string URI1 = "";

        //SS
        foreach (DataRow row1 in dt.Rows)
        {
        repeat:

            if (row1["employer_rin"].ToString() == "")
                goto blankrepeat;
            if (row1["employee_rin"].ToString().Trim() == "" && row1["employee_tin"].ToString().Trim() == "" && row1["employee_phone"].ToString().Trim() == "")

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Something Went Wrong.');</script>", false);

            URI1 = PAYEClass.URL_API + "TaxPayer/SearchTaxPayerByRIN?TaxPayerRIN=" + row1["employee_rin"].ToString().Trim();

            string InsCompRes = "";
            string headers = "";
            using (var wc = new WebClient())
            {
                var test = Session["token"].ToString();
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;

                InsCompRes = wc.DownloadString(URI1);

                var des = (Receiver)JsonConvert.DeserializeObject(InsCompRes, typeof(Receiver));
                //  var des1 = (Result)JsonConvert.DeserializeObject(InsCompRes, typeof(Result));


                DataTable dt_list_ins = new DataTable();

                /******************** Check With Mobile No. Start*****************************/

                if (row1["employee_rin"].ToString().Trim() != "")
                {
                    URI1 = PAYEClass.URL_API + "TaxPayer/SearchTaxPayerByRIN?TaxPayerRIN=" + row1["employee_rin"].ToString().Trim();
                    using (var wc_mobile = new WebClient())
                    {
                        wc_mobile.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        wc_mobile.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                        InsCompRes = wc.DownloadString(URI1);
                        des = (Receiver)JsonConvert.DeserializeObject(InsCompRes, typeof(Receiver));
                    }
                }
                else if (row1["employee_phone"].ToString().Trim() != "")
                {
                    URI1 = PAYEClass.URL_API + "TaxPayer/SearchTaxPayerByMobileNumber?MobileNumber=" + row1["employee_phone"].ToString().Trim();
                    using (var wc_mobile = new WebClient())
                    {
                        wc_mobile.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        wc_mobile.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                        InsCompRes = wc.DownloadString(URI1);
                        if (InsCompRes != null)
                        {
                            des = (Receiver)JsonConvert.DeserializeObject(InsCompRes, typeof(Receiver));
                        }
                    }

                }
                else if (row1["employee_tin"].ToString().Trim() != "")
                {
                    URI1 = PAYEClass.URL_API + "TaxPayer/SearchTaxPayerByTIN?TaxPayerTIN=" + row1["employee_tin"].ToString().Trim();
                    using (var wc_mobile = new WebClient())
                    {
                        wc_mobile.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        wc_mobile.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                        InsCompRes = wc.DownloadString(URI1);
                        des = (Receiver)JsonConvert.DeserializeObject(InsCompRes, typeof(Receiver));
                        //  var des1 = (Result)JsonConvert.DeserializeObject(InsCompRes, typeof(Result));

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Something Went Wrong.');</script>", false);
                }
                JObject jObject = new JObject();
                string taxPayerId = "";
                string taxPayerName = "";
                if (des != null && (des.Success).ToLower().ToString() == "true")
                {
                    jObject = JObject.Parse(InsCompRes);
                    // token = PAYEClass.getToken();
                    if (InsCompRes != "" && InsCompRes.Split('[')[1].ToString() != "]}" && (jObject["Result"][0]["TaxPayerTypeID"]).ToString().Trim() == "1")
                    {
                        taxPayerId = (jObject["Result"][0]["TaxPayerID"]).ToString();
                        taxPayerName = (jObject["Result"][0]["TaxPayerName"]).ToString();
                        string[] res_AddInd_Business_API;
                        string URI_AddInd_Business_API = PAYEClass.URL_API + "TaxPayerAsset/AddBusinessToIndividual";


                        //string myParameters_AddInd_Business_API = "{\"TaxPayerID\": " + dt_list_api.Rows[0]["TaxPayerID"].ToString() + ", \"AssetID\": " + Session["BusinessID"].ToString() + ", \"TaxPayerRoleID\": " + dt_list_api.Rows[0]["TaxPayerTypeID"].ToString() + "}";
                        string myParameters_AddInd_Business_API = "{\"TaxPayerID\": " + taxPayerId + ", \"AssetID\": " + Session["BusinessID"].ToString().Trim() + ", \"TaxPayerRoleID\": " + 7 + "}";
                        string InsCompRes_AddIndBusinessAPI = "";
                        try
                        {
                            using (WebClient wc_AddInd_Business_API = new WebClient())
                            {
                                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                                string json = JsonConvert.SerializeObject(myParameters_AddInd_Business_API);
                                // InsCompRes_AddIndBusinessAPI = wc.UploadString(URI_AddInd_Business_API, json);
                                InsCompRes_AddIndBusinessAPI = wc.UploadString(new Uri(URI_AddInd_Business_API), "POST", json);

                                res_AddInd_Business_API = InsCompRes_AddIndBusinessAPI.Split('"');
                            }

                        }
                        catch (Exception eexe)
                        {

                        }

                        string enviro = PAYEClass.enviro;
                        if (enviro == "l")
                        {
                            string[] res_Input_API;
                            string URI_Input_API = PAYEClass.URL_API + "DataWarehouse/PAYEInput/Insert";
                            string tax_of = "For Review";
                            tax_of = Session["TaxOfc"].ToString();
                            string myParameters_Input_API = "{\n    \"TranscationDate\": \"" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "\",\n    \"Employer_RIN\": \"" + drpbusiness.SelectedValue.ToString() + "\",\n    \"Employee_RIN\": \"" + jObject["Result"][0]["TaxPayerRIN"].ToString().Trim() + "\",\n    \"Assessment_Year\": " + row1["assessment_year"].ToString().Trim() + ",\n    \"Start_Month\": " + DateTime.ParseExact(row1["start_month"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"End_Month\": " + DateTime.ParseExact(row1["end_month"].ToString().Substring(0, 3), "MMM", CultureInfo.InvariantCulture).Month + ",\n    \"Annual_Basic\": " + row1["annual_basic"].ToString() + ",\n    \"Annual_Rent\": " + row1["annual_rent"].ToString() + ",\n    \"Annual_Transport\": " + row1["annual_transport"].ToString() + ",\n    \"Annual_Utility\": " + row1["annual_utility"].ToString() + ",\n    \"Annual_Meal\": " + row1["annual_meal"].ToString() + ",\n    \"Other_Allowances_Annual\": " + row1["other_allowances_annual"].ToString().Trim() + ",\n    \"Leave_Transport_Grant_Annual\": " + row1["leave_transport_grant_annual"].ToString().Trim() + ",\n    \"pension_contribution_declared\": " + row1["pension_contribution_declared"].ToString().Trim() + ",\n    \"nhf_contribution_declared\": " + row1["nhf_contribution_declared"].ToString().Trim() + ",\n    \"nhis_contribution_declared\": " + row1["nhis_contribution_declared"].ToString().Trim() + ",\n    \"Tax_Office\":\"" + tax_of + "\"\n}";

                            string InsCompRes_InputAPI = "";
                            using (WebClient wc_Input_API = new WebClient())
                            {
                                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                                wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                                string json = JsonConvert.SerializeObject(myParameters_Input_API);
                                InsCompRes_InputAPI = wc.UploadString(new Uri(URI_Input_API), "POST", json);

                                res_Input_API = InsCompRes_InputAPI.Split('"');
                            }
                        }
                    }
                    else
                    {
                        taxPayerId = "1";
                        string[] res_AddInd_API;
                        string URI_AddInd_API = PAYEClass.URL_API + "TaxPayer/Individual/Insert";
                        //  = PAYEClass.URL_API + "TaxPayer/Individual/Insert";
                        string myParameters_AddInd_API = "{\n    \"TaxPayerTypeId\":1,\n    \"GenderID\": 1,\n    \"TitleID\": 2,\n    \"FirstName\": \"" + row1["first_name"].ToString().Trim() + "\",\n    \"LastName\": \"" + row1["surname"].ToString().Trim() + "\",\n    \"MiddleName\": \"" + row1["middle_name"].ToString().Trim() + "\",\n    \"DOB\": \"01/01/2004\",\n    \"TIN\": \"" + row1["employee_tin"].ToString().Trim() + "\",\n    \"MobileNumber1\": \"" + row1["employee_phone"].ToString().Trim() + "\",\n    \"EmailAddress1\": \"abc@gmail.com\",\n    \"BiometricDetails\": \"\",\n    \"TaxOfficeID\": 34,\n    \"MaritalStatusID\": 3,\n    \"NationalityID\": 1,\n    \"EconomicActivitiesID\": 13,\n    \"NotificationMethodID\": 3,\n    \"ContactAddress\": \"None Listed\"\n}";

                        string InsCompRes_AddIndAPI = "";
                        using (WebClient wc_AddInd_API = new WebClient())
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                            wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                            InsCompRes_AddIndAPI = wc.UploadString(new Uri(URI_AddInd_API), "POST", myParameters_AddInd_API);

                            res_AddInd_API = InsCompRes_AddIndAPI.Split('"');
                            if (res_AddInd_API[2].ToString() == ":false,")
                            {
                                showmsg(2, "Something Wrong in Sheet:" + res_AddInd_API[5].ToString().Trim());
                                return;
                            }
                        }
                        goto repeat;
                    }

                }
                double kk;
                {
                    if (String.IsNullOrEmpty(row1["annual_basic"].ToString()))
                        row1["annual_basic"] = "0";

                    if (String.IsNullOrEmpty(row1["annual_rent"].ToString()))
                        row1["annual_rent"] = "0";

                    if (String.IsNullOrEmpty(row1["annual_transport"].ToString()))
                        row1["annual_transport"] = "0";


                    if (String.IsNullOrEmpty(row1["annual_utility"].ToString()))
                        row1["annual_utility"] = "0";

                    if (String.IsNullOrEmpty(row1["annual_meal"].ToString()))
                        row1["annual_meal"] = "0";

                    if (String.IsNullOrEmpty(row1["other_allowances_annual"].ToString()))
                        row1["other_allowances_annual"] = "0";

                    if (String.IsNullOrEmpty(row1["leave_transport_grant_annual"].ToString()))
                        row1["leave_transport_grant_annual"] = "0";

                    if (String.IsNullOrEmpty(row1["pension_contribution_declared"].ToString()))
                        row1["pension_contribution_declared"] = "0";

                    if (String.IsNullOrEmpty(row1["nhf_contribution_declared"].ToString()))
                        row1["nhf_contribution_declared"] = "0";


                    if (String.IsNullOrEmpty(row1["nhis_contribution_declared"].ToString()))
                        row1["nhis_contribution_declared"] = "0";

                    kk = (Convert.ToDouble(row1["annual_basic"]) + Convert.ToDouble(row1["annual_rent"]) + Convert.ToDouble(row1["annual_transport"]) + Convert.ToDouble(row1["annual_utility"])
                        + Convert.ToDouble(row1["annual_meal"]) + Convert.ToDouble(row1["leave_transport_grant_annual"]) + Convert.ToDouble(row1["other_allowances_annual"]));

                }

                SqlParameter[] pram = new SqlParameter[32];

                pram[0] = new SqlParameter("@TaxPayerID", taxPayerId);
                pram[1] = new SqlParameter("@TaxPayerTypeID", 1);

                string taxpayerTypeName = "";
                if (jObject["Result"][0]["TaxPayerTypeID"].ToString() == "1")
                    taxpayerTypeName = "Individual";

                pram[2] = new SqlParameter("@TaxPayerTypeName", taxpayerTypeName);
                pram[3] = new SqlParameter("@TaxPayerName", jObject["Result"][0]["TaxPayerName"].ToString());
                pram[4] = new SqlParameter("@TaxPayerRIN", jObject["Result"][0]["TaxPayerRIN"].ToString().Trim());
                pram[5] = new SqlParameter("@MobileNumber", jObject["Result"][0]["TaxPayerMobileNumber"].ToString().Trim());
                pram[6] = new SqlParameter("@ContactAddress", jObject["Result"][0]["TaxPayerAddress"].ToString().Trim());
                pram[7] = new SqlParameter("@EmailAddress", "");
                pram[8] = new SqlParameter("@TIN", row1["employee_tin"].ToString().Trim());

                pram[9] = new SqlParameter("@employerName", drpfupcomapny.SelectedItem.Text);
                pram[10] = new SqlParameter("@employerAddress", row1["employer_address"].ToString().Trim());
                pram[11] = new SqlParameter("@employerRIN", drpfupcomapny.SelectedValue.ToString());
                pram[12] = new SqlParameter("@StartMonth", row1["start_month"].ToString());
                pram[13] = new SqlParameter("@Nationality", row1["nationality"].ToString());
                pram[14] = new SqlParameter("@Title", row1["title"].ToString());
                pram[15] = new SqlParameter("@FirstName", row1["first_name"].ToString().Trim());
                pram[16] = new SqlParameter("@Surname", row1["surname"].ToString().Trim());
                pram[17] = new SqlParameter("@Assessment_Year", row1["assessment_year"].ToString().Trim());
                pram[18] = new SqlParameter("@EndMonth", row1["end_month"].ToString().Trim());

                pram[19] = new SqlParameter("@Basic", row1["annual_basic"].ToString());
                pram[20] = new SqlParameter("@Rent", row1["annual_rent"].ToString());
                pram[21] = new SqlParameter("@Trans", row1["annual_transport"].ToString());
                pram[22] = new SqlParameter("@Gross", kk.ToString());
                pram[23] = new SqlParameter("@LTG", row1["leave_transport_grant_annual"].ToString().Trim());
                pram[24] = new SqlParameter("@AnnualUtility", row1["annual_utility"].ToString().Trim());
                pram[25] = new SqlParameter("@AnnualMeal", row1["annual_meal"].ToString().Trim());
                pram[26] = new SqlParameter("@Others", row1["other_allowances_annual"].ToString().Trim());

                pram[27] = new SqlParameter("@Pension", row1["pension_contribution_declared"].ToString().Trim());
                pram[28] = new SqlParameter("@NHF", row1["nhf_contribution_declared"].ToString().Trim());
                pram[29] = new SqlParameter("@NHIS", row1["nhis_contribution_declared"].ToString().Trim());
                pram[30] = new SqlParameter("@BusinessRIN", drpbusiness.SelectedValue.ToString());

                pram[31] = new SqlParameter("@SucessID", 1);
                pram[31].Direction = System.Data.ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(PAYEClass.connection, CommandType.StoredProcedure, "Adm_INS_UploadXL_API", pram);
                status += int.Parse(pram[31].Value.ToString());

                row1["employee_rin"] = jObject["Result"][0]["TaxPayerRIN"].ToString().Trim();

            }
        blankrepeat: { }
        }

        DataTable dt_list_inputfile = new DataTable();
        SqlDataAdapter Adp_inputfile = new SqlDataAdapter("select (firstname+' '+surname) as Name, EmployeeRIN as taxpayerRIN,EmployeeTIN as tp_TIN, Assessment_Year as Tax_Year, AnnualGross, EmployerName as CompanyName, EmployerRIN as CompanyRIN, EmployerAddress as ContactAddress,(CASE WHEN (endmonth is NULL) then 'Active' else 'Active' end) as Active from payeInputfile  where EmployerRIN='" + Session["compRIN"].ToString() + "' and Assessment_Year='" + Session["Tax_Year"].ToString() + "'", con);
        Adp_inputfile.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp_inputfile.Fill(dt_list_inputfile);

        Session["dt_l"] = dt_list_inputfile;

        //foreach (DataRow row1 in dt.Rows)
        //{
        //    if (row1["employee_rin"].ToString() != "")
        //        st = 0;
        //    foreach (DataRow row2 in ((DataTable)Session["dt_l"]).Rows)
        //    {
        //        if (row1["employee_rin"].ToString() == row2["taxpayerRIN"].ToString())
        //        {
        //            st = 1;
        //        }

        //    }

        //    if (st == 0)
        //    {
        //        // showmsg(2, "Any EmployeeRIN not exist in database. Please Add First.");

        //        showmsg(2, "Employee RIN - " + row1["employee_rin"].ToString() + " Salary Details Not Uploaded.");
        //        return;
        //    }

        //}

        showmsg(1, "File Uploaded Successfully.");

    }

    public DataTable getdatatable(string filename)
    {
        DataTable dt = new DataTable();
        Workbook workbook = new Workbook();
        workbook.LoadFromFile(filename);
        Worksheet sheet = workbook.Worksheets[0];
        dt = sheet.ExportDataTable(sheet.Range, true, true);
        return dt;
    }

    public void binddrop(string bizRin, string compRin)
    {
        string qry = "Select distinct TaxPayerRIN as CompanyRIN, TaxPayerName as CompanyName  from CompanyList_API  where TaxPayerRIN = '" + compRin + "'"; ;
        DataTable dt = new DataTable();
        dt = PAYEClass.fetchdata(qry);
        drpfupcomapny.DataSource = dt;
        drpfupcomapny.DataTextField = "CompanyName";
        drpfupcomapny.DataValueField = "CompanyRIN";
        drpfupcomapny.DataBind();

        dt = null;
        // qry = "Select distinct BusinessRIN_1,BusinessName from vw_InputFile";

        qry = "select distinct BusinessRIN as BusinessRIN_1,BusinessName  from Businesses_API_Main where BusinessRIN = '" + bizRin + "'";
        dt = PAYEClass.fetchdata(qry);
        drpbusiness.DataSource = dt;
        drpbusiness.DataTextField = "BusinessName";
        drpbusiness.DataValueField = "BusinessRIN_1";
        drpbusiness.DataBind();
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

    public DataTable test22(string filename)
    {
        // Load the Excel package from the file path
        using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filename)))
        {
            // Assuming there is only one worksheet in the Excel file
            var worksheet = package.Workbook.Worksheets[0];

            // Read the Excel data into a DataTable
            DataTable dt = new DataTable();

            // Loop through the rows of the worksheet and add them to the DataTable
            foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
            {
                dt.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                var row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                var newRow = dt.NewRow();

                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                dt.Rows.Add(newRow);
            }

            // Now 'dt' DataTable contains the Excel data
            return dt;
        }

    }

}