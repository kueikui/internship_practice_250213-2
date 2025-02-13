<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="test.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <!-- Bootstrap 5 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
    <!-- Bootstrap5 icon-->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css"/>
    <!-- Bootstrap 5 JavaScript -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script> 
</head>
<body  style="background: linear-gradient(to top, #8ABDDC, #5192B7);"><!--class="bg-primary bg-gradient"-->
    <form id="form1" runat="server">
        <div class="d-flex justify-content-center align-items-center vh-100">
            <div class="card shadow-lg p-4" style="width: 400px;"><!--background-color: #b6d7ff;-->
                <div class="card-body text-center">
                    <div class="row g-3">
                        <div class="col-12">
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-person"></i></span>
                                <asp:TextBox ID="TextBox3" runat="server" class="form-control" style="background-color:white" placeholder="account"></asp:TextBox><br />
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="input-group">
                                <span class="input-group-text"><i class="bi bi-lock"></i></span>
                                <asp:TextBox ID="TextBox4" runat="server" class="form-control" Style="background-color: white" placeholder="password" TextMode="Password"></asp:TextBox><br />
                            </div>
                        </div>
                    </div>
                    <br />
                    <asp:Button ID="Button3" runat="server" Text="login" OnClick="login_Click" class="btn btn-info w-100" style="font-size:20px; font-weight: bold;" ValidationGroup="LoginValidation" />
                    <asp:Button ID="Button4" runat="server" Text="sign up" OnClick="sign_up_Click" class="btn btn-link"/><br />
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="#FF5050"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="請輸入帳號" ControlToValidate="TextBox3" ForeColor="#FF5050" ValidationGroup="LoginValidation"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入密碼" ControlToValidate="TextBox4" ForeColor="#FF5050" ValidationGroup="LoginValidation"></asp:RequiredFieldValidator>
                    <br />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
