using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string user = txtUsername.Text;
            string pass = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;

            if (pass != confirm)
            {
                MessageBox.Show("パスワードが一致しません。");
                return;
            }

            if (DBHelper.RegisterUser(user, pass))
            {
                MessageBox.Show("登録完了ログインしてください。");
                LoginForm lf = new LoginForm();
                lf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("登録に失敗しました。ユーザー名が重複している可能性があります。");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Show();
            this.Hide();
        }
    }
}
