<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeImplement.aspx.cs" Inherits="Change" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="vendor/metisMenu/metisMenu.min.js"></script>

    <!-- Custom Theme JavaScript -->
    <script src="dist/js/sb-admin-2.js"></script>

    <!-- Page-Level Demo Scripts - Notifications - Use for reference -->


    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Change has been updated Successfully! Change ID is 
            <asp:LinkButton ID="lbl_ChangeID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ChangeID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

        <div id="div_main" runat="server" class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-lg-2">
                                <%--<label>Ticket Status</label>--%>
                                <asp:Label ID="lbl_ChangeStatus" runat="server" Text="Ticket Status" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-lg-2">
                                <label>#</label>
                                <asp:Label ID="lbl_Change_ID" runat="server" Text="Ticket ID" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="col-lg-6">
                                <%--<label>Ticket Title</label>--%>
                                <asp:Label ID="lbl_Change_Title" runat="server" Text="Ticket Title" Font-Bold="true"></asp:Label>
                            </div>

                        </div>

                    </div>
                    <div class="panel-body">

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Change Header
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Change Requestor</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_ChangeRequestor" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Category</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Category" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_Category_SelectedIndexChanged">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Sub Category</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_SubCategory" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Scheduled Start</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_datefrom" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>

                                            </div>
                                            <div class="col-lg-2">
                                                <label>Scheduled End</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_dateto" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>

                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Service Affected</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <%-- <asp:ListBox ID="dd_ServiceAffected_" runat="server" CssClass="form-control" AutoPostBack="false" SelectionMode="Multiple">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:ListBox>--%>
                                                <div style="height: 200px; overflow: scroll">
                                                    <asp:CheckBoxList ID="dd_ServiceAffected" runat="server" AutoPostBack="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                </div>


                                            </div>
                                            <div class="col-lg-2">
                                                <label>Reason for Change </label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_ReasonForChange" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>




                                </div>

                            </div>

                        </div>
                        <!-- /.row (nested) -->
                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Change Detail
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Impact</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Impact" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Urgency</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Urgency" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Priority</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Priority" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Stage</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Stage" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Change Risk</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Risk" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2">
                                                <label>Reference Incident#</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_refIncident" runat="server" CssClass="form-control" AutoPostBack="false"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Is Vendor Involved</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_VendorInvolved" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2">
                                            </div>
                                            <div class="col-lg-3">
                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Change Plan</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txt_Details" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Details"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Backout Plan</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txt_BackoutPlan" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Backout Plan"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <%-- <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Attachment</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:LinkButton ID="lb_Attachment" runat="server" Text="No Attachment" CssClass="alert-link" OnClick="DownloadFile"></asp:LinkButton>
                                            </div>


                                        </div>
                                    </div>--%>


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

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Change Roles
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <div class="form-group">
                                        <div class="row">

                                            <div class="col-lg-2">
                                                <label>Change Approver 1</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Approver" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Change Approver 2</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Approver2" runat="server" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>




                                    <div class="form-group">
                                        <div class="row">

                                            <asp:GridView ID="gv_taskList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:BoundField DataField="Task_Implementer" HeaderText="Task Implementer" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Name" HeaderText="Task Name" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Description" HeaderText="Task Description" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Start" HeaderText="Task Start" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_End" HeaderText="Task End" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Status" HeaderText="Task Status" SortExpression="Task_ID" />
                                                </Columns>
                                            </asp:GridView>

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
                                        <asp:Label ID="lbl_TaskID" runat="server" Text="Ticket Status" Font-Bold="true"></asp:Label>
                                    </div>


                                </div>

                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <%--<div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Task Completion Attachment</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:FileUpload ID="fp_attachment" runat="server" CssClass="form-control" />
                                            </div>


                                        </div>
                                    </div>--%>

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
                                                <label>Task Result</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:DropDownList ID="dd_Result" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Success" Value="Success"></asp:ListItem>
                                                    <asp:ListItem Text="Failure" Value="Failure"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Task Completion Comments</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:TextBox ID="txt_ImplementationComments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Task Completion Comments" Enabled="true"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                     <div class="form-group">
                                        <div class="row">
                                            
                                            <div class="col-lg-2">
                                                <label>Scheduled End</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_closingdate" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>
                                                <ajaxx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_closingdate" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                                                <cc1:TimeSelector ID="TimeSelector4" runat="server">
                                                </cc1:TimeSelector>
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

