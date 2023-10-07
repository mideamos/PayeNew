using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

public partial class GovernmentAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        binddropdown();

        if (Session["roleId"] != null)
        {
            string roleId = Session["roleId"].ToString();

            if (roleId == "1")
            {
                txttype.Enabled = false;
                txtgovtname.Enabled = false;
                txtgovttin.Enabled = false;
                txtgovtphone1.Enabled = false;
                txtgovtphone2.Enabled = false;
                txtgovtemail1.Enabled = false;
                drpgovttaxoffice.Enabled = false;
                drpgovtprefnotification.Enabled = false;
                drpgovtprefnotification.Enabled = false;
                txtaddress.Enabled = false;
                btngovtsave.Enabled = false;
                //txt_mobile.Enabled = false;
                //txt_RIN.Enabled = false;


            }
            else if (roleId == "2" || roleId == "3")
            {
                txttype.Enabled = true;
                txtgovtname.Enabled = true;
                txtgovttin.Enabled = true;
                txtgovtphone1.Enabled = true;
                txtgovtphone2.Enabled = true;
                txtgovtemail1.Enabled = true;
                drpgovttaxoffice.Enabled = true;
                txtaddress.Enabled = true;
                btngovtsave.Enabled = true;
                //txt_mobile.Enabled = true;
                //txt_RIN.Enabled = true;
           

            }
        }
    }

    public void binddropdown()
    {
        string token = Session["token"].ToString();
       // drpgovttaxoffice.DataSource = PAYEClass.processAPI("https://stage-api.eirsautomation.xyz/ReferenceData/TaxOffice/List","",token);

        drpgovttaxoffice.DataSource = PAYEClass.processAPI(PAYEClass.URL_API + "ReferenceData/TaxOffice/List", "", token);
        drpgovttaxoffice.DataTextField = "TaxOfficeName";
        drpgovttaxoffice.DataValueField = "TaxOfficeID";
        drpgovttaxoffice.DataBind();

        //drpgovtprefnotification.DataSource = PAYEClass.processAPI("https://stage-api.eirsautomation.xyz/ReferenceData/NotificationMethod/List", "",token);

        drpgovtprefnotification.DataSource = PAYEClass.processAPI(PAYEClass.URL_API + "ReferenceData/NotificationMethod/List", "", token);
        drpgovtprefnotification.DataTextField = "NotificationMethodName";
        drpgovtprefnotification.DataValueField = "NotificationMethodID";
        drpgovtprefnotification.DataBind();

   }
}