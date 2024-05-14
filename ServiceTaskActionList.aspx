<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServiceTaskActionList.aspx.cs" Inherits="TicketList" %>

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
                Search Services Pending for Action
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->
               <%-- <div class="col-lg-12">

                    <div class="form-group">

                        <div class="row">

                            <div class="col-lg-1">
                                <label>Service Profile</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Profile" runat="server" CssClass="form-control" AutoPostBack="false">

                                    <asp:ListItem Text="Assigned to me" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Open" Value="O"></asp:ListItem>

                                </asp:DropDownList>
                            </div>


                        </div>

                    </div>
                </div>--%>


                <div class="col-lg-12">

                    <div class="form-group">

                        <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
                            <Columns>
                                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                                <asp:TemplateField HeaderText="Service ID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Service_ID") %>' CommandName="Show"></asp:LinkButton>
                                        <asp:HiddenField ID="hd_ServiceTransID" runat="server" Value='<%# Eval("ServiceTrans_ID") %>' />
                                        <asp:HiddenField ID="hd_ImplementerEmail" runat="server" Value='<%# Eval("ImplementerEmail") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Ticket_ID" />

                                <asp:BoundField DataField="ServiceName" HeaderText="Service Name" SortExpression="Ticket_ID" />
                                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="Ticket_ID" />
                                <asp:BoundField DataField="RequestedFor" HeaderText="Requested For" SortExpression="Ticket_ID" />
                                <asp:BoundField DataField="team" HeaderText="Team" SortExpression="Ticket_ID" />
                                <asp:BoundField DataField="Implementer" HeaderText="Implementer" SortExpression="Ticket_ID" />



                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="dd_Action" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Pick Up" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="Pick Up and Start Working" Value="1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_Approve" runat="server" Text="Perform Action" CssClass="btn btn-success" OnClick="btn_Approve_Click" />
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

            </div>

        </div>
    </div>




</asp:Content>

