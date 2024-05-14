<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewTicket_old.aspx.cs" Inherits="ViewTicket" %>

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
                                    <b>Status By</b>
                                </div>

                                <div class="col-lg-3">
                                    <%--<label>Ticket Title</label>--%>
                                    <asp:Label ID="lbl_StatusBy" runat="server" Text="Status By"></asp:Label>
                                </div>

                                <div class="col-lg-2">
                                    <b>Status Date</b>
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


                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>Status Change Comments</label>
                                </div>
                                <div class="col-lg-8">
                                    <asp:TextBox ID="txt_Comments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Comments"></asp:TextBox>
                                </div>


                            </div>
                        </div>


                    </div>
                </div>
                <div class="panel-footer">
                    <%--<button type="button" class="btn btn-success">Resolve</button>--%>
                    <asp:Button ID="btn_Active" runat="server" Text="Active" CssClass="btn btn-primary" OnClick="btn_Active_Click" />
                    <asp:Button ID="btn_Resolve" runat="server" Text="Resolve" CssClass="btn btn-success" OnClick="btn_Resolve_Click" />
                    <asp:Button ID="btn_MoreInfo" runat="server" Text="Request More Info" CssClass="btn btn-info" OnClick="btn_MoreInfo_Click" />
                    <asp:Button ID="btn_Close" runat="server" Text="Close" CssClass="btn btn-warning" OnClick="btn_Close_Click" />
                    <asp:Button ID="btn_ForceClose" runat="server" Text="Force Close" CssClass="btn btn-danger" OnClick="btn_ForceClose_Click" />
                    <%-- <button type="button" class="btn btn-info">Request More Info</button>
                    <button type="button" class="btn btn-warning">Close</button>
                    <button type="button" class="btn btn-danger">Force Close</button>--%>
                </div>
            </div>


            <div class="panel panel-danger">
                <div class="panel-heading">
                    Files:
                       
                </div>
                <div class="panel-body">
                    <asp:LinkButton ID="lb_Attachment" runat="server" Text="No Attachment" CssClass="alert-link" OnClick="DownloadFile"></asp:LinkButton>
                </div>
                <%-- <div class="panel-footer">
                    Panel Footer
                       
                </div>--%>
            </div>


            <div class="panel panel-green">
                <div class="panel-heading">
                    Activity                       
                </div>
                <div class="panel-body">

                    <asp:Literal ID="lt_Activity" runat="server"></asp:Literal>

                </div>
            </div>

        </div>
    </div>


</asp:Content>

