<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Depression.aspx.cs" Inherits="licenta_test.Depression" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="quizForm" runat="server">
        <div>
            <asp:PlaceHolder runat="server" ID="quizContainer"></asp:PlaceHolder>
            <asp:Button runat="server" ID="submitButton" Text="Submit" OnClick="submitButton_Click" CssClass="submit-button" />
        </div>
    </form>
</body>
</html>
