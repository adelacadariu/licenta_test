<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Anxiety.aspx.cs" Inherits="licenta_test.Anxiety" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="ie=edge" />
    <title>Quick Quiz - Play</title>
    <link rel="stylesheet" href="quiz/app.css" />
    <link rel="stylesheet" href="quiz/game.css" />

    <style>
        .question-container {
            margin-bottom: 10px;
        }
        .choice-container {
            margin-left: 20px;
        }
        .submit-button {
            margin-top: 20px;
        }
    </style>
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
