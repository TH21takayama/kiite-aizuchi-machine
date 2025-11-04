using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 聞いて_相槌マシーン
{
    public partial class VoiceForm : Form
    {
        public VoiceForm()
        {
            InitializeComponent();

            //声
            VoiceBox.Items.Add("女性A"); //母
            VoiceBox.Items.Add("女性B"); //高山
            VoiceBox.Items.Add("男性A"); //倉橋
            VoiceBox.Items.Add("男性B"); //中谷

            //会話スタイル
            ToneBox.Items.Add("愚痴");
            ToneBox.Items.Add("自慢");
            ToneBox.Items.Add("汎用");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void VoiceBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Next_Click(object sender, EventArgs e)
        {
            string selectedVoice = VoiceBox.SelectedItem?.ToString();
            string selectedTone = ToneBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedVoice) && string.IsNullOrEmpty(selectedTone))
            {
                MessageBox.Show("声と会話スタイルを選んでください。");
                return;
            }
            else if (string.IsNullOrEmpty(selectedVoice))
            {
                MessageBox.Show("声を選んでください。");
                return;
            }
            else if (string.IsNullOrEmpty(selectedTone))
            {
                MessageBox.Show("会話スタイルを選んでください。");
                return;
            }

            //MainFormのインスタンスを作成
            MainForm mainForm = new MainForm(this);

            //プロパティに値をセット
            mainForm.SelectedVoice = selectedVoice;
            mainForm.SelectedTone = selectedTone;

            //MainFormを表示してVoiceFormは非表示
            mainForm.Show();
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)//リセットボタン
        {
            VoiceBox.SelectedIndex = -1;
            ToneBox.SelectedIndex = -1;
        }
    }
}
