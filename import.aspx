<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="import.aspx.cs" Inherits="test.import" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
        <!-- 引入 Bootstrap 5 -->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet"/>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <br />

        <div class="d-flex justify-content-center align-items-center vh-10">
            <asp:FileUpload ID="formFile" runat="server" CssClass="form-control" style="width: 300px;" />&nbsp
            <asp:Button ID="Button1" runat="server" Text="import" CssClass="btn btn-primary" OnClick="Button1_Click" />
            <asp:Label ID="message" runat="server" Text="" Visible="False" ForeColor="#FF5050"></asp:Label>
        </div>
        <div class="d-flex justify-content-center align-items-center vh-10">
            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered table-primary text-white">
                <HeaderStyle CssClass="table-warning text-white" />
                <RowStyle CssClass="table-light" />
            </asp:GridView>
        </div>
        <div class="d-flex justify-content-center align-items-center vh-10">
            <asp:Button ID="back" runat="server" Text="back" CssClass="btn btn-primary" OnClick="Back_Click" />
        </div>
    </form>
</body>
</html>
