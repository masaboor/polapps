<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ActiveTaskList.aspx.cs" Inherits="ActiveTasktList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <style type="text/css">

        .marginDiv {
            margin-top: 25px;
        }

    </style>


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    </div>


    <div class="col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Search Tickets
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->


                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                          <%--  <div class="col-lg-1">
                                <label>Ticket Profile</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Profile" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Assigned to me" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Created by me" Value="2"></asp:ListItem>

                                </asp:DropDownList>
                            </div>--%>

                            <%--<div class="col-lg-1">
                                <label>Date From</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_datefrom" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_datefrom" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                            </div>--%>

                           <%-- <div class="col-lg-1">
                                <label>Date To</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_dateto" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_dateto" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                            </div>--%>
                        </div>
                    </div>
                </div>


                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-1">
                                <label>Assigned To</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_AssigendTo" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

                                </asp:DropDownList>
                            </div>

                            <div class="col-lg-1">
                                <label>Priority</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Priority" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

                                </asp:DropDownList>
                            </div>
                           <%-- <div class="col-lg-1">
                                <label>Category</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Category" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                </asp:DropDownList>
                            </div>--%>

                                                    <!-- Added Hassam 29th-Aug-2022 -->
                             <!-- ---------------------------------------------------------------------------------- -->


                             <div class="col-lg-1">
                                <label>Ticket ID</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="text_TicketID" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Enter Ticket ID"></asp:TextBox>
                            </div>

                            <!-- ---------------------------------------------------------------------------------- -->



                        </div>
                    </div>

                </div>

                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-6">
                                <asp:Button ID="btn_search" runat="server" Text="Show Tickets" CssClass="btn btn-info" OnClick="btn_search_Click" />
                               <%-- <asp:Button ID="btn_download" runat="server" Text="Download Tickets" CssClass="btn btn-success" OnClick="btn_download_Click" />--%>
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

               <%-- <asp:TemplateField HeaderText="Ticket ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Ticket_ID") %>' CommandName="Show"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>--%>

                  <asp:TemplateField HeaderText="Ticket ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Ticket_ID") %>' CommandName="Show"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="Ticket_ID" HeaderText="Title" SortExpression="Ticket_ID" />--%>
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Owner" HeaderText="Owner" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Assigned_to" HeaderText="Assigned To" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="SubType" HeaderText="Sub Type" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Created_Date" HeaderText="Created Date" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Updated_Date" HeaderText="Updated Date" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="Actiontime" HeaderText="Action Time" SortExpression="Ticket_ID" />
            </Columns>
        </asp:GridView>

    </div>


</asp:Content>

