<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeEditing.aspx.cs" Inherits="Change" %>

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
    <script>
        // tooltip demo
        $('.tooltip-demo').tooltip({
            selector: "[data-toggle=tooltip]",
            container: "body"
        })
        // popover demo
        $("[data-toggle=popover]")
            .popover()
    </script>

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Change has been created Successfully! Change ID is 
            <asp:LinkButton ID="lbl_ChangeID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ChangeID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

        <div id="div_main" runat="server" class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Create New Change
                       
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
                                                <label>Title</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <%--<input class="form-control" placeholder="Enter Title">--%>
                                                <asp:TextBox ID="txt_Title" runat="server" CssClass="form-control" placeholder="Enter Title"></asp:TextBox>
                                            </div>

                                        </div>

                                    </div>

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
                                                <ajaxx:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_datefrom" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                                                <cc1:TimeSelector ID="TimeSelector3" runat="server">
                                                </cc1:TimeSelector>

                                            </div>
                                            <div class="col-lg-2">
                                                <label>Scheduled End</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_dateto" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>
                                                <ajaxx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_dateto" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                                                <cc1:TimeSelector ID="TimeSelector4" runat="server">
                                                </cc1:TimeSelector>
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
                                                <asp:DropDownList ID="dd_Urgency" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_Urgency_SelectedIndexChanged">
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

                                    <%--<div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Attachment</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:FileUpload ID="fp_attachment" runat="server" CssClass="form-control" />

                                            </div>
                                            <div class="col-lg-2">
                                                <asp:LinkButton ID="lb_Attachment" runat="server" Text="No Attachment" CssClass="alert-link" OnClick="DownloadFile"></asp:LinkButton>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Note: Selecting a new attachment will overwrite the previous one!</label>
                                            </div>

                                        </div>
                                    </div>--%>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Attachment</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:FileUpload ID="fp_attachment" runat="server" CssClass="form-control" />
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:Button ID="btn_AddAttachment" runat="server" Text="Add Attachment" CssClass="btn btn-primary" OnClick="btn_AddAttachment_Click" />
                                            </div>


                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-10">
                                                <asp:GridView ID="gv_AttachmentList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCommand="gv_AttachmentList_RowCommand">
                                                    <Columns>

                                                        <asp:BoundField DataField="AttachmentID" HeaderText="ID" SortExpression="Ticket_ID" />
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
                                                <label>Change Approver</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Approver" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>

                                                <asp:DropDownList ID="dd_Approver2" runat="server" Visible="false" CssClass="form-control" AutoPostBack="false">
                                                </asp:DropDownList>

                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <asp:Button ID="btn_modal" class="btn btn-primary btn-lg" data-toggle="modal" data-target="#myModal" Text="Add Action">Add Task</asp:Button>
                                            </div>
                                            <div class="col-lg-3">
                                            </div>



                                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                                                <div class="modal-dialog">
                                                    <div class="modal-content">
                                                        <div class="modal-header">
                                                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                                            <h4 class="modal-title" id="myModalLabel">Add Task for Change</h4>
                                                        </div>
                                                        <div class="modal-body">


                                                            <label>Implementer</label>

                                                            <asp:DropDownList ID="dd_Implementer" runat="server" CssClass="form-control" AutoPostBack="false">
                                                                <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                            </asp:DropDownList>

                                                            <label>Task Name</label>

                                                            <asp:TextBox ID="txt_TaskName" runat="server" CssClass="form-control" AutoPostBack="false"></asp:TextBox>

                                                            <label>Task Description</label>

                                                            <asp:TextBox ID="txt_TaskDescription" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Task Description"></asp:TextBox>

                                                            <label>Task Start Date</label>

                                                            <asp:TextBox ID="txt_TaskStart" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>
                                                            <ajaxx:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txt_TaskStart" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                                                            <cc1:TimeSelector ID="TimeSelector1" runat="server">
                                                            </cc1:TimeSelector>


                                                            <label>Task End Date</label>

                                                            <asp:TextBox ID="txt_TaskEnd" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Starting Date"></asp:TextBox>
                                                            <ajaxx:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txt_TaskEnd" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>
                                                            <cc1:TimeSelector ID="TimeSelector2" runat="server">
                                                            </cc1:TimeSelector>
                                                            <%--  <div class="form-group">
                                                                <div class="row">
                                                                    <div class="col-lg-2">
                                                                        <label>Implementer</label>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:DropDownList ID="dd_Implementer" runat="server" CssClass="form-control" AutoPostBack="false">
                                                                            <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                                        </asp:DropDownList>
                                                                    </div>

                                                                    <div class="col-lg-2">
                                                                        <label>Task Name</label>
                                                                    </div>
                                                                    <div class="col-lg-3">
                                                                        <asp:TextBox ID="txt_taskName" runat="server" CssClass="form-control" AutoPostBack="false"></asp:TextBox>
                                                                    </div>

                                                                </div>
                                                            </div>--%>
                                                        </div>
                                                        <div class="modal-footer">




                                                            <asp:Button ID="btn_CloseModal" class="btn btn-default" data-dismiss="modal">Cancel</asp:Button>
                                                            <asp:Button ID="btn_SaveAction" runat="server" class="btn btn-primary" Text="Save Action" OnClick="btn_SaveAction_Click"></asp:Button>
                                                        </div>
                                                    </div>
                                                    <!-- /.modal-content -->
                                                </div>
                                                <!-- /.modal-dialog -->
                                            </div>

                                        </div>
                                    </div>



                                    <div class="form-group">
                                        <div class="row">

                                            <asp:GridView ID="gv_taskList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                                                <Columns>
                                                    <%-- <asp:BoundField DataField="Ticket_ID" HeaderText="Ticket ID" SortExpression="Ticket_ID" />--%>
                                                    <asp:TemplateField HeaderText="Task ID">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hd_ImplementerID" runat="server" Value='<%# Eval("Implementer_ID") %>' />
                                                            <asp:Label ID="lbl_TaskID" runat="server" Text='<%# Eval("Task_ID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Task_Implementer" HeaderText="Task Implementer" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Name" HeaderText="Task Name" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Description" HeaderText="Task Description" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_Start" HeaderText="Task Start" SortExpression="Task_ID" />
                                                    <asp:BoundField DataField="Task_End" HeaderText="Task End" SortExpression="Task_ID" />

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
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-1">

                                    <asp:Button ID="btn_SaveDraft" runat="server" Visible="false" Text="Update Draft" CssClass="btn btn-primary btn-lg btn-info" OnClick="btn_SaveDraft_Click" />

                                </div>
                                <div class="col-lg-11">

                                    <asp:Button ID="btn_Save" runat="server" Text="Edit Change" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_SaveNew_Click" />

                                </div>

                            </div>
                        </div>


                    </div>
                </div>


            </div>
        </div>


    </div>

</asp:Content>

