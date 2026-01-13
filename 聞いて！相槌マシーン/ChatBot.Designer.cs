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
            CharaIcon = new PictureBox();
            menuStrip1 = new MenuStrip();
            メニュToolStripMenuItem = new ToolStripMenuItem();
            イラストToolStripMenuItem = new ToolStripMenuItem();
            音声ToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)CharaIcon).BeginInit();
            menuStrip1.SuspendLayout();
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
            // 
            // btnSaveLog
            // 
            btnSaveLog.Location = new Point(440, 411);
            btnSaveLog.Name = "btnSaveLog";
            btnSaveLog.Size = new Size(94, 29);
            btnSaveLog.TabIndex = 2;
            btnSaveLog.Text = "保存";
            btnSaveLog.UseVisualStyleBackColor = true;
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
            label1.Location = new Point(627, 38);
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
            // 
            // lstChatHistory
            // 
            lstChatHistory.FormattingEnabled = true;
            lstChatHistory.Location = new Point(627, 86);
            lstChatHistory.Name = "lstChatHistory";
            lstChatHistory.Size = new Size(151, 184);
            lstChatHistory.TabIndex = 6;
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(183, 38);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.ReadOnly = true;
            rtbChatLog.Size = new Size(415, 326);
            rtbChatLog.TabIndex = 7;
            rtbChatLog.TabStop = false;
            rtbChatLog.Text = "";
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(183, 371);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(220, 27);
            txtUserInput.TabIndex = 8;
            // 
            // cmbChatMode
            // 
            cmbChatMode.FormattingEnabled = true;
            cmbChatMode.Location = new Point(183, 411);
            cmbChatMode.Name = "cmbChatMode";
            cmbChatMode.Size = new Size(151, 28);
            cmbChatMode.TabIndex = 9;
            // 
            // CharaIcon
            // 
            CharaIcon.Location = new Point(12, 239);
            CharaIcon.Margin = new Padding(2);
            CharaIcon.Name = "CharaIcon";
            CharaIcon.Size = new Size(150, 143);
            CharaIcon.TabIndex = 11;
            CharaIcon.TabStop = false;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { メニュToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 13;
            menuStrip1.Text = "menuStrip1";
            // 
            // メニュToolStripMenuItem
            // 
            メニュToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { イラストToolStripMenuItem, 音声ToolStripMenuItem });
            メニュToolStripMenuItem.Name = "メニュToolStripMenuItem";
            メニュToolStripMenuItem.Size = new Size(65, 24);
            メニュToolStripMenuItem.Text = "メニュー";
            // 
            // イラストToolStripMenuItem
            // 
            イラストToolStripMenuItem.Name = "イラストToolStripMenuItem";
            イラストToolStripMenuItem.Size = new Size(224, 26);
            イラストToolStripMenuItem.Text = "イラスト";
            // 
            // 音声ToolStripMenuItem
            // 
            音声ToolStripMenuItem.Name = "音声ToolStripMenuItem";
            音声ToolStripMenuItem.Size = new Size(224, 26);
            音声ToolStripMenuItem.Text = "音声";
            // 
            // ChatBot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CharaIcon);
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
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "ChatBot";
            Text = "ChatBot";
            Load += ChatBot_Load;
            ((System.ComponentModel.ISupportInitialize)CharaIcon).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
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
        private PictureBox CharaIcon;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem メニュToolStripMenuItem;
        private ToolStripMenuItem イラストToolStripMenuItem;
        private ToolStripMenuItem 音声ToolStripMenuItem;
    }
}