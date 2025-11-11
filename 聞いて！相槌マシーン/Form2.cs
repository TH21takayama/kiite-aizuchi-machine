using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace 聞いて_相槌マシーン
{
    public partial class VoiceForm : Form
    {
        private string username; // ログインしたユーザー名を保持
        public VoiceForm(string user)
        {
            InitializeComponent();

            username = user;

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

            // ✅ 前回設定を反映
            LoadPreviousSettings();
        }
        private void LoadPreviousSettings()
        {
            var (voice, tone) = DBHelper.LoadUserSettings(username);

            if (!string.IsNullOrEmpty(voice) && VoiceBox.Items.Contains(voice))
            {
                VoiceBox.Text = voice;
            }
            else
            {
                VoiceBox.Text = "声を選んでね";
            }

            if (!string.IsNullOrEmpty(tone) && ToneBox.Items.Contains(tone))
            {
                ToneBox.Text = tone;
            }
            else
            {
                ToneBox.Text = "会話スタイルをえらんでね";
            }
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

            // ★ 確認メッセージを追加（シンプル表示）★
            string message = $"声: {selectedVoice}\n会話スタイル: {selectedTone}\n\nこれでよろしいですか？";
            DialogResult result = MessageBox.Show(
                message,
                "",  // タイトルを空にしてスッキリ
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None  // アイコン非表示でシンプルに
            );

            if (result == DialogResult.Yes)
            {
                // DBに保存
                DBHelper.SaveUserSettings(username, selectedVoice, selectedTone);

                // MainFormに遷移
                MainForm mainForm = new MainForm(this)
                {
                    SelectedVoice = selectedVoice,
                    SelectedTone = selectedTone
                };
                mainForm.Show();
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e) // リセットボタン
        {
            VoiceBox.Text = "";
            ToneBox.Text = "";
        }

        private void VoiceForm_Load(object sender, EventArgs e)
        {

        }
    }
}
