using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EmployeeRole : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
        }
    }


    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (dd_EmployeeRole.SelectedValue == "0")
        {
            lbl_status.Text = "Please select Employee Role";
            return;
        }

        if (dd_EmployeeName.SelectedValue == "0")
        {
            lbl_status.Text = "Please select Employee Name";
            return;
        }

        //string serviceID = txt_ServiceID.Text;
        string employeeRole = dd_EmployeeRole.SelectedValue;
        string employeeID = dd_EmployeeName.SelectedValue;



        try
        {

            int chk_dup = Check_Duplicates(employeeRole, employeeID);
            switch (chk_dup)
            {
                case 0:
                    {
                       // string status = "1";
                        string changeQuery = "insert into UserRoleEmployee (Role_ID, loginID, Status) values ('" + employeeRole + "', '" + employeeID + "', '1')";
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
                            
                            GridViewRefresh();
                            lbl_status.Text = "Role Added!";
                        }
                        else
                        {
                            lbl_status.Text = "Error Saving Implementer Master!";
                        }
                        break;
                    }
                case 1: { lbl_status.Text = "User Alredy Exist!"; return; }
                case 2: { lbl_status.Text = "User Does not Exist in CIS!"; return; }
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
        SqlCommand cmd = new SqlCommand("select distinct UserRoleEmployee.loginID as EmployeeID, Employee.Employee_name as EmployeeName, UserRoles.RoleName as RoleName, UserRoles.Role_ID as RoleID from UserRoleEmployee inner join UserRoles on UserRoleEmployee.Role_ID = UserRoles.Role_ID inner join Employee on Employee.Employee_ID = UserRoleEmployee.loginID order by EmployeeName");
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
            
            
            ((DataControlField)gv_ticketList.Columns
              .Cast<DataControlField>()
              .Where(clmn => (clmn.HeaderText == "Role ID"))
              .SingleOrDefault()).Visible = false;
        }
        else
        {
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No DATA!";
        }
    }
    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "show")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);



            ((DataControlField)gv_ticketList.Columns
              .Cast<DataControlField>()
              .Where(clmn => (clmn.HeaderText == "Role ID"))
              .SingleOrDefault()).Visible = true;



            int RowIndex = gvr.RowIndex;
            string employeeID = gv_ticketList.Rows[RowIndex].Cells[0].Text;
            string role = gv_ticketList.Rows[RowIndex].Cells[3].Text;
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection conn = new SqlConnection(constr);
                string deleteRole = "delete from UserRoleEmployee where loginID = '" + employeeID + "' and Role_ID = '" + role + "'";
                String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlCommand cmd = new SqlCommand(deleteRole);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                int x = 0;
                conn.Open();
                x = cmd.ExecuteNonQuery();
                conn.Close();
                if (x > 0)
                {
                    GridViewRefresh();
                    lbl_status.Text = "Role Successfully Removed!";

                }
                else
                {
                    lbl_status.Text = "Error Removing Role!";
                }
            }
            catch (Exception ex)
            {

                lbl_status.Text = ex.Message.ToString();
            }

            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");





            ((DataControlField)gv_ticketList.Columns
              .Cast<DataControlField>()
              .Where(clmn => (clmn.HeaderText == "Role ID"))
              .SingleOrDefault()).Visible = false;

        }
    }

    public int Check_Duplicates(string employeeRole, string employeeID)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        string activityQuery = @"select * from UserRoleEmployee where Role_ID = '" + employeeRole + "' and loginID = '" + employeeID + "'";
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
            string activityQuery1 = @"select * from Employee where Employee_ID = '" + employeeID + "'";
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
        GridViewRefresh();
        lbl_EmployeeName.Visible = true;
        dd_EmployeeName.Visible = true;
        lbl_EmployeeRole.Visible = true;
        dd_EmployeeRole.Visible = true;
        btn_Save.Visible = true;
        btn_searchemp.Visible = false;
    }
    protected void dd_EmployeeRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
           GridViewRefresh();
        }
        catch (Exception ex)
        {
            
        }
    }

    protected void dd_EmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //GridViewRefresh();
        }
        catch (Exception ex)
        {

        }
    }

}