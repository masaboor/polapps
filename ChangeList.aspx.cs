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
            GetRequestor();
            GetCategory();
            GetStage();
            //dd_Profile.SelectedValue = "1";
            //btn_search_Click(sender, e);

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

    public void GetStage()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Change_Stage where Stage_ID > 0");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Stage.DataTextField = "Stage_Description";
            dd_Stage.DataValueField = "Stage_ID";
            dd_Stage.DataSource = dt;
            dd_Stage.DataBind();
            dd_Stage.Items.Insert(0, new ListItem("All", "All"));
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
        SqlCommand cmd = new SqlCommand("select * from change_status where status_ID <> '2'");
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
                              on  t.approver2 = e3.Employee_ID
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
                "   union all select 'Approver' 'Role', e.Employee_name 'Person', CASE  WHEN  c.Pending_with <> '00' THEN 'Pending' " +
                "   ELSE 'Done'   END as Status  from Change_new c inner join Employee e on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + "'  union all  select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status WHEN '0' THEN 'Pending' WHEN '2' THEN 'Work In Progress' ELSE 'Done' END as Status from Change_tasks c inner join Employee e" +
                "   on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + "'";


            //string pendingWith = "  select 'Approver' 'Role', e.Employee_name 'Person', CASE WHEN c.Approver1 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
            //   "   ELSE 'Pending'  END as Status  from Change_new c inner join Employee e  on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + "' " +
            //   "   union all select 'Approver' 'Role', e.Employee_name 'Person', CASE  WHEN c.Approver2 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
            //   "   ELSE 'Pending'   END as Status  from Change_new c inner join Employee e on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + "'  union all  select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status WHEN '0' THEN 'Pending' WHEN '2' THEN 'Work In Progress' ELSE 'Done' END as Status from Change_tasks c inner join Employee e" +
            //   "   on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + "'";
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
        string profile = "";
        string changeId = "";
        string stage = "";
        string dateparam = "";
        string requestor = "";

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

        if (dd_Stage.SelectedValue == "All")
        {

        }
        else
        {
            stage = " and t.Stage_ID = '" + dd_Stage.SelectedValue + "'";
        }

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




        GetTicketsBySearch(profile, dateparam, stage, requestor, changeId);

    }

    public void GetTicketsBySearch(string Status, string datepa, string stage, string requestor, string changeId)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);



       // string query = @"select t.change_id, 
       //                     t.title, 
       //                     ts.Status_Description 'Status', 
       //                     e1.Employee_name 'Requestor', 
       //                     e2.Employee_name 'Approver1', 
							//e3.Employee_name 'Approver2',
							//s.Stage_description 'Stage',
							//CONVERT(nvarchar, t.start_date, 103) 'Start_date',
							//CONVERT(nvarchar, t.end_date, 103) 'End_date'
							//from change_new t
       //                       inner join change_Status ts
       //                       on t.Status = ts.Status_ID
							//  inner join change_stage s
							//  on t.stage_id = s.stage_id
       //                       inner join Employee e1
       //                       on  t.requestor = e1.Employee_ID
       //                       left outer join Employee e2
       //                       on  t.approver1 = e2.Employee_ID
							//   left outer join Employee e3
       //                       on  t.approver2 = e3.Employee_ID where 1 = 1 " + datepa + Status + stage + requestor + changeId + " order by t.change_ID desc";

        string query = @"select t.change_id, 
                            t.title, 
                            ts.Status_Description 'Status', 
                            e1.Employee_name 'Requestor', 
                            ur.RoleName 'Approver1', 
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
                              on  t.approver2 = e3.Employee_ID
                               left join UserRoles ur on t.Pending_with = ur.Role_ID
                                where 1 = 1 " + datepa + Status + stage + requestor + changeId + " order by t.change_ID desc";


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
                            e1.Employee_name 'Requestor', 
                            e2.Employee_name 'First Approver', 
							e3.Employee_name 'Second Approver',
							s.Stage_description 'Stage',
							CONVERT(nvarchar, t.start_date, 103) 'Start Date',
							CONVERT(nvarchar, t.end_date, 103) 'End Date'
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
                              on  t.approver2 = e3.Employee_ID where 1 = 1 " + datepa + Status + " order by t.change_ID desc";

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
            Email.ExportToSpreadsheet(dt, "Change");
        }
        else
        {
            lbl_status.Text = "No Changes found for your selection!";
        }
    }
}