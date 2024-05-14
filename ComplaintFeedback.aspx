<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ComplaintFeedback.aspx.cs" Inherits="ViewTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Ticket has been Updated Successfully! Ticket ID is 
            <asp:LinkButton ID="lbl_ticketID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ticketID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

    </div>


    <div id="div_main" runat="server" class="row">
        <div class="col-lg-12">

            <div class="panel panel-primary">
                <div class="panel-heading">


                    <div class="row">
                        <div class="col-lg-2">
                            <%--<label>Ticket Status</label>--%>
                            <asp:Label ID="lbl_TicketStatus" runat="server" Text="Ticket Status" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-2">
                            <label>#</label>
                            <asp:Label ID="lbl_Ticket_ID" runat="server" Text="Ticket ID" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-6">
                            <%--<label>Ticket Title</label>--%>
                            <asp:Label ID="lbl_Ticket_Title" runat="server" Text="Ticket Title" Font-Bold="true"></asp:Label>
                        </div>

                    </div>

                </div>
                <div class="panel-body">
                    <div class="col-lg-10">

                        <div class="form-group">
                            <div class="row">

                                <div class="col-lg-7">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_Description" runat="server" Text="High Priority Software Request" Font-Bold="true"></asp:Label>
                                </div>




                                <div class="col-lg-1">
                                    <label>Type</label>
                                </div>
                                <div class="col-lg-2">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_SubType" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </div>

                            </div>
                        </div>



                        <div class="form-group">
                            <div class="row">

                                <div class="col-lg-2">
                                    <b>Assigned To</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_AssignedToo" runat="server" Text="Assigned To"></asp:Label>
                                </div>

                                <div class="col-lg-2">
                                    <b>Owned By</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_OwnedBy" runat="server" Text="Assigned To"></asp:Label>
                                    <asp:HiddenField ID="hd_ownerEmail" runat="server" />
                                </div>

                            </div>
                        </div>




                        <div class="form-group">
                            <div class="row">

                                <div class="col-lg-2">
                                    <b>Created By</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_CreatedBy" runat="server" Text="Created By"></asp:Label>
                                </div>

                                <div class="col-lg-2">
                                    <b>Created Date</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_CreatedDate" runat="server" Text="Created Date"></asp:Label>
                                </div>

                            </div>
                        </div>



                        <div class="form-group">
                            <div class="row">

                                <div class="col-lg-2">
                                    <b>Last Status By</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_StatusBy" runat="server" Text="Status By"></asp:Label>
                                </div>

                                <div class="col-lg-2">
                                    <b>Last Status Date</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_StatusDate" runat="server" Text="Status Date"></asp:Label>
                                </div>

                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">

                                <div class="col-lg-2">
                                    <label>Details</label>

                                </div>

                                <div class="col-lg-8">
                                    <asp:Label ID="lbl_Details" runat="server" Text="Details of Ticket by Creator"></asp:Label>
                                </div>


                            </div>
                        </div>




                    </div>
                </div>

            </div>


            <div class="panel panel-danger">
                <div class="panel-heading">
                    Files Attached by Requestor:
                       
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-10">
                                <asp:GridView ID="gv_AttachmentList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_AttachmentList_RowCommand">
                                    <Columns>

                                        <asp:BoundField DataField="Attachment_ID" HeaderText="ID" SortExpression="Ticket_ID" />
                                        <asp:BoundField DataField="filename" HeaderText="File Name" SortExpression="Ticket_ID" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lb_show" runat="server" Text='Download' CommandName="down"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <%-- <div class="panel-footer">
                    Panel Footer
                       
                </div>--%>
            </div>


            <div class="panel panel-green">
                <div class="panel-heading">
                    Activity Log                       
                </div>
                <div class="panel-body">

                    <asp:Literal ID="lt_Activity" runat="server"></asp:Literal>

                </div>
            </div>


            <div class="panel panel-red">
                <div class="panel-heading">
                    Action                      
                </div>
                <div class="panel-body">

                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <label>Requestor Impression</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Status" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="Satisfied" Value="S"></asp:ListItem>
                                    <asp:ListItem Text="Not Satisfied" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </div>


                        </div>
                    </div>


                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <label>Requestor Feedback</label>
                            </div>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txt_Comments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Feedback"></asp:TextBox>
                            </div>


                        </div>
                    </div>



                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-6">
                                <asp:GridView ID="gv_TaskAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_TaskAttachment_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="Employee_ID" HeaderText="Employee ID" SortExpression="Ticket_ID" />
                                        <asp:BoundField DataField="Employee_Name" HeaderText="Employee Name" SortExpression="Ticket_ID" />
                                        <asp:BoundField DataField="AttachmentID" HeaderText="Attachment ID" SortExpression="Ticket_ID" />
                                        <asp:BoundField DataField="AttachmentName" HeaderText="File Name" SortExpression="Ticket_ID" />
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lb_show" runat="server" Text='Download' CommandName="down"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="panel-footer">


                    <div class="form-group">
                        <div class="row">
                            <asp:Button ID="btn_Active" runat="server" Text="Update Feedback" CssClass="btn btn-primary" OnClick="btn_Active_Click" />
                        </div>
                    </div>


                </div>
            </div>

        </div>
    </div>


</asp:Content>

