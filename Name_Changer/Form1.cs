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
using System.Security.AccessControl;
using System.Globalization;
using System.Collections;

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
            listView1.Columns.Add("SIZE");
            listView1.Columns.Add("MODIFY DATE");
            listView1.Columns.Add("CREATE DATE");

            listView1.Columns[3].Tag = "Number";

            listView1.Columns[0].Width = 150;
            listView1.Columns[1].Width = 150;
            listView1.Columns[2].Width = 150;
            listView1.Columns[3].Width = 150;
            listView1.Columns[4].Width = 150;
            listView1.Columns[5].Width = 150;
        }

        private void ItemInsert(object sender, DragEventArgs e)
        {
            string[] location_list = (string[])e.Data.GetData(DataFormats.FileDrop);

            listView1.BeginUpdate();

            int folder_check = 0;

            

            foreach (string loc in location_list)
            {
                if ((File.GetAttributes(loc) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if (MessageBox.Show("폴더를 추가하려면 YES, 폴더의 하위파일을 추가하려면 NO를 눌러주세요.", "하위파일 추가여부 선택", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        folder_check = 1;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (string loc in location_list)
            {
                if(folder_check == 1)
                {
                    getDirectory(loc);
                }
                else
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(loc));
                    FileInfo info = new FileInfo(loc);

                    item.SubItems.Add(Path.GetFileName(loc));
                    item.SubItems.Add(Path.GetDirectoryName(loc));
                    item.SubItems.Add(info.Length.ToString() + " Bytes");
                    item.SubItems.Add(info.LastWriteTime.ToString());
                    item.SubItems.Add(info.CreationTime.ToString());

                    listView1.Items.Add(item);
                }
            }

            listView1.EndUpdate();
        }

        private void getDirectory(string dir)
        {
            if ((File.GetAttributes(dir) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                string[] files = Directory.GetFiles(dir);
                string[] dirs = Directory.GetDirectories(dir);

                foreach(string dir_2 in dirs)
                {
                    getDirectory(dir_2);
                }

                foreach(string file in files)
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(file));

                    item.SubItems.Add(Path.GetFileName(file));
                    item.SubItems.Add(Path.GetDirectoryName(file));

                    listView1.Items.Add(item);
                }
            }
            else
            {
                ListViewItem item = new ListViewItem(Path.GetFileName(dir));

                item.SubItems.Add(Path.GetFileName(dir));
                item.SubItems.Add(Path.GetDirectoryName(dir));

                listView1.Items.Add(item);
            }
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
            newForm.Text = "번호 붙이기";
            newForm.comboBox1.Items.Add("맨앞에 번호 붙이기");
            newForm.comboBox1.Items.Add("맨뒤에 번호 붙이기");
            newForm.comboBox1.Items.Add("폴더별로 맨앞에 번호 붙이기");
            newForm.comboBox1.Items.Add("폴더별로 맨뒤에 번호 붙이기");
            
            newForm.comboBox1.SelectedIndex = 0;

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                num_count = Convert.ToInt32(newForm.input2);

                switch (newForm.input3)
                {
                    case 0:
                        for (i = 0; i < listView1.Items.Count; i++)
                        {
                            number = Convert.ToString(num_count);

                            number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                            listView1.Items[i].SubItems[1].Text = number + Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                            num_count++;
                        }
                        break;
                    case 1:
                        for (i = 0; i < listView1.Items.Count; i++)
                        {
                            number = Convert.ToString(num_count);

                            number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                            listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + number + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                            num_count++;
                        }
                        break;
                    case 2:
                        number = Convert.ToString(num_count);

                        number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                        listView1.Items[0].SubItems[1].Text = number + Path.GetFileNameWithoutExtension(listView1.Items[0].SubItems[1].Text) + Path.GetExtension(listView1.Items[0].SubItems[1].Text);

                        num_count++;

                        for (i = 1; i < listView1.Items.Count; i++)
                        {
                            if (listView1.Items[i - 1].SubItems[2].Text == listView1.Items[i].SubItems[2].Text)
                            {
                                number = Convert.ToString(num_count);

                                number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                                listView1.Items[i].SubItems[1].Text = number + Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                                num_count++;
                            }
                            else
                            {
                                num_count = Convert.ToInt32(newForm.input2);

                                number = Convert.ToString(num_count);

                                number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                                listView1.Items[i].SubItems[1].Text = number + Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                                num_count++;
                            }
                        }
                        break; 
                    case 3:
                        number = Convert.ToString(num_count);

                        number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                        listView1.Items[0].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[0].SubItems[1].Text) + number + Path.GetExtension(listView1.Items[0].SubItems[1].Text);

                        num_count++;

                        for (i = 1; i < listView1.Items.Count; i++)
                        {
                            if (listView1.Items[i - 1].SubItems[2].Text == listView1.Items[i].SubItems[2].Text)
                            {
                                number = Convert.ToString(num_count);

                                number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                                listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + number + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                                num_count++;
                            }
                            else
                            {
                                num_count = Convert.ToInt32(newForm.input2);

                                number = Convert.ToString(num_count);

                                number = number.PadLeft(Convert.ToInt32(newForm.input1), '0');

                                listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + number + Path.GetExtension(listView1.Items[i].SubItems[1].Text);

                                num_count++;
                            }
                        }
                        break;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i;
            string old_name, new_name;
            string error_message = "";

            if (MessageBox.Show("실제 파일에 적용하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    old_name = listView1.Items[i].SubItems[2].Text + "\\" + listView1.Items[i].SubItems[0].Text;

                    if ((File.GetAttributes(old_name) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        new_name = Path.Combine(listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[1].Text);

                        if (Directory.Exists(new_name))
                        {
                            error_message += old_name + " -> " + new_name + " 이동 실패 \n";
                        }
                        else
                        {
                            Directory.Move(old_name, new_name);
                            listView1.Items[i].SubItems[0].Text = listView1.Items[i].SubItems[1].Text;
                        }
                            
                        
                    }
                    else
                    {
                        new_name = Path.Combine(listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[1].Text);

                        if (File.Exists(new_name))
                        {
                            error_message += old_name + " -> " + new_name + " 이동 실패 \n";
                        }
                        else
                        {
                            File.Move(old_name, new_name);
                            listView1.Items[i].SubItems[0].Text = listView1.Items[i].SubItems[1].Text;
                        }
                    }
                }

                if (error_message.Length > 0)
                {
                    MessageBox.Show(error_message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i;

            Form2 newForm = new Form2();

            newForm.label1.Text = "대상 문자열";
            newForm.label2.Text = "변경 문자열";
            newForm.Text = "문자열 바꾸기";
            newForm.comboBox1.Visible = false;
            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Replace(newForm.input1, newForm.input2);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.comboBox1.Visible = false;

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[1].Text = newForm.input1 + listView1.Items[i].SubItems[1].Text;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.comboBox1.Visible = false;

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + newForm.input1 + Path.GetExtension(listView1.Items[i].SubItems[1].Text);
                }
            }
        }

        // 묶인곳 지우기
        private void button7_Click(object sender, EventArgs e)
        {
            int i;
            int start_str_index, end_str_index;

            Form2 newForm = new Form2();

            newForm.label1.Text = "시작 문자열";
            newForm.label2.Text = "끝 문자열";
            newForm.comboBox1.Visible = false;
            newForm.Text = "묶인곳 지우기";

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    start_str_index = listView1.Items[i].SubItems[1].Text.IndexOf(newForm.input1);
                    end_str_index = listView1.Items[i].SubItems[1].Text.IndexOf(newForm.input2);

                    Console.WriteLine(start_str_index);
                    Console.WriteLine(end_str_index);
                    listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Remove(start_str_index, end_str_index - start_str_index + 1);
                }
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

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                switch (newForm.input1)
                {
                    case 0:
                        for (i = 0; i < listView1.Items.Count; i++)
                        {
                            listView1.Items[i].SubItems[1].Text = Regex.Replace(listView1.Items[i].SubItems[1].Text, @"\D", "") + Path.GetExtension(listView1.Items[i].SubItems[1].Text);
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

        private void button10_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[0].Text;
            }
        }

        //자릿수 맞추기
        private void button11_Click(object sender, EventArgs e)
        {
            int i, start_index, end_index;
            string number;
            
            Form3 newForm = new Form3();

            newForm.label1.Text = "자릿수";

            newForm.comboBox1.Items.Add("맨 앞 숫자");
            newForm.comboBox1.Items.Add("맨 뒤 숫자");

            newForm.comboBox1.SelectedIndex = 0;

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                switch (newForm.input2)
                {
                    case 0:
                        for (i = 0; i < listView1.Items.Count; i++)
                        {
                            var matches = Regex.Split(listView1.Items[i].SubItems[1].Text, @"[^0-9]+").Where(s => s != String.Empty).ToArray();

                            start_index = listView1.Items[i].SubItems[1].Text.IndexOf(matches[0]);
                            end_index = start_index + matches[0].Length;
                            number = matches[0].PadLeft(Convert.ToInt32(newForm.input1), '0');

                            listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Substring(0, start_index) + number + listView1.Items[i].SubItems[1].Text.Substring(end_index);
                        }
                        break;
                    case 1:
                        for (i = 0; i < listView1.Items.Count; i++)
                        {
                            var matches = Regex.Split(listView1.Items[i].SubItems[1].Text, @"[^0-9]+").Where(s => s != String.Empty).ToArray();

                            start_index = listView1.Items[i].SubItems[1].Text.LastIndexOf(matches[matches.Length - 1]);
                            end_index = start_index + matches[matches.Length - 1].Length;
                            number = matches[matches.Length - 1].PadLeft(Convert.ToInt32(newForm.input1), '0');

                            listView1.Items[i].SubItems[1].Text = listView1.Items[i].SubItems[1].Text.Substring(0, start_index) + number + listView1.Items[i].SubItems[1].Text.Substring(end_index);
                        }
                        break;
                }
            }   
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int sortColumn = -1;

            Form4 newForm = new Form4();

            newForm.comboBox1.Items.Add("이름 오름차순");
            newForm.comboBox1.Items.Add("이름 내림차순");
            newForm.comboBox1.Items.Add("변경이름 오름차순");
            newForm.comboBox1.Items.Add("변경이름 내림차순");
            newForm.comboBox1.Items.Add("파일크기 오름차순");
            newForm.comboBox1.Items.Add("파일크기 내림차순");
            newForm.comboBox1.Items.Add("수정시간 오름차순");
            newForm.comboBox1.Items.Add("수정시간 내림차순");
            newForm.comboBox1.Items.Add("만든시간 오름차순");
            newForm.comboBox1.Items.Add("만든시간 내림차순");


            newForm.comboBox1.SelectedIndex = 0;

            newForm.StartPosition = FormStartPosition.CenterParent;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                switch (newForm.input1)
                {
                    case 0:
                        sortColumn = 0;
                        listView1.Sorting = SortOrder.Ascending;
                        break;
                    case 1:
                        sortColumn = 0;
                        listView1.Sorting = SortOrder.Descending;
                        break;
                    case 2:
                        sortColumn = 1;
                        listView1.Sorting = SortOrder.Ascending;
                        break;
                    case 3:
                        sortColumn = 1;
                        listView1.Sorting = SortOrder.Descending;
                        break;
                    case 4:
                        sortColumn = 3;
                        listView1.Sorting = SortOrder.Ascending;
                        break;
                    case 5:
                        sortColumn = 3;
                        listView1.Sorting = SortOrder.Descending;
                        break;
                    case 6:
                        sortColumn = 4;
                        listView1.Sorting = SortOrder.Ascending;
                        break;
                    case 7:
                        sortColumn = 4;
                        listView1.Sorting = SortOrder.Descending;
                        break;
                    case 8:
                        sortColumn = 5;
                        listView1.Sorting = SortOrder.Ascending;
                        break;
                    case 9:
                        sortColumn = 5;
                        listView1.Sorting = SortOrder.Descending;
                        break;
                }

                listView1.Sort();
                listView1.ListViewItemSorter = new MyListViewComparer(sortColumn, listView1.Sorting);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3();

            newForm.label1.Text = "변경할 이름";
            newForm.comboBox1.Visible = false;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                int index = listView1.FocusedItem.Index;

                listView1.Items[index].SubItems[1].Text = newForm.input1 + Path.GetExtension(listView1.Items[index].SubItems[1].Text);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            int index = listView1.FocusedItem.Index;

            if (index > 0)
            {
                var item = listView1.Items[index];

                listView1.Items.Remove(item);

                listView1.Items.Insert(index - 1, item);

                listView1.Items[index - 1].Focused = true;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            int index = listView1.FocusedItem.Index;

            if (index < listView1.Items.Count - 1)
            {
                var item = listView1.Items[index];

                listView1.Items.Remove(item);

                listView1.Items.Insert(index + 1, item);

                listView1.Items[index + 1].Focused = true;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string error_message = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult dialog_value = fbd.ShowDialog();

            if(dialog_value == DialogResult.OK)
            {
                int i;
                string old_name, new_name;

                for (i = 0; i < listView1.Items.Count; i++)
                {
                    //old_name = listView1.Items[i].SubItems[2].Text + "\\" + listView1.Items[i].SubItems[0].Text;
                    old_name = Path.Combine(listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[0].Text);
                    listView1.Items[i].SubItems[2].Text = fbd.SelectedPath;

                    if ((File.GetAttributes(old_name) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        new_name = Path.Combine(listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[1].Text);

                        if (Directory.Exists(new_name))
                        {
                            error_message += old_name + " -> " + new_name + " 이동 실패 \n";
                        }
                        else
                        {
                            Directory.Move(old_name, new_name);
                        }
                    }
                    else
                    {
                        new_name = Path.Combine(listView1.Items[i].SubItems[2].Text, listView1.Items[i].SubItems[1].Text);

                        if (File.Exists(new_name))
                        {
                            error_message += old_name + " -> " + new_name + " 이동 실패 \n";
                        }
                        else
                        {
                            File.Move(old_name, new_name);
                        }
                    }
                }

                if (error_message.Length > 0)
                {
                    MessageBox.Show(error_message);
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            int i;

            for (i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.label1.Text = "추가할 확장자";
            newForm.comboBox1.Visible = false;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[1].Text = Path.GetFileName(listView1.Items[i].SubItems[1].Text) + "." + newForm.input1;
                }
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            int i;

            Form3 newForm = new Form3();

            newForm.label1.Text = "변경할 확장자";
            newForm.comboBox1.Visible = false;

            DialogResult dialog_value = newForm.ShowDialog();

            if (dialog_value == DialogResult.OK)
            {
                for (i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[1].Text = Path.GetFileNameWithoutExtension(listView1.Items[i].SubItems[1].Text) + "." + newForm.input1;
                }
            }
        }
    }

    class MyListViewComparer : IComparer
    {
        private int col;
        private SortOrder order;

        public MyListViewComparer(int column, SortOrder order)
        {
            col = column;
            this.order = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;

            if(((ListViewItem)x).ListView.Columns[col].Tag == null)
            {
                ((ListViewItem)x).ListView.Columns[col].Tag = "Text";
            }

            if(((ListViewItem)x).ListView.Columns[col].Tag.ToString() == "Number")
            {
                double d1 = double.Parse(((ListViewItem)x).SubItems[col].Text.Substring(0, ((ListViewItem)x).SubItems[col].Text.Length - 5));
                double d2 = double.Parse(((ListViewItem)y).SubItems[col].Text.Substring(0, ((ListViewItem)y).SubItems[col].Text.Length - 5));

                Console.WriteLine(d1);
                Console.WriteLine(d2);

                returnVal = d1.CompareTo(d2);

                if (order == SortOrder.Descending)
                {
                    returnVal *= -1;
                }
            }
            
            else if(((ListViewItem)x).ListView.Columns[col].Tag.ToString() == "Text")
            {
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);

                if (order == SortOrder.Descending)
                {
                    returnVal *= -1;
                }
            }

            //Console.WriteLine(((ListViewItem)x).SubItems[col].Text);
            //Console.WriteLine(((ListViewItem)y).SubItems[col].Text);
            //Console.WriteLine(returnVal);

            return returnVal;
        }
    }
}
