using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class ChatBot : Form
    {
        // ユーザー情報と選択された音声・会話スタイル
        public string CurrentUser { get; set; }
        public string SelectedVoice { get; set; }
        public string SelectedTone { get; set; }

        private VoiceForm voiceForm;

        // 音声再生用
        private WaveOutEvent waveOut;
        private AudioFileReader audioReader;

        // 音声フォルダのマップ（女性A・B、男性A・B → 実際のフォルダ名）
        private Dictionary<string, string> voiceFolderMap = new Dictionary<string, string>()
        {
            {"女性A","相槌_母"},
            {"女性B","相槌_高山"},
            {"男性A","相槌_倉橋"},
            {"男性B","相槌_中谷"}
        };

        // コンストラクター
        public ChatBot(VoiceForm vf, string selectedVoice, string selectedTone)
        {
            InitializeComponent();

            voiceForm = vf; // VoiceFormの参照
            SelectedVoice = selectedVoice; // 選択された音声
            SelectedTone = selectedTone;   // 選択された会話スタイル

            // Enterキーで送信できるようにする
            txtUserInput.KeyDown += TxtUserInput_KeyDown;
        }

        // KeyDownイベントの処理
        private void TxtUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // 改行やビープ音を防ぐ
                btnSend.PerformClick();     // 送信ボタン押下と同じ動作
            }
        }

        // 音声再生・タイマーなどのリソース解放
        private void StopListening()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();
        }

        // フォームロード時の初期設定
        private void ChatBot_Load(object sender, EventArgs e)
        {
            // 音声が選ばれていない場合は警告
            if (string.IsNullOrEmpty(SelectedVoice))
            {
                MessageBox.Show("音声が選択されていません。");
                return;
            }

            // 音声のベースフォルダ
            string baseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");

            // 選択された声が存在するか確認
            if (!voiceFolderMap.ContainsKey(SelectedVoice))
                return;

            // 選択された声のフォルダパス
            string voiceFolder = Path.Combine(baseFolder, voiceFolderMap[SelectedVoice]);
            if (!Directory.Exists(voiceFolder))
                return;

            // 音声ラベルに選択された声を表示
            Voice.Text = "音声-" + SelectedVoice;

            // 会話スタイルをサブフォルダ名から取得してコンボボックスに追加
            string[] styleFolders = Directory.GetDirectories(voiceFolder);
            List<string> chatModes = styleFolders.Select(f => Path.GetFileName(f)).ToList();
            cmbChatMode.Items.Clear();
            cmbChatMode.Items.AddRange(chatModes.ToArray());

            // 初期選択を設定（選択画面で選ばれた会話スタイル）
            if (!string.IsNullOrEmpty(SelectedTone) && chatModes.Contains(SelectedTone))
            {
                cmbChatMode.SelectedItem = SelectedTone;
            }
            else if (chatModes.Count > 0)
            {
                cmbChatMode.SelectedIndex = 0; // デフォルトは最初のスタイル
            }
        }

        // 送信ボタン押下時
        private async void btnSend_Click(object sender, EventArgs e)
        {
            // 入力テキストを取得
            string userText = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(userText)) return;

            // RichTextBoxにユーザー発言を表示
            rtbChatLog.AppendText($"{CurrentUser}: {userText}\n");

            // 最新行までスクロール
            rtbChatLog.SelectionStart = rtbChatLog.Text.Length;
            rtbChatLog.ScrollToCaret();

            // TextBoxをクリア
            txtUserInput.Clear();

            // 0.5秒遅延してから相槌を再生
            await Task.Delay(500);
            PlayRandomAizuchi();
        }

        // ランダムで相槌を選んで再生・表示
        private void PlayRandomAizuchi()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\相槌");

            // 音声または会話スタイルが未選択なら中断
            if (string.IsNullOrEmpty(SelectedVoice) || string.IsNullOrEmpty(SelectedTone))
                return;

            // 声フォルダを取得
            if (!voiceFolderMap.TryGetValue(SelectedVoice, out string voiceFolderName))
                return;

            // ➤ SelectedTone を参照する
            string styleFolder = Path.Combine(basePath, voiceFolderName, SelectedTone);
            if (!Directory.Exists(styleFolder))
                return;

            // wav を取得
            string[] wavFiles = Directory.GetFiles(styleFolder, "*.wav");
            if (wavFiles.Length == 0) return;

            // ランダム選択
            Random rnd = new Random();
            string selectedFile = wavFiles[rnd.Next(wavFiles.Length)];

            // 前の再生停止
            waveOut?.Stop();
            waveOut?.Dispose();
            audioReader?.Dispose();

            audioReader = new AudioFileReader(selectedFile);
            waveOut = new WaveOutEvent();
            waveOut.Init(audioReader);
            waveOut.Play();

            // 字幕用整形
            string subtitle = Regex.Replace(Path.GetFileNameWithoutExtension(selectedFile), @"^\d+_", "");

            // 表示
            this.Invoke(new Action(() =>
            {
                rtbChatLog.AppendText("相槌: " + subtitle + Environment.NewLine);
            }));
        }

        // cmbChatMode の選択が変わったとき
        private void cmbChatMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChatMode.SelectedItem != null)
            {
                // 選択された会話スタイルを SelectedTone に反映
                SelectedTone = cmbChatMode.SelectedItem.ToString();

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            StopListening();
            voiceForm.Show();
            this.Hide();
        }

        private void btnClear_Click(object sender, EventArgs e) 
        {
            rtbChatLog.Clear(); // RichTextBoxの中身をすべてクリア
            //スクロールの位置もリセット
            rtbChatLog.SelectionStart = 0;
            rtbChatLog.ScrollToCaret();
        }

        private void btnSaveLog_Click(object sender, EventArgs e) 
        {
        
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

        private void Voice_Click(object sender, EventArgs e) 
        {
        
        }
    }
}
