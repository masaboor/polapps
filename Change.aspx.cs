using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;

public partial class CreateChange : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                DataTable attachments = new DataTable();
                attachments.Columns.Add("AttachmentID", typeof(string));
                attachments.Columns.Add("AttachmentName", typeof(string));
                attachments.Columns.Add("Dataa", typeof(Byte[]));
                attachments.AcceptChanges();
                Session["attachments"] = attachments;

                DataTable tasks = new DataTable();
                tasks.Columns.Add("Task_ID", typeof(string));
                tasks.Columns.Add("Implementer_ID", typeof(string));
                tasks.Columns.Add("Implementer_Email", typeof(string));
                tasks.Columns.Add("Task_Implementer", typeof(string));
                tasks.Columns.Add("Task_Name", typeof(string));
                tasks.Columns.Add("Task_Description", typeof(string));
                tasks.Columns.Add("Task_Start", typeof(string));
                tasks.Columns.Add("Task_End", typeof(string));
                tasks.AcceptChanges();
                Session["tasks"] = tasks;
                
                dd_ChangeRequestor.DataBind();
                foreach (ListItem item in dd_ChangeRequestor.Items)
                {
                    if (item.Value.Contains(Session["UserID"].ToString()))
                    {
                        item.Selected = true;
                        dd_ChangeRequestor.Enabled = false;
                        break;
                    }
                }

                dd_Stage.SelectedValue = "1";
                dd_Stage.Enabled = false;
                dd_Approver.Enabled = false;
                GetDelegatedApprover();

                txt_datefrom.Text = DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                txt_dateto.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            }
        }
        catch(Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void dd_Urgency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dd_Urgency.SelectedValue == "05")
        {
            dd_Approver.Items.Clear();
            dd_Approver.Visible = false;
            dd_Approver2.Items.Clear();
        }
        else
        { }
    }

    protected void dd_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        dd_SubCategory.DataBind();
        dd_Approver.DataBind();
        dd_MainImplementer.DataBind();
    }
 
    public string GetMaxChange()
    {
        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select isnull(max(CAST(RIGHT(change_id,6) as int)),0) from change_new");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
            maxticket = dt.Rows[0][0].ToString();
        else
            lbl_status.Text = "No DATA!";
        return maxticket;
    }

    public string GetFirstApprover(string departmentType, string requestType)
    {
        string firstApproval = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string query = "select Approver1 from CR_WorkFlowSetups where Department = '" + departmentType + "' and Type = '" + requestType + "'";
        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
           return firstApproval = dt.Rows[0][0].ToString();
        else
        return firstApproval;
    }

    public void GetAndDropValuesInHistoryWorkFlow(string changeID, string departmentType, string requestType)
    {

        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string queryWorkFlowSetup = "select Approver1, Approver2, Approver3, Approver4, Approver5, Approver6 from CR_WorkFlowSetups where Department = '" + departmentType + "' and Type = '" + requestType + "'";
        SqlCommand cmd = new SqlCommand(queryWorkFlowSetup);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        try
        {
            sda.Fill(dt);
        //con.Close();

            if (dt.Rows.Count > 0)
            {
                int count = 0;
                bool nullReached = false;
                Queue<string> approvers = new Queue<string>();

                foreach (DataRow row in dt.Rows)
                {
                    foreach(var item in row.ItemArray)
                    {
                        if(!String.IsNullOrEmpty(item.ToString()) || item.ToString() != "" || item != DBNull.Value)
                        {
                            approvers.Enqueue(item.ToString());
                            count++;
                        }
                        else
                        {
                            nullReached = true;
                            break;
                        }
                    }

                    if (nullReached == true) break;
                }

                for (int i = 1; i <= count; i++)
                {
                    string Step = i.ToString();
                    string approver = approvers.Dequeue();
                    string insertIntoWorkFlowCycle = "insert into CR_HistoryWorkFlowCycles values ('" + changeID + "', '" + Step + "', '" + approver + "')";
                    cmd.CommandText = insertIntoWorkFlowCycle;
                    cmd.ExecuteNonQuery();
                }
  

            }


        }
        catch (Exception ex)
        {

            
        }
        finally
        {
            con.Close();
            con.Dispose();
 
        }

            
    }

    protected void btn_Save_Click(object sender, EventArgs e)
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
        if (dd_Impact.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Impact!";
            // showAlert("Please Select Impact!");
            return;
        }
        if (dd_Urgency.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Urgency!";
            //showAlert("Please Select Urgency!");
            return;
        }
        if (dd_Priority.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Priority!";
            // showAlert("Please Select Priority!");
            return;
        }
        if (dd_Risk.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Risk!";
            // showAlert("Please Select Risk!");
            return;
        }
        if (txt_refIncident.Text == "")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Enter Reference Incident#!";
            // showAlert("Please Enter Reference Incident#!");
            return;
        }
        if (dd_Category.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Category!";
            // showAlert("Please Select Category!");
            return;
        }
        if (dd_SubCategory.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Sub Category!";
            //showAlert("Please Select Sub Category!");
            return;
        }
        if (txt_datefrom.Text == "")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Enter Scheduled Start Date!";
            //showAlert("Please Enter Scheduled Start Date!");
            return;
        }
        if (txt_dateto.Text == "")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Enter Scheduled End Date!";
            // showAlert("Please Enter Scheduled End Date!");
            return;
        }
        if (dd_ReasonForChange.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Reason For Change!";
            //showAlert("Please Select Reason For Change!");
            return;
        }
        if (txt_Details.Text == "")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Enter Change Plan!";
            //showAlert("Please Enter Change Plan!");
            return;
        }
        if (txt_BackoutPlan.Text == "")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Enter Backout Plan!";
            //showAlert("Please Enter Backout Plan!");
            return;
        }
        if (dd_MainImplementer.SelectedValue == "0")
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select Implementer!";
            //showAlert("Please Select Implementer!");
            return;
        }

        DataTable tasks = new DataTable();
        tasks = Session["tasks"] as DataTable;

        #endregion

        double newmaxchange = double.Parse(GetMaxChange()) + 1;
        string changeID = "CR-" + DateTime.Now.ToString("yy") + "-" + addZeroNew(newmaxchange.ToString(), 6);
        string requestType = "2";

        string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString())
            + ":" + addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;
        string datetime2 = txt_dateto.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString())
            + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

        string[] approverEmail = dd_Approver.SelectedValue.Split('#');
        string[] approver2Email = dd_Approver2.SelectedValue.Split('#');
        //string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');
        string[] mainImplementerEmail = dd_MainImplementer.SelectedValue.Split('#');
        string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');
        string departmentType = dd_Category.SelectedValue;

        string changeQuery = "", TaskQuery = "", serviceQuery = "";
        List<string> queries = new List<string>();

        DataTable attachments = new DataTable();
        attachments = Session["attachments"] as DataTable;

        string firstApprover = GetFirstApprover(departmentType, requestType);

        if (dd_Urgency.SelectedValue == "05")
        {
            changeQuery = @"INSERT INTO Change_new (Change_ID, Title, Requestor, Submit_Date, Submit_DateTime, Impact_ID, Urgency_ID, Priority_ID, Stage_ID, Risk_ID, 
                    Type_ID, SubType_ID, Start_Date, End_Date, Reason_ID, Change_Plan, Backout_Plan, Modify_DateTime, Modify_User, Status, ref_incident_no,Vendor_Involved)
                     VALUES ( '" + changeID + @"', '" + txt_Title.Text + @"', '" + Session["UserID"].ToString() + @"', getdate(), getdate(), '" + dd_Impact.SelectedValue + @"', 
                     '" + dd_Urgency.SelectedValue + @"', '" + dd_Priority.SelectedValue + @"', '4', '" + dd_Risk.SelectedValue + @"', '" + dd_Category.SelectedValue + @"', 
                     '" + dd_SubCategory.SelectedValue + @"', CONVERT(datetime, '" + datetime1 + @"', 103), CONVERT(datetime, '" + datetime2 + @"', 103), 
                     '" + dd_ReasonForChange.SelectedValue + @"', '" + txt_Details.Text.Replace("'", "") + @"', '" + txt_BackoutPlan.Text.Replace("'", "") + @"', 
                     getdate(), '" + Session["UserID"].ToString() + @"', '3','" + txt_refIncident.Text + "','" + dd_VendorInvolved.SelectedValue + "')";
        }
        else
        {
            changeQuery = @"INSERT INTO Change_new (Change_ID, Title, Requestor, Submit_Date, Submit_DateTime, Approver1, Approver2, Impact_ID, Urgency_ID, 
                Priority_ID, Stage_ID, Risk_ID, Type_ID, SubType_ID, Start_Date, End_Date, Reason_ID, Change_Plan, Backout_Plan, Modify_DateTime, Modify_User, 
                Status, ref_incident_no,Vendor_Involved, Step, Pending_with) VALUES ('" + changeID + @"', '" + txt_Title.Text + @"', '" + Session["UserID"].ToString() + @"', getdate(), 
                getdate(), '" + approverEmail[0] + @"', '" + approver2Email[0] + @"', '" + dd_Impact.SelectedValue + @"', '" + dd_Urgency.SelectedValue + @"',
                '" + dd_Priority.SelectedValue + @"', '3', '" + dd_Risk.SelectedValue + @"', '" + dd_Category.SelectedValue + @"', '" + dd_SubCategory.SelectedValue + @"', 
                CONVERT(datetime, '" + datetime1 + @"', 103), CONVERT(datetime, '" + datetime2 + @"', 103), '" + dd_ReasonForChange.SelectedValue + @"', 
                '" + txt_Details.Text.Replace("'", "") + @"', '" + txt_BackoutPlan.Text.Replace("'", "") + @"', getdate(), '" + Session["UserID"].ToString() + @"'
                , '1','" + txt_refIncident.Text + "','" + dd_VendorInvolved.SelectedValue + "', '1' , '" + firstApprover + "')";
        }

        GetAndDropValuesInHistoryWorkFlow(changeID, departmentType, requestType);

