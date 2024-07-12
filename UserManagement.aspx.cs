using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserManagement : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(PAYEClass.connection);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["user_id"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            getAll();
        }

    }

    protected void getAll()
    {
        DataTable dt_list = new DataTable();

        SqlDataAdapter Adp = new SqlDataAdapter("SELECT  [AdminUserId],P.UserType,R.Role,[Password],[Username],[Email],[FirstName] + ' ' +[LastName] as FullName,IsActive,[Designation],[Phone]  FROM [AdminUser] A  LEFT JOIN PayeRole R on A.RoleId = R.RoleId  LEFT JOIN PayeUserType P on A.PayeUserTypeId = P.UserTypeId", con);
        Adp.SelectCommand.CommandTimeout = PAYEClass.defaultTimeout;
        Adp.Fill(dt_list);
        Session["dt_l"] = dt_list;
        grd_user_management.DataSource = dt_list;
        grd_user_management.DataBind();

        int pagesize = grd_user_management.Rows.Count;
        int from_pg = 1;
        int to = grd_user_management.Rows.Count;
        int totalcount = dt_list.Rows.Count;
        lblpagefrom.Text = from_pg.ToString();
        lblpageto.Text = (from_pg + pagesize - 1).ToString();
        lbltoal.Text = totalcount.ToString();

        if (totalcount < grd_user_management.PageSize)
            div_paging.Style.Add("margin-top", "0px");
        else
            div_paging.Style.Add("margin-top", "-60px");
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        grd_user_management.PageIndex = e.NewPageIndex;
        grd_user_management.DataSource = Session["dt_l"];

        grd_user_management.DataBind();

        if (e.NewPageIndex + 1 == 1)
        {
            lblpagefrom.Text = "1";
        }
        else
        {
            lblpagefrom.Text = ((grd_user_management.Rows.Count * e.NewPageIndex) + 1).ToString();
        }

        lblpageto.Text = ((e.NewPageIndex + 1) * grd_user_management.Rows.Count).ToString();

    }


    protected void btn_make_user_inactive_Click(object sender, EventArgs e)
    {
        string id = "";
        GridViewRow clickedRow = ((LinkButton)sender).NamingContainer as GridViewRow;
        string email = grd_user_management.Rows[clickedRow.RowIndex].Cells[2].Text.ToString();
        string neid = grd_user_management.Rows[clickedRow.RowIndex].Cells[5].Text.ToString();
        SqlConnection con = new SqlConnection(PAYEClass.connection.ToString());
        if (neid == "0")
            id = "1";
        if (neid == "1")
            id = "0";

        try
        {
            SqlCommand q1 = new SqlCommand("update AdminUser set IsActive = '" + id + "' where Email ='" + email + "'", con);
            con.Open();
            q1.ExecuteNonQuery();
            con.Close();
            getAll();
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('User Status Changed Successfully');</script>", false);
           
            return;
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