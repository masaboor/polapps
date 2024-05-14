<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ViewEmail.aspx.cs" Inherits="ViewEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h3>Email Body</h3>

    <br />

    <div  class="form-group">

        <asp:Literal ID="lt_main" runat="server"></asp:Literal>

    </div>

</asp:Content>