//        string MainTaskQuery = @"INSERT INTO Change_tasks(Change_ID, Task_Name, Task_Description, Implementer, Status, Task_start, Task_end) VALUES 
//                    ( '" + changeID + "', '" + txt_Title.Text + "', '" + txt_Details.Text + "', '" + mainImplementerEmail[0] + "', '0', "
//                + "CONVERT(datetime, '" + datetime1 + @"', 103) ,CONVERT(datetime, '" + datetime2 + @"', 103) )";

        string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description) values
                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString()
                   + " Initiated the Change" + @"')";

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        int  s = 0; //x = 0, y = 0, z = 0, p = 0, q = 0, r = 0,

        try
        {
            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);


            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd = con.CreateCommand();
            SqlTransaction transaction = con.BeginTransaction("Transaction1");
            cmd.Transaction = transaction;

            try
            {
                queries.Add(changeQuery); //cmd.CommandText = changeQuery; x = cmd.ExecuteNonQuery();
                //queries.Add(MainTaskQuery); //cmd.CommandText = MainTaskQuery;r = cmd.ExecuteNonQuery();

                DataRow row = tasks.NewRow();
                row["Task_ID"] = (tasks.Rows.Count + 1).ToString();
                row["Implementer_ID"] = mainImplementerEmail[0];
                row["Implementer_Email"] = mainImplementerEmail[1];
                row["Task_Implementer"] = dd_MainImplementer.SelectedItem.Text;
                row["Task_Name"] = txt_Title.Text;
                row["Task_Description"] = txt_Details.Text;
                row["Task_Start"] = datetime1;
                row["Task_End"] = datetime2;
                tasks.Rows.Add(row);
                tasks.AcceptChanges();

                foreach (DataRow item in tasks.Rows)
                {
                    TaskQuery = @"INSERT INTO Change_tasks(Change_ID, Task_Name, Task_Description, Implementer, Status, Task_start, Task_end) VALUES 
                            ('" + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" + item["Implementer_ID"].ToString()
                        + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" + item["Task_End"].ToString() + @"', 103) )";
                    queries.Add(TaskQuery); //cmd.CommandText = TaskQuery;z = cmd.ExecuteNonQuery();
                }

                foreach (DataRow item in attachments.Rows)
                {
                    TaskQuery = @"INSERT INTO Change_attachments ( Change_ID, attachment_ID, attachment, filename)
                            VALUES ('" + changeID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

                    if (cmd.Parameters.Contains("@Attachment"))
                        cmd.Parameters.Clear();
                    cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = item["Dataa"] as Byte[];
                    cmd.Parameters.Add("@Filename", SqlDbType.VarChar).Value = item["attachmentname"].ToString();
                    cmd.CommandText = TaskQuery;
                    s = cmd.ExecuteNonQuery();
                }

                foreach (ListItem item in dd_ServiceAffected.Items)
                {
                    if (item.Selected)
                    {
                        serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                        queries.Add(serviceQuery); //cmd.CommandText = serviceQuery; p = cmd.ExecuteNonQuery();
                    }
                }

                queries.Add(logQuery); //cmd.CommandText = logQuery; y = cmd.ExecuteNonQuery();
                 
                #region EmailWork

                string emailBody = @" <p>Dear <b>Approvers</b>, <br /> a Change has been initiated in CIS by <b>" + Session["Username"].ToString()
                    + @"</b> and is now available for your Approval, the Details of the change are given below:  </p>
                        <table> <tr>
                            <td colspan=""1""> <b>Change ID: </b> </td>
                            <td colspan=""1""> <p>" + changeID + @"</p> </td>
                            <td colspan=""1""> <b>Reference Incident ID: </b> </td>
                            <td colspan=""1""> <p>" + txt_refIncident.Text + @"</p> </td>
                        </tr> <tr>
                            <td colspan=""1""> <b>Change Title: </b> </td>
                            <td colspan=""2""> <p>" + txt_Title.Text + @"</p> </td>
                            <td colspan=""1""> </td>
                        </tr> <tr>
                            <td colspan=""1""> <b>Category: </b> </td>
                            <td colspan=""1""> <p>" + dd_Category.SelectedItem.Text + @"</p> </td>
                            <td colspan=""1""> <b>Sub Category: </b> </td>
                            <td colspan=""1""> <p>" + dd_SubCategory.SelectedItem.Text + @"</p> </td>
                        </tr> <tr>
                            <td colspan=""1""> <b>Reason for Change: </b> </td>
                            <td colspan=""3""> <p>" + dd_ReasonForChange.SelectedItem.Text + @"</p> </td>
                        </tr> <tr>
                            <td colspan=""1""> <b>Scheduled Start: </b> </td>
                            <td colspan=""1""> <p>" + txt_datefrom.Text + @"</p> </td>
                            <td colspan=""1""> <b>Scheduled End: </b> </td>
                            <td colspan=""1""> <p>" + txt_dateto.Text + @"</p> </td>
                        </tr> <tr>
                            <td colspan=""1""> <b>Services Affected: </b> </td>
                            <td colspan=""3""> <p>";

                foreach (ListItem item in dd_ServiceAffected.Items)
                    if (item.Selected)
                        emailBody += item.Text + ",";
             
                emailBody = emailBody.TrimEnd(',');
                emailBody += @"</p> </td> </tr> <tr>
                        <td colspan=""1""> <b>Impact: </b> </td>
                        <td colspan=""1""> <p>" + dd_Impact.SelectedItem.Text + @"</p> </td>
                        <td colspan=""1""> <b>Urgency: </b> </td>
                        <td colspan=""1""> <p>" + dd_Urgency.SelectedItem.Text + @"</p> </td>   
                    </tr> <tr>
                        <td colspan=""1""> <b>Priority: </b> </td>
                        <td colspan=""1""> <p>" + dd_Priority.SelectedItem.Text + @"</p> </td>
                        <td colspan=""1""> <b>Risk: </b> </td>
                        <td colspan=""1""> <p>" + dd_Risk.SelectedItem.Text + @"</p> </td>
                    </tr> <tr>
                        <td colspan=""1""> <b>Is Vendor Involved </b> </td>
                        <td colspan=""1""> <p>" + dd_VendorInvolved.SelectedItem.Text + @"</p> </td>                                              
                    </tr> <tr>
                        <td colspan=""1""> <b>Change Plan </b> </td>
                        <td colspan=""3""> <p>" + txt_Details.Text + @"</p> </td>
                    </tr> <tr>
                        <td colspan=""1""> <b>Backout Plan </b> </td>
                        <td colspan=""3""> <p>" + txt_BackoutPlan.Text + @"</p> </td>
                    </tr> </table>
                    <table style=""width: 100%""> <tr>                       
                    <td><b>Task Implementer</b> </td>
                    <td><b>Task Name</b> </td>
                    <td><b>Task Description</b> </td>
                    <td><b>Task Start</b> </td>
                    <td><b>Task End</b> </td> </tr>";

                foreach (DataRow item in tasks.Rows)
                    //dd_Implementer.Items.FindByValue(item["Implementer_ID"].ToString()).Text.ToString()
                    emailBody += @"<tr> <td> <p>" + item["Task_Implementer"].ToString() + @" </p> </td>
                            <td> <p>" + item["Task_Name"].ToString() + @" </p> </td>
                            <td> <p>" + item["Task_Description"].ToString() + @" </p> </td>
                            <td> <p>" + item["Task_Start"].ToString() + @" </p> </td>
                            <td> <p>" + item["Task_End"].ToString() + @" </p> </td> </tr>";
                emailBody += "</table>";

                if (dd_Urgency.SelectedValue != "05")
                    emailBody += @"<b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
                            <br /> <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>";

                emailBody += @" <h3>Activity</h3> <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
                        <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                        <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

                string emailIDs = "", emailCC = "", emailQuery = "";

                emailCC = requestorEmail[1];
                foreach (DataRow item in tasks.Rows)
                    emailCC += ";" + item["Implementer_Email"].ToString();

                if (dd_Urgency.SelectedValue == "05")
                {
                    DataTable emergencyEmail = GetEmergencyApproversEmail();
                    if (emergencyEmail.Rows.Count > 0)
                    {
                        foreach (DataRow item in emergencyEmail.Rows)
                            emailIDs += item["Email"] + ";";
                        emailIDs = emailIDs.TrimEnd(';');
                    }
                }
                else
                    emailIDs = approverEmail[1];
                        //+ approver2Email[1];

                #endregion

                //SMTP Email

                try
                {
                    //Commenting below code - M.Rahim 17-Feb-2022
                    //Email.SendEmail(emailIDs, emailCC, "For your Approval, Change ID: " + changeID, emailBody);
                    string[] code = changeID.Split('-');
                    int changeCode = Int32.Parse(code[2]);

                    emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, 
                    From_User, Created_Date) values ('" + emailIDs + "', '" + emailCC + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody
                        + @"', 'Y', 'CIS', 'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());";

                    queries.Add(emailQuery); //cmd.CommandText = emailQuery; q = cmd.ExecuteNonQuery();

                    foreach (string cmx in queries)
                    {
                        cmd.CommandText = cmx;
                        cmd.ExecuteNonQuery();
                    }


                    
                    //changeCode = addZeroForInt(changeCode, 6);

                    //Added M.Rahim - 17-Feb-2022
                    conHRSmart.Open();
                    emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, 
                    From_UserCode, Created_Date, Module_Tran_Code) values ('" + emailIDs + "', '" + emailCC + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody
                      + @"', 'N', 'CIS', 'Change', '', '', GETDATE(), '" + changeCode + "');";

                    SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
                    command.ExecuteNonQuery();
                    conHRSmart.Close();
                    conHRSmart.Dispose();
                }
                catch (Exception ee)
                {
                    conHRSmart.Close();
                    conHRSmart.Dispose();
                }
                

                div_success.Visible = true;
                lbl_ChangeID.Text = changeID;
                div_main.Visible = false;
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lbl_status.Text = "Change Creation Failed!! Please try again!" + ex.ToString();
                showAlert("Change Creation Failed!! Please try again!");
            }
            finally
            {
                con.Close();
                con.Dispose();
                conHRSmart.Close();
                conHRSmart.Dispose();
            }
        }
        catch (Exception oi)
        {
            lbl_status.Text = "Change Creation Failed!! Please try again! " + oi.Message.ToString();
        }
    }

    protected void btn_SaveAction_Click(object sender, EventArgs e)
    {
        string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');
        string datetime1 = txt_TaskStart.Text + " " + addZero(TimeSelector1.Hour.ToString()) + ":" + addZero(TimeSelector1.Minute.ToString())
            + ":" + addZero(TimeSelector1.Second.ToString()) + ":000 " + TimeSelector1.AmPm;
        string datetime2 = txt_TaskEnd.Text + " " + addZero(TimeSelector2.Hour.ToString()) + ":" + addZero(TimeSelector2.Minute.ToString())
            + ":" + addZero(TimeSelector2.Second.ToString()) + ":000 " + TimeSelector2.AmPm;

        DataTable tasks = new DataTable();
        tasks = Session["tasks"] as DataTable;

        DataRow row = tasks.NewRow();
        row["Task_ID"] = (tasks.Rows.Count + 1).ToString();
        row["Implementer_ID"] = implementerEmail[0];
        row["Implementer_Email"] = implementerEmail[1];
        row["Task_Implementer"] = dd_Implementer.SelectedItem.Text;
        row["Task_Name"] = txt_TaskName.Text;
        row["Task_Description"] = txt_TaskDescription.Text;
        row["Task_Start"] = datetime1;
        row["Task_End"] = datetime2;
        tasks.Rows.Add(row);
        tasks.AcceptChanges();
        gv_taskList.DataSource = tasks;
        gv_taskList.DataBind();
        Session["tasks"] = tasks;
    }
    
    protected void btn_SaveDraft_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_status.Text = "";

            #region Validation

            if (txt_Title.Text == "")
            {
                showAlert("Please Enter Title!");
                return;
            }

            if (dd_Category.SelectedValue == "0")
            {
                showAlert("Please Select Category!");
                return;
            }

            if (dd_SubCategory.SelectedValue == "0")
            {
                showAlert("Please Select Sub Category!");
                return;
            }

            DataTable tasks = new DataTable();
            tasks = Session["tasks"] as DataTable;

            #endregion

            double newmaxchange = double.Parse(GetMaxChange()) + 1;
            string changeID = "CR-" + DateTime.Now.ToString("yy") + "-" + addZeroNew(newmaxchange.ToString(), 6);
            string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString()) 
                + ":" + addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;
            string datetime2 = txt_dateto.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString()) 
                + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;
            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
            string[] approver2Email = dd_Approver2.SelectedValue.Split('#');
            //string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');
            string[] mainImplementerEmail = dd_MainImplementer.SelectedValue.Split('#');
            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');
            string departmentType = dd_Category.SelectedValue;
            string requestType = "2";

            string firstApprover = GetFirstApprover(departmentType, requestType);

            DataTable attachments = new DataTable();
            attachments = Session["attachments"] as DataTable;

            string changeQuery = @"INSERT INTO Change_new (Change_ID, Title, Requestor, Submit_Date, Submit_DateTime, Approver1, Approver2, Impact_ID, Urgency_ID,
                    Priority_ID, Stage_ID, Risk_ID, Type_ID, SubType_ID, Start_Date, End_Date, Reason_ID, Change_Plan, Backout_Plan, Modify_DateTime, Modify_User, 
                    Status, ref_incident_no,Vendor_Involved, Step, Pending_with) VALUES ('" + changeID + @"', '" + txt_Title.Text + @"', '" + Session["UserID"].ToString() + @"', 
                    getdate(), getdate(), '" + approverEmail[0] + @"', '" + approver2Email[0] + @"', '" + dd_Impact.SelectedValue + @"', '" + dd_Urgency.SelectedValue + @"',
                    '" + dd_Priority.SelectedValue + @"', '1', '" + dd_Risk.SelectedValue + @"', '" + dd_Category.SelectedValue + @"', '" + dd_SubCategory.SelectedValue + @"',
                    CONVERT(datetime, '" + datetime1 + @"', 103), CONVERT(datetime, '" + datetime2 + @"', 103), '" + dd_ReasonForChange.SelectedValue + @"', 
                    '" + txt_Details.Text.Replace("'", "") + @"', '" + txt_BackoutPlan.Text.Replace("'", "") + @"', getdate(), '" + Session["UserID"].ToString() + @"', 
                    '0','" + txt_refIncident.Text + "','" + dd_VendorInvolved.SelectedValue + "', '1', '" + firstApprover + "')";

            //string MainTaskQuery = @"INSERT INTO Change_tasks (Change_ID, Task_Name, Task_Description, Implementer, Status, Task_start, Task_end) VALUES ('"
            //    + changeID + "','" + txt_Title.Text + "','" + txt_Details.Text + "','" + mainImplementerEmail[0] + "','0',CONVERT(datetime, '"
            //    + datetime1 + @"', 103) ,CONVERT(datetime, '" + datetime2 + @"', 103) )";

            string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description) values ('" + changeID + @"', '1', '"
                + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Saved the Change as Draft" + @"')";

            string TaskQuery = "", serviceQuery = "";

            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd = con.CreateCommand();
            SqlTransaction transaction = con.BeginTransaction("Transaction1");
            cmd.Transaction = transaction;

            int x = 0, y = 0, z = 0, p = 0, q = 0 ;

            try
            {
                cmd.CommandText = changeQuery;
                x = cmd.ExecuteNonQuery();
                //cmd.CommandText = MainTaskQuery//y = cmd.ExecuteNonQuery();

                DataRow row = tasks.NewRow();
                row["Task_ID"] = (tasks.Rows.Count + 1).ToString();
                row["Implementer_ID"] = mainImplementerEmail[0];
                row["Implementer_Email"] = mainImplementerEmail[1];
                row["Task_Implementer"] = dd_MainImplementer.SelectedItem.Text;
                row["Task_Name"] = txt_Title.Text;
                row["Task_Description"] = txt_Details.Text;
                row["Task_Start"] = datetime1;
                row["Task_End"] = datetime2;
                tasks.Rows.Add(row);
                tasks.AcceptChanges();

                foreach (DataRow item in tasks.Rows)
                {
                    TaskQuery = @"INSERT INTO Change_tasks (Change_ID, Task_Name, Task_Description, Implementer, Status, Task_start, Task_end) VALUES ('" 
                        + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" + item["Implementer_ID"].ToString() 
                        + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" + item["Task_End"].ToString() + @"', 103) )";
                    cmd.CommandText = TaskQuery;
                    z = cmd.ExecuteNonQuery();
                }

                foreach (DataRow item in attachments.Rows)
                {
                    TaskQuery = @"INSERT INTO Change_attachments (Change_ID, attachment_ID, attachment, filename) VALUES ('" + changeID + "','" 
                        + item["attachmentID"].ToString() + "',@Attachment,@Filename)";
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

                foreach (ListItem item in dd_ServiceAffected.Items)
                {
                    if (item.Selected)
                    {
                        serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                        cmd.CommandText = serviceQuery;
                        p = cmd.ExecuteNonQuery();
                    }
                }

                cmd.CommandText = logQuery;
                q = cmd.ExecuteNonQuery();

                transaction.Commit();
               
                if (x > 0)
                {

                    div_success.Visible = true;
                    lbl_ChangeID.Text = changeID;
                    div_main.Visible = false;
                }
                else
                {
                    lbl_status.Text = "Change Creation Failed!! Please try again!";
                    showAlert("Change Creation Failed!! Please try again!");
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                lbl_status.Text = "Change Draft save Failed!! Please try again! " + ex.Message.ToString();
                //Response.Write(ex.Message);
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        catch (Exception oi)
        {
            lbl_status.Text = "Change Draft save Failed!! Please try again! " + oi.Message.ToString();
        }
    }

    public DataTable GetEmergencyApproversEmail()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select ea.Employee_ID, e.Email from Emergency_Approvers ea
							   inner join Employee e
							   on e.Employee_ID = ea.Employee_ID";
        SqlCommand cmd = new SqlCommand(activityQuery);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        try
        {
            con.Open();
            sda.Fill(dt);
        }
        catch (Exception ex)
        { }
        finally
        {
            con.Close();
        }
        return dt;
    }
    
    public void GetDelegatedApprover()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select c.temp_employee_id + '#' + e.email 'Employee_ID' 
                    from change_delegation c 
                    inner join employee e on c.temp_employee_id = e.employee_id 
                    where convert(date,GETDATE(),103) between convert(date,c.start_date,103) and convert(date,c.end_date,103)");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Approver2.Items.Insert(0, new ListItem(dt.Rows[0][0].ToString(), dt.Rows[0][0].ToString()));
        }
        else
        {
            //lbl_status.Text = "No DATA!";
            dd_Approver2.Items.Insert(0, new ListItem("Arshad Manzoor", "PK002C#arshad.manzoor@pakoxygen.com"));
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
            string ext = Path.GetExtension(filename).ToLower();
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
    
    protected void gv_taskList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            DataTable tasks = new DataTable();
            tasks = Session["tasks"] as DataTable;
            tasks.Rows.RemoveAt(RowIndex);
            tasks.AcceptChanges();
            Session["tasks"] = tasks;
            gv_taskList.DataSource = tasks;
            gv_taskList.DataBind();
        }
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

    public string addZero(string inti)
    {
        if (inti.Length == 1)
            return "0" + inti;
        else
            return inti;
    }

    public string addZeroNew(string inti, int limit)
    {
        string final = "";

        int zerotoadd = limit - inti.Length;
        for (int i = 0; i < zerotoadd; i++)
            final = final + "0";
        return final + inti;
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

    protected void lbl_ChangeID_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangeView.aspx?ChangeID=" + lbl_ChangeID.Text);
    }
}

