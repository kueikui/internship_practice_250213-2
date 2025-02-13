using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace test
{
    public partial class signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
                string name = TextBox1.Text.Trim();
                string account = TextBox2.Text.Trim();
                string password = TextBox3.Text.Trim();

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                {
                    message.Text = "所有欄位都必須填寫";
                    return;
                }

                string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM account WHERE uAccount = @account";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@account", account);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        message.Text = "帳號已存在";
                    }
                    else
                    {
                        message.Text = "";
                        string insertQuery = "INSERT INTO account (uName, uAccount, uPassword) VALUES (@name, @account, @password)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@name", name);
                            insertCmd.Parameters.AddWithValue("@account", account);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            insertCmd.ExecuteNonQuery();
                            string script = "alert('註冊資料成功！'); window.location='home.aspx';";
                            ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                            //Response.Redirect("login.aspx");

                        }
                    }
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("login.aspx");
        }
    }
}