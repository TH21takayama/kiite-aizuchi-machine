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
            SuspendLayout();
            // 
            // btnSend
            // 
            btnSend.Location = new Point(511, 462);
            btnSend.Margin = new Padding(4);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(118, 36);
            btnSend.TabIndex = 0;
            btnSend.Text = "送信";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(425, 514);
            btnClear.Margin = new Padding(4);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(118, 36);
            btnClear.TabIndex = 1;
            btnClear.Text = "クリア";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnSaveLog
            // 
            btnSaveLog.Location = new Point(550, 514);
            btnSaveLog.Margin = new Padding(4);
            btnSaveLog.Name = "btnSaveLog";
            btnSaveLog.Size = new Size(118, 36);
            btnSaveLog.TabIndex = 2;
            btnSaveLog.Text = "保存";
            btnSaveLog.UseVisualStyleBackColor = true;
            btnSaveLog.Click += btnSaveLog_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(675, 514);
            btnExit.Margin = new Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(118, 36);
            btnExit.TabIndex = 3;
            btnExit.Text = "戻る";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 19);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(84, 25);
            label1.TabIndex = 4;
            label1.Text = "履歴一覧";
            // 
            // Voice
            // 
            Voice.AutoSize = true;
            Voice.Location = new Point(229, 9);
            Voice.Margin = new Padding(4, 0, 4, 0);
            Voice.Name = "Voice";
            Voice.Size = new Size(55, 25);
            Voice.TabIndex = 5;
            Voice.Text = "音声-";
            Voice.Click += Voice_Click;
            // 
            // lstChatHistory
            // 
            lstChatHistory.FormattingEnabled = true;
            lstChatHistory.ItemHeight = 25;
            lstChatHistory.Location = new Point(15, 48);
            lstChatHistory.Margin = new Padding(4);
            lstChatHistory.Name = "lstChatHistory";
            lstChatHistory.Size = new Size(188, 404);
            lstChatHistory.TabIndex = 6;
            lstChatHistory.SelectedIndexChanged += lstChatHistory_SelectedIndexChanged;
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(229, 48);
            rtbChatLog.Margin = new Padding(4);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(636, 406);
            rtbChatLog.TabIndex = 7;
            rtbChatLog.Text = "";
            rtbChatLog.TextChanged += rtbChatLog_TextChanged;
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(229, 464);
            txtUserInput.Margin = new Padding(4);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(274, 31);
            txtUserInput.TabIndex = 8;
            txtUserInput.TextChanged += txtUserInput_TextChanged;
            // 
            // cmbChatMode
            // 
            cmbChatMode.FormattingEnabled = true;
            cmbChatMode.Location = new Point(229, 514);
            cmbChatMode.Margin = new Padding(4);
            cmbChatMode.Name = "cmbChatMode";
            cmbChatMode.Size = new Size(188, 33);
            cmbChatMode.TabIndex = 9;
            cmbChatMode.SelectedIndexChanged += cmbChatMode_SelectedIndexChanged;
            // 
            // ChatBot
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 562);
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
            Margin = new Padding(4);
            Name = "ChatBot";
            Text = "ChatBot";
            Load += ChatBot_Load;
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
    }
}