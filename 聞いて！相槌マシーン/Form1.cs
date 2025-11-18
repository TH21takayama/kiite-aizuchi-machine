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
            JimakuSwitch.Text = isJimakuOn ? "字幕OFF" : "字幕ON";

            characterPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
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

            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();
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
            }
        }

        private void CheckSilence(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - lastVoiceTime).TotalMilliseconds > 1000)
            {
                silenceCheckTimer.Stop();

                responseDelayTimer = new System.Timers.Timer(500);
                responseDelayTimer.Elapsed += (s, args) =>
                {
                    responseDelayTimer.Stop();
                    PlayRandomVoiceAndImage();
                    lastVoiceTime = DateTime.Now;
                };
                responseDelayTimer.Start();
            }
        }

        private void PlayRandomVoiceAndImage()
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

            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();

            audioReader = new AudioFileReader(clipPath);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioReader);
            waveOut.Play();

            // ✅ 字幕から番号＋_を削除
            string subtitle = Path.GetFileNameWithoutExtension(clipPath);
            subtitle = Regex.Replace(subtitle, @"^\d+_", "");

            Invoke(new Action(() =>
            {
                if (isJimakuOn)
                {
                    bubbleText.Text = subtitle;

                    // 吹き出しの希望位置
                    int x = characterPictureBox.Left - speechBubblePanel.Width - 10;
                    int y = characterPictureBox.Top + (characterPictureBox.Height / 2) - (speechBubblePanel.Height / 2);

                    // フォーム内に収まるように補正
                    if (x < 0) x = 10; // 左にはみ出さない
                    if (y < 0) y = 10; // 上にはみ出さない
                    if (y + speechBubblePanel.Height > this.ClientSize.Height)
                        y = this.ClientSize.Height - speechBubblePanel.Height - 10; // 下にはみ出さない

                    speechBubblePanel.Location = new Point(x, y);
                    speechBubblePanel.Visible = true;
                    speechBubblePanel.Invalidate();
                    speechBubblePanel.BringToFront();
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

            Rectangle rect = new Rectangle(0, 0, speechBubblePanel.Width - 1, speechBubblePanel.Height - 1);

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

            // 吹き出しのしっぽ
            Point[] tail = {
                new Point(rect.Right, rect.Bottom / 2),
                new Point(rect.Right + 20, rect.Bottom / 2 + 10),
                new Point(rect.Right, rect.Bottom / 2 + 20)
            };
            g.FillPolygon(Brushes.White, tail);
            g.DrawPolygon(Pens.Gray, tail);
        }

        private void JimakuSwitch_Click(object sender, EventArgs e)
        {
            isJimakuOn = !isJimakuOn;
            JimakuSwitch.Text = isJimakuOn ? "字幕OFF" : "字幕ON";
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
    }
}