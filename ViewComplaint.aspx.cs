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

            GetStatuses();


        }
    }


    public void GetTicket(string TicketID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Complaint_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', t.status 'StatusCode', t.employee_ID, t.assigned_employee_ID, e4.username 'AssignedBy',
                            e1.username 'Owner', 
                            e2.username 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            t.Entry_DateTime 'Created_Date',
                            t.Modify_DateTime 'Updated_Date',
                            e3.username 'StatusBy',
                            t.Details,
                             p.Priority_Desc + ' Priority ' + tt.Type_Desc + ' ' + c.Category_Desc 'Description', t.requestor_impression
                              from Complaint t
                              inner join Complaint_Status ts
                              on t.Status = ts.Status
                              inner join HRSmart_Linde..AD_Users e1
                              on  t.Employee_ID = e1.loginid
                              left outer join HRSmart_Linde..AD_Users e2
                              on  t.Assigned_Employee_ID = e2.loginid
                              left outer join HRSmart_Linde..AD_Users e4
                              on t.assigner_ID = e4.loginid
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


        DataTable TSattachments = new DataTable();
        TSattachments.Columns.Add("Employee_ID", typeof(string));
        TSattachments.Columns.Add("Employee_Name", typeof(string));
        TSattachments.Columns.Add("AttachmentID", typeof(string));
        TSattachments.Columns.Add("AttachmentName", typeof(string));
        TSattachments.Columns.Add("Dataa", typeof(Byte[]));

        TSattachments.AcceptChanges();

        Session["TSattachments"] = TSattachments;

        DataTable Sattachments = new DataTable();

        Sattachments = GetChangeTaskAttachments(TicketID);

        if (Sattachments.Rows.Count > 0)
        {

            foreach (DataRow item in Sattachments.Rows)
            {
                DataRow drr = TSattachments.NewRow();
                drr["Employee_ID"] = item["Employee_ID"].ToString();
                drr["Employee_Name"] = item["Employee_Name"].ToString();
                drr["AttachmentID"] = item["Attachment_ID"].ToString();
                drr["AttachmentName"] = item["filename"].ToString();
                drr["Dataa"] = item["Attachment"] as Byte[];

                TSattachments.Rows.Add(drr);
                TSattachments.AcceptChanges();
            }

            gv_TaskAttachment.DataSource = TSattachments;
            gv_TaskAttachment.DataBind();

            Session["TSattachments"] = TSattachments;
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


            //string dept = dt.Rows[0]["assigned_dept_id"].ToString();
            //GetEmployees(dept);


            lbl_AssignedToo.Text = dt.Rows[0]["Assigned_to"].ToString();
            lbl_OwnedBy.Text = dt.Rows[0]["AssignedBy"].ToString();


            lbl_CreatedBy.Text = dt.Rows[0]["Owner"].ToString();
            lbl_CreatedDate.Text = dt.Rows[0]["Created_Date"].ToString();

            lbl_StatusBy.Text = dt.Rows[0]["StatusBy"].ToString();
            lbl_StatusDate.Text = dt.Rows[0]["Updated_Date"].ToString();


            if (dt.Rows[0]["requestor_impression"].ToString() == "S")
            {
                lbl_requestorImpression.Text = "SATISFIED";
            }
            if (dt.Rows[0]["requestor_impression"].ToString() == "N")
            {
                lbl_requestorImpression.Text = "NOT SATISFIED";
            }


            if (dt.Rows[0]["StatusCode"].ToString() == "2")
            {
                
            }
            else
            {
                dd_Status.SelectedValue = dt.Rows[0]["StatusCode"].ToString();
            }
            

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

            //if (dt.Rows[0]["employee_ID"].ToString() == Session["UserID"].ToString())
            //{
            //    if (dt.Rows[0]["StatusCode"].ToString() == "1" || dt.Rows[0]["StatusCode"].ToString() == "3")
            //    {
            //        btn_Resolve.Visible = true;
                  
            //        btn_Active.Visible = false;
            //    }
            //    else if ((dt.Rows[0]["StatusCode"].ToString() == "2"))
            //    {
            //        btn_Resolve.Visible = false;
                  
            //        btn_Active.Visible = true;
            //        btn_Active.Text = "Update Info";
            //        //active true
            //    }
            //    else if ((dt.Rows[0]["StatusCode"].ToString() == "5"))
            //    {
            //        //active true
            //        btn_Resolve.Visible = false;
                  
            //        btn_Active.Visible = true;
            //    }
            //    else
            //    {
            //        btn_Resolve.Visible = false;
                  
            //        btn_Active.Visible = false;
            //        //active false
            //    }
            //}
            //else if (dt.Rows[0]["assigned_employee_ID"].ToString() == Session["UserID"].ToString())
            //{
            //    if (dt.Rows[0]["StatusCode"].ToString() == "1")
            //    {
            //        btn_Resolve.Visible = true;
                  
            //        btn_Active.Visible = false;
            //        //active false
            //    }
            //    else
            //    {
            //        btn_Resolve.Visible = false;
                   
            //        btn_Active.Visible = false;
            //        //active false
            //    }
            //}
            //else
            //{
            //    btn_Resolve.Visible = false;
              
            //    btn_Active.Visible = false;
            //    //active false
            //}
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
                            where ag.Group_ID = '" + dept + "'";
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
                    cmd.CommandText = "select filename, Attachment from ticket_Attachments where ticket_ID=@Id and Attachment_ID = '" + attachmentId + "'";
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

    protected void btn_Resolve_Click(object sender, EventArgs e)
    {

        string ticketID = Request.QueryString["Ticket"];

        DataTable TSattachments = new DataTable();

        TSattachments = Session["TSattachments"] as DataTable;

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '3', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        int q = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();


            string delattachmentquery = "delete from Ticket_Status_attachments where Ticket_ID = '" + ticketID + "'";

            cmd.CommandText = delattachmentquery;

            int xx = cmd.ExecuteNonQuery();

            foreach (DataRow item in TSattachments.Rows)
            {
                string TaskQuery = @"INSERT INTO Ticket_Status_attachments
                                                           (Ticket_ID,Employee_ID,Employee_Name,
                                                           attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + ticketID + "','" + Session["UserID"].ToString() + "','" + Session["UserName"].ToString() + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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
                                            ('" + ticketID + @"', '3', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Resolved the Ticket" + @"')";

            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();
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
            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
            showAlert("Ticket Creation Failed!! Please try again!");
        }
    }

//    protected void btn_MoreInfo_Click(object sender, EventArgs e)
//    {
//        string ticketID = Request.QueryString["Ticket"];

//        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
//        SqlConnection con = new SqlConnection(strConnString);

//        string ticketQuery = "update ticket set status = '2', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

//        SqlCommand cmd = new SqlCommand(ticketQuery);
//        cmd.CommandType = CommandType.Text;
//        cmd.Connection = con;

//        int x = 0;
//        int y = 0;

//        try
//        {
//            con.Open();
//            x = cmd.ExecuteNonQuery();

//            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
//                                            values
//                                            ('" + ticketID + @"', '2', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Requested more info for the Ticket" + @"')";

//            cmd.CommandText = logQuery;

//            y = cmd.ExecuteNonQuery();
//        }

//        catch (Exception ex)
//        {
//            //Response.Write(ex.Message);
//        }

//        finally
//        {
//            con.Close();
//            con.Dispose();
//        }

//        if (x > 0)
//        {
//            div_success.Visible = true;
//            lbl_ticketID.Text = ticketID;
//            div_main.Visible = false;
//        }
//        else
//        {
//            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
//            showAlert("Ticket Creation Failed!! Please try again!");
//        }
//    }

//    protected void btn_Close_Click(object sender, EventArgs e)
//    {
//        string ticketID = Request.QueryString["Ticket"];

//        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
//        SqlConnection con = new SqlConnection(strConnString);

//        string ticketQuery = "update ticket set status = '4', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "', close_user = '" + Session["UserID"].ToString() + "', close_date = getdate(), close_datetime = getdate() where ticket_ID = '" + ticketID + "'";

//        SqlCommand cmd = new SqlCommand(ticketQuery);
//        cmd.CommandType = CommandType.Text;
//        cmd.Connection = con;

//        int x = 0;
//        int y = 0;

//        try
//        {
//            con.Open();
//            x = cmd.ExecuteNonQuery();

//            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
//                                            values
//                                            ('" + ticketID + @"', '4', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Closed the Ticket" + @"')";

//            cmd.CommandText = logQuery;

//            y = cmd.ExecuteNonQuery();
//        }

//        catch (Exception ex)
//        {
//            //Response.Write(ex.Message);
//        }

//        finally
//        {
//            con.Close();
//            con.Dispose();
//        }

//        if (x > 0)
//        {
//            div_success.Visible = true;
//            lbl_ticketID.Text = ticketID;
//            div_main.Visible = false;
//        }
//        else
//        {
//            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
//            showAlert("Ticket Creation Failed!! Please try again!");
//        }
//    }

//    protected void btn_ForceClose_Click(object sender, EventArgs e)
//    {
//        string ticketID = Request.QueryString["Ticket"];

//        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
//        SqlConnection con = new SqlConnection(strConnString);

//        string ticketQuery = "update ticket set status = '5', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "', close_user = '" + Session["UserID"].ToString() + "', close_date = getdate(), close_datetime = getdate() where ticket_ID = '" + ticketID + "'";

//        SqlCommand cmd = new SqlCommand(ticketQuery);
//        cmd.CommandType = CommandType.Text;
//        cmd.Connection = con;

//        int x = 0;
//        int y = 0;

//        try
//        {
//            con.Open();
//            x = cmd.ExecuteNonQuery();

//            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
//                                            values
//                                            ('" + ticketID + @"', '5', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Force Closed the Ticket" + @"')";

//            cmd.CommandText = logQuery;

//            y = cmd.ExecuteNonQuery();
//        }

//        catch (Exception ex)
//        {
//            //Response.Write(ex.Message);
//        }

//        finally
//        {
//            con.Close();
//            con.Dispose();
//        }

//        if (x > 0)
//        {
//            div_success.Visible = true;
//            lbl_ticketID.Text = ticketID;
//            div_main.Visible = false;
//        }
//        else
//        {
//            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
//            showAlert("Ticket Creation Failed!! Please try again!");
//        }
//    }



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
                cmd.CommandText = "select filename, Attachment from Ticket where Ticket_ID=@Id";
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

    protected void btn_Active_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];

        DataTable TSattachments = new DataTable();

        TSattachments = Session["TSattachments"] as DataTable;

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '"+dd_Status.SelectedValue+"', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();


            string delattachmentquery = "delete from Ticket_Status_attachments where Ticket_ID = '" + ticketID + "'";

            cmd.CommandText = delattachmentquery;

            int xx = cmd.ExecuteNonQuery();

            foreach (DataRow item in TSattachments.Rows)
            {
                string TaskQuery = @"INSERT INTO Ticket_Status_attachments
                                                           (Ticket_ID,Employee_ID,Employee_Name,
                                                           attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + ticketID + "','" + Session["UserID"].ToString() + "','" + Session["UserName"].ToString() + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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
                                            ('" + ticketID + @"', '"+dd_Status.SelectedValue+"', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Updated the Ticket status to " + dd_Status.SelectedItem.Text + @"')";


            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();
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
            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
            showAlert("Ticket Creation Failed!! Please try again!");
        }
    }

    protected void btn_AddTaskAttachment_Click(object sender, EventArgs e)
    {
        if (fp_attachment.HasFile)
        {

            DataTable TSattachments = new DataTable();

            TSattachments = Session["TSattachments"] as DataTable;

            DataRow row = TSattachments.NewRow();
            row["Employee_ID"] = Session["UserID"].ToString();
            row["Employee_Name"] = Session["username"].ToString();
            row["AttachmentID"] = (TSattachments.Rows.Count + 1).ToString();


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


                TSattachments.Rows.Add(row);
                TSattachments.AcceptChanges();

                gv_TaskAttachment.DataSource = TSattachments;
                gv_TaskAttachment.DataBind();

                Session["TSattachments"] = TSattachments;


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

            DataTable TSattachments = new DataTable();

            TSattachments = Session["TSattachments"] as DataTable;

            TSattachments.Rows.RemoveAt(RowIndex);

            TSattachments.AcceptChanges();

            Session["TSattachments"] = TSattachments;

            gv_TaskAttachment.DataSource = TSattachments;
            gv_TaskAttachment.DataBind();




        }

        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[0].Text;

            DataTable TSattachments = new DataTable();

            TSattachments = Session["TSattachments"] as DataTable;

            byte[] bytess;
            string fileName, contentType;

            bytess = TSattachments.Rows[RowIndex]["dataa"] as byte[];

            contentType = Path.GetExtension(TSattachments.Rows[RowIndex]["AttachmentName"].ToString());
            fileName = TSattachments.Rows[RowIndex]["AttachmentName"].ToString();


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


    public DataTable GetChangeTaskAttachments(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from Complaint_Status_attachments where Complaint_id = '" + changeee + "'";

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

    public void GetStatuses()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Ticket_Status");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Status.DataTextField = "status_Desc";
            dd_Status.DataValueField = "status";
            dd_Status.DataSource = dt;
            dd_Status.DataBind();
            dd_Status.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }
    protected void btn_ReAssign_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];

        DataTable TSattachments = new DataTable();

        TSattachments = Session["TSattachments"] as DataTable;

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

        string ticketQuery = "update ticket set status = '2', Assigned_Employee_ID = '" + reassign[0].ToString() + "', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();


            string delattachmentquery = "delete from Ticket_Status_attachments where Ticket_ID = '" + ticketID + "'";

            cmd.CommandText = delattachmentquery;

            int xx = cmd.ExecuteNonQuery();

            foreach (DataRow item in TSattachments.Rows)
            {
                string TaskQuery = @"INSERT INTO Ticket_Status_attachments
                                                           (Ticket_ID,Employee_ID,Employee_Name,
                                                           attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + ticketID + "','" + Session["UserID"].ToString() + "','" + Session["UserName"].ToString() + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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
                                            ('" + ticketID + @"', '2', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Re-Assigned the Ticket to " + dd_AssignToPerson.SelectedItem.Text + @"')";


            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();
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
            lbl_status.Text = "Ticket Creation Failed!! Please try again!";
            showAlert("Ticket Creation Failed!! Please try again!");
        }
    }
}