using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetUnresolvedTickets();
            GetPendingChangeActions_new();
            GetPendingServiceActions();
            GetPendingChangeActions_Emergency();
            BindChart1();
            BindChart2();
        }
    }


    public void GetUnresolvedTickets()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"
                                select count(*) UnresolvedTicketCount from ticket where
                                 Assigned_Employee_ID = '" + Session["UserID"].ToString() + @"' and
                                  Status  in ('1','2','4','5','6')";

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
            lbl_ticketCount.Text = dt.Rows[0]["UnresolvedTicketCount"].ToString();
        }
        else
        {
            lbl_ticketCount.Text = "0";
        }
    }

    public void GetPendingChangeActions()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) 'ChangeActionCount' from change where ((Reviewer = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Approver = '" + Session["UserID"].ToString() + @"' and status = '2')
                                            or (Implementer = '" + Session["UserID"].ToString() + @"' and status = '3'))";



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
            lbl_ActionsCount.Text = dt.Rows[0]["ChangeActionCount"].ToString();
        }
        else
        {
            lbl_ActionsCount.Text = "0";
        }
    }

    public void GetPendingChangeActions_new()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) 'ChangeActionCount' from change_new where ((Approver1 = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Approver2 = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Implementer = '" + Session["UserID"].ToString() + @"' and status = '3'))";


        query = @"select sum(a.ChangeActionCount) 'ChangeActionCount' from
                        (select count(*) 'ChangeActionCount' from change_new
                         where urgency_ID <> '05' and ((Approver1 = '" + Session["UserID"].ToString() + @"' and approve1_date is null and status = '1')
                        or (Approver2 = '" + Session["UserID"].ToString() + @"' and approve2_date is null and status = '1'))

                        union all

                        select COUNT(*) 'ChangeActionCount' from Change_tasks ct
                         inner join Change_new c
						                        on c.Change_ID = ct.Change_ID
						                        and c.Status = '3'
                         where ct.status <> '1' and ct.Implementer = '" + Session["UserID"].ToString() + @"') a";




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
            lbl_ActionsCount.Text = dt.Rows[0]["ChangeActionCount"].ToString();
        }
        else
        {
            lbl_ActionsCount.Text = "0";
        }
    }

    public void GetPendingChangeActions_Emergency()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) 'ChangeActionCount' from change_new where ((Approver1 = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Approver2 = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Implementer = '" + Session["UserID"].ToString() + @"' and status = '3'))";


        query = @"select sum(a.ChangeActionCount) 'ChangeActionCount' from
                        (select count(*) 'ChangeActionCount' from change_new
                         where urgency_ID = '05' and status = '1') a";




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
            lbl_EmerActionsCount.Text = dt.Rows[0]["ChangeActionCount"].ToString();
        }
        else
        {
            lbl_EmerActionsCount.Text = "0";
        }
    }

    public void GetPendingServiceActions()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) 'ServiceActionCount' from service where (
                                             (Approver = '" + Session["UserID"].ToString() + @"' and status = '1')
                                            or (Implementer = '" + Session["UserID"].ToString() + @"' and status = '2'))";


        query = @"select sum(a.ApprovalCount) TotalActionCount from
                        (select count(*) ApprovalCount					
							                        from service_new t
                          
                                                    inner join service_tasks c
							                          on t.Service_ID = c.Service_ID
							 
							                          where c.Approver = '" + Session["UserID"].ToString() + @"'
							                          and c.Status = '0'
							                          and c.Approve_Date is null

							                          union all


							                        select count(*) ApprovalCount						
							                        from service_new t
                            
                                                    inner join service_tasks c
							                          on t.Service_ID = c.Service_ID
							 
							                          where (c.implementer = '" + Session["UserID"].ToString() + @"' or c.implementer = 'O')
							                          and c.Status in ('0','2') 
							                          and c.Approve_Date is not null and c.AssignGroup_ID in (select group_id from AssignmentGrp_Emp
														                                                        where employee_id = '" + Session["UserID"].ToString() + @"' and status = '1')) a";



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
            lbl_ServiceCount.Text = dt.Rows[0]["TotalActionCount"].ToString();
        }
        else
        {
            lbl_ServiceCount.Text = "0";
        }
    }


    private void BindChart1()
    {
        DataTable dsChartData = new DataTable();

        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) TicketCount,b.Status_Desc 'Status' from Ticket a
                            inner join Ticket_Status b
                            on a.Status = b.Status
                            group by b.Status_Desc";

        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        con.Open();
        sda.Fill(dsChartData);
        con.Close();

        StringBuilder strScript = new StringBuilder();

        try
        {
            //dsChartData = GetChartData();

            strScript.Append(@"<script type='text/javascript'>  
                    google.load('visualization', '1', {packages: ['corechart']}); </script>  
                      
                    <script type='text/javascript'>  
                     
                    function drawChart() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['Task', 'Hours of Day'],");

            foreach (DataRow row in dsChartData.Rows)
            {
                strScript.Append("['" + row["Status"] + "'," + row["TicketCount"] + "],");
            }
            strScript.Remove(strScript.Length - 1, 1);
            strScript.Append("]);");

            strScript.Append(@" var options = {     
                                    title: 'Total Ticket Count By Status',            
                                    is3D: true,          
                                    };   ");

            strScript.Append(@"var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));          
                                chart.draw(data, options);        
                                }    
                            google.setOnLoadCallback(drawChart);  
                            ");
            strScript.Append(" </script>");

            ltScripts.Text = strScript.ToString();
        }
        catch
        {
        }
        finally
        {
            dsChartData.Dispose();
            strScript.Clear();
        }
    }

    private void BindChart2()
    {
        DataTable dsChartData = new DataTable();

        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string query = @"select count(*) 'TicketCount',b.Employee_name  from ticket a
                                    inner join Employee b
                                    on a.Assigned_Employee_ID = b.Employee_ID
                                     where status <> '3'
                                    group by b.Employee_name ";

        SqlCommand cmd = new SqlCommand(query);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        con.Open();
        sda.Fill(dsChartData);
        con.Close();

        StringBuilder strScript = new StringBuilder();

        try
        {
            //dsChartData = GetChartData();

            strScript.Append(@"<script type='text/javascript'>  
                    google.load('visualization', '1', {packages: ['corechart']}); </script>  
                      
                    <script type='text/javascript'>  
                     
                    function drawChart() {         
                    var data = google.visualization.arrayToDataTable([  
                    ['Task', 'Hours of Day'],");

            foreach (DataRow row in dsChartData.Rows)
            {
                strScript.Append("['" + row["Employee_name"] + "'," + row["TicketCount"] + "],");
            }
            strScript.Remove(strScript.Length - 1, 1);
            strScript.Append("]);");

            strScript.Append(@" var options = {     
                                    title: 'UnResolved Tickets of Users',            
                                    is3D: true,          
                                    };   ");

            strScript.Append(@"var chart = new google.visualization.PieChart(document.getElementById('piechart_3d2'));          
                                chart.draw(data, options);        
                                }    
                            google.setOnLoadCallback(drawChart);  
                            ");
            strScript.Append(" </script>");

            ltScripts2.Text = strScript.ToString();
        }
        catch
        {
        }
        finally
        {
            dsChartData.Dispose();
            strScript.Clear();
        }
    } 
}