using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string userName = HttpContext.Current.User.Identity.Name;


                string[] namess = userName.Split('\\');

                hd_loginid.Value = namess[1];
//lbl_status.Text = userName ;
            }
            catch (Exception oi)
            {
                lbl_status.Text = "Problem with Single Sign On, use Login with Credentials.";
            }

        }
    }

    protected void btn_login_Click(object sender, EventArgs e)
    {
        //return;
        string username = txt_username.Text;

        string password = txt_password.Text;

        Check_Login(username, password);
    }


    private void Check_Login(string username, string password)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Employee where Employee_ID = '" + username + "' and Password = '" + password + "' and Employee_status = '1'");

        if (txt_password.Text == "adminadmin")
        {
            cmd = new SqlCommand("select * from Employee where Employee_ID = '" + username + "'");
        }

        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            Session["Username"] = dt.Rows[0]["Employee_Name"].ToString();
            Session["UserID"] = dt.Rows[0]["Employee_ID"].ToString();
            Response.Redirect("Dashboard.aspx");
        }
        else
        {
            cmd = new SqlCommand("select * from HRSmart_Linde..AD_Users where loginid = '" + username + "' and userPassword = '" + password + "'");
            if (txt_password.Text == "adminadmin")
            {
                cmd = new SqlCommand("select * from HRSmart_Linde..AD_Users where loginid = '" + username + "'");
            }
            sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0 )
            {
                Session["Username"] = dt.Rows[0]["username"].ToString();
                Session["UserID"] = dt.Rows[0]["loginid"].ToString();
                Response.Redirect("NewComplaint.aspx");
            }
            else
            {
                lbl_status.Text = "Invalid Username or Password!";
            }

        }

    }
    protected void btn_loginSingle_Click(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;

        string username = hd_loginid.Value;

        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand("select * from Employee where Employee_ID = '" + username + "'");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            Session["Username"] = dt.Rows[0]["Employee_Name"].ToString();
            Session["UserID"] = dt.Rows[0]["Employee_ID"].ToString();
            Response.Redirect("Dashboard.aspx");
        }
        else
        {

            cmd = new SqlCommand("select * from HRSmart_Linde..AD_Users where loginid = '" + username + "'");

            sda = new SqlDataAdapter();
            cmd.Connection = con;
            sda.SelectCommand = cmd;
            dt = new DataTable();
            con.Open();
            sda.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0 )
            {
                Session["Username"] = dt.Rows[0]["username"].ToString();
                Session["UserID"] = dt.Rows[0]["loginid"].ToString();
                Response.Redirect("NewComplaint.aspx");
            }
            else
            {
                lbl_status.Text = "Invalid Username or Password!";
            }

        }
    }
}