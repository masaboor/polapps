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
            //GridViewRefresh();
            //GetMaxServiceId();
        }
    }

    public void GetMaxServiceId()
    {
        //        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        //        SqlConnection con = new SqlConnection(constr);
        //        SqlCommand cmd = new SqlCommand(@"select case when LEN(max(CAST(impact_id as int)) + 1) = 2
        //                                         then CAST((max(CAST(impact_id as int)) + 1) as nvarchar)
        //                                          else (REPLICATE('0',1) + CAST((max(CAST(impact_id as int)) + 1) as nvarchar))
        //                                           end as 'impactID' from change_impact");
        //        SqlDataAdapter sda = new SqlDataAdapter();
        //        cmd.Connection = con;
        //        sda.SelectCommand = cmd;
        //        DataTable dt = new DataTable();
        //        con.Open();
        //        sda.Fill(dt);
        //        con.Close();

        //        if (dt.Rows.Count > 0)
        //        {
        //            txt_ServiceID.Text = dt.Rows[0][0].ToString();
        //        }
        //        else
        //        {
        //            lbl_status.Text = "No DATA!";
        //        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (txt_ServiceID.Text == "")
        {
            lbl_status.Text = "Kindly Enter ID!";
            return;
        }
        //if (txt_ServiceDesc.Text == "")
        //{
        //    lbl_status.Text = "Kindly Enter Name!";
        //    return;
        //}
        if (dd_group.SelectedValue == "0")
        {
            lbl_status.Text = "Kindly Select Application Group!";
            return;
        }

        string serviceID = txt_ServiceID.Text;
        //string serviceDesc = txt_ServiceDesc.Text;

        try
        {

            int chk_dup = Check_Duplicates(serviceID);
            switch (chk_dup)
            {
                case 0: { break; }
                case 1: { lbl_status.Text = "User Alredy Exist!"; return;  }
                case 2: { lbl_status.Text = "User Does not Exist in CIS!"; return;  }
            }

            string changeQuery = "insert into AssignmentGrp_Emp (Group_ID,  Employee_ID, Status)  values ('" + dd_group.SelectedValue.ToString().PadLeft(dd_group.SelectedValue.Length + 1,'0') + "' , '" + serviceID + "','1' )";
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
                lbl_status.Text = "Implementer Added!";
                GridViewRefresh();

            }
            else
            {
                lbl_status.Text = "Error Saving Implementer Master!";
            }
        }
        catch (Exception uy)
        {
            lbl_status.Text = uy.Message.ToString();
        }


    }

    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            int RowIndex = gvr.RowIndex;
            string implementerID = gv_ticketList.Rows[RowIndex].Cells[0].Text;
            // string role = gv_ticketList.Rows[RowIndex].Cells[2].Text;
            try
            {

            string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlConnection conn = new SqlConnection(constr);
            string deleteImplementer = "update AssignmentGrp_Emp set Status = '0' where Employee_ID = '" + implementerID + "' ";
            String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            SqlCommand cmd = new SqlCommand(deleteImplementer);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            int x = 0;
            conn.Open();
            x = cmd.ExecuteNonQuery();
            conn.Close();
            if (x > 0)
            {
                lbl_status.Text = "Implementer Successfully Removed!";
                GridViewRefresh();

            }
            else
            {
                lbl_status.Text = "Error Removing Implementer Master!";
            }
            }
            catch (Exception ex)
            {

                lbl_status.Text = ex.Message.ToString();
            }

            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");


        }
    }


    public void GridViewRefresh()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select distinct a.Employee_ID as 'user_id', b.Employee_name as 'user_name' from AssignmentGrp_Emp as a join Employee as b on a.Employee_ID = b.Employee_ID where a.Status = '1' and b.Employee_status = '1' and a.Group_ID = '0" + dd_group.SelectedValue + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            lbl_status.Text = "";
            gv_ticketList.DataSource = dt;
            gv_ticketList.DataBind();
        }
        else
        {
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No DATA!";
        }
    }

    public int Check_Duplicates(string mainid)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select * from AssignmentGrp_Emp where Employee_ID = '" + mainid + "' and Group_ID = '"+ dd_group.SelectedValue + "'";
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
            return 1;
        }
        else
        {
            SqlConnection con1 = new SqlConnection(constr);
            string activityQuery1 = @"select * from Employee where Employee_ID = '" + mainid + "'";
            SqlCommand cmd1 = new SqlCommand(activityQuery1);
            SqlDataAdapter sda1 = new SqlDataAdapter();
            cmd1.Connection = con1;
            sda1.SelectCommand = cmd1;
            DataTable dt1 = new DataTable();
            con1.Open();
            sda1.Fill(dt1);
            con1.Close();

            if (dt1.Rows.Count > 0)
            {
                return 0;
            }
            else
            {
                return 2;
            }

        }
    }
    protected void btn_searchemp_Click(object sender, EventArgs e)
    {
        lbl_impid.Visible = true;
        //lbl_impname.Visible = true;
        txt_ServiceID.Visible = true;
        //txt_ServiceDesc.Visible = true;

        btn_Save.Visible = true;

        btn_searchemp.Visible = false;
    }
    protected void dd_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRefresh();
        }
        catch (Exception ex)
        {
            //lbl_status.Text = "Unable to Load Sub Types.";
        }
    }
}