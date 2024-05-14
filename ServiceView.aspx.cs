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


            DataTable dt = new DataTable();

            dt = GetChangeView(ServiceID);

            if (dt.Rows.Count > 0)
            {
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
                                                  <div class=""progress-bar"" role=""progressbar"" aria-valuenow="""+hd_percent.Value+@"""
                                                  aria-valuemin=""0"" aria-valuemax=""100"" style=""width:"+hd_percent.Value+@"%"">
                                                    "+hd_percent.Value+@"%
                                                  </div>
                                                </div>";

                        lt_percent.Text = progresstext;
                    }


                }
  lbl_Change_ID_new.Text = ServiceID;

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


            }
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
                                    end as 'Implementer', case when c.Approve_Date is not null and c.Status = '0' then 'Approved'									
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
    protected void gv_tasks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Downnn")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            //LinkButton lb = (LinkButton)gv_taskList.Rows[RowIndex].FindControl("lb_TaskAttachment");

            //Response.Redirect("ChangeView.aspx?ChangeID=" + lb.Text);


            string ticketID = Request.QueryString["serviceID"];

            string taskID = gv_tasks.Rows[RowIndex].Cells[0].Text;

            DropDownList dd_taskAttachment = gv_tasks.Rows[RowIndex].FindControl("dd_taskAttachment") as DropDownList;

            string attachmentID = dd_taskAttachment.SelectedValue;

            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select filename, Attachment from service_task_attachments where service_ID=@Id and task_ID =@Id2 and attachment_ID = '" + attachmentID + "'";
                    cmd.Parameters.AddWithValue("@Id", ticketID);
                    cmd.Parameters.AddWithValue("@Id2", taskID);
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
}