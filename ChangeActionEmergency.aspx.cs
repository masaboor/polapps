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

             GetTicketsBySearch();
            //DataTable authe = new DataTable();

            //authe = Check_Authr();

            //if (authe.Rows.Count > 0)
            //{
            //    GetTicketsBySearch();
            //}
            //else
            //{
            //    lbl_status.Text = "You are not authorized to approve any Emergency Change!";
            //}
        }
    }


    //public DataTable Check_Authr()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand("select * from Emergency_Approvers  where Employee_id = '" + Session["UserID"].ToString() + "'");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();


    //    return dt;
    //}

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
            gv_implement.DataSource = dt;
            gv_implement.DataBind();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        //if (e.CommandName == "Approval")
        //{
        //    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

        //    int RowIndex = gvr.RowIndex;



        //    //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

        //    Response.Redirect("ChangeApproval.aspx?ChangeID=" + gvr.Cells[1].Text);
        //}


        if (e.CommandName == "Implementation")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;
            string CaseOf = "Emergency";

            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ChangeImplement.aspx?ChangeID=" + gvr.Cells[1].Text + "&TaskID=" + gvr.Cells[3].Text + "&Case=" +CaseOf);
        }


    }

    //protected void btn_search_Click(object sender, EventArgs e)
    //{


    //    GetTicketsBySearch();

    //}


    public void GetTicketsBySearch()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select t.change_id, t.title, ct.task_id,ct.Task_Name, e1.Employee_name 'Implementer',  ct.Task_start 'Start_date', ct.Task_end 'End_date', 'Implementation' 'ActionName' from change_new t  inner join Change_tasks ct on ct.Change_ID = t.Change_ID and ct.Status <> '1' and ct.Implementer = '" + Session["UserID"] + "'  inner join Employee e1    on  ct.implementer = e1.Employee_ID   and t.urgency_ID = '05' and t.stage_ID = '4' order by t.change_ID desc";

       // string query = @"select t.change_id, 
       //                     t.title, 
       //                     ts.Status_Description 'Status', 
       //                     e1.Employee_name 'Requestor', 
       //                     cu.Urgency_Description 'Urgency',
       //                     e2.Employee_name 'Approver', 
							//s.Stage_description 'Stage',
							//t.start_date 'Start_date',
							//t.end_date 'End_date', 'Approval' 'ActionName'
							//from change_new t
       //                       inner join change_Status ts
       //                       on t.Status = ts.Status_ID
							//  inner join change_stage s
							//  on t.stage_id = s.stage_id
       //                       inner join Employee e1
       //                       on  t.Requestor = e1.Employee_ID
       //                       left outer join Employee e2
       //                       on  t.Approver1 = e2.Employee_ID inner join change_urgency cu on cu.urgency_ID = t.urgency_ID where 1 = 1 and t.urgency_ID = '05' and t.stage_ID = '3' 
							//   order by t.change_ID desc";



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
            gv_implement.DataSource = dt;
            gv_implement.DataBind();
        }
        else
        {
            gv_implement.DataSource = null;
            gv_implement.DataBind();
            lbl_status.Text = "No Emergency Changes Actions found for your selection!";
        }




    }
}