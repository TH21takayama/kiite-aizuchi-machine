using System;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class VoiceForm : Form
    {
        public VoiceForm()
        {
            InitializeComponent();

            // コンボボックスを手入力可能に設定
            VoiceBox.DropDownStyle = ComboBoxStyle.DropDown;
            ToneBox.DropDownStyle = ComboBoxStyle.DropDown;

            // 声の選択肢
            VoiceBox.Items.Add("女性A"); // 母
            VoiceBox.Items.Add("女性B"); // 高山
            VoiceBox.Items.Add("男性A"); // 倉橋
            VoiceBox.Items.Add("男性B"); // 中谷

            // 会話スタイルの選択肢
            ToneBox.Items.Add("愚痴");
            ToneBox.Items.Add("自慢");
            ToneBox.Items.Add("汎用");
        }

        private void Next_Click(object sender, EventArgs e)
        {
            string selectedVoice = VoiceBox.Text.Trim();
            string selectedTone = ToneBox.Text.Trim();

            // 声の入力チェック
            if (string.IsNullOrEmpty(selectedVoice))
            {
                MessageBox.Show("声を選んでください。");
                return;
            }
            if (!VoiceBox.Items.Contains(selectedVoice))
            {
                MessageBox.Show("存在しない声が入力されています。");
                return;
            }

            // 会話スタイルの入力チェック
            if (string.IsNullOrEmpty(selectedTone))
            {
                MessageBox.Show("会話スタイルを選んでください。");
                return;
            }
            if (!ToneBox.Items.Contains(selectedTone))
            {
                MessageBox.Show("存在しない会話スタイルが入力されています。");
                return;
            }

            // MainFormのインスタンスを作成
            MainForm mainForm = new MainForm(this)
            {
                SelectedVoice = selectedVoice,
                SelectedTone = selectedTone
            };

            // MainFormを表示してVoiceFormは非表示
            mainForm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) // リセットボタン
        {
            VoiceBox.Text = "";
            ToneBox.Text = "";
        }

        private void button1_Click_1(object sender, EventArgs e) // 戻るボタン
        {
            // ログイン画面に戻る処理
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close(); // 現在のフォームを閉じる
        }
    }
}