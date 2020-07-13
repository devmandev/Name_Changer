using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listView1.Columns.Add("OLD NAME");
            listView1.Columns.Add("NEW NAME");
            listView1.Columns.Add("PATH");

            listView1.Columns[0].Width = 150;
            listView1.Columns[1].Width = 150;
            listView1.Columns[2].Width = 150;
        }

        private void ItemInsert(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            listView1.BeginUpdate();

            foreach (string file in files)
            {
                ListViewItem item = new ListViewItem(Path.GetFileName(file));

                item.SubItems.Add(Path.GetFileName(file));
                item.SubItems.Add(Path.GetDirectoryName(file));

                listView1.Items.Add(item);
            }

            listView1.EndUpdate();
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = Path.GetExtension(listView1.Items[i].SubItems[1].Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string number;
            int i;
            int num_count;

            //pad_len = Convert.ToInt32(textBox1.Text);
            //start_number = Convert.ToInt32(textBox2.Text);

            Form2 newForm = new Form2();

            newForm.label1.Text = "번호의 자릿수";
            newForm.label2.Text = "시작할 번호";

            newForm.ShowDialog();

            num_count = Convert.ToInt32(newForm.input2);

            for (i = 0; i < listView1.Items.Count; i++)
            {
                number = Convert.ToString(num_count);

                number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                listView1.Items[i].SubItems[1].Text = number + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                num_count++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            string old_name, new_name;

            for (i = 0; i < listView1.Items.Count; i++)
            {
                old_name = listView1.Items[i].SubItems[2].Text + "\\" + listView1.Items[i].SubItems[0].Text;
                new_name = listView1.Items[i].SubItems[2].Text + "\\" + listView1.Items[i].SubItems[1].Text;

                File.Move(old_name, new_name);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i;

            Form2 newForm = new Form2();

            newForm.label1.Text = "대상 문자열";
            newForm.label2.Text = "변경 문자열";

            newForm.ShowDialog();

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Replace(newForm.input1, newForm.input2);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.ShowDialog();

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = newForm.input1 + listView1.Items[i].SubItems[1].Text;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.ShowDialog();

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + newForm.input1 + Path.GetExtension(listView1.Items[i].SubItems[1].Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i;
            int start_str_index, end_str_index;

            Form2 newForm = new Form2();

            newForm.label1.Text = "시작 문자열";
            newForm.label2.Text = "끝 문자열";

            newForm.ShowDialog();

            for (i = 0; i < listView1.Items.Count; i++)
            {
                start_str_index = listView1.Items[i].SubItems[1].Text.IndexOf(newForm.input1);
                end_str_index = listView1.Items[i].SubItems[1].Text.IndexOf(newForm.input2);


                Console.WriteLine(start_str_index);
                Console.WriteLine(end_str_index);
                listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Remove(start_str_index, end_str_index - start_str_index + 1);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int i, count;

            count = listView1.Items.Count;

            for (i = 0; i < count; i++)
            {
                listView1.Items.RemoveAt(0);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int i;

            Form4 newForm = new Form4();

            newForm.comboBox1.Items.Add("숫자만 남기기");
            newForm.comboBox1.Items.Add("숫자만 지우기");

            newForm.comboBox1.SelectedIndex = 0;

            newForm.ShowDialog();

            switch (newForm.input1)
            {
                case 0:
                    for (i = 0; i < listView1.Items.Count; i++)
                    {
                        listView1.Items[i].SubItems[1].Text = Regex.Replace(listView1.Items[i].SubItems[1].Text, @"\D", "");
                    }
                    break;
                case 1:
                    for (i = 0; i < listView1.Items.Count; i++)
                    {
                        listView1.Items[i].SubItems[1].Text = Regex.Replace(listView1.Items[i].SubItems[1].Text, @"\d", "");
                    }
                    break;
            }
        }
    }
}
