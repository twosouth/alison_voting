<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="frmStart.aspx.vb" Inherits="SenateVoteOOB.frmStart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Names="Arial" ForeColor="#000066" 
                        Text="Start Display Board:"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:DropDownList ID="cboMode" runat="server" AutoPostBack="True" 
                        Font-Bold="False" Font-Size="Medium" ForeColor="Black" Height="30px" 
                        Width="150px">
                        <asp:ListItem Selected="True" Value="Production">Production</asp:ListItem>
                        <asp:ListItem>Test Mode</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Height="30px" Text="Go" 
                        Width="150px" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
