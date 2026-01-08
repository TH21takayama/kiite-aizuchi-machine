namespace 聞いて_相槌マシーン
{
    partial class VoiceForm
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
            VoiceBox = new ComboBox();
            Next = new Button();
            license = new Label();
            ToneBox = new ComboBox();
            reset = new Button();
            buttonback = new Button();
            UserLabel = new Label();
            chatbutton = new Button();
            SuspendLayout();
            // 
            // VoiceBox
            // 
            VoiceBox.FormattingEnabled = true;
            VoiceBox.Location = new Point(140, 82);
            VoiceBox.Margin = new Padding(2);
            VoiceBox.Name = "VoiceBox";
            VoiceBox.Size = new Size(354, 28);
            VoiceBox.TabIndex = 0;
            VoiceBox.Text = "声を選んでね";
            // 
            // Next
            // 
            Next.Location = new Point(397, 246);
            Next.Margin = new Padding(2);
            Next.Name = "Next";
            Next.Size = new Size(150, 50);
            Next.TabIndex = 1;
            Next.Text = "音声会話";
            Next.UseVisualStyleBackColor = true;
            Next.Click += Next_Click;
            // 
            // license
            // 
            license.AutoSize = true;
            license.Location = new Point(10, 7);
            license.Margin = new Padding(2, 0, 2, 0);
            license.Name = "license";
            license.Size = new Size(220, 20);
            license.TabIndex = 2;
            license.Text = "Voiced by https://CoeFont.cloud";
            // 
            // ToneBox
            // 
            ToneBox.FormattingEnabled = true;
            ToneBox.Location = new Point(143, 166);
            ToneBox.Margin = new Padding(2);
            ToneBox.Name = "ToneBox";
            ToneBox.Size = new Size(354, 28);
            ToneBox.TabIndex = 3;
            ToneBox.Text = "会話スタイルをえらんでね";
            // 
            // reset
            // 
            reset.Location = new Point(525, 310);
            reset.Name = "reset";
            reset.Size = new Size(94, 29);
            reset.TabIndex = 4;
            reset.Text = "リセット";
            reset.UseVisualStyleBackColor = true;
            reset.Click += buttonReset_Click;
            // 
            // buttonback
            // 
            buttonback.Location = new Point(12, 310);
            buttonback.Name = "buttonback";
            buttonback.Size = new Size(94, 29);
            buttonback.TabIndex = 5;
            buttonback.Text = "戻る";
            buttonback.UseVisualStyleBackColor = true;
            buttonback.Click += buttonback_Click;
            // 
            // UserLabel
            // 
            UserLabel.AutoSize = true;
            UserLabel.Location = new Point(12, 42);
            UserLabel.Name = "UserLabel";
            UserLabel.Size = new Size(53, 20);
            UserLabel.TabIndex = 6;
            UserLabel.Text = "ユーザー";
            // 
            // chatbutton
            // 
            chatbutton.Location = new Point(86, 246);
            chatbutton.Margin = new Padding(2, 2, 2, 2);
            chatbutton.Name = "chatbutton";
            chatbutton.Size = new Size(150, 50);
            chatbutton.TabIndex = 7;
            chatbutton.Text = "チャット";
            chatbutton.UseVisualStyleBackColor = true;
            chatbutton.Click += chatbutton_Click;
            // 
            // VoiceForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 360);
            Controls.Add(chatbutton);
            Controls.Add(UserLabel);
            Controls.Add(buttonback);
            Controls.Add(reset);
            Controls.Add(ToneBox);
            Controls.Add(license);
            Controls.Add(Next);
            Controls.Add(VoiceBox);
            Margin = new Padding(2);
            Name = "VoiceForm";
            Text = "選択";
            Load += VoiceForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox VoiceBox;
        private Button Next;
        private Label license;
        private ComboBox ToneBox;
        private Button reset;
        private Button buttonback;
        private Label UserLabel;
        private Button chatbutton;
    }
}