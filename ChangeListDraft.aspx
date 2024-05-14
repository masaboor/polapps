<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeListDraft.aspx.cs" Inherits="TicketList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    </div>


    <div class="col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                List of Change Draft
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->


                <div class="col-lg-12">

                    <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand" >
                        <Columns>
                            <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                            <asp:TemplateField HeaderText="Change ID">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Change_ID") %>' CommandName="Show"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="Requestor" HeaderText="Requestor" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="Approver1" HeaderText="Approver 1" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="Approver2" HeaderText="Approver 2" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="start_date" HeaderText="Start Date" SortExpression="Ticket_ID" />
                            <asp:BoundField DataField="end_date" HeaderText="End Date" SortExpression="Ticket_ID" />
                        </Columns>
                    </asp:GridView>

                </div>




            </div>

        </div>
    </div>


</asp:Content>

