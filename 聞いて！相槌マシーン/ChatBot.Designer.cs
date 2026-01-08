namespace 聞いて_相槌マシーン
{
    partial class ChatBot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnSend = new Button();
            btnClear = new Button();
            btnSaveLog = new Button();
            btnExit = new Button();
            label1 = new Label();
            Voice = new Label();
            lstChatHistory = new ListBox();
            rtbChatLog = new RichTextBox();
            txtUserInput = new TextBox();
            cmbChatMode = new ComboBox();
            btnVoice = new Button();
            CharaIcon = new PictureBox();
            btnChara = new Button();
            ((System.ComponentModel.ISupportInitialize)CharaIcon).BeginInit();
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(409, 370);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 0;
            btnSend.Text = "送信";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(340, 411);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(94, 29);
            btnClear.TabIndex = 1;
            btnClear.Text = "クリア";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnSaveLog
            // 
            btnSaveLog.Location = new Point(440, 411);
            btnSaveLog.Name = "btnSaveLog";
            btnSaveLog.Size = new Size(94, 29);
            btnSaveLog.TabIndex = 2;
            btnSaveLog.Text = "保存";
            btnSaveLog.UseVisualStyleBackColor = true;
            btnSaveLog.Click += btnSaveLog_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(540, 411);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(94, 29);
            btnExit.TabIndex = 3;
            btnExit.Text = "戻る";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 4;
            label1.Text = "履歴一覧";
            // 
            // Voice
            // 
            Voice.AutoSize = true;
            Voice.Location = new Point(183, 7);
            Voice.Name = "Voice";
            Voice.Size = new Size(45, 20);
            Voice.TabIndex = 5;
            Voice.Text = "音声-";
            Voice.Click += Voice_Click;
            // 
            // lstChatHistory
            // 
            lstChatHistory.FormattingEnabled = true;
            lstChatHistory.Location = new Point(12, 38);
            lstChatHistory.Name = "lstChatHistory";
            lstChatHistory.Size = new Size(151, 184);
            lstChatHistory.TabIndex = 6;
            lstChatHistory.SelectedIndexChanged += lstChatHistory_SelectedIndexChanged;
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(183, 38);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(510, 326);
            rtbChatLog.TabIndex = 7;
            rtbChatLog.Text = "";
            rtbChatLog.ReadOnly = true;
            rtbChatLog.TabStop = false;

            rtbChatLog.TextChanged += rtbChatLog_TextChanged;

            rtbChatLog.TextChanged += rtbChatLog_TextChanged;
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(183, 371);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(220, 27);
            txtUserInput.TabIndex = 8;
            txtUserInput.TextChanged += txtUserInput_TextChanged;
            // 
            // cmbChatMode
            // 
            cmbChatMode.FormattingEnabled = true;
            cmbChatMode.Location = new Point(183, 411);
            cmbChatMode.Name = "cmbChatMode";
            cmbChatMode.Size = new Size(151, 28);
            cmbChatMode.TabIndex = 9;
            cmbChatMode.SelectedIndexChanged += cmbChatMode_SelectedIndexChanged;
            // 
            // btnVoice
            // 
            btnVoice.Location = new Point(701, 7);
            btnVoice.Margin = new Padding(2, 2, 2, 2);
            btnVoice.Name = "btnVoice";
            btnVoice.Size = new Size(90, 27);
            btnVoice.TabIndex = 10;
            btnVoice.Text = "音声オフ";
            btnVoice.UseVisualStyleBackColor = true;
            btnVoice.Click += btnVoice_Click;
            // 
            // CharaIcon
            // 
            CharaIcon.Location = new Point(12, 239);
            CharaIcon.Margin = new Padding(2, 2, 2, 2);
            CharaIcon.Name = "CharaIcon";
            CharaIcon.Size = new Size(150, 143);
            CharaIcon.TabIndex = 11;
            CharaIcon.TabStop = false;
            // 
            // btnChara
            // 
            btnChara.Location = new Point(701, 50);
            btnChara.Margin = new Padding(2, 2, 2, 2);
            btnChara.Name = "btnChara";
            btnChara.Size = new Size(90, 27);
            btnChara.TabIndex = 12;
            btnChara.Text = "イラストオフ";
            btnChara.UseVisualStyleBackColor = true;
            btnChara.Click += btnChara_Click;
            // 
            // ChatBot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnChara);
            Controls.Add(CharaIcon);
            Controls.Add(btnVoice);
            Controls.Add(cmbChatMode);
            Controls.Add(txtUserInput);
            Controls.Add(rtbChatLog);
            Controls.Add(lstChatHistory);
            Controls.Add(Voice);
            Controls.Add(label1);
            Controls.Add(btnExit);
            Controls.Add(btnSaveLog);
            Controls.Add(btnClear);
            Controls.Add(btnSend);
            Name = "ChatBot";
            Text = "ChatBot";
            Load += ChatBot_Load;
            ((System.ComponentModel.ISupportInitialize)CharaIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private Button btnClear;
        private Button btnSaveLog;
        private Button btnExit;
        private Label label1;
        private Label Voice;
        private ListBox lstChatHistory;
        private RichTextBox rtbChatLog;
        private TextBox txtUserInput;
        private ComboBox cmbChatMode;
        private Button btnVoice;
        private PictureBox CharaIcon;
        private Button btnChara;
    }
}