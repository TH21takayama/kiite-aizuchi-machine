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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            DBHelper.Initialize();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            if (DBHelper.AuthenticateUser(user, pass))
            {
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
    }
}
