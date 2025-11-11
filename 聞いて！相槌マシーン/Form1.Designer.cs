namespace 聞いて_相槌マシーン
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Start = new Button();
            license = new Label();
            back = new Button();
            VoiceLabel = new Label();
            ToneLabel = new Label();
            jimaku = new Label();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(248, 182);
            Start.Margin = new Padding(2);
            Start.Name = "Start";
            Start.Size = new Size(280, 129);
            Start.TabIndex = 0;
            Start.Text = "開始";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // license
            // 
            license.AutoSize = true;
            license.Location = new Point(12, 9);
            license.Margin = new Padding(2, 0, 2, 0);
            license.Name = "license";
            license.Size = new Size(268, 25);
            license.TabIndex = 1;
            license.Text = "Voiced by https://CoeFont.cloud";
            // 
            // back
            // 
            back.Location = new Point(12, 404);
            back.Margin = new Padding(2);
            back.Name = "back";
            back.Size = new Size(112, 34);
            back.TabIndex = 4;
            back.Text = "戻る";
            back.UseVisualStyleBackColor = true;
            back.Click += back_Click;
            // 
            // VoiceLabel
            // 
            VoiceLabel.AutoSize = true;
            VoiceLabel.Location = new Point(12, 34);
            VoiceLabel.Margin = new Padding(2, 0, 2, 0);
            VoiceLabel.Name = "VoiceLabel";
            VoiceLabel.Size = new Size(48, 25);
            VoiceLabel.TabIndex = 5;
            VoiceLabel.Text = "音声";
            // 
            // ToneLabel
            // 
            ToneLabel.AutoSize = true;
            ToneLabel.Location = new Point(12, 59);
            ToneLabel.Margin = new Padding(2, 0, 2, 0);
            ToneLabel.Name = "ToneLabel";
            ToneLabel.Size = new Size(68, 25);
            ToneLabel.TabIndex = 6;
            ToneLabel.Text = "スタイル";
            // 
            // jimaku
            // 
            jimaku.AutoSize = true;
            jimaku.Location = new Point(12, 84);
            jimaku.Margin = new Padding(4, 0, 4, 0);
            jimaku.Name = "jimaku";
            jimaku.Size = new Size(48, 25);
            jimaku.TabIndex = 7;
            jimaku.Text = "字幕";
            jimaku.Click += jimaku_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(jimaku);
            Controls.Add(ToneLabel);
            Controls.Add(VoiceLabel);
            Controls.Add(back);
            Controls.Add(license);
            Controls.Add(Start);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "メイン";
            Load += MainForm_Load_1;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Start;
        private Label license;
        private Label label1;
        private NumericUpDown Time;
        private Button back;
        private Label VoiceLabel;
        private Label ToneLabel;
        private Label jimaku;
    }
}
