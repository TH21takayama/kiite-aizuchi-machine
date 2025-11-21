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
        public ChatBot(VoiceForm vf)
        {
            InitializeComponent();
            voiceForm = vf; // これで null にならなくなる
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
    }
}
