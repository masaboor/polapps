using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
            GetTaskStatus();


            string changeID = Request.QueryString["ChangeID"];
            string Case = Request.QueryString["Case"];

            DataTable dt = new DataTable();

            dt = GetChangeView(changeID, Case);


            txt_dateto.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (dt.Rows.Count > 0)
            {

                DataTable tattachments = new DataTable();
                tattachments.Columns.Add("Task_ID", typeof(string));
                tattachments.Columns.Add("AttachmentID", typeof(string));
                tattachments.Columns.Add("AttachmentName", typeof(string));
                tattachments.Columns.Add("Dataa", typeof(Byte[]));

                tattachments.AcceptChanges();

                Session["tattachments"] = tattachments;


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

                lbl_TaskID.Text = Request.QueryString["TaskID"];


                DataTable attachhessss = new DataTable();

                attachhessss = GetChangeAttachments(changeID);

                if (attachhessss.Rows.Count > 0)
                {
                    gv_AttachmentList.DataSource = attachhessss;
                    gv_AttachmentList.DataBind();
                }


                DataTable tatachhessss = new DataTable();

                tatachhessss = GetChangeTaskAttachments(changeID, lbl_TaskID.Text);

                if (tatachhessss.Rows.Count > 0)
                {

                    foreach (DataRow item in tatachhessss.Rows)
                    {
                        DataRow drr = tattachments.NewRow();
                        drr["Task_ID"] = item["Task_ID"].ToString();
                        drr["AttachmentID"] = item["Attachment_ID"].ToString();
                        drr["AttachmentName"] = item["filename"].ToString();
                        drr["Dataa"] = item["Attachment"] as Byte[];

                        tattachments.Rows.Add(drr);
                        tattachments.AcceptChanges();
                    }

                    gv_TaskAttachment.DataSource = tattachments;
                    gv_TaskAttachment.DataBind();

                    Session["tattachments"] = tattachments;
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

    public void GetTaskStatus()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select Status_id, Status_description from Task_Status  where status_id in (1,2)   order by Status_id desc");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Result.DataTextField = "Status_description";
            dd_Result.DataValueField = "Status_id";
            dd_Result.DataSource = dt;
            dd_Result.DataBind();
            //dd_Result.Items.Insert(0, new ListItem("Please Select", "0"));
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

    public DataTable GetChangeView(string changeee, string Case)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string query = "";
        if (!String.IsNullOrEmpty(Case))
        {
            query = " select c.Approver1, c.Approver2, c.* from Change_new c inner join employee e1 on e1.employee_ID = c.Requestor  where c.change_id = '" + changeee + "'";
        }
        else
        {
           query  = "select  e1.employee_name 'Approver1', e2.employee_name 'Approver2',c.* from Change_new c inner join employee e1 on e1.employee_ID = c.approver1 inner join employee e2 on e2.employee_ID = c.approver2 where c.change_id = '" + changeee + "'";
        }

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

    public DataTable GetChangeTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select ct.task_name, ct.task_description, e.employee_name 'Task_Implementer', e.email 'Implementer_Email', 
                ct.task_start, ct.task_end, cs.status_description 'Task_Status'  from Change_new c inner join change_tasks ct on c.change_ID = ct.change_ID 
                inner join employee e on ct.implementer = e.employee_ID inner join task_status cs on ct.status = cs.status_ID where c.change_id = '" + changeee + @"'");
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

    public DataTable GetRemainingTasks(string changeee)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        
        SqlConnection conn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select * from  change_tasks ct where ct.change_id = '" + changeee + @"' and ct.status = '0'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = conn;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        conn.Open();
        sda.Fill(dt);
        conn.Close();


        if (dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return dt;
        }
    }

    // Added Hassam 26th-Aug-22
    public void ExecuteUpdateCommand_ChangeTaskTable(string changeID, string TaskID, string updateClosingDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection conn = new SqlConnection(constr);
        conn.Open();
        SqlCommand cmd = new SqlCommand(@"update change_tasks set status = '1', Task_end = CONVERT(datetime, '" + updateClosingDate + @"', 103) , implement_Date = getdate(), implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "', task_result = '" + dd_Result.SelectedValue + "' where change_ID = '" + changeID + "' and task_ID = '" + TaskID + "'", conn);
        cmd.ExecuteNonQuery();
        conn.Close();
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {

        if (txt_ImplementationComments.Text == "")
        {
            showAlert("Please Enter Implementation Comments!");
            return;
        }

        if (txt_dateto.Text == "")
        {
            showAlert("Please Enter Closing Date!");
            return;
        }

        string ChangeID = Request.QueryString["ChangeID"];

        string taskID = Request.QueryString["TaskID"];


        string stage = "4";
        string status = "3";


        DataTable tattachments = new DataTable();
        tattachments = Session["tattachments"] as DataTable;

        List<string> querries = new List<string>();

        String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        SqlConnection con = new SqlConnection(strConnString);
        SqlCommand cmd = new SqlCommand();        
        cmd.CommandType = CommandType.Text;
        cmd.Connection = con;

        con.Open();
        cmd = con.CreateCommand();
        SqlTransaction transaction = con.BeginTransaction("Transaction1");
        cmd.Transaction = transaction;

        try    
        {    
            #region IfResult2
            if (dd_Result.SelectedValue == "2")
            {
                int x = 0;
             
                string ticketQuery = "update change_tasks set status = '" + dd_Result.SelectedValue + "', implement_Date = getdate(), implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "', task_result = '" + dd_Result.SelectedValue + "' where change_ID = '" + ChangeID + "' and task_ID = '" + taskID + "'";
                querries.Add(ticketQuery);//cmd.CommandText = ticketQuery; x = cmd.ExecuteNonQuery();

                string delattachmentquery = "delete from Change_task_attachments where Change_ID = '" + ChangeID + "' and task_ID = '" + taskID + "'";
                querries.Add(delattachmentquery);//cmd.CommandText = delattachmentquery;x = cmd.ExecuteNonQuery();

                foreach (DataRow item in tattachments.Rows)
                {
                    string TaskQuery = @"INSERT INTO Change_task_attachments
                                                               (Change_ID,Task_ID,
                                                               attachment_ID
                                                               ,attachment
                                                               ,filename)
                                                         VALUES ('" + ChangeID + "','"+taskID+"','" + item["attachmentID"].ToString() + "',@Attachment,@Filename)";

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

                int y = 0;
                string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                                values
                                                ('" + ChangeID + @"', '" + status + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Updated the Task" + @"','" + txt_ImplementationComments.Text + "')";
                querries.Add(logQuery);//cmd.CommandText = logQuery;y = cmd.ExecuteNonQuery();

                div_success.Visible = true;
                lbl_Change_ID.Text = ChangeID;
                lbl_ChangeID.Text = ChangeID;
                div_main.Visible = false;
                
            }
            #endregion

            #region elseResultNot2 
            else
            {
                string closingDate = txt_closingdate.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString())
           + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

                int x = 0; int y = 0; int z = 0;
                // Added Hassam 26th-Aug-22
                ExecuteUpdateCommand_ChangeTaskTable(ChangeID, taskID, closingDate);


                //string ticketQuery = "update change_tasks set status = '1', implement_Date = getdate(), implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "', task_result = '" + dd_Result.SelectedValue + "' where change_ID = '" + ChangeID + "' and task_ID = '" + taskID + "'";


                #region fileAttachment
                if (fp_attachment.HasFile)
                {

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


                         string ticketQuery = "update change_tasks set status = '1', implement_Date = getdate(), implement_datetime = getdate(), implement_comments = '" + txt_ImplementationComments.Text + "', Attachment = @Attachment, Filename = @filename, task_result = '" + dd_Result.SelectedValue + "' where change_ID = '" + ChangeID + "' and task_ID = '" + taskID + "'";

                        cmd = new SqlCommand(ticketQuery);

                        cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = bytes;
                        cmd.Parameters.Add("@filename", SqlDbType.VarChar).Value = filename;

                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;

                        //con.Open();


                        x = cmd.ExecuteNonQuery();

                        string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                                values
                                                ('" + ChangeID + @"', '" + status + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Implemented the Task" + @"','" + txt_ImplementationComments.Text + "')";

                        querries.Add(logQuery);//cmd.CommandText = logQuery;y = cmd.ExecuteNonQuery();

                        DataTable remaining = GetRemainingTasks(ChangeID);

                        bool ToUpdateChange = false;

                        if (remaining.Rows.Count > 0)
                        {
                            //tasks are remaining
                        }
                        else
                        {
                            stage = "5";
                            status = "4";
                            ToUpdateChange = true;
                        }


                        if (ToUpdateChange == true)
                        {
                            string changeupdateQuery = "update change_new set stage_ID = '" + stage + "', status = '" + status + "' where change_id = '" + ChangeID + @"'";
                            querries.Add(changeupdateQuery); //cmd.CommandText = changeupdateQuery;z = cmd.ExecuteNonQuery();
                        }
                    }
                }
                #endregion
                #region noFileAttachment
                else
                {
                    //querries.Add(ticketQuery);//cmd.CommandText = ticketQuery;x = cmd.ExecuteNonQuery();

                    string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description, status_comments)
                                                values
                                                ('" + ChangeID + @"', '" + status + "', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Implemented the Change" + @"','" + txt_ImplementationComments.Text + "')";
                    querries.Add(logQuery);//cmd.CommandText = logQuery;y = cmd.ExecuteNonQuery();

                    DataTable remaining = GetRemainingTasks(ChangeID);

                    // bool ToUpdateChange = false;

                    // Added Hassam 26th-Aug-22
                    if (remaining.Rows.Count > 0)
                    {}
                    else
                    {
                        stage = "5";
                        status = "4";
                        string changeupdateQuery = "update change_new set stage_ID = '" + stage + "', status = '" + status + "' where change_id = '" + ChangeID + @"'";
                        querries.Add(changeupdateQuery);
                    }

                    //if (remaining.Rows.Count > 0) //tasks are remaining 
                    //{ }
                    //else
                    //{
                    //    stage = "5";
                    //    status = "4";
                    //    ToUpdateChange = true;
                    //}

                    //if (ToUpdateChange == true)
                    //{
                    //    string changeupdateQuery = "update change_new set stage_ID = '" + stage + "', status = '" + status + "' where change_id = '" + ChangeID + @"'";
                    //    querries.Add(changeupdateQuery); //cmd.CommandText = changeupdateQuery;z = cmd.ExecuteNonQuery();
                    //}
                }
                #endregion

               
                div_success.Visible = true;
                lbl_Change_ID.Text = ChangeID;
                lbl_ChangeID.Text = ChangeID;
                div_main.Visible = false;
            }
            #endregion

            #region new Email
            int q = 0;
            string emailBody = @" <p>Dear <b>All</b>, a Task has been marked as " + dd_Result.SelectedItem.Text + @" in CIS by <b>" + Session["Username"].ToString() 
                + @"</b>, the Details of the change are given below:  </p>
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
                <td><b>Task Status</b>
                </td>
                </tr>";

            DataTable tasks = new DataTable();
            tasks = GetChangeTasks(ChangeID);
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
                                            <td>
                                        <p>" + item["Task_Status"].ToString() + @"
                                        </p>
                                    </td>
                                </tr>";
            }

            emailBody += "</table>";


            emailBody += @"<b>Change Approver: </b> <p>" + dd_Approver.SelectedItem.Text + @"</p>";


            emailBody += @"<h3>Activity</h3>";

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

            emailBody += @"<p>To view this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
                      <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";

            string emailQuery = ""; string emailIDs = "";
            DataTable emergencyEmail = GetImplementersEmail(ChangeID);
            
            if (emergencyEmail.Rows.Count > 0)
            {
                foreach (DataRow item in emergencyEmail.Rows)
                {
                    emailIDs += item["Email"] + ";";
                }

                emailIDs = emailIDs.TrimEnd(';');
            }
            //SMTP Email
            
            //Email.SendEmail(emailIDs, "", "Change Implemented, Change ID: " + ChangeID, emailBody);
            
            emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User,
                        Created_Date) values ('" + emailIDs + "', 'Change Implemented, Change ID: " + ChangeID + @"', '" + emailBody + @"', 'Y', 'CIS', 
                        'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());";
            querries.Add(emailQuery); // cmd.CommandText = emailQuery; q = cmd.ExecuteNonQuery();


            #endregion
            //con.Open();
            foreach (string cmx in querries)
            {
                cmd.CommandText = cmx;
                cmd.ExecuteNonQuery();
            }



            //M.Rahim Added - 17.Feb.2022
            String strConnString_HRSmart = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
            SqlConnection conHRSmart = new SqlConnection(strConnString_HRSmart);

            //Added M.Rahim - 28-Feb-2022
            conHRSmart.Open();
            emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_UserCode,
                        Created_Date) values ('" + emailIDs + "', 'Change Implemented, Change ID: " + ChangeID + @"', '" + emailBody + @"', 'N', 'CIS', 
                        'Change', '', '', GETDATE());";

            SqlCommand command = new SqlCommand(emailQuery, conHRSmart);
            command.ExecuteNonQuery();
            conHRSmart.Close();
            conHRSmart.Dispose();




            transaction.Commit();
        }

        catch (Exception ex)
        {
            transaction.Rollback();
            lbl_status.Text = "Change Implementation Failed!! Please try again!" + ex.Message;
            showAlert("Change Implementation Failed!! Please try again!");
        }

        finally
        {
            con.Close();
            con.Dispose();
        }

    }

    public DataTable GetImplementersEmail(string change_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select c.Approver1  'Person', e.Email from Change_new c inner join Employee e
							   on c.Approver1 = e.Employee_ID
							   where change_id = '" + change_id + @"'
							   union all
    						    select c.Approver2  'Person', e.Email from Change_new c inner join Employee e
							   on c.Approver2 = e.Employee_ID
							   where change_id = '" + change_id + @"'
							   union all
							   select ct.Implementer 'Person', e.Email from Change_tasks ct
							   inner join Employee e
							   on ct.Implementer = e.Employee_ID
							   where change_id = '" + change_id + @"'
                               union all
							   select c.Requestor 'Person', e.Email from Change_new c inner join Employee e
							   on c.Requestor = e.Employee_ID
							   where change_id = '" + change_id + @"'";

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

    protected void btn_AddTaskAttachment_Click(object sender, EventArgs e)
    {
        if (fp_attachment.HasFile)
        {

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            DataRow row = tattachments.NewRow();
            row["Task_ID"] = lbl_TaskID.Text;
            row["AttachmentID"] = (tattachments.Rows.Count + 1).ToString();


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


                tattachments.Rows.Add(row);
                tattachments.AcceptChanges();

                gv_TaskAttachment.DataSource = tattachments;
                gv_TaskAttachment.DataBind();

                Session["tattachments"] = tattachments;


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

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            tattachments.Rows.RemoveAt(RowIndex);

            tattachments.AcceptChanges();

            Session["tattachments"] = tattachments;

            gv_TaskAttachment.DataSource = tattachments;
            gv_TaskAttachment.DataBind();




        }

        if (e.CommandName == "down")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;


            string attachmentId = gvr.Cells[0].Text;

            DataTable tattachments = new DataTable();

            tattachments = Session["tattachments"] as DataTable;

            byte[] bytess;
            string fileName, contentType;

            bytess = tattachments.Rows[RowIndex]["dataa"] as byte[];

            contentType = Path.GetExtension(tattachments.Rows[RowIndex]["AttachmentName"].ToString());
            fileName = tattachments.Rows[RowIndex]["AttachmentName"].ToString();


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

    public DataTable GetChangeTaskAttachments(string changeee, string taskID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = "select * from change_task_attachments where change_id = '" + changeee + "' and task_id = '" + taskID + "'";

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
}