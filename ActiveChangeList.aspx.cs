using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class ActiveChangeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetRequestor();
           // GetStage();
            OnPageReferesh();
        }
    }

    public void GetRequestor()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Employee_ID, Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Requestor.DataTextField = "Employee_name";
            dd_Requestor.DataValueField = "Employee_ID";
            dd_Requestor.DataSource = dt;
            dd_Requestor.DataBind();
            dd_Requestor.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    //public void GetStage()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand("select * from Change_Stage where Stage_ID > 0");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        dd_Stage.DataTextField = "Stage_Description";
    //        dd_Stage.DataValueField = "Stage_ID";
    //        dd_Stage.DataSource = dt;
    //        dd_Stage.DataBind();
    //        dd_Stage.Items.Insert(0, new ListItem("All", "All"));
    //    }
    //    else
    //    {
    //        lbl_status.Text = "No DATA!";
    //    }
    //}


    protected void OnPageReferesh()
    {
        string Department = Request.QueryString["Department"];

        string Status = "t.Status = '3'";
        string SortByDepartment = "and Type_ID = (select Dept_ID from Department where Dept_Desc = '" + Department + "')";



        GetChange(Status, SortByDepartment);

    }

    public void GetChange(string Status, string Department)
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
                              inner join Employee e2
                              on  t.approver1 = e2.Employee_ID
							   inner join Employee e3
                              on  t.approver2 = e3.Employee_ID where " + Status + Department + " order by t.change_ID desc";

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

            Response.Redirect("ChangeView.aspx?ChangeID=" + lb.Text);
        }

        if (e.CommandName == "Pending")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            string changeID = lb.Text;

            //get data of pending with and show it in the gridview

            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


            SqlConnection con = new SqlConnection(constr);
            string pendingWith = "  select 'Approver' 'Role', e.Employee_name 'Person', CASE WHEN c.Approver1 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
              "   ELSE 'Pending'  END as Status  from Change_new c inner join Employee e  on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + "' " +
              "   union all select 'Approver' 'Role', e.Employee_name 'Person', CASE  WHEN c.Approver2 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
              "   ELSE 'Pending'   END as Status  from Change_new c inner join Employee e on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + "'  union all  select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status WHEN '0' THEN 'Pending' WHEN '2' THEN 'Work In Progress' ELSE 'Done' END as Status from Change_tasks c inner join Employee e" +
              "   on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + "'";
            SqlCommand cmd = new SqlCommand(pendingWith);
            //SqlCommand cmd = new SqlCommand(@"select 'Approver' 'Role', e.Employee_name 'Person', CASE ISNULL(c.Approve1_Comments,'0') 
            //                                              WHEN '0' THEN 'Pending' 
            //                                              ELSE 'Done' 
            //                                            END as Status  from Change_new c
            //                                            inner join Employee e
            //                                            on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + @"'

            //                                            union all

            //                                            select 'Approver' 'Role', e.Employee_name 'Person', CASE ISNULL(c.Approve2_Comments,'0') 
            //                                              WHEN '0' THEN 'Pending' 
            //                                              ELSE 'Done' 
            //                                            END as Status  from Change_new c
            //                                            inner join Employee e
            //                                            on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + @"'

            //                                            union all

            //                                            select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status
            //                                              WHEN '0' THEN 'Pending' 
            //                                                WHEN '2' THEN 'Work In Progress'
            //                                              ELSE 'Done' 
            //                                            END as Status  from Change_tasks c
            //                                            inner join Employee e
            //                                            on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + @"'");
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            DataTable dtpending = new DataTable();
            con.Open();
            sda.Fill(dtpending);
            con.Close();

            if (dtpending.Rows.Count > 0)
            {
                gv_pendingWith.DataSource = dtpending;
                gv_pendingWith.DataBind();
            }
            else
            {
                gv_pendingWith.DataSource = null;
                gv_pendingWith.DataBind();
            }


            mp1.Show();
        }
    }


    protected void btn_search_Click(object sender, EventArgs e)
    {
        string changeId = "";
        string requestor = "";

        //if (txt_datefrom.Text != "")
        //{

        //    dateparam = " and t.submit_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105)";

        //}



        //if (dd_Stage.SelectedValue == "All")
        //{

        //}
        //else
        //{
        //    stage = " and t.Stage_ID = '" + dd_Stage.SelectedValue + "'";
        //}

        if (dd_Requestor.SelectedValue == "All")
        {

        }
        else
        {
            requestor = " and e1.Employee_ID = '" + dd_Requestor.SelectedValue + "'";
        }

        if (text_ChangeID.Text == "")
        {

        }
        else
        {
            changeId = " and t.change_id = '" + text_ChangeID.Text + "'";
        }




        GetTicketsBySearch(requestor, changeId);

    }

    public void GetTicketsBySearch(string requestor, string changeId)
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
                              on  t.approver2 = e3.Employee_ID where 1 = 1 " + requestor + changeId + " order by t.change_ID desc";


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