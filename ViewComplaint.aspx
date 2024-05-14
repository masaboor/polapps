<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewComplaint.aspx.cs" Inherits="ViewTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Complaint has been Updated Successfully! Ticket ID is 
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
                            <asp:Label ID="lbl_TicketStatus" runat="server" Text="Complaint Status" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-2">
                            <label>#</label>
                            <asp:Label ID="lbl_Ticket_ID" runat="server" Text="Complaint ID" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-6">
                            <%--<label>Ticket Title</label>--%>
                            <asp:Label ID="lbl_Ticket_Title" runat="server" Text="Complaint Title" Font-Bold="true"></asp:Label>
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
                                    <b>Assigned By</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_OwnedBy" runat="server" Text="Assigned By"></asp:Label>
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
                                    <label>Requestor Impression</label>

                                </div>

                                <div class="col-lg-8">
                                    <asp:Label ID="lbl_requestorImpression" runat="server" Text=""></asp:Label>
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
                    Files Attached by Assignee                      
                </div>
                <div class="panel-body">

                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <%--<label>Ticket Status</label>--%>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_Status" Visible="false" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="Success" Value="Success"></asp:ListItem>
                                    <asp:ListItem Text="Failure" Value="Failure"></asp:ListItem>
                                </asp:DropDownList>
                            </div>


                        </div>
                    </div>


                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <%--<label>Status Change Comments</label>--%>
                            </div>
                            <div class="col-lg-8">
                                <asp:TextBox ID="txt_Comments" Visible="false" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Comments"></asp:TextBox>
                            </div>


                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <%-- <label>Status Attachment</label>--%>
                            </div>
                            <div class="col-lg-4">
                                <asp:FileUpload ID="fp_attachment" Visible="false" runat="server" CssClass="form-control" />
                            </div>
                            <div class="col-lg-4">
                                <asp:Button ID="btn_AddTaskAttachment" Visible="false" runat="server" Text="Add Status Attachment" CssClass="btn btn-primary" OnClick="btn_AddTaskAttachment_Click" />
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
                            <asp:Button ID="btn_Active" Visible="false" runat="server" Text="Update Status" CssClass="btn btn-primary" OnClick="btn_Active_Click" />
                            <asp:Button ID="btn_Resolve" Visible="false" runat="server" Text="Resolve Complaint" CssClass="btn btn-success" OnClick="btn_Resolve_Click" />
                        </div>
                    </div>


                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <%-- <label>Assign to Person</label>--%>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_AssignToPerson" Visible="false" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <asp:Button ID="btn_ReAssign" Visible="false" runat="server" Text="Re-Assign Complaint" CssClass="btn btn-info" OnClick="btn_ReAssign_Click" />

                </div>
            </div>

        </div>
    </div>


</asp:Content>

