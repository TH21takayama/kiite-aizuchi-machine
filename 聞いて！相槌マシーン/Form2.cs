using System;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    // 声と会話スタイルの選択画面
    public partial class VoiceForm : Form
    {
        private string currentUser; // ✅ ログイン画面から受け取ったユーザー名を保持

        public VoiceForm(string username)
        {
            InitializeComponent();
            currentUser = username;

            // ✅ ユーザー名を表示するラベル
            UserLabel.Text = $"ユーザー：{currentUser}";

            // コンボボックスを手入力可能に設定
            VoiceBox.DropDownStyle = ComboBoxStyle.DropDown;
            ToneBox.DropDownStyle = ComboBoxStyle.DropDown;

            // 声の選択肢
            VoiceBox.Items.AddRange(new string[] { "女性A", "女性B", "男性A", "男性B" });

            // 会話スタイルの選択肢
            ToneBox.Items.AddRange(new string[] { "愚痴", "自慢", "汎用" });

            // ✅ 前回選択を復元
            var settings = DBHelper.GetUserSettings(currentUser);
            if (!string.IsNullOrEmpty(settings.Voice)) VoiceBox.Text = settings.Voice;
            if (!string.IsNullOrEmpty(settings.Tone)) ToneBox.Text = settings.Tone;


        }

        private void Next_Click(object sender, EventArgs e)
        {
            string selectedVoice = VoiceBox.Text.Trim();
            string selectedTone = ToneBox.Text.Trim();

            // 声の入力チェック
            if (string.IsNullOrEmpty(selectedVoice) || !VoiceBox.Items.Contains(selectedVoice))
            {
                MessageBox.Show("声を正しく選んでください。");
                return;
            }

            // 会話スタイルの入力チェック
            if (string.IsNullOrEmpty(selectedTone) || !ToneBox.Items.Contains(selectedTone))
            {
                MessageBox.Show("会話スタイルを正しく選んでください。");
                return;
            }

            // ✅ DBに保存（字幕・画像はMainFormで更新）
            DBHelper.SaveUserSettings(currentUser, selectedVoice, selectedTone, true, true);

            // ✅ MainFormにユーザー名を渡す
            MainForm mainForm = new MainForm(this, currentUser)
            {
                SelectedVoice = selectedVoice,
                SelectedTone = selectedTone
            };

            mainForm.Show();
            this.Hide();
        }

        private void buttonReset_Click(object sender, EventArgs e) // リセットボタン
        {
            // DB を初期化（ユーザーの Voice, Tone, JimakuOn, ImageOn をリセット）
            DBHelper.ResetUserSettings(currentUser);

            // UIを初期状態に戻す
            VoiceBox.Text = "声を選んでね";
            ToneBox.Text = "会話スタイルをえらんでね";
        }

        private void button1_Click(object sender, EventArgs e) // 戻るボタン
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close(); // 現在のフォームを閉じる
        }

        private void VoiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}