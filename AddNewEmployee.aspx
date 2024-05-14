
<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEmployee.aspx.cs" Inherits="Employee" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddNewEmployee.aspx.cs" Inherits="Employee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

      <style type="text/css">

        .marginDiv {
            margin-top: 25px;
        }

    </style>

    <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    <div class="panel panel-info">
        <div class="panel-heading">
            Add new Employee
                       
        </div>
        <div class="panel-body">

            <div class="form-group">

                <div class="row">

                    <div class="col-lg-10">

                        <div class="col-lg-2">
                            <asp:LinkButton runat="server" ID="btn_searchemp" CssClass="btn btn-info" OnClick="btn_searchemp_Click">
                            <i class="glyphicon glyphicon-plus" aria-hidden="true"></i> New
                            </asp:LinkButton>
                        </div>

                    </div>
                </div>

                <div class="form form-group">
                </div>

                <div class="row">
                    <div class="col-lg-4">
                        <%--<label>Application Group</label>--%>
                         <asp:Label ID="lbl_group" runat="server" Visible="false" Font-Bold="True">Application Group</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-4">

                        <asp:DropDownList ID="dd_group" runat="server" Visible="false" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource2" DataTextField="Dept_Desc"
                            DataValueField="Dept_ID" OnSelectedIndexChanged="dd_group_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<% $ connectionStrings:ConString %>"
                            SelectCommand="SELECT Dept_ID='0', Dept_Desc='Please Select'
                                        UNION
                                        SELECT Dept_ID, Dept_Desc FROM Department
                                        WHERE status = '1'"></asp:SqlDataSource>

                           <%-- SelectCommand="SELECT Dept_ID=0, Dept_Desc='Please Select'
                                        UNION
                                        SELECT Dept_ID, Dept_Desc FROM Department
                                        WHERE status = '1'"></asp:SqlDataSource>--%>

                    </div>





                     <%--<div class="col-lg-4">

                        <asp:DropDownList ID="dd_employeeAD" runat="server" Visible="true" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource1" DataTextField="UserName"
                            DataValueField="LoginId" OnSelectedIndexChanged="dd_employeeAD_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<% $ connectionStrings:ConString_HRSmart %>"
                            SelectCommand="SELECT LoginId='0', UserName='Please Select'
                                        UNION
                                        SELECT LoginId, UserName FROM AD_Users
                                        WHERE ActiveFlag = 'Y'"></asp:SqlDataSource>

                   

                    </div>--%>







                  <%--   <div class="col-lg-1">
                            <asp:Label ID="lbl_impid" runat="server" Visible="false" Font-Bold="True">Employee ID</asp:Label>
                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceID" runat="server" Visible="false" CssClass="form-control" placeholder="Employee ID" Text="" Enabled="true"></asp:TextBox>
                        </div>


                    
                        <div class="col-lg-1">

                            <asp:Label ID="lbl_empname" runat="server" Visible="False" Font-Bold="True">Employee Name</asp:Label>

                        </div>

                        <div class="col-lg-3 ">

                            <asp:TextBox ID="txt_empname" Visible="false" runat="server" CssClass="form-control" placeholder="Name" Font-Bold="False"></asp:TextBox>

                        </div>--%>
                </div>


                 <div class="row">
                    <div class="col-lg-4">
                       <%-- <label>Employee Name</label>--%>
                        <asp:Label ID="lbl_employee" runat="server" Visible="false" Font-Bold="True">Employee Name</asp:Label>
                    </div>
                </div>
                <div class="row">
                   

                     <div class="col-lg-4">

                        <asp:DropDownList ID="dd_employeeAD" runat="server" Visible="false" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource1" DataTextField="UserName"
                            DataValueField="LoginId" OnSelectedIndexChanged="dd_employeeAD_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<% $ connectionStrings:ConString_HRSmart %>"
                            SelectCommand="SELECT LoginId='0', UserName='Please Select'
                                        UNION
                                        SELECT LoginId, UserName FROM AD_Users
                                        WHERE ActiveFlag = 'Y' order by UserName"></asp:SqlDataSource>

                           <%-- SelectCommand="SELECT Dept_ID=0, Dept_Desc='Please Select'
                                        UNION
                                        SELECT Dept_ID, Dept_Desc FROM Department
                                        WHERE status = '1'"></asp:SqlDataSource>--%>

                    </div>







                  <%--   <div class="col-lg-1">
                            <asp:Label ID="lbl_impid" runat="server" Visible="false" Font-Bold="True">Employee ID</asp:Label>
                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceID" runat="server" Visible="false" CssClass="form-control" placeholder="Employee ID" Text="" Enabled="true"></asp:TextBox>
                        </div>


                    
                        <div class="col-lg-1">

                            <asp:Label ID="lbl_empname" runat="server" Visible="False" Font-Bold="True">Employee Name</asp:Label>

                        </div>

                        <div class="col-lg-3 ">

                            <asp:TextBox ID="txt_empname" Visible="false" runat="server" CssClass="form-control" placeholder="Name" Font-Bold="False"></asp:TextBox>

                        </div>--%>
                </div>




             <%--   <div class="form form-group">
                     <div class="row">
                       


                    <div class="col-lg-1 marginDiv">

                            <asp:Label ID="lbl_empemail" runat="server" Visible="False" Font-Bold="True">Employee Email
                            </asp:Label>

                        </div>

                        <div class="col-lg-3 marginDiv">

                            <asp:TextBox ID="txt_empemail" Visible="false" runat="server" CssClass="form-control" placeholder="Email" Font-Bold="False"></asp:TextBox>

                        </div>

                    <div class="col-lg-1 marginDiv">

                            <asp:Label ID="lbl_empnumber" runat="server" Visible="False" Font-Bold="True">Employee Number</asp:Label>

                    </div>

                        <div class="col-lg-3 marginDiv">

                            <asp:TextBox ID="txt_empnumber" Visible="false" runat="server" CssClass="form-control" placeholder="Number" Font-Bold="False"></asp:TextBox>

                        </div>

                        
                    <div class="col-lg-1 marginDiv">

                            <asp:Label ID="lbl_emppassword" runat="server" Visible="False" Font-Bold="True">Employee Password</asp:Label>

                        </div>

                        <div class="col-lg-3 marginDiv">

                            <asp:TextBox ID="txt_emppassword" Visible="false" TextMode="Password" runat="server" CssClass="form-control" placeholder="Password" Font-Bold="False"></asp:TextBox>

                        </div>


                </div>

                </div>--%>

               


                <div class="form form-group">
                </div>

                <asp:Button ID="btn_Edit" runat="server" Visible="false" Text="Update Employee" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Edit_Click" />
                <asp:Button ID="btn_Save" runat="server" Visible="false" Text="Save Employee" CssClass="btn btn-outline btn-success btn-lg btn-block" OnClick="btn_Save_Click" />

            </div>



            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" OnRowCommand="gv_ticketList_RowCommand" AutoGenerateColumns="False">
                <Columns>

                    <asp:BoundField DataField="user_id" HeaderText="Employee ID" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="user_name" HeaderText="Employee Name" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="emp_group" HeaderText="Group" SortExpression="Ticket_ID" />
                       <asp:TemplateField HeaderText="Remove Employee">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_editemp" CssClass="btn btn-primary" runat="server" Text="Edit" CommandName="editemp"></asp:LinkButton>
                        <asp:LinkButton ID="lb_employee" CssClass="btn btn-danger" runat="server" Text="Inactive" CommandName="remove"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>

    </div>

</asp:Content>

