﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeActionEmergency.aspx.cs" Inherits="TicketList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    </div>

     <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">List of Emergency Changes</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <div class="form-group">

         <asp:GridView ID="gv_implement" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
            <Columns>
                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("ActionName") %>' CommandName='<%# Eval("ActionName") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Change_ID" HeaderText="Change ID" SortExpression="Change_ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Change_ID" />
                <asp:BoundField DataField="Task_ID" HeaderText="Task_ID" SortExpression="Change_ID" />
                <asp:BoundField DataField="Task_name" HeaderText="Task Name" SortExpression="Change_ID" />
                <asp:BoundField DataField="Implementer" HeaderText="Implementer" SortExpression="Change_ID" />
                <asp:BoundField DataField="start_date" HeaderText="Start Date" SortExpression="Change_ID" />
                <asp:BoundField DataField="end_date" HeaderText="End Date" SortExpression="Change_ID" />


            </Columns>
        </asp:GridView>

       <%-- <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
            <Columns>--%>
                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                <%--<asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("ActionName") %>' CommandName='<%# Eval("ActionName") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Change_ID" HeaderText="Change ID" SortExpression="Change_ID" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Change_ID" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Change_ID" />
                <asp:BoundField DataField="Requestor" HeaderText="Requestor" SortExpression="Change_ID" />
                <asp:BoundField DataField="Urgency" HeaderText="Urgency" SortExpression="Change_ID" />
                <asp:BoundField DataField="Approver" HeaderText="Approver" SortExpression="Change_ID" />
                <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Change_ID" />
                <asp:BoundField DataField="start_date" HeaderText="Start Date" SortExpression="Change_ID" />
                <asp:BoundField DataField="end_date" HeaderText="End Date" SortExpression="Change_ID" />
            </Columns>
        </asp:GridView>--%>



    </div>


</asp:Content>