//public void ValidateMyForm()
//{
//    if (txt_Title.Text == "")
//    {
//        showAlert("Please Enter Title!");
//        return;
//    }
//    //if (dd_ChangeOwner.SelectedValue == "0")
//    //{
//    //    showAlert("Please Select Owner!");
//    //    return;
//    //}

//    if (dd_Impact.SelectedValue == "0")
//    {
//        showAlert("Please Select Impact!");
//        return;
//    }

//    if (dd_Urgency.SelectedValue == "0")
//    {
//        showAlert("Please Select Urgency!");
//        return;
//    }

//    if (dd_Priority.SelectedValue == "0")
//    {
//        showAlert("Please Select Priority!");
//        return;
//    }

//    if (dd_Risk.SelectedValue == "0")
//    {
//        showAlert("Please Select Risk!");
//        return;
//    }
//    if (txt_refIncident.Text == "")
//    {
//        showAlert("Please Enter Reference Incident#!");
//        return;
//    }

//    if (dd_Category.SelectedValue == "0")
//    {
//        showAlert("Please Select Category!");
//        return;
//    }

//    if (dd_SubCategory.SelectedValue == "0")
//    {
//        showAlert("Please Select Sub Category!");
//        return;
//    }

//    if (txt_datefrom.Text == "")
//    {
//        showAlert("Please Enter Scheduled Start Date!");
//        return;
//    }

