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
            DataTable attachments = new DataTable();
            attachments.Columns.Add("AttachmentID", typeof(string));
            attachments.Columns.Add("AttachmentName", typeof(string));
            attachments.Columns.Add("Dataa", typeof(Byte[]));

            attachments.AcceptChanges();

            Session["attachments"] = attachments;


            DataTable services = new DataTable();
            services.Columns.Add("Service_ID", typeof(string));
            services.Columns.Add("Service_Name", typeof(string));

            services.AcceptChanges();

            Session["services"] = services;


            GetServices();
            GetReportedBy();

        }
    }
    protected void btn_Save_Click_old(object sender, EventArgs e)
    {

        //        try
        //        {

        //            lbl_status.Text = "";

        //            #region Validation

        //            if (txt_Title.Text == "")
        //            {
        //                showAlert("Please Enter Title!");
        //                return;
        //            }

        //            if (dd_Impact.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Impact!");
        //                return;
        //            }

        //            if (dd_Urgency.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Urgency!");
        //                return;
        //            }

        //            if (dd_Priority.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Priority!");
        //                return;
        //            }


        //            if (dd_Category.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Category!");
        //                return;
        //            }

        //            if (dd_SubCategory.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Sub Category!");
        //                return;
        //            }

        //            if (txt_datefrom.Text == "")
        //            {
        //                showAlert("Please Enter Scheduled Start Date!");
        //                return;
        //            }




        //            if (txt_Details.Text == "")
        //            {
        //                showAlert("Please Enter Details!");
        //                return;
        //            }


        //            if (dd_Implementer.SelectedValue == "0")
        //            {
        //                showAlert("Please Select Implementer!");
        //                return;
        //            }

        //            #endregion

        //            string maxchange = GetMaxService();

        //            double newmaxchange = double.Parse(maxchange);

        //            newmaxchange = newmaxchange + 1;

        //            string changeID = newmaxchange.ToString();

        //            if (fp_attachment.HasFile)
        //            {

        //                string filePath = fp_attachment.PostedFile.FileName;
        //                string filename = Path.GetFileName(filePath);
        //                string ext = Path.GetExtension(filename);
        //                string contenttype = String.Empty;

        //                //Set the contenttype based on File Extension

        //                switch (ext)
        //                {
        //                    case ".doc":
        //                        contenttype = "application/vnd.ms-word";
        //                        break;

        //                    case ".docx":
        //                        contenttype = "application/vnd.ms-word";
        //                        break;

        //                    case ".xls":
        //                        contenttype = "application/vnd.ms-excel";
        //                        break;

        //                    case ".xlsx":
        //                        contenttype = "application/vnd.ms-excel";
        //                        break;

        //                    case ".jpg":
        //                        contenttype = "image/jpg";
        //                        break;

        //                    case ".png":
        //                        contenttype = "image/png";
        //                        break;

        //                    case ".gif":
        //                        contenttype = "image/gif";
        //                        break;

        //                    case ".pdf":
        //                        contenttype = "application/pdf";
        //                        break;

        //                    case ".eml":
        //                        contenttype = "email";
        //                        break;
        //                    case ".msg":
        //                        contenttype = "email";
        //                        break;

        //                }

        //                if (contenttype != String.Empty)
        //                {
        //                    Stream fs = fp_attachment.PostedFile.InputStream;

        //                    BinaryReader br = new BinaryReader(fs);

        //                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

        //                    //insert the file into database


        //                    string changeQuery = @"INSERT INTO Service
        //                                                       (Service_ID
        //                                                       ,Title
        //                                                       ,Requestor, submit_user
        //                                                       ,Submit_Date
        //                                                       ,Submit_DateTime
        //                                                       ,Approver
        //                                                       ,Implementer
        //                                                       ,Impact_ID
        //                                                       ,Urgency_ID
        //                                                       ,Priority_ID
        //                                                       ,Stage_ID
        //                                                       ,Type_ID
        //                                                       ,SubType_ID
        //                                                       ,Details
        //                                                       ,Attachment
        //                                                       ,filename
        //                                                       ,Modify_DateTime
        //                                                       ,Modify_User
        //                                                       ,Status, due_days)
        //                                                 VALUES
        //                                                       ('" + changeID + @"'
        //                                                       ,'" + txt_Title.Text + @"',
        //                                                       ,'" + txt_ReportedBy.Text + @"','" + Session["UserID"].ToString() + @"'
        //                                                       ,getdate()
        //                                                       ,getdate()
        //                                                       ,'" + dd_Approver.SelectedValue + @"'
        //                                                       ,'" + dd_Implementer.SelectedValue + @"'
        //                                                       ,'" + dd_Impact.SelectedValue + @"'
        //                                                       ,'" + dd_Urgency.SelectedValue + @"'
        //                                                       ,'" + dd_Priority.SelectedValue + @"'
        //                                                       ,'2'
        //                                                       ,'" + dd_Category.SelectedValue + @"'
        //                                                       ,'" + dd_SubCategory.SelectedValue + @"'
        //                                                       ,'" + txt_Details.Text + @"'
        //                                                       ,@Attachment
        //                                                       ,@filename
        //                                                       ,getdate()
        //                                                       ,'" + Session["UserID"].ToString() + @"'
        //                                                       ,'1','" + txt_datefrom.Text + "')";

        //                    SqlCommand cmd = new SqlCommand(changeQuery);

        //                    cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = bytes;
        //                    cmd.Parameters.Add("@filename", SqlDbType.VarChar).Value = filename;

        //                    String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        //                    SqlConnection con = new SqlConnection(strConnString);
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Connection = con;

        //                    int x = 0;
        //                    int y = 0;
        //                    int p = 0;

        //                    try
        //                    {
        //                        con.Open();
        //                        x = cmd.ExecuteNonQuery();


        //                        foreach (ListItem item in dd_ServiceAffected.Items)
        //                        {
        //                            if (item.Selected)
        //                            {
        //                                string serviceQuery = "insert into Service_ServiceTrans (Service_ID, ServiceTrans_ID) values ('" + changeID + "','" + item.Value + "')";
        //                                cmd.CommandText = serviceQuery;

        //                                p = cmd.ExecuteNonQuery();
        //                            }

        //                        }


        //                        string logQuery = @"insert into service_log (service_id, status, status_by, status_datetime, status_description)
        //                                                                    values
        //                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Service" + @"')";

        //                        cmd.CommandText = logQuery;

        //                        y = cmd.ExecuteNonQuery();
        //                    }

        //                    catch (Exception ex)
        //                    {
        //                        lbl_status.Text = ex.Message;
        //                        showAlert(ex.Message);
        //                    }

        //                    finally
        //                    {
        //                        con.Close();
        //                        con.Dispose();
        //                    }

        //                    if (x > 0)
        //                    {

        //                        div_success.Visible = true;
        //                        lbl_ChangeID.Text = changeID;
        //                        div_main.Visible = false;
        //                        //showAlert("Ticket Created Successfully!");

        //                        //Response.Redirect("Dashboard.aspx");
        //                    }
        //                    else
        //                    {
        //                        lbl_status.Text = "Service Creation Failed!! Please try again!";
        //                        showAlert("Service Creation Failed!! Please try again!");
        //                    }

        //                    //lblMessage.ForeColor = System.Drawing.Color.Green;

        //                    //lblMessage.Text = "File Uploaded Successfully";

        //                }
        //                else
        //                {
        //                    lbl_status.Text = "File format not recognised." + " Upload Image/Word/PDF/Excel formats";
        //                    showAlert("File format not recognised." + " Upload Image/Word/PDF/Excel formats");
        //                }
        //            }
        //            else
        //            {



        //                string changeQuery = @"INSERT INTO service
        //                                                       (Service_ID
        //                                                       ,Title
        //                                                       ,Requestor, submit_user
        //                                                       ,Submit_Date
        //                                                       ,Submit_DateTime
        //                                                       ,Approver
        //                                                       ,Implementer
        //                                                       ,Impact_ID
        //                                                       ,Urgency_ID
        //                                                       ,Priority_ID
        //                                                       ,Stage_ID
        //                                                       ,Type_ID
        //                                                       ,SubType_ID
        //                                                       ,Details
        //                                                       ,Modify_DateTime
        //                                                       ,Modify_User
        //                                                       ,Status, due_days)
        //                                                 VALUES
        //                                                       ('" + changeID + @"'
        //                                                       ,'" + txt_Title.Text + @"'
        //                                                        ,'" + txt_ReportedBy.Text + @"'
        //                                                       ,'" + Session["UserID"].ToString() + @"'
        //                                                       ,getdate()
        //                                                       ,getdate()
        //                                                       ,'" + dd_Approver.SelectedValue + @"'
        //                                                       ,'" + dd_Implementer.SelectedValue + @"'
        //                                                       ,'" + dd_Impact.SelectedValue + @"'
        //                                                       ,'" + dd_Urgency.SelectedValue + @"'
        //                                                       ,'" + dd_Priority.SelectedValue + @"'
        //                                                       ,'2'
        //                                                       ,'" + dd_Category.SelectedValue + @"'
        //                                                       ,'" + dd_SubCategory.SelectedValue + @"'
        //                                                       ,'" + txt_Details.Text + @"'
        //                                                       ,getdate()
        //                                                       ,'" + Session["UserID"].ToString() + @"'
        //                                                       ,'1','" + txt_datefrom.Text + "')";

        //                SqlCommand cmd = new SqlCommand(changeQuery);


        //                String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        //                SqlConnection con = new SqlConnection(strConnString);
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Connection = con;

        //                int x = 0;
        //                int y = 0;
        //                int p = 0;

        //                try
        //                {
        //                    con.Open();
        //                    x = cmd.ExecuteNonQuery();


        //                    foreach (ListItem item in dd_ServiceAffected.Items)
        //                    {
        //                        if (item.Selected)
        //                        {
        //                            string serviceQuery = "insert into Service_ServiceTrans (Service_ID, ServiceTrans_ID) values ('" + changeID + "','" + item.Value + "')";
        //                            cmd.CommandText = serviceQuery;

        //                            p = cmd.ExecuteNonQuery();
        //                        }

        //                    }



        //                    string logQuery = @"insert into service_log (service_id, status, status_by, status_datetime, status_description)
        //                                                                    values
        //                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Service" + @"')";

        //                    cmd.CommandText = logQuery;

        //                    y = cmd.ExecuteNonQuery();
        //                }

        //                catch (Exception ex)
        //                {
        //                    //Response.Write(ex.Message);
        //                }

        //                finally
        //                {
        //                    con.Close();
        //                    con.Dispose();
        //                }

        //                if (x > 0)
        //                {

        //                    div_success.Visible = true;
        //                    lbl_ChangeID.Text = changeID;
        //                    div_main.Visible = false;
        //                    //showAlert("Ticket Created Successfully!");

        //                    //Response.Redirect("Dashboard.aspx");
        //                }
        //                else
        //                {
        //                    lbl_status.Text = "Service Creation Failed!! Please try again!";
        //                    showAlert("Service Creation Failed!! Please try again!");
        //                }
        //            }



        //        }
        //        catch (Exception iu)
        //        {
        //            lbl_status.Text = iu.Message.ToString();
        //        }

    }

    public string GetMaxChange()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Change_ID as int)),0) from Change_new");
        SqlCommand cmd = new SqlCommand("select isnull(max(CAST(RIGHT(service_id,6) as int)),0) from service_new");
        //
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            maxticket = dt.Rows[0][0].ToString();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
        return maxticket;
    }

    public string addZeroNew(string inti, int limit)
    {
        string final = "";

        int zerotoadd = limit - inti.Length;
        for (int i = 0; i < zerotoadd; i++)
        {
            final = final + "0";
        }

        return final + inti;
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {

        try
        {
            div_error.Visible = false;
            lbl_error.Text = "";
            lbl_status.Text = "";

            #region Validation

            if (txt_Title.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Title!";
                //showAlert("Please Enter Title!");
                return;
            }

            if (dd_ReportedBy.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Reported By!";
                //showAlert("Please Select Reported By!");
                return;
            }

            if (txt_ReportedFor.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Reported For!";
                //showAlert("Please Enter Reported For!");
                return;
            }

            if (txt_ReferenceNo.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Reference Number!";
                //showAlert("Please Enter Reference Number!");
                return;
            }

            if (gv_tasks.Rows.Count > 0)
            {

            }
            else
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Services to Offer!";
                //showAlert("Please Select Services to Offer!");
                return;
            }

            if (txt_Details.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Details!";
                //showAlert("Please Enter Details!");
                return;
            }



            #endregion


            string maxservice = GetMaxChange();

            double newmaxservice = double.Parse(maxservice);

            newmaxservice = newmaxservice + 1;

            //string changeID = newmaxchange.ToString();

            string serviceID = "SR-" + DateTime.Now.ToString("yy") + "-" + addZeroNew(newmaxservice.ToString(), 6);


            string[] repppportedBy = dd_ReportedBy.SelectedValue.Split('#');




            DataTable attachments = new DataTable();

            attachments = Session["attachments"] as DataTable;


            string serviceQuery = @"INSERT INTO Service_new
                                                       (Service_ID 
                                                       ,Title 
                                                       ,RequestedBy 
                                                       ,RequestedFor 
                                                       ,ReferenceNo 
                                                       ,Submit_Date 
                                                       ,Submit_DateTime 
                                                       ,Details 
                                                       ,Status 
                                                       ,Stage_ID 
                                                       ,submit_user )
                                                 VALUES
                                                       ( '" + serviceID + @"'
                                                       , '" + txt_Title.Text + @"'
                                                       , '" + repppportedBy[0] + @"'
                                                       , '" + txt_ReportedFor.Text + @"'
                                                       , '" + txt_ReferenceNo.Text + @"'
                                                       , getdate()
                                                       , getdate()
                                                       , '" + txt_Details.Text + @"'
                                                       , '1'
                                                       , '2'
                                                       , '" + Session["UserID"].ToString() + @"')";

            SqlCommand cmd = new SqlCommand(serviceQuery);

            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            int x = 0;

            int q = 0;

            try
            {
                con.Open();
                x = cmd.ExecuteNonQuery();

                foreach (GridViewRow item in gv_tasks.Rows)
                {

                    string serviceTransS = item.Cells[0].Text;

                    string serviceTransDesc = item.Cells[1].Text;

                    DropDownList dd_Team = (DropDownList)item.FindControl("dd_Team");

                    HiddenField hd_Approver = (HiddenField)item.FindControl("hd_Approver");

                    string[] apppprover = hd_Approver.Value.Split('#');

                    DropDownList dd_Assignee = (DropDownList)item.FindControl("dd_Assignee");

                    string[] assssignee = dd_Assignee.SelectedValue.Split('#');

                    string TaskQuery = @"INSERT INTO Service_tasks 
                                                   (Service_ID 
                                                   ,ServiceTrans_ID 
                                                   ,AssignGroup_ID 
                                                   ,Approver 
                                                   ,Implementer 
                                                   ,Task_Description 
                                                   ,Status 
                                                   ,[Primary]
                                                   ,SLA_Days, Progress )
                                             VALUES
                                                   ( '" + serviceID + @"', '" + serviceTransS + @"',  '" + dd_Team.SelectedValue + @"',  '" + apppprover[0] + @"',
                                                    '" + assssignee[0] + @"', 
                                                    '" + serviceTransDesc + @"',  
                                                    '0',  
                                                    'N',  
                                                     0,0)";

                    int z = 0;

                    cmd.CommandText = TaskQuery;

                     z = cmd.ExecuteNonQuery();
                }

                foreach (DataRow item in attachments.Rows)
                {
                    string TaskQuery = @"INSERT INTO service_attachments
                                                           (service_ID
                                                           ,attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + serviceID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

                    int at = 0;

                    if (cmd.Parameters.Contains("@Attachment"))
                    {
                        cmd.Parameters.Clear();
                        //cmd.Parameters.Remove("@Filename");
                    }

                    cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = item["Dataa"] as Byte[];
                    cmd.Parameters.Add("@Filename", SqlDbType.VarChar).Value = item["attachmentname"].ToString();


                    cmd.CommandText = TaskQuery;

                    at = cmd.ExecuteNonQuery();
                }
                int lg = 0;
                string logQuery = @"insert into service_log (service_id, status, status_by, status_datetime, status_description, status_comments)
                                                                    values
                                                                    ('" + serviceID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"','Service Initiated')";

                cmd.CommandText = logQuery;

                 lg = cmd.ExecuteNonQuery();


                #region EmailWork

                string emailBody = @"<p>Dear <b>Approver</b>,<br /> a Service Request has been initiated in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your Approval, the Details of the Service are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Service ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + serviceID + @"</p>
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
                                                                                <p>" + dd_ReportedBy.SelectedItem.Text + @"</p>
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
                                       
                                     <td><b>Task ID</b>
                                    </td>
                                     <td><b>Task Name</b>
                                    </td>
                                     <td><b>Task Team</b>
                                    </td>
                                     <td><b>Implementer</b>
                                    </td>
                                    
                                    </tr>";

                                        foreach (GridViewRow row in gv_tasks.Rows)
                                        {

                                            DropDownList dd_Team = (DropDownList)row.FindControl("dd_Team");

                                            DropDownList dd_Assignee = (DropDownList)row.FindControl("dd_Assignee");

                                            emailBody += @"  <tr>
                                      
                                                                 <td>
                                                                    <p>"+row.Cells[0].Text+@"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>" + row.Cells[1].Text + @"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>"+dd_Team.SelectedItem.Text+@"
                                                                    </p>
                                                                </td>
                                                                 <td>
                                                                    <p>"+dd_Assignee.SelectedItem.Text+@"
                                                                    </p>
                                                                </td>
                                       
                                                            </tr>";
                                        }


                                        DataTable Approverss = GetApproversEmail(serviceID);

                                        string emailIDs = "";

                                        string approverNames = "";

                                        if (Approverss.Rows.Count > 0)
                                        {

                                            foreach (DataRow item in Approverss.Rows)
                                            {
                                                emailIDs += item["Email"] + ";";
                                                approverNames += item["Employee_name"] + ","; 
                                            }

                                            emailIDs = emailIDs.TrimEnd(';');
                                            approverNames = approverNames.TrimEnd(',');
                                        }






                            emailBody += @" </table><b>Task Approver(s): </b> <p>"+approverNames+@"</p>
        
                          
                                
                                                                    <h3>Activity</h3>
                                
                                                                    <p>" + Session["Username"].ToString() + @" Initiated the Service</p>
                                
                                                                    <p>To perform action on this Service, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                
                                                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

                //Commenting below code - M.Rahim 17-Feb-2022
                //SMTP Email
                //Email.SendEmail(emailIDs, "", "For your Approval, Service ID: " + serviceID, emailBody);
                
                string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + emailIDs + "', 'For your Approval, Service ID: " + serviceID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Service', 'Email Sent form aplicationvia SMTP', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";
                cmd.CommandText = emailQuery;                
                 q = cmd.ExecuteNonQuery();


                //Added M.Rahim - 17-Feb-2022
                conHRSmart.Open();
                emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + emailIDs + "', 'For your Approval, Service ID: " + serviceID + @"', '" + emailBody + @"', 'N', 'CIS', 'Service', 'Email Sent form aplicationvia SMTP', '', GETDATE());
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

            if (x > 0)
            {
                div_success.Visible = true;
                lbl_ChangeID.Text = serviceID;
                div_main.Visible = false;
            }
            else
            {
                lbl_status.Text = "Service Creation Failed!! Please try again!";
                showAlert("Service Creation Failed!! Please try again!");
            }

        }
        catch (Exception oi)
        {
            lbl_status.Text = oi.Message.ToString();
        }


    }


    public DataTable GetApproversEmail(string serviceID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select distinct a.Approver, b.Employee_name, b.Email from Service_tasks a
                                inner join Employee b
                                on a.Approver = b.Employee_ID
                                 where a.service_id = '" + serviceID + "'";
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

    public string GetMaxService()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Service_ID as int)),0) from Service");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            maxticket = dt.Rows[0][0].ToString();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
        return maxticket;
    }

    protected void lbl_ChangeID_Click(object sender, EventArgs e)
    {

    }

    public void GetReportedBy()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"SELECT LoginId + '#' + ISNULL(EmailAddress,'') 'UserID', UserName 
                                                      FROM HRSmart_Linde..AD_Users
                                                      where ActiveFlag = 'Y'
                                                      order by UserName");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_ReportedBy.DataTextField = "UserName";
            dd_ReportedBy.DataValueField = "UserID";
            dd_ReportedBy.DataSource = dt;
            dd_ReportedBy.DataBind();
            dd_ReportedBy.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetServices()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select service_id, service_description from service_services_new  where Service_ID <> 'All'");
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
        if (e.CommandName == "Del")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            DataTable attachments = new DataTable();

            attachments = Session["attachments"] as DataTable;

            attachments.Rows.RemoveAt(RowIndex);

            attachments.AcceptChanges();

            Session["attachments"] = attachments;

            gv_AttachmentList.DataSource = attachments;
            gv_AttachmentList.DataBind();




        }
    }
    protected void btn_AddAttachment_Click(object sender, EventArgs e)
    {


        if (fp_attachment.HasFile)
        {

            DataTable attachments = new DataTable();

            attachments = Session["attachments"] as DataTable;

            DataRow row = attachments.NewRow();
            row["AttachmentID"] = (attachments.Rows.Count + 1).ToString();


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


                attachments.Rows.Add(row);
                attachments.AcceptChanges();

                gv_AttachmentList.DataSource = attachments;
                gv_AttachmentList.DataBind();

                Session["attachments"] = attachments;


            }
        }
        else
        {
            lbl_status.Text = "Please select file to upload!";
            showAlert("Please select file to upload!");
            return;
        }






    }
    protected void btn_Process_Click(object sender, EventArgs e)
    {
        DataTable services = new DataTable();

        services = Session["services"] as DataTable;

        services.Rows.Clear();

        int serviceCount = 0;

        foreach (ListItem item in dd_ServiceAffected.Items)
        {
            if (item.Selected)
            {
                serviceCount++;
                DataRow row = services.NewRow();
                row["Service_ID"] = item.Value;
                row["Service_Name"] = item.Text;

                services.Rows.Add(row);
                services.AcceptChanges();


            }
        }

        if (serviceCount > 0)
        {
            btn_Save.Enabled = true;
        }
        else
        {
            showAlert("Please select one or more Services to proceed!");
            btn_Save.Enabled = false;
            return;
        }

        gv_tasks.DataSource = services;
        gv_tasks.DataBind();

        Session["services"] = services;


        foreach (GridViewRow item in gv_tasks.Rows)
        {
            string serviceID = item.Cells[0].Text;

            DataTable dtx = new DataTable();

            dtx = GetServiceAssignGroups(serviceID);

            if (dtx.Rows.Count > 0)
            {
                DropDownList dd_Team = (DropDownList)item.FindControl("dd_Team");

                dd_Team.DataTextField = "Dept_LongDesc";
                dd_Team.DataValueField = "Dept_ID";
                dd_Team.DataSource = dtx;
                dd_Team.DataBind();


                DataTable dtxx = new DataTable();

                dtxx = GetTeamMembers(dd_Team.SelectedValue);

                if (dtxx.Rows.Count > 0)
                {
                    DropDownList dd_Assignee = (DropDownList)item.FindControl("dd_Assignee");

                    dd_Assignee.DataTextField = "Employee_name";
                    dd_Assignee.DataValueField = "Employee_ID";
                    dd_Assignee.DataSource = dtxx;
                    dd_Assignee.DataBind();
                    dd_Assignee.Items.Insert(0, new ListItem("Open", "O"));

                    DataRow[] Approver = dtxx.Select("service_mgr = '1'");

                    if (Approver.Length > 0)
                    {
                        HiddenField hd_Approver = (HiddenField)item.FindControl("hd_Approver");

                        hd_Approver.Value = Approver[0][0].ToString();
                    }
                }


            }
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
                                                                  where a.Group_ID = '" + AssignGroupID + "' and a.Status = '1'";
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
}