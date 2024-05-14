using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmailLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //GetApplicationName();
            //gv_ticketList.DataBind();
        }
    }

    //public void GetApplicationName()
    //{

    //    string constr = ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand(@" SELECT DISTINCT From_Application FROM [HRSmart_Linde].[dbo].[Email_Logs]");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        dd_Application.DataTextField = "From_Application";
    //        dd_Application.DataValueField = "From_Application";
    //        dd_Application.DataSource = dt;
    //        dd_Application.DataBind();
    //        dd_Application.Items.Insert(0, new ListItem("--- Select One ---", "-1"));
    //    }
    //    else
    //    {
    //        div_error.Visible = true;
    //        lbl_error.Text = "No Data";
    //    }
    //}

    //protected void btn_search_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        div_error.Visible = false;
    //        lbl_error.Text = "";

    //        if (String.IsNullOrEmpty(txt_datefrom.Text))
    //        {
    //            div_error.Visible = true;
    //            lbl_error.Text = "Please Select From Date!";
    //            return;
    //        }
    //        if (String.IsNullOrEmpty(txt_dateto.Text))
    //        {
    //            div_error.Visible = true;
    //            lbl_error.Text = "Please Select To Date!";
    //            return;
    //        }

    //        string application = "", dateparam = "", pagename = "";

    //        if (txt_dateto.Text != "" && txt_dateto.Text != "")
    //        {
    //            //dateparam = " and CreatedDate between '" + txt_datefrom.Text + "' and '" + txt_dateto.Text + "'";
    //            dateparam = " and Created_Date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
    //        }

    //        if (dd_Application.Text == "-1")
    //        {
    //            div_error.Visible = true;
    //            lbl_error.Text = "Please select Application Name!";
    //            return;
    //        }
    //        else
    //        {
    //            application = "and From_Application = '" + dd_Application.SelectedValue + "'";
    //        }

    //        string constr = ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
    //        SqlConnection con = new SqlConnection(constr);


    //        string query = @"SELECT *  FROM [HRSmart_Linde].[dbo].[Email_Logs] WHERE 1=1 " + application + dateparam + pagename + " order by Created_Date desc";



    //        SqlCommand cmd = new SqlCommand(query);
    //        SqlDataAdapter sda = new SqlDataAdapter();
    //        cmd.Connection = con;
    //        sda.SelectCommand = cmd;
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        sda.Fill(dt);
    //        con.Close();

    //        if (dt.Rows.Count > 0)
    //        {
    //            gv_ticketList.DataSource = null;
    //            gv_ticketList.DataBind();
    //            gv_ticketList.DataSource = dt;
    //            gv_ticketList.DataBind();
    //            lbl_error.Text = "";
    //        }
    //        else
    //        {
    //            div_error.Visible = true;
    //            lbl_error.Text = "No Records found for your selection!";
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        div_error.Visible = true;
    //        lbl_error.Text = ex.Message;
    //        //throw;
    //    }

    //}

    //protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Show")
    //    {
    //        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

    //        int RowIndex = gvr.RowIndex;

    //        LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

    //        Response.Redirect("ViewEmail.aspx?LogID=" + lb.Text);
    //    }
    //}

}