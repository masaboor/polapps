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
        try
        {
            if (!IsPostBack)
            {

                GetChanges();
                //dd_Profile.SelectedValue = "1";
                //btn_search_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }



    public void GetChanges()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.change_id, 
                            t.title, 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Requestor', 
                            e2.Employee_name 'Approver1', 
							e3.Employee_name 'Approver2', 
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start_date',
							CONVERT(nvarchar, t.end_date, 103) 'End_date'
							from change_new t
                              inner join change_Status ts
                              on t.Status = ts.Status_ID
							  inner join change_stage s
							  on t.stage_id = s.stage_id
                              inner join Employee e1
                              on  t.requestor = e1.Employee_ID
                              left outer join Employee e2
                              on  t.approver1 = e2.Employee_ID
							   left outer join Employee e3
                              on  t.approver2 = e3.Employee_ID where t.status = '0' and t.requestor = '"+Session["UserID"].ToString()+@"'
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

            Response.Redirect("ChangeEditDraft.aspx?ChangeID=" + lb.Text);
        }
    }



    public void GetTicketsBySearch(string Status, string datepa)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);



        string query = @"select t.change_id, 
                            t.title, 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Requestor', 
                            e2.Employee_name 'Approver1', 
							e3.Employee_name 'Approver2',
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start_date',
							CONVERT(nvarchar, t.end_date, 103) 'End_date'
							from change_new t
                              inner join change_Status ts
                              on t.Status = ts.Status_ID
							  inner join change_stage s
							  on t.stage_id = s.stage_id
                              inner join Employee e1
                              on  t.requestor = e1.Employee_ID
                              left outer join Employee e2
                              on  t.approver1 = e2.Employee_ID
							   left outer join Employee e3
                              on  t.approver2 = e3.Employee_ID where 1 = 1 " + datepa + Status + " order by CAST(t.change_ID as int) desc";


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

}