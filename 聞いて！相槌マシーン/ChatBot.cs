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

            if (string.IsNullOrWhiteSpace(CurrentUser))
                CurrentUser = "default_user";

            txtUserInput.KeyDown += TxtUserInput_KeyDown;
        }

        private void ChatBot_Load(object sender, EventArgs e)
        {
            btnSaveLog.Click += btnSaveLog_Click;
            btnClear.Click += btnClear_Click;
            lstChatHistory.SelectedIndexChanged += lstChatHistory_SelectedIndexChanged;

            音声ToolStripMenuItem.Click += 音声ToolStripMenuItem_Click;
            イラストToolStripMenuItem.Click += イラストToolStripMenuItem_Click;

            音声ToolStripMenuItem.Checked = true;
            イラストToolStripMenuItem.Checked = true;

            rtbChatLog.ReadOnly = true;
            rtbChatLog.TabStop = false;

            Voice.Text = "音声-" + SelectedVoice;

            LoadChatHistoryList();
            LoadChatModes();
        }

        private void LoadChatModes()
        {
            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            string voiceFolder = Path.Combine(baseFolder, voiceFolderMap[SelectedVoice]);

            cmbChatMode.Items.Clear();
            cmbChatMode.Items.AddRange(
                Directory.GetDirectories(voiceFolder)
                .Select(Path.GetFileName)
                .ToArray());

            cmbChatMode.SelectedItem = SelectedTone ?? cmbChatMode.Items[0];
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

            AppendChat($"{CurrentUser}: {txtUserInput.Text}");
            txtUserInput.Clear();

            await Task.Delay(500);
            PlayRandomAizuchi();
        }

        // ★ 共通表示処理（自動スクロール）
        private void AppendChat(string text)
        {
            rtbChatLog.AppendText(text + Environment.NewLine);
            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.ScrollToCaret();
        }

        private void PlayRandomAizuchi()
        {
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

            AppendChat($"{SelectedVoice}: {subtitle}");
        }

        private void 音声ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isVoiceOn = !isVoiceOn;
            音声ToolStripMenuItem.Checked = isVoiceOn;
            if (!isVoiceOn) waveOut?.Stop();
        }

        private void イラストToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isImgeOn = !isImgeOn;
            イラストToolStripMenuItem.Checked = isImgeOn;
            if (!isImgeOn) CharaIcon.Image = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rtbChatLog.Clear();
            rtbChatLog.SelectionStart = 0;
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rtbChatLog.Text))
            {
                MessageBox.Show("保存するチャットがありません。");
                return;
            }

            string userFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ChatLogs",
                CurrentUser
            );

            Directory.CreateDirectory(userFolder);

            string fileName = Microsoft.VisualBasic.Interaction.InputBox(
                "保存する名前を入力してください",
                "名前を付けて保存",
                ""
            );

            if (string.IsNullOrWhiteSpace(fileName)) return;

            File.WriteAllText(Path.Combine(userFolder, fileName + ".txt"), rtbChatLog.Text);

            if (!lstChatHistory.Items.Contains(fileName))
                lstChatHistory.Items.Add(fileName);

            MessageBox.Show("保存しました！");
        }

        private void lstChatHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChatHistory.SelectedItem == null) return;

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ChatLogs",
                CurrentUser,
                lstChatHistory.SelectedItem + ".txt"
            );

            if (File.Exists(path))
                rtbChatLog.Text = File.ReadAllText(path);
        }

        private void LoadChatHistoryList()
        {
            lstChatHistory.Items.Clear();

            string userFolder = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ChatLogs",
                CurrentUser
            );

            if (!Directory.Exists(userFolder)) return;

            foreach (var f in Directory.GetFiles(userFolder, "*.txt"))
                lstChatHistory.Items.Add(Path.GetFileNameWithoutExtension(f));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            waveOut?.Stop();
            voiceForm.Show();
            Hide();
        }
    }
}
