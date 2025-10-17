using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class MainForm : Form
    {
        // 選択された声とスタイルを受け取るプロパティ
        public string SelectedVoice { get; set; }
        public string SelectedTone { get; set; }

        private VoiceForm voiceForm;
        private bool isPlaying = false; // 音声再生中かどうか
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer(); // タイマー
        private Random random = new Random(); // ランダム再生用

        // 声とフォルダ名の対応表
        private Dictionary<string, string> voiceFolderMap = new Dictionary<string, string>()
        {
            {"女性A","相槌_母"},
            {"女性B","相槌_高山"},
            {"男性A","相槌_倉橋"},
            {"男性B","相槌_中谷"}
        };

        private int GetInterval()
        {
            int interval = (int)(Time.Value * 1000);
            if (interval <= 0) interval = 1;
            return interval;
        }

        public MainForm(VoiceForm vf)
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            voiceForm = vf;
            timer.Tick += Timer_Tick;
        }

        public void MainForm_Load(object sender, EventArgs e)
        {
            // プレースホルダーチェック
            if (SelectedVoice == "声を選んでね" || SelectedTone == "会話スタイルを選んでね")
            {
                MessageBox.Show("声と会話スタイルを正しく選んでください。");
                voiceForm.Show();
                this.Hide();
                return;
            }

            // 受け取ったやつを確認
            MessageBox.Show($"受け取った声：{SelectedVoice}\n受け取ったスタイル:{SelectedTone}、でよろしいですか？");

            // ラベルに表示
            VoiceLabel.Text = $"音声：{SelectedVoice}";
            ToneLabel.Text = $"スタイル：{SelectedTone}";
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // 未使用なら削除してもOK
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (!timer.Enabled)
            {
                isPlaying = true;
                Start.Text = "停止";
                timer.Interval = GetInterval();
                timer.Start();
            }
            else
            {
                timer.Stop();
                isPlaying = false;
                Start.Text = "開始";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            PlayRandomVoice();
            timer.Interval = GetInterval();
        }

        private void PlayRandomVoice()
        {
            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            if (!voiceFolderMap.ContainsKey(SelectedVoice)) return;

            string voiceFolderName = voiceFolderMap[SelectedVoice];
            string voiceFolder = Path.Combine(baseFolder, voiceFolderName);
            string styleFolder = Path.Combine(voiceFolder, SelectedTone);

            if (!Directory.Exists(styleFolder)) return;

            string[] voiceFiles = Directory.GetFiles(styleFolder, "*.wav");
            if (voiceFiles.Length == 0) return;

            int index = random.Next(voiceFiles.Length);
            string clipPath = voiceFiles[index];

            SoundPlayer player = new SoundPlayer(clipPath);
            player.Play();
        }

        private void back_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            voiceForm.Show();
            this.Hide();
        }
    }
}