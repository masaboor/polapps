<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ImplementerMaster.aspx.cs" Inherits="ServiceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    <div class="panel panel-info">
        <div class="panel-heading">
            Add new Implementer
                       
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
                        <label>Application Group</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-4">

                        <asp:DropDownList ID="dd_group" runat="server" Visible="true" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource2" DataTextField="Type_Desc"
                            DataValueField="Type_ID" OnSelectedIndexChanged="dd_group_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<% $ connectionStrings:ConString %>"
                            SelectCommand="SELECT Type_ID=0, Type_Desc='Please Select'
                                        UNION
                                        SELECT Type_ID, Type_Desc FROM Ticket_type
                                        WHERE status = '1'"></asp:SqlDataSource>

                    </div>
                </div>


                <div class="row">
                    <div class="col-lg-4">
                        <%--<label>Implementer Name</label>--%>
                        <asp:Label ID="lbl_impid" runat="server" Visible="False" Font-Bold="True">Implementer Name</asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-4">

                        <asp:DropDownList ID="txt_ServiceID" runat="server" Visible="false" CssClass="form-control" AutoPostBack="True"
                            DataSourceID="SqlDataSource4" DataTextField="Employee_name"
                            DataValueField="Employee_ID" OnSelectedIndexChanged="dd_group_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<% $ connectionStrings:ConString %>"
                            SelectCommand="SELECT Employee_ID='0', Employee_name='Please Select'
                                        UNION
                                        SELECT Employee_ID, Employee_name FROM Employee
                                        WHERE Employee_status = '1'"></asp:SqlDataSource>

                    </div>
                </div>

                <div class="form form-group">
                </div>

               <%-- <div class="row">
                        <div class="col-lg-2">
                            <asp:Label ID="lbl_impid" runat="server" Visible="false" Font-Bold="True">Implementer ID</asp:Label>
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

                <asp:Button ID="btn_Save" runat="server" Visible="false" Text="Save Implementer" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

            </div>



            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" OnRowCommand="gv_ticketList_RowCommand" AutoGenerateColumns="False">
                <Columns>

                    <asp:BoundField DataField="user_id" HeaderText="Implementer ID" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="user_name" HeaderText="Implementer Name" SortExpression="Ticket_ID" />
                    <asp:TemplateField HeaderText="Delete Implementer">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_deleteImp" CssClass="btn btn-danger" runat="server" Text="Remove" CommandName="show"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>


                </Columns>
            </asp:GridView>

        </div>

    </div>

</asp:Content>

