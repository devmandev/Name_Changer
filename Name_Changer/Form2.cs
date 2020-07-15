using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public string input1;
        public string input2;
        public int input3;
        string name;

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                switch (this.Text)
                {
                    case "묶인곳 지우기":
                        MessageBox.Show("시작 문자열을 입력해주세요.");
                        return;
                    case "번호 붙이기":
                        MessageBox.Show("자릿수를 입력해주세요.");
                        return;
                    case "문자열 바꾸기":
                        MessageBox.Show("대상 문자열을 입력해주세요.");
                        return;
                }
            }

            if (textBox2.Text == "")
            {
                switch (this.Text)
                {
                    case "묶인곳 지우기":
                        MessageBox.Show("끝 문자열을 입력해주세요.");
                        return;
                    case "번호 붙이기":
                        MessageBox.Show("시작 번호를 입력해주세요.");
                        return;
                }
            }


            input1 = textBox1.Text;
            input2 = textBox2.Text;

            //pad_len = Convert.ToInt32(textBox1.Text);
            //start_number = Convert.ToInt32(textBox2.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            input3 = comboBox1.SelectedIndex;
        }
    }
}