//    if (txt_dateto.Text == "")
//    {
//        showAlert("Please Enter Scheduled End Date!");
//        return;
//    }

//    if (dd_ServiceAffected.SelectedValue == "0")
//    {
//        showAlert("Please Select Services Affected!");
//        return;
//    }

//    if (dd_ReasonForChange.SelectedValue == "0")
//    {
//        showAlert("Please Select Reason For Change!");
//        return;
//    }

//    if (txt_Details.Text == "")
//    {
//        showAlert("Please Enter Details!");
//        return;
//    }

//    //if (dd_Reviewer.SelectedValue == "0")
//    //{
//    //    showAlert("Please Select Reviewer!");
//    //    return;
//    //}

//    if (dd_Implementer.SelectedValue == "0")
//    {
//        showAlert("Please Select Implementer!");
//        return;
//    }

//}

////public void GetRequestor()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
////                                            order by Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_ChangeRequestor.DataTextField = "Employee_name";
////            dd_ChangeRequestor.DataValueField = "Employee_ID";
////            dd_ChangeRequestor.DataSource = dt;
////            dd_ChangeRequestor.DataBind();
////            dd_ChangeRequestor.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetOwner()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID,Employee_name from Employee
////                                                order by Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_ChangeOwner.DataTextField = "Employee_name";
////            dd_ChangeOwner.DataValueField = "Employee_ID";
////            dd_ChangeOwner.DataSource = dt;
////            dd_ChangeOwner.DataBind();
////            dd_ChangeOwner.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetImpact()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select Impact_id, impact_description from change_impact");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Impact.DataTextField = "impact_description";
////            dd_Impact.DataValueField = "Impact_id";
////            dd_Impact.DataSource = dt;
////            dd_Impact.DataBind();
////            dd_Impact.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetUrgency()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select urgency_id, urgency_description from change_urgency");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Urgency.DataTextField = "urgency_description";
////            dd_Urgency.DataValueField = "urgency_id";
////            dd_Urgency.DataSource = dt;
////            dd_Urgency.DataBind();
////            dd_Urgency.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetPriority()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select * from Priority where status = '1'");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Priority.DataTextField = "Priority_Desc";
////            dd_Priority.DataValueField = "Priority_ID";
////            dd_Priority.DataSource = dt;
////            dd_Priority.DataBind();
////            dd_Priority.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetStages()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select stage_id, stage_description from change_stage");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Stage.DataTextField = "stage_description";
////            dd_Stage.DataValueField = "stage_id";
////            dd_Stage.DataSource = dt;
////            dd_Stage.DataBind();
////            dd_Stage.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetRisks()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select risk_id, risk_description from change_risk");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Risk.DataTextField = "risk_description";
////            dd_Risk.DataValueField = "risk_id";
////            dd_Risk.DataSource = dt;
////            dd_Risk.DataBind();
////            dd_Risk.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetTypes()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select * from Ticket_type where status = '1'");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Category.DataTextField = "Type_Desc";
////            dd_Category.DataValueField = "Type_ID";
////            dd_Category.DataSource = dt;
////            dd_Category.DataBind();
////            dd_Category.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetServices()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select service_id, service_description from change_services_new");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_ServiceAffected.DataTextField = "service_description";
////            dd_ServiceAffected.DataValueField = "service_id";
////            dd_ServiceAffected.DataSource = dt;
////            dd_ServiceAffected.DataBind();
////            dd_ServiceAffected.Items.Insert(0, new ListItem("All", "All"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetReasons()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select reason_id, reason_description from change_reason");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_ReasonForChange.DataTextField = "reason_description";
////            dd_ReasonForChange.DataValueField = "reason_id";
////            dd_ReasonForChange.DataSource = dt;
////            dd_ReasonForChange.DataBind();
////            dd_ReasonForChange.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetApprover(string type_id)
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID + '#' + e.email 'Employee_ID',e.Employee_name from Type_AssignmentGrp ta
////                                            inner join Employee e
////                                            on ta.dept_id = e.Dept_ID
////											inner join AssignmentGrp_Emp ag
////											on ag.Group_ID = e.Dept_ID
////											and ag.Employee_ID = e.Employee_ID
////                                            where ta.type_id = '" + type_id + @"'
////											and ag.change_mgr = '1'
////                                            and e.Employee_ID <> 'PK002C'
////											 order by e.Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Approver.DataTextField = "Employee_name";
////            dd_Approver.DataValueField = "Employee_ID";
////            dd_Approver.DataSource = dt;
////            dd_Approver.DataBind();
////            //dd_Approver.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetApprover()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
////                                                order by Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Approver.DataTextField = "Employee_name";
////            dd_Approver.DataValueField = "Employee_ID";
////            dd_Approver.DataSource = dt;
////            dd_Approver.DataBind();
////            dd_Approver.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetDelegatedApprover()
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand(@"select c.temp_employee_id + '#' + e.email 'Employee_ID' from change_delegation c inner join employee e on c.temp_employee_id = e.employee_id where convert(date,GETDATE(),103) between convert(date,c.start_date,103) and convert(date,c.end_date,103)");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Approver2.Items.Insert(0, new ListItem(dt.Rows[0][0].ToString(), dt.Rows[0][0].ToString()));
////        }
////        else
////        {
////            //lbl_status.Text = "No DATA!";
////            dd_Approver2.Items.Insert(0, new ListItem("Arshad Manzoor", "PK002C#arshad.manzoor@pakoxygen.com"));
////        }
////    }

////    public void GetImplementer(string type_id)
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        //        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID,-- + '#' + e.email 'Employee_ID',
////        //                                            e.Employee_name from Type_AssignmentGrp ta
////        //                                            inner join Employee e
////        //                                            on ta.dept_id = e.Dept_ID
////        //                                            inner join AssignmentGrp_Emp ag
////        //											on ag.Group_ID = e.Dept_ID
////        //											and ag.Employee_ID = e.Employee_ID
////        //                                            where ta.type_id = '" + type_id + @"'
////        //                                             order by Employee_name");

////        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID,-- + '#' + e.email 'Employee_ID',
////                                            e.Employee_name from Type_AssignmentGrp ta                                           
////                                            inner join AssignmentGrp_Emp ag
////											on ta.Dept_ID = ag.Group_ID
////											 inner join Employee e
////                                            on ag.Employee_ID = e.Employee_ID
////                                            where ta.type_id = '" + type_id + @"'
////                                             order by Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_Implementer.DataTextField = "Employee_name";
////            dd_Implementer.DataValueField = "Employee_ID";
////            dd_Implementer.DataSource = dt;
////            dd_Implementer.DataBind();
////            dd_Implementer.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetMainImplementer(string type_id)
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        //        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID,-- + '#' + e.email 'Employee_ID',
////        //                                            e.Employee_name from Type_AssignmentGrp ta
////        //                                            inner join Employee e
////        //                                            on ta.dept_id = e.Dept_ID
////        //                                            inner join AssignmentGrp_Emp ag
////        //											on ag.Group_ID = e.Dept_ID
////        //											and ag.Employee_ID = e.Employee_ID
////        //                                            where ta.type_id = '" + type_id + @"'
////        //                                             order by Employee_name");

