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
            label1 = new Label();
            Time = new NumericUpDown();
            back = new Button();
            VoiceLabel = new Label();
            ToneLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)Time).BeginInit();
            SuspendLayout();
            // 
            // Start
            // 
            Start.Location = new Point(248, 183);
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
            license.Name = "license";
            license.Size = new Size(268, 25);
            license.TabIndex = 1;
            license.Text = "Voiced by https://CoeFont.cloud";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(512, 9);
            label1.Name = "label1";
            label1.Size = new Size(76, 25);
            label1.TabIndex = 2;
            label1.Text = "間隔(秒)";
            // 
            // Time
            // 
            Time.Location = new Point(594, 7);
            Time.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Time.Name = "Time";
            Time.Size = new Size(180, 31);
            Time.TabIndex = 3;
            Time.Value = new decimal(new int[] { 1, 0, 0, 0 });
            Time.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // back
            // 
            back.Location = new Point(12, 404);
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
            VoiceLabel.Name = "VoiceLabel";
            VoiceLabel.Size = new Size(48, 25);
            VoiceLabel.TabIndex = 5;
            VoiceLabel.Text = "音声";
            // 
            // ToneLabel
            // 
            ToneLabel.AutoSize = true;
            ToneLabel.Location = new Point(12, 59);
            ToneLabel.Name = "ToneLabel";
            ToneLabel.Size = new Size(68, 25);
            ToneLabel.TabIndex = 6;
            ToneLabel.Text = "スタイル";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ToneLabel);
            Controls.Add(VoiceLabel);
            Controls.Add(back);
            Controls.Add(Time);
            Controls.Add(label1);
            Controls.Add(license);
            Controls.Add(Start);
            Name = "MainForm";
            Text = "メイン";
            ((System.ComponentModel.ISupportInitialize)Time).EndInit();
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
    }
}
