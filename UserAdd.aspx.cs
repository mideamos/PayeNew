using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class UserAdd : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            binddropdown();
        }
    }

    public void binddropdown()
    {
        DataTable dt_list = new DataTable();

        SqlDataAdapter Adp = new SqlDataAdapter("SELECT Role,RoleId From PayeRole", con);
        Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp.Fill(dt_list);
        drptitle.DataSource = dt_list;
        drptitle.DataTextField = "Role";
        drptitle.DataValueField = "RoleId";
        drptitle.DataBind();
    }


    protected void btn_add_user_Click(object sender, EventArgs e)
    {
        // GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        string email = txtemail1.Text.ToString().Trim();
        string firstname = txtfirstname.Text.ToString().Trim();
        string lastname = txtlastname.Text.ToString().Trim();
        string userName = txtuserName.Text.ToString().Trim();
        string design = txtdesign.Text.ToString().Trim();
        string middlename = txtmiddlename.Text.ToString().Trim();
        string phone = txtphone1.Text.ToString().Trim();
        string password = txtpassword.Text.ToString().Trim();
        string passwor1d = txtpassword2.Text.ToString().Trim();
        string roleId = drptitle.SelectedItem.Value.Trim();
        if (password != passwor1d)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Password and Confirm Password Not Same.');</script>", false);
            return;
        }
        SqlConnection con = new SqlConnection(PAYEClass.connection.ToString());

        try
        {
            string loginqry = "Select * from AdminUser where Email=@a and FirstName=@b and UserName = @c";
            SqlCommand cmd = new SqlCommand(loginqry, con);
            cmd.Parameters.AddWithValue("@a", email);
            cmd.Parameters.AddWithValue("@b", firstname);
            cmd.Parameters.AddWithValue("@c", userName);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
            da.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('User Already Exist.');</script>", false);
                return;
            }

            DateTime currDate = DateTime.Now;
            string CurrentDate = currDate.ToString("MM-dd-yyyy");

            string q1 = "insert into AdminUser ([PayeUserTypeId],[RoleId],[Password],[Email],[FirstName],[LastName],[MiddleName],[Phone],[Designation],Username,CreatedBy,CreateddDate, ModifiedBy, ModifiedDate) Values(1,@RoleId,@Password,@Email,@FirstName,@LastName,@MiddleName,@Phone,@Designation,@Username,@CreatedBy,@CreateddDate, @ModifiedBy, @ModifiedDate)";
            SqlCommand cmd2 = new SqlCommand(q1, con);
            cmd2.Parameters.AddWithValue("@RoleId", roleId);
            cmd2.Parameters.AddWithValue("@Email", email);
            cmd2.Parameters.AddWithValue("@FirstName", firstname);
            cmd2.Parameters.AddWithValue("@LastName", lastname);
            cmd2.Parameters.AddWithValue("@MiddleName", middlename);
            cmd2.Parameters.AddWithValue("@Phone", phone);
            cmd2.Parameters.AddWithValue("@Designation", design);
            cmd2.Parameters.AddWithValue("@Password", password);
            cmd2.Parameters.AddWithValue("@Username", userName);
            cmd2.Parameters.AddWithValue("@CreatedBy", userName);
            cmd2.Parameters.AddWithValue("@CreateddDate", CurrentDate);
            cmd2.Parameters.AddWithValue("@ModifiedBy", "");
            cmd2.Parameters.AddWithValue("@ModifiedDate", "");
            con.Open();
            cmd2.ExecuteNonQuery();
            con.Close();

            txtemail1.Text = string.Empty;
            txtfirstname.Text = string.Empty;
            txtlastname.Text = string.Empty;
            txtuserName.Text = string.Empty;
            txtdesign.Text = string.Empty;
            txtmiddlename.Text = string.Empty;
            txtphone1.Text = string.Empty;
            txtpassword.Text = string.Empty;
            txtpassword2.Text = string.Empty;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('User Added Successfully');</script>", false);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "hideImage()", true);
            con.Close();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Unable To Add user');</script>", false);
            return;
        }
    }
}