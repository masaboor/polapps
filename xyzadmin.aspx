<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xyzadmin.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        td, th {
            height: 25px;
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <b>Change ID</b>
            <br />
            <asp:TextBox ID="txt_change" runat="server" AutoPostBack="false"></asp:TextBox>
            <br />
            <asp:FileUpload ID="FileUpload1" runat="server" />

            <br />
            <b>Details</b>
            <br />
            <asp:TextBox ID="txt_details" runat="server" Height="103px" Width="340px"></asp:TextBox>
            <br />

            <asp:Label ID="lbl_status" runat="server"></asp:Label>
            <br />

            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="Upload" />

        </div>
    </form>
</body>
</html>
