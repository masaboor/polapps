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

public partial class Change : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetRequestor();
            GetOwner();
            GetImpact();
            GetUrgency();
            GetPriority();
            GetTypes();
            GetStages();
            GetServices();
            GetReasons();
            GetReviewer();
            GetRisks();
            GetApprover();
            GetImplementer();

            string changeID = Request.QueryString["ChangeID"];

            DataTable dt = new DataTable();

            dt = GetChangeView(changeID);



            if (dt.Rows.Count > 0)
            {
                txt_Title.Text = dt.Rows[0]["title"].ToString();
                //dd_ChangeRequestor.SelectedValue = dt.Rows[0]["requestor"].ToString();

                foreach (ListItem item in dd_ChangeRequestor.Items)
                {
                    if (item.Value.Contains(dt.Rows[0]["requestor"].ToString()))
                    {
                        item.Selected = true;
                    }

                }

                dd_ChangeOwner.SelectedValue = dt.Rows[0]["owner"].ToString();
                dd_Impact.SelectedValue = dt.Rows[0]["impact_id"].ToString();
                dd_Urgency.SelectedValue = dt.Rows[0]["urgency_id"].ToString();
                dd_Priority.SelectedValue = dt.Rows[0]["priority_id"].ToString();
                dd_Stage.SelectedValue = dt.Rows[0]["stage_id"].ToString();
                dd_Risk.SelectedValue = dt.Rows[0]["risk_id"].ToString();
                dd_Category.SelectedValue = dt.Rows[0]["type_id"].ToString();

                dd_Category_SelectedIndexChanged(sender, e);

                dd_SubCategory.SelectedValue = dt.Rows[0]["subtype_id"].ToString();
                txt_datefrom.Text = dt.Rows[0]["start_date"].ToString();
                txt_dateto.Text = dt.Rows[0]["end_date"].ToString();
                dd_ServiceAffected.SelectedValue = dt.Rows[0]["service_id"].ToString();
                dd_ReasonForChange.SelectedValue = dt.Rows[0]["reason_id"].ToString();
                txt_Details.Text = dt.Rows[0]["details"].ToString();

                //dd_Reviewer.SelectedValue = dt.Rows[0]["reviewer"].ToString();

                foreach (ListItem item in dd_Reviewer.Items)
                {
                    if (item.Value.Contains(dt.Rows[0]["reviewer"].ToString()))
                    {
                        item.Selected = true;
                    }

                }
                //dd_Approver.SelectedValue = dt.Rows[0]["approver"].ToString();

                foreach (ListItem item in dd_Approver.Items)
                {
                    if (item.Value.Contains(dt.Rows[0]["approver"].ToString()))
                    {
                        item.Selected = true;
                    }

                }
                //dd_Implementer.SelectedValue = dt.Rows[0]["implementer"].ToString();

                foreach (ListItem item in dd_Implementer.Items)
                {
                    if (item.Value.Contains(dt.Rows[0]["implementer"].ToString()))
                    {
                        item.Selected = true;
                    }

                }

                lb_Attachment.Text = dt.Rows[0]["filename"].ToString();

                txt_refIncident.Text = dt.Rows[0]["ref_incident_no"].ToString();

                lbl_ChangeStatus.Text = dd_Stage.SelectedItem.Text;
                lbl_Change_ID.Text = changeID;
                lbl_Change_Title.Text = dt.Rows[0]["title"].ToString();


                DataTable serv = new DataTable();

                serv = GetServicesOfChange(changeID);

                if (serv.Rows.Count > 0)
                {
                    foreach (DataRow item in serv.Rows)
                    {
                        dd_ServiceAffected.Items.FindByValue(item[1].ToString()).Selected = true;
                    }
                }

                dd_ServiceAffected.Attributes.Add("disabled", "");

                DataTable dtActivity = new DataTable();

                dtActivity = GetChangeLogs(changeID);

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

    public DataTable GetServicesOfChange(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select change_id,service_id from Change_ServiceTrans where change_id = '" + changeee + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();


        return dt;
    }

    public DataTable GetChangeView(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select * from Change where change_id = '" + changeee + @"'");
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

    public DataTable GetChangeLogs(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = "select * from change_log where change_id = '" + changeee + "' order by status_datetime desc";
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

    public void GetRequestor()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_ChangeRequestor.DataTextField = "Employee_name";
            dd_ChangeRequestor.DataValueField = "Employee_ID";
            dd_ChangeRequestor.DataSource = dt;
            dd_ChangeRequestor.DataBind();
            dd_ChangeRequestor.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetOwner()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID,Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_ChangeOwner.DataTextField = "Employee_name";
            dd_ChangeOwner.DataValueField = "Employee_ID";
            dd_ChangeOwner.DataSource = dt;
            dd_ChangeOwner.DataBind();
            dd_ChangeOwner.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetImpact()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select Impact_id, impact_description from change_impact");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Impact.DataTextField = "impact_description";
            dd_Impact.DataValueField = "Impact_id";
            dd_Impact.DataSource = dt;
            dd_Impact.DataBind();
            dd_Impact.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetUrgency()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select urgency_id, urgency_description from change_urgency");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Urgency.DataTextField = "urgency_description";
            dd_Urgency.DataValueField = "urgency_id";
            dd_Urgency.DataSource = dt;
            dd_Urgency.DataBind();
            dd_Urgency.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
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

    public void GetStages()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select stage_id, stage_description from change_stage");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Stage.DataTextField = "stage_description";
            dd_Stage.DataValueField = "stage_id";
            dd_Stage.DataSource = dt;
            dd_Stage.DataBind();
            dd_Stage.Items.Insert(0, new ListItem("Please Select", "0"));
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
            dd_Category.DataTextField = "Type_Desc";
            dd_Category.DataValueField = "Type_ID";
            dd_Category.DataSource = dt;
            dd_Category.DataBind();
            dd_Category.Items.Insert(0, new ListItem("Please Select", "0"));
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
        SqlCommand cmd = new SqlCommand("select service_id, service_description from change_services");
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
            dd_ServiceAffected.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetRisks()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select risk_id, risk_description from change_risk");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Risk.DataTextField = "risk_description";
            dd_Risk.DataValueField = "risk_id";
            dd_Risk.DataSource = dt;
            dd_Risk.DataBind();
            dd_Risk.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetReasons()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select reason_id, reason_description from change_reason");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_ReasonForChange.DataTextField = "reason_description";
            dd_ReasonForChange.DataValueField = "reason_id";
            dd_ReasonForChange.DataSource = dt;
            dd_ReasonForChange.DataBind();
            dd_ReasonForChange.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetReviewer()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Reviewer.DataTextField = "Employee_name";
            dd_Reviewer.DataValueField = "Employee_ID";
            dd_Reviewer.DataSource = dt;
            dd_Reviewer.DataBind();
            dd_Reviewer.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetApprover()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Approver.DataTextField = "Employee_name";
            dd_Approver.DataValueField = "Employee_ID";
            dd_Approver.DataSource = dt;
            dd_Approver.DataBind();
            dd_Approver.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    public void GetImplementer()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
                                            order by Employee_name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Implementer.DataTextField = "Employee_name";
            dd_Implementer.DataValueField = "Employee_ID";
            dd_Implementer.DataSource = dt;
            dd_Implementer.DataBind();
            dd_Implementer.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

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
            dd_SubCategory.DataTextField = "subtype_desc";
            dd_SubCategory.DataValueField = "subtype_id";
            dd_SubCategory.DataSource = dt;
            dd_SubCategory.DataBind();
            dd_SubCategory.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }
    protected void dd_Category_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tickettype = dd_Category.SelectedValue;
        GetSubTypes(tickettype);
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

    protected void DownloadFile(object sender, EventArgs e)
    {
        string ticketID = Request.QueryString["ChangeID"];
        byte[] bytes;
        string fileName, contentType;
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select filename, Attachment from Change where change_ID=@Id";
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
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        if (txt_ApprovalComments.Text == "")
        {
            showAlert("Please Enter Approval Comments!");
            return;
        }

        string ChangeID = Request.QueryString["ChangeID"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update change set status = '3', stage_ID = '4', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approve_comments = '" + txt_ApprovalComments.Text + "', approve_date = getdate(), approve_DateTime = getdate() where change_ID = '" + ChangeID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ChangeID + @"', '3', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Approved the Change" + @"','"+txt_ApprovalComments.Text+"')";

            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();


            int q = 0;

            string emailBody = @"<p>Dear <b>" + dd_Implementer.SelectedItem.Text + @"</b>, a Change has been approved in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your implementation, the Details of the change are given below:  </p>
        
        
                                            <table>
        
                                                <tr>
                                                    <td colspan=""1"">
                                                        <b>Change ID: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + ChangeID + @"</p>
                                                    </td>
                                                    <td colspan=""1"">
                                                        <b>Reference Incident ID: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                    <p> " + txt_refIncident.Text + @"</p>
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
                                                        <b>Impact: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + dd_Impact.SelectedItem.Text + @"</p>
                                                    </td>
                                                   <td colspan=""1"">
                                                        <b>Urgency: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                         <p> " + dd_Urgency.SelectedItem.Text + @"</p>
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
                                                        <b>Risk: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                            <p> " + dd_Risk.SelectedItem.Text + @"</p>
                                                    </td>
                                                </tr>
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Category: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + dd_Category.SelectedItem.Text + @"</p>
                                                    </td>
                                                   <td colspan=""1"">
                                                        <b>Sub Category: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                    <p> " + dd_SubCategory.SelectedItem.Text + @"</p>
                                                    </td>
                                                </tr>


                                                     <tr>
                                                    <td colspan=""1"">
                                                        <b>Reason for Change: </b>
                                                    </td>
                                                     <td colspan=""2"">
                                                        <p> " + dd_ReasonForChange.SelectedItem.Text + @"</p>
                                                    </td>
                                                    <td colspan=""1"">
                                                  
                                                    </td>
                                                </tr>
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Scheduled Start: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                       <p> " + txt_datefrom.Text + @"</p>
                                                    </td>
                                                   <td colspan=""1"">
                                                        <b>Scheduled End: </b>
                                                    </td>
                                                    <td colspan=""1"">
                                                    <p> " + txt_dateto.Text + @"</p>
                                                    </td>
                                                </tr>
            
                                                 <tr>
                                                    <td colspan=""1"">
                                                        <b>Services Affected: </b>
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
                                                        <b>Details </b>
                                                    </td>
                                                     <td colspan=""3"">
                                                       <p> " + txt_Details.Text + @"</p>
                                                    </td>
               
                                                </tr>
            
                                            </table>
        
        
                                            <b>Change Reviewer: </b> <p>" + dd_Reviewer.SelectedItem.Text + @"</p>
        
                                            <b>Change Approver: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
        
                                            <b>Change Implementer: </b> <p>" + dd_Implementer.SelectedItem.Text + @"</p>
        
                                            <h3>Activity</h3>
        
                                            ";

            DataTable dtActivity = new DataTable();

            dtActivity = GetChangeLogs(ChangeID);

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                    emailBody += toWrite;

                }
            }

            emailBody += @"
        
                                            <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
        
                                            <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";


            string[] reviewerEmail = dd_Reviewer.SelectedValue.Split('#');
            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
            string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');

            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');

            string emailQuery = @"insert into Email_Logs (To_Address,Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + implementerEmail[1] + "', 'For your Implementation, Change ID: " + ChangeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";


            cmd.CommandText = emailQuery;

            q = cmd.ExecuteNonQuery();

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
            lbl_Change_ID.Text = ChangeID;
            lbl_ticketID.Text = ChangeID;
            div_main.Visible = false;
        }
        else
        {
            lbl_status.Text = "Change Approval Failed!! Please try again!";
            showAlert("Change Approval Failed!! Please try again!");
        }
    }
    protected void lbl_ticketID_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangeView.aspx?ChangeID=" + lbl_ticketID.Text);
    }
    protected void btn_Reject_Click(object sender, EventArgs e)
    {
        if (txt_ApprovalComments.Text == "")
        {
            showAlert("Please Enter Rejection Comments!");
            return;
        }

        string ChangeID = Request.QueryString["ChangeID"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        string ticketQuery = "update change set status = '9', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approve_comments = '" + txt_ApprovalComments.Text + "', approve_date = getdate(), approve_DateTime = getdate() where change_ID = '" + ChangeID + "'";

        SqlCommand cmd = new SqlCommand(ticketQuery);
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        try
        {
            con.Open();
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ChangeID + @"', '9', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Rejected the Change" + @"','" + txt_ApprovalComments.Text + "')";

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
            lbl_Change_ID.Text = ChangeID;
            lbl_ticketID.Text = ChangeID;
            div_main.Visible = false;
        }
        else
        {
            lbl_status.Text = "Change Rejection Failed!! Please try again!";
            showAlert("Change Rejection Failed!! Please try again!");
        }
    }
}