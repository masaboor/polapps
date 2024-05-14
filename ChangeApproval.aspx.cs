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


                if (dd_Urgency.SelectedValue == "05")
                {
                    dd_Approver.Visible = false;
                    dd_Approver2.Visible = false;
                }


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
        SqlCommand cmd = new SqlCommand(@"select ct.task_name, ct.task_description, e.employee_name 'Task_Implementer', e.email 'Implementer_Email',
            ct.task_start, ct.task_end, cs.status_description 'Task_Status'  from Change_new c inner join change_tasks ct on c.change_ID = ct.change_ID inner join employee e on ct.implementer = e.employee_ID inner join task_status cs on ct.status = cs.status_ID where c.change_id = '" + changeee + @"'");
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
        SqlCommand cmd = new SqlCommand("select change_id,service_id from Change_ServiceTrans where change_id = '" + changeee + "' order by 2");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();


        return dt;
    }

    public DataTable GetChange(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select * from Change_new c where c.change_id = '" + changeee + @"'");
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

    public DataTable GetStepAndApprover(string changeID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string checkStepsAndApprovers = "select * from CR_HistoryWorkFlowCycles where CR_No = '" + changeID + "'";
        SqlCommand cmd = new SqlCommand(checkStepsAndApprovers);
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

    public List<string> GetApproversEmails(string approver)
    {
        List<string> EmailLists = new List<string>();
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string getEmails = " select E.Email from Employee E inner join UserRoleEmployee URE on URE.loginID = E.Employee_ID inner join UserRoles UR on UR.Role_ID = URE.Role_ID where URE.Role_ID = '" + approver + "'";
        SqlCommand cmd = new SqlCommand(getEmails);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            DataTable table = dt;
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    EmailLists.Add(item.ToString());
                }
            }
        }
        return EmailLists;
    }
    public bool ExecuteUpdateStepAndApprovers(string query)
    {
        bool success = false;
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;
        con.Open();
        cmd = con.CreateCommand();
        SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
        cmd.Transaction = transaction;
        try
        {
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            transaction.Commit();
            success = true;
        }
        catch (Exception)
        {
            transaction.Rollback();

        }
        finally
        {
            con.Close();
            con.Dispose();
        }

        return success;

    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {

        if (txt_ApprovalComments.Text == "")
        {
            showAlert("Please Enter Comments!");
            return;
        }

        string ChangeID = Request.QueryString["ChangeID"];
        string status = "1";
        bool isSuccess = false;
        List<string> Emails = new List<string>();

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        //M.Rahim Added - 17.Feb.2022
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);
      


        //string fieldToUpdate = "";
        //string status = "1";
        //string stage = "3";

        DataTable checker = GetChange(ChangeID);

        if (checker.Rows.Count > 0)
        {
            //if (Convert.ToInt32(checker.Rows[0]["Status"]) > 2 && Convert.ToInt32(checker.Rows[0]["Stage_ID"]) > 2)
            //{
            //    lbl_status.Text = "Change has Already Passed this stage.";
            //    showAlert("Change has Already Passed this stage.");
            //    return;
            //}

            if (Convert.ToInt32(checker.Rows[0]["Status"]) == 1)
            {
                int currentStep = Convert.ToInt32(checker.Rows[0]["Step"]);

                int totalSteps = GetStepAndApprover(ChangeID).Rows.Count;

                string stepToUpdate = "";
                string approverToUpdate = "";
                string updateQuery = "";

                if (currentStep < totalSteps)
                {
                    string requiredStep = (currentStep + 1).ToString();
                    //DataRow requiredRow = GetStepAndApprover(ChangeID).AsEnumerable().
                    //    Where(r => r.Field<String>("Step") == requiredStep);


                    DataRow requiredRow = GetStepAndApprover(ChangeID).AsEnumerable().
                        SingleOrDefault(r => r.Field<string>("Step") == requiredStep);

                        stepToUpdate = requiredRow["Step"].ToString();
                        approverToUpdate = requiredRow["CurrentApprover"].ToString();

                        updateQuery = "update Change_new set Step = '" + stepToUpdate + "', Pending_with = '" + approverToUpdate + "' where Change_ID = '" + ChangeID + "'";
                        isSuccess = ExecuteUpdateStepAndApprovers(updateQuery);
                        Emails = GetApproversEmails(approverToUpdate);

                }
                if(currentStep == totalSteps)
                {
                    stepToUpdate = "0";
                    approverToUpdate = "00";
                    status = "3";
                    updateQuery = "update Change_new set Stage_ID = '4', Status = '" + status + "', Step = '" + stepToUpdate + "', Pending_with = '" + approverToUpdate + "' where Change_ID = '" + ChangeID + "'";
                    isSuccess = ExecuteUpdateStepAndApprovers(updateQuery);
                }

            }

            


            //if (checker.Rows[0]["Approve1_date"].ToString() == "" && checker.Rows[0]["Approve2_date"].ToString() == "")
            //{
            //    //approval remaining from both

            //    if (Session["UserID"].ToString() == checker.Rows[0]["Approver1"].ToString())
            //    {
            //        fieldToUpdate = "1";
            //    }
            //    if (Session["UserID"].ToString() == checker.Rows[0]["Approver2"].ToString())
            //    {
            //        fieldToUpdate = "2";
            //    }

            //} 
            //else if (checker.Rows[0]["Approve1_date"].ToString() == "")
            //{
            //    //approval remaining from approver1
            //    fieldToUpdate = "1";

            //    status = "3";
            //    stage = "4";
            //}
            //else if (checker.Rows[0]["Approve2_date"].ToString() == "")
            //{
            //    //approval remaining from approver2
            //    fieldToUpdate = "2";

            //    status = "3";
            //    stage = "4";
            //}
            //else
            //{ }
        }


        //string ticketQuery = "update change_new set status = '" + status + "', stage_ID = '" + stage + "', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approve" + fieldToUpdate + "_comments = '" + txt_ApprovalComments.Text + "', approve" + fieldToUpdate + "_date = getdate(), approve" + fieldToUpdate + "_DateTime = getdate() where change_ID = '" + ChangeID + "'";

        //if (dd_Urgency.SelectedValue == "05")
        //{
        //    ticketQuery = "update change_new set status = '3', stage_ID = '4', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approver1 = '" + Session["UserID"].ToString() + "',approve1_comments = '" + txt_ApprovalComments.Text + "', approve1_date = getdate(), approve1_DateTime = getdate(),    approver2 = '" + Session["UserID"].ToString() + "',approve2_comments = '" + txt_ApprovalComments.Text + "', approve2_date = getdate(), approve2_DateTime = getdate()   where change_ID = '" + ChangeID + "'";
        //}

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        DataTable tasks = GetChangeTasks(ChangeID);
        DataTable dtActivity = GetChangeLogs(ChangeID);
        DataTable emergencyEmail = GetImplementersEmail(ChangeID);
        DataTable ApproverAndRequestorEmail = GetApproverAndRequestorEmail(ChangeID);

        con.Open();
        cmd = con.CreateCommand();
        SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
        cmd.Transaction = transaction;
        try
        {

            if (dd_Urgency.SelectedValue == "05")
            {
               string ticketQuery = "update change_new set status = '3', stage_ID = '4', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approver1 = '" + Session["UserID"].ToString() + "',approve1_comments = '" + txt_ApprovalComments.Text + "', approve1_date = getdate(), approve1_DateTime = getdate(),    approver2 = '" + Session["UserID"].ToString() + "',approve2_comments = '" + txt_ApprovalComments.Text + "', approve2_date = getdate(), approve2_DateTime = getdate()   where change_ID = '" + ChangeID + "'";

                cmd.CommandText = ticketQuery;
                x = cmd.ExecuteNonQuery();
            }

           

            string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ChangeID + @"', '" + status + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Approved the Change" + @"','" + txt_ApprovalComments.Text + "')";

            cmd.CommandText = logQuery;
            y = cmd.ExecuteNonQuery();

            #region new Email

            int q = 0;

            string available = "";

            if (dd_Urgency.SelectedValue == "05" || status == "3")
            {
                available = " and is now available for your Implementation";
            }
            else
            {
                available = "";
            }

            string emailBody = @" <p>Dear <b>Implementer</b>, a Change has been Approved in CIS by <b>" + Session["Username"].ToString() + @"</b> " + available + @", the Details of the change are given below:  </p>
                                
                                
                                                                    <table>
                                
                                                                        <tr>
                                                                            <td colspan=""1"">
                                                                                <b>Change ID: </b>
                                                                            </td>
                                                                             <td colspan=""1"">
                                                                                <p>" + ChangeID + @"</p>
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
                                                                              <p>" + lbl_Change_Title.Text + @"</p>
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
                emailBody += @"<b>Change Approver: </b> <p>" + Session["Username"].ToString() + @"</p>";
            }
            else
            {
                emailBody += @"<b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
        
                            <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>";
            }

            emailBody += @"<h3>Activity</h3>";

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString()
                        + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                    emailBody += toWrite;

                }
            }

            emailBody += @"<p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

            string emailQuery = ""; string emailIDs = ""; string emailCC = ""; string emailList = "";

            if (emergencyEmail.Rows.Count > 0)
            {

                foreach (DataRow item in emergencyEmail.Rows)
                {
                    emailIDs += item["Email"] + ";";
                }

                emailIDs = emailIDs.TrimEnd(';');
            }

            //if (ApproverAndRequestorEmail.Rows.Count > 0)
            //{

            //    foreach (DataRow item in ApproverAndRequestorEmail.Rows)
            //    {
            //        emailCC += item["Email"] + ";";
            //    }

            //    emailCC = emailCC.TrimEnd(';');
            //}
            string[] code = ChangeID.Split('-');
            int changeCode = Int32.Parse(code[2]);
            string emailSubject = "For your Implementation, Change ID:"  + ChangeID ;

            if (Emails.Count > 0)
            {
                string[] emailSubAndBody = emailForApprover(changeCode);
                emailSubject = emailSubAndBody[0];
                emailBody = emailSubAndBody[1];
                foreach (var item in Emails)
                {
                    emailList = item + ";";
                }
                emailList = emailList.TrimEnd(';');
            }

            if(Emails.Count == 0)
            {
                foreach (DataRow item in emergencyEmail.Rows)
                {
                    emailList += item["Email"] + ";";
                    //if(emailList == "No Email")
                    //{
                    //    emailList = "";
                    //}
                }

                emailList = emailIDs.TrimEnd(';');
            }

            //Commenting below code - M.Rahim 17-Feb-2022
            //SMTP Email
            //Email.SendEmail(emailIDs, emailCC, "For your Implementation, Change ID: " + ChangeID, emailBody);
           


            emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, 
                Created_Date) values ('" + emailList + "', '" + emailCC + "', '" + emailSubject + "', '" + emailBody + @"', 'Y', 'CIS', 'Change', 
                '', '" + Session["UserID"].ToString() + @"', GETDATE());";

            cmd.CommandText = emailQuery;
            q = cmd.ExecuteNonQuery();



            //Added M.Rahim - 17-Feb-2022
            conHRSmart.Open();
            emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, 
                Created_Date, Module_Tran_Code) values ('" + emailList + "', '" + emailCC + "', '" + emailSubject + "', '" + emailBody + @"', 'N', 'CIS', 'Change', 
                '', '', GETDATE(), '" + changeCode + "');";
            SqlCommand command = new SqlCommand(emailQuery, conHRSmart);  
            command.ExecuteNonQuery(); 
            conHRSmart.Close();
            conHRSmart.Dispose();






            #endregion

            if (isSuccess == true)
            {
                div_success.Visible = true;
                lbl_Change_ID.Text = ChangeID;
                lbl_ChangeID.Text = ChangeID;
                div_main.Visible = false;
            }
            else
            {
                lbl_status.Text = "Change Approval Failed!! Please try again!";
                showAlert("Change Approval Failed!! Please try again!");
            }
            transaction.Commit();

        }

        catch (Exception ex)
        {
            transaction.Rollback();
            lbl_status.Text = "Change Approval Failed!! Please try again!" + ex.Message;
            showAlert("Change Approval Failed!! Please try again!");
        }

        finally
        {
            con.Close();
            con.Dispose();
            conHRSmart.Close();
            conHRSmart.Dispose();
        }
    }

    protected void btn_Reject_Click(object sender, EventArgs e)
    {

        if (txt_ApprovalComments.Text == "")
        {
            showAlert("Please Enter Comments!");
            return;
        }

        string ChangeID = Request.QueryString["ChangeID"];

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);

        //M.Rahim Added - 17.Feb.2022
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

        string fieldToUpdate = "";
        string status = "";
        string stage = "";

        DataTable checker = GetChange(ChangeID);

        if (checker.Rows.Count > 0)
        {
            if (Convert.ToInt32(checker.Rows[0]["Status"]) > 2  && Convert.ToInt32(checker.Rows[0]["Stage_ID"]) > 2)
            {
                lbl_status.Text = "Change has Already Passed this stage.";
                showAlert("Change has Already Passed this stage.");
                return;
            }
            else if (checker.Rows[0]["Approve1_date"].ToString() == "") //approval remaining from approver1
            {
                fieldToUpdate = "1";
                status = "9";
                stage = "5";
            }
            else if (checker.Rows[0]["Approve2_date"].ToString() == "")  //approval remaining from approver2
            {
                fieldToUpdate = "2";
                status = "9";
                stage = "5";
            }
            else
            { }
        }


        string ticketQuery = "update change_new set status = '" + status + "', stage_ID = '" + stage + "', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approve" + fieldToUpdate + "_comments = '" + txt_ApprovalComments.Text + "', approve" + fieldToUpdate + "_date = getdate(), approve" + fieldToUpdate + "_DateTime = getdate() where change_ID = '" + ChangeID + "'";

        if (dd_Urgency.SelectedValue == "05")
        {
            ticketQuery = "update change_new set status = '9', stage_ID = '5', modify_datetime = getdate(), modify_user = '" + Session["UserID"].ToString() + "', approver1 = '" + Session["UserID"].ToString() + "',approve1_comments = '" + txt_ApprovalComments.Text + "', approve1_date = getdate(), approve1_DateTime = getdate(),    approver2 = '" + Session["UserID"].ToString() + "',approve2_comments = '" + txt_ApprovalComments.Text + "', approve2_date = getdate(), approve2_DateTime = getdate()   where change_ID = '" + ChangeID + "'";
        }

        SqlCommand cmd = new SqlCommand();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        int x = 0;
        int y = 0;

        DataTable tasks = GetChangeTasks(ChangeID);
        DataTable dtActivity = GetChangeLogs(ChangeID);
        DataTable emergencyEmail = GetImplementersEmail(ChangeID);
        DataTable ApproverAndRequestorEmail = GetApproverAndRequestorEmail(ChangeID);

        con.Open();
        cmd = con.CreateCommand();
        SqlTransaction transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);
        cmd.Transaction = transaction;
        try
        {
            cmd.CommandText = ticketQuery;
            x = cmd.ExecuteNonQuery();

            string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                            values
                                            ('" + ChangeID + @"', '" + status + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Rejected the Change" + @"','" + txt_ApprovalComments.Text + "')";

            cmd.CommandText = logQuery;

            y = cmd.ExecuteNonQuery();

            #region new Email

            int q = 0;

            string emailBody = @" <p>Dear <b>All</b>, a Change has been Rejected in CIS by <b>" + Session["Username"].ToString() + @"</b> " 
                + @", the Details of the change are given below:  </p>
                    <table>
                        <tr>
                            <td colspan=""1"">
                                <b>Change ID: </b>
                            </td>
                                <td colspan=""1"">
                                <p>" + ChangeID + @"</p>
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
                                <p>" + lbl_Change_Title.Text + @"</p>
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
                emailBody += @"<b>Change Approver: </b> <p>" + Session["Username"].ToString() + @"</p>";
            }
            else
            {
                emailBody += @"<b>Change Approver 1: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>
                            <b>Change Approver 2: </b> <p>" + dd_Approver2.SelectedItem.Text + @"</p>";
            }

            emailBody += @"<h3>Activity</h3>";

            if (dtActivity.Rows.Count > 0)
            {
                foreach (DataRow item in dtActivity.Rows)
                {
                    string toWrite = "<p>" + item["Status_Description"].ToString() + " on " + item["Status_DateTime"].ToString()
                        + ", Comments: " + item["Status_Comments"].ToString() + "</p>";
                    emailBody += toWrite;

                }
            }

            emailBody += @"<p>To View this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                                    <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

            string emailQuery = ""; string emailIDs = ""; string emailCC = "";

            if (emergencyEmail.Rows.Count > 0)
            {

                foreach (DataRow item in emergencyEmail.Rows)
                {
                    emailCC += item["Email"] + ";";
                }

                emailCC = emailCC.TrimEnd(';');
            }

            if (ApproverAndRequestorEmail.Rows.Count > 0)
            {

                foreach (DataRow item in ApproverAndRequestorEmail.Rows)
                {
                    emailIDs += item["Email"] + ";";
                }

                emailIDs = emailIDs.TrimEnd(';');
            }

            //SMTP Email
            //Email.SendEmail(emailIDs, emailCC, "Change Rejected, Change ID: " + ChangeID, emailBody);
            emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, 
                Created_Date) values ('" + emailIDs + "', '" + emailCC + "', 'Change Rejected, Change ID: " + ChangeID + @"', '" + emailBody + @"', 'Y', 'CIS', 'Change', 
                '', '" + Session["UserID"].ToString() + @"', GETDATE());";

            cmd.CommandText = emailQuery;
            q = cmd.ExecuteNonQuery();

            #endregion

             

            //Added M.Rahim - 17-Feb-2022
            conHRSmart.Open();
            emailQuery = @"insert into Email_Logs (To_Address, CC_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode, 
                Created_Date) values ('" + emailIDs + "', '" + emailCC + "', 'Change Rejected, Change ID: " + ChangeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', 
                '', '', GETDATE());";
            SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
            command.ExecuteNonQuery();
            conHRSmart.Close();
            conHRSmart.Dispose();



            if (x > 0)
            {
                div_success.Visible = true;
                lbl_Change_ID.Text = ChangeID;
                lbl_ChangeID.Text = ChangeID;
                div_main.Visible = false;
            }
            else
            {
                lbl_status.Text = "Change Approval Failed!! Please try again!";
                showAlert("Change Approval Failed!! Please try again!");
            }
            transaction.Commit();

        }

        catch (Exception ex)
        {
            transaction.Rollback();
            lbl_status.Text = "Change Approval Failed!! Please try again!" + ex.Message;
            showAlert("Change Approval Failed!! Please try again!");
        }

        finally
        {
            con.Close();
            con.Dispose();
            conHRSmart.Close();
            conHRSmart.Dispose();
        }
    }

    public string[] emailForApprover(int changeCode)
    {
        string[] emailBody = new string[2];
        //List<string> emailDetailsList = new List<string>();
        String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString_HRSmart);
        //string query = "select Email_Subject, Email_Body from Email_Logs where From_Application = 'CIS' and Module_Tran_Code = @num order by Email_Log_ID";
        SqlCommand cmd = new SqlCommand("select Email_Subject, Email_Body from Email_Logs where From_Application = 'CIS' and Module_Tran_Code = CAST('" + changeCode + "' AS int) order by Email_Log_ID");
       // cmd.Parameters.AddWithValue("@num", changeCode);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();
        con.Dispose();
        if(dt.Rows.Count > 0)
        {
            //emailDetailsList.Add(dt.Rows[0]["Em"].)
            emailBody[0] = dt.Rows[0]["Email_Subject"].ToString();
            emailBody[1] = dt.Rows[0]["Email_Body"].ToString();
        }

        return emailBody;
    }


    public DataTable GetImplementersEmail(string change_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select ct.Implementer, e.Email from Change_tasks ct
							   inner join Employee e
							   on ct.Implementer = e.Employee_ID
							   where change_id = '" + change_id + "'";
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

    public DataTable GetApproverAndRequestorEmail(string change_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select  e.Email from Change_new ct
                                inner join Employee e 
	                                on ct.Approver1 = e.Employee_ID 
		                                or ct.Approver2 = e.Employee_ID 
		                                or ct.Requestor = e.Employee_ID
   							   where change_id = '" + change_id + "'";
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
}