<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


     <style type="text/css">

        .marginDiv {
            margin-left: 100px;
        }

    </style>

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">Dashboard</h1>
        </div>
        <!-- /.col-lg-12 -->
    </div>

    <div class="row">

        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-ticket fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <%--<div class="huge">13</div>--%>
                            <asp:Label ID="lbl_ticketCount" runat="server" Text="0" CssClass="huge"></asp:Label>
                            <div>My Unresolved Tickets!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="TicketActionList.aspx">View UnResolved Tickets</a>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>


       <%-- <div class="col-lg-3 col-md-6">
            <div class="panel panel-yellow">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-plus-square fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <i class="fa fa-meh-o fa-4x"></i>

                            <div>Facing any Issue?</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">

                        <a class="pull-left" href="NewTicket.aspx">Create Ticket</a>

                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>--%>


        <div class="col-lg-3 col-md-6">
            <div class="panel panel-green">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-tasks fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <%--<div class="huge">12</div>--%>
                            <asp:Label ID="lbl_ActionsCount" runat="server" Text="0" CssClass="huge"></asp:Label>
                            <div>My Pending Actions!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="ChangeActionList.aspx">Change Actions</a>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>


        <div class="col-lg-3 col-md-6">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-rocket fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <%--<div class="huge">12</div>--%>
                            <asp:Label ID="lbl_ServiceCount" runat="server" Text="0" CssClass="huge"></asp:Label>
                            <div>My Pending Service Actions!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="ServiceActionList.aspx">Service Actions</a>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

        <div class="col-lg-3 col-md-6">
            <div class="panel panel-red">
                <div class="panel-heading">
                    <div class="row">
                        <div class="col-xs-3">
                            <i class="fa fa-tasks fa-5x"></i>
                        </div>
                        <div class="col-xs-9 text-right">
                            <%--<div class="huge">12</div>--%>
                            <asp:Label ID="lbl_EmerActionsCount" runat="server" Text="0" CssClass="huge"></asp:Label>
                            <div>Pending Emergency Actions!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="ChangeActionEmergency.aspx">Emergency Actions</a>
                        <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                        <div class="clearfix"></div>
                    </div>
                </a>
            </div>
        </div>

    </div>

  <%--  <div class="row">

        
    </div>--%>

    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
   


        <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
            <Columns>
                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>
               <%-- <asp:BoundField DataField="Serial No." HeaderText="Serial" SortExpression="Ticket_ID" />--%>
                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Ticket_ID" />
               <%-- <asp:BoundField DataField="TotalActiveTickets" HeaderText="Total Active Tasks" SortExpression="Ticket_ID" />--%>

               
                <asp:TemplateField HeaderText="Open Tickets">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("TotalActiveTickets") %>' CommandName="ShowTickets"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Pending for Implementation">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_change" runat="server" Text='<%# Eval("TotalActiveChanges") %>' CommandName="ShowChanges"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Open Services">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_service" runat="server" Text='<%# Eval("TotalActiveServices") %>' CommandName="ShowServices"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>


     <div class="row marginDiv">

         <iframe title="CIS - Ticket Management" width="1024" height="804" 
             src=https://app.powerbi.com/view?r=eyJrIjoiYjE5NzA1N2UtZTUzNi00NGVjLTljMjgtYmRjMDVjMjY4ZjNhIiwidCI6IjE4NzQ3ZGYzLTc5OTYtNGVmMi1iMGFlLTk0MDcxYWI5YTVkYiIsImMiOjl9&pageName=ReportSection 
             frameborder="0" allowFullScreen="true"></iframe>
       <%-- <div class="col-lg-6">

            <asp:Literal ID="ltScripts" runat="server"></asp:Literal>
            <div id="piechart_3d" style="width: 700px; height: 400px;">
            </div>
        </div>

         <div class="col-lg-6">

            <asp:Literal ID="ltScripts2" runat="server"></asp:Literal>
            <div id="piechart_3d2" style="width: 700px; height: 400px;">
            </div>
        </div>--%>
    </div>




</asp:Content>

