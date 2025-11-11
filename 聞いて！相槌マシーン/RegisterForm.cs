using System;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 入力チェック
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("ユーザー名を入力してください。");
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("パスワードを入力してください。");
                return;
            }

            // 登録処理
            bool success = DBHelper.RegisterUser(username, password);
            if (success)
            {
                MessageBox.Show("登録が完了しました。ログインしてください。");
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("登録に失敗しました。ユーザー名が既に存在する可能性があります。");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}