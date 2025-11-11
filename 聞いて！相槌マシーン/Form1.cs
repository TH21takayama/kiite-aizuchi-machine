using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows.Forms;
using NAudio.Wave;
using System.Timers;

namespace 聞いて_相槌マシーン
{
    // 音声再生画面
    public partial class MainForm : Form
    {
        public string SelectedVoice { get; set; }
        public string SelectedTone { get; set; }

        private VoiceForm voiceForm;
        private string currentUser; // ✅ ログインユーザー名を保持
        private SoundPlayer player = null;
        private Random random = new Random();

        private WaveInEvent waveIn;
        private DateTime lastVoiceTime;
        private System.Timers.Timer silenceCheckTimer;
        private System.Timers.Timer responseDelayTimer;

        private bool isJimakuOn = true; // ✅ 字幕表示状態（初期値ON）

        // 声の選択肢とフォルダ対応
        private Dictionary<string, string> voiceFolderMap = new Dictionary<string, string>()
        {
            {"女性A","相槌_母"},
            {"女性B","相槌_高山"},
            {"男性A","相槌_倉橋"},
            {"男性B","相槌_中谷"}
        };

        // ✅ コンストラクタでユーザー名を受け取る
        public MainForm(VoiceForm vf, string username)
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
            voiceForm = vf;
            currentUser = username; // ✅ ユーザー名を保持
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (SelectedVoice == "声を選んでね" || SelectedTone == "会話スタイルを選んでね")
            {
                MessageBox.Show("声と会話スタイルを正しく選んでください。");
                voiceForm.Show();
                this.Hide();
                return;
            }

            VoiceLabel.Text = $"音声：{SelectedVoice}";
            ToneLabel.Text = $"スタイル：{SelectedTone}";
            UserLabel.Text = $"ユーザー：{currentUser}"; // ✅ ユーザー名を表示
            jimaku.Text = ""; // 字幕初期化
            JimakuSwitch.Text = "字幕OFF"; // ✅ 初期表示
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (waveIn == null)
            {
                Start.Text = "停止";
                StartListening();
            }
            else
            {
                StopListening();
                Start.Text = "開始";
            }
        }

        private void StartListening()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(16000, 1);
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.StartRecording();

            lastVoiceTime = DateTime.Now;

            silenceCheckTimer = new System.Timers.Timer(200);
            silenceCheckTimer.Elapsed += CheckSilence;
            silenceCheckTimer.Start();
        }

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

            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            float sum = 0;
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i]);
                float amplitude = sample / 32768f;
                sum += amplitude * amplitude;
            }

            float rms = (float)Math.Sqrt(sum / (e.BytesRecorded / 2));
            if (rms > 0.02f) // 音があると判断
            {
                lastVoiceTime = DateTime.Now;
            }
        }

        private void CheckSilence(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - lastVoiceTime).TotalMilliseconds > 1000)
            {
                silenceCheckTimer.Stop();

                responseDelayTimer = new System.Timers.Timer(500); // 0.5秒後に相槌
                responseDelayTimer.Elapsed += (s, args) =>
                {
                    responseDelayTimer.Stop();
                    PlayRandomVoice();
                    lastVoiceTime = DateTime.Now;
                    silenceCheckTimer.Start(); // 再開
                };
                responseDelayTimer.Start();
            }
        }

        private void PlayRandomVoice()
        {
            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            if (!voiceFolderMap.ContainsKey(SelectedVoice)) return;

            string voiceFolderName = voiceFolderMap[SelectedVoice];
            string styleFolder = Path.Combine(baseFolder, voiceFolderName, SelectedTone);
            if (!Directory.Exists(styleFolder)) return;

            string[] voiceFiles = Directory.GetFiles(styleFolder, "*.wav");
            if (voiceFiles.Length == 0) return;

            int index = random.Next(voiceFiles.Length);
            string clipPath = voiceFiles[index];

            // 再生
            if (player != null)
            {
                player.Stop();
                player.Dispose();
            }
            player = new SoundPlayer(clipPath);
            player.Play();

            // ✅ 字幕表示（ONのときのみ）
            if (isJimakuOn)
            {
                string subtitle = Path.GetFileNameWithoutExtension(clipPath);
                Invoke(new Action(() => jimaku.Text = subtitle));
            }
            else
            {
                Invoke(new Action(() => jimaku.Text = ""));
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            StopListening();
            voiceForm.Show();
            this.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopListening();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            SelectedVoice = "声を選んでね";
            SelectedTone = "会話スタイルを選んでね";

            VoiceLabel.Text = $"音声：{SelectedVoice}";
            ToneLabel.Text = $"スタイル：{SelectedTone}";
            jimaku.Text = "";
        }

        // ✅ 字幕ON/OFF切り替え
        private void JimakuSwitch_Click(object sender, EventArgs e)
        {
            isJimakuOn = !isJimakuOn;
            JimakuSwitch.Text = isJimakuOn ? "字幕OFF" : "字幕ON";
            if (!isJimakuOn)
            {
                jimaku.Text = ""; // OFFにしたら字幕消す
            }
        }
    }
}