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

public partial class Change : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

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

            DataTable attachments = new DataTable();
            attachments.Columns.Add("AttachmentID", typeof(string));
            attachments.Columns.Add("AttachmentName", typeof(string));
            attachments.Columns.Add("Dataa", typeof(Byte[]));

            attachments.AcceptChanges();

            Session["attachments"] = attachments;



            GetRequestor();
            dd_ChangeRequestor.SelectedValue = Session["UserID"].ToString();

            foreach (ListItem item in dd_ChangeRequestor.Items)
            {
                if (item.Value.Contains(Session["UserID"].ToString()))
                {
                    item.Selected = true;
                }

            }

            dd_ChangeRequestor.Enabled = false;
            //GetOwner();
            GetImpact();
            GetUrgency();
            GetPriority();
            GetTypes();
            GetStages();
            dd_Stage.SelectedValue = "1";
            dd_Stage.Enabled = false;
            GetServices();
            GetReasons();
            //GetReviewer();
            GetRisks();



            string changeID = Request.QueryString["ChangeID"];

            DataTable dt = new DataTable();

            dt = GetChangeView(changeID);



            if (dt.Rows.Count > 0)
            {
                txt_Title.Text = dt.Rows[0]["title"].ToString();
                dd_ChangeRequestor.SelectedValue = dt.Rows[0]["requestor"].ToString();

                dd_ChangeRequestor.Enabled = false;

                dd_Impact.SelectedValue = dt.Rows[0]["impact_id"].ToString();

                //dd_Impact.Enabled = false;

                dd_Urgency.SelectedValue = dt.Rows[0]["urgency_id"].ToString();

                //dd_Urgency.Enabled = false;

                dd_Priority.SelectedValue = dt.Rows[0]["priority_id"].ToString();

                //dd_Priority.Enabled = false;

                dd_Stage.SelectedValue = dt.Rows[0]["stage_id"].ToString();

                //dd_Stage.Enabled = false;

                dd_Risk.SelectedValue = dt.Rows[0]["risk_id"].ToString();

                //dd_Risk.Enabled = false;

                dd_Category.SelectedValue = dt.Rows[0]["type_id"].ToString();

                //dd_Category.Enabled = false;

                dd_VendorInvolved.SelectedValue = dt.Rows[0]["vendor_involved"].ToString();

                //dd_VendorInvolved.Enabled = false;

                dd_Category_SelectedIndexChanged(sender, e);

                dd_Category.Enabled = false;

                dd_SubCategory.SelectedValue = dt.Rows[0]["subtype_id"].ToString();

                //dd_SubCategory.Enabled = false;

                DateTime startttttt = DateTime.Parse(dt.Rows[0]["start_date"].ToString());



                txt_datefrom.Text = startttttt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//dt.Rows[0]["start_date"].ToString();

                TimeSelector3.Hour = startttttt.Hour;
                TimeSelector3.Minute = startttttt.Minute;
                TimeSelector3.Second = startttttt.Second;
                //TimeSelector3.AmPm = (Enum)startttttt.ToString("tt", CultureInfo.InvariantCulture);

                //txt_datefrom.Enabled = false;


                DateTime endddddd = DateTime.Parse(dt.Rows[0]["end_date"].ToString());



                txt_dateto.Text = endddddd.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);//dt.Rows[0]["start_date"].ToString();

                TimeSelector4.Hour = endddddd.Hour;
                TimeSelector4.Minute = endddddd.Minute;
                TimeSelector4.Second = endddddd.Second;

                //txt_dateto.Text = dt.Rows[0]["end_date"].ToString();

                //txt_dateto.Enabled = false;

                //dd_ServiceAffected.SelectedValue = dt.Rows[0]["service_id"].ToString();
                dd_ReasonForChange.SelectedValue = dt.Rows[0]["reason_id"].ToString();

                //dd_ReasonForChange.Enabled = false;

                txt_Details.Text = dt.Rows[0]["change_plan"].ToString();

                //txt_Details.Enabled = false;

                txt_BackoutPlan.Text = dt.Rows[0]["backout_plan"].ToString();

                //txt_BackoutPlan.Enabled = false;

                //dd_Approver.Items.Insert(0, new ListItem(dt.Rows[0]["approver1"].ToString(), dt.Rows[0]["approver1"].ToString()));// = dt.Rows[0]["approver"].ToString();
                //dd_Approver2.Items.Insert(0, new ListItem(dt.Rows[0]["approver2"].ToString(), dt.Rows[0]["approver2"].ToString()));

                dd_Approver.Enabled = false;
                dd_Approver2.Enabled = false;


                DataTable atachhessss = new DataTable();

                atachhessss = GetChangeAttachments(changeID);

                if (atachhessss.Rows.Count > 0)
                {

                    foreach (DataRow item in atachhessss.Rows)
                    {
                        DataRow drr = attachments.NewRow();
                        drr["AttachmentID"] = item["Attachment_ID"].ToString();
                        drr["AttachmentName"] = item["filename"].ToString();
                        drr["Dataa"] = item["Attachment"] as Byte[];

                        attachments.Rows.Add(drr);
                        attachments.AcceptChanges();
                    }

                    gv_AttachmentList.DataSource = attachments;
                    gv_AttachmentList.DataBind();

                    Session["attachments"] = attachments;
                }



                txt_refIncident.Text = dt.Rows[0]["ref_incident_no"].ToString();

                //txt_refIncident.Enabled = false;



                //lbl_ChangeStatus.Text = dd_Stage.SelectedItem.Text;
                //lbl_Change_ID.Text = changeID;
                //lbl_Change_Title.Text = dt.Rows[0]["title"].ToString();


                DataTable taskssss = new DataTable();

                taskssss = GetChangeTasks(changeID);

                if (taskssss.Rows.Count > 0)
                {

                    foreach (DataRow item in taskssss.Rows)
                    {
                        DataRow drr = tasks.NewRow();
                        drr["Task_ID"] = item["Task_ID"].ToString();
                        drr["Implementer_ID"] = item["Implementer_ID"].ToString();
                        drr["Implementer_Email"] = item["Implementer_Email"].ToString();
                        drr["Task_Implementer"] = item["Task_Implementer"].ToString();
                        drr["Task_Name"] = item["Task_Name"].ToString();
                        drr["Task_Description"] = item["Task_Description"].ToString();
                        drr["Task_start"] = item["Task_start"].ToString();
                        drr["Task_end"] = item["Task_end"].ToString();

                        tasks.Rows.Add(drr);
                        tasks.AcceptChanges();
                    }

                    gv_taskList.DataSource = tasks;
                    gv_taskList.DataBind();

                    Session["tasks"] = tasks;
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

                //dd_ServiceAffected.Enabled = false;

                //DataTable dtActivity = new DataTable();

                //dtActivity = GetChangeLogs(changeID);

                //if (dtActivity.Rows.Count > 0)
                //{
                //    foreach (DataRow item in dtActivity.Rows)
                //    {
                //        string toWrite = " <div class=\"alert alert-info\"> " + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString() + ", Comments: " + item["Status_Comments"].ToString() + " </div> ";
                //        lt_Activity.Text += toWrite;
                //    }
                //}
            }


            dd_Urgency_SelectedIndexChanged(sender, e);



            GetDelegatedApprover();
            dd_Approver.Enabled = false;

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

    public DataTable GetChangeTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select ROW_NUMBER() Over ( Order By ct.task_name ) 'Task_ID', ct.task_name, ct.task_description, 
                e.employee_name 'Task_Implementer', e.email 'Implementer_Email',
                FORMAT(ct.task_start,'dd/MM/yyyy hh:mm:ss.000') 'task_start', FORMAT(ct.task_end,'dd/MM/yyyy hh:mm:ss.000') 'task_end', ct.implementer 'Implementer_ID'  from Change_new c inner join change_tasks ct on c.change_ID = ct.change_ID inner join employee e on ct.implementer = e.employee_ID inner join task_status cs on ct.status = cs.status_ID where c.change_id = '" + changeee + @"'");
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
        SqlCommand cmd = new SqlCommand("select service_id, service_description from change_services_new order by 2");
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

    public void GetImplementer(string type_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID + '#' + e.email 'Employee_ID',
                                            e.Employee_name from Type_AssignmentGrp ta
                                            inner join Employee e
                                            on ta.dept_id = e.Dept_ID
                                            inner join AssignmentGrp_Emp ag
											on ag.Group_ID = e.Dept_ID
											and ag.Employee_ID = e.Employee_ID
                                            where ta.type_id = '" + type_id + @"'
                                            order by Employee_name");
        // and e.Employee_ID <> 'PK002C' AND ag.change_mgr <> '1' order by Employee_name");
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

        GetApprover(tickettype);
        GetImplementer(tickettype);

    }

    public string GetMaxChange()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Change_ID as int)),0) from Change_new");
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

    public void ValidateMyForm()
    {
        if (txt_Title.Text == "")
        {
            showAlert("Please Enter Title!");
            return;
        }
        //if (dd_ChangeOwner.SelectedValue == "0")
        //{
        //    showAlert("Please Select Owner!");
        //    return;
        //}

        if (dd_Impact.SelectedValue == "0")
        {
            showAlert("Please Select Impact!");
            return;
        }

        if (dd_Urgency.SelectedValue == "0")
        {
            showAlert("Please Select Urgency!");
            return;
        }

        if (dd_Priority.SelectedValue == "0")
        {
            showAlert("Please Select Priority!");
            return;
        }

        if (dd_Risk.SelectedValue == "0")
        {
            showAlert("Please Select Risk!");
            return;
        }
        if (txt_refIncident.Text == "")
        {
            showAlert("Please Enter Reference Incident#!");
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

        if (txt_datefrom.Text == "")
        {
            showAlert("Please Enter Scheduled Start Date!");
            return;
        }

        if (txt_dateto.Text == "")
        {
            showAlert("Please Enter Scheduled End Date!");
            return;
        }

        if (dd_ServiceAffected.SelectedValue == "0")
        {
            showAlert("Please Select Services Affected!");
            return;
        }

        if (dd_ReasonForChange.SelectedValue == "0")
        {
            showAlert("Please Select Reason For Change!");
            return;
        }

        if (txt_Details.Text == "")
        {
            showAlert("Please Enter Details!");
            return;
        }

        //if (dd_Reviewer.SelectedValue == "0")
        //{
        //    showAlert("Please Select Reviewer!");
        //    return;
        //}

        if (dd_Implementer.SelectedValue == "0")
        {
            showAlert("Please Select Implementer!");
            return;
        }

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
    
    protected void btn_SaveAction_Click(object sender, EventArgs e)
    {
        string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');
        DataTable tasks = new DataTable();

        tasks = Session["tasks"] as DataTable;

        DataRow row = tasks.NewRow();
        row["Task_ID"] = (tasks.Rows.Count + 1).ToString();
        row["Implementer_ID"] = implementerEmail[0];
        row["Implementer_Email"] = implementerEmail[1];
        row["Task_Implementer"] = dd_Implementer.SelectedItem.Text;
        row["Task_Name"] = txt_TaskName.Text;
        row["Task_Description"] = txt_TaskDescription.Text;


        string datetime1 = txt_TaskStart.Text + " " + addZero(TimeSelector1.Hour.ToString()) + ":" + addZero(TimeSelector1.Minute.ToString()) + ":" + addZero(TimeSelector1.Second.ToString()) + ":000 " + TimeSelector1.AmPm;

        //DateTime time2 = DateTime.Parse(string.Format("{0} {1}:{2}:{3} {4}", txt_dateto.Text, TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));
        //string datetime2 = time2.Date.ToString("dd/MM/yyyy") + " " + time2.ToLongTimeString();
        string datetime2 = txt_TaskEnd.Text + " " + addZero(TimeSelector2.Hour.ToString()) + ":" + addZero(TimeSelector2.Minute.ToString()) + ":" + addZero(TimeSelector2.Second.ToString()) + ":000 " + TimeSelector2.AmPm;

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

            //if (dd_Impact.SelectedValue == "0")
            //{
            //    showAlert("Please Select Impact!");
            //    return;
            //}

            //if (dd_Urgency.SelectedValue == "0")
            //{
            //    showAlert("Please Select Urgency!");
            //    return;
            //}

            //if (dd_Priority.SelectedValue == "0")
            //{
            //    showAlert("Please Select Priority!");
            //    return;
            //}

            //if (dd_Risk.SelectedValue == "0")
            //{
            //    showAlert("Please Select Risk!");
            //    return;
            //}
            //if (txt_refIncident.Text == "")
            //{
            //    showAlert("Please Enter Reference Incident#!");
            //    return;
            //}

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

            //if (txt_datefrom.Text == "")
            //{
            //    showAlert("Please Enter Scheduled Start Date!");
            //    return;
            //}

            //if (txt_dateto.Text == "")
            //{
            //    showAlert("Please Enter Scheduled End Date!");
            //    return;
            //}

            //if (dd_ServiceAffected.SelectedValue == "0")
            //{
            //    showAlert("Please Select Services Affected!");
            //    return;
            //}

            //if (dd_ReasonForChange.SelectedValue == "0")
            //{
            //    showAlert("Please Select Reason For Change!");
            //    return;
            //}

            //if (txt_Details.Text == "")
            //{
            //    showAlert("Please Enter Change Plan!");
            //    return;
            //}

            //if (txt_BackoutPlan.Text == "")
            //{
            //    showAlert("Please Enter Backout Plan!");
            //    return;
            //}

            DataTable tasks = new DataTable();

            tasks = Session["tasks"] as DataTable;

            if (tasks.Rows.Count > 0)
            {

            }
            else
            {
                //showAlert("Please Create Change Tasks!");
                //return;
            }

            //if (dd_Reviewer.SelectedValue == "0")
            //{
            //    showAlert("Please Select Reviewer!");
            //    return;
            //}

            //if (dd_Implementer.SelectedValue == "0")
            //{
            //    showAlert("Please Select Implementer!");
            //    return;
            //}



            #endregion




            string changeID = Request.QueryString["ChangeID"];



            string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString()) + ":" + addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;

            string datetime2 = txt_dateto.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString()) + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
            string[] approver2Email = dd_Approver2.SelectedValue.Split('#');

            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');


            DataTable attachments = new DataTable();

            attachments = Session["attachments"] as DataTable;



            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);

            con.Open();

            SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);

            List<string> queries = new List<string>();


            string changeQuery = @"update Change_new
                                                       set Title = '" + txt_Title.Text + @"'
                                                       ,Submit_Date = getdate()
                                                       ,Submit_DateTime = getdate()
                                                       ,Approver1 = '" + approverEmail[0] + @"'
                                                       ,Approver2 = '" + approver2Email[0] + @"'
                                                       ,Impact_ID = '" + dd_Impact.SelectedValue + @"'
                                                       ,Urgency_ID = '" + dd_Urgency.SelectedValue + @"'
                                                       ,Priority_ID = '" + dd_Priority.SelectedValue + @"'
                                                       ,Stage_ID = '1'
                                                       ,Risk_ID = '" + dd_Risk.SelectedValue + @"'
                                                       ,Type_ID = '" + dd_Category.SelectedValue + @"'
                                                       ,SubType_ID = '" + dd_SubCategory.SelectedValue + @"'
                                                       ,Start_Date = CONVERT(datetime, '" + datetime1 + @"', 103)  
                                                       ,End_Date = CONVERT(datetime, '" + datetime2 + @"', 103)                                                      
                                                       ,Reason_ID = '" + dd_ReasonForChange.SelectedValue + @"'
                                                       ,Change_Plan = '" + txt_Details.Text.Replace("'", "") + @"'
                                                       ,Backout_Plan = '" + txt_BackoutPlan.Text.Replace("'", "") + @"'
                                                       ,Modify_DateTime = getdate()
                                                       ,Modify_User = '" + Session["UserID"].ToString() + @"'
                                                       ,Status = '0', ref_incident_no = '" + txt_refIncident.Text + "',Vendor_Involved = '" + dd_VendorInvolved.SelectedValue + "' where change_ID = '" + changeID + @"'";






            SqlCommand cmd = new SqlCommand(changeQuery);



            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.Transaction = transaction;

            cmd.ExecuteNonQuery();

            int x = 0;
            int y = 0;
            int p = 0;


            int q = 0;

            try
            {
                string delTasks = "delete from Change_tasks where Change_ID = '" + changeID + "'";

                //cmd.CommandText = delTasks;

                queries.Add(delTasks);

                foreach (DataRow item in tasks.Rows)
                {
                    string TaskQuery = @"INSERT INTO Change_tasks
                                                           (Change_ID
                                                           ,Task_Name
                                                           ,Task_Description
                                                           ,Implementer
                                                           ,Status
                                                           ,Task_start
                                                           ,Task_end)
                VALUES ('" + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" + item["Implementer_ID"].ToString() 
                           + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" + item["Task_End"].ToString() + @"', 103) )";

                    int z = 0;

                    //cmd.CommandText = TaskQuery;

                    queries.Add(TaskQuery);

                    //z = cmd.ExecuteNonQuery();
                }



                string delattachmentquery = "delete from change_attachments where Change_ID = '" + changeID + "'";

                cmd.CommandText = delattachmentquery;

                x = cmd.ExecuteNonQuery();

                foreach (DataRow item in attachments.Rows)
                {
                    string TaskQuery = @"INSERT INTO Change_attachments
                                                           (Change_ID
                                                           ,attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + changeID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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


                string delServices = "delete from Change_ServiceTrans where Change_ID = '" + changeID + "'";

                //cmd.CommandText = delServices;

                queries.Add(delServices);


                foreach (ListItem item in dd_ServiceAffected.Items)
                {
                    if (item.Selected)
                    {
                        string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                        //cmd.CommandText = serviceQuery;

                        queries.Add(serviceQuery);

                        //p = cmd.ExecuteNonQuery();
                    }

                }



                string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
                                                                    values
                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Saved the Change as Draft" + @"')";

                //cmd.CommandText = logQuery;

                queries.Add(logQuery);

                foreach (string cmx in queries)
                {
                    cmd.CommandText = cmx;
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();

                div_success.Visible = true;
                lbl_ChangeID.Text = changeID;
                div_main.Visible = false;

            }

            catch (Exception ex)
            {
                //Response.Write(ex.Message);

                transaction.Rollback();
                lbl_status.Text = ex.Message;
                showAlert(ex.Message);
            }

            finally
            {
                con.Close();
                con.Dispose();
            }



            //showAlert("Ticket Created Successfully!");

            //Response.Redirect("Dashboard.aspx");





        }
        catch (Exception oi)
        {
            lbl_status.Text = oi.Message.ToString();
        }
    }

    protected void btn_SaveNew_Click(object sender, EventArgs e)
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

            if (dd_Impact.SelectedValue == "0")
            {
                showAlert("Please Select Impact!");
                return;
            }

            if (dd_Urgency.SelectedValue == "0")
            {
                showAlert("Please Select Urgency!");
                return;
            }

            if (dd_Priority.SelectedValue == "0")
            {
                showAlert("Please Select Priority!");
                return;
            }

            if (dd_Risk.SelectedValue == "0")
            {
                showAlert("Please Select Risk!");
                return;
            }
            if (txt_refIncident.Text == "")
            {
                showAlert("Please Enter Reference Incident#!");
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

            if (txt_datefrom.Text == "")
            {
                showAlert("Please Enter Scheduled Start Date!");
                return;
            }

            if (txt_dateto.Text == "")
            {
                showAlert("Please Enter Scheduled End Date!");
                return;
            }

            //if (dd_ServiceAffected.SelectedValue == "0")
            //{
            //    showAlert("Please Select Services Affected!");
            //    return;
            //}

            if (dd_ReasonForChange.SelectedValue == "0")
            {
                showAlert("Please Select Reason For Change!");
                return;
            }

            if (txt_Details.Text == "")
            {
                showAlert("Please Enter Change Plan!");
                return;
            }

            if (txt_BackoutPlan.Text == "")
            {
                showAlert("Please Enter Backout Plan!");
                return;
            }

            DataTable tasks = new DataTable();

            tasks = Session["tasks"] as DataTable;

            if (tasks.Rows.Count > 0)
            {

            }
            else
            {
                lbl_status.Text = "There must be atleast one task to perform!";
                showAlert("There must be atleast one task to perform!");
                return;
            }

            //if (dd_Reviewer.SelectedValue == "0")
            //{
            //    showAlert("Please Select Reviewer!");
            //    return;
            //}

            //if (dd_Implementer.SelectedValue == "0")
            //{
            //    showAlert("Please Select Implementer!");
            //    return;
            //}



            #endregion

            string changeID = Request.QueryString["ChangeID"];

            string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString()) + ":" + 
                addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;
            string datetime2 = txt_dateto.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString()) + ":" + 
                addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
            string[] approver2Email = dd_Approver2.SelectedValue.Split('#');
            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');

            DataTable attachments = new DataTable();
            attachments = Session["attachments"] as DataTable;

            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            con.Open();
            SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
            List<string> queries = new List<string>();

            string changeQuery = @"update Change_new
                                                       set Title = '" + txt_Title.Text + @"'
                                                       ,Submit_Date = getdate()
                                                       ,Submit_DateTime = getdate()
                                                       ,Approver1 = '" + approverEmail[0] + @"'
                                                       ,Approver2 = '" + approver2Email[0] + @"'
                                                       ,Impact_ID = '" + dd_Impact.SelectedValue + @"'
                                                       ,Urgency_ID = '" + dd_Urgency.SelectedValue + @"'
                                                       ,Priority_ID = '" + dd_Priority.SelectedValue + @"'
                                                       ,Stage_ID = '3'
                                                       ,Risk_ID = '" + dd_Risk.SelectedValue + @"'
                                                       ,Type_ID = '" + dd_Category.SelectedValue + @"'
                                                       ,SubType_ID = '" + dd_SubCategory.SelectedValue + @"'
                                                       ,Start_Date = CONVERT(datetime, '" + datetime1 + @"', 103)  
                                                       ,End_Date = CONVERT(datetime, '" + datetime2 + @"', 103)                                                      
                                                       ,Reason_ID = '" + dd_ReasonForChange.SelectedValue + @"'
                                                       ,Change_Plan = '" + txt_Details.Text.Replace("'", "") + @"'
                                                       ,Backout_Plan = '" + txt_BackoutPlan.Text.Replace("'", "") + @"'
                                                       ,Modify_DateTime = getdate()
                                                       ,Modify_User = '" + Session["UserID"].ToString() + @"'
                                                       ,Status = '1', ref_incident_no = '" + txt_refIncident.Text + "',Vendor_Involved = '" + dd_VendorInvolved.SelectedValue + "' where change_ID = '" + changeID + @"'";



            if (dd_Urgency.SelectedValue == "05")
            {
                changeQuery = @"update Change_new
                                                       set Title = '" + txt_Title.Text + @"'
                                                       ,Submit_Date = getdate()
                                                       ,Submit_DateTime = getdate()
                                                       ,Impact_ID = '" + dd_Impact.SelectedValue + @"'
                                                       ,Urgency_ID = '" + dd_Urgency.SelectedValue + @"'
                                                       ,Priority_ID = '" + dd_Priority.SelectedValue + @"' ,approver1 = NULL, approver2 = NULL
                                                       ,Stage_ID = '3'
                                                       ,Risk_ID = '" + dd_Risk.SelectedValue + @"'
                                                       ,Type_ID = '" + dd_Category.SelectedValue + @"'
                                                       ,SubType_ID = '" + dd_SubCategory.SelectedValue + @"'
                                                       ,Start_Date = CONVERT(datetime, '" + datetime1 + @"', 103)  
                                                       ,End_Date = CONVERT(datetime, '" + datetime2 + @"', 103)                                                      
                                                       ,Reason_ID = '" + dd_ReasonForChange.SelectedValue + @"'
                                                       ,Change_Plan = '" + txt_Details.Text.Replace("'", "") + @"'
                                                       ,Backout_Plan = '" + txt_BackoutPlan.Text.Replace("'", "") + @"'
                                                       ,Modify_DateTime = getdate()
                                                       ,Modify_User = '" + Session["UserID"].ToString() + @"'
                                                       ,Status = '1', ref_incident_no = '" + txt_refIncident.Text + "',Vendor_Involved = '" + dd_VendorInvolved.SelectedValue + "' where change_ID = '" + changeID + @"'";
            }


            SqlCommand cmd = new SqlCommand(changeQuery);



            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.Transaction = transaction;

            cmd.ExecuteNonQuery();

            int x = 0;
            int y = 0;
            int p = 0;


            int q = 0;

            try
            {
                string delTasks = "delete from Change_tasks where Change_ID = '" + changeID + "'";

                //cmd.CommandText = delTasks;

                queries.Add(delTasks);



                foreach (DataRow item in tasks.Rows)
                {
                    string TaskQuery = @"INSERT INTO Change_tasks
                                                           (Change_ID
                                                           ,Task_Name
                                                           ,Task_Description
                                                           ,Implementer
                                                           ,Status
                                                           ,Task_start
                                                           ,Task_end)
        VALUES ('" + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" + item["Implementer_ID"].ToString() 
                   + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" + item["Task_End"].ToString() + @"', 103) )";

                    int z = 0;

                    //cmd.CommandText = TaskQuery;

                    queries.Add(TaskQuery);

                    //z = cmd.ExecuteNonQuery();
                }


                string delattachmentquery = "delete from change_attachments where Change_ID = '" + changeID + "'";

                cmd.CommandText = delattachmentquery;

                x = cmd.ExecuteNonQuery();

                foreach (DataRow item in attachments.Rows)
                {
                    string TaskQuery = @"INSERT INTO Change_attachments
                                                           (Change_ID
                                                           ,attachment_ID
                                                           ,attachment
                                                           ,filename)
                                                     VALUES ('" + changeID + "','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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


                string delServices = "delete from Change_ServiceTrans where Change_ID = '" + changeID + "'";

                //cmd.CommandText = delServices;

                queries.Add(delServices);


                foreach (ListItem item in dd_ServiceAffected.Items)
                {
                    if (item.Selected)
                    {
                        string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                        //cmd.CommandText = serviceQuery;

                        queries.Add(serviceQuery);

                        //p = cmd.ExecuteNonQuery();
                    }

                }



                string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
                                                                    values
                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"')";

                queries.Add(logQuery);

                #region new Email

                string emailBody = @" <p>Dear <b>Approver</b>, a Change has been initiated in CIS by <b>" + Session["Username"].ToString() 
                    + @"</b> and is now available for your Approval, the Details of the change are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Change ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + changeID + @"</p>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                                <b>Reference Incident ID: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + txt_refIncident.Text + @"</p>
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
                                                                                <b>Category: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + dd_Category.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Sub Category: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + dd_SubCategory.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                        </tr>
                        
                        
                                                                             <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Reason for Change: </b>
                                                                            </td>
                                                                             <td colspan=""3"">
                                                                               <p>" + dd_ReasonForChange.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                          
                                                                        </tr>
                                    
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Scheduled Start: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                               <p>" + txt_datefrom.Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Scheduled End: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                            <p>" + txt_dateto.Text + @"</p>
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
                                                                                <b>Impact: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + dd_Impact.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Urgency: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                                 <p>" + dd_Urgency.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                        </tr>
                                    
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Priority: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + dd_Priority.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                           <td colspan=""1"">
                                                                                <b>Risk: </b>
                                                                            </td>
                                                                            <td colspan=""1"">
                                                                                    <p>" + dd_Risk.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                          <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Is Vendor Involved </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + dd_VendorInvolved.SelectedItem.Text + @"</p>
                                                                            </td>
                                                                          
                                                                        </tr>
                                    
                                                                        
                                    
                                                                         <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Change Plan </b>
                                                                            </td>
                                                                             <td colspan=""3"">
                                                                               <p>" + txt_Details.Text + @"</p>
                                                                            </td>
                                       
                                                                        </tr>
                                                                        
                                                                            <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Backout Plan </b>
                                                                            </td>
                                                                             <td colspan=""3"">
                                                                               <p>" + txt_BackoutPlan.Text + @"</p>
                                                                            </td>
                                       
                                                                        </tr>
                                    
                                                                    </table>
        
                                <table style=""width: 100%"">
                                    <tr>
                                       
                                     <td><b>Task Implementer</b>
                                    </td>
                                     <td><b>Task Name</b>
                                    </td>
                                     <td><b>Task Description</b>
                                    </td>
                                     <td><b>Task Start</b>
                                    </td>
                                     <td><b>Task End</b>
                                    </td>
                                    </tr>";

                foreach (DataRow item in tasks.Rows)
                {

                    emailBody += @"<tr>
                                      
                                         <td>
                                            <p>" + item["Task_Implementer"].ToString() + @"
                                            </p>
                                        </td>
                                         <td>
                                            <p>" + item["Task_Name"].ToString() + @"
                                            </p>
                                        </td>
                                         <td>
                                            <p>" + item["Task_Description"].ToString() + @"
                                            </p>
                                        </td>
                                         <td>
                                            <p>" + item["Task_Start"].ToString() + @"
                                            </p>
                                        </td>
                                         <td>
                                            <p>" + item["Task_End"].ToString() + @"
                                            </p>
                                        </td>
                                    </tr>";
                }

                emailBody += "</table>";

                if (dd_Urgency.SelectedValue == "05")
                {

                }
                else
                {
                    emailBody += @"<b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
        
                            <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>";
                }

                emailBody += @" <h3>Activity</h3>
                                
                <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
                                
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
                    emailIDs = approverEmail[1] + ";" + approver2Email[1];

                #endregion

                //SMTP Email
                //Email.SendEmail(emailIDs, emailCC, "For your Approval, Change ID: " + changeID, emailBody);
                emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, 
                    From_User, Created_Date) values ('" + emailIDs + "', '" + emailCC + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody
                    + @"', 'Y', 'CIS', 'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());";

                queries.Add(emailQuery);



                foreach (string cmx in queries)
                {
                    cmd.CommandText = cmx;
                    cmd.ExecuteNonQuery();
                }




                //Added M.Rahim - 17-Feb-2022
                conHRSmart.Open();
                emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, 
                Created_Date) values ('" + emailIDs + "', '" + emailCC + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', 
                '', '', GETDATE());";
                SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
                command.ExecuteNonQuery();
                conHRSmart.Close();
                conHRSmart.Dispose();

                transaction.Commit();

                div_success.Visible = true;
                lbl_ChangeID.Text = changeID;
                div_main.Visible = false;

            }

            catch (Exception ex)
            {
                //Response.Write(ex.Message);

                transaction.Rollback();
                lbl_status.Text = ex.Message;
                showAlert(ex.Message);
            }

            finally
            {
                con.Close();
                con.Dispose();
                conHRSmart.Close();
                conHRSmart.Dispose();
            }
            //showAlert("Ticket Created Successfully!");
            //Response.Redirect("Dashboard.aspx");

        }
        catch (Exception oi)
        {
            lbl_status.Text = oi.Message.ToString();
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

        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[0].Text;

            DataTable attachments = new DataTable();

            attachments = Session["attachments"] as DataTable;

            byte[] bytess;
            string fileName, contentType;

            bytess = attachments.Rows[RowIndex]["dataa"] as byte[];

            contentType = Path.GetExtension(attachments.Rows[RowIndex]["AttachmentName"].ToString());
            fileName = attachments.Rows[RowIndex]["AttachmentName"].ToString();





            //string ticketID = Request.QueryString["ChangeID"];
            //byte[] bytes;
            //string fileName, contentType;
            //string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(constr))
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "select filename, Attachment from Change_Attachments where change_ID=@Id and Attachment_ID = '" + attachmentId + "'";
            //        cmd.Parameters.AddWithValue("@Id", ticketID);
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            sdr.Read();
            //            bytes = (byte[])sdr["Attachment"];
            //            contentType = Path.GetExtension(sdr["filename"].ToString());
            //            fileName = sdr["filename"].ToString();
            //        }
            //        con.Close();
            //    }
            //}
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
//    protected void btn_Save_Click(object sender, EventArgs e)
//    {
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

//            if (dd_Risk.SelectedValue == "0")
//            {
//                showAlert("Please Select Risk!");
//                return;
//            }
//            if (txt_refIncident.Text == "")
//            {
//                showAlert("Please Enter Reference Incident#!");
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

//            if (txt_dateto.Text == "")
//            {
//                showAlert("Please Enter Scheduled End Date!");
//                return;
//            }

//            //if (dd_ServiceAffected.SelectedValue == "0")
//            //{
//            //    showAlert("Please Select Services Affected!");
//            //    return;
//            //}

//            if (dd_ReasonForChange.SelectedValue == "0")
//            {
//                showAlert("Please Select Reason For Change!");
//                return;
//            }

//            if (txt_Details.Text == "")
//            {
//                showAlert("Please Enter Change Plan!");
//                return;
//            }

//            if (txt_BackoutPlan.Text == "")
//            {
//                showAlert("Please Enter Backout Plan!");
//                return;
//            }

//            DataTable tasks = new DataTable();

//            tasks = Session["tasks"] as DataTable;

//            if (tasks.Rows.Count > 0)
//            {

//            }
//            else
//            {
//                showAlert("Please Create Change Tasks!");
//                return;
//            }

//            //if (dd_Reviewer.SelectedValue == "0")
//            //{
//            //    showAlert("Please Select Reviewer!");
//            //    return;
//            //}

//            //if (dd_Implementer.SelectedValue == "0")
//            //{
//            //    showAlert("Please Select Implementer!");
//            //    return;
//            //}



//            #endregion


//            //if (dd_ChangeOwner.SelectedValue == dd_Reviewer.SelectedValue)
//            //{
//            //    showAlert("Owner and Reviewer cannot be same!");
//            //    return;
//            //}


//            string maxchange = GetMaxChange();

//            double newmaxchange = double.Parse(maxchange);

//            newmaxchange = newmaxchange + 1;

//            string changeID = newmaxchange.ToString();

//            //DateTime time1 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector3.Hour, TimeSelector3.Minute, TimeSelector3.Second, TimeSelector3.AmPm));

//            //DateTime time2 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));


//            //DateTime time1 = DateTime.Parse(string.Format("{0} {1}:{2}:{3} {4}", txt_datefrom.Text, TimeSelector3.Hour, TimeSelector3.Minute, TimeSelector3.Second, TimeSelector3.AmPm));
//            //string datetime1 = time1.Date.ToString("dd/MM/yyyy") + " " + time1.ToLongTimeString();

//            string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString()) + ":" + addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;

//            //DateTime time2 = DateTime.Parse(string.Format("{0} {1}:{2}:{3} {4}", txt_dateto.Text, TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));
//            //string datetime2 = time2.Date.ToString("dd/MM/yyyy") + " " + time2.ToLongTimeString();
//            string datetime2 = txt_datefrom.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString()) + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

//            //Response.Write(datetime1);
//            //lbl_status.Text = datetime1;
//            ////temporarily
//            //return;

//            //string[] reviewerEmail = dd_Reviewer.SelectedValue.Split('#');
//            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
//            string[] approver2Email = dd_Approver2.SelectedValue.Split('#');
//            //string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');

//            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');

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


//                    string changeQuery = @"INSERT INTO Change_new
//                                                       (Change_ID
//                                                       ,Title
//                                                       ,Requestor
//                                                       ,Submit_Date
//                                                       ,Submit_DateTime
//                                                       ,Approver1
//                                                       ,Approver2
//                                                       ,Impact_ID
//                                                       ,Urgency_ID
//                                                       ,Priority_ID
//                                                       ,Stage_ID
//                                                       ,Risk_ID
//                                                       ,Type_ID
//                                                       ,SubType_ID
//                                                       ,Start_Date
//                                                       ,End_Date                                                       
//                                                       ,Reason_ID
//                                                       ,Change_Plan
//                                                       ,Backout_Plan
//                                                       ,Attachment
//                                                       ,filename
//                                                       ,Modify_DateTime
//                                                       ,Modify_User
//                                                       ,Status, ref_incident_no,Vendor_Involved)
//                                                 VALUES
//                                                       ('" + changeID + @"'
//                                                       ,'" + txt_Title.Text + @"'
//                                                       ,'" + Session["UserID"].ToString() + @"'
//                                                       ,getdate()
//                                                       ,getdate()
//                                                       ,'" + approverEmail[0] + @"'
//                                                       ,'" + approver2Email[0] + @"'
//                                                       ,'" + dd_Impact.SelectedValue + @"'
//                                                       ,'" + dd_Urgency.SelectedValue + @"'
//                                                       ,'" + dd_Priority.SelectedValue + @"'
//                                                       ,'3'
//                                                       ,'" + dd_Risk.SelectedValue + @"'
//                                                       ,'" + dd_Category.SelectedValue + @"'
//                                                       ,'" + dd_SubCategory.SelectedValue + @"'
//                                                       ,CONVERT(datetime, '" + datetime1 + @"', 103)  
//                                                       ,CONVERT(datetime, '" + datetime2 + @"', 103)
//                                                       ,'" + dd_ReasonForChange.SelectedValue + @"'
//                                                       ,'" + txt_Details.Text.Replace("'", "") + @"'
//                                                       ,'" + txt_BackoutPlan.Text.Replace("'", "") + @"'
//                                                       ,@Attachment
//                                                       ,@filename
//                                                       ,getdate()
//                                                       ,'" + Session["UserID"].ToString() + @"'
//                                                       ,'1','" + txt_refIncident.Text + "','" + dd_VendorInvolved.SelectedValue + "')";

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

//                    int q = 0;

//                    try
//                    {
//                        con.Open();
//                        x = cmd.ExecuteNonQuery();


//                        foreach (DataRow item in tasks.Rows)
//                        {
//                            string TaskQuery = @"INSERT INTO Change_tasks
//                                                           (Change_ID
//                                                           ,Task_Name
//                                                           ,Task_Description
//                                                           ,Implementer
//                                                           ,Status
//                                                           ,Task_start
//                                                           ,Task_end)
//                             VALUES ('" + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" 
//                            + item["Implementer_ID"].ToString() + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" 
//                            + item["Task_End"].ToString() + @"', 103) )";

//                            int z = 0;

//                            cmd.CommandText = TaskQuery;

//                            z = cmd.ExecuteNonQuery();
//                        }


//                        foreach (ListItem item in dd_ServiceAffected.Items)
//                        {
//                            if (item.Selected)
//                            {
//                                string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
//                                cmd.CommandText = serviceQuery;

//                                p = cmd.ExecuteNonQuery();
//                            }

//                        }


//                        string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
//                            values
//                            ('"+ changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"')";

//                        cmd.CommandText = logQuery;

//                        y = cmd.ExecuteNonQuery();

//                        #region EmailWork

//                        string emailBody = @"<p>Dear All <b>,</b> a Change has been initiated in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your review, the Details of the change are given below:  </p>
//                                
//                                
//                                                                    <table>
//                                
//                                                                        <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Change ID: </b>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                                <p> " + changeID + @"</p>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                                <b>Reference Incident ID: </b>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                            <p> " + txt_refIncident.Text + @"</p>
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Change Title: </b>
//                                                                            </td>
//                                                                             <td colspan=""2"">
//                                                                              <p>" + txt_Title.Text + @"</p>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Impact: </b>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                                <p> " + dd_Impact.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                           <td colspan=""1"">
//                                                                                <b>Urgency: </b>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                                 <p> " + dd_Urgency.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Priority: </b>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                                <p> " + dd_Priority.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                           <td colspan=""1"">
//                                                                                <b>Risk: </b>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                                    <p> " + dd_Risk.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Category: </b>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                                <p> " + dd_Category.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                           <td colspan=""1"">
//                                                                                <b>Sub Category: </b>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                            <p> " + dd_SubCategory.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                        </tr>
//                        
//                        
//                                                                             <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Reason for Change: </b>
//                                                                            </td>
//                                                                             <td colspan=""2"">
//                                                                                <p> " + dd_ReasonForChange.SelectedItem.Text + @"</p>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                          
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Scheduled Start: </b>
//                                                                            </td>
//                                                                             <td colspan=""1"">
//                                                                               <p> " + txt_datefrom.Text + @"</p>
//                                                                            </td>
//                                                                           <td colspan=""1"">
//                                                                                <b>Scheduled End: </b>
//                                                                            </td>
//                                                                            <td colspan=""1"">
//                                                                            <p> " + txt_dateto.Text + @"</p>
//                                                                            </td>
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Services Affected: </b>
//                                                                            </td>
//                                                                             <td colspan=""3"">
//                                                                                <p>";


//                        foreach (ListItem item in dd_ServiceAffected.Items)
//                        {
//                            if (item.Selected)
//                            {
//                                emailBody += item.Text + ",";
//                            }
//                        }

//                        emailBody = emailBody.TrimEnd(',');


//                        emailBody += @"</p>
//                                                                            </td>
//                                       
//                                                                        </tr>
//                                    
//                                                                         <tr>
//                                                                            <td colspan=""1"">
//                                                                                <b>Details </b>
//                                                                            </td>
//                                                                             <td colspan=""3"">
//                                                                               <p> " + txt_Details.Text + @"</p>
//                                                                            </td>
//                                       
//                                                                        </tr>
//                                    
//                                                                    </table>
//                                
//                                                                    <b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
//                                
//                                                                    <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>
//                                
//                                                         
//                                                                    <h3>Activity</h3>
//                                
//                                                                    <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
//                                
//                                                                    <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
//                                
//                                                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

//                        string emailIDs = approverEmail[1] + ";" + approver2Email[1] + ";" + requestorEmail[1];
//                        foreach (DataRow item in tasks.Rows)
//                            emailIDs += ";" + item["Implementer_Email"].ToString();

//                        //SMTP Email
//                        Email.SendEmail(emailIDs, "", "For your Approval, Change ID: " + changeID, emailBody);
//                        string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
//                        values ('" + emailIDs + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', ' ', '" + Session["UserID"].ToString() + @"', GETDATE());
//                                                                        ";


//                        cmd.CommandText = emailQuery;

//                        q = cmd.ExecuteNonQuery();

//                        #endregion

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
//                        lbl_status.Text = "Change Creation Failed!! Please try again!";
//                        showAlert("Change Creation Failed!! Please try again!");
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

//                string changeQuery = @"INSERT INTO Change_new
//                                                       (Change_ID
//                                                       ,Title
//                                                       ,Requestor
//                                                       ,Submit_Date
//                                                       ,Submit_DateTime
//                                                       ,Approver1
//                                                       ,Approver2
//                                                       ,Impact_ID
//                                                       ,Urgency_ID
//                                                       ,Priority_ID
//                                                       ,Stage_ID
//                                                       ,Risk_ID
//                                                       ,Type_ID
//                                                       ,SubType_ID
//                                                       ,Start_Date
//                                                       ,End_Date                                                       
//                                                       ,Reason_ID
//                                                       ,Change_Plan
//                                                       ,Backout_Plan
//                                                       ,Modify_DateTime
//                                                       ,Modify_User
//                                                       ,Status, ref_incident_no,Vendor_Involved)
//                                                 VALUES
//                                                       ('" + changeID + @"'
//                                                       ,'" + txt_Title.Text + @"'
//                                                       ,'" + Session["UserID"].ToString() + @"'
//                                                       ,getdate()
//                                                       ,getdate()
//                                                       ,'" + approverEmail[0] + @"'
//                                                       ,'" + approver2Email[0] + @"'
//                                                       ,'" + dd_Impact.SelectedValue + @"'
//                                                       ,'" + dd_Urgency.SelectedValue + @"'
//                                                       ,'" + dd_Priority.SelectedValue + @"'
//                                                       ,'3'
//                                                       ,'" + dd_Risk.SelectedValue + @"'
//                                                       ,'" + dd_Category.SelectedValue + @"'
//                                                       ,'" + dd_SubCategory.SelectedValue + @"'
//                                                       ,CONVERT(datetime, '" + datetime1 + @"', 103)  
//                                                       ,CONVERT(datetime, '" + datetime2 + @"', 103)
//                                                       ,'" + dd_ReasonForChange.SelectedValue + @"'
//                                                       ,'" + txt_Details.Text.Replace("'", "") + @"'
//                                                       ,'" + txt_BackoutPlan.Text.Replace("'", "") + @"'
//                                                       ,getdate()
//                                                       ,'" + Session["UserID"].ToString() + @"'
//                                                       ,'1','" + txt_refIncident.Text + "','" + dd_VendorInvolved.SelectedValue + "')";



//                SqlCommand cmd = new SqlCommand(changeQuery);


//                String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
//                SqlConnection con = new SqlConnection(strConnString);
//                cmd.CommandType = CommandType.Text;
//                cmd.Connection = con;

//                int x = 0;
//                int y = 0;
//                int p = 0;


//                int q = 0;

//                try
//                {
//                    con.Open();
//                    x = cmd.ExecuteNonQuery();

//                    foreach (DataRow item in tasks.Rows)
//                    {
//                        string TaskQuery = @"INSERT INTO Change_tasks
//                                                           (Change_ID
//                                                           ,Task_Name
//                                                           ,Task_Description
//                                                           ,Implementer
//                                                           ,Status
//                                                           ,Task_start
//                                                           ,Task_end)
//                         VALUES ('" + changeID + "','" + item["Task_Name"].ToString() + "','" + item["Task_Description"].ToString() + "','" 
//                            + item["Implementer_ID"].ToString() + "','0',CONVERT(datetime, '" + item["Task_Start"].ToString() + @"', 103) ,CONVERT(datetime, '" 
//                            + item["Task_End"].ToString() + @"', 103) )";

//                        int z = 0;

//                        cmd.CommandText = TaskQuery;

//                        z = cmd.ExecuteNonQuery();
//                    }


//                    foreach (ListItem item in dd_ServiceAffected.Items)
//                    {
//                        if (item.Selected)
//                        {
//                            string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
//                            cmd.CommandText = serviceQuery;

//                            p = cmd.ExecuteNonQuery();
//                        }

//                    }



//                    string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
//                                                                    values
//                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"')";

//                    cmd.CommandText = logQuery;

//                    y = cmd.ExecuteNonQuery();


//                    #region EmailWork

//                    string emailBody = @"<p>Dear All<b>,</b> a Change has been initiated in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your review, the Details of the change are given below:  </p>
//                            
//                            
//                                                                <table>
//                            
//                                                                    <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Change ID: </b>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                            <p> " + changeID + @"</p>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                            <b>Reference Incident ID: </b>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                        <p> " + txt_refIncident.Text + @"</p>
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Change Title: </b>
//                                                                        </td>
//                                                                         <td colspan=""2"">
//                                                                          <p>" + txt_Title.Text + @"</p>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Impact: </b>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                            <p> " + dd_Impact.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                       <td colspan=""1"">
//                                                                            <b>Urgency: </b>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                             <p> " + dd_Urgency.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Priority: </b>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                            <p> " + dd_Priority.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                       <td colspan=""1"">
//                                                                            <b>Risk: </b>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                                <p> " + dd_Risk.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Category: </b>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                            <p> " + dd_Category.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                       <td colspan=""1"">
//                                                                            <b>Sub Category: </b>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                        <p> " + dd_SubCategory.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                    </tr>
//                    
//                    
//                                                                         <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Reason for Change: </b>
//                                                                        </td>
//                                                                         <td colspan=""2"">
//                                                                            <p> " + dd_ReasonForChange.SelectedItem.Text + @"</p>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                      
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Scheduled Start: </b>
//                                                                        </td>
//                                                                         <td colspan=""1"">
//                                                                           <p> " + txt_datefrom.Text + @"</p>
//                                                                        </td>
//                                                                       <td colspan=""1"">
//                                                                            <b>Scheduled End: </b>
//                                                                        </td>
//                                                                        <td colspan=""1"">
//                                                                        <p> " + txt_dateto.Text + @"</p>
//                                                                        </td>
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Services Affected: </b>
//                                                                        </td>
//                                                                         <td colspan=""3"">
//                                                                            <p>";


//                    foreach (ListItem item in dd_ServiceAffected.Items)
//                    {
//                        if (item.Selected)
//                        {
//                            emailBody += item.Text + ",";
//                        }
//                    }

//                    emailBody = emailBody.TrimEnd(',');


//                    emailBody += @"</p>
//                                                                        </td>
//                                   
//                                                                    </tr>
//                                
//                                                                     <tr>
//                                                                        <td colspan=""1"">
//                                                                            <b>Details </b>
//                                                                        </td>
//                                                                         <td colspan=""3"">
//                                                                           <p> " + txt_Details.Text + @"</p>
//                                                                        </td>
//                                   
//                                                                    </tr>
//                                
//                                                                </table>
//                            
//                            
//                                                                <b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
//                            
//                                                                <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>
//                                                        
//                                                                <h3>Activity</h3>
//                            
//                                                                <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
//                            
//                                                                <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
//                            
//                                                                <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

//                    string emailIDs = approverEmail[1] + ";" + approver2Email[1] + ";" + requestorEmail[1];
//                    foreach (DataRow item in tasks.Rows)
//                        emailIDs += ";" + item["Implementer_Email"].ToString();

//                    //SMTP Email
//                    Email.SendEmail(emailIDs, "", "For your Approval, Change ID: " + changeID, emailBody);
//                    string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
//                        values ('" + emailIDs + "', 'For your Approval, Change ID: " + changeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', ' ', '" + Session["UserID"].ToString() + @"', GETDATE());
//                                                                        ";


//                    cmd.CommandText = emailQuery;

//                    q = cmd.ExecuteNonQuery();

//                    #endregion
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
//                    lbl_status.Text = "Change Creation Failed!! Please try again!";
//                    showAlert("Change Creation Failed!! Please try again!");
//                }
//            }



//        }
//        catch (Exception oi)
//        {
//            lbl_status.Text = oi.Message.ToString();
//        }


//    }
