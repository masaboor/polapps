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

        status = " and t.employee_id = '" + Session["UserID"].ToString() + "' and t.status = '3'";


        //if (dd_Profile.SelectedValue == "All")
        //{ }
        //else
        //{
        //    if (dd_Profile.SelectedValue == "1")
        //    {
        //        profile = " and t.Assigned_Employee_ID = '" + Session["UserID"].ToString() + "'";
        //    }
        //    else
        //    {
        //        profile = " and t.Employee_ID = '" + Session["UserID"].ToString() + "'";
        //    }
        //}




        GetTicketsBySearch(profile, status, category, priority, dateparam);
    }
    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ComplaintFeedback.aspx?Ticket=" + lb.Text);
        }


    }



    public void GetTicketsBySearch(string Profile, string Status, string Category, string Priority, string datepa)
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
                              inner join HRSmart_Linde..AD_Users e2
                              on  t.Assigned_Employee_ID = e2.loginid
                              inner join Complaint_Category c
                              on t.Category_ID = c.Category_ID
                              inner join Complaint_type tt
                              on t.Type_ID = tt.Type_ID
                              inner join Complaint_SubType tts
                              on t.Type_ID = tts.Type_ID
                              and t.SubType_ID = tts.SubType_ID
                              inner join Priority p
                              on t.Priority_ID = p.Priority_ID where 1 = 1 " + datepa + Profile + Status + Category + Priority + " order by t.Complaint_ID desc";

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