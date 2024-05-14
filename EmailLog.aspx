<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmailLog.aspx.cs" Inherits="EmailLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


      <div class="row marginDiv">

         <iframe title="CIS - Ticket Management" width="1300" height="804" 
             src="https://app.powerbi.com/reportEmbed?reportId=30a30c09-4245-4e9b-9fcd-0176a3972680&autoAuth=true&ctid=18747df3-7996-4ef2-b0ae-94071ab9a5db" 
             frameborder="0" allowFullScreen="true"></iframe>
    </div>
    
      <%--  <div id="div_error" runat="server" visible="false" class="alert alert-danger">
           <asp:Label ID="lbl_error" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>      
        </div>

    <div style="max-width:100%">

   
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
                                <asp:DropDownList ID="dd_Application" runat="server" CssClass="form-control" AutoPostBack="true">
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

               

                <!-- ---------------------------------------------------------------------------------- -->



                <div class="col-lg-12">

                    <div class="form-group">
                        <div class="row">

                            <div class="col-lg-6">
                                <asp:Button ID="btn_search" runat="server" Text="Show Logs" CssClass="btn btn-success" OnClick="btn_search_Click" />

                            </div>

                        </div>
                    </div>
                </div>

            </div>

        </div>
    </div>
    <div class="form-group">

        <div class="  col-md-12 tab-responsive" style="overflow: auto; width: 100%; height: 400px;">

            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_ticketList_RowCommand">
                <Columns>


                    <asp:TemplateField HeaderText="Log ID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lb_ticket" runat="server" Text='<%# Eval("Email_Log_ID") %>' CommandName="Show"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="To_Address" HeaderText="To_Address" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Email_Subject" HeaderText="Email_Subject" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Sent_Flag" HeaderText="Sent_Flag" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="From_Application" HeaderText="From_Application" SortExpression="Ticket_ID" />

                    <asp:BoundField DataField="Confirmation_Msg" HeaderText="Confirmation_Msg" SortExpression="Ticket_ID" />

                    <asp:BoundField DataField="Created_Date" HeaderText="Created_Date" SortExpression="Ticket_ID" />
                </Columns>
            </asp:GridView>

        </div>




    </div>
         </div>--%>

</asp:Content>

