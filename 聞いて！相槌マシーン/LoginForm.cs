using System;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            DBHelper.Initialize();
            btnLogin.Enabled = false; // 初期状態は無効
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void UpdateLoginButtonState()
        {
            // ユーザー名とパスワードが空でない場合のみログインボタンを有効化
            btnLogin.Enabled = !string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            if (DBHelper.AuthenticateUser(user, pass))
            {
                // ✅ ユーザー名をVoiceFormに渡す
                VoiceForm vf = new VoiceForm(user);
                vf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("ユーザー名またはパスワードが間違っています。");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm rf = new RegisterForm();
            rf.Show();
            this.Hide();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}