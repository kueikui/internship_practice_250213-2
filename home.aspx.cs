using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using NPOI.XSSF.UserModel;
using System.Web.Services.Description;
using MathNet.Numerics;
using System.Globalization;
using NPOI.SS.Formula.Functions;
using System.Web.DynamicData;
using NPOI.XWPF.UserModel;

namespace test
{
    public partial class home : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        int closefile = 0;//close
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["login"] == null && Session["clogin"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                if (Session["clogin"] != null)//是管理員登入
                {
                    Import.Visible = true;
                    Nav_Panel.Visible = true;
                    GridView1.Visible = true;
                    GridView2.Visible = false;
                }
                else//不是管理員
                {
                    Import.Visible= false;
                    Nav_Panel.Visible = false;
                    GridView2.Visible = true;
                    GridView1.Visible = false;
                }
                Add_Panel.Visible = false;              
                AddName_text.Text = "";
                AddAccount_text.Text = "";
                AddPassword_text.Text = "";
                AddPhone_text.Text = "";
                message1.Visible = false;
                message.Visible = false;
                //AddGender_text.Text = "";
                BindGridView();
            }
           
        }
        private void BindGridView()
        {           
            if (Session["login"] != null)//一般
            {
                string query = "SELECT * FROM account WHERE uAccount = @uAccount";
                string login_Account = Session["login"].ToString();

                SqlConnection conn = new SqlConnection(connStr);

                conn.Open();

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@uAccount", login_Account);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                GridView2.DataSource = dt;
                GridView2.DataBind();
            }
            else if (Session["clogin"] != null)//管理
            {
                SqlConnection conn = new SqlConnection(connStr);
                string query = "SELECT * FROM account";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            Session["login"] = null;
            Session["clogin"] = null;
            Response.Redirect("login.aspx");
        }

        protected void Export_Click(object sender, EventArgs e)
        { 
            if (ViewState["EditingRow"] != null)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('請先關閉編輯資料行');", true);
                return;
            }
            // 建立 Excel
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("帳號列表");

            DataTable dt = new DataTable();

            GridView targetGridView = null;

            if (Session["login"] != null)
            {
                targetGridView = GridView2;
            }
            else if (Session["clogin"] != null)
            {
                targetGridView = GridView1;
            }

            if (targetGridView != null && targetGridView.HeaderRow != null)
            {
                foreach (TableCell cell in targetGridView.HeaderRow.Cells)
                {
                    if (cell.Text != "&nbsp;")
                    {
                        dt.Columns.Add(cell.Text);
                    }
                }

                // 取得資料列
                foreach (GridViewRow row in targetGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        dr[columnIndex++] = (row.FindControl("lblName") as Label)?.Text.Trim();
                        dr[columnIndex++] = (row.FindControl("lblAccount") as Label)?.Text.Trim();
                        dr[columnIndex++] = (row.FindControl("lblPassword") as Label)?.Text.Trim();
                        dr[columnIndex++] = (row.FindControl("lblPhone") as Label)?.Text.Trim();
                        dr[columnIndex++] = (row.FindControl("lblGender") as Label)?.Text.Trim();

                        // 將 DataRow 加入 DataTable
                        dt.Rows.Add(dr);
                    }
                }
            }

            IRow headerRow = sheet.CreateRow(0);//title
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
            }

            for (int i = 0; i < dt.Rows.Count; i++)//data
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                }
            }
 

            // 將 Excel 寫入 MemoryStream 儲存 Excel 檔案的資料
            using (MemoryStream exportData = new MemoryStream())
            {   
                workbook.Write(exportData);
                workbook.Close();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ExportData.xlsx");
                Response.BinaryWrite(exportData.ToArray());
                Response.End();
            }
        }

        protected void Import_Click(object sender, EventArgs e)
        {
            Response.Redirect("import.aspx");
        }

        protected void AddPanel_btn_Click(object sender, EventArgs e)
        {
            Add_Panel.Visible = true;
            Nav_Panel.Visible = false;
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            string name = AddName_text.Text.Trim();
            string account = AddAccount_text.Text.Trim();
            string password = AddPassword_text.Text.Trim();
            string phone = AddPhone_text.Text.Trim();
            string gender = AddGender_list.Text;

            if (string.IsNullOrEmpty(name))
            {
                message.Text = "姓名為必填";
                return;
            }
            else if (string.IsNullOrEmpty(account))
            {
                message.Text = "帳號為必填";
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                message.Text = "密碼為必填";
                return;
            }
            if (phone.Length != 10)
            {
                message.Visible = true;
                message.Text = "電話格式錯誤";
                return ;
            }
            
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
                        message.Visible = true;
                        message.Text = "帳號已存在";
                    }
                    else
                    {
                        message.Visible = false;
                        message.Text = "";
                        string insertQuery = "INSERT INTO account (uName, uAccount, uPassword,uPhone,uGender) VALUES (@name, @account, @password,@phone,@gender)";
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                        {
                            insertCmd.Parameters.AddWithValue("@name", name);
                            insertCmd.Parameters.AddWithValue("@account", account);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            insertCmd.Parameters.AddWithValue("@phone", string.IsNullOrEmpty(phone) ? (object)DBNull.Value : phone);
                            insertCmd.Parameters.AddWithValue("@gender", string.IsNullOrEmpty(gender) ? (object)DBNull.Value : gender);

                            insertCmd.ExecuteNonQuery();
                            string script = "alert('新增成功！'); window.location='home.aspx';";
                            ClientScript.RegisterStartupScript(this.GetType(), "SuccessAlert", script, true);
                        }
                    }
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            Add_Panel.Visible = false;
            Nav_Panel.Visible = true;
            message.Visible=false;
            AddName_text.Text = "";
            AddAccount_text.Text = "";
            AddPassword_text.Text = "";
            AddPhone_text.Text = "";
            AddGender_list.SelectedIndex = 0;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            if (Search_text.Text=="")
            {
                message1.Visible = true;
                message1.Text = "請輸入搜尋內容";
                BindGridView();
                return;
            }
            if (Search_list.Text.ToString() == "Name")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    ShowAll_btn.Visible = true;

                    conn.Open();
                    string query = "SELECT * FROM account WHERE uName LIKE @searchText";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + Search_text.Text + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            message1.Visible = false;
                            GridView1.Visible = true;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            message1.Text = "";
                        }
                        else
                        {
                            GridView1.Visible = false;
                            message1.Visible = true;
                            message1.Text = "找不到符合的資料";
                        }
                    }
                }
            }
            else if (Search_list.Text.ToString() == "Account")
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    ShowAll_btn.Visible = true;

                    conn.Open();
                    string query = "SELECT * FROM account WHERE uAccount LIKE @searchText";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + Search_text.Text + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            message1.Visible = false;
                            GridView1.Visible = true;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            message1.Text = "";
                        }
                        else
                        {
                            GridView1.Visible = false;
                            message1.Visible = true;
                            message1.Text = "找不到符合的資料";
                        }
                    }
                }
            }
            else if(Search_list.Text.ToString() == "Select")
            {
                GridView1.Visible = false;
                message1.Visible = true;
                message1.Text = "請選搜尋類別";
            }
        }
        protected void ShowAll_Click(object sender, EventArgs e)
        {
            ShowAll_btn.Visible = false;
            Search_text.Text = "";
            message1.Visible = false;
            message1.Text = "";
            Search_list.SelectedIndex = 0;
            BindGridView();
        }
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ViewState["EditingRow"] = e.NewEditIndex;
            GridView targetGridView = null;

            if (Session["login"] != null)
            {
                targetGridView = GridView2;
            }
            else if (Session["clogin"] != null)
            {
                targetGridView = GridView1;
            }
            targetGridView.EditIndex = e.NewEditIndex;
            BindGridView();

            // 取得當前編輯的行
            GridViewRow row = targetGridView.Rows[e.NewEditIndex];

            // 設置 uAccount (帳號) 的 TextBox 為唯讀
            TextBox txtAccount = row.FindControl("txtAccount") as TextBox;
            if (txtAccount != null)
            {
                txtAccount.Enabled = false;  // 讓帳號欄位不能編輯
            }
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView targetGridView = null;
            ViewState["EditingRow"] = null;
            if (Session["login"] != null)
            {
                targetGridView = GridView2;
            }
            else if (Session["clogin"] != null)
            {
                targetGridView = GridView1;
            }
            targetGridView.EditIndex = -1;
            BindGridView();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView targetGridView = null;
            ViewState["EditingRow"] = null;
            if (Session["login"] != null)
            {
                targetGridView = GridView2;
            }
            else if (Session["clogin"] != null)
            {
                targetGridView = GridView1;
            }
            GridViewRow row = targetGridView.Rows[e.RowIndex];
            string uName = (row.FindControl("txtName") as TextBox).Text;
            string uAccount = (row.FindControl("txtAccount") as TextBox).Text;
            string uPassword = (row.FindControl("txtPassword") as TextBox).Text;
            string uPhone = (row.FindControl("txtPhone") as TextBox).Text;
            string uGender = (row.FindControl("ddlGender") as DropDownList).SelectedValue;

            // 驗證電話號碼格式
            string phonePattern = "(\\d{10})";
            if (uPhone!=""&&uPhone.Length != 10)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('電話號碼格式不正確');", true);
                e.Cancel = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(uPhone))
            {
                uPhone = null;
            }
            if (!string.IsNullOrEmpty(uPhone) && !System.Text.RegularExpressions.Regex.IsMatch(uPhone, phonePattern))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('電話號碼格式不正確');", true);
                e.Cancel = true;
                return;
            }
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE account SET uName = @uName, uPassword=@uPassword,  uPhone = @uPhone, uGender = @uGender WHERE uAccount = @uAccount";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@uName", uName);
                    cmd.Parameters.AddWithValue("@uAccount", uAccount);
                    cmd.Parameters.AddWithValue("@uPassword",uPassword);
                    cmd.Parameters.AddWithValue("@uPhone", (object)uPhone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@uGender", uGender);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            targetGridView.EditIndex = -1;
            BindGridView(); // 重新綁定 GridView
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView targetGridView = null;

            if (Session["login"] != null)
            {
                targetGridView = GridView2;
            }
            else if (Session["clogin"] != null)
            {
                targetGridView = GridView1;
            }

            int rowIndex = e.RowIndex;
            GridViewRow row = targetGridView.Rows[rowIndex];

            string uAccount = (row.FindControl("lblAccount") as Label).Text;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM account WHERE uAccount = @uAccount";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@uAccount", uAccount);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            BindGridView();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }
}

/*protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)//edit
        {
            GridView1.EditIndex = e.NewEditIndex;

            // 取得當前編輯行
            GridViewRow row = GridView1.Rows[e.NewEditIndex];

            BindGridView();

            // 假設 ID 欄位是第一欄（索引 0）
            TextBox txtID = (TextBox)row.FindControl("txtAccount");
            if (txtID != null)
            {
                txtID.Enabled = false;
            }
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)//update
        {
            // 取得編輯後的欄位值
            string name = e.NewValues["uName"].ToString().Trim();
            string password = e.NewValues["uPassword"].ToString().Trim();
            string account = e.NewValues["uAccount"].ToString().Trim();
            string phone = e.NewValues["uPhone"] != null ? e.NewValues["uPhone"].ToString().Trim() : "";
            string gender = e.NewValues["uGender"] != null ? e.NewValues["uGender"].ToString().Trim() : "";
            //e.OldValues
            
            // 驗證電話號碼格式
            string phonePattern = "((\\d{10})|(((\\(\\d{2}\\))|(\\d{2}-))?\\d{4}(-)?\\d{3}(\\d)?))";

            if (!string.IsNullOrEmpty(phone) && !System.Text.RegularExpressions.Regex.IsMatch(phone, phonePattern))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('電話號碼格式不正確');", true);
                e.Cancel = true;
                return;
            }
            SqlConnection conn = new SqlConnection(connStr);
            string query = "UPDATE account SET uName = @name, uPassword = @password ,uPhone=@phone ,uGender=@gender WHERE uAccount = @account";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@account", account);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@gender", gender);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            Response.Write("<script>alert('存入成功')</script>");
            GridView1.EditIndex = -1;
            BindGridView();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)//cancel
        {
            GridView1.EditIndex = -1;
            BindGridView();
        }*/

/* protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edity")
            { 
                
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = GridView1.Rows[index];
                //TableCell ac = selectedRow.Cells[3]; //tablecell沒有enable
                //ac.Enabled = false;
                TextBox a= new TextBox();
                //a = selectedRow.Cells[3].Enabled(false);
                TextBox txtBox = selectedRow.Cells[3].FindControl("TextBoxID") as TextBox;
                //ac.
                GridView1.EditIndex = index;
                BindGridView(); // 重新綁定數據
            }
            else if (e.CommandName == "deletey")
            {
                if (Session["clogin"] == null)
                {
                    Response.Write("<script>alert('您沒有權限刪除帳號');</script>");
                    return;
                }

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = GridView1.Rows[index];
                string account = selectedRow.Cells[3].Text;//account

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string query = "DELETE FROM account WHERE uAccount = @uAccount";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@uAccount", account);
                    cmd.ExecuteNonQuery();
                }
                BindGridView();
            }
        }*/