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

            //dd_Profile.SelectedValue = "1";
            //btn_search_Click(sender, e);

        }
    }

    public void GetCategory()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from change_status");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Profile.DataTextField = "Status_Description";
            dd_Profile.DataValueField = "Status_ID";
            dd_Profile.DataSource = dt;
            dd_Profile.DataBind();
            dd_Profile.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetChanges()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.change_id, 
                            t.title, 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Owner', 
                            e2.Employee_name 'Reviewer', 
							e3.Employee_name 'Approver', 
							e4.Employee_name 'Implementer', 
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start_date',
							CONVERT(nvarchar, t.end_date, 103) 'End_date'
							from change t
                              inner join change_Status ts
                              on t.Status = ts.Status_ID
							  inner join change_stage s
							  on t.stage_id = s.stage_id
                              inner join Employee e1
                              on  t.owner = e1.Employee_ID
                              inner join Employee e2
                              on  t.reviewer = e2.Employee_ID
							   inner join Employee e3
                              on  t.approver = e3.Employee_ID
							   inner join Employee e4
                              on  t.implementer = e4.Employee_ID
							   order by t.change_ID desc";

        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            gv_ticketList.DataSource = dt;
            gv_ticketList.DataBind();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ChangeView_old.aspx?ChangeID=" + lb.Text);
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        string profile = "";
        string status = "";
        string category = "";
        string priority = "";

        string dateparam = "";

        if (txt_dateto.Text != "" && txt_dateto.Text != "")
        {
            dateparam = " and t.submit_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
        }


        if (dd_Profile.SelectedValue == "All")
        { }
        else
        {
            profile = " and t.Status = '" + dd_Profile.SelectedValue + "'";
        }

        GetTicketsBySearch(profile, dateparam);

    }

    public void GetTicketsBySearch(string Status, string datepa)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string query = @"select t.change_id, 
                            t.title, 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Owner', 
                            e2.Employee_name 'Reviewer', 
							e3.Employee_name 'Approver', 
							e4.Employee_name 'Implementer', 
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start_date',
							CONVERT(nvarchar, t.end_date, 103) 'End_date'
							from change t
                              inner join change_Status ts
                              on t.Status = ts.Status_ID
							  inner join change_stage s
							  on t.stage_id = s.stage_id
                              inner join Employee e1
                              on  t.owner = e1.Employee_ID
                              inner join Employee e2
                              on  t.reviewer = e2.Employee_ID
							   inner join Employee e3
                              on  t.approver = e3.Employee_ID
							   inner join Employee e4
                              on  t.implementer = e4.Employee_ID where 1 = 1 " + datepa + Status + " order by CAST(t.change_ID as int) desc";
        
        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            gv_ticketList.DataSource = dt;
            gv_ticketList.DataBind();
        }
        else
        {
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No Changes found for your selection!";
        }
    }

    protected void btn_download_Click(object sender, EventArgs e)
    {
        string profile = "", dateparam = "";

        if (txt_dateto.Text != "" && txt_dateto.Text != "")
        {
            dateparam = " and t.submit_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
        }

        if (dd_Profile.SelectedValue == "All")
        { }
        else
        {
            profile = " and t.Status = '" + dd_Profile.SelectedValue + "'";
        }
        GetTicketsBySearch_Excel(profile, dateparam);
    }

    public void GetTicketsBySearch_Excel(string Status, string datepa)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.change_id 'Change ID', 
                            t.title 'Title', 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Requester', 
                            e2.Employee_name 'Reviewer', 
							e3.Employee_name 'Approver', 
							e4.Employee_name 'Implementer', 
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start Date',
							CONVERT(nvarchar, t.end_date, 103) 'End Date'
							from change t
                              inner join change_Status ts
                              on t.Status = ts.Status_ID
							  inner join change_stage s
							  on t.stage_id = s.stage_id
                              inner join Employee e1
                              on  t.owner = e1.Employee_ID
                              inner join Employee e2
                              on  t.reviewer = e2.Employee_ID
							   inner join Employee e3
                              on  t.approver = e3.Employee_ID
							   inner join Employee e4
                              on  t.implementer = e4.Employee_ID where 1 = 1 " + datepa + Status + " order by CAST(t.change_ID as int) desc";


        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            Email.ExportToSpreadsheet(dt, "Changes");
        }
        else
        {
            lbl_status.Text = "No Changes found for your selection!";
        }
    }
}