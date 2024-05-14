using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Username"] != null)
            {
                lbl_Username.Text = Session["Username"].ToString();

            }

            if (IsPostBack)
            {
                // It is a postback
            }
            else
            {
                GetMenu_new();
                // It is not a postback
            }
        }
        catch(Exception ex)
        {
            Response.Redirect("Login.aspx");
        }
    }

    protected void lb_logout_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("Login.aspx");
    }


    public void LoadMenu()
    {
        lt_menu.Text = "<li> \n" +
                       "     <a href = \"Dashboard.aspx\" ><i class=\"fa fa-dashboard fa-fw\"></i>Dashboard</a>\n" +
                       " </li>\n" +
                       " <li>\n" +
                       "     <a href = \"#\" ><i class=\"fa fa-bar-chart-o fa-fw\"></i>Ticket Management<span class=\"fa arrow\"></span></a>\n" +
                       "     <ul class=\"nav nav-second-level\">\n" +
                       "         <li>\n" +
                       "             <a href = \"NewTicket.aspx\" > New Ticket</a>\n" +
                       "         </li>\n" +
                       "         <li>\n" +
                       "             <a href = \"TicketList.aspx\" > Ticket List</a>\n" +
                       "         </li>\n" +
                       "     </ul>\n" +
                       "     <!-- /.nav-second-level -->\n" +
                       " </li>";



    }


    //public void GetMenu()
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


    //    SqlConnection con = new SqlConnection(constr);
    //    SqlCommand cmd = new SqlCommand("select Child_ID, Child_Description, Child_URL from Child_Menu where parent_id = '1' and status = '1'");
    //    SqlDataAdapter sda = new SqlDataAdapter();
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    DataTable dt = new DataTable();
    //    DataTable dt_setup = new DataTable();
    //    DataTable dt1 = new DataTable();
    //    DataTable dt2 = new DataTable();
    //    DataTable dt3 = new DataTable();
    //    con.Open();
    //    sda.Fill(dt);
    //    con.Close();



    //    cmd = new SqlCommand("select Child_ID, Child_Description, Child_URL from Child_Menu where parent_id = '2' and status = '1'");
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    con.Open();
    //    sda.Fill(dt1);
    //    con.Close();



    //    cmd = new SqlCommand("select Child_ID, Child_Description, Child_URL from Child_Menu where parent_id = '3' and status = '1'");
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    con.Open();
    //    sda.Fill(dt2);
    //    con.Close();


    //    cmd = new SqlCommand("select Child_ID, Child_Description, Child_URL from Child_Menu where parent_id = '4' and status = '1'");
    //    cmd.Connection = con;
    //    sda.SelectCommand = cmd;
    //    con.Open();
    //    sda.Fill(dt3);
    //    con.Close();


    //    if (dt.Rows.Count > 0)
    //    {
    //        lt_menu.Text = "<li> \n" +
    //                  "     <a href = \"dashboard.aspx\" ><i class=\"fa fa-dashboard fa-fw\"></i>Dashboard</a>\n" +
    //                  " </li>\n" +
    //                  " <li>\n" +
    //                  "     <a href = \"#\" ><i class=\"fa fa-ticket fa-fw\"></i>Ticket Management<span class=\"fa arrow\"></span></a>\n" +
    //                  "     <ul class=\"nav nav-second-level\">\n";

    //        foreach (DataRow item in dt.Rows)
    //        {
    //            lt_menu.Text += "         <li>\n" +
    //                  "             <a href = \"" + item["child_url"].ToString() + "\" > " + item["child_description"].ToString() + "</a>\n" +
    //                  "         </li>\n";
    //            //"         <li>\n" +
    //            //"             <a href = \"NewTicket.aspx\" > New Ticket</a>\n" +
    //            //"         </li>\n" +
    //            //"         <li>\n" +
    //            //"             <a href = \"TicketList.aspx\" > Ticket List</a>\n" +
    //            //"         </li>\n" +


    //        }

    //        lt_menu.Text += "     </ul>\n" +
    //                     "     <!-- /.nav-second-level -->\n" +
    //                     " </li>";


    //        lt_menu.Text += " <li>\n" +
    //                 "     <a href = \"#\" ><i class=\"fa fa-sliders fa-fw\"></i>Change Management Setup<span class=\"fa arrow\"></span></a>\n" +
    //                 "     <ul class=\"nav nav-second-level\">\n";




    //        lt_menu.Text += "     </ul>\n" +
    //                    "     <!-- /.nav-second-level -->\n" +
    //                    " </li>";


    //        lt_menu.Text += " <li>\n" +
    //                  "     <a href = \"#\" ><i class=\"fa fa-sliders fa-fw\"></i>Change Management<span class=\"fa arrow\"></span></a>\n" +
    //                  "     <ul class=\"nav nav-second-level\">\n";

    //        foreach (DataRow item in dt1.Rows)
    //        {
    //            lt_menu.Text += "         <li>\n" +
    //                  "             <a href = \"" + item["child_url"].ToString() + "\" > " + item["child_description"].ToString() + "</a>\n" +
    //                  "         </li>\n";
    //            //"         <li>\n" +
    //            //"             <a href = \"NewTicket.aspx\" > New Ticket</a>\n" +
    //            //"         </li>\n" +
    //            //"         <li>\n" +
    //            //"             <a href = \"TicketList.aspx\" > Ticket List</a>\n" +
    //            //"         </li>\n" +


    //        }

    //        lt_menu.Text += "     </ul>\n" +
    //                     "     <!-- /.nav-second-level -->\n" +
    //                     " </li>";




    //        lt_menu.Text += " <li>\n" +
    //                  "     <a href = \"#\" ><i class=\"fa fa-rocket fa-fw\"></i>Service Management<span class=\"fa arrow\"></span></a>\n" +
    //                  "     <ul class=\"nav nav-second-level\">\n";

    //        foreach (DataRow item in dt2.Rows)
    //        {
    //            lt_menu.Text += "         <li>\n" +
    //                  "             <a href = \"" + item["child_url"].ToString() + "\" > " + item["child_description"].ToString() + "</a>\n" +
    //                  "         </li>\n";
    //            //"         <li>\n" +
    //            //"             <a href = \"NewTicket.aspx\" > New Ticket</a>\n" +
    //            //"         </li>\n" +
    //            //"         <li>\n" +
    //            //"             <a href = \"TicketList.aspx\" > Ticket List</a>\n" +
    //            //"         </li>\n" +


    //        }

    //        lt_menu.Text += "     </ul>\n" +
    //                     "     <!-- /.nav-second-level -->\n" +
    //                     " </li>";


    //        lt_menu.Text += " <li>\n" +
    //                 "     <a href = \"#\" ><i class=\"fa fa-sliders fa-fw\"></i>Change Management Old<span class=\"fa arrow\"></span></a>\n" +
    //                 "     <ul class=\"nav nav-second-level\">\n";

    //        foreach (DataRow item in dt3.Rows)
    //        {
    //            lt_menu.Text += "         <li>\n" +
    //                  "             <a href = \"" + item["child_url"].ToString() + "\" > " + item["child_description"].ToString() + "</a>\n" +
    //                  "         </li>\n";
    //            //"         <li>\n" +
    //            //"             <a href = \"NewTicket.aspx\" > New Ticket</a>\n" +
    //            //"         </li>\n" +
    //            //"         <li>\n" +
    //            //"             <a href = \"TicketList.aspx\" > Ticket List</a>\n" +
    //            //"         </li>\n" +


    //        }

    //        lt_menu.Text += "     </ul>\n" +
    //                     "     <!-- /.nav-second-level -->\n" +
    //                     " </li>";
    //    }
    //    else
    //    {

    //    }
    //}

    public void GetMenu_new()
    {
        string constr = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;


        SqlConnection con = new SqlConnection(constr);

        string queryyyy = @"select p.Parent_ID, p.Parent_Description,
                                        c.Child_ID, c.Child_Description, c.Child_URL from Parent_Menu p
                                        inner join Child_Menu c
                                        on p.Parent_ID = c.Parent_ID
                                        where p.Status = '1'
                                        and c.Status = '1'
										and (CAST(c.Parent_ID AS nvarchar) + CAST(c.Child_ID as nvarchar)) in (select  CAST(parent_id as nvarchar)  + CAST(child_id as nvarchar) from useraccessrights where role_id in (select role_id from userroleemployee where loginid = '"+Session["UserID"].ToString()+@"'))
                                        order by p.Parent_ID, p.Parent_Description,
                                        c.Child_ID, c.Child_Description";

//        SqlCommand cmd = new SqlCommand(@"select p.Parent_ID, p.Parent_Description,
//                                        c.Child_ID, c.Child_Description, c.Child_URL from Parent_Menu p
//                                        inner join Child_Menu c
//                                        on p.Parent_ID = c.Parent_ID
//                                        where p.Status = '1'
//                                        and c.Status = '1'
//                                        order by p.Parent_ID, p.Parent_Description,
//                                        c.Child_ID, c.Child_Description");

        SqlCommand cmd = new SqlCommand(queryyyy);
        SqlDataAdapter sda = new SqlDataAdapter();
        cmd.Connection = con;
        sda.SelectCommand = cmd;
        DataTable dt = new DataTable();

        con.Open();
        sda.Fill(dt);
        con.Close();





        if (dt.Rows.Count > 0)
        {
            //lt_menu.Text = "<li> \n" +
            //          "     <a href = \"dashboard.aspx\" ><i class=\"fa fa-dashboard fa-fw\"></i>Dashboard</a>\n" +
            //          " </li>\n";
                    


            DataView view = new DataView(dt);
            DataTable distinctValues = view.ToTable(true, "Parent_ID");

            foreach (DataRow item in distinctValues.Rows)
            {
                DataRow[] children = dt.Select("Parent_ID = '" + item[0].ToString() + "'", null);

                lt_menu.Text += " <li>\n" +
                    "     <a href = \"#\" ><i class=\"fa fa-sliders fa-fw\"></i>" + children[0]["Parent_Description"].ToString() + "<span class=\"fa arrow\"></span></a>\n" +
                    "     <ul class=\"nav nav-second-level\">\n";

                foreach (DataRow itemx in children)
                {
                    lt_menu.Text += "         <li>\n" +
                      "             <a href = \"" + itemx["child_url"].ToString() + "\" > " + itemx["child_description"].ToString() + "</a>\n" +
                      "         </li>\n";
                }

                lt_menu.Text += "     </ul>\n" +
                         "     <!-- /.nav-second-level -->\n" +
                         " </li>";


            }



        }
        else
        {

        }
    }
}