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


            btn_search_Click(sender, e);

        }
    }



    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ServiceView.aspx?ServiceID=" + lb.Text);
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {


        GetTicketsBySearch();

    }


    public void GetTicketsBySearch()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);



        string query = @"select t.service_id, 
                            t.title, 
                            c.ServiceTrans_ID,
                            c.task_Description 'ServiceName', 
                            t.requestedby 'RequestedByID',t.requestedfor,
							b.username 'requestedby', d.Dept_LongDesc 'Team',	c.AssignGroup_ID 'TeamCode',						
							case when c.Implementer = 'O' then 'Open' else e.Employee_name end as 'Implementer', e.email 'ImplementerEmail'							
							from service_new t
                            inner join HRSmart_Linde..AD_Users b on b.loginid = t.requestedby
                            inner join service_tasks c
							  on t.Service_ID = c.Service_ID
							  inner join Department d
							  on d.Dept_ID = c.AssignGroup_ID
							  left outer join Employee e
							  on e.Employee_ID = c.Implementer
							  where c.Approver = '" + Session["UserID"].ToString() + @"'
							  and c.Status = '0'
							  and c.Approve_Date is null
							  order by t.service_id";


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
            lbl_status.Text = "No Pending Services found for your Approval!";
        }
    }

    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.Parent.Parent;
        int idx = row.RowIndex;

        LinkButton lb_ticket = (LinkButton)gv_ticketList.Rows[idx].FindControl("lb_ticket");
        HiddenField hd_ServiceTransID = (HiddenField)gv_ticketList.Rows[idx].FindControl("hd_ServiceTransID");
        TextBox txt_Comments = (TextBox)gv_ticketList.Rows[idx].FindControl("txt_Comments");

        Label lbl_ApproveStatus = (Label)gv_ticketList.Rows[idx].FindControl("lbl_ApproveStatus");

        string serviceID = lb_ticket.Text;
        string serviceTrans_ID = hd_ServiceTransID.Value;
        string approvalComments = txt_Comments.Text.Replace("'", "");

        string servicename = gv_ticketList.Rows[idx].Cells[2].Text;

        string serviceQuery = @"update service_tasks set approve_date = getdate(), approve_comments = '" + approvalComments + "' where service_ID = '" + serviceID + "' and serviceTrans_ID = '" + serviceTrans_ID + "'";

        SqlCommand cmd = new SqlCommand(serviceQuery);


        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;

        con.Open();
        x = cmd.ExecuteNonQuery();


        string logQuery = @"insert into service_log (Service_ID, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + serviceID + @"', '2', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Approved the Task " + servicename + "" + @"','" + approvalComments + "')";


        cmd.CommandText = logQuery;

        int y = cmd.ExecuteNonQuery();

        string stage = "";
        string status = "";

        DataTable remaining = GetRemainingTasks(serviceID);

        bool ToUpdateChange = false;

        if (remaining.Rows.Count > 0)
        {
            //tasks are remaining
        }
        else
        {
            stage = "3";
            status = "2";
            ToUpdateChange = true;
        }


        if (ToUpdateChange == true)
        {
            string changeupdateQuery = "update service_new set stage_ID = '" + stage + "', status = '" + status + "' where service_id = '" + serviceID + @"'";
            cmd.CommandText = changeupdateQuery;

            int z = cmd.ExecuteNonQuery();
        }

        string emailids = "";

        if (row.Cells[6].Text == "Open")
        {
            HiddenField hd_Team = (HiddenField)row.FindControl("hd_Team");

            string teamcode = hd_Team.Value;

            DataTable Approverss = GetTeamEmail(teamcode);



            if (Approverss.Rows.Count > 0)
            {

                foreach (DataRow item in Approverss.Rows)
                {
                    emailids += item["Email"] + ";";
                }

                emailids = emailids.TrimEnd(';');
            }
        }
        else
        {
            HiddenField hd_ImplementerEmail = (HiddenField)row.FindControl("hd_ImplementerEmail");
            emailids = hd_ImplementerEmail.Value;

        }


        #region EmailWork

        string emailBody = @"<p>Dear <b>Implementer(s)</b>, a Service Request has been Approved in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your Implementation, the Details of the Service are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + serviceID + @"</p>
                                                                            </td>
                                                                          
                                                                        </tr>
                                                                        
                                                                           <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service Title: </b>
                                                                            </td>
                                                                             <td colspan=""2"">
                                                                              <p>" + gv_ticketList.Rows[idx].Cells[1].Text + @"</p>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Reported By: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + gv_ticketList.Rows[idx].Cells[3].Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Reported For: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + gv_ticketList.Rows[idx].Cells[4].Text + @"</p>
                                                                            </td>
                                                                        </tr>
                        
                                                                    </table>
        
                                <table style=""width: 100%"">
                                    <tr>
                                       
                                    
                                     <td><b>Task Name</b>
                                    </td>
                                     <td><b>Task Team</b>
                                    </td>
                                     <td><b>Implementer</b>
                                    </td>
                                    
                                    </tr>  <tr>
                                      
                                                               
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[2].Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[5].Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[6].Text + @"
                                                                    </p>
                                                                </td>
                                       
                                                            </tr> </table>
        
                          
                                
                                                                    <h3>Activity</h3>";

        DataTable dtActivity = new DataTable();

        dtActivity = GetChangeLogs(serviceID);

        if (dtActivity.Rows.Count > 0)
        {
            foreach (DataRow item in dtActivity.Rows)
            {
                string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                emailBody += toWrite;

            }
        }

        emailBody += @"<p>To perform action on this Service, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                
                                                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";




        //Commenting below code - M.Rahim 17-Feb-2022
        //SMTP Email
        //Email.SendEmail(emailids, "", "For your Implementation, Service ID: " + serviceID, emailBody);


        string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + emailids + "', 'For your Implementation, Service ID: " + serviceID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Service', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";
        cmd.CommandText = emailQuery;
        int q = cmd.ExecuteNonQuery();
        con.Close();



        //Added M.Rahim - 17-Feb-2022 
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);
        conHRSmart.Open();
        emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + emailids + "', 'For your Implementation, Service ID: " + serviceID + @"', '" + emailBody + @"', 'N', 'CIS', 'Service', '', '', GETDATE());
                                                ";
        SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
        command.ExecuteNonQuery();
        conHRSmart.Close();
        conHRSmart.Dispose();


        #endregion



        if (x > 0)
        {
            lbl_ApproveStatus.Text = "Service Approved";

            btn.Enabled = false;

            txt_Comments.Enabled = false;

        }

    }

    protected void btn_Disapprove_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.Parent.Parent;
        int idx = row.RowIndex;

        LinkButton lb_ticket = (LinkButton)gv_ticketList.Rows[idx].FindControl("lb_ticket");
        HiddenField hd_ServiceTransID = (HiddenField)gv_ticketList.Rows[idx].FindControl("hd_ServiceTransID");
        TextBox txt_Comments = (TextBox)gv_ticketList.Rows[idx].FindControl("txt_Comments");

        Label lbl_ApproveStatus = (Label)gv_ticketList.Rows[idx].FindControl("lbl_ApproveStatus");

        string serviceID = lb_ticket.Text;
        string serviceTrans_ID = hd_ServiceTransID.Value;
        string approvalComments = txt_Comments.Text.Replace("'", "");

        string servicename = gv_ticketList.Rows[idx].Cells[2].Text;

        string serviceQuery = @"update service_tasks set approve_date = getdate(), approve_comments = '" + approvalComments + "' where service_ID = '" + serviceID + "' and serviceTrans_ID = '" + serviceTrans_ID + "'";

        SqlCommand cmd = new SqlCommand(serviceQuery);


        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;

        con.Open();
        x = cmd.ExecuteNonQuery();


        string logQuery = @"insert into service_log (Service_ID, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + serviceID + @"', '9', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Rejected the Task " + servicename + "" + @"','" + approvalComments + "')";


        cmd.CommandText = logQuery;

        int y = cmd.ExecuteNonQuery();

        string stage = "";
        string status = "";

        DataTable remaining = GetRemainingTasks(serviceID);

        bool ToUpdateChange = false;

        if (remaining.Rows.Count > 0)
        {
            //tasks are remaining
        }
        else
        {
            stage = "4";
            status = "9";
            ToUpdateChange = true;
        }


        if (ToUpdateChange == true)
        {
            string changeupdateQuery = "update service_new set stage_ID = '" + stage + "', status = '" + status + "' where service_id = '" + serviceID + @"'";
            cmd.CommandText = changeupdateQuery;

            int z = cmd.ExecuteNonQuery();
        }

        string emailids = "";

        if (row.Cells[6].Text == "Open")
        {
            HiddenField hd_Team = (HiddenField)row.FindControl("hd_Team");

            string teamcode = hd_Team.Value;

            DataTable Approverss = GetTeamEmail(teamcode);



            if (Approverss.Rows.Count > 0)
            {

                foreach (DataRow item in Approverss.Rows)
                {
                    emailids += item["Email"] + ";";
                }

                emailids = emailids.TrimEnd(';');
            }
        }
        else
        {
            HiddenField hd_ImplementerEmail = (HiddenField)row.FindControl("hd_ImplementerEmail");
            emailids = hd_ImplementerEmail.Value;

        }


        #region EmailWork

        string emailBody = @"<p>Dear <b>Implementer(s)</b>, a Service Request has been Rejected in CIS by <b>" + Session["Username"].ToString() 
            + @"</b> and is now available for your Implementation, the Details of the Service are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + serviceID + @"</p>
                                                                            </td>
                                                                          
                                                                        </tr>
                                                                        
                                                                           <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service Title: </b>
                                                                            </td>
                                                                             <td colspan=""2"">
                                                                              <p>" + gv_ticketList.Rows[idx].Cells[1].Text + @"</p>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Reported By: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + gv_ticketList.Rows[idx].Cells[3].Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Reported For: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + gv_ticketList.Rows[idx].Cells[4].Text + @"</p>
                                                                            </td>
                                                                        </tr>
                        
                                                                    </table>
        
                                <table style=""width: 100%"">
                                    <tr>
                                       
                                    
                                     <td><b>Task Name</b>
                                    </td>
                                     <td><b>Task Team</b>
                                    </td>
                                     <td><b>Implementer</b>
                                    </td>
                                    
                                    </tr>  <tr>
                                      
                                                               
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[2].Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[5].Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + gv_ticketList.Rows[idx].Cells[6].Text + @"
                                                                    </p>
                                                                </td>
                                       
                                                            </tr> </table>
        
                          
                                
                                                                    <h3>Activity</h3>";

        DataTable dtActivity = new DataTable();

        dtActivity = GetChangeLogs(serviceID);

        if (dtActivity.Rows.Count > 0)
        {
            foreach (DataRow item in dtActivity.Rows)
            {
                string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                emailBody += toWrite;

            }
        }

        emailBody += @"<p>To perform action on this Service, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                
                                                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";


        //SMTP Email
        //Email.SendEmail(emailids, "", "Service Rejected, Service ID: " + serviceID, emailBody);
        string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + emailids + "', 'Service Rejected, Service ID: " + serviceID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Service', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";
        cmd.CommandText = emailQuery;
        int q = cmd.ExecuteNonQuery();
        con.Close();


        //Added M.Rahim - 17-Feb-2022 
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

        conHRSmart.Open();
        emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + emailids + "', 'Service Rejected, Service ID: " + serviceID + @"', '" + emailBody + @"', 'N', 'CIS', 'Service', '', '', GETDATE());
                                                ";
        SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
        command.ExecuteNonQuery();
        conHRSmart.Close();
        conHRSmart.Dispose();

        #endregion



        if (x > 0)
        {
            lbl_ApproveStatus.Text = "Service Rejected";

            btn.Enabled = false;

            txt_Comments.Enabled = false;

        }

    }

    public DataTable GetChangeLogs(string serviceid)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = "select * from service_log where service_id = '" + serviceid + "' order by status_datetime desc";
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

    public DataTable GetTeamEmail(string teamcode)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select b.Email from AssignmentGrp_Emp a
                                 inner join Employee b
                                 on a.Group_ID = b.Dept_ID
                                 and a.Employee_ID = b.Employee_ID
                                 where a.Group_ID = '" + teamcode + "'";
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
        SqlCommand cmd = new SqlCommand(@"select * from  service_tasks ct where ct.service_id = '" + changeee + @"' and ct.status = '0' and ct.approve_date is null");
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