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
            dd_ChangeRequestor.SelectedValue = Session["UserID"].ToString();

            foreach (ListItem item in dd_ChangeRequestor.Items)
            {
                if (item.Value.Contains(Session["UserID"].ToString()))
                {
                    item.Selected = true;
                }

            }

            dd_ChangeRequestor.Enabled = false;
            GetOwner();
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
            GetApprover();

            foreach (ListItem item in dd_Approver.Items)
            {
                if (item.Value.Contains("PK002C"))
                {
                    item.Selected = true;
                }

            }

            //dd_Approver.SelectedValue = "PK002C";


            GetDelegatedApprover();
            dd_Approver.Enabled = false;


            //check for delegation here

            //GetImplementer();

            txt_datefrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_dateto.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

    public void GetReviewer(string type_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID + '#' + e.email 'Employee_ID',e.Employee_name from Type_AssignmentGrp ta
                                            inner join Employee e
                                            on ta.dept_id = e.Dept_ID
                                            where ta.type_id = '" + type_id + @"'
                                            and Employee_ID <> 'PK002C' order by Employee_name");
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

    public void GetDelegatedApprover()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select temp_employee_id from change_delegation where convert(date,GETDATE(),103) between convert(date,start_date,103) and convert(date,end_date,103)");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Approver.SelectedValue = dt.Rows[0][0].ToString();
        }
        else
        {
            //lbl_status.Text = "No DATA!";
        }
    }

    public void GetImplementer(string type_id)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select e.Dept_id,e.Employee_ID + '#' + e.email 'Employee_ID',e.Employee_name from Type_AssignmentGrp ta
                                            inner join Employee e
                                            on ta.dept_id = e.Dept_ID
                                            where ta.type_id = '" + type_id + @"'
                                            and Employee_ID <> 'PK002C' order by Employee_name");
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

        GetReviewer(tickettype);
        GetImplementer(tickettype);
    }

    public string GetMaxChange()
    {

        string maxticket = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        //SqlCommand cmd = new SqlCommand("select isnull(MAX(Ticket_ID),0) from ticket");
        SqlCommand cmd = new SqlCommand("select isnull(MAX(CAST(Change_ID as int)),0) from Change");
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
    protected void btn_Save_Click(object sender, EventArgs e)
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
            if (dd_ChangeOwner.SelectedValue == "0")
            {
                showAlert("Please Select Owner!");
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
                showAlert("Please Enter Details!");
                return;
            }

            if (dd_Reviewer.SelectedValue == "0")
            {
                showAlert("Please Select Reviewer!");
                return;
            }

            if (dd_Implementer.SelectedValue == "0")
            {
                showAlert("Please Select Implementer!");
                return;
            }

            #endregion


            if (dd_ChangeOwner.SelectedValue == dd_Reviewer.SelectedValue)
            {
                showAlert("Owner and Reviewer cannot be same!");
                return;
            }


            string maxchange = GetMaxChange();

            double newmaxchange = double.Parse(maxchange);

            newmaxchange = newmaxchange + 1;

            string changeID = newmaxchange.ToString();

            //DateTime time1 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector3.Hour, TimeSelector3.Minute, TimeSelector3.Second, TimeSelector3.AmPm));

            //DateTime time2 = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));


            //DateTime time1 = DateTime.Parse(string.Format("{0} {1}:{2}:{3} {4}", txt_datefrom.Text, TimeSelector3.Hour, TimeSelector3.Minute, TimeSelector3.Second, TimeSelector3.AmPm));
            //string datetime1 = time1.Date.ToString("dd/MM/yyyy") + " " + time1.ToLongTimeString();

            string datetime1 = txt_datefrom.Text + " " + addZero(TimeSelector3.Hour.ToString()) + ":" + addZero(TimeSelector3.Minute.ToString()) + ":" + addZero(TimeSelector3.Second.ToString()) + ":000 " + TimeSelector3.AmPm;

            //DateTime time2 = DateTime.Parse(string.Format("{0} {1}:{2}:{3} {4}", txt_dateto.Text, TimeSelector4.Hour, TimeSelector4.Minute, TimeSelector4.Second, TimeSelector4.AmPm));
            //string datetime2 = time2.Date.ToString("dd/MM/yyyy") + " " + time2.ToLongTimeString();
            string datetime2 = txt_datefrom.Text + " " + addZero(TimeSelector4.Hour.ToString()) + ":" + addZero(TimeSelector4.Minute.ToString()) + ":" + addZero(TimeSelector4.Second.ToString()) + ":000 " + TimeSelector4.AmPm;

            //Response.Write(datetime1);
            //lbl_status.Text = datetime1;
            ////temporarily
            //return;

            string[] reviewerEmail = dd_Reviewer.SelectedValue.Split('#');
            string[] approverEmail = dd_Approver.SelectedValue.Split('#');
            string[] implementerEmail = dd_Implementer.SelectedValue.Split('#');

            string[] requestorEmail = dd_ChangeRequestor.SelectedValue.Split('#');

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

                    //insert the file into database


                    string changeQuery = @"INSERT INTO Change
                                                       (Change_ID
                                                       ,Title
                                                       ,Requestor
                                                       ,Owner
                                                       ,Submit_Date
                                                       ,Submit_DateTime
                                                       ,Reviewer
                                                       ,Approver
                                                       ,Implementer
                                                       ,Impact_ID
                                                       ,Urgency_ID
                                                       ,Priority_ID
                                                       ,Stage_ID
                                                       ,Risk_ID
                                                       ,Type_ID
                                                       ,SubType_ID
                                                       ,Start_Date
                                                       ,End_Date
                                                       
                                                       ,Reason_ID
                                                       ,Details
                                                       ,Attachment
                                                       ,filename
                                                       ,Modify_DateTime
                                                       ,Modify_User
                                                       ,Status, ref_incident_no)
                                                 VALUES
                                                       ('" + changeID + @"'
                                                       ,'" + txt_Title.Text + @"'
                                                       ,'" + Session["UserID"].ToString() + @"'
                                                       ,'" + dd_ChangeOwner.SelectedValue + @"'
                                                       ,getdate()
                                                       ,getdate()
                                                       ,'" + reviewerEmail[0] + @"'
                                                       ,'" + approverEmail[0] + @"'
                                                       ,'" + implementerEmail[0] + @"'
                                                       ,'" + dd_Impact.SelectedValue + @"'
                                                       ,'" + dd_Urgency.SelectedValue + @"'
                                                       ,'" + dd_Priority.SelectedValue + @"'
                                                       ,'2'
                                                       ,'" + dd_Risk.SelectedValue + @"'
                                                       ,'" + dd_Category.SelectedValue + @"'
                                                       ,'" + dd_SubCategory.SelectedValue + @"'
                                                       ,CONVERT(datetime, '" + datetime1 + @"', 103)  
                                                       ,CONVERT(datetime, '" + datetime2 + @"', 103)
                                                       ,'" + dd_ReasonForChange.SelectedValue + @"'
                                                       ,'" + txt_Details.Text.Replace("'", "") + @"'
                                                       ,@Attachment
                                                       ,@filename
                                                       ,getdate()
                                                       ,'" + Session["UserID"].ToString() + @"'
                                                       ,'1','" + txt_refIncident.Text + "')";

                    SqlCommand cmd = new SqlCommand(changeQuery);

                    cmd.Parameters.Add("@Attachment", SqlDbType.Binary).Value = bytes;
                    cmd.Parameters.Add("@filename", SqlDbType.VarChar).Value = filename;

                    String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                    SqlConnection con = new SqlConnection(strConnString);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    int x = 0;
                    int y = 0;
                    int p = 0;

                    int q = 0;

                    try
                    {
                        con.Open();
                        x = cmd.ExecuteNonQuery();


                        foreach (ListItem item in dd_ServiceAffected.Items)
                        {
                            if (item.Selected)
                            {
                                string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                                cmd.CommandText = serviceQuery;

                                p = cmd.ExecuteNonQuery();
                            }

                        }


                        string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
                                                                    values
                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"')";

                        cmd.CommandText = logQuery;

                        y = cmd.ExecuteNonQuery();

                        string emailBody = @"<p>Dear <b>" + dd_Reviewer.SelectedItem.Text + @"</b>, a Change has been initiated in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your review, the Details of the change are given below:  </p>
        
        
                                            <table>
        
                                                <tr>
                                                    <td colspan=""1"">
                                                        <b>Change ID: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + changeID + @"</p>
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
        
                                            <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
        
                                            <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
        
                                            <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";


                       

                        string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + reviewerEmail[1] + "', 'For your Reviewal, Change ID: " + changeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());
                                                ";


                        cmd.CommandText = emailQuery;

                        q = cmd.ExecuteNonQuery();

                    }

                    catch (Exception ex)
                    {
                        lbl_status.Text = ex.Message;
                        showAlert(ex.Message);
                    }

                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }

                    if (x > 0)
                    {

                        div_success.Visible = true;
                        lbl_ChangeID.Text = changeID;
                        div_main.Visible = false;
                        //showAlert("Ticket Created Successfully!");

                        //Response.Redirect("Dashboard.aspx");
                    }
                    else
                    {
                        lbl_status.Text = "Change Creation Failed!! Please try again!";
                        showAlert("Change Creation Failed!! Please try again!");
                    }

                    //lblMessage.ForeColor = System.Drawing.Color.Green;

                    //lblMessage.Text = "File Uploaded Successfully";

                }
                else
                {
                    lbl_status.Text = "File format not recognised." + " Upload Image/Word/PDF/Excel formats";
                    showAlert("File format not recognised." + " Upload Image/Word/PDF/Excel formats");
                }
            }
            else
            {



                string changeQuery = @"INSERT INTO Change
                                                       (Change_ID
                                                       ,Title
                                                       ,Requestor
                                                       ,Owner
                                                       ,Submit_Date
                                                       ,Submit_DateTime
                                                       ,Reviewer
                                                       ,Approver
                                                       ,Implementer
                                                       ,Impact_ID
                                                       ,Urgency_ID
                                                       ,Priority_ID
                                                       ,Stage_ID
                                                       ,Risk_ID
                                                       ,Type_ID
                                                       ,SubType_ID
                                                       ,Start_Date
                                                       ,End_Date
                                                       
                                                       ,Reason_ID
                                                       ,Details
                                                       ,Modify_DateTime
                                                       ,Modify_User
                                                       ,Status, ref_incident_no)
                                                 VALUES
                                                       ('" + changeID + @"'
                                                       ,'" + txt_Title.Text + @"'
                                                       ,'" + Session["UserID"].ToString() + @"'
                                                       ,'" + dd_ChangeOwner.SelectedValue + @"'
                                                       ,getdate()
                                                       ,getdate()
                                                       ,'" + reviewerEmail[0] + @"'
                                                       ,'" + approverEmail[0] + @"'
                                                       ,'" + implementerEmail[0] + @"'
                                                       ,'" + dd_Impact.SelectedValue + @"'
                                                       ,'" + dd_Urgency.SelectedValue + @"'
                                                       ,'" + dd_Priority.SelectedValue + @"'
                                                       ,'2'
                                                       ,'" + dd_Risk.SelectedValue + @"'
                                                       ,'" + dd_Category.SelectedValue + @"'
                                                       ,'" + dd_SubCategory.SelectedValue + @"'
                                                       ,CONVERT(datetime, '" + datetime1 + @"', 103)  
                                                       ,CONVERT(datetime, '" + datetime2 + @"', 103)
                                                       ,'" + dd_ReasonForChange.SelectedValue + @"'
                                                       ,'" + txt_Details.Text.Replace("'", "") + @"'
                                                       ,getdate()
                                                       ,'" + Session["UserID"].ToString() + @"'
                                                       ,'1','" + txt_refIncident.Text + "')";

                SqlCommand cmd = new SqlCommand(changeQuery);


                String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection con = new SqlConnection(strConnString);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                int x = 0;
                int y = 0;
                int p = 0;


                int q = 0;

                try
                {
                    con.Open();
                    x = cmd.ExecuteNonQuery();


                    foreach (ListItem item in dd_ServiceAffected.Items)
                    {
                        if (item.Selected)
                        {
                            string serviceQuery = "insert into Change_ServiceTrans (Change_ID, Service_ID) values ('" + changeID + "','" + item.Value + "')";
                            cmd.CommandText = serviceQuery;

                            p = cmd.ExecuteNonQuery();
                        }

                    }



                    string logQuery = @"insert into change_log (change_id, status, status_by, status_datetime, status_description)
                                                                    values
                                                                    ('" + changeID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Initiated the Change" + @"')";

                    cmd.CommandText = logQuery;

                    y = cmd.ExecuteNonQuery();


                    string emailBody = @"<p>Dear <b>" + dd_Reviewer.SelectedItem.Text + @"</b>, a Change has been initiated in CIS by <b>" + Session["Username"].ToString() + @"</b> and is now available for your review, the Details of the change are given below:  </p>
        
        
                                            <table>
        
                                                <tr>
                                                    <td colspan=""1"">
                                                        <b>Change ID: </b>
                                                    </td>
                                                     <td colspan=""1"">
                                                        <p> " + changeID + @"</p>
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
        
                                            <p>" + Session["Username"].ToString() + @" Initiated the Change</p>
        
                                            <p>To perform action on this change, please click on this link:   <a href=""http://10.85.1.249/CIS/Login.aspx""> Redirect me to CIS</a> </p>
        
                                            <b>Note: </b><p>This is a system generated notification, Please do not reply.</p>";


                    string emailQuery = @"insert into Email_Logs (To_Address, Email_Subject, Email_Body, Sent_Flag, From_Application, From_Module, Confirmation_Msg, From_User, Created_Date)
                                                values ('" + reviewerEmail[1] + "', 'For your Reviewal, Change ID: " + changeID + @"', '" + emailBody + @"', 'N', 'CIS', 'Change', '', '" + Session["UserID"].ToString() + @"', GETDATE());
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
                    lbl_ChangeID.Text = changeID;
                    div_main.Visible = false;
                    //showAlert("Ticket Created Successfully!");

                    //Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    lbl_status.Text = "Change Creation Failed!! Please try again!";
                    showAlert("Change Creation Failed!! Please try again!");
                }
            }



        }
        catch (Exception oi)
        {
            lbl_status.Text = oi.Message.ToString();
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
        if (dd_ChangeOwner.SelectedValue == "0")
        {
            showAlert("Please Select Owner!");
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

        if (dd_Reviewer.SelectedValue == "0")
        {
            showAlert("Please Select Reviewer!");
            return;
        }

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
}