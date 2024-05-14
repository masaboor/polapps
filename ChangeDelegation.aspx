<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChangeDelegation.aspx.cs" Inherits="ChangeDelegation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="form-group">

        <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

        <div class="col-lg-12">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    Create Change Approval Delegation
                       
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-2">
                                <label>Change Approver</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_ChangeApprover" runat="server" CssClass="form-control" AutoPostBack="false">
                                    <asp:ListItem Text="Please Select" Value="0"></asp:ListItem>

                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2">
                                <label>Change Delegated Approver</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:DropDownList ID="dd_ChangeNewApprover" runat="server" CssClass="form-control" AutoPostBack="false">
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


                            </div>
                            <div class="col-lg-2">
                                <label>Scheduled End</label>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ID="txt_dateto" runat="server" CssClass="form-control" AutoPostBack="false" placeholder="Ending Date"></asp:TextBox>
                                <ajaxx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_dateto" Format="dd/MM/yyyy"></ajaxx:CalendarExtender>

                            </div>

                        </div>
                    </div>

                    <div class="form-group">
                        <div class="row">
                            <div class="col-lg-12">

                                <asp:Button ID="btn_Save" runat="server" Text="Create Delegation" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

