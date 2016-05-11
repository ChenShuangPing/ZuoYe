using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //-------------------------------------------变量
        private GetHTML html;

        private SetExcel excel = new SetExcel();

        public PersonalSettings personalSettings = new PersonalSettings();
        //-----------------------------------------------------

        public Form1()
        {
            InitializeComponent();
        }

        private void Product_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = ((TabControl)sender).TabPages[e.Index].Text;
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat(StringFormatFlags.DirectionVertical);
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(text, SystemInformation.MenuFont, brush, e.Bounds, sf);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            textBox_Totalmoney.Text = personalSettings.money.ToString();

            //设置ListView2
            List<string[]> listA = excel.GetTheList();

            if (listA != null)
            {
                int listACount = listA.Count;
                for(int i = 0; i < listACount; i++)
                {
                    ListViewItem newItem;

                    newItem = new ListViewItem(listA[i]);
                    listView2.Items.Add(newItem);
                }
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //                               表示退格键
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
                e.Handled = true;//表示已经处理过（就是不处理）
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            //检测是否已经输入了小数点
            bool IsContainsDot = this.textBox6.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContainsDot && (e.KeyChar == 46)) //如果输入了小数点，并且再次输入
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            //检测是否已经输入了小数点
            bool IsContainsDot = this.textBox5.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContainsDot && (e.KeyChar == 46)) //如果输入了小数点，并且再次输入
            {
                e.Handled = true;
            }
        }

        //筛选
        private void button1_Click(object sender, EventArgs e)
        {
            int minDays;
            int maxDays;
            int minMoney;
            int maxMoney;
            float minRate;
            float maxRate;
            bool isDESC;//是否降序
            if (textBox1.Text == "")
            {
                minDays = 0;
            }   
            else
            {
                minDays = int.Parse(textBox1.Text) * 30;
            }

            if(textBox2.Text == "")
            {
                maxDays = -1;
            }
            else
            {
                maxDays = int.Parse(textBox2.Text) * 30;
            }

            if(textBox4.Text == "")
            {
                minMoney = 0;
            }
            else
            {
                minMoney = int.Parse(textBox4.Text) * 10000;
            }

            if(textBox3.Text == "")
            {
                maxMoney = -1;
            }
            else
            {
                maxMoney = int.Parse(textBox3.Text) * 10000;
            }

            if(textBox6.Text == "")
            {
                minRate = 0;
            }
            else
            {
                minRate = float.Parse(textBox6.Text) / 100f;
            }

            if(textBox5.Text == "")
            {
                maxRate = -1;
            }
            else
            {
                maxRate = float.Parse(textBox5.Text) / 100f;
            }

            if (comboBox1.SelectedIndex == 0)
                isDESC = true;
            else
                isDESC = false;

            

            html = new GetHTML(GetWebsite.Get(minDays, maxDays, minMoney, maxMoney, minRate, maxRate, isDESC));

            UpdateListView();

        }

        //刷新ListView
        private void UpdateListView()
        {
            //改变ListView的Item
            listView1.Items.Clear();
            int productsCount = html.products.Count;

            for (int i = 0; i < productsCount; i++)
            {
                string[] a = new string[7];
                ListViewItem newItem;
                /*
                public string name = "";
            public float rate;//预计年化率
            public string term = "";//剩余期限
            public double projectValue;//项目价值(元)
            public double interest;//利息(元)
            public string collectionDate;//预计下一收款日
            public double transferPrice;//转让价格（元）
                */
                a[0] = html.products[i].name;
                a[1] = html.products[i].rate.ToString() + "%";
                a[2] = html.products[i].term.ToString();
                a[3] = html.products[i].projectValue.ToString() + "元";
                a[4] = html.products[i].interest.ToString() + "元";
                a[5] = html.products[i].transferPrice.ToString() + "元";
                a[6] = html.products[i].collectionDate.Substring(8);

                newItem = new ListViewItem(a);

                listView1.Items.Add(newItem);
            }

            //设置当前页
            int page = GetTheCurrentPage();
            labelPage.Text = page.ToString() + "页";
        }


        //上一页
        private void button2_Click(object sender, EventArgs e)
        {
            if (html.website == "")
                return;

            int oldPage = GetTheCurrentPage();
            if (oldPage == 1)
                return;

            int newPage = oldPage - 1;
            SetTheCurrentPage(newPage);
            html.UpdateHTML();
            UpdateListView();

        }

        private int GetTheCurrentPage()
        {
            int index_CurrentPage = html.website.IndexOf("currentPage");
            index_CurrentPage += 12;
            int index_CurrentPageEnd = html.website.IndexOf("&", index_CurrentPage);

            return int.Parse(html.website.Substring(index_CurrentPage, index_CurrentPageEnd - index_CurrentPage));
        }

        private void SetTheCurrentPage(int newP)
        {
            int index_CurrentPage = html.website.IndexOf("currentPage");
            index_CurrentPage += 12;
            int index_CurrentPageEnd = html.website.IndexOf("&", index_CurrentPage);

            html.website = html.website.Remove(index_CurrentPage, index_CurrentPageEnd - index_CurrentPage);
            html.website = html.website.Insert(index_CurrentPage, newP.ToString());
        }

        //下一页
        private void button3_Click(object sender, EventArgs e)
        {
            if (html.website == "")
                return;

            SetTheCurrentPage(GetTheCurrentPage() + 1);
            html.UpdateHTML();
            UpdateListView();

        }

        //跳转
        private void button4_Click(object sender, EventArgs e)
        {
            if (html.website == "" || pageTextBox.Text == "")
                return;

            SetTheCurrentPage(int.Parse(pageTextBox.Text));
            html.UpdateHTML();
            UpdateListView();

        }

        //双击ListView
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listView1.Items != null)
            {
                string name = listView1.SelectedItems[0].Text;
                DialogResult dr = MessageBox.Show("是否投资项目：" + name, "投资", MessageBoxButtons.YesNo);
                if(dr == DialogResult.Yes)
                {
                    Form2 form2 = new Form2();
                    form2.label_Money.Text = personalSettings.money.ToString();
                    if(form2.ShowDialog() == DialogResult.OK)
                    {
                        //在excel中添加
                        excel.AddLine("", "购买", form2.money.ToString(), name);
                        personalSettings.SetMoney(personalSettings.money - form2.money);
                        textBox_Totalmoney.Text = personalSettings.money.ToString();

                        //更新ListView2
                        string[] a = new string[4];
                        ListViewItem newItem;

                        a[0] = "";
                        a[1] = "购买";
                        a[2] = form2.money.ToString();
                        a[3] = name;

                        newItem = new ListViewItem(a);
                        listView2.Items.Add(newItem);

                        tabControl1.SelectedIndex = 2;

                    }
                }
            }
        }
    }
}
