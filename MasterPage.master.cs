using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public String year = "2021";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime curretnDate = DateTime.Today; ; // random date
        year = curretnDate.ToString("yyyy");
        // Session.Timeout = 20;
        if (Session["user_id"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            username.Text = Session["username"].ToString();
            //if (Session["roleId"].ToString() == "1")
            //{
            //    Panel panel = (Panel)Master.FindControl("fitHid");
            //    panel.Visible = false;
            //}
        }
    }
    protected void onLastRecord(object sender, EventArgs e)
    {
        try
        {
            DateTime currentDte = DateTime.Now;
            DateTime lastyearDate = currentDte.AddYears(-1);
            string query = "DELETE FROM Assessment_Rules_API WHERE  TaxYear= '" + currentDte.Year+"'";
            SqlConnection con1 = new SqlConnection(PAYEClass.connection);

            SqlCommand truncate = new SqlCommand(query, con1);
            con1.Open();
            truncate.ExecuteNonQuery();
            

            string qry = "insert into Assessment_Rules_API select  TaxPayerID,TaxPayerTypeID,TaxPayerTypeName,TaxPayerRIN,AssetID,AssetTypeID,AssetTypeName,AssetRIN,ProfileID,ProfileReferenceNo,ProfileDescription,AssessmentRuleID,AssessmentRuleCode,AssessmentRuleName,RuleRunID,RuleRunName,PaymentFrequencyID,PaymentFrequencyName,AssessmentAmount,TaxYear = '"+ currentDte.Year + "',PaymentOptionID,PaymentOptionName,TaxMonth from Assessment_Rules_API where TaxYear =  '"+ lastyearDate.Year + "'";
            SqlCommand strQry = new SqlCommand(qry, con1);
            strQry.ExecuteNonQuery();
                // Response.Write("<script>alert('Selected Employer Added Successfully!');</script>");

                query = "DELETE FROM AddPayeInputFile WHERE  TaxYear= '" + currentDte.Year + "'";
                
                 truncate = new SqlCommand(query, con1);
                
                truncate.ExecuteNonQuery();
                


                query = "INSERT INTO AddPayeInputFile SELECT CompanyRIN,BusinessRIN,TaxYear='" + currentDte.Year + "' FROM  AddPayeInputFile WHERE TaxYear=  '" + lastyearDate.Year + "'";
               
                truncate = new SqlCommand(query, con1);
               
                truncate.ExecuteNonQuery();
                con1.Close();


                ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Data Populated Successfully');</script>", false);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
            //  Response.Redirect("PayeInputFile_N.aspx");


        }
        catch (Exception e11)
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "AlertMessage", "<script language=\"javascript\"  type=\"text/javascript\">;alert('Error Occured!');</script>", false);
        }
    }
}
