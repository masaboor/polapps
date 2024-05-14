using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ServiceMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridViewRefresh();
            GetMaxServiceId();
        }
    }

    public void GetMaxServiceId()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select case when LEN(max(CAST(service_id as int)) + 1) = 3
                                         then CAST((max(CAST(service_id as int)) + 1) as nvarchar)
                                         when  LEN(max(CAST(service_id as int)) + 1) = 2
                                         then (REPLICATE('0',1) + CAST((max(CAST(service_id as int)) + 1) as nvarchar))
                                          else (REPLICATE('0',2) + CAST((max(CAST(service_id as int)) + 1) as nvarchar))
                                           end as 'serviceID' from Change_Services_New
                                           where service_id <> 'All'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            txt_ServiceID.Text = dt.Rows[0][0].ToString();
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        string serviceID = txt_ServiceID.Text;
        string serviceDesc = txt_ServiceDesc.Text;

        try
        {

            DataTable dt = Check_Duplicates(serviceID);
            if (dt.Rows.Count > 0)
            {
                lbl_status.Text = "Cannot enter duplicate rows!";
                return;
            }

            string changeQuery = "insert into Change_Services_New (Service_ID, Service_Description)  values ('" + serviceID + "','" + serviceDesc + "')";
            SqlCommand cmd = new SqlCommand(changeQuery);
            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            int x = 0;
            con.Open();
            x = cmd.ExecuteNonQuery();
            con.Close();

            if (x > 0)
            {
                lbl_status.Text = "Service Master Added!";
                GridViewRefresh();

            }
            else
            {
                lbl_status.Text = "Error Saving Service Master!";
            }
        }
        catch (Exception uy)
        {
            lbl_status.Text = uy.Message.ToString();
        }

      
    }


    public void GridViewRefresh()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select * from Change_Services_New");
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
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }


    public DataTable Check_Duplicates(string mainid)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select * from Change_Services_New where service_id = '" + mainid + "'";
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
}