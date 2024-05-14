﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReasonMaster.aspx.cs" Inherits="ServiceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    <div class="panel panel-info">
        <div class="panel-heading">
            Add new Reason Master
                       
        </div>
        <div class="panel-body">

            <div class="form-group">

                <div class="row">

                    <div class="col-lg-10">

                        <div class="col-lg-2">
                            <label>Reason Master ID</label>
                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceID" runat="server" CssClass="form-control" Text="" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="col-lg-2">

                            <label>Reason Description</label>

                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceDesc" runat="server" CssClass="form-control" placeholder="Reason Description"></asp:TextBox>

                        </div>

                    </div>

                </div>

                <asp:Button ID="btn_Save" runat="server" Text="Save Reason Master" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

            </div>



            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                <Columns>

                    <asp:BoundField DataField="Reason_ID" HeaderText="Reason ID" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Reason_Description" HeaderText="Reason Description" SortExpression="Ticket_ID" />


                </Columns>
            </asp:GridView>

        </div>

    </div>

</asp:Content>

