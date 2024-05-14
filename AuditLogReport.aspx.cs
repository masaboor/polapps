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

public partial class AuditLogReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetApplicationName();
            //GetCategory();
            //GetStage();
            //dd_Profile.SelectedValue = "1";
            //btn_search_Click(sender, e);

        }
    }


    public void GetApplicationName()
    {
        div_error.Visible = false;
        lbl_error.Text = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select [ApplicationID], [Name] from [tbl_Applications]
                                            order by Name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Application.DataTextField = "Name";
            dd_Application.DataValueField = "ApplicationID";
            dd_Application.DataSource = dt;
            dd_Application.DataBind();
            dd_Application.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            div_error.Visible = true;
            lbl_error.Text = "No Data";
        }
    }

    protected void dd_Application_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string application = "";
            if (dd_Application.Text != "All")
            {
                application = dd_Application.SelectedValue;
                GetFormName(application);
            }

        }
        catch (Exception)
        {

            throw;
        }

    }
    public void GetFormName(string application)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select DISTINCT CONCAT(LTRIM(Controller), RTRIM(Form)) as [Form] FROM [LogTrail].[dbo].[v_ApplicationLogs] where ApplicationID = '"+application+"'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_pageName.DataTextField = "Form";
            dd_pageName.DataValueField = "";
            dd_pageName.DataSource = dt;
            dd_pageName.DataBind();
            dd_pageName.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            //lbl_status.Text = "No DATA!";
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

    //public void GetCategory()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand("select * from change_status where status_ID <> '2'");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        dd_Profile.DataTextField = "Status_Description";
    //        dd_Profile.DataValueField = "Status_ID";
    //        dd_Profile.DataSource = dt;
    //        dd_Profile.DataBind();
    //        dd_Profile.Items.Insert(0, new ListItem("All", "All"));
    //    }
    //    else
    //    {
    //        lbl_status.Text = "No DATA!";
    //    }
    //}

    //public void GetChanges()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);

    //    string query = @"select t.change_id, 
    //                        t.title, 
    //                        ts.Status_Description 'Status', 
    //                        e1.Employee_name 'Requestor', 
    //                        e2.Employee_name 'Approver1', 
    //			e3.Employee_name 'Approver2', 
    //			s.Stage_description 'Stage',
    //			CONVERT(nvarchar, t.start_date, 103) 'Start_date',
    //			CONVERT(nvarchar, t.end_date, 103) 'End_date'
    //			from change_new t
    //                          inner join change_Status ts
    //                          on t.Status = ts.Status_ID
    //			  inner join change_stage s
    //			  on t.stage_id = s.stage_id
    //                          inner join Employee e1
    //                          on  t.requestor = e1.Employee_ID
    //                          inner join Employee e2
    //                          on  t.approver1 = e2.Employee_ID
    //			   inner join Employee e3
    //                          on  t.approver2 = e3.Employee_ID
    //			   order by t.change_ID desc";

    //    SqlCommand cmd = new SqlCommand(query);
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        gv_ticketList.DataSource = dt;
    //        gv_ticketList.DataBind();
    //    }
    //    else
    //    {
    //        lbl_status.Text = "No DATA!";
    //    }
    //}

    //protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName == "Show")
    //    {
    //        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

    //        int RowIndex = gvr.RowIndex;

    //        LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

    //        Response.Redirect("ChangeView.aspx?ChangeID=" + lb.Text);
    //    }

    //    if (e.CommandName == "Pending")
    //    {
    //        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

    //        int RowIndex = gvr.RowIndex;

    //        LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

    //        string changeID = lb.Text;

    //        //get data of pending with and show it in the gridview

    //        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //        SqlConnection con = new SqlConnection(constr);
    //        string pendingWith = "  select 'Approver' 'Role', e.Employee_name 'Person', CASE WHEN c.Approver1 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
    //            "   ELSE 'Pending'  END as Status  from Change_new c inner join Employee e  on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + "' " +
    //            "   union all select 'Approver' 'Role', e.Employee_name 'Person', CASE  WHEN  c.Pending_with <> '00' THEN 'Pending' " +
    //            "   ELSE 'Done'   END as Status  from Change_new c inner join Employee e on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + "'  union all  select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status WHEN '0' THEN 'Pending' WHEN '2' THEN 'Work In Progress' ELSE 'Done' END as Status from Change_tasks c inner join Employee e" +
    //            "   on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + "'";


    //        //string pendingWith = "  select 'Approver' 'Role', e.Employee_name 'Person', CASE WHEN c.Approver1 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
    //        //   "   ELSE 'Pending'  END as Status  from Change_new c inner join Employee e  on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + "' " +
    //        //   "   union all select 'Approver' 'Role', e.Employee_name 'Person', CASE  WHEN c.Approver2 <> (Select ur.loginID from UserRoleEmployee ur inner join change_new cn on ur.Role_ID = cn.Pending_with  where cn.Change_ID = '" + changeID + "') or c.Pending_with = '00' THEN 'Done' " +
    //        //   "   ELSE 'Pending'   END as Status  from Change_new c inner join Employee e on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + "'  union all  select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status WHEN '0' THEN 'Pending' WHEN '2' THEN 'Work In Progress' ELSE 'Done' END as Status from Change_tasks c inner join Employee e" +
    //        //   "   on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + "'";
    //        SqlCommand cmd = new SqlCommand(pendingWith);


    //        //SqlCommand cmd = new SqlCommand(@"select 'Approver' 'Role', e.Employee_name 'Person', CASE ISNULL(c.Approve1_Comments,'0') 
    //        //                                              WHEN '0' THEN 'Pending' 
    //        //                                              ELSE 'Done' 
    //        //                                            END as Status  from Change_new c
    //        //                                            inner join Employee e
    //        //                                            on c.Approver1 = e.Employee_ID where c.Change_ID = '" + changeID + @"'

    //        //                                            union all

    //        //                                            select 'Approver' 'Role', e.Employee_name 'Person', CASE ISNULL(c.Approve2_Comments,'0') 
    //        //                                              WHEN '0' THEN 'Pending' 
    //        //                                              ELSE 'Done' 
    //        //                                            END as Status  from Change_new c
    //        //                                            inner join Employee e
    //        //                                            on c.Approver2 = e.Employee_ID where c.Change_ID = '" + changeID + @"'

    //        //                                            union all

    //        //                                            select 'Implementer' 'Role', e.Employee_name 'Person', CASE c.status
    //        //                                              WHEN '0' THEN 'Pending' 
    //        //                                                WHEN '2' THEN 'Work In Progress'
    //        //                                              ELSE 'Done' 
    //        //                                            END as Status  from Change_tasks c
    //        //                                            inner join Employee e
    //        //                                            on c.Implementer = e.Employee_ID where c.Change_ID = '" + changeID + @"'");
    //        SqlDataAdapter sda = new SqlDataAdapter();
    //        cmd.Connection = con;
    //        sda.SelectCommand = cmd;
    //        DataTable dtpending = new DataTable();
    //        con.Open();
    //        sda.Fill(dtpending);
    //        con.Close();

    //        if (dtpending.Rows.Count > 0)
    //        {
    //            gv_pendingWith.DataSource = dtpending;
    //            gv_pendingWith.DataBind();
    //        }
    //        else
    //        {
    //            gv_pendingWith.DataSource = null;
    //            gv_pendingWith.DataBind();
    //        }


    //        mp1.Show();
    //    }
    //}

    //protected void btn_search_Click(object sender, EventArgs e)
    //{
    //    string profile = "";
    //    string changeId = "";
    //    string stage = "";
    //    string dateparam = "";
    //    string requestor = "";

    //    if (txt_dateto.Text != "" && txt_dateto.Text != "")
    //    {

    //        dateparam = " and t.submit_date between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";

    //    }


    //    //if (dd_Profile.SelectedValue == "All")
    //    //{ }
    //    //else
    //    //{
    //    //    profile = " and t.Status = '" + dd_Profile.SelectedValue + "'";
    //    //}

    //    if (dd_Stage.SelectedValue == "All")
    //    {

    //    }
    //    else
    //    {
    //        stage = " and t.Stage_ID = '" + dd_Stage.SelectedValue + "'";
    //    }

    //    if (dd_Application.SelectedValue == "All")
    //    {

    //    }
    //    else
    //    {
    //        requestor = " and e1.Employee_ID = '" + dd_Application.SelectedValue + "'";
    //    }

    //    if (text_ChangeID.Text == "")
    //    {

    //    }
    //    else
    //    {
    //        changeId = " and t.change_id = '" + text_ChangeID.Text + "'";
    //    }




    //    GetTicketsBySearch(profile, dateparam, stage, requestor, changeId);

    //}

    //public void GetTicketsBySearch(string Status, string datepa, string stage, string requestor, string changeId)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);



    //    // string query = @"select t.change_id, 
    //    //                     t.title, 
    //    //                     ts.Status_Description 'Status', 
    //    //                     e1.Employee_name 'Requestor', 
    //    //                     e2.Employee_name 'Approver1', 
    //    //e3.Employee_name 'Approver2',
    //    //s.Stage_description 'Stage',
    //    //CONVERT(nvarchar, t.start_date, 103) 'Start_date',
    //    //CONVERT(nvarchar, t.end_date, 103) 'End_date'
    //    //from change_new t
    //    //                       inner join change_Status ts
    //    //                       on t.Status = ts.Status_ID
    //    //  inner join change_stage s
    //    //  on t.stage_id = s.stage_id
    //    //                       inner join Employee e1
    //    //                       on  t.requestor = e1.Employee_ID
    //    //                       left outer join Employee e2
    //    //                       on  t.approver1 = e2.Employee_ID
    //    //   left outer join Employee e3
    //    //                       on  t.approver2 = e3.Employee_ID where 1 = 1 " + datepa + Status + stage + requestor + changeId + " order by t.change_ID desc";

    //    string query = @"select t.change_id, 
    //                        t.title, 
    //                        ts.Status_Description 'Status', 
    //                        e1.Employee_name 'Requestor', 
    //                        ur.RoleName 'Approver1', 
    //			s.Stage_description 'Stage',
    //			CONVERT(nvarchar, t.start_date, 103) 'Start_date',
    //			CONVERT(nvarchar, t.end_date, 103) 'End_date'
    //			from change_new t
    //                          inner join change_Status ts
    //                          on t.Status = ts.Status_ID
    //			  inner join change_stage s
    //			  on t.stage_id = s.stage_id
    //                          inner join Employee e1
    //                          on  t.requestor = e1.Employee_ID 
    //                          left outer join Employee e2
    //                          on  t.approver1 = e2.Employee_ID
    //			   left outer join Employee e3
    //                          on  t.approver2 = e3.Employee_ID
    //                           left join UserRoles ur on t.Pending_with = ur.Role_ID
    //                            where 1 = 1 " + datepa + Status + stage + requestor + changeId + " order by t.change_ID desc";


    //    SqlCommand cmd = new SqlCommand(query);
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        gv_ticketList.DataSource = dt;
    //        gv_ticketList.DataBind();
    //    }
    //    else
    //    {
    //        gv_ticketList.DataSource = null;
    //        gv_ticketList.DataBind();
    //        lbl_status.Text = "No Changes found for your selection!";
    //    }
    //}

    protected void btn_download_Click(object sender, EventArgs e)
    {
        div_error.Visible = false;
        lbl_error.Text = "";

        //if (dd_Application.SelectedValue == "All")
        //{
        //    div_error.Visible = true;
        //    lbl_error.Text = "Please Select as Application!";
        //    return;
        //}
        if (String.IsNullOrEmpty(txt_datefrom.Text))
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select From Date!";
            return;
        }
        if (String.IsNullOrEmpty(txt_dateto.Text))
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select To Date!";
            return;
        }

        string application = "", dateparam = "", pagename = "";

        if (txt_dateto.Text != "" && txt_dateto.Text != "")
        {
            //dateparam = " and CreatedDate between '" + txt_datefrom.Text + "' and '" + txt_dateto.Text + "'";
            dateparam = " and CreatedDate between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
        }

        if(dd_Application.SelectedValue == "All")
        {

        }
        else
        {
            application = "and ApplicationID = '" + dd_Application.SelectedValue + "'";
        }

        if (dd_pageName.Text == "All")
        {

        }
        else
        {
            pagename = "and Controller = '" + dd_pageName.Text + "' OR Form = '"+ dd_pageName.Text + "'";
        }

        GetBySearch_Excel(application, dateparam, pagename);

    }

    public void GetBySearch_Excel(string application, string dateparam, string pagename)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);


        string query = @"SELECT * FROM [v_ApplicationLogs] WHERE 1=1 " + application + dateparam + pagename + " order by CreatedDate desc, CreatedTime desc";



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
            div_error.Visible = false;
            lbl_error.Text = "";
            Email.ExportToSpreadsheet(dt, "Audit_Log_Report");
        }
        else
        {
            div_error.Visible = true;
            lbl_error.Text = "No Records found for your selection!";
        }
    }


    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {

            div_error.Visible = false;
            lbl_error.Text = "";

            //if (dd_Application.SelectedValue == "All")
            //{
            //    div_error.Visible = true;
            //    lbl_error.Text = "Please Select as Application!";
            //    return;
            //}
            if (String.IsNullOrEmpty(txt_datefrom.Text))
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select From Date!";
                return;
            }
            if (String.IsNullOrEmpty(txt_dateto.Text))
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select To Date!";
                return;
            }

            string application = "", dateparam = "", pagename = "";

            if (txt_dateto.Text != "" && txt_dateto.Text != "")
            {
                //dateparam = " and CreatedDate between '" + txt_datefrom.Text + "' and '" + txt_dateto.Text + "'";
                dateparam = " and CreatedDate between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
            }

            if (dd_Application.SelectedValue == "All")
            {

            }
            else
            {
                application = "and ApplicationID = '" + dd_Application.SelectedValue + "'";
            }

            if (dd_pageName.Text == "All")
            {

            }
            else
            {
                pagename = "and (Controller = '" + dd_pageName.Text + "' OR Form = '" + dd_pageName.Text + "')";
            }

            string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);


            string query = @"SELECT * FROM [v_ApplicationLogs] WHERE 1=1 " + application + dateparam + pagename + " order by CreatedDate desc, CreatedTime desc";



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
                gv_log.DataSource = null;
                gv_log.DataBind();
                gv_log.DataSource = dt;
                gv_log.DataBind();
                lbl_error.Text = "";
            }
            else
            {
                div_error.Visible = true;
                lbl_error.Text = "No Records found for your selection!";
            }

        }
        catch (Exception ex)
        {

            //throw;
        }

    }
}