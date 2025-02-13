using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
namespace test
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TextBox3.Text = "";
            TextBox4.Text = "";
        }

        protected void login_Click(object sender, EventArgs e)
        { // 取得使用者輸入
            string inputAccount = Request.Form["TextBox3"];
            string inputPassword = Request.Form["TextBox4"];

            // 取得連接字串
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            // 連接 SQL Server 並檢查帳號密碼
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM account WHERE uAccount = @account AND uPassword = @password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@account", inputAccount);
                    cmd.Parameters.AddWithValue("@password", inputPassword);

                    int userExists = (int)cmd.ExecuteScalar();

                    if (userExists > 0)
                    {          
                        Label1.Text = "";
                        Session["login"] = inputAccount;
                        Response.Redirect("home.aspx");
                        
                    }
                    else
                    {
                        string query2 = "SELECT COUNT(*) FROM Caccount WHERE cAccount = @cAccount AND cPassword = @cPassword";

                        using (SqlCommand cmd2 = new SqlCommand(query2, conn))
                        {
                            cmd2.Parameters.AddWithValue("@cAccount", inputAccount);
                            cmd2.Parameters.AddWithValue("@cPassword", inputPassword);

                            int userExists2 = (int)cmd2.ExecuteScalar();

                            if (userExists2 > 0)
                            {
                                Label1.Text = "";
                                Session["clogin"] = inputAccount;
                                Response.Redirect("home.aspx");

                            }
                            else
                            {
                                Label1.Text = "輸入錯誤";
                            }
                        }
                        Label1.Text = "輸入錯誤";
                    }
                }
            }
        }
        protected void sign_up_Click(object sender, EventArgs e)
        {
            Response.Redirect("signup.aspx");
        }
    }
}