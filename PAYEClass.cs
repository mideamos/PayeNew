using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Schema;
using System.Configuration;
/// <summary>
/// Summary description for PAYEClass
/// </summary>
public static class PAYEClass
{

    public static string enviro = System.Configuration.ConfigurationManager.AppSettings["enviro"].ToString();
    public static string connection = System.Configuration.ConfigurationManager.ConnectionStrings["constring"].ToString();
    public static int defaultTimeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["defaultTImeout"].ToString());
    public static string currency = System.Configuration.ConfigurationManager.AppSettings["currency"].ToString();
    public static string uploadurl = System.Configuration.ConfigurationManager.AppSettings["uploadurl"].ToString();
    public static string ftpusername = System.Configuration.ConfigurationManager.AppSettings["ftpusername"].ToString();
    public static string ftppassword = System.Configuration.ConfigurationManager.AppSettings["ftppassword"].ToString();
    public static string username = System.Configuration.ConfigurationManager.AppSettings["username"].ToString();
    public static string password = System.Configuration.ConfigurationManager.AppSettings["password"].ToString();
    public static string uploadurltxtfile = System.Configuration.ConfigurationManager.AppSettings["uploadurltxtfile"].ToString();


    public class Token
    {
        public string access_token { get; set; }
    }

    //public static string URL_API = "https://stage-api.eirsautomation.xyz/";
    //public static string URL_API = "https://api.eirs.gov.ng/";
    //public static string URL_API = "http://localhost:56892/";

    public static string URL_API = ConfigurationManager.AppSettings["ApiBaseUrl"];


    public static DataTable ToDataTable<T>(this IList<T> data)
    {
        PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object[] values = new object[props.Count];
        foreach (T item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (props[i].PropertyType == typeof(DateTime))
                {
                    DateTime currDT = (DateTime)props[i].GetValue(item);
                    values[i] = currDT.ToUniversalTime();
                }
                else
                {
                    values[i] = props[i].GetValue(item);
                }
            }
            table.Rows.Add(values);
        }
        return table;
    }


    //public static String getToken()
    //{
    //    String access_token = null;

    //    if (HttpContext.Current.Session["token"] == null)
    //    {
    //        //SqlConnection con = new SqlConnection(PAYEClass.connection);
    //        //con.Open();

    //        //DataTable dt_list = new DataTable();
    //        //SqlDataAdapter Adp = new SqlDataAdapter("select * from TokenManagement", con);
    //        //Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;

    //        //Adp.Fill(dt_list);
    //        //con.Close();

    //        //if (dt_list.Rows.Count > 0)
    //        //{
    //        //    DataRow row = dt_list.Rows[0];
    //        //    DateTime date = DateTime.Parse(row["createdAt"].ToString()).Date;
    //        //    var todayDate = DateTime.Now.Date;

    //        //    if (todayDate.CompareTo(date) >= 0)
    //        //    {
    //        //        access_token = row["token"]+"";
    //        //    }
    //        //}

    //        if (access_token == null)
    //        {
    //            string URI = PAYEClass.URL_API + "Account/Login";
    //            string user = PAYEClass.username;
    //            string password = PAYEClass.password;
    //            string myParameters = "UserName=" + user + "&Password=" + password + "&grant_type=password";
    //            string BearerToken = "";
    //            using (WebClient wc = new WebClient())
    //            {
    //                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
    //                BearerToken = wc.UploadString(URI, myParameters);
    //            }

    //            Token TokenObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(BearerToken);
    //            access_token = TokenObj.access_token;

    //            //SqlConnection con1 = new SqlConnection(PAYEClass.connection);
    //            //con1.Open();
    //            //SqlCommand cmd = new SqlCommand("delete from TokenManagement; insert into TokenManagement (token) values (" +
    //            //     "'" + access_token + "')", con1);

    //            //cmd.ExecuteNonQuery();
    //            //con1.Close();
    //        }

    //    }
    //    else
    //    {
    //        access_token = HttpContext.Current.Session["token"].ToString();
    //    }
    //    HttpContext.Current.Session["token"] = access_token;
    //    return access_token;
    //}
    public static String getToken()
    {

        string URI = PAYEClass.URL_API + "Account/Login";
        string user = PAYEClass.username;
        string password = PAYEClass.password;
        string myParameters = "UserName=" + user + "&Password=" + password + "&grant_type=password";
        string BearerToken = "";
        using (WebClient wc = new WebClient())
        {
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            BearerToken = wc.UploadString(URI, myParameters);
        }

        Token TokenObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(BearerToken);
        string access_token = TokenObj.access_token;

        return access_token;
    }
    public static DataTable fetchdata(string query)
    {
        SqlConnection con = new SqlConnection(connection);
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(query, con);
        da.SelectCommand.CommandTimeout = defaultTimeout;
        DataTable dt = new DataTable();
        using (SqlCommand cmd = new SqlCommand(query, con))


        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        {
            cmd.CommandTimeout = 0;
            adapter.Fill(dt);
        }

        // da.Fill(dt);
        con.Close();
        con.Dispose();
        return dt;


    }

    public static MemoryStream DataTableToExcelXlsx(DataTable table, string sheetName)
    {
        MemoryStream result = new MemoryStream();
        ExcelPackage excelpack = new ExcelPackage();
        ExcelWorksheet worksheet = excelpack.Workbook.Worksheets.Add(sheetName);

        int col = 1;
        int row = 1;
        foreach (DataColumn column in table.Columns)
        {
            var headerCells = worksheet.Cells[row, col];
            worksheet.Cells[row, col].Value = column.ColumnName.ToString();
            // Set their foreground color (text color) to White.
            headerCells.Style.Font.Color.SetColor(System.Drawing.Color.White);
            // Set their background color to DarkBlue.
            headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.DarkBlue);
            col++;
        }
        col = 1;
        row = 2;
        foreach (DataRow rw in table.Rows)
        {
            foreach (DataColumn cl in table.Columns)
            {
                if (rw[cl.ColumnName] != DBNull.Value)
                    worksheet.Cells[row, col].Value = rw[cl.ColumnName].ToString();
                col++;
            }
            row++;
            col = 1;
        }
        excelpack.SaveAs(result);
        return result;
    }
    public static DataTable returnDataTable()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("uspGetPreAssessment", con);
                //cmd.Parameters.Add("",SqlDbType.Int);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@intStatus", "3");
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                cmd.Parameters.Add(param);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                return null;
            }
        }
        catch
        {
            return null;
        }
    }


    public static int insertupdateordelete(string query)
    {
        SqlConnection con = new SqlConnection(connection);
        SqlCommand cmd = new SqlCommand(query, con);
        con.Open();
        int status = cmd.ExecuteNonQuery();
        con.Close();
        return status;
    }

    public static string base64Decode(string data)
    {
        try
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(data);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception("Error in base64Decode" + e.Message);
        }
    }

    public static string generaterin(string name, string comprin, string dob)
    {
        string first2 = name.ToString().Substring(0, 2);
        string middle2 = comprin.Substring(0, 2);
        Random rnd = new Random();
        string rin = dob + first2 + middle2 + rnd.Next(111, 999).ToString();
        return rin;
    }

    public static string base64Encode(string data)
    {
        try
        {
            byte[] encData_byte = new byte[data.Length];
            encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception e)
        {
            throw new Exception("Error in base64Encode" + e.Message);
        }
    }

    public static double computeformula(double basic, double rent, double trans, string type)
    {
        double output = 0;
        // string qry = "Select RuleFormula,RuleCode from RuleEngine where RuleStatus='A' and RuleDesc='" + type + "'";
        // DataTable dt = new DataTable();
        // dt = fetchdata(qry);
        //  if (dt.Rows.Count > 0)
        //  {
        //     string formula = dt.Rows[0]["RuleFormula"].ToString().Trim();
        //     formula = formula.Replace("B", basic.ToString());
        //     formula = formula.Replace("R", rent.ToString());
        //     formula = formula.Replace("T", trans.ToString());

        //    DataTable dtfinal = new DataTable();
        // output = (double)dtfinal.Compute(formula, "");
        if (type == "NHF")
            output = ((2.5 * basic) / 100);
        //output = (basic * (2.5 / 100));

        if (type == "NHIS")
            output = ((5 * basic) / 100);

        if (type == "PENSION")
            output = ((8 * (basic + rent + trans)) / 100);

        //  }

        return output;
    }

    public static double[] calculatetax(double annualIncome, double basic, double rent, double trans, double pension, double pensiondeclared, double nhf, double nhfdeclared, double nhis, double nhisdeclared, int no_of_months)
    {
        double finalcra = 0;
        double finalpension = 0;
        double finalnhf = 0;
        double finalnhis = 0;

        if (pension != 1)
        {
            finalpension = pensiondeclared;
        }
        else
        {
            if (pensiondeclared != 0)
            {
                finalpension = computeformula(basic, rent, trans, "PENSION");
            }
        }

        if (nhf != 1.0)
        {
            finalnhf = nhfdeclared;
        }
        else
        {
            if (nhfdeclared != 0)
            {
                finalnhf = computeformula(basic, rent, trans, "NHF");
            }
        }

        if (nhis != 1.0)
        {
            finalnhis = nhisdeclared;
        }
        else
        {
            if (nhisdeclared != 0)
            {
                finalnhis = computeformula(basic, rent, trans, "NHIS");
            }
        }

        double tax_exempt = finalpension + finalnhf + finalnhis;

        double gross = annualIncome - tax_exempt;
        double cra1 = ((gross * (20)) / 100) + 200000;
        double cra2 = ((gross * (20)) / 100) + ((gross * (1)) / 100);

        if (gross > 20000000)
        {
            finalcra = cra2;
        }
        else
        {
            finalcra = cra1;
        }

        double tax_free_pay = finalcra + tax_exempt;
        double chargableincome = annualIncome - tax_free_pay;

        double annualtax = calculate_tax(chargableincome, gross);
        double monthlytax = annualtax / 12;
        annualtax = monthlytax * no_of_months; // use months to get the correct tax


        double[] charges = new double[8];
        charges[0] = Math.Round(finalcra, 2);
        charges[1] = Math.Round(finalpension, 2);
        charges[2] = Math.Round(finalnhf, 2);
        charges[3] = Math.Round(finalnhis, 2);
        charges[4] = Math.Round(tax_free_pay, 2);
        charges[5] = Math.Round(chargableincome, 2);
        charges[6] = Math.Round(annualtax, 2);
        charges[7] = Math.Round(monthlytax, 2);

        return charges;
    }

    public static double calculate_tax(double ch_income, double gross)
    {
        double calc_tax = 0;
        double min_tax = 0;
        double finaltax = 0;

        if (ch_income <= 0)
        {
            calc_tax = ((gross * 1) / 100);
        }
        else if (ch_income <= 300000)
        {
            calc_tax = (ch_income * (7)) / 100;
        }
        else if (ch_income <= 600000)
        {
            calc_tax = (((ch_income - 300000) * (11)) / 100) + 21000;
        }
        else if (ch_income <= 1100000)
        {
            calc_tax = (((ch_income - 600000) * (15)) / 100) + 54000;
        }
        else if (ch_income <= 1600000)
        {
            calc_tax = (((ch_income - 1100000) * (19)) / 100) + 129000;
        }
        else if (ch_income <= 3200000)
        {
            calc_tax = (((ch_income - 1600000) * (21)) / 100) + 224000;
        }
        else
        {
            calc_tax = (((ch_income - 3200000) * (24)) / 100) + 560000;
        }
        min_tax = (gross * (1)) / 100;

        if (calc_tax < min_tax)
        {
            finaltax = min_tax;
        }
        else
        {
            finaltax = calc_tax;
        }
        return finaltax;
    }

    public static string generateAssessmentNo(string companyrin)
    {
        Random rnd = new Random();
        return companyrin + DateTime.Now.Year + rnd.Next(11111, 99999);
    }

    public static DataTable processAPI(string api_url, string parameters, string token)
    {
        string[] res;

        string URI = api_url;
        string myParameters = parameters;

        string InsCompRes = "";
        using (WebClient wc = new WebClient())
        {
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
            InsCompRes = wc.DownloadString(URI);
            res = InsCompRes.Split('"');

        }
        DataTable dtprocessed = (DataTable)JsonConvert.DeserializeObject("[" + InsCompRes.Split('[')[1].Replace("}]", "") + "]", (typeof(DataTable)));
        return dtprocessed;
    }

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
