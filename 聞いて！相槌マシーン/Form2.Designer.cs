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
            button1 = new Button();
            UserLabel = new Label();
            SuspendLayout();
            // 
            // VoiceBox
            // 
            VoiceBox.FormattingEnabled = true;
            VoiceBox.Location = new Point(175, 102);
            VoiceBox.Margin = new Padding(2);
            VoiceBox.Name = "VoiceBox";
            VoiceBox.Size = new Size(442, 33);
            VoiceBox.TabIndex = 0;
            VoiceBox.Text = "声を選んでね";
            // 
            // Next
            // 
            Next.Location = new Point(294, 308);
            Next.Margin = new Padding(2);
            Next.Name = "Next";
            Next.Size = new Size(188, 62);
            Next.TabIndex = 1;
            Next.Text = "決定";
            Next.UseVisualStyleBackColor = true;
            Next.Click += Next_Click;
            // 
            // license
            // 
            license.AutoSize = true;
            license.Location = new Point(12, 9);
            license.Margin = new Padding(2, 0, 2, 0);
            license.Name = "license";
            license.Size = new Size(268, 25);
            license.TabIndex = 2;
            license.Text = "Voiced by https://CoeFont.cloud";
            // 
            // ToneBox
            // 
            ToneBox.FormattingEnabled = true;
            ToneBox.Location = new Point(179, 208);
            ToneBox.Margin = new Padding(2);
            ToneBox.Name = "ToneBox";
            ToneBox.Size = new Size(442, 33);
            ToneBox.TabIndex = 3;
            ToneBox.Text = "会話スタイルをえらんでね";
            // 
            // reset
            // 
            reset.Location = new Point(656, 388);
            reset.Margin = new Padding(4, 4, 4, 4);
            reset.Name = "reset";
            reset.Size = new Size(118, 36);
            reset.TabIndex = 4;
            reset.Text = "リセット";
            reset.UseVisualStyleBackColor = true;
            reset.Click += reset_Click;
            // 
            // button1
            // 
            button1.Location = new Point(12, 310);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 5;
            button1.Text = "戻る";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // UserLabel
            // 
            UserLabel.AutoSize = true;
            UserLabel.Location = new Point(12, 42);
            UserLabel.Name = "UserLabel";
            UserLabel.Size = new Size(50, 20);
            UserLabel.TabIndex = 6;
            UserLabel.Text = "label1";
            // 
            // VoiceForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
        private Button button1;
        private Label UserLabel;
    }
}