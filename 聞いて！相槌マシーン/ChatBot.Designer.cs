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
            label2 = new Label();
            lstChatHistory = new ListBox();
            rtbChatLog = new RichTextBox();
            txtUserInput = new TextBox();
            cmbChatMode = new ComboBox();
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
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(699, 342);
            label2.Name = "label2";
            label2.Size = new Size(76, 20);
            label2.TabIndex = 5;
            label2.Text = "入力待ち…";
            // 
            // lstChatHistory
            // 
            lstChatHistory.FormattingEnabled = true;
            lstChatHistory.Location = new Point(12, 38);
            lstChatHistory.Name = "lstChatHistory";
            lstChatHistory.Size = new Size(151, 324);
            lstChatHistory.TabIndex = 6;
            // 
            // rtbChatLog
            // 
            rtbChatLog.Location = new Point(183, 38);
            rtbChatLog.Name = "rtbChatLog";
            rtbChatLog.Size = new Size(510, 326);
            rtbChatLog.TabIndex = 7;
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
            // ChatBot
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbChatMode);
            Controls.Add(txtUserInput);
            Controls.Add(rtbChatLog);
            Controls.Add(lstChatHistory);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnExit);
            Controls.Add(btnSaveLog);
            Controls.Add(btnClear);
            Controls.Add(btnSend);
            Name = "ChatBot";
            Text = "ChatBot";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnSend;
        private Button btnClear;
        private Button btnSaveLog;
        private Button btnExit;
        private Label label1;
        private Label label2;
        private ListBox lstChatHistory;
        private RichTextBox rtbChatLog;
        private TextBox txtUserInput;
        private ComboBox cmbChatMode;
    }
}