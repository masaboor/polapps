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

public partial class NewTicket : System.Web.UI.Page
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

            Session["Ticketattachments"] = attachments;

            GetCategory();
            GetPriority();
            GetTypes();
            GetDepartments();

            dd_Owner.Items.Clear();
            dd_Owner.Items.Insert(0, new ListItem(Session["Username"].ToString(), Session["UserID"].ToString()));
        }
    }



    public string GetMaxTicket()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Ticket_ID as int)),0) from ticket");
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

    public void GetPriority()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Priority where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Priority.DataTextField = "Priority_Desc";
            dd_Priority.DataValueField = "Priority_ID";
            dd_Priority.DataSource = dt;
            dd_Priority.DataBind();
            dd_Priority.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }


    public void GetTypes()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Ticket_type where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Type.DataTextField = "Type_Desc";
            dd_Type.DataValueField = "Type_ID";
            dd_Type.DataSource = dt;
            dd_Type.DataBind();
            dd_Type.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    //public void GetSubTypes(string tickettype)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand("select subtype_id, subtype_desc from Ticket_Subtype where type_ID = '" + tickettype + "' and status = '1'");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();

    //    if (dt.Rows.Count > 0)
    //    {
    //        dd_SubType.DataTextField = "subtype_desc";
    //        dd_SubType.DataValueField = "subtype_id";
    //        dd_SubType.DataSource = dt;
    //        dd_SubType.DataBind();
    //        dd_SubType.Items.Insert(0, new ListItem("Please Select", "0"));
    //    }
    //    else
    //    {
    //        lbl_status.Text = "No DATA!";
    //    }
    //}

    public void GetSubTypes(string tickettype)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select subtype_id, subtype_desc from Ticket_Subtype where type_ID = '" + tickettype + "' and status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_SubType.DataTextField = "subtype_desc";
            dd_SubType.DataValueField = "subtype_id";
            dd_SubType.DataSource = dt;
            dd_SubType.DataBind();
            dd_SubType.Items.Insert(0, new ListItem("Please Select", "0"));
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
        SqlCommand cmd = new SqlCommand("select * from Category where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Category.DataTextField = "Category_Desc";
            dd_Category.DataValueField = "Category_ID";
            dd_Category.DataSource = dt;
            dd_Category.DataBind();
            dd_Category.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }


    public void GetDepartments()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Department where status = '1'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_AssignDept.DataTextField = "Dept_Desc";
            dd_AssignDept.DataValueField = "Dept_ID";
            dd_AssignDept.DataSource = dt;
            dd_AssignDept.DataBind();
            dd_AssignDept.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetEmployees(string dept)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        string query = @"select e.Employee_ID + '#' + e.email 'Employee_ID', e.employee_name from AssignmentGrp_Emp ag inner join Employee e
                            on ag.Employee_ID = e.Employee_ID
                            where ag.Group_ID = '" + dept + "' and ag.Status <> 0 and e.Employee_status = '1'";
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

    protected void dd_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tickettype = dd_Type.SelectedValue;
        GetSubTypes(tickettype);

    }

    protected void dd_AssignDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        string dept = dd_AssignDept.SelectedValue;
        GetEmployees(dept);
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            div_error.Visible = false;
            lbl_status.Text = "";
            lbl_error.Text = "";

            #region Validation

            if (txt_Title.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Title!";
                //showAlert("Please Enter Title!");
                return;
            }
            if (dd_Owner.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Owner!";
                //showAlert("Please Select Owner!");
                return;
            }

            if (dd_Priority.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Priority!";
                //showAlert("Please Select Priority!");
                return;
            }


            if (dd_Category.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Category!";
                //showAlert("Please Select Category!");
                return;
            }

            if (dd_Type.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Type!";
                // showAlert("Please Select Type!");
                return;
            }

            if (dd_SubType.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Sub Type!";
                //showAlert("Please Select Sub Type!");
                return;
            }

            if (txt_recipent.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Recipent!";
                // showAlert("Please Enter Recipent!");
                return;
            }

            if (dd_AssignDept.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Assignment Group!";
                //showAlert("Please Select Assignment Group!");
                return;
            }

            if (dd_AssignToPerson.SelectedValue == "0")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select Assigned Person!";
                // showAlert("Please Select Assigned Person!");
                return;
            }


            //if (dd_ServiceAffected.SelectedValue == "0")
            //{
            //    showAlert("Please Select Services Affected!");
            //    return;
            //}


            if (txt_Details.Text == "")
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Enter Details!";
                //showAlert("Please Enter Details!");
                return;
            }


            #endregion


            //string maxticket = GetMaxTicket();

            //double newmaxticket = double.Parse(maxticket);

            //newmaxticket = newmaxticket + 1;

            //string ticketID = newmaxticket.ToString();

            string maxticket = GetMaxTicketnew();

            double newmaxticket = double.Parse(maxticket);

            newmaxticket = newmaxticket + 1;

            //string changeID = newmaxchange.ToString();

            string ticketID = "IR-" + DateTime.Now.ToString("yy") + "-" + addZeroNew(newmaxticket.ToString(), 6);

            string[] implementerEmail = dd_AssignToPerson.SelectedValue.Split('#');


            DataTable attachments = new DataTable();

            attachments = Session["Ticketattachments"] as DataTable;

            string ticketQuery = @"insert into Ticket (Ticket_ID,
                                                        Ticket_Date, 
                                                        Entry_DateTime, 
                                                        Dept_ID, Title,
                                                        Employee_ID, 
                                                        Assigned_Dept_ID, 
                                                        Assigned_Employee_ID,
                                                        Priority_ID, 
                                                        Category_ID, 
                                                        Type_ID, 
                                                        SubType_ID, 
                                                        Details, 
                                                        Status, Recipent)
                                                        values
                                                        ('" + ticketID + @"',
                                                        getdate(), 
                                                        getdate(), 
                                                        (select dept_id from Employee where Employee_ID = '" + Session["UserID"].ToString() + @"'), 
                                                        '" + txt_Title.Text + @"',                                                        
                                                        '" + Session["UserID"].ToString() + @"', 
                                                        '" + dd_AssignDept.SelectedValue + @"', 
                                                        '" + implementerEmail[0] + @"',
                                                        '" + dd_Priority.SelectedValue + @"', 
                                                        '" + dd_Category.SelectedValue + @"', 
                                                        '" + dd_Type.SelectedValue + @"', 
                                                        '" + dd_SubType.SelectedValue + @"', 
                                                        '" + txt_Details.Text.Replace("'", "") + @"', 
                                                        '1','"+txt_recipent.Text+"')";

            SqlCommand cmd = new SqlCommand(ticketQuery);


            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            int x = 0;
            int y = 0;

            int q = 0;

            try
            {
                con.Open();
                x = cmd.ExecuteNonQuery();


                foreach (DataRow item in attachments.Rows)
                {
                    string TaskQuery = @"INSERT INTO Ticket_attachments
                                                           (Ticket_ID
                                                           ,attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + ticketID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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




                string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Created the Ticket" + @"')";

                cmd.CommandText = logQuery;

                y = cmd.ExecuteNonQuery();



                string emailBody = @"<p>Dear <b>" + dd_AssignToPerson.SelectedItem.Text + @"</b>, an Incident has been raised in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now pending for your action, the Details of the Incident are given below:  </p>
        
        
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
                                                      <p>" + txt_Title.Text + @"</p>
                                                    </td>
                                                     <td colspan=""1"">
                                                    </td>
                                                </tr>
            
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Priority: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + dd_Priority.SelectedItem.Text + @"</p>
                                                    </td>
                                                   <td colspan=""1"">
                                                        <b>Category: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p>  " + dd_Category.SelectedItem.Text + @"</p>
                                                    </td>
                                                </tr>
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Type: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p>  " + dd_Type.SelectedItem.Text + @"</p>
                                                    </td>
                                                   <td colspan=""1"">
                                                        <b>Sub Type: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                    <p>  " + dd_SubType.SelectedItem.Text + @"</p>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td colspan=""1"">
                                                        <b>Service Recipent: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p>  " + txt_recipent.Text + @"</p>
                                                    </td>
                                                  
                                                </tr>
                                                    
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Details </b>
                                                    </td>
                                                     <td colspan=""3"">
                                                       <p> " + txt_Details.Text + @"</p>
                                                    </td>
               
                                                </tr>
            
                                            </table>
        
        
                                            <b>Incident Owner: </b> <p> " + dd_Owner.SelectedItem.Text + @"</p>

                                            <b>Incident Submitter: </b> <p>" + Session["Username"].ToString() + @"</p>
        
                                            <b>Incident Actioner: </b> <p> " + dd_AssignToPerson.SelectedItem.Text + @"</p>
        
        
                                            <h3>Activity</h3>
        
                                            <p>" + Session["Username"].ToString() + " Created the Ticket" + @"</p>
        
                                            <p>To perform action on this Incident, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
        
                                            <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";



                //Commenting below code - M.Rahim 17-Feb-2022
                //SMTP Email
                //Email.SendEmail(implementerEmail[1], "", "For your Action, Ticket ID: " + ticketID, emailBody);


                string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + implementerEmail[1] + "', 'For your Action, Ticket ID: " + ticketID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Incident', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";
                cmd.CommandText = emailQuery;
                q = cmd.ExecuteNonQuery();


                //Added M.Rahim - 17-Feb-2022
                conHRSmart.Open(); 
                emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, Created_Date)
                                                values ('" + implementerEmail[1] + "', 'For your Action, Ticket ID: " + ticketID + @"', '" + emailBody + @"', 'N', 'CIS', 'Incident', '', '', GETDATE());
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
                conHRSmart.Close();
                conHRSmart.Dispose();
            }

            if (x > 0)
            {

                div_success.Visible = true;
                lbl_ticketID.Text = ticketID;
                div_main.Visible = false;
                //showAlert("Ticket Created Successfully!");

                //Response.Redirect("Dashboard.aspx");
            }
            else
            {
                lbl_status.Text = "Ticket Creation Failed!! Please try again!";
                showAlert("Ticket Creation Failed!! Please try again!");
            }

        }
        catch (Exception po)
        {
            lbl_status.Text = po.Message.ToString();
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
        Response.Redirect("ViewTicket.aspx?Ticket=" + lbl_ticketID.Text);
    }

    protected void gv_AttachmentList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            DataTable attachments = new DataTable();

            attachments = Session["Ticketattachments"] as DataTable;

            attachments.Rows.RemoveAt(RowIndex);

            attachments.AcceptChanges();

            Session["Ticketattachments"] = attachments;

            gv_AttachmentList.DataSource = attachments;
            gv_AttachmentList.DataBind();




        }
    }
    protected void btn_AddAttachment_Click(object sender, EventArgs e)
    {


        if (fp_attachment.HasFile)
        {

            DataTable attachments = new DataTable();

            attachments = (DataTable)Session["Ticketattachments"];

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

                Session["Ticketattachments"] = attachments;


            }
        }
        else
        {
            lbl_status.Text = "Please select file to upload!";
            showAlert("Please select file to upload!");
            return;
        }






    }

    public string GetMaxTicketnew()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Change_ID as int)),0) from Change_new");
        SqlCommand cmd = new SqlCommand("select isnull(max(CAST(RIGHT(ticket_id,6) as int)),0) from ticket");
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
}