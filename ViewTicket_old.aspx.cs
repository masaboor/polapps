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
        }
    }


    public void GetTicket(string TicketID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select t.Ticket_ID, 
                            t.title, 
                            ts.Status_Desc 'Status', t.status 'StatusCode', t.employee_ID, t.assigned_employee_ID,
                            e1.Employee_name 'Owner', 
                            e2.Employee_name 'Assigned_to', 
                            c.Category_Desc 'Category',
                            tt.Type_Desc 'Type',
                            tts.SubType_Desc 'SubType',
                            p.Priority_Desc 'Priority',
                            t.Entry_DateTime 'Created_Date',
                            t.Modify_DateTime 'Updated_Date',
                            e3.Employee_name 'StatusBy',
                            t.Details,
                            t.filename 'AttachmentName', p.Priority_Desc + ' Priority ' + tt.Type_Desc + ' ' + c.Category_Desc 'Description'
                              from ticket t
                              inner join Ticket_Status ts
                              on t.Status = ts.Status
                              inner join Employee e1
                              on  t.Employee_ID = e1.Employee_ID
                              inner join Employee e2
                              on  t.Assigned_Employee_ID = e2.Employee_ID
                              inner join Category c
                              on t.Category_ID = c.Category_ID
                              inner join Ticket_type tt
                              on t.Type_ID = tt.Type_ID
                              inner join Ticket_SubType tts
                              on t.Type_ID = tts.Type_ID
                              and t.SubType_ID = tts.SubType_ID
                              inner join Priority p
                              on t.Priority_ID = p.Priority_ID
                              left outer join Employee e3
                              on t.Modify_User = e3.Employee_ID
                              where t.Ticket_ID = '" + TicketID + "'";


        string activityQuery = "select * from ticket_log where ticket_id = '" + TicketID + "' order by status_datetime desc";

        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();


        con.Open();
        sda.Fill(dt);
        con.Close();





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
            lbl_Ticket_ID.Text = dt.Rows[0]["Ticket_ID"].ToString();
            lbl_Ticket_Title.Text = dt.Rows[0]["title"].ToString();

            lbl_SubType.Text = dt.Rows[0]["SubType"].ToString();

            lbl_Description.Text = dt.Rows[0]["Description"].ToString();





            lbl_AssignedToo.Text = dt.Rows[0]["Assigned_to"].ToString();
            lbl_OwnedBy.Text = dt.Rows[0]["Owner"].ToString();


            lbl_CreatedBy.Text = dt.Rows[0]["Owner"].ToString();
            lbl_CreatedDate.Text = dt.Rows[0]["Created_Date"].ToString();

            lbl_StatusBy.Text = dt.Rows[0]["StatusBy"].ToString();
            lbl_StatusDate.Text = dt.Rows[0]["Updated_Date"].ToString();

            lbl_Details.Text = dt.Rows[0]["Details"].ToString();


            lb_Attachment.Text = dt.Rows[0]["AttachmentName"].ToString();

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = " <div class=\"alert alert-info\"> " + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + " </div> ";
                    lt_Activity.Text += toWrite;
                }
            }

            if (dt.Rows[0]["employee_ID"].ToString() == Session["UserID"].ToString())
            {
                if (dt.Rows[0]["StatusCode"].ToString() == "1" || dt.Rows[0]["StatusCode"].ToString() == "3")
                {
                    btn_Resolve.Visible = true;
                    btn_MoreInfo.Visible = false;
                    btn_ForceClose.Visible = false;
                    btn_Close.Visible = true;
                    btn_Active.Visible = false;
                }
                else if ((dt.Rows[0]["StatusCode"].ToString() == "2"))
                {
                    btn_Resolve.Visible = false;
                    btn_MoreInfo.Visible = false;
                    btn_ForceClose.Visible = false;
                    btn_Close.Visible = true;
                    btn_Active.Visible = true;
                    btn_Active.Text = "Update Info";
                    //active true
                }
                else if ((dt.Rows[0]["StatusCode"].ToString() == "5"))
                {
                    //active true
                    btn_Resolve.Visible = false;
                    btn_MoreInfo.Visible = false;
                    btn_ForceClose.Visible = false;
                    btn_Close.Visible = false;
                    btn_Active.Visible = true;
                }
                else
                {
                    btn_Resolve.Visible = false;
                    btn_MoreInfo.Visible = false;
                    btn_ForceClose.Visible = false;
                    btn_Close.Visible = false;
                    btn_Active.Visible = false;
                    //active false
                }
            }
            else if (dt.Rows[0]["assigned_employee_ID"].ToString() == Session["UserID"].ToString())
            {
                if (dt.Rows[0]["StatusCode"].ToString() == "1")
                {
                    btn_Resolve.Visible = true;
                    btn_MoreInfo.Visible = true;
                    btn_ForceClose.Visible = true;
                    btn_Close.Visible = false;
                    btn_Active.Visible = false;
                    //active false
                }
                else
                {
                    btn_Resolve.Visible = false;
                    btn_MoreInfo.Visible = false;
                    btn_ForceClose.Visible = false;
                    btn_Close.Visible = false;
                    btn_Active.Visible = false;
                    //active false
                }
            }
            else
            {
                btn_Resolve.Visible = false;
                btn_MoreInfo.Visible = false;
                btn_ForceClose.Visible = false;
                btn_Close.Visible = false;
                btn_Active.Visible = false;
                //active false
            }
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    protected void btn_Resolve_Click(object sender, EventArgs e)
    {

        string ticketID = Request.QueryString["Ticket"];

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

    protected void btn_MoreInfo_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '2', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '2', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Requested more info for the Ticket" + @"')";

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

    protected void btn_Close_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '4', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "', close_user = '" + Session["UserID"].ToString() + "', close_date = getdate(), close_datetime = getdate() where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '4', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Closed the Ticket" + @"')";

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

    protected void btn_ForceClose_Click(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["Ticket"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '5', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "', close_user = '" + Session["UserID"].ToString() + "', close_date = getdate(), close_datetime = getdate() where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '5', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Force Closed the Ticket" + @"')";

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

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update ticket set status = '1', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', status_comments = '" + txt_Comments.Text + "' where ticket_ID = '" + ticketID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Activated the Ticket" + @"')";

            if (btn_Active.Text == "Update Info")
            {
                logQuery = @"insert into ticket_log (ticket_id, status, status_by, status_datetime, status_description)
                                            values
                                            ('" + ticketID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Updated info for the Ticket" + @"')";

            }

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