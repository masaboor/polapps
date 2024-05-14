<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Service.aspx.cs" Inherits="Service" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        
        <div id="div_error" runat="server" visible="false" class="alert alert-danger">
           <asp:Label ID="lbl_error" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>      
        </div>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Service Request has been created Successfully! Service ID is 
            <asp:LinkButton ID="lbl_ChangeID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ChangeID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

        <div id="div_main" runat="server" class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        Create New Service Request
                       
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
                                                <label>Title</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <%--<input class="form-control" placeholder="Enter Title">--%>
                                                <asp:TextBox ID="txt_Title" runat="server" CssClass="form-control" placeholder="Enter Title"></asp:TextBox>
                                                 <asp:RegularExpressionValidator Display = "Dynamic" ControlToValidate = "txt_Title" ID="RegularExpressionValidator1" ValidationExpression = "^[\s\S]{0,100}$" runat="server" ErrorMessage="Maximum 100 characters allowed."></asp:RegularExpressionValidator>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Requested By</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_ReportedBy" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                            <div class="col-lg-2">
                                                <label>Requested For</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ReportedFor" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reported for"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Reference #</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ReferenceNo" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reference Number"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Services Offered</label>
                                            </div>
                                            <div class="col-lg-5">
                                                <%-- <asp:ListBox ID="dd_ServiceAffected_" runat="server" CssClass="form-control" AutoPostBack="false" SelectionMode="Multiple">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:ListBox>--%>
                                                <div style="height: 200px; overflow: scroll">
                                                    <asp:CheckBoxList ID="dd_ServiceAffected" runat="server" AutoPostBack="false" RepeatDirection="Vertical"></asp:CheckBoxList>
                                                </div>


                                            </div>

                                            <div class="col-lg-2">
                                                <asp:Button ID="btn_Process" runat="server" class="btn btn-primary btn-lg" Text="Process" OnClick="btn_Process_Click"></asp:Button>
                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-10">
                                                <asp:GridView ID="gv_tasks" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                                                    <Columns>

                                                        <asp:BoundField DataField="Service_ID" HeaderText="Service ID" SortExpression="Ticket_ID" />
                                                        <asp:BoundField DataField="Service_Name" HeaderText="Service Name" SortExpression="Ticket_ID" />
                                                         <asp:TemplateField HeaderText="Team">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="dd_Team" runat="server" CssClass="form-control" AutoPostBack="true" onselectedindexchanged="dd_Team_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>

                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Assignee">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="dd_Assignee" runat="server" CssClass="form-control" AutoPostBack="false">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>

                                                                <asp:HiddenField ID="hd_Approver" runat="server" />
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
                                Service Detail
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">



                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Details</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txt_Details" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Details"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

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




                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-12">

                                    <asp:Button ID="btn_Save" runat="server" Text="Initiate Service Request" Enabled="false" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

                                </div>

                            </div>
                        </div>


                    </div>
                </div>


            </div>
        </div>


    </div>

</asp:Content>

