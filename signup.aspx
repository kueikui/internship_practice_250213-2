<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="test.signup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!-- jQuery（放在最前面）-->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>

    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css"/>
    <!-- Bootstrap 5 JavaScript (包含 Popper.js) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body style="background: linear-gradient(to top, #8ABDDC, #5192B7);">
    <form id="form1" runat="server">
        <div class="d-flex justify-content-center align-items-center vh-100">
            <div class="card shadow-lg p-4" style="width: 400px;">
                <div class="card-body text-center">
                    <div class="row g-3">
                        <div class="col-12">
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-info-circle"></i></span>
                                <asp:TextBox ID="TextBox1" runat="server" class="form-control" style="background-color:white" placeholder="username"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-person"></i></span>
                                <asp:TextBox ID="TextBox2" runat="server" class="form-control" style="background-color:white" placeholder="account"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-lock"></i></span>
                                <asp:TextBox ID="TextBox3" runat="server" class="form-control" style="background-color:white" placeholder="password"></asp:TextBox><br />
                            </div>
                        </div>
                    </div>
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="sign up" OnClick="Button1_Click" class="btn btn-success" ValidationGroup="sign up" />&nbsp
                    <asp:Button ID="Button2" runat="server" Text="back" OnClick="Button2_Click" class="btn btn-secondary"/>
                    <asp:Label ID="message" runat="server" Text="" ForeColor="#FF5050"></asp:Label><br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入姓名" ForeColor="#FF5050" ControlToValidate="TextBox1" ValidationGroup="sign up"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入帳號" ForeColor="#FF5050" ControlToValidate="TextBox2" ValidationGroup="sign up"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="請輸入密碼" ForeColor="#FF5050" ControlToValidate="TextBox3" ValidationGroup="sign up"></asp:RequiredFieldValidator>
                    <br />
                    
                </div>
            </div>
        </div>
    </form>
</body>
</html>
