using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AuditLogExt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetApplicationName();
            //GetCategory();
            //GetStage();
            //dd_Profile.SelectedValue = "1";
            //btn_search_Click(sender, e);

        }
    }


    public void GetApplicationName()
    {
        div_error.Visible = false;
        lbl_error.Text = "";
        string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(@"select [ApplicationID], [Name] from [tbl_Applications]
                                            order by Name");
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();
        con.Open();
        sda.Fill(dt);
        con.Close();

        if (dt.Rows.Count > 0)
        {
            dd_Application.DataTextField = "Name";
            dd_Application.DataValueField = "ApplicationID";
            dd_Application.DataSource = dt;
            dd_Application.DataBind();
            dd_Application.Items.Insert(0, new ListItem("All", "All"));
        }
        else
        {
            div_error.Visible = true;
            lbl_error.Text = "No Data";
        }
    }

    protected void dd_Application_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string application = "";
            if (dd_Application.Text != "All")
            {
                application = dd_Application.SelectedValue;
                //GetFormName(application);
            }

        }
        catch (Exception)
        {

            throw;
        }

    }

   

    protected void btn_download_Click(object sender, EventArgs e)
    {
        div_error.Visible = false;
        lbl_error.Text = "";

        //if (dd_Application.SelectedValue == "All")
        //{
        //    div_error.Visible = true;
        //    lbl_error.Text = "Please Select as Application!";
        //    return;
        //}
        if (String.IsNullOrEmpty(txt_datefrom.Text))
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select From Date!";
            return;
        }
        if (String.IsNullOrEmpty(txt_dateto.Text))
        {
            div_error.Visible = true;
            lbl_error.Text = "Please Select To Date!";
            return;
        }

        string application = "", dateparam = "", pagename = "";

        if (txt_dateto.Text != "" && txt_dateto.Text != "")
        {
            //dateparam = " and CreatedDate between '" + txt_datefrom.Text + "' and '" + txt_dateto.Text + "'";
            dateparam = " and CreatedDate between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
        }

        if (dd_Application.SelectedValue == "All")
        {

        }
        else
        {
            application = "and ApplicationID = '" + dd_Application.SelectedValue + "'";
        }

        //if (dd_pageName.Text == "All")
        //{

        //}
        //else
        //{
        //    pagename = "and Controller = '" + dd_pageName.Text + "' OR Form = '" + dd_pageName.Text + "'";
        //}

        GetBySearch_Excel(application, dateparam, pagename);

    }

    public void GetBySearch_Excel(string application, string dateparam, string pagename)
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);


        string query = @"SELECT * FROM [v_ApplicationLogs] WHERE 1=1 " + application + dateparam + pagename + " order by CreatedDate desc, CreatedTime desc";
        try
        {

        


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
                div_error.Visible = false;
                lbl_error.Text = "";

                string[] ColumnsToBeDeleted = { "LogsID", "ApplicationID", "ActionMethod", "Controller", "Form", "Status"};

                foreach (string ColName in ColumnsToBeDeleted)
                {
                    if (dt.Columns.Contains(ColName))
                        dt.Columns.Remove(ColName);
                }

               
                Email.ExportToSpreadsheet(dt, "Audit_Log_Report");
            }
            else
            {
                div_error.Visible = true;
                lbl_error.Text = "No Records found for your selection!";
            }
        }
        catch (Exception ex)
        {

            //throw;
        }
    }


    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {

            div_error.Visible = false;
            lbl_error.Text = "";

            //if (dd_Application.SelectedValue == "All")
            //{
            //    div_error.Visible = true;
            //    lbl_error.Text = "Please Select as Application!";
            //    return;
            //}
            if (String.IsNullOrEmpty(txt_datefrom.Text))
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select From Date!";
                return;
            }
            if (String.IsNullOrEmpty(txt_dateto.Text))
            {
                div_error.Visible = true;
                lbl_error.Text = "Please Select To Date!";
                return;
            }

            string application = "", dateparam = "", pagename = "";

            if (txt_dateto.Text != "" && txt_dateto.Text != "")
            {
                //dateparam = " and CreatedDate between '" + txt_datefrom.Text + "' and '" + txt_dateto.Text + "'";
                dateparam = " and CreatedDate between CONVERT(DATE, '" + txt_datefrom.Text + "', 105) and CONVERT(DATE, '" + txt_dateto.Text + "', 105)";
            }

            if (dd_Application.SelectedValue == "All")
            {

            }
            else
            {
                application = "and ApplicationID = '" + dd_Application.SelectedValue + "'";
            }

            //if (dd_pageName.Text == "All")
            //{

            //}
            //else
            //{
            //    pagename = "and (Controller = '" + dd_pageName.Text + "' OR Form = '" + dd_pageName.Text + "')";
            //}

            string constr = ConfigurationManager.ConnectionStrings["ConString_LogTrail"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);


            string query = @"SELECT * FROM [v_ApplicationLogs] WHERE 1=1 " + application + dateparam + pagename + " order by CreatedDate desc, CreatedTime desc";



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
                gv_log.DataSource = null;
                gv_log.DataBind();
                gv_log.DataSource = dt;
                gv_log.DataBind();
                lbl_error.Text = "";
            }
            else
            {
                div_error.Visible = true;
                lbl_error.Text = "No Records found for your selection!";
            }

        }
        catch (Exception ex)
        {

            //throw;
        }

    }
}