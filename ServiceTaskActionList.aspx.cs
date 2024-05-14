using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            btn_search_Click(sender, e);

        }
    }



    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;

            LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");

            Response.Redirect("ServiceView.aspx?ServiceID=" + lb.Text);
        }
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {


        GetTicketsBySearch();

    }


    public void GetTicketsBySearch()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);



        string query = @"select t.service_id, 
                            t.title, 
                            c.ServiceTrans_ID,
                            c.task_Description 'ServiceName', 
                            t.requestedby 'RequestedByID',t.requestedfor,
							b.username 'requestedby', d.Dept_LongDesc 'Team',							
							case when c.Implementer = 'O' then 'Open' else e.Employee_name end as 'Implementer', e.email 'ImplementerEmail'							
							from service_new t
                            inner join HRSmart_Linde..AD_Users b on b.loginid = t.requestedby
                            inner join service_tasks c
							  on t.Service_ID = c.Service_ID
							  inner join Department d
							  on d.Dept_ID = c.AssignGroup_ID
							  left outer join Employee e
							  on e.Employee_ID = c.Implementer
							  where (c.implementer = '" + Session["UserID"].ToString() + @"' or c.implementer = 'O')
							  and c.Status in ('0','2') 
							  and c.Approve_Date is not null and c.AssignGroup_ID in (select group_id from AssignmentGrp_Emp
														                                where employee_id = '" + Session["UserID"].ToString() + @"' and status = '1')
							  order by t.service_id";


        


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
            gv_ticketList.DataSource = dt;
            gv_ticketList.DataBind();


            foreach (GridViewRow item in gv_ticketList.Rows)
            {
                string implementerr = item.Cells[6].Text;
                if (implementerr == Session["UserName"].ToString())
                {
                    DropDownList dd_Action = (DropDownList)item.FindControl("dd_Action");

                    dd_Action.Items.Clear();

                    dd_Action.Items.Add(new ListItem("Start Working", "2"));
                }
            }
        }
        else
        {
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No Pending Services found for your Approval!";
        }
    }
    protected void btn_Approve_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        GridViewRow row = (GridViewRow)btn.Parent.Parent;
        int idx = row.RowIndex;

        LinkButton lb_ticket = (LinkButton)gv_ticketList.Rows[idx].FindControl("lb_ticket");
        HiddenField hd_ServiceTransID = (HiddenField)gv_ticketList.Rows[idx].FindControl("hd_ServiceTransID");

        DropDownList dd_Action = (DropDownList)gv_ticketList.Rows[idx].FindControl("dd_Action");

        Label lbl_ApproveStatus = (Label)gv_ticketList.Rows[idx].FindControl("lbl_ApproveStatus");

        

        string serviceID = lb_ticket.Text;
        string serviceTrans_ID = hd_ServiceTransID.Value;

        string serviceName = gv_ticketList.Rows[idx].Cells[2].Text;


        if (dd_Action.SelectedValue == "0")
        {
            // just pick up, update the service task with the implemeter id

            string serviceQuery = @"update service_tasks set implementer = '" + Session["UserID"].ToString() + "' where service_ID = '" + serviceID + "' and serviceTrans_ID = '" + serviceTrans_ID + "'";

            SqlCommand cmd = new SqlCommand(serviceQuery);


            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            int x = 0;

            con.Open();
            x = cmd.ExecuteNonQuery();


            string logQuery = @"insert into service_log (service_id, status, status_by, status_datetime, status_description, status_comments)
                                                                    values
                                                                    ('" + serviceID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Picked up the Service: " + serviceName + "" + @"','Service Picked Up')";

            cmd.CommandText = logQuery;

            int y = cmd.ExecuteNonQuery();

            con.Close();

            if (x > 0)
            {
                lbl_ApproveStatus.Text = "Service Picked Up!";

                dd_Action.Enabled = false;

                btn.Enabled = false;
            }
        }

        if (dd_Action.SelectedValue == "1")
        {
            // pick up and start working, update the service task with the implemeter id and redirect the user to service action page
            string serviceQuery = @"update service_tasks set implementer = '" + Session["UserID"].ToString() + "' where service_ID = '" + serviceID + "' and serviceTrans_ID = '" + serviceTrans_ID + "'";

            SqlCommand cmd = new SqlCommand(serviceQuery);


            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;

            int x = 0;

            con.Open();
            x = cmd.ExecuteNonQuery();


            string logQuery = @"insert into service_log (service_id, status, status_by, status_datetime, status_description, status_comments)
                                                                    values
                                                                    ('" + serviceID + @"', '1', '" + Session["UserID"].ToString() + @"', getdate(), '" + Session["Username"].ToString() + " Picked up the Service: " + serviceName + "" + @"','Service Picked Up')";

            cmd.CommandText = logQuery;

            int y = cmd.ExecuteNonQuery();

            con.Close();

            if (x > 0)
            {
                lbl_ApproveStatus.Text = "Service Picked Up!";

                dd_Action.Enabled = false;

                btn.Enabled = false;

                //now open the service action in new tab


                string url = "serviceaction.aspx?ServiceID=" + serviceID + "&ServiceTransID=" + serviceTrans_ID;
                string script = string.Format("window.open('{0}');", url);

                Page.ClientScript.RegisterStartupScript(this.GetType(),
                    "newPage" + UniqueID, script, true);


                //--------------------------------------
            }
        }

        if (dd_Action.SelectedValue == "2")
        {
            // check if the service task belongs to the user, if yes then redirect the user to service action page otherwise show error

            string url = "serviceaction.aspx?ServiceID=" + serviceID + "&ServiceTransID=" + serviceTrans_ID;
            string script = string.Format("window.open('{0}');", url);

            Page.ClientScript.RegisterStartupScript(this.GetType(),
                "newPage" + UniqueID, script, true);
        }



    }
}