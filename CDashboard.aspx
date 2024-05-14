<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CDashboard.aspx.cs" Inherits="Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                            <div>Unresolved Complaints!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="TicketActionList.aspx">View UnResolved Complaints</a>
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
                            <div>Pending Actions!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="ChangeActionList.aspx">Complaint Actions</a>
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
                            <div>UnAssigned Complaints!</div>
                        </div>
                    </div>
                </div>
                <a href="#">
                    <div class="panel-footer">
                        <a class="pull-left" href="ServiceActionList.aspx">Assign Complaints</a>
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
    <div class="row">
        <div class="col-lg-6">

            <asp:Literal ID="ltScripts" runat="server"></asp:Literal>
            <div id="piechart_3d" style="width: 700px; height: 400px;">
            </div>
        </div>

         <div class="col-lg-6">

            <asp:Literal ID="ltScripts2" runat="server"></asp:Literal>
            <div id="piechart_3d2" style="width: 700px; height: 400px;">
            </div>
        </div>
    </div>


</asp:Content>

