using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;
using System.Timers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace 聞いて_相槌マシーン
{
    public partial class MainForm : Form
    {
        public string SelectedVoice { get; set; }
        public string SelectedTone { get; set; }

        private VoiceForm voiceForm;
        private string currentUser;
        private Random random = new Random();

        private WaveInEvent waveIn;
        private DateTime lastVoiceTime;
        private System.Timers.Timer silenceCheckTimer;
        private System.Timers.Timer responseDelayTimer;

        private bool isJimakuOn = true;
        private bool isImageOn = true;

        private string characterImageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "キャラ絵");

        private Dictionary<string, string> voiceFolderMap = new Dictionary<string, string>()
        {
            {"女性A","相槌_母"},
            {"女性B","相槌_高山"},
            {"男性A","相槌_倉橋"},
            {"男性B","相槌_中谷"}
        };

        private WaveOutEvent waveOut;
        private AudioFileReader audioReader;

        // ✅ 吹き出し用Panelと字幕Label
        private Panel speechBubblePanel;
        private Label bubbleText;

        // 最後に相槌を打った時間
        private DateTime lastResponseTime = DateTime.MinValue; 
        // 相槌の最小間隔（2秒など）
        private int minIntervalMs = 2000;

        //相槌のフラグ
        private bool isPlaying = false;

        // ユーザーが話すまで相槌を打たない
        private bool waitForUserVoice = true;

        //音声認識中のフラグ
        private bool isUserSpeaking = false;

        public MainForm(VoiceForm vf, string username)
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
            voiceForm = vf;
            currentUser = username;

            // ✅ 吹き出しPanel初期化
            speechBubblePanel = new Panel
            {
                Size = new Size(300, 150),
                Visible = false,
                BackColor = Color.Transparent
            };
            speechBubblePanel.Paint += DrawSpeechBubble;

            // ✅ 吹き出し内の字幕Label
            bubbleText = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Meiryo", 14, FontStyle.Bold),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            speechBubblePanel.Controls.Add(bubbleText);
            this.Controls.Add(speechBubblePanel);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var settings = DBHelper.GetUserSettings(currentUser);
            isJimakuOn = settings.JimakuOn;
            isImageOn = settings.ImageOn;

            if (SelectedVoice == "声を選んでね" || SelectedTone == "会話スタイルを選んでね")
            {
                MessageBox.Show("声と会話スタイルを正しく選んでください。");
                voiceForm.Show();
                this.Hide();
                return;
            }

            VoiceLabel.Text = $"音声：{SelectedVoice}";
            ToneLabel.Text = $"スタイル：{SelectedTone}";
            UserLabel.Text = $"ユーザー：{currentUser}";
            JimakuSwitch.Text = isJimakuOn ? "字幕オフ" : "字幕オン";

            characterPictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // 最初から画像を表示
            if (Directory.Exists(characterImageFolder))
            {
                string[] imageFiles = Directory.GetFiles(characterImageFolder, "*.png")
                    .Concat(Directory.GetFiles(characterImageFolder, "*.jpg")).ToArray();

                if (imageFiles.Length > 0)
                {
                    int imgIndex = random.Next(imageFiles.Length);
                    string imgPath = imageFiles[imgIndex];

                    try
                    {
                        using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                        {
                            Image img = Image.FromStream(fs);
                            // PictureBox に直接表示するためコピーを作る
                            characterPictureBox.Image = new Bitmap(img);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("画像読み込みエラー: " + ex.Message);
                    }
                }
            }
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
            if (rms > 0.02f)
            {
                lastVoiceTime = DateTime.Now;
                waitForUserVoice = false;

                // ★ ユーザーが話している → ボタン点灯
                if (!isUserSpeaking)
                {
                    isUserSpeaking = true;
                    Invoke(new Action(() =>
                    {
                        Start.BackColor = Color.LightGreen;  // 点灯色（お好みで変更可）
                    }));
                }
            }
            else
            {
                // ★ 音が小さくなったら = 話していない
                if (isUserSpeaking && (DateTime.Now - lastVoiceTime).TotalMilliseconds > 200)
                {
                    isUserSpeaking = false;
                    Invoke(new Action(() =>
                    {
                        Start.BackColor = SystemColors.Control; // 元に戻す
                    }));
                }
            }

            if (rms > 0.07f)
            {
                lastVoiceTime = DateTime.Now;

                // ユーザーが話した → 次の無音で相槌OKに
                waitForUserVoice = false;
            }
        }

        private void CheckSilence(object sender, ElapsedEventArgs e)
        {
            // ユーザーが話すまでは相槌を禁止
            if (waitForUserVoice) return;

            // 最小間隔
            if ((DateTime.Now - lastResponseTime).TotalMilliseconds < minIntervalMs)
                return;

            // 無音判定
            if ((DateTime.Now - lastVoiceTime).TotalMilliseconds > 350)
            {
                if (responseDelayTimer == null || !responseDelayTimer.Enabled)
                {
                    responseDelayTimer = new System.Timers.Timer(500);
                    responseDelayTimer.Elapsed += (s, args) =>
                    {
                        responseDelayTimer.Stop();
                        responseDelayTimer.Dispose();
                        responseDelayTimer = null;

                        PlayRandomVoiceAndImage();

                        lastResponseTime = DateTime.Now;
                        lastVoiceTime = DateTime.Now;

                        // 💡相槌を出したので、次はユーザーが話すまで無音を無視する！
                        waitForUserVoice = true;
                    };
                    responseDelayTimer.AutoReset = false;
                    responseDelayTimer.Start();
                }
            }
        }

        private void PlayRandomVoiceAndImage()
        {
            if (isPlaying) return; // 再生中なら何もしない
            isPlaying = true;

            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");
            if (!voiceFolderMap.ContainsKey(SelectedVoice)) { isPlaying = false; return; }

            string voiceFolderName = voiceFolderMap[SelectedVoice];
            string styleFolder = Path.Combine(baseFolder, voiceFolderName, SelectedTone);
            if (!Directory.Exists(styleFolder)) { isPlaying = false; return; }

            string[] voiceFiles = Directory.GetFiles(styleFolder)
                .Where(f => f.EndsWith(".wav") || f.EndsWith(".mp3"))
                .ToArray();
            if (voiceFiles.Length == 0) { isPlaying = false; return; }

            int index = random.Next(voiceFiles.Length);
            string clipPath = voiceFiles[index];

            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();

            audioReader = new AudioFileReader(clipPath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioReader);

            // 再生終了イベントでフラグを戻す
            waveOut.PlaybackStopped += (s, e) =>
            {
                isPlaying = false;
                waveOut.Dispose();
                audioReader.Dispose();
                waveOut = null;
                audioReader = null;
            };

            waveOut.Play();

            // ✅ 字幕から番号＋_を削除
            string subtitle = Path.GetFileNameWithoutExtension(clipPath);
            subtitle = Regex.Replace(subtitle, @"^\d+_", "");

            Invoke(new Action(() =>
            {
                if (isJimakuOn)
                {
                    bubbleText.Text = subtitle;

                    int tailWidth = 20;
                    int tailHeight = 20;

                    // フォーム左端からキャラクター左端までの幅に固定（吹き出し本体）
                    int panelWidth = characterPictureBox.Left - 10;
                    if (panelWidth < 50) panelWidth = 50;

                    // 高さはキャラの高さに合わせる
                    int panelHeight = characterPictureBox.Height / 2;
                    if (panelHeight > this.ClientSize.Height - 30)
                        panelHeight = this.ClientSize.Height - 30;

                    // パネル全体のサイズは吹き出し本体＋しっぽ分
                    speechBubblePanel.Size = new Size(panelWidth + tailWidth, panelHeight + tailHeight);

                    bubbleText.MaximumSize = new Size(panelWidth - 10, panelHeight - 10);
                    bubbleText.AutoSize = false;
                    bubbleText.Dock = DockStyle.Fill;
                    bubbleText.TextAlign = ContentAlignment.MiddleCenter;

                    // パネル位置をキャラの下に表示（しっぽ分を上に空ける）
                    int x = 10;
                    int y = characterPictureBox.Top + characterPictureBox.Height - panelHeight - tailHeight;
                    speechBubblePanel.Location = new Point(x, y);

                    speechBubblePanel.Visible = true;
                    speechBubblePanel.BringToFront();
                    speechBubblePanel.Invalidate();
                }
                else
                {
                    speechBubblePanel.Visible = false;
                }
            }));

            if (isImageOn && Directory.Exists(characterImageFolder))
            {
                string[] imageFiles = Directory.GetFiles(characterImageFolder, "*.png");
                if (imageFiles.Length > 0)
                {
                    int imgIndex = random.Next(imageFiles.Length);
                    string imgPath = imageFiles[imgIndex];

                    try
                    {
                        using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                        {
                            Image img = Image.FromStream(fs);
                            Invoke(new Action(() =>
                            {
                                characterPictureBox.Image = img;
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("画像読み込みエラー: " + ex.Message);
                    }
                }
            }
        }

        private void DrawSpeechBubble(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int tailWidth = 20;
            int tailHeight = 20;

            // 吹き出し本体の矩形（しっぽ分を除く）
            Rectangle rect = new Rectangle(0, 0, speechBubblePanel.Width - tailWidth - 1, speechBubblePanel.Height - tailHeight - 1);

            using (GraphicsPath path = new GraphicsPath())
            {
                int radius = 20;
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();

                using (SolidBrush brush = new SolidBrush(Color.White))
                using (Pen pen = new Pen(Color.Gray, 2))
                {
                    g.FillPath(brush, path);
                    g.DrawPath(pen, path);
                }
            }

            // しっぽを描画（右下に向ける）
            Point[] tail = {
                new Point(rect.Right, rect.Bottom - rect.Height / 4),
                new Point(rect.Right + tailWidth, rect.Bottom - rect.Height / 4 + tailHeight / 2),
                new Point(rect.Right, rect.Bottom - rect.Height / 4 + tailHeight)
            };

            g.FillPolygon(Brushes.White, tail);
            g.DrawPolygon(Pens.Gray, tail);
        }

        private void JimakuSwitch_Click(object sender, EventArgs e)
        {
            isJimakuOn = !isJimakuOn;
            JimakuSwitch.Text = isJimakuOn ? "字幕オフ" : "字幕オン";
            if (!isJimakuOn) speechBubblePanel.Visible = false;
            DBHelper.SaveUserSettings(currentUser, SelectedVoice, SelectedTone, isJimakuOn, isImageOn);
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
            DBHelper.SaveUserSettings(currentUser, SelectedVoice, SelectedTone, isJimakuOn, isImageOn);
        }

        private void reset_Click(object sender, EventArgs e)
        {
            SelectedVoice = "声を選んでね";
            SelectedTone = "会話スタイルを選んでね";

            VoiceLabel.Text = $"音声：{SelectedVoice}";
            ToneLabel.Text = $"スタイル：{SelectedTone}";
            bubbleText.Text = "";
            speechBubblePanel.Visible = false;
        }

        private void characterSwitch_Click(object sender, EventArgs e)
        {
            // ON/OFF切り替え
            isImageOn = !isImageOn;

            // ボタンのテキストを変更
            characterSwitch.Text = isImageOn ? "キャラ絵オフ" : "キャラ絵オン";

            // OFFならキャラ絵を非表示
            if (!isImageOn)
            {
                characterPictureBox.Image = null;
            }
            else
            {
                // ONに戻した場合、画像を再表示（フォルダからランダム選択）
                if (Directory.Exists(characterImageFolder))
                {
                    string[] imageFiles = Directory.GetFiles(characterImageFolder, "*.png")
                        .Concat(Directory.GetFiles(characterImageFolder, "*.jpg")).ToArray();

                    if (imageFiles.Length > 0)
                    {
                        int imgIndex = random.Next(imageFiles.Length);
                        string imgPath = imageFiles[imgIndex];

                        try
                        {
                            using (FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                            {
                                Image img = Image.FromStream(fs);
                                characterPictureBox.Image = img;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("画像読み込みエラー: " + ex.Message);
                        }
                    }
                }
            }

            // DBに状態を保存
            DBHelper.SaveUserSettings(currentUser, SelectedVoice, SelectedTone, isJimakuOn, isImageOn);
        }
    }
}