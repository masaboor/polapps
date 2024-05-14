<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ServiceImplement.aspx.cs" Inherits="Change" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div id="div_success" runat="server" visible="false" class="alert alert-success">
            Service has been Implemented Successfully! Change ID is 
            <asp:LinkButton ID="lbl_ticketID" runat="server" Text="" CssClass="alert-link" OnClick="lbl_ticketID_Click"></asp:LinkButton>
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
                                                <asp:TextBox ID="txt_Title" runat="server" CssClass="form-control" placeholder="Enter Title" Enabled="false"></asp:TextBox>
                                            </div>

                                        </div>

                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Reported By</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_ReportedBy" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Reported By" Enabled="false"></asp:TextBox>
                                            </div>

                                            <div class="col-lg-2">
                                                <label>Priority</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Priority" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>


                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Impact</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Impact" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Urgency</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Urgency" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Stage</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Stage" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
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
                                Service Detail
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Category</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Category" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="dd_Category_SelectedIndexChanged" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Sub Category</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_SubCategory" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>




                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Services</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:ListBox ID="dd_ServiceAffected" runat="server" CssClass="form-control" AutoPostBack="false" SelectionMode="Multiple">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:ListBox>



                                            </div>

                                            <div class="col-lg-2">
                                                <label>Due in Days</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:TextBox ID="txt_datefrom" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false" placeholder="Due in Days"></asp:TextBox>



                                            </div>

                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Details</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:TextBox ID="txt_Details" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Details" Enabled="false"></asp:TextBox>
                                            </div>


                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Attachment</label>
                                            </div>
                                            <div class="col-lg-8">
                                                <asp:LinkButton ID="lb_Attachment" runat="server" Text="No Attachment" CssClass="alert-link" OnClick="DownloadFile"></asp:LinkButton>
                                            </div>


                                        </div>
                                    </div>



                                </div>
                            </div>
                        </div>

                        <div class="panel panel-info">
                            <div class="panel-heading">
                                Service Roles
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <div class="form-group">
                                        <div class="row">

                                            <div class="col-lg-2">
                                                <label>Change Approver</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Approver" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Change Implementer</label>
                                            </div>
                                            <div class="col-lg-3">
                                                <asp:DropDownList ID="dd_Implementer" runat="server" CssClass="form-control" AutoPostBack="false" Enabled="false">
                                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                                </asp:DropDownList>
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
                                Action
                       
                            </div>
                            <div class="panel-body">

                                <div class="col-lg-10">

                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-2">
                                                <label>Result</label>
                                            </div>
                                            <div class="col-lg-2">
                                                <asp:DropDownList ID="dd_Result" runat="server" CssClass="form-control" AutoPostBack="false">
                                                    <asp:ListItem Text="Success" Value="Success"></asp:ListItem>
                                                    <asp:ListItem Text="Failure" Value="Failure"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-lg-2">
                                                <label>Implementation Comments</label>
                                            </div>
                                            <div class="col-lg-4">
                                                <asp:TextBox ID="txt_ImplementationComments" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Enter Implementation Comments" Enabled="true"></asp:TextBox>
                                            </div>

                                        </div>
                                    </div>



                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-lg-7">
                                                <asp:Button ID="btn_Save" runat="server" Text="Mark as Implemented" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

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

