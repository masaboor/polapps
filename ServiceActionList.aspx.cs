using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            GetCategory();
         

        }
    }


    public void GetCategory()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Service_Stage where Stage_ID in (2,3)");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Profile.DataTextField = "Stage_Description";
            dd_Profile.DataValueField = "Stage_ID";
            dd_Profile.DataSource = dt;
            dd_Profile.DataBind();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }



    protected void btn_search_Click(object sender, EventArgs e)
    {

        lbl_status.Text = "";

        if (dd_Profile.SelectedValue == "All")
        { }
        else
        {
            if (dd_Profile.SelectedValue == "2")
            {

                Response.Redirect("ServiceTaskApproval.aspx");
              
            }
            else if (dd_Profile.SelectedValue == "3")
            {

                Response.Redirect("ServiceTaskActionList.aspx");
            }
        }




    }


}