<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServiceList.aspx.cs" Inherits="TicketList" %>

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
                Search Services
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->


                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-1">
                                <label>Service Profile</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Profile" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Assigned to me" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Created by me" Value="2"></asp:ListItem>

                                </asp:DropDownList>
                            </div>

                            <div class="col-lg-1">
                                <label>Date From</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_datefrom" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_datefrom" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                            </div>

                            <div class="col-lg-1">
                                <label>Date To</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_dateto" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_dateto" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                            </div>
                        </div>
                    </div>
                </div>



                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-6">
                                <asp:Button ID="btn_search" runat="server" Text="Show Services" CssClass="btn btn-info" OnClick="btn_search_Click" />
                                <asp:Button ID="btn_download" runat="server" Text="Download Services" CssClass="btn btn-success" OnClick="btn_download_Click" />
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>

    <div class="form-group">

        <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
            <Columns>
                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>

                <asp:TemplateField HeaderText="Service ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Service_ID") %>' CommandName="Show"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Ticket_ID" />
                <%--<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Ticket_ID" />--%>
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="RequestedBy" HeaderText="Requested By" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="RequestedFor" HeaderText="Requested For" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="TotalServices" HeaderText="Total Services Offered" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="CompletedServices" HeaderText="Services Completed" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Submit_Date" HeaderText="Submit Date" SortExpression="Ticket_ID" />

                <%--<asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Ticket_ID" />--%>
            </Columns>
        </asp:GridView>

    </div>


</asp:Content>

