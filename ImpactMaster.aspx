<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ImpactMaster.aspx.cs" Inherits="ServiceMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="lbl_status" runat="server" CssClass="text-danger" Text="&nbsp;"></asp:Label>

    <div class="panel panel-info">
        <div class="panel-heading">
            Add new Impact Master
                       
        </div>
        <div class="panel-body">

            <div class="form-group">

                <div class="row">

                    <div class="col-lg-10">

                        <div class="col-lg-2">
                            <label>Impact Master ID</label>
                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceID" runat="server" CssClass="form-control" Text="" Enabled="false"></asp:TextBox>
                        </div>

                        <div class="col-lg-2">

                            <label>Impact Description</label>

                        </div>

                        <div class="col-lg-3">

                            <asp:TextBox ID="txt_ServiceDesc" runat="server" CssClass="form-control" placeholder="Impact Description"></asp:TextBox>

                        </div>

                    </div>

                </div>

                <asp:Button ID="btn_Save" runat="server" Text="Save Impact Master" CssClass="btn btn-outline btn-primary btn-lg btn-block" OnClick="btn_Save_Click" />

            </div>



            <asp:GridView ID="gv_ticketList" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                <Columns>

                    <asp:BoundField DataField="Impact_ID" HeaderText="Impact ID" SortExpression="Ticket_ID" />
                    <asp:BoundField DataField="Impact_Description" HeaderText="Impact Description" SortExpression="Ticket_ID" />


                </Columns>
            </asp:GridView>

        </div>

    </div>

</asp:Content>

