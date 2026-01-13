using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private WaveOutEvent waveOut;
        private AudioFileReader audioReader;

        // 状態フラグ（Checked から同期）
        private bool isVoiceOn = true;
        private bool isImgeOn = true;

        private Dictionary<string, string> voiceFolderMap = new()
        {
            {"女性A","相槌_母"},
            {"女性B","相槌_高山"},
            {"男性A","相槌_倉橋"},
            {"男性B","相槌_中谷"}
        };

        public ChatBot(VoiceForm vf, string selectedVoice, string selectedTone)
        {
            InitializeComponent();

            voiceForm = vf;
            SelectedVoice = selectedVoice;
            SelectedTone = selectedTone;

            txtUserInput.KeyDown += TxtUserInput_KeyDown;
        }

        private void ChatBot_Load(object sender, EventArgs e)
        {
            // ===== メニュー初期化 =====
            音声ToolStripMenuItem.Checked = true;
            イラストToolStripMenuItem.Checked = true;

            音声ToolStripMenuItem.Click += 音声ToolStripMenuItem_Click;
            イラストToolStripMenuItem.Click += イラストToolStripMenuItem_Click;

            SyncFlagsFromMenu();

            Voice.Text = "音声-" + SelectedVoice;

            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            string voiceFolder = Path.Combine(baseFolder, voiceFolderMap[SelectedVoice]);

            cmbChatMode.Items.AddRange(
                Directory.GetDirectories(voiceFolder)
                .Select(Path.GetFileName)
                .ToArray());

            cmbChatMode.SelectedItem = SelectedTone ?? cmbChatMode.Items[0];
        }

        private void SyncFlagsFromMenu()
        {
            isVoiceOn = 音声ToolStripMenuItem.Checked;
            isImgeOn = イラストToolStripMenuItem.Checked;
        }

        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSend.PerformClick();
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserInput.Text)) return;

            rtbChatLog.AppendText($"{CurrentUser}: {txtUserInput.Text}\n");
            txtUserInput.Clear();

            await Task.Delay(500);
            PlayRandomAizuchi();
        }

        private void PlayRandomAizuchi()
        {
            SyncFlagsFromMenu();

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            string folder = Path.Combine(basePath, voiceFolderMap[SelectedVoice], SelectedTone);

            var files = Directory.GetFiles(folder)
                .Where(f => f.EndsWith(".wav") || f.EndsWith(".mp3"))
                .ToArray();

            if (files.Length == 0) return;

            string file = files[new Random().Next(files.Length)];
            string subtitle = Regex.Replace(Path.GetFileNameWithoutExtension(file), @"^\d+_", "");

            if (isVoiceOn)
            {
                waveOut?.Stop();
                audioReader?.Dispose();

                audioReader = new AudioFileReader(file);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioReader);
                waveOut.Play();
            }

            if (isImgeOn)
            {
                string iconFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\キャラアイコン");
                var icons = Directory.GetFiles(iconFolder, "*.png");

                if (icons.Length > 0)
                {
                    using var img = Image.FromFile(icons[new Random().Next(icons.Length)]);
                    CharaIcon.Image = new Bitmap(img, CharaIcon.Width, CharaIcon.Height);
                }
            }
            else
            {
                CharaIcon.Image = null;
            }

            rtbChatLog.AppendText($"{SelectedVoice}: {subtitle}\n");
        }

        // ===== メニューイベント =====
        private void 音声ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            音声ToolStripMenuItem.Checked = !音声ToolStripMenuItem.Checked;

            if (!音声ToolStripMenuItem.Checked)
                waveOut?.Stop();
        }

        private void イラストToolStripMenuItem_Click(object sender, EventArgs e)
        {
            イラストToolStripMenuItem.Checked = !イラストToolStripMenuItem.Checked;

            if (!イラストToolStripMenuItem.Checked)
                CharaIcon.Image = null;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            waveOut?.Stop();
            voiceForm.Show();
            Hide();
        }
    }
}