////        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID,-- + '#' + e.email 'Employee_ID',
////                                            e.Employee_name from Type_AssignmentGrp ta                                           
////                                            inner join AssignmentGrp_Emp ag
////											on ta.Dept_ID = ag.Group_ID
////											 inner join Employee e
////                                            on ag.Employee_ID = e.Employee_ID
////                                            where ta.type_id = '" + type_id + @"'
////                                             order by Employee_name");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_MainImplementer.DataTextField = "Employee_name";
////            dd_MainImplementer.DataValueField = "Employee_ID";
////            dd_MainImplementer.DataSource = dt;
////            dd_MainImplementer.DataBind();
////            dd_MainImplementer.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }

////    public void GetSubTypes(string tickettype)
////    {
////        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


////        SqlConnection con = new SqlConnection(constr);
////        SqlCommand cmd = new SqlCommand("select subtype_id, subtype_desc from Change_Subtype where type_ID = '" + tickettype + "' and status = '1'");
////        SqlDataAdapter sda = new SqlDataAdapter();
////        cmd.Connection = con;
////        sda.SelectCommand = cmd;
////        DataTable dt = new DataTable();
////        con.Open();
////        sda.Fill(dt);
////        con.Close();

////        if (dt.Rows.Count > 0)
////        {
////            dd_SubCategory.DataTextField = "subtype_desc";
////            dd_SubCategory.DataValueField = "subtype_id";
////            dd_SubCategory.DataSource = dt;
////            dd_SubCategory.DataBind();
////            dd_SubCategory.Items.Insert(0, new ListItem("Please Select", "0"));
////        }
////        else
////        {
////            lbl_status.Text = "No DATA!";
////        }
////    }



//GetRequestor();
//dd_ChangeRequestor.SelectedValue = Session["UserID"].ToString();//GetOwner();
//GetImpact();
//GetUrgency();
//GetPriority();
//GetTypes();
//GetStages();
//GetServices();
//GetReasons();
//GetReviewer();
//GetRisks();
//GetApprover();

//foreach (ListItem item in dd_Approver.Items)
//{
//    if (item.Value.Contains("PK002C"))
//    {
//        item.Selected = true;
//    }

//}

//dd_Approver.SelectedValue = "PK002C";


//GetDelegatedApprover();



//check for delegation here

//GetImplementer();


