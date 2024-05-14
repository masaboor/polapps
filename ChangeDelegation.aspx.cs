using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangeDelegation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txt_datefrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_dateto.Text = DateTime.Now.ToString("dd/MM/yyyy");

            GetApprover();
            GetNewApprover();
        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {

        try
        {

            string oldapprover = dd_ChangeApprover.SelectedValue;
            string newapprover = dd_ChangeNewApprover.SelectedValue;

            string datefrom = txt_datefrom.Text;
            string dateto = txt_dateto.Text;


            string changeQuery = "insert into change_delegation (Employee_ID, start_date, end_date, temp_employee_id) values ('" + oldapprover + "',convert(date,'" + datefrom + "',103),convert(date,'" + dateto + "',103),'" + newapprover + "')";
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
                lbl_status.Text = "Delegation Created";
                showAlert("Delegation Created");
            }
            else
            {
                lbl_status.Text = "Could not insert delegation!";
                showAlert("Could not insert delegation!");
            }
        }
        catch (Exception oi)
        {
            lbl_status.Text = oi.Message;
            showAlert(oi.Message);
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


    public void GetApprover()
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
            dd_ChangeApprover.DataTextField = "Employee_name";
            dd_ChangeApprover.DataValueField = "Employee_ID";
            dd_ChangeApprover.DataSource = dt;
            dd_ChangeApprover.DataBind();
            dd_ChangeApprover.Items.Insert(0, new ListItem("Please Select", "0"));

            dd_ChangeApprover.SelectedValue = "PK002C";
            dd_ChangeApprover.Enabled = false;
        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }


    public void GetNewApprover()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select Dept_id,Employee_ID,Employee_name from Employee where Employee_id not in ('PK002C')
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
            dd_ChangeNewApprover.DataTextField = "Employee_name";
            dd_ChangeNewApprover.DataValueField = "Employee_ID";
            dd_ChangeNewApprover.DataSource = dt;
            dd_ChangeNewApprover.DataBind();
            dd_ChangeNewApprover.Items.Insert(0, new ListItem("Please Select", "0"));

        }
        else
        {
            lbl_status.Text = "No DATA!";
        }
    }
}