<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestInv.aspx.cs" Inherits="TestInv" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btn_PDFEmail" runat="server" Text="Convert HTML to PDF and Send Email with Attachment" OnClick="btn_PDFEmail_Click" /> 
    
    <asp:Button  runat="server" Text="String to PDF" OnClick="StringToPdf" />
    </div>
    </form>
</body>
</html>
