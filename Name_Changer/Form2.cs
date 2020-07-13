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

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("자릿수를 입력해주세요.");
                return;
            }

            if (textBox2.Text == "")
            {
                MessageBox.Show("시작 번호를 입력해주세요.");
                return;
            }

            input1 = textBox1.Text;
            input2 = textBox2.Text;

            //pad_len = Convert.ToInt32(textBox1.Text);
            //start_number = Convert.ToInt32(textBox2.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
