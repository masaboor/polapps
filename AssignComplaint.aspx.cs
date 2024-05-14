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

public partial class ViewTicket : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            string ticketID = Request.QueryString["Ticket"];

            GetTicket(ticketID);

            GetEmployees();

        }
    }


    public void GetTicket(string TicketID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Complaint_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', t.status 'StatusCode', t.employee_ID, t.assigned_employee_ID,
                            e1.UserName 'Owner', e1.emailaddress 'OwnerEmail',
                            e2.UserName 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            t.Entry_DateTime 'Created_Date',
                            t.Modify_DateTime 'Updated_Date',
                            e3.username 'StatusBy',
                            t.Details, p.Priority_Desc + ' Priority ' + tt.Type_Desc + ' ' + c.Category_Desc 'Description'
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
                              on t.Priority_ID = p.Priority_ID
                              left outer join HRSmart_Linde..AD_Users e3
                              on t.Modify_User = e3.loginid
                              where t.Complaint_ID = '" + TicketID + "'";


        string activityQuery = "select * from Complaint_log where Complaint_id = '" + TicketID + "' order by status_datetime desc";

        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();


        con.Open();
        sda.Fill(dt);
        con.Close();


        DataTable attachhessss = new DataTable();

        attachhessss = GetChangeAttachments(TicketID);

        if (attachhessss.Rows.Count > 0)
        {
            gv_AttachmentList.DataSource = attachhessss;
            gv_AttachmentList.DataBind();
        }





        DataTable dtActivity = new DataTable();

        SqlCommand cmd2 = new SqlCommand(activityQuery);
        SqlDataAdapter sda2 = new SqlDataAdapter();
        cmd2.Connection = con;
        sda2.SelectCommand = cmd2;


        con.Open();
        sda2.Fill(dtActivity);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            lbl_TicketStatus.Text = dt.Rows[0]["Status"].ToString();
            lbl_Ticket_ID.Text = dt.Rows[0]["Complaint_ID"].ToString();
            lbl_Ticket_Title.Text = dt.Rows[0]["title"].ToString();

            lbl_SubType.Text = dt.Rows[0]["SubType"].ToString();

            lbl_Description.Text = dt.Rows[0]["Description"].ToString();





            lbl_AssignedToo.Text = dt.Rows[0]["Assigned_to"].ToString();
            lbl_OwnedBy.Text = dt.Rows[0]["Owner"].ToString();

            hd_ownerEmail.Value = dt.Rows[0]["OwnerEmail"].ToString();



            lbl_CreatedBy.Text = dt.Rows[0]["Owner"].ToString();
            lbl_CreatedDate.Text = dt.Rows[0]["Created_Date"].ToString();

            lbl_StatusBy.Text = dt.Rows[0]["StatusBy"].ToString();
            lbl_StatusDate.Text = dt.Rows[0]["Updated_Date"].ToString();



            if (dt.Rows[0]["StatusBy"].ToString() == "")
            {
                lbl_StatusBy.Text = "No Status till Now.";
                lbl_StatusDate.Text = "No Status till Now.";
            }

            lbl_Details.Text = dt.Rows[0]["Details"].ToString();


            //lb_Attachment.Text = dt.Rows[0]["AttachmentName"].ToString();

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = " <div class=\"alert alert-info\"> " + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + " </div> ";
                    lt_Activity.Text += toWrite;
                }
            }


        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }


    public void GetEmployees()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        string query = @"select e.loginid + '#' + e.emailaddress 'Employee_ID', e.username 'employee_name' from complaint_resolvers ag inner join HRSmart_Linde..AD_Users e
                            on ag.Employee_ID = e.loginid
                            where ag.status = '1'";
        SqlConnection con = new SqlConnection(constr);
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
            dd_AssignToPerson.DataTextField = "employee_name";
            dd_AssignToPerson.DataValueField = "employee_id";
            dd_AssignToPerson.DataSource = dt;
            dd_AssignToPerson.DataBind();
            dd_AssignToPerson.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public DataTable GetChangeAttachments(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from Complaint_attachments where Complaint_id = '" + changeee + "'";

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


    protected void gv_AttachmentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {


        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[0].Text;

            string ticketID = Request.QueryString["Ticket"];
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select filename, Attachment from Complaint_Attachments where Complaint_ID=@Id and Attachment_ID = '" + attachmentId + "'";
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

    protected void lbl_ticketID_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewComplaint.aspx?Ticket=" + lbl_ticketID.Text);
    }


    protected void DownloadFile(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];
        byte[] bytes;
        string fileName, contentType;
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select filename, Attachment from Complaint where Complaint_ID=@Id";
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



    protected void btn_ReAssign_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];



        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string[] reassign = dd_AssignToPerson.SelectedValue.Split('#');

        if (reassign.Length > 1)
        {

        }
        else
        {
            lbl_status.Text = "Ticket Re-Assigning Failed!! Please try again!";
            showAlert("Ticket Re-Assigning Failed!! Please try again!");
            return;
        }

        string ticketQuery = "update Complaint set status = '2', Assigner_ID = '" + Session["UserID"].ToString() + "', Assign_date = getdate(), Assign_datetime = getdate(),Assigned_Employee_ID = '" + reassign[0].ToString() + "', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where Complaint_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();






            string logQuery = @"insert into Complaint_log (Complaint_id, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ticketID + @"', '2', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Assigned the Complaint to " + dd_AssignToPerson.SelectedItem.Text + @"','"+txt_Comments.Text+"')";


            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();

            string emailBody = @"<p>Dear <b>" + lbl_OwnedBy.Text + @"</b>, Incident# " + ticketID + " has been Re-Assigned to " + dd_AssignToPerson.SelectedItem.Text + " in CIS by <b>" + Session["Username"].ToString() + @"</b>, the Details of the Incident are given below:  </p>
                    
                    
                                                        <table>
                    
                                                            <tr>
                                                                <td colspan=""1"">
                                                                    <b>Ticket ID: </b>
                                                                </td>
                                                                 <td colspan=""1"">
                                                                    <p> " + ticketID + @"</p>
                                                                </td>
                                                                <td colspan=""1"">
                                                                   
                                                                </td>
                                                                <td colspan=""1"">
                                                             
                                                                </td>
                                                            </tr>
                        
                                                             <tr>
                                                                <td colspan=""1"">
                                                                    <b>Change Title: </b>
                                                                </td>
                                                                 <td colspan=""2"">
                                                                  <p>" + lbl_Ticket_Title.Text + @"</p>
                                                                </td>
                                                                 <td colspan=""1"">
                                                                </td>
                                                            </tr>
                        
                        
                                                             <tr>
                                                                <td colspan=""1"">
                                                                    <b>Priority: </b>
                                                                </td>
                                                                 <td colspan=""3"">
                                                                    <p> " + lbl_Description.Text + @"</p>
                                                                </td>
                                                             
                                                            </tr>
                        
                                                             <tr>
                                                                <td colspan=""1"">
                                                                    <b>Type: </b>
                                                                </td>
                                                                 <td colspan=""3"">
                                                                    <p>  " + lbl_SubType.Text + @"</p>
                                                                </td>
                                                             
                                                            </tr>
            
                                                            
                                                                
                        
                                                             <tr>
                                                                <td colspan=""1"">
                                                                    <b>Details </b>
                                                                </td>
                                                                 <td colspan=""3"">
                                                                   <p> " + lbl_Details.Text + @"</p>
                                                                </td>
                           
                                                            </tr>
                        
                                                        </table>
                    
                    
                                                        <b>Incident Owner: </b> <p> " + lbl_OwnedBy.Text + @"</p>
            
                                                        <b>Incident Submitter: </b> <p>" + lbl_OwnedBy.Text + @"</p>
                    
                                                        <b>Incident Actioner: </b> <p> " + dd_AssignToPerson.SelectedItem.Text + @"</p>
                    
                    
                                                        <h3>Activity</h3>";


            DataTable dtActivity = new DataTable();

            dtActivity = GetChangeLogs(ticketID);

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + "</p>";
                    emailBody += toWrite;

                }
            }




            emailBody += @"<p>To View this Incident, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                    
                                                        <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";


            //SMTP Email
            //Email.SendEmail( hd_ownerEmail.Value + "," + reassign[1].ToString(), "", "Ticket Re-Assigned, Ticket ID: " + ticketID, emailBody);

            string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                            values ('" + hd_ownerEmail.Value + ";" + reassign[1].ToString() + "', 'Ticket Re-Assigned, Ticket ID: " + ticketID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Incident', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                            ";
            cmd.CommandText = emailQuery;
            int q = cmd.ExecuteNonQuery();

            /**
             <tr>
                                                                <td colspan=""1"">
                                                                    <b>Service Recipent: </b>
                                                                </td>
                                                                 <td colspan=""1"">
                                                                    <p>  " + txt_recipent.Text + @"</p>
                                                                </td>
                                                              
                                                            </tr>
             **/


             

            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);


            conHRSmart.Open();
            emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + hd_ownerEmail.Value + ";" + reassign[1].ToString() + "', 'Ticket Re-Assigned, Ticket ID: " + ticketID + @"', '" + emailBody + @"', 'N', 'CIS', 'Incident', '', '', GETDATE());
                                                ";
            SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
            command.ExecuteNonQuery();
            conHRSmart.Close();
            conHRSmart.Dispose();
        }

        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }

        finally
        {
            con.Close();
            con.Dispose();
        }

        if (x > 0)
        {
            div_success.Visible = true;
            lbl_ticketID.Text = ticketID;
            div_main.Visible = false;
        }
        else
        {
            lbl_status.Text = "Complaint Assigning Failed!! Please try again!";
            showAlert("Complaint Assigning Failed!! Please try again!");
        }
    }

    public DataTable GetChangeLogs(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = "select * from Complaint_log where Complaint_id = '" + changeee + "' order by status_datetime desc";
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
}