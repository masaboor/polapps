using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AssignComplaint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_datefrom.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txt_dateto.Text = System.DateTime.Now.ToString("dd/MM/yyyy");


            GetCategory();
            GetPriority();
            GetStatus();

        }
    }

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
        SqlCommand cmd = new SqlCommand("select * from Complaint_Status where status not in ('0')");
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
            dd_Status.Items.Insert(0, new ListItem("All", "All"));
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
        SqlCommand cmd = new SqlCommand("select * from Complaint_Category where status = '1'");
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

    protected void btn_search_Click(object sender, EventArgs e)
    {
        string profile = "";
        string status = "";
        string category = "";
        string priority = "";

        string dateparam = "";

        if (txt_dateto.Text != "" && txt_dateto.Text != "")
        {

            dateparam = " and t.Complaint_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";

        }

        if (dd_Status.SelectedValue == "All")
        {
            status = "";
        }
        if (dd_Status.SelectedValue == "1")
        {
            status = " and t.status = '1' ";
        }
        if (dd_Status.SelectedValue == "2")
        {
            status = " and t.status <> '1' ";
        }
        




        GetTicketsBySearch(status, category, priority, dateparam);
    }
    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ViewComplaint.aspx?Ticket=" + lb.Text);
        }
    }



    public void GetTicketsBySearch(string Status, string Category, string Priority, string datepa)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Complaint_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', 
                            e1.username 'Owner', 
                            e2.username 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            CONVERT(nvarchar, t.Complaint_Date, 103) 'Created_Date',
                            CONVERT(nvarchar, t.Modify_DateTime, 103) + ' ' + CONVERT(nvarchar, t.Modify_DateTime, 108) 'Updated_Date', 
                            'Days: ' + CAST(DateDiff(dd, t.Entry_DateTime,  t.Modify_DateTime) as nvarchar) + ' Hrs: ' + CAST(DateDiff(hh, t.Entry_DateTime,  t.Modify_DateTime) % 24 as nvarchar) + ' Mins: ' + CAST(DateDiff(mi, t.Entry_DateTime,  t.Modify_DateTime) % 60 as nvarchar)  as ActionTime
                              from Complaint t
                              inner join Complaint_Status ts
                              on t.Status = ts.Status
                              inner join HRSmart_Linde..AD_Users e1
                              on  t.Employee_ID = e1.loginid
                              left outer join HRSmart_Linde..AD_Users e2
                              on  t.Assigned_Employee_ID = e2.loginid
                              inner join Complaint_Category c
                              on t.Category_ID = c.Category_ID
                              inner join Complaint_type tt
                              on t.Type_ID = tt.Type_ID
                              inner join Complaint_SubType tts
                              on t.Type_ID = tts.Type_ID
                              and t.SubType_ID = tts.SubType_ID
                              inner join Priority p
                              on t.Priority_ID = p.Priority_ID where 1 = 1 " + datepa  + Status + Category + Priority + " order by t.Complaint_ID desc";

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
            lbl_status.Text = "No Complaints found for your selection!";
        }
    }
}