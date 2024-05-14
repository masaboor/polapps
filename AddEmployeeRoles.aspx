<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEmployeeRoles.aspx.cs" Inherits="EmployeeRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    <div class="panel panel-info">
        <div class="panel-heading">
            Add Employee Role
                       
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
                    <div class="col-lg-2">
                            <asp:Label ID="lbl_EmployeeRole" runat="server" Visible="false" Font-Bold="True">Employee Role</asp:Label>
                       </div>
                </div>
                <div class="row">
                    <div class="col-lg-4">

                        <asp:DropDownList ID="dd_EmployeeRole" runat="server" Visible="false" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource2" DataTextField="RoleName"
                            DataValueField="Role_ID" OnSelectedIndexChanged="dd_EmployeeRole_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<% $ connectionStrings:ConString %>"
                            SelectCommand="SELECT Role_ID='0', RoleName='Please Select'
                                        UNION
                                        SELECT Role_ID, RoleName FROM UserRoles
                                        WHERE status = '1' and Role_ID <> '00'"></asp:SqlDataSource>

                    </div>
                </div>


                    <div class="row">
                    <div class="col-lg-4">
                        <asp:Label ID="lbl_EmployeeName" runat="server" Visible="false" Font-Bold="True">Employee Name</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-4">

                        <asp:DropDownList ID="dd_EmployeeName" runat="server" Visible="False" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource1" DataTextField="Employee_name"
                            DataValueField="Employee_ID" OnSelectedIndexChanged="dd_EmployeeName_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<% $ connectionStrings:ConString %>"
                            SelectCommand="SELECT Employee_ID='0', Employee_name='Please Select'
                                        UNION
                                        SELECT Employee_ID, Employee_name FROM Employee"></asp:SqlDataSource>

                    </div>
                </div>




                <div class="form form-group">
                </div>

              <%--  <div class="row">
                        <div class="col-lg-2">
                            <asp:Label ID="lbl_impid" runat="server" Visible="false" Font-Bold="True">Employee ID</asp:Label>
                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceID" runat="server" Visible="false" CssClass="form-control" Text="" Enabled="true"></asp:TextBox>
                        </div>

                        <div class="col-lg-2">

                            <asp:Label ID="lbl_impname" runat="server" Visible="False" Font-Bold="True">Implementer Name</asp:Label>

                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceDesc" Visible="false" runat="server" CssClass="form-control" placeholder="Name" Font-Bold="False"></asp:TextBox>

                        </div>

                </div>--%>


                <div class="form form-group">
                </div>

                <asp:Button ID="btn_Save" runat="server" Visible="false" Text="Assign Roles" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

            </div>



            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" OnRowCommand="gv_ticketList_RowCommand" AutoGenerateColumns="False">
                <Columns>

                    <asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" SortExpression="Employee_Details" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" SortExpression="Employee_Details" />
                    <asp:BoundField DataField="RoleName" HeaderText="Assigned Role" SortExpression="Employee_Details" />
                    <asp:BoundField DataField="RoleID" HeaderText="Role ID" Visible="true" SortExpression="Ticket_ID" />
                     <asp:TemplateField HeaderText="Remove Roles">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_remove" CssClass="btn btn-danger" runat="server" Text="Remove"  CommandName="show"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>


                </Columns>
            </asp:GridView>

        </div>

    </div>

</asp:Content>

