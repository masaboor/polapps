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
            //GetPriority();
            //GetStatus();
            //GetChanges();

            //dd_Profile.SelectedValue = "1";
            //btn_search_Click(sender, e);

        }
    }


    public void GetCategory()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Change_Stage where Stage_ID in (2,3,4)");
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
        if (e.CommandName == "Reviewal")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            
            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ChangeReview.aspx?ChangeID=" + gvr.Cells[1].Text);
        }

        if (e.CommandName == "Approval")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;



            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ChangeApproval_old.aspx?ChangeID=" + gvr.Cells[1].Text);
        }

        if (e.CommandName == "Implementation")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;



            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ChangeImplement_old.aspx?ChangeID=" + gvr.Cells[1].Text);
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        string profile = "";
        string actionname = "";

        lbl_status.Text = "";

        if (dd_Profile.SelectedValue == "All")
        { }
        else
        {
            if (dd_Profile.SelectedValue == "2")
            {

                //Reviewal
                actionname = "Reviewal";
                profile = " and t.reviewer = '" + Session["UserID"].ToString() + "' and t.stage_ID= '" + dd_Profile.SelectedValue + "' and t.status <> '9'";
            }
            else if (dd_Profile.SelectedValue == "3")
            {

                //Approval
                actionname = "Approval";
                profile = " and t.approver = '" + Session["UserID"].ToString() + "' and t.stage_ID= '" + dd_Profile.SelectedValue + "'  and t.status <> '9'";
            }
            else if (dd_Profile.SelectedValue == "4")
            {

                //Implementation
                actionname = "Implementation";
                profile = " and t.implementer = '" + Session["UserID"].ToString() + "' and t.stage_ID= '" + dd_Profile.SelectedValue + "'  and t.status <> '9'";
            }
        }


       


        GetTicketsBySearch(profile, actionname);

    }


    public void GetTicketsBySearch(string Status, string ActionName)
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
							t.start_date 'Start_date',
							t.end_date 'End_date', '"+ActionName+@"' 'ActionName'
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
                              on  t.implementer = e4.Employee_ID " + Status + @"
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
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No Change Actions found for your selection!";
        }
    }
}