<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AuditLogExt.aspx.cs" Inherits="AuditLogExt" %>

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


        <div id="div_error" runat="server" visible="false" class="alert alert-danger">
           <asp:Label ID="lbl_error" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>      
        </div>




    <div class="col-lg-12">
        <div class="panel panel-primary">
            <div class="panel-heading">
                Generate Report
                       
            </div>
            <div class="panel-body">
                <!-- /.row (nested) -->


                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                             <div class="col-lg-1">
                                <label>Application Name</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Application" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_Application_SelectedIndexChanged">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

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
                <!-- Added Hassam 29th-Aug-2022 -->
                <!-- ---------------------------------------------------------------------------------- -->

               
                 <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <%-- <div class="col-lg-1">
                                <label>Form Name</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_pageName" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="All" Value="All"></asp:ListItem>

                                </asp:DropDownList>
                            </div>--%>

                        
                        </div>
                    </div>
                </div>

                <!-- ---------------------------------------------------------------------------------- -->



                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-6">
                                <asp:Button ID="btn_search" runat="server" Text="Show Logs" CssClass="btn btn-success" OnClick="btn_search_Click" />
                                <asp:Button ID="btn_download" runat="server" Text="Download Logs" CssClass="btn btn-info" OnClick="btn_download_Click" />
                            </div>

                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>


      <div class="tab-content">

        <asp:GridView ID="gv_log" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" Width="100%" AllowSorting="false">
            <Columns>
                <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>


                 <asp:BoundField DataField="Name" HeaderText="App Name" SortExpression="Name" />
                <%--<asp:BoundField DataField="ActionMethod" HeaderText="Method" SortExpression="ActionMethod" />--%>
                   <%-- <asp:TemplateField HeaderText="Controller/Form">
                        <ItemTemplate>
                            <%# string.Format("{0}{1}", Eval("Controller") ,Eval("Form"))%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
             <%--   <asp:BoundField DataField="Controller" HeaderText="Controller" SortExpression="Controller" />
                <asp:BoundField DataField="Form" HeaderText="Form" SortExpression="Form" />--%>
                <asp:BoundField DataField="HostName" HeaderText="Host Name" SortExpression="HostName" />
                <asp:BoundField DataField="IP" HeaderText="IP" SortExpression="IP" />
                <%--<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />--%>
                <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
                <asp:BoundField DataField="CreatedBy" HeaderText="Creator ID" SortExpression="CreatedBy" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" SortExpression="CreatedDate" />
                <asp:BoundField DataField="CreatedTime" HeaderText="Time" SortExpression="CreatedDate" />
                <asp:BoundField DataField="RecordID" HeaderText="Record ID" SortExpression="RecordID" />

              
            </Columns>
        </asp:GridView>

    </div>

    <div class="form-group">

        

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

