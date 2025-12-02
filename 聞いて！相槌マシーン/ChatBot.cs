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

        // 音声オン/オフフラグ
        private bool isVoiceOn = true;

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

            // チャット履歴読み込み（ユーザーごと）
            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatLogs", CurrentUser);
            Directory.CreateDirectory(userFolder);

            string[] files = Directory.GetFiles(userFolder, "*.txt");
            foreach (string f in files)
            {
                lstChatHistory.Items.Add(Path.GetFileNameWithoutExtension(f));
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

            if (string.IsNullOrEmpty(SelectedVoice) || string.IsNullOrEmpty(SelectedTone))
                return;

            if (!voiceFolderMap.TryGetValue(SelectedVoice, out string voiceFolderName))
                return;

            string styleFolder = Path.Combine(basePath, voiceFolderName, SelectedTone);
            //MessageBox.Show(styleFolder); 参照してるファイル確認用
            if (!Directory.Exists(styleFolder))
                return;

            string[] wavFiles = Directory.GetFiles(styleFolder)
            .Where(f => f.EndsWith(".wav") || f.EndsWith(".mp3"))
            .ToArray();

            if (wavFiles.Length == 0) return;

            Random rnd = new Random();
            string selectedFile = wavFiles[rnd.Next(wavFiles.Length)];

            // 音声オンの場合のみ再生
            if (isVoiceOn)
            {
                waveOut?.Stop();
                waveOut?.Dispose();
                audioReader?.Dispose();

                audioReader = new AudioFileReader(selectedFile);
                waveOut = new WaveOutEvent();
                waveOut.Init(audioReader);
                waveOut.Play();
            }

            string subtitle = Regex.Replace(Path.GetFileNameWithoutExtension(selectedFile), @"^\d+_", "");
            this.Invoke(new Action(() =>
            {
                rtbChatLog.AppendText(SelectedVoice +": " + subtitle + Environment.NewLine);
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
            // チャットが空なら保存しない
            if (string.IsNullOrWhiteSpace(rtbChatLog.Text))
            {
                MessageBox.Show("保存するチャットがありません。");
                return;
            }

            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatLogs", CurrentUser);
            Directory.CreateDirectory(userFolder);

            string fileName = "";
            bool overwrite = false;

            if (lstChatHistory.SelectedItem != null)
            {
                // 既に選択されている履歴を上書きする場合
                fileName = lstChatHistory.SelectedItem.ToString();
                var result = MessageBox.Show($"{fileName} に上書きしますか？", "確認", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Cancel)
                    return;
                else if (result == DialogResult.No)
                    overwrite = false; // 新しい名前で保存する
                else
                    overwrite = true; // 上書き
            }

            if (!overwrite)
            {
                // 新しい名前をユーザーに入力させる
                fileName = Microsoft.VisualBasic.Interaction.InputBox(
                    "保存する名前を入力してください",
                    "名前を付けて保存",
                    "");

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show("保存がキャンセルされました。");
                    return;
                }
            }

            string fullPath = Path.Combine(userFolder, fileName + ".txt");
            File.WriteAllText(fullPath, rtbChatLog.Text);

            // ListBoxにない場合は追加
            if (!lstChatHistory.Items.Contains(fileName))
                lstChatHistory.Items.Add(fileName);

            MessageBox.Show("保存しました！");
        }

        private void rtbChatLog_TextChanged(object sender, EventArgs e)
        {

        }

        private void lstChatHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstChatHistory.SelectedItem == null) return;

            string fileName = lstChatHistory.SelectedItem.ToString();
            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ChatLogs", CurrentUser);
            string fullPath = Path.Combine(userFolder, fileName + ".txt");

            if (File.Exists(fullPath))
            {
                rtbChatLog.Text = File.ReadAllText(fullPath);
            }
        }

        private void txtUserInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void Voice_Click(object sender, EventArgs e)
        {

        }

        // 音声オン/オフ切り替え
        private void btnVoice_Click(object sender, EventArgs e)
        {
            isVoiceOn = !isVoiceOn;

            if (isVoiceOn)
            {
                btnVoice.Text = "音声オフ";
            }
            else
            {
                btnVoice.Text = "音声オン";
                waveOut?.Stop(); // 再生中の音声を停止
            }
        }
    }
}
