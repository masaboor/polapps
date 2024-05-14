<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ActiveChangeList.aspx.cs" Inherits="ActiveChangeList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 600px;
            height: 400px;
        }


    </style>

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    </div>


    <div class="col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Search Changes
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->


                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <%--<div class="col-lg-1">
                                <label> Change Status</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Profile" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                    <asp:ListItem Text="Assigned to me" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Created by me" Value="2"></asp:ListItem>

                                </asp:DropDownList>
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
                <!-- Added Hassam 29th-Aug-2022 -->
                <!-- ---------------------------------------------------------------------------------- -->

                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                           <%-- <div class="col-lg-1">
                                <label>Stage</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Stage" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

                                </asp:DropDownList>
                            </div>--%>


                           <%--  <div class="col-lg-1">
                                <label>Date From</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_datefrom" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_datefrom" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                            </div>--%>

                            <div class="col-lg-1">
                                <label>Requestor</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Requestor" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

                                </asp:DropDownList>
                            </div>

                     
                            <div class="col-lg-1">
                                <label>Change ID</label>
                            </div>
                            <div class="col-lg-3">
                                   <asp:TextBox ID="text_ChangeID" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Enter Change ID"></asp:TextBox>
                            </div>


                        </div>
                    </div>

                </div>



                <!-- ---------------------------------------------------------------------------------- -->



                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-6">
                                <asp:Button ID="btn_search" runat="server" Text="Show Changes" CssClass="btn btn-info" OnClick="btn_search_Click" />
                             <%--   <asp:Button ID="btn_download" runat="server" Text="Download Changes" CssClass="btn btn-success" OnClick="btn_download_Click" />--%>
                            </div>

                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>

    <div class="form-group">

        <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" OnRowCommand="gv_ticketList_RowCommand" AutoGenerateColumns="False" >
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
                <asp:TemplateField HeaderText="Pending With">
                    <ItemTemplate>
                        <asp:LinkButton ID="lb_PendingWith" runat="server" Text='Pending with..' CommandName="Pending"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="start_date" HeaderText="Start Date" SortExpression="Ticket_ID" />
                <asp:BoundField DataField="end_date" HeaderText="End Date" SortExpression="Ticket_ID" />
            </Columns>
        </asp:GridView>

        <div style="visibility: hidden">
            <asp:Button ID="btn_modal_hidden" runat="server" />
        </div>

        <ajaxx:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btn_modal_hidden"
            CancelControlID="btnClose" BackgroundCssClass="modalBackground">
        </ajaxx:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">

            <asp:GridView ID="gv_pendingWith" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">

                <Columns>
                    <asp:BoundField DataField="Role" HeaderText="Role" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Person" HeaderText="Person" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Ticket_ID" />
                </Columns>

            </asp:GridView>
            <br />
            <asp:Button ID="btnClose" runat="server" Text="Close" />
        </asp:Panel>

    </div>


</asp:Content>

