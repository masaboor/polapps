<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServiceTaskApproval.aspx.cs" Inherits="TicketList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    </div>


    <%--<div class="col-lg-12">--%>
    <div class="panel panel-primary">
        <div class="panel-heading">
            Services Pending for Approval
                       
        </div>
        <div class="panel-body">
            <!-- /.row (nested) -->


            <%--<div class="col-lg-12">--%>

            <%-- <div class="form-group">--%>

            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
                <Columns>
                    <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                    <asp:TemplateField HeaderText="Service ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Service_ID") %>' CommandName="Show"></asp:LinkButton>
                            <asp:HiddenField ID="hd_ServiceTransID" runat="server" Value='<%# Eval("ServiceTrans_ID") %>' />
                            <asp:HiddenField ID="hd_ImplementerEmail" runat="server" Value='<%# Eval("ImplementerEmail") %>' />
                            <asp:HiddenField ID="hd_Team" runat="server" Value='<%# Eval("teamcode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Ticket_ID" />

                    <asp:BoundField DataField="ServiceName" HeaderText="Service Name" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="RequestedFor" HeaderText="Requested For" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="team" HeaderText="Team" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Implementer" HeaderText="Implementer" SortExpression="Ticket_ID" />


                    <asp:TemplateField HeaderText="Comments">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_Comments" runat="server" CssClass="form-control" placeholder="Approval Comments"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Approve">
                        <ItemTemplate>
                            <asp:Button ID="btn_Approve" runat="server" Text="Approve" CssClass="btn btn-success" OnClick="btn_Approve_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reject">
                        <ItemTemplate>
                            <asp:Button ID="btn_Disapprove" runat="server" Text="Reject" CssClass="btn btn-danger" OnClick="btn_Disapprove_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ApproveStatus" runat="server" CssClass="text-danger"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>

        </div>
    </div>

    <%--</div>--%>

    <%--</div>--%>
    <%--</div>--%>
</asp:Content>

