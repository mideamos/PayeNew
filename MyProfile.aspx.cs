using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.UI;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit.HTMLEditor.ToolbarButton;

public partial class MyProfile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            //CheckUserRole();
            if (Request.QueryString["emailEMP"] != null)
            {
                string email = Request.QueryString["emailEMP"].ToString();
                drpusername_SelectedIndexChanged(email);
            }
            else
                drpusername_SelectedIndexChanged("");
        }

    }
    private void CheckUserRole()
    {
        string RoleId = Session["roleId"].ToString();

        if (RoleId != "1")
        {
            lblMessage.Text = "You don't have access here.";
            lblMessage.Visible = true;
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('You don't have access here.');</script>", false);
            Response.Redirect("dashboard.aspx");
        }

    }
    protected void drpusername_SelectedIndexChanged(string em)
    {
        string id = Session["user_id"].ToString(); string loginqry = "";
        if (em == "" || em == null)
            loginqry = "SELECT  [AdminUserId],P.UserType,R.Role,[Password],[Username],[Email],[FirstName] ,[LastName],IsActive,[Designation],[Phone]  FROM [AdminUser] A LEFT JOIN PayeRole R on A.RoleId = R.RoleId  LEFT JOIN PayeUserType P on A.PayeUserTypeId = P.UserTypeId  WHERE A.AdminUserId ='" + id + "'";
        else
            loginqry = "SELECT  [AdminUserId],P.UserType,R.Role,[Password],[Username],[Email],[FirstName] ,[LastName],IsActive,[Designation],[Phone]  FROM [AdminUser] A LEFT JOIN PayeRole R on A.RoleId = R.RoleId  LEFT JOIN PayeUserType P on A.PayeUserTypeId = P.UserTypeId  WHERE A.Email ='" + em + "'";

        SqlConnection con = new SqlConnection(PAYEClass.connection);
        SqlCommand cmd = new SqlCommand(loginqry, con);
        System.Data.DataTable dt = new System.Data.DataTable();
        con.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        da.Fill(dt);
        con.Close();
        if (dt.Rows.Count > 0)
        {
            txt_fname.Text = dt.Rows[0]["FirstName"].ToString();
            txt_lname.Text = dt.Rows[0]["LastName"].ToString();
            txt_phn.Text = dt.Rows[0]["Phone"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            Session["email"] = txt_email.Text;
            //txt_password.Text = dt.Rows[0]["Password"].ToString();
            txt_password.Text = "******";
        }
        else
        {
            txt_fname.Text = "";
            txt_lname.Text = "";
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
     
        string id = Session["user_id"].ToString();
        SqlConnection con = new SqlConnection(PAYEClass.connection.ToString());

        string password = txt_new_password.Text.ToString().Trim();
        string conpassword = txt_con_password.Text.ToString().Trim();
        if (password != conpassword)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Unable To Update user As Confirm Password Not Same With Password');</script>", false);
            return;
        }
        try
        {
            string email = "";
            if (Request.QueryString["emailEMP"] != null && !string.IsNullOrEmpty(Request.QueryString["emailEMP"].ToString()))
            {
                email = Request.QueryString["emailEMP"].ToString();
            }
            else if (Session["email"] != null)
            {
                email = Session["email"].ToString();
            }

            string username = Session["username"].ToString();
            DateTime currDate = DateTime.Now;
            string CurrentDate = currDate.ToString("MM-dd-yyyy");
            string query = " update AdminUser set Password = @Password, ModifiedBy = '"+ username + "', ModifiedDate = '"+ CurrentDate + "' WHERE Email ='" + email + "'";
            //SqlCommand cmd2 = new SqlCommand(q1, con);
            //cmd2.Parameters.AddWithValue("@Password", password);
            //con.Open();
            //cmd2.ExecuteNonQuery();
            //con.Close();
          
            using (SqlConnection con1 = new SqlConnection(PAYEClass.connection))
            {
                con1.Open();

                System.Data.DataTable dt5;

                if (!string.IsNullOrEmpty(query))
                {
                    dt5 = GetqueryDate(query, password);
                }

                con1.Close();
                txt_new_password.Text = string.Empty; 
                txt_con_password.Text= string.Empty;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('User Updated Successfully');</script>", false);

            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);
            con.Close();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Unable To Update user');</script>", false);
            return;
        }
    }
    public static System.Data.DataTable GetqueryDate(string query, string Password)
    {
        using (SqlConnection con = new SqlConnection(PAYEClass.connection))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Password", Password);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                System.Data.DataTable dt1 = new System.Data.DataTable();
                adapter.Fill(dt1);
                return dt1;
            }
        }
    }
}