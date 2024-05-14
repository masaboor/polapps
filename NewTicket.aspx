<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NewTicket.aspx.cs" Inherits="NewTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_error" runat="server" visible="false" class="alert alert-danger">
           <asp:Label ID="lbl_error" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>      
        </div>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Ticket has been created Successfully! Ticket ID is 
            <asp:LinkButton ID="lbl_ticketID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ticketID_Click"></asp:LinkButton>
            <a href="Dashboard.aspx" class="alert-link">Go to Dashboard</a>.
                           
        </div>

    </div>



    <div id="div_main" runat="server" class="row">
        <div class="col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Create New Ticket
                       
                </div>
                <div class="panel-body">
                    <!-- /.row (nested) -->

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
                                    <label>Type</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_Type" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_Type_SelectedIndexChanged">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-2">
                                    <label>Sub Type</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_SubType" runat="server" CssClass="form-control" AutoPostBack="false">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>Service Recipent</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:TextBox ID="txt_recipent" runat="server" CssClass="form-control" AutoPostBack="false"></asp:TextBox>
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
                                    <label>Category</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_Category" runat="server" CssClass="form-control" AutoPostBack="false">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>



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
                                    <label>Owner/Requestor</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_Owner" runat="server" CssClass="form-control" AutoPostBack="false">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>


                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>Assignment Group</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_AssignDept" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_AssignDept_SelectedIndexChanged">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-lg-2">
                                    <label>Assign to Person</label>
                                </div>
                                <div class="col-lg-3">
                                    <asp:DropDownList ID="dd_AssignToPerson" runat="server" CssClass="form-control" AutoPostBack="false">
                                        <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>

                            </div>
                        </div>


                        <%--<div class="form-group">
                            <div class="row">
                                <div class="col-lg-2">
                                    <label>Attachment</label>
                                </div>
                                <div class="col-lg-8">
                                    <asp:FileUpload ID="fp_attachment" runat="server" CssClass="form-control" />
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
                                <div class="col-lg-10">

                                    <asp:Button ID="btn_Save" runat="server" Text="Create Ticket" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

                                </div>

                            </div>
                        </div>





                    </div>


                </div>
                <!-- /.panel-body -->
            </div>
            <!-- /.panel -->
        </div>
    </div>

</asp:Content>

