using NAudio.Wave;
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
    public partial class ChatBot : Form
    {
        public string CurrentUser { get; set; }
        public string SelectedVoice { get; set; }
        public string SelectedTone { get; set; }

        private VoiceForm voiceForm;

        // コンストラクターで VoiceForm を受け取る
        public ChatBot(VoiceForm vf, string selectedVoice, string selectedTone)
        {
            InitializeComponent();
            voiceForm = vf; // これで null にならなくなる

            // VoiceFormで選択された値を受け取る
            SelectedVoice = selectedVoice;
            SelectedTone = selectedTone;
        }

        private WaveInEvent waveIn;
        private System.Timers.Timer silenceCheckTimer;
        private System.Timers.Timer responseDelayTimer;
        private WaveOutEvent waveOut;
        private AudioFileReader audioReader;

        private void StopListening()
        {
            waveIn?.StopRecording();
            waveIn?.Dispose();
            waveIn = null;

            silenceCheckTimer?.Stop();
            silenceCheckTimer?.Dispose();
            silenceCheckTimer = null;

            responseDelayTimer?.Stop();
            responseDelayTimer?.Dispose();
            responseDelayTimer = null;

            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();
        }

        private void ChatBot_Load(object sender, EventArgs e)
        {
            // まずは選択された声を取得
            if (string.IsNullOrEmpty(SelectedVoice))
            {
                MessageBox.Show("音声が選択されていません。");
                return;
            }

            // 音声フォルダのパス
            string baseFolder = @"C:\Users\osiky\source\repos\聞いて！相槌マシーン\聞いて！相槌マシーン\相槌";
            Dictionary<string, string> voiceFolderMap = new Dictionary<string, string>()
            {
                {"女性A","相槌_母"},
                {"女性B","相槌_高山"},
                {"男性A","相槌_倉橋"},
                {"男性B","相槌_中谷"}
            };


            if (!voiceFolderMap.ContainsKey(SelectedVoice))
                return;

            string voiceFolder = Path.Combine(baseFolder, voiceFolderMap[SelectedVoice]);

            if (!Directory.Exists(voiceFolder))
                return;

            // 音声ラベルに選択された声を表示
            Voice.Text = "音声-" + SelectedVoice;

            // 会話スタイルはサブフォルダ名
            string[] styleFolders = Directory.GetDirectories(voiceFolder);
            List<string> chatModes = styleFolders.Select(f => Path.GetFileName(f)).ToList();

            cmbChatMode.Items.Clear();
            cmbChatMode.Items.AddRange(chatModes.ToArray());

            // 初期選択は SelectedTone
            if (!string.IsNullOrEmpty(SelectedTone) && chatModes.Contains(SelectedTone))
            {
                cmbChatMode.SelectedItem = SelectedTone;
            }
            else if (chatModes.Count > 0)
            {
                cmbChatMode.SelectedIndex = 0;
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StopListening();
            voiceForm.Show();
            this.Hide();
        }

        private void rtbChatLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstChatHistory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtUserInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // TextBox の入力を取得してトリム
            string userText = txtUserInput.Text.Trim();

            // 空なら何もしない
            if (string.IsNullOrEmpty(userText)) return;

            // RichTextBox に表示（ユーザーの発言）
            rtbChatLog.AppendText($"あなた: {userText}\n");

            // TextBox をクリア
            txtUserInput.Clear();
        }

        private void cmbChatMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {

        }

        private void Voice_Click(object sender, EventArgs e)
        {

        }
    }
}
