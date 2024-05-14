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
            GetPriority();
            GetStatus();
           // GetTeamForReassignment();

            dd_Profile.SelectedValue = "1";
            btn_search_Click(sender, e);

        }
    }

    //public void GetTeamForReassignment()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
    //    SqlConnection con = new SqlConnection(constr);
    //    string selectApplicationTeam = "select Dept_ID, Dept_Desc from Department";
    //    SqlCommand cmd = new SqlCommand(selectApplicationTeam);
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if(dt.Rows.Count > 0)
    //    {
    //        dd_SelectTeam.DataTextField = "Dept_Desc";
    //        dd_SelectTeam.DataValueField = "Dept_ID";
    //        dd_SelectTeam.DataSource = dt;
    //        dd_SelectTeam.DataBind();
    //        dd_SelectTeam.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //}

    public void GetPriority()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Priority where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Priority.DataTextField = "Priority_Desc";
            dd_Priority.DataValueField = "Priority_ID";
            dd_Priority.DataSource = dt;
            dd_Priority.DataBind();
            dd_Priority.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetStatus()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Ticket_Status where status in ('1','2','4','5','6')");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Status.DataTextField = "Status_Desc";
            dd_Status.DataValueField = "Status";
            dd_Status.DataSource = dt;
            dd_Status.DataBind();
            dd_Status.Items.Insert(0, new ListItem("All UnResolved", "All"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetCategory()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Category where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Category.DataTextField = "Category_Desc";
            dd_Category.DataValueField = "Category_ID";
            dd_Category.DataSource = dt;
            dd_Category.DataBind();
            dd_Category.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetTickets()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Ticket_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', 
                            e1.Employee_name 'Owner', 
                            e2.Employee_name 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            CONVERT(nvarchar, t.Ticket_Date, 103) 'Created_Date',
                            CONVERT(nvarchar, t.Modify_DateTime, 103) + ' ' + CONVERT(nvarchar, t.Modify_DateTime, 108) 'Updated_Date',
                           'Days: ' + CAST(DateDiff(dd, t.Entry_DateTime,  t.Modify_DateTime) as nvarchar) + ' Hrs: ' + CAST(DateDiff(hh, t.Entry_DateTime,  t.Modify_DateTime) % 24 as nvarchar) + ' Mins: ' + CAST(DateDiff(mi, t.Entry_DateTime,  t.Modify_DateTime) % 60 as nvarchar)  as ActionTime
                              from ticket t
                              inner join Ticket_Status ts
                              on t.Status = ts.Status
                              inner join Employee e1
                              on  t.Employee_ID = e1.Employee_ID
                              inner join Employee e2
                              on  t.Assigned_Employee_ID = e2.Employee_ID
                              inner join Category c
                              on t.Category_ID = c.Category_ID
                              inner join Ticket_type tt
                              on t.Type_ID = tt.Type_ID
                              inner join Ticket_SubType tts
                              on t.Type_ID = tts.Type_ID
                              and t.SubType_ID = tts.SubType_ID
                              inner join Priority p
                              on t.Priority_ID = p.Priority_ID where t.status in ('0','1','2','4','5','6') order by t.Ticket_ID desc";

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

    //protected void dd_SelectTeam_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //   string DeptId = dd_SelectTeam.SelectedValue;
    //   GetReassignmentMembers(DeptId);
    //}

    //public void GetReassignmentMembers(string DeptId)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    string selectEmploye = "select Employee_ID, Employee_name from Employee where Dept_ID = '0" + DeptId + "'";
    //    SqlCommand cmd = new SqlCommand(selectEmploye);
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();
    //    if(dt.Rows.Count > 0)
    //    {
    //        dd_TeamMember.DataTextField = "Employee_name";
    //        dd_TeamMember.DataValueField = "Employee_ID";
    //        dd_TeamMember.DataSource = dt;
    //        dd_TeamMember.DataBind();
    //       // dd_TeamMember.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //}

    //protected void btn_Reassign_Click(object sender, EventArgs e)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

    //    SqlConnection con = new SqlConnection(constr);
    //    string assignNewEmployee = "  update Ticket set Assigned_Employee_ID ='" + dd_TeamMember.SelectedValue + "' where Ticket_ID = '" + text_TaskReassign.Text + "'";
    //    SqlCommand cmd = new SqlCommand(assignNewEmployee);
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

       
    //}
    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("TicketAction.aspx?Ticket=" + lb.Text);
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

            dateparam = " and t.ticket_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";

        }


        if (dd_Profile.SelectedValue == "All")
        { }
        else
        {
            if (dd_Profile.SelectedValue == "1")
            {
                profile = " and t.Assigned_Employee_ID = '" + Session["UserID"].ToString() + "'";
            }
            else
            {
                profile = " and t.Employee_ID = '" + Session["UserID"].ToString() + "'";
            }
        }


        if (dd_Status.SelectedValue == "All")
        {
            status = " and t.status in ('1','2','4','5','6')";
        }
        else
        {
            status = " and t.status = '" + dd_Status.SelectedValue + "'";
        }



        if (dd_Category.SelectedValue == "All")
        {

        }
        else
        {
            category = " and t.Category_ID = '" + dd_Category.SelectedValue + "'";
        }


        if (dd_Priority.SelectedValue == "All")
        {

        }
        else
        {
            priority = " and t.Priority_ID = '" + dd_Priority.SelectedValue + "'";
        }


        GetTicketsBySearch(profile, status, category, priority, dateparam);

    }


    public void GetTicketsBySearch(string Profile, string Status, string Category, string Priority, string datepa)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Ticket_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', 
                            e1.Employee_name 'Owner', 
                            e2.Employee_name 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            CONVERT(nvarchar, t.Ticket_Date, 103) 'Created_Date',
                            CONVERT(nvarchar, t.Modify_DateTime, 103) + ' ' + CONVERT(nvarchar, t.Modify_DateTime, 108) 'Updated_Date', 
                            'Days: ' + CAST(DateDiff(dd, t.Entry_DateTime,  t.Modify_DateTime) as nvarchar) + ' Hrs: ' + CAST(DateDiff(hh, t.Entry_DateTime,  t.Modify_DateTime) % 24 as nvarchar) + ' Mins: ' + CAST(DateDiff(mi, t.Entry_DateTime,  t.Modify_DateTime) % 60 as nvarchar)  as ActionTime
                              from ticket t
                              inner join Ticket_Status ts
                              on t.Status = ts.Status
                              inner join Employee e1
                              on  t.Employee_ID = e1.Employee_ID
                              inner join Employee e2
                              on  t.Assigned_Employee_ID = e2.Employee_ID
                              inner join Category c
                              on t.Category_ID = c.Category_ID
                              inner join Ticket_type tt
                              on t.Type_ID = tt.Type_ID
                              inner join Ticket_SubType tts
                              on t.Type_ID = tts.Type_ID
                              and t.SubType_ID = tts.SubType_ID
                              inner join Priority p
                              on t.Priority_ID = p.Priority_ID where 1 = 1  " + datepa + Profile + Status + Category + Priority + " order by t.Ticket_ID desc";

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
            lbl_status.Text = "No Tickets found for your selection!";
        }
    }

    
}