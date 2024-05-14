using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //GridViewRefresh();
            //GetMaxServiceId();
        }
    }

    // public void GetMaxServiceId()
    // {
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
    // }

    //public void updateEmployeeRole(string employeeID)
    //{
    //    char roleID = '4';
    //    string employeeRole = "insert into UserRoleEmployee (Role_ID, loginID, Status) values ('" + roleID + "', '" + employeeID + "', '1')";
    //    SqlCommand cmd = new SqlCommand(employeeRole);
    //    String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
    //    SqlConnection con = new SqlConnection(strConnString);
    //    cmd.CommandType = CommandType.Text;
    //    cmd.Connection = con;
    //    con.Open();
    //    cmd.ExecuteNonQuery();
    //    con.Close();
    //}

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        if (dd_employeeAD.SelectedValue == "")
        {
            lbl_status.Text = "Kindly select an Employee!";
            return;
        }
        if (dd_group.SelectedValue == "0")
        {
            lbl_status.Text = "Kindly Select Application Group!";
            return;
        }

        //if (txt_empname.Text == "")
        //{
        //    lbl_status.Text = "Kindly Enter Employee Name!";
        //    return;
        //}


        //if (txt_emppassword.Text == "")
        //{
        //    lbl_status.Text = "Chose a password";
        //    return;
        //}

        string employeeID = dd_employeeAD.SelectedValue;
        string employeeDept = dd_group.SelectedValue;
        //string employeeName = txt_empname.Text;
        //string employeeEmail = txt_empemail.Text;
        //string employeeNumber = txt_empnumber.Text;
        //string empPass = txt_emppassword.Text;


        try
        {

            int chk_dup = Check_Duplicates(employeeID, employeeDept);
            int dataRetrieved = 0;
            switch (chk_dup)
            {
                case 0: { lbl_status.Text = "User Alredy Exist!";  break; }
                case 1: 
                    {
                        // string addEmployee = "insert into Employee (Dept_ID, Employee_ID, Employee_name, Email, Phone, Password) values ('0" + dd_group.SelectedValue + "', '" + employeeID + "', '" + employeeName + "', '" + employeeEmail + "', '" + employeeNumber + "', '" + empPass + "')";
                        // string addEmployee = "IF NOT EXISTS (SELECT * FROM Employee where Employee_ID = '" + employeeID + "') BEGIN TRY BEGIN TRANSACTION  INSERT INTO Employee (Dept_ID, Employee_ID, Employee_name, Email, Phone, Password)   VALUES ('0" + dd_group.SelectedValue + "', '" + employeeID + "', '" + employeeName + "', '" + employeeEmail + "', '" + employeeNumber + "', '" + empPass + "');  INSERT INTO AssignmentGrp_Emp (Group_ID, Employee_ID, Status)    VALUES ('0" + dd_group.SelectedValue + "', '" + employeeID + "', '1');   COMMIT; END TRY BEGIN CATCH  ROLLBACK; END CATCH;";
                       
                        string hrConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString_HRSmart"].ConnectionString;
                        SqlConnection con = new SqlConnection(hrConnString);
                        string ADEmployee = " select loginId, UserName, UserPassword, EmailAddress, MobileNo from AD_Users where LoginId = '" + dd_employeeAD.SelectedValue + "'";
                        SqlCommand cmd = new SqlCommand(ADEmployee);
                        SqlDataAdapter sda = new SqlDataAdapter();
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        DataTable dt = new DataTable();
                        con.Open();
                        sda.Fill(dt);
                        con.Close();
                        con.Dispose();
                        if (dt.Rows.Count > 0)
                        {
                            string dept = dd_group.SelectedValue;
                            string loginID = dt.Rows[0]["loginId"].ToString();
                            string employeeName = dt.Rows[0]["UserName"].ToString();
                            string empPass = dt.Rows[0]["UserPassword"].ToString();
                            string employeeEmail = dt.Rows[0]["EmailAddress"].ToString();
                            string employeeNumber = dt.Rows[0]["MobileNo"].ToString();

                            string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                            SqlConnection con2 = new SqlConnection(strConnString);
                            string addEmployee = "insert into Employee (Dept_ID, Employee_ID, Employee_name, Email, Phone, Password, Employee_status) values ('" + dept + "', '" + employeeID + "', '" + employeeName + "', '" + employeeEmail + "', '" + employeeNumber + "', '" + empPass + "', '1')";
                            SqlCommand cmd2 = new SqlCommand(addEmployee);
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Connection = con2;
                            con2.Open();
                            dataRetrieved = cmd2.ExecuteNonQuery();
                            con2.Close();
                            if (dataRetrieved > 0) { GridViewRefresh(); lbl_status.Text = "Employee Added Successfully!"; }
                            else { lbl_status.Text = "Error Saving Employee"; }
                        }

                        //cmd.CommandType = CommandType.Text;
                        //cmd.Connection = con;
                        //con.Open();
                        //dataRetrieved = cmd.ExecuteNonQuery();
                        //con.Close();
                        //updateEmployeeRole(employeeID);
                        //if (dataRetrieved > 0) { lbl_status.Text = "Employee Added Successfully!"; GridViewRefresh(); }
                        //else { lbl_status.Text = "Error Saving Employee"; }
                        return ; 
                    }
                    case 5: 
                    {
                        string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                        SqlConnection con = new SqlConnection(strConnString);
                        string update = "update Employee set Employee_status = '1' where Employee_ID = '" + employeeID + "'";
                        SqlCommand cmd = new SqlCommand(update);
                        SqlDataAdapter sda = new SqlDataAdapter();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        dataRetrieved = cmd.ExecuteNonQuery();
                        con.Close();
                        if (dataRetrieved > 0) {  GridViewRefresh(); lbl_status.Text = "Employee Added Successfully!"; }
                        else { lbl_status.Text = "Error Saving Employee"; }
                        break;
                    }
            }

            //string changeQuery = "insert into AssignmentGrp_Emp (Group_ID,  Employee_ID, Status)  values ('" + dd_group.SelectedValue.ToString().PadLeft(dd_group.SelectedValue.Length + 1, '0') + "' , '" + serviceID + "','1' )";
            //SqlCommand cmd = new SqlCommand(changeQuery);
            //String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            //SqlConnection con = new SqlConnection(strConnString);
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = con;
            //int x = 0;
            //con.Open();
            //x = cmd.ExecuteNonQuery();
            //con.Close();

            //if (x > 0)
            //{
            //    lbl_status.Text = "Implementer Added!";
            //    GridViewRefresh();

            //}
            //else
            //{
            //    lbl_status.Text = "Error Saving Implementer Master!";
            //}
        }
        catch (Exception uy)
        {
            lbl_status.Text = uy.Message.ToString();
        }


    }

    protected void btn_Edit_Click(object sender, EventArgs e)
    {
        if (dd_employeeAD.SelectedValue == "")
        {
            lbl_status.Text = "Kindly select an Employee!";
            return;
        }
        if (dd_group.SelectedValue == "0")
        {
            lbl_status.Text = "Kindly Select Application Group!";
            return;
        }

   

        string employeeID = dd_employeeAD.SelectedValue;
        string employeeDept = dd_group.SelectedValue;



        try
        {

            int chk_dup = Check_Duplicates(employeeID, employeeDept);
            int dataRetrieved = 0;
            switch (chk_dup)
            {
                case 0: { lbl_status.Text = "User Alredy Exist!"; break; }
                case 1:
                    {
                        
                        string strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                        SqlConnection con = new SqlConnection(strConnString);
                        string update = "update employee set Dept_ID = '" + dd_group.SelectedValue + "' where Employee_ID = '" + dd_employeeAD.SelectedValue + "'";
                        SqlCommand cmd = new SqlCommand(update);
                        SqlDataAdapter sda = new SqlDataAdapter();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        dataRetrieved = cmd.ExecuteNonQuery();
                        con.Close();
                        if (dataRetrieved > 0) { lbl_status.Text = "Employee Added Successfully!"; GridViewRefresh(); }
                        else { lbl_status.Text = "Error Updating Employee"; }

                        return;
                    }
       
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

        string showEmployee = "select Employee_ID as 'user_id', Employee_name as 'user_name', Dept_ID  as 'emp_group' from Employee where Employee_status = '1' and Dept_ID = '" + dd_group.SelectedValue + "'";
        SqlCommand cmd = new SqlCommand(showEmployee);
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
               .Where(fld => (fld.HeaderText == "Group"))
               .SingleOrDefault()).Visible = false;
        }
        else
        {
            gv_ticketList.DataSource = null;
            gv_ticketList.DataBind();
            lbl_status.Text = "No DATA!";
        }
    }

    public int Check_Duplicates(string mainid, string employeeDept)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        //SqlConnection con = new SqlConnection(constr);
        //string activityQuery = @"select * from AssignmentGrp_Emp where Employee_ID = '" + mainid + "' and Group_ID = '" + dd_group.SelectedValue + "'";
        //SqlCommand cmd = new SqlCommand(activityQuery);
        //SqlDataAdapter sda = new SqlDataAdapter();
        //cmd.Connection = con;
        //sda.SelectCommand = cmd;
        //DataTable dt = new DataTable();
        //con.Open();
        //sda.Fill(dt);
        //con.Close();

        //if (dt.Rows.Count > 0)
        //{
        //    return 1;
        //}
        //else
        //{
            SqlConnection con1 = new SqlConnection(constr);
            string activityQuery1 = @"select * from Employee where Employee_ID = '" + mainid + "' and Dept_ID = '" + employeeDept + "'";
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
                string statusValue = dt1.Rows[0]["Employee_Status"].ToString();
                string dept = dt1.Rows[0]["Dept_ID"].ToString();
                if(statusValue == "0")
                {
                    return 5;
                }
                    return 0;
            }
            else
            {
                return 1;
            }

       // }
    }
    protected void btn_searchemp_Click(object sender, EventArgs e)
    {
        //lbl_impid.Visible = true;
        //txt_ServiceID.Visible = true;
        //lbl_empname.Visible = true;
        //txt_empname.Visible = true;
        //lbl_empemail.Visible = true;
        //txt_empemail.Visible = true;
        //lbl_empnumber.Visible = true;
        //txt_empnumber.Visible = true;
        //lbl_emppassword.Visible = true;
        //txt_emppassword.Visible = true;
        //txt_ServiceDesc.Visible = true;
       // GridViewRefresh();
        lbl_employee.Visible = true;
        lbl_group.Visible = true;
        dd_group.Visible = true;
        dd_employeeAD.Visible = true;
        btn_Save.Visible = true;
       // btn_Edit.Visible = true;

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

    protected void dd_employeeAD_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //GridViewRefresh();
        }
        catch (Exception ex)
        {
            //lbl_status.Text = "Unable to Load Sub Types.";
        }
    }

    protected void gv_ticketList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "remove")
        {
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            ((DataControlField)gv_ticketList.Columns
               .Cast<DataControlField>()
               .Where(fld => (fld.HeaderText == "Group"))
               .SingleOrDefault()).Visible = true;

            int RowIndex = gvr.RowIndex;
            string employeeID = gv_ticketList.Rows[RowIndex].Cells[0].Text;
            string dept = gv_ticketList.Rows[RowIndex].Cells[2].Text;
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlConnection conn = new SqlConnection(constr);
                string inActivatEemployee = "update Employee set Employee_status = '0' where Employee_ID = '" + employeeID + "' and Dept_ID = '" + dept + "'";
                String strConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
                SqlCommand cmd = new SqlCommand(inActivatEemployee);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;
                int x = 0;
                conn.Open();
                x = cmd.ExecuteNonQuery();
                conn.Close();
                if (x > 0)
                {
                   
                    GridViewRefresh();
                    lbl_status.Text = "Employee Inactivated Successfully!";
                }
                else
                {
                    lbl_status.Text = "Error Inactivating Employee!";
                }
            }
            catch (Exception ex)
            {

                lbl_status.Text = ex.Message.ToString();
            }

            //LinkButton lb = (LinkButton)gv_ticketList.Rows[RowIndex].FindControl("lb_ticket");


        }

        if(e.CommandName == "editemp")
        {
            btn_Edit.Visible = true;
            btn_Save.Visible = false;
            bool caseLock = false;
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            int RowIndex = gvr.RowIndex;
            string employeeID = gv_ticketList.Rows[RowIndex].Cells[0].Text;
            string employeeName = gv_ticketList.Rows[RowIndex].Cells[1].Text;

            try
            {
                dd_employeeAD.SelectedValue = employeeID;

                lbl_status.Text = "";
            }
            catch (Exception ex)
            {

                caseLock = true;
            }
        
            if (caseLock)
            {
                try
                {
                    string lowerID = employeeID.ToLower();
                    dd_employeeAD.SelectedValue = lowerID;
                }
                catch (Exception ex)
                {


                    lbl_status.Text = "User might not exist in Active Directory";
                }
                
            }
        }

    }

}
