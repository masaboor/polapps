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
            GetImpact();
            GetUrgency();
            GetPriority();
            GetTypes();
            GetStages();
            GetServices();
            GetReasons();
            GetRisks();


            string changeID = Request.QueryString["ChangeID"];

            DataTable dt = new DataTable();

            dt = GetChangeView(changeID);



            if (dt.Rows.Count > 0)
            {
                lbl_Change_Title.Text = dt.Rows[0]["title"].ToString();
                dd_ChangeRequestor.SelectedValue = dt.Rows[0]["requestor"].ToString();

                dd_ChangeRequestor.Enabled = false;

                dd_Impact.SelectedValue = dt.Rows[0]["impact_id"].ToString();

                dd_Impact.Enabled = false;

                dd_Urgency.SelectedValue = dt.Rows[0]["urgency_id"].ToString();

                dd_Urgency.Enabled = false;

                dd_Priority.SelectedValue = dt.Rows[0]["priority_id"].ToString();

                dd_Priority.Enabled = false;

                dd_Stage.SelectedValue = dt.Rows[0]["stage_id"].ToString();

                dd_Stage.Enabled = false;

                dd_Risk.SelectedValue = dt.Rows[0]["risk_id"].ToString();

                dd_Risk.Enabled = false;

                dd_Category.SelectedValue = dt.Rows[0]["type_id"].ToString();

                dd_Category.Enabled = false;

                dd_VendorInvolved.SelectedValue = dt.Rows[0]["vendor_involved"].ToString();

                dd_VendorInvolved.Enabled = false;

                dd_Category_SelectedIndexChanged(sender, e);

                dd_Category.Enabled = false;

                dd_SubCategory.SelectedValue = dt.Rows[0]["subtype_id"].ToString();

                dd_SubCategory.Enabled = false;

                txt_datefrom.Text = dt.Rows[0]["start_date"].ToString();

                txt_datefrom.Enabled = false;

                txt_dateto.Text = dt.Rows[0]["end_date"].ToString();

                txt_dateto.Enabled = false;

                //dd_ServiceAffected.SelectedValue = dt.Rows[0]["service_id"].ToString();
                dd_ReasonForChange.SelectedValue = dt.Rows[0]["reason_id"].ToString();

                dd_ReasonForChange.Enabled = false;

                txt_Details.Text = dt.Rows[0]["change_plan"].ToString();

                txt_Details.Enabled = false;

                txt_BackoutPlan.Text = dt.Rows[0]["backout_plan"].ToString();

                txt_BackoutPlan.Enabled = false;

                dd_Approver.Items.Insert(0, new ListItem(dt.Rows[0]["approver1"].ToString(), dt.Rows[0]["approver1"].ToString()));// = dt.Rows[0]["approver"].ToString();
                dd_Approver2.Items.Insert(0, new ListItem(dt.Rows[0]["approver2"].ToString(), dt.Rows[0]["approver2"].ToString()));

                dd_Approver.Enabled = false;
                dd_Approver2.Enabled = false;


                

                txt_refIncident.Text = dt.Rows[0]["ref_incident_no"].ToString();

                txt_refIncident.Enabled = false;



                lbl_ChangeStatus.Text = dd_Stage.SelectedItem.Text;
                lbl_Change_ID.Text = changeID;
                lbl_Change_Title.Text = dt.Rows[0]["title"].ToString();


                DataTable attachhessss = new DataTable();

                attachhessss = GetChangeAttachments(changeID);

                if (attachhessss.Rows.Count > 0)
                {
                    gv_AttachmentList.DataSource = attachhessss;
                    gv_AttachmentList.DataBind();
                }

                DataTable taskssss = new DataTable();

                taskssss = GetChangeTasks(changeID);

                if (taskssss.Rows.Count > 0)
                {
                    gv_taskList.DataSource = taskssss;
                    gv_taskList.DataBind();
                }

                if (taskssss.Rows.Count > 0)
                {
                    foreach (GridViewRow item in gv_taskList.Rows)
                    {
                        string taskID = item.Cells[0].Text;
                        DropDownList dd_taskAttachment = item.FindControl("dd_taskAttachment") as DropDownList;

                        DataTable taskkkattaccchh = new DataTable();

                        taskkkattaccchh = GetAttachmentsOfChangeTasks(changeID, taskID);

                        if (taskkkattaccchh.Rows.Count > 0)
                        {
                            dd_taskAttachment.DataTextField = "filename";
                            dd_taskAttachment.DataValueField = "attachment_ID";
                            dd_taskAttachment.DataSource = taskkkattaccchh;
                            dd_taskAttachment.DataBind();
                        }
                    }
                }

                DataTable serv = new DataTable();

                serv = GetServicesOfChange(changeID);

                if (serv.Rows.Count > 0)
                {
                    foreach (DataRow item in serv.Rows)
                    {
                        dd_ServiceAffected.Items.FindByValue(item[1].ToString()).Selected = true;
                    }
                }

                //dd_ServiceAffected.Attributes.Add("disabled", "");

                dd_ServiceAffected.Enabled = false;

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

    public void GetRequestor()
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

    //    public void GetOwner()
    //    {
    //        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //        SqlConnection con = new SqlConnection(constr);
    //        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID,Employee_name from Employee
    //                                            order by Employee_name");
    //        SqlDataAdapter sda = new SqlDataAdapter();
    //        cmd.Connection = con;
    //        sda.SelectCommand = cmd;
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        sda.Fill(dt);
    //        con.Close();

    //        if (dt.Rows.Count > 0)
    //        {
    //            dd_ChangeOwner.DataTextField = "Employee_name";
    //            dd_ChangeOwner.DataValueField = "Employee_ID";
    //            dd_ChangeOwner.DataSource = dt;
    //            dd_ChangeOwner.DataBind();
    //            dd_ChangeOwner.Items.Insert(0, new ListItem("Please Select", "0"));
    //        }
    //        else
    //        {
    //            lbl_status.Text = "No DATA!";
    //        }
    //    }

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
        SqlCommand cmd = new SqlCommand("select service_id, service_description from change_services_new");
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
            dd_ServiceAffected.Items.Insert(0, new ListItem("All", "All"));
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

    public void GetApprover(string type_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID + '#' + e.email 'Employee_ID',e.Employee_name from Type_AssignmentGrp ta
                                            inner join Employee e
                                            on ta.dept_id = e.Dept_ID
											inner join AssignmentGrp_Emp ag
											on ag.Group_ID = e.Dept_ID
											and ag.Employee_ID = e.Employee_ID
                                            where ta.type_id = '" + type_id + @"'
											and ag.change_mgr = '1'
                                            and e.Employee_ID <> 'PK002C'
											 order by e.Employee_name");
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
            //dd_Approver.Items.Insert(0, new ListItem("Please Select", "0"));
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }

    //    public void GetApprover()
    //    {
    //        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //        SqlConnection con = new SqlConnection(constr);
    //        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID + '#' + email 'Employee_ID',Employee_name from Employee
    //                                            order by Employee_name");
    //        SqlDataAdapter sda = new SqlDataAdapter();
    //        cmd.Connection = con;
    //        sda.SelectCommand = cmd;
    //        DataTable dt = new DataTable();
    //        con.Open();
    //        sda.Fill(dt);
    //        con.Close();

    //        if (dt.Rows.Count > 0)
    //        {
    //            dd_Approver.DataTextField = "Employee_name";
    //            dd_Approver.DataValueField = "Employee_ID";
    //            dd_Approver.DataSource = dt;
    //            dd_Approver.DataBind();
    //            dd_Approver.Items.Insert(0, new ListItem("Please Select", "0"));
    //        }
    //        else
    //        {
    //            lbl_status.Text = "No DATA!";
    //        }
    //    }

    public void GetDelegatedApprover()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select c.temp_employee_id + '#' + e.email 'Employee_ID' from change_delegation c inner join employee e on c.temp_employee_id = e.employee_id where convert(date,GETDATE(),103) between convert(date,c.start_date,103) and convert(date,c.end_date,103)");
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


    public void GetSubTypes(string tickettype)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select subtype_id, subtype_desc from Change_Subtype where type_ID = '" + tickettype + "' and status = '1'");
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

        //GetApprover(tickettype);
        //GetImplementer(tickettype);
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

    public string addZero(string inti)
    {
        if (inti.Length == 1)
        {
            return "0" + inti;
        }
        else
        {
            return inti;
        }
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
                cmd.CommandText = "select filename, Attachment from Change_new where change_ID=@Id";
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

    public DataTable GetChangeView(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select  e1.employee_name 'Approver1', e2.employee_name 'Approver2',c.* from Change_new c left outer join employee e1 on e1.employee_ID = c.approver1 left outer join employee e2 on e2.employee_ID = c.approver2 where c.change_id = '" + changeee + @"'");
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

    public DataTable GetChangeTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select ct.task_ID,ct.task_name, ct.task_description, e.employee_name 'Task_Implementer', ct.task_start, ct.task_end, cs.status_description 'Task_Status', ct.implement_Comments, ct.Task_Result, ct.filename  from Change_new c inner join change_tasks ct on c.change_ID = ct.change_ID inner join employee e on ct.implementer = e.employee_ID inner join task_status cs on ct.status = cs.status_ID where c.change_id = '" + changeee + @"' order by ct.task_ID");
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
    protected void gv_taskList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Downnn")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            //LinkButton lb = (LinkButton)gv_taskList.Rows[RowIndex].FindControl("lb_TaskAttachment");

            //Response.Redirect("ChangeView.aspx?ChangeID=" + lb.Text);


            string ticketID = Request.QueryString["ChangeID"];

            string taskID = gv_taskList.Rows[RowIndex].Cells[0].Text;

            DropDownList dd_taskAttachment = gv_taskList.Rows[RowIndex].FindControl("dd_taskAttachment") as DropDownList;

            string attachmentID = dd_taskAttachment.SelectedValue;

            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select filename, Attachment from Change_task_attachments where change_ID=@Id and task_ID =@Id2 and attachment_ID = '" + attachmentID + "'";
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


    public DataTable GetChangeAttachments(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from change_attachments where change_id = '" + changeee + "'";

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

            string ticketID = Request.QueryString["ChangeID"];
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select filename, Attachment from Change_Attachments where change_ID=@Id and Attachment_ID = '" + attachmentId + "'";
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

    public DataTable GetAttachmentsOfChangeTasks(string changeee, string taskID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select filename,Attachment_ID from Change_task_attachments where change_id = '" + changeee + "' and task_ID = '" + taskID + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();


        return dt;
    }
}