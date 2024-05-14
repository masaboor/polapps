<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServiceAction.aspx.cs" Inherits="Service" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Service has been updated Successfully! Change ID is 
            <asp:LinkButton ID="lbl_ChangeID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ChangeID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

        <div id="div_main" runat="server" class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Action on Service Request
                       
                    </div>
                    <div class="panel-body">

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Service Header
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <div class="form-group">

                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Service ID</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ServiceID" Enabled="false" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Service ID"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="form-group">

                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Title</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <%--<input class="form-control" placeholder="Enter Title">--%>
                                                <asp:TextBox ID="txt_Title" Enabled="false" runat="server" CssClass="form-control" placeholder="Enter Title"></asp:TextBox>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Reported By</label>
                                            </div>
                                            <div class="col-lg-3">

                                                <asp:TextBox ID="dd_ReportedBy" Enabled="false" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reported for"></asp:TextBox>
                                            </div>

                                            <div class="col-lg-2">
                                                <label>Reported For</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ReportedFor" Enabled="false" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reported for"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Reference #</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ReferenceNo" Enabled="false" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reference Number"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Services Offered</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <%-- <asp:ListBox ID="dd_ServiceAffected_" runat="server" CssClass="form-control" AutoPostBack="false" SelectionMode="Multiple">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:ListBox>--%>
                                                <div style="height: 200px; overflow: scroll">
                                                    <asp:CheckBoxList ID="dd_ServiceAffected" Enabled="false" runat="server" AutoPostBack="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                </div>


                                            </div>



                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Details</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txt_Details" Enabled="false" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Details"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>


                                    <br />






                                </div>

                            </div>

                        </div>

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Service Initiator Attachments
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">


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
                            </div>
                        </div>



                        <!-- /.row (nested) -->
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Service List of Tasks
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-10">
                                                <asp:GridView ID="gv_tasks" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                                                    <Columns>

                                                        <asp:BoundField DataField="Service_ID" HeaderText="Service ID" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="Service_Name" HeaderText="Service Name" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="Team" HeaderText="Team" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="Implementer" HeaderText="Implementer" SortExpression="Ticket_ID" />


                                                        <asp:BoundField DataField="Status" HeaderText="Task Status" SortExpression="Task_ID" />

                                                        <asp:TemplateField HeaderText="Progress %">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hd_percent" runat="server" Value='<%# Eval("Percent") %>' />
                                                                <asp:Literal ID="lt_percent" runat="server"></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Attachment">
                                                            <ItemTemplate>
                                                                <%--<asp:LinkButton ID="lb_TaskAttachment" runat="server" Text='<%# Eval("filename") %>' CommandName="Attachh"></asp:LinkButton>--%>

                                                                <asp:DropDownList ID="dd_taskAttachment" runat="server" CssClass="form-control" AutoPostBack="false" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Download">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lb_TaskAttachment" runat="server" Text='Download' CommandName="Downnn"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>




                                </div>
                            </div>
                        </div>



                        <div class="panel panel-green">
                            <div class="panel-heading">
                                Activity                       
                            </div>
                            <div class="panel-body">

                                <asp:Literal ID="lt_Activity" runat="server"></asp:Literal>

                            </div>
                        </div>


                        <div class="panel panel-info">
                            <div class="panel-heading">


                                <div class="row">
                                    <div class="col-lg-10">
                                        <label>Action for Task</label>
                                        <asp:Label ID="lbl_TaskID" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    </div>


                                </div>

                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">



                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Task Progress Attachment</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:FileUpload ID="fp_attachment" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:Button ID="btn_AddTaskAttachment" runat="server" Text="Add Task Attachment" CssClass="btn btn-primary" OnClick="btn_AddTaskAttachment_Click" />
                                            </div>


                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-6">
                                                <asp:GridView ID="gv_TaskAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_TaskAttachment_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="Task_ID" HeaderText="Task ID" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="AttachmentID" HeaderText="Attachment ID" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="AttachmentName" HeaderText="File Name" SortExpression="Ticket_ID" />
                                                        <asp:TemplateField HeaderText="Download">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lb_show" runat="server" Text='Download' CommandName="down"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lb_delete" runat="server" Text='Delete' CommandName="Del"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Task Status</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:DropDownList ID="dd_Status" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Task Update Comments</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:TextBox ID="txt_ImplementationComments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Task Status Comments" Enabled="true"></asp:TextBox>
                                            </div>

                                        </div>

                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Task Progress in %</label>
                                            </div>
                                            <div class="col-lg-1">
                                                <asp:TextBox ID="txt_Progress" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="%"></asp:TextBox>
                                            </div>
                                        </div>

                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-10">
                                                <asp:Button ID="btn_Save" runat="server" Text="Update Task" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

                                            </div>
                                        </div>
                                    </div>




                                </div>
                            </div>
                        </div>



                    </div>
                </div>


            </div>
        </div>


    </div>

</asp:Content>

