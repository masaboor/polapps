using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string ServiceID = Request.QueryString["ServiceID"];

            string serviceTransID = Request.QueryString["serviceTransID"];

            DataTable dt = new DataTable();

            dt = GetChangeView(ServiceID);

            if (dt.Rows.Count > 0)
            {
                txt_ServiceID.Text = ServiceID;
                txt_Title.Text = dt.Rows[0]["title"].ToString();
                dd_ReportedBy.Text = dt.Rows[0]["requestedby"].ToString();
                txt_ReportedFor.Text = dt.Rows[0]["RequestedFor"].ToString();
                txt_ReferenceNo.Text = dt.Rows[0]["ReferenceNo"].ToString();
                txt_Details.Text = dt.Rows[0]["Details"].ToString();


                GetServices(ServiceID);


                DataTable attachhessss = new DataTable();

                attachhessss = GetChangeAttachments(ServiceID);

                if (attachhessss.Rows.Count > 0)
                {
                    gv_AttachmentList.DataSource = attachhessss;
                    gv_AttachmentList.DataBind();
                }


                DataTable tattachments = new DataTable();
                tattachments.Columns.Add("Task_ID", typeof(string));
                tattachments.Columns.Add("AttachmentID", typeof(string));
                tattachments.Columns.Add("AttachmentName", typeof(string));
                tattachments.Columns.Add("Dataa", typeof(Byte[]));

                tattachments.AcceptChanges();

                Session["tattachments"] = tattachments;


                DataTable taskssss = new DataTable();

                taskssss = GetChangeTasks(ServiceID);

                if (taskssss.Rows.Count > 0)
                {
                    gv_tasks.DataSource = taskssss;
                    gv_tasks.DataBind();

                    foreach (GridViewRow item in gv_tasks.Rows)
                    {
                        HiddenField hd_percent = (HiddenField)item.FindControl("hd_percent");

                        Literal lt_percent = (Literal)item.FindControl("lt_percent");

                        string progresstext = @"<div class=""progress"">
                                                  <div class=""progress-bar"" role=""progressbar"" aria-valuenow=""" + hd_percent.Value + @"""
                                                  aria-valuemin=""0"" aria-valuemax=""100"" style=""width:" + hd_percent.Value + @"%"">
                                                    " + hd_percent.Value + @"%
                                                  </div>
                                                </div>";

                        lt_percent.Text = progresstext;
                    }


                    DataRow[] taskname = taskssss.Select("Service_ID = '" + serviceTransID + "'");
                    if (taskname.Length > 0)
                    {
                        lbl_TaskID.Text = taskname[0]["Service_ID"].ToString() + "-" + taskname[0]["Service_Name"].ToString();

                        txt_Progress.Text = taskname[0]["Percent"].ToString();
                    }

                }


                if (taskssss.Rows.Count > 0)
                {
                    foreach (GridViewRow item in gv_tasks.Rows)
                    {
                        string taskID = item.Cells[0].Text;
                        DropDownList dd_taskAttachment = item.FindControl("dd_taskAttachment") as DropDownList;

                        DataTable taskkkattaccchh = new DataTable();

                        taskkkattaccchh = GetAttachmentsOfChangeTasks(ServiceID, taskID);

                        if (taskkkattaccchh.Rows.Count > 0)
                        {
                            dd_taskAttachment.DataTextField = "filename";
                            dd_taskAttachment.DataValueField = "attachment_ID";
                            dd_taskAttachment.DataSource = taskkkattaccchh;
                            dd_taskAttachment.DataBind();
                        }
                    }
                }


                GetTaskStatus();


                DataTable dtActivity = new DataTable();

                dtActivity = GetChangeLogs(ServiceID);

                if (dtActivity.Rows.Count > 0)
                {
                    foreach (DataRow item in dtActivity.Rows)
                    {
                        string toWrite = " <div class=\"alert alert-info\"> " + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + " </div> ";
                        lt_Activity.Text += toWrite;
                    }
                }


                DataTable tatachhessss = new DataTable();

                tatachhessss = GetChangeTaskAttachments(ServiceID, serviceTransID);

                if (tatachhessss.Rows.Count > 0)
                {

                    foreach (DataRow item in tatachhessss.Rows)
                    {
                        DataRow drr = tattachments.NewRow();
                        drr["Task_ID"] = item["Task_ID"].ToString();
                        drr["AttachmentID"] = item["Attachment_ID"].ToString();
                        drr["AttachmentName"] = item["filename"].ToString();
                        drr["Dataa"] = item["Attachment"] as Byte[];

                        tattachments.Rows.Add(drr);
                        tattachments.AcceptChanges();
                    }

                    gv_TaskAttachment.DataSource = tattachments;
                    gv_TaskAttachment.DataBind();

                    Session["tattachments"] = tattachments;
                }


            }
        }
    }


    public void GetTaskStatus()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select Status_id, Status_description from Task_Status  where status_id in (1,2)   order by Status_id desc");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Status.DataTextField = "Status_description";
            dd_Status.DataValueField = "Status_id";
            dd_Status.DataSource = dt;
            dd_Status.DataBind();
            //dd_Result.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public DataTable GetChangeLogs(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = "select * from service_log where service_id = '" + changeee + "' order by status_datetime desc";
        SqlCommand cmd = new SqlCommand(activityQuery);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    public DataTable GetAttachmentsOfChangeTasks(string changeee, string taskID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select filename,Attachment_ID from service_task_attachments where service_id = '" + changeee + "' and task_ID = '" + taskID + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();


        return dt;
    }

    public DataTable GetChangeTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select c.ServiceTrans_ID 'Service_ID', c.Task_Description 'Service_Name',
                                    c.AssignGroup_ID, d.Dept_LongDesc 'Team',
                                    case when c.Implementer = 'O' then 'Open'
                                    else (select e.Employee_name from Employee e where e.Dept_ID = c.AssignGroup_ID and e.Employee_ID = c.Implementer)
                                    end as 'Implementer',case when c.Approve_Date is not null and c.Status = '0' then 'Approved'									
									else e.Status_Description end as 'Status', c.progress 'Percent'
                                     from service_tasks c
                                     inner join department d
                                     on d.Dept_ID = c.AssignGroup_ID
                                     inner join Task_Status e
                                     on e.Status_ID = c.Status
                                    where c.Service_ID = '" + changeee + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    public DataTable GetChangeAttachments(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from service_attachments where service_id = '" + changeee + "'";

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
            return dt;
        }
        else
        {
            return dt;
        }
    }

    public DataTable GetChangeView(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select a.Title, a.RequestedFor, a.ReferenceNo, a.Details, b.UserName 'requestedby' from Service_new a
                                    inner join HRSmart_Linde..AD_Users b on b.loginid = a.requestedby where a.Service_ID = '" + changeee + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }


    public void GetServices(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select a.service_id, a.service_description from service_services_new a inner join service_tasks b 
                                            on a.Service_ID = b.ServiceTrans_ID
                                              where b.Service_ID = '" + changeee + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_ServiceAffected.DataTextField = "service_description";
            dd_ServiceAffected.DataValueField = "service_id";
            dd_ServiceAffected.DataSource = dt;
            dd_ServiceAffected.DataBind();
            //dd_ServiceAffected.Items.Insert(0, new ListItem("All", "All"));

            foreach (ListItem item in dd_ServiceAffected.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }



    public void showAlert(string msg)
    {
        string message = msg;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append("<script type = 'text/javascript'>");

        sb.Append("window.onload=function(){");

        sb.Append("alert('");

        sb.Append(message);

        sb.Append("')};");

        sb.Append("</script>");

        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
    }

    protected void gv_AttachmentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[0].Text;

            string ticketID = Request.QueryString["ServiceID"];
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select filename, Attachment from Service_Attachments where Service_ID=@Id and Attachment_ID = '" + attachmentId + "'";
                    cmd.Parameters.AddWithValue("@Id", ticketID);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Attachment"];
                        contentType = Path.GetExtension(sdr["filename"].ToString());
                        fileName = sdr["filename"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

        }
    }


    public DataTable GetServiceAssignGroups(string serviceID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select b.Dept_ID, b.Dept_LongDesc from servicetrans_assignGroup a
                                                              inner join department b
                                                              on a.assignGroup_ID = b.Dept_ID
                                                              where a.Service_ID = '" + serviceID + "'";
        SqlCommand cmd = new SqlCommand(activityQuery);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    public DataTable GetTeamMembers(string AssignGroupID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select b.Employee_ID + '#' + b.Email 'Employee_ID', b.Employee_name, a.service_mgr from AssignmentGrp_Emp a
                                                                  inner join Employee b
                                                                  on a.Group_ID = b.Dept_ID
                                                                  and a.Employee_ID = b.Employee_ID
                                                                  where a.Group_ID = '" + AssignGroupID + "'";
        SqlCommand cmd = new SqlCommand(activityQuery);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    protected void dd_Team_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.Parent.Parent;
        int idx = row.RowIndex;

        DataTable dtx = new DataTable();

        dtx = GetTeamMembers(ddl.SelectedValue);

        if (dtx.Rows.Count > 0)
        {
            DropDownList dd_Assignee = (DropDownList)gv_tasks.Rows[idx].FindControl("dd_Assignee");

            dd_Assignee.DataTextField = "Employee_name";
            dd_Assignee.DataValueField = "Employee_ID";
            dd_Assignee.DataSource = dtx;
            dd_Assignee.DataBind();
            dd_Assignee.Items.Insert(0, new ListItem("Open", "O"));

            DataRow[] Approver = dtx.Select("service_mgr = '1'");

            if (Approver.Length > 0)
            {
                HiddenField hd_Approver = (HiddenField)gv_tasks.Rows[idx].FindControl("hd_Approver");

                hd_Approver.Value = Approver[0][0].ToString();
            }
        }


    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (txt_ImplementationComments.Text == "")
        {
            showAlert("Please Enter Status Comments!");
            txt_ImplementationComments.Focus();
            return;
        }

        int n;
        bool isNumeric = int.TryParse(txt_Progress.Text, out n);

        if (isNumeric == false)
        {

            showAlert("Please Enter Task Percent % in Number!");
            txt_Progress.Focus();
            return;
        }

        string ServiceID = Request.QueryString["ServiceID"];

        string serviceTransID = Request.QueryString["serviceTransID"];

        string[] serviceName = lbl_TaskID.Text.Split('-');




        DataTable tattachments = new DataTable();

        tattachments = Session["tattachments"] as DataTable;

        string stage = "3";
        string status = "2";

        int x = 0;
        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        //Hassam Added - 24.Aug.2022
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

        try
        {


        string ticketQuery = "update Service_tasks set status = '" + dd_Status.SelectedValue + "', Progress = " + txt_Progress.Text + " , implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "' where Service_ID = '" + ServiceID + "' and ServiceTrans_ID = '" + serviceTransID + "'";

        if (dd_Status.SelectedValue == "1")
        {
            ticketQuery = "update Service_tasks set status = '" + dd_Status.SelectedValue + "', Progress = 100 , implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "' where Service_ID = '" + ServiceID + "' and ServiceTrans_ID = '" + serviceTransID + "'";
        }


        SqlCommand cmd;

        cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        con.Open();


        x = cmd.ExecuteNonQuery();

        string delattachmentquery = "delete from Service_task_attachments where Service_ID = '" + ServiceID + "' and task_ID = '" + serviceTransID + "'";

        cmd.CommandText = delattachmentquery;

        x = cmd.ExecuteNonQuery();

        foreach (DataRow item in tattachments.Rows)
        {
            string TaskQuery = @"INSERT INTO Service_task_attachments
                                                           (Service_ID,Task_ID,
                                                           attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + ServiceID + "','" + serviceTransID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

            int z = 0;

            if (cmd.Parameters.Contains("@Attachment"))
            {
                cmd.Parameters.Clear();
                //cmd.Parameters.Remove("@Filename");
            }

            cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = item["Dataa"] as Byte[];
            cmd.Parameters.Add("@Filename", SqlDbType.VarChar).Value = item["attachmentname"].ToString();


            cmd.CommandText = TaskQuery;

            z = cmd.ExecuteNonQuery();
        }

        int y = 0;

        string logQuery = @"insert into service_log (Service_ID, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ServiceID + @"', '" + dd_Status.SelectedValue + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Updated the Task: " + serviceName[1] + @"','" + txt_ImplementationComments.Text + " " + txt_Progress.Text + "%')";

        if (dd_Status.SelectedValue == "1")
        {
            logQuery = @"insert into service_log (Service_ID, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ServiceID + @"', '" + dd_Status.SelectedValue + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Completed the Task: " + serviceName[1] + @"','" + txt_ImplementationComments.Text + "')";
        }

        cmd.CommandText = logQuery;

        y = cmd.ExecuteNonQuery();


        DataTable remaining = GetRemainingTasks(ServiceID);

        bool ToUpdateChange = false;

        if (remaining.Rows.Count > 0)
        {
            //tasks are remaining
        }
        else
        {
            stage = "4";
            status = "3";
            ToUpdateChange = true;
        }


        if (ToUpdateChange == true)
        {
            string changeupdateQuery = "update service_new set stage_ID = '" + stage + "', status = '" + status + "' where service_id = '" + ServiceID + @"'";
            cmd.CommandText = changeupdateQuery;

            int z = cmd.ExecuteNonQuery();
        }

        #region EmailWork

        string emailids = "";

        DataTable Approverss = GetTeamEmail(ServiceID,serviceTransID);



        if (Approverss.Rows.Count > 0)
        {

            foreach (DataRow item in Approverss.Rows)
            {
                emailids += item["Email"] + ";";
            }

            emailids = emailids.TrimEnd(';');
        }

        string emailBody = @"<p>Dear <b>All</b>, a Service Request task has been Marked as <b>" + dd_Status.SelectedItem.Text + "</b> in CIS by <b>" + Session["Username"].ToString() + @"</b>, the Details of the Service are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + ServiceID + @"</p>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                                <b>Reference No.: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + txt_ReferenceNo.Text + @"</p>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                           <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service Title: </b>
                                                                            </td>
                                                                             <td colspan=""2"">
                                                                              <p>" + txt_Title.Text + @"</p>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Reported By: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + dd_ReportedBy.Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Reported For: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + txt_ReportedFor.Text + @"</p>
                                                                            </td>
                                                                        </tr>

  <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Services Offered: </b>
                                                                            </td>
                                                                             <td colspan=""3"">
                                                                                <p>";


        foreach (ListItem item in dd_ServiceAffected.Items)
        {
            if (item.Selected)
            {
                emailBody += item.Text + ",";
            }
        }

        emailBody = emailBody.TrimEnd(',');


        emailBody += @"</p>
                                                                            </td>
                                       
                                                                        </tr>

                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Details</b>
                                                                            </td>
                                                                             <td colspan=""3"">
                                                                               <p>" + txt_Details.Text + @"</p>
                                                                            </td>
                                       
                                                                        </tr>
                        
                                                                    </table>


                                
        
                                <table style=""width: 100%"">
                                    <tr>
                                       
                                    
                                     <td><b>Task Name</b>
                                    </td>
                                     <td><b>Task Status</b>
                                    </td>
                                     <td><b>Task Percentage</b>
                                    </td>
                                     <td><b>Status Comments</b>
                                    </td>
                                    
                                    </tr>  <tr>
                                      
                                                               
                                                                 <td>
                                                                    <p>" + serviceName[1] + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + dd_Status.SelectedItem.Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + txt_Progress.Text + @"
                                                                    </p>
                                                                </td>
                                                                    <td>
                                                                    <p>" + txt_ImplementationComments.Text + @"
                                                                    </p>
                                                                </td>
                                       
                                                            </tr> </table>
        
                          
                                
                                                                    <h3>Activity</h3>";

        DataTable dtActivity = new DataTable();

        dtActivity = GetChangeLogs(ServiceID);

        if (dtActivity.Rows.Count > 0)
        {
            foreach (DataRow item in dtActivity.Rows)
            {
                string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                emailBody += toWrite;

            }
        }

     


        emailBody += @"<p>To View this Service, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                
                                                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

        //SMTP Email
        //Email.SendEmail(emailids, "", "Service Updated, Service ID: " + ServiceID, emailBody);
        string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + emailids + "', 'Service Updated, Service ID: " + ServiceID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Service', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";


        cmd.CommandText = emailQuery;
        int q = cmd.ExecuteNonQuery();

        //Added Hassam 24-Aug-22
        conHRSmart.Open();
        emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + emailids + "', 'Service Updated, Service ID: " + ServiceID + @"', '" + emailBody + @"', 'N', 'CIS', 'Services', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";
        SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
        command.ExecuteNonQuery();
        conHRSmart.Close();
        conHRSmart.Dispose();



            #endregion

        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

        finally
        {
            con.Close();
            con.Dispose();
            conHRSmart.Close();
            conHRSmart.Dispose();
        }

        div_success.Visible = true;
        lbl_ChangeID.Text = ServiceID;
        div_main.Visible = false;



    }


    public DataTable GetTeamEmail(string Service, string ServiceTrans)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string activityQuery = @"select b.Email from Service_new a
                                             inner join Employee b
                                             on a.submit_user = b.Employee_ID
                                              where a.Service_ID = '"+Service+@"'

                                              union all

                                              select b.Email from Service_tasks a
                                             inner join Employee b
                                             on a.Approver = b.Employee_ID
                                              where a.Service_ID = '" + Service + @"'
                                              and a.ServiceTrans_ID = '"+ServiceTrans+@"'

                                              union all 

                                              select b.Email from Service_tasks a
                                             inner join Employee b
                                             on a.Implementer = b.Employee_ID
                                              where a.Service_ID = '" + Service + @"'
                                              and a.ServiceTrans_ID = '" + ServiceTrans + @"'";

        SqlCommand cmd = new SqlCommand(activityQuery);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    public DataTable GetRemainingTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select * from  service_tasks ct where ct.service_id = '" + changeee + @"' and ct.status in ('0','2')");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    protected void btn_AddTaskAttachment_Click(object sender, EventArgs e)
    {
        if (fp_attachment.HasFile)
        {

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            DataRow row = tattachments.NewRow();

            string[] a = lbl_TaskID.Text.Split('-');

            row["Task_ID"] = a[0];
            row["AttachmentID"] = (tattachments.Rows.Count + 1).ToString();


            string filePath = fp_attachment.PostedFile.FileName;
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;

            //Set the contenttype based on File Extension

            switch (ext)
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    break;

                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    break;

                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    break;

                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    break;

                case ".jpg":
                    contenttype = "image/jpg";
                    break;

                case ".png":
                    contenttype = "image/png";
                    break;

                case ".gif":
                    contenttype = "image/gif";
                    break;

                case ".pdf":
                    contenttype = "application/pdf";
                    break;

                case ".eml":
                    contenttype = "email";
                    break;
                case ".msg":
                    contenttype = "email";
                    break;

            }

            if (contenttype != String.Empty)
            {
                Stream fs = fp_attachment.PostedFile.InputStream;

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);


                row["AttachmentName"] = Path.GetFileName(filePath);
                row["Dataa"] = bytes;


                tattachments.Rows.Add(row);
                tattachments.AcceptChanges();

                gv_TaskAttachment.DataSource = tattachments;
                gv_TaskAttachment.DataBind();

                Session["tattachments"] = tattachments;


            }
        }
        else
        {
            lbl_status.Text = "Please select file to upload!";
            showAlert("Please select file to upload!");
            return;
        }
    }
    protected void gv_TaskAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            tattachments.Rows.RemoveAt(RowIndex);

            tattachments.AcceptChanges();

            Session["tattachments"] = tattachments;

            gv_TaskAttachment.DataSource = tattachments;
            gv_TaskAttachment.DataBind();




        }

        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[1].Text;

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            byte[] bytess;
            string fileName, contentType;

            bytess = tattachments.Rows[RowIndex]["dataa"] as byte[];

            contentType = Path.GetExtension(tattachments.Rows[RowIndex]["AttachmentName"].ToString());
            fileName = tattachments.Rows[RowIndex]["AttachmentName"].ToString();


            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytess);
            Response.Flush();
            Response.End();

        }
    }


    public DataTable GetChangeTaskAttachments(string changeee, string taskID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from service_task_attachments where service_id = '" + changeee + "' and task_id = '" + taskID + "'";

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
            return dt;
        }
        else
        {
            return dt;
        }
    }
    protected void lbl_ChangeID_Click(object sender, EventArgs e)
    {

    }
}