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

        //同步可用资金和日期
        private void UpdateTotalMoneyAndDate()
        {
            
            //初始资金
            label_DefaultMoney.Text = personalSettings.defaultMoney.ToString();
            textBox_DefaultMoney.Text = personalSettings.defaultMoney.ToString();
            //可用资金
            label_TotalMoney.Text = personalSettings.money.ToString();
            textBox_Totalmoney.Text = personalSettings.money.ToString();
            //同步日期
            textBox_Date.Text = personalSettings.date;
            label_Date.Text = textBox_Date.Text;
            //初始日期
            textBox_DefaultDate.Text = personalSettings.defaultDate;
            label_DefaultDate.Text = personalSettings.defaultDate;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            UpdateTotalMoneyAndDate();

            //设置ListView2
            List<string[]> listA = excel.listOfExcel;
            listView2.Items.Clear();
            if (listA != null)
            {
                int listACount = listA.Count;
                for (int i = 0; i < listACount; i++)
                {
                    ListViewItem newItem;

                    string[] newString = new string[5];
                    for (int j = 0; j < 4; j++)
                        newString[j] = listA[i][j];
                    newString[4] = "";
                    newItem = new ListViewItem(newString);
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

            try
            {
                if (textBox1.Text == "")
                {
                    minDays = 0;
                }
                else
                {
                    minDays = int.Parse(textBox1.Text) * 30;
                }

                if (textBox2.Text == "")
                {
                    maxDays = -1;
                }
                else
                {
                    maxDays = int.Parse(textBox2.Text) * 30;
                }

                if (textBox4.Text == "")
                {
                    minMoney = 0;
                }
                else
                {
                    minMoney = int.Parse(textBox4.Text) * 10000;
                }

                if (textBox3.Text == "")
                {
                    maxMoney = -1;
                }
                else
                {
                    maxMoney = int.Parse(textBox3.Text) * 10000;
                }

                if (textBox6.Text == "")
                {
                    minRate = 0;
                }
                else
                {
                    minRate = float.Parse(textBox6.Text) / 100f;
                }

                if (textBox5.Text == "")
                {
                    maxRate = -1;
                }
                else
                {
                    maxRate = float.Parse(textBox5.Text) / 100f;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (comboBox1.SelectedIndex == 0)
                isDESC = true;
            else
                isDESC = false;

            //异步调用
            PrintResultDel del = new PrintResultDel(PrintResult);
            IAsyncResult ar = del.BeginInvoke(minDays, maxDays, minMoney, maxMoney, minRate,
                                              maxRate, isDESC,
                                              new AsyncCallback(PrintResultCallBack), del);

            

        }

        //处理html函数
        private void PrintResult(int minDays, int maxDays, int minMoney, int maxMoney,
                                  float minRate, float maxRate, bool isDESC)
        {
            button1.Visible = false;
            html = new GetHTML(GetWebsite.Get(minDays, maxDays, minMoney, maxMoney, minRate, maxRate, isDESC));
        }

        //声明委托
        private delegate void PrintResultDel(int minDays, int maxDays, int minMoney, int maxMoney,
                                  float minRate, float maxRate, bool isDESC);

        //异步回调方法
        private void PrintResultCallBack(IAsyncResult ar)
        {
            button1.Visible = true;
            UpdateListView(listView1);
        }

        //刷新ListView
        private void UpdateListView(ListView listView)
        {
            //改变ListView的Item
            listView.Items.Clear();
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

                listView.Items.Add(newItem);
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
            UpdateListView(listView1);

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
            UpdateListView(listView1);

        }

        //跳转
        private void button4_Click(object sender, EventArgs e)
        {
            if (html.website == "" || pageTextBox.Text == "")
                return;

            SetTheCurrentPage(int.Parse(pageTextBox.Text));
            html.UpdateHTML();
            UpdateListView(listView1);

        }

        //双击ListView
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.Items != null)
            {
                string name = listView1.SelectedItems[0].Text;
                DialogResult dr = MessageBox.Show("是否投资项目：" + name, "投资", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Form2 form2 = new Form2();
                    form2.label_Money.Text = personalSettings.money.ToString();
                    form2.textBox2.Text = personalSettings.date;
                    if (form2.ShowDialog() == DialogResult.OK)
                    {
                        //在excel中添加
                        excel.AddLine(form2.date, "购买", form2.money.ToString(), name);
                        //更新exceList数据
                        excel.listOfExcel = excel.GetTheList();

                        personalSettings.SetMoney(personalSettings.money - form2.money);

                        //更新ListView2
                        string[] a = new string[4];
                        ListViewItem newItem;

                        a[0] = form2.date;
                        a[1] = "购买";
                        a[2] = form2.money.ToString();
                        a[3] = name;

                        newItem = new ListViewItem(a);
                        listView2.Items.Add(newItem);

                        tabControl1.SelectedIndex = 2;

                        //更新可用资金和日期
                        UpdateTotalMoneyAndDate();

                        //更新最后记录时间
                        excel.UpdateTheLastDate();

                    }
                }
            }
        }


        //设置初始资金
        private void button6_Click(object sender, EventArgs e)
        {
            
            int value;
            try
            {
                value = int.Parse(textBox_DefaultMoney.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (value < 0)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

            personalSettings.SetDefaultMoney(value);
            UpdateTotalMoneyAndDate();

            

            button7_Click(sender, e);

            
        }

        //设置日期
        private void button7_Click(object sender, EventArgs e)
        {
            string date = textBox_Date.Text;

            //判断是不是合法日期
            if (!IsLegalDate(date))
            {
                MessageBox.Show("输入的日期不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //判断excel表格里面存在数据的时间
            if(excel.lastDate != "0")
            {
                if(int.Parse(excel.lastDate) > int.Parse(date))
                {
                    MessageBox.Show("输入的日期与存储投资记录冲突", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //设置date
            personalSettings.date = date;
            UpdateTotalMoneyAndDate();

            if (excel.listOfExcel.Count != 0)
            {
                CalculateIncome(date);
                UpdateListView2(excel.listOfExcel);
            }

            MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        //更新ListView2和label_TotalIncome（即所有收益赎回和未赎回的）和label_TodayIncome和label_IncomeRate
        private void UpdateListView2(List<string[]> listOfExcel)
        {
            int listACount = listOfExcel.Count;
            double totalIncome = 0;
            double todayIncome = 0;
            double incomeRate = 0;
            listView2.Items.Clear();
            for (int i = 0; i < listACount; i++)
            {
                ListViewItem newItem;
                string[] newString = new string[5];//写入ListView2的Item
                for (int x = 0; x < 4; x++)
                    newString[x] = listOfExcel[i][x];
                double newDouble = double.Parse(listOfExcel[i][4]);
                newString[4] = newDouble.ToString("f3");
                newItem = new ListViewItem(newString);
                listView2.Items.Add(newItem);
                if (listOfExcel[i][5] == "1")
                {
                   
                    continue;
                }
                    

                //计算totalIncome
                totalIncome += double.Parse(listOfExcel[i][4]);

                //计算todayIncome
                if(listOfExcel[i][1] == "购买")
                    todayIncome += double.Parse(listOfExcel[i][4]);
            }

            //设置label_TotalIncome
            label_TotalIncome.Text = totalIncome.ToString("f3");

            //设置label_TodayIncome
            label_TodayIncome.Text = todayIncome.ToString("f3");

            //设置label_IncomeRate
            DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.defaultDate));
            DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.date));
            TimeSpan span = newDate.Subtract(oldDate);
            //日期相差的天数
            int dayDiff = span.Days;
            incomeRate = totalIncome / personalSettings.defaultMoney / (dayDiff) * 365;
            incomeRate *= 100;//百分化处理;
            label_IncomeRate.Text = incomeRate.ToString("f2");
        }



        //计算每个项目的收益
        private void CalculateIncome(string date)
        {
            List<string[]> listOfExcel = excel.listOfExcel;//设置每个项目的收益
            int listOfExcelCount = listOfExcel.Count;
            for (int i = 0; i < listOfExcelCount; i++)
            {
                if (listOfExcel[i][5] == "1")
                    continue;
                double income;
                if (listOfExcel[i][1] == "赎回")
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (listOfExcel[j][5] == "0")
                            continue;
                        if (listOfExcel[i][3] == listOfExcel[j][3])
                        {
                            income = double.Parse(listOfExcel[i][2]) - double.Parse(listOfExcel[j][2]);
                            listOfExcel[i][4] = income.ToString();
                            listOfExcel[j][4] = income.ToString();
                            break;
                        }
                    }
                    continue;
                }
                DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(listOfExcel[i][0]));
                DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(date));
                TimeSpan span = newDate.Subtract(oldDate);
                //日期相差的天数
                int dayDiff = span.Days;

                income = dayDiff * double.Parse(listOfExcel[i][2]) * 0.84 / 365;
                //收益
                listOfExcel[i][4] = income.ToString();

            }

        }


        //判断日期是否合法
        private bool IsLegalDate(string date)
        {
            if (date.Length != 8)
                return false;

            int month = int.Parse(date.Substring(4, 2));
            if (month > 12)
                return false;

            int year = int.Parse(date.Substring(0, 4));

            int day = int.Parse(date.Substring(6, 2));
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                        return false;
                    break;
                case 2:
                    if (((year % 4 == 0) && (year % 100 != 0))
                        || (year % 400 == 0))
                    {
                        if (day > 29)
                            return false;
                    }
                    else
                    {
                        if (day > 28)
                            return false;
                    }
                    break;

                default:
                    if (day > 30)
                        return false;
                    break;

            }

            return true;
        }

        private string ChangeDateToDateTimeType(string date)
        {
            date = date.Insert(4, "-");
            date = date.Insert(7, "-");
            return date;
        }

        //设置可用资金
        private void button9_Click(object sender, EventArgs e)
        {
            int value;
            try
            {
                value = int.Parse(textBox_Totalmoney.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (value < 0)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            personalSettings.SetMoney(value);
            UpdateTotalMoneyAndDate();

            

            MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            string date = textBox_DefaultDate.Text;

            //判断是不是合法日期
            if (!IsLegalDate(date))
            {
                MessageBox.Show("输入的日期不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

           

            //设置date
            personalSettings.SetDefaultDate(date);
            UpdateTotalMoneyAndDate();

            if (excel.listOfExcel.Count != 0)
            {
                CalculateIncome(personalSettings.date);
                UpdateListView2(excel.listOfExcel);
            }
  
            MessageBox.Show("设置成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //更新
        private void button10_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);
        }

        //赎回
        private void button5_Click(object sender, EventArgs e)
        {
            int indexOfItem = 0;
            ListViewItem selectedItem = null;
            for(int i = 0; i < listView2.Items.Count; i++)
            {
                if(listView2.Items[i].Selected == true)
                {
                    selectedItem = listView2.Items[i];
                    indexOfItem = i;
                    break;
                }
            }
            if(selectedItem == null)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            if (excel.listOfExcel[indexOfItem][1] == "赎回" || excel.listOfExcel[indexOfItem][5] == "1")
            {
                MessageBox.Show("项目已被赎回");
                return;
            }

            Form4 form4 = new Form4();
            form4.textBox2.Text = personalSettings.date;
            form4.oldDate = excel.listOfExcel[indexOfItem][0];
            if (form4.ShowDialog() == DialogResult.OK)
            {
                DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(excel.listOfExcel[indexOfItem][0]));
                DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(form4.textBox2.Text));
                TimeSpan span = newDate.Subtract(oldDate);
                //日期相差的天数
                int dayDiff = span.Days;
                double income = dayDiff * double.Parse(excel.listOfExcel[indexOfItem][2]) * 0.84 / 365 + double.Parse(excel.listOfExcel[indexOfItem][2]);
                excel.AddLine(form4.textBox2.Text, "赎回", income.ToString(), excel.listOfExcel[indexOfItem][3]);

                //更新exceList数据
                excel.listOfExcel = excel.GetTheList();

                //设置ListView2
                List<string[]> listA = excel.listOfExcel;
                listView2.Items.Clear();
                if (listA != null)
                {
                    int listACount = listA.Count;
                    for (int i = 0; i < listACount; i++)
                    {
                        ListViewItem newItem;

                        string[] newString = new string[5];
                        for (int j = 0; j < 4; j++)
                            newString[j] = listA[i][j];
                        newString[4] = "";
                        newItem = new ListViewItem(newString);
                        listView2.Items.Add(newItem);
                    }
                }

                //更新可用资金
                personalSettings.SetMoney(personalSettings.money + (int)income);

                UpdateTotalMoneyAndDate();

            }


        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel|*.xlsx";
            openFileDialog.RestoreDirectory = true;
            string fName;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fName = openFileDialog.FileName;
                try
                {
                    excel = new SetExcel(fName);

                    UpdateTotalMoneyAndDate();
                    

                    //设置ListView2
                    List<string[]> listA = excel.listOfExcel;

                    if (listA != null)
                    {
                        int listACount = listA.Count;
                        listView2.Items.Clear();
                        for (int i = 0; i < listACount; i++)
                        {
                            ListViewItem newItem;

                            string[] newString = new string[5];
                            for (int j = 0; j < 4; j++)
                                newString[j] = listA[i][j];
                            newString[4] = "";
                            newItem = new ListViewItem(newString);
                            listView2.Items.Add(newItem);
                        }
                    }
                    MessageBox.Show("导入成功");
                }
                catch(Exception ex)
                {
                    MessageBox.Show("导入失败");
                }
                
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel|*.xlsx";
            //保存对话框是否记忆上次打开的目录 
            sfd.RestoreDirectory = true;
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.Copy(excel.file, sfd.FileName, true);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("导出错误");
                }

                MessageBox.Show("导出成功");
            }
            
        }

        //显示图表
        private void button14_Click(object sender, EventArgs e)
        {
            if(int.Parse(personalSettings.date) <= int.Parse(personalSettings.defaultDate))
            {
                MessageBox.Show("当前日期不能大于或等于初始日期");
                return;
            }


            List<double> totalIncomes = new List<double>();
            List<double> todayIncomes = new List<double>();
            List<double> incomeRates = new List<double>();
            List<string> stringDays = new List<string>();
            List<DateTime> days = new List<DateTime>();
            

            //相差天数（取其中的10天）
            DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.defaultDate));
            DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.date));
            TimeSpan span = newDate.Subtract(oldDate);
            //日期相差的天数
            int dayDiff = span.Days;

            float dayInterval = (float)dayDiff / 10f;

            for (int i = 0; i < 9; i++)
                days.Add(oldDate.AddDays((int)(i * dayInterval)));
            days.Add(newDate);

            for(int i = 0; i < 10; i++)
            {
                List<string[]> listOfExcel = new List<string[]>(excel.listOfExcel);
                /*for(int j = 0; j < excel.listOfExcel.Count; j++)
                {
                    listOfExcel.Add(excel.listOfExcel[j]);
                }*/
                string date = days[i].ToString("yyyyMMdd");
                stringDays.Add(date);
                ResetListOfExcel(listOfExcel, date);
                CalculateIncomeForChart(listOfExcel, date);
                UpdateForChart(listOfExcel, totalIncomes, todayIncomes, incomeRates);
            }

            Form3 form3 = new Form3();
            form3.todayIncomes = todayIncomes;
            form3.totalIncomes = totalIncomes;
            form3.incomeRates = incomeRates;
            form3.stringDays = stringDays;
            form3.Show();
        }

        private void ResetListOfExcel(List<string[]> listOfExcel, string date)
        {
            for (int i = 0; i < listOfExcel.Count; i++)
            {
                //删除超过的时间
                if (int.Parse(listOfExcel[i][0]) > int.Parse(date))
                {
                    listOfExcel.RemoveAt(i);
                    i--;
                }
            }


            //重新设置
            for (int i = 0; i < listOfExcel.Count; i++)
            {
                listOfExcel[i][4] = "";
                listOfExcel[i][5] = "0";

                if (listOfExcel[i][1] == "赎回")
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (listOfExcel[j][3] == listOfExcel[i][3])//名字相同
                        {
                            listOfExcel[j][5] = "1";//代表已经赎回
                            break;
                        }

                    }
                }

            }


        }


        //计算每个项目的收益
        private void CalculateIncomeForChart(List<string[]> listOfExcel, string date)
        {
            int listOfExcelCount = listOfExcel.Count;
            for (int i = 0; i < listOfExcelCount; i++)
            {
                if (listOfExcel[i][5] == "1")
                    continue;
                double income;
                if (listOfExcel[i][1] == "赎回")
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (listOfExcel[j][5] == "0")
                            continue;
                        if (listOfExcel[i][3] == listOfExcel[j][3])
                        {
                            income = double.Parse(listOfExcel[i][2]) - double.Parse(listOfExcel[j][2]);
                            listOfExcel[i][4] = income.ToString();
                            listOfExcel[j][4] = income.ToString();
                            break;
                        }
                    }
                    continue;
                }
                DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(listOfExcel[i][0]));
                DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(date));
                TimeSpan span = newDate.Subtract(oldDate);
                //日期相差的天数
                int dayDiff = span.Days;

                income = dayDiff * double.Parse(listOfExcel[i][2]) * 0.84 / 365;
                //收益
                listOfExcel[i][4] = income.ToString();

            }

        }

        //更新ListView2和label_TotalIncome（即所有收益赎回和未赎回的）和label_TodayIncome和label_IncomeRate
        private void UpdateForChart(List<string[]> listOfExcel,
                                     List<double> totalIncomes, 
                                       List<double> todayIncomes, 
                                        List<double> incomeRates)
        {
            int listACount = listOfExcel.Count;
            double totalIncome = 0;
            double todayIncome = 0;
            double incomeRate = 0;
            for (int i = 0; i < listACount; i++)
            {
                if (listOfExcel[i][5] == "1")
                {
                    continue;
                }


                //计算totalIncome
                totalIncome += double.Parse(listOfExcel[i][4]);

                //计算todayIncome
                if (listOfExcel[i][1] == "购买")
                    todayIncome += double.Parse(listOfExcel[i][4]);
            }

            //设置totalIncomes
            totalIncomes.Add(totalIncome);

            //设置todayIncomes
            todayIncomes.Add(todayIncome);

            //设置incomeRates
            DateTime oldDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.defaultDate));
            DateTime newDate = Convert.ToDateTime(ChangeDateToDateTimeType(personalSettings.date));
            TimeSpan span = newDate.Subtract(oldDate);
            //日期相差的天数
            int dayDiff = span.Days;
            incomeRate = totalIncome / personalSettings.defaultMoney / (dayDiff) * 365;
            incomeRate *= 100;//百分化处理;
            incomeRates.Add(incomeRate);
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            //检测是否已经输入了小数点
            bool IsContainsDot = this.textBox9.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContainsDot && (e.KeyChar == 46)) //如果输入了小数点，并且再次输入
            {
                e.Handled = true;
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            //检测是否已经输入了小数点
            bool IsContainsDot = this.textBox8.Text.Contains(".");
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8) && (e.KeyChar != 46))
            {
                e.Handled = true;
            }
            else if (IsContainsDot && (e.KeyChar == 46)) //如果输入了小数点，并且再次输入
            {
                e.Handled = true;
            }
        }

        int minDays;
        int maxDays;
        int minMoney;
        int maxMoney;
        float minRate;
        float maxRate;
        bool isDESC;//是否降序
        private void button17_Click(object sender, EventArgs e)
        {
           

            try
            {
                if (textBox13.Text == "")
                {
                    minDays = 0;
                }
                else
                {
                    minDays = int.Parse(textBox13.Text) * 30;
                }

                if (textBox12.Text == "")
                {
                    maxDays = -1;
                }
                else
                {
                    maxDays = int.Parse(textBox12.Text) * 30;
                }

                if (textBox11.Text == "")
                {
                    minMoney = 0;
                }
                else
                {
                    minMoney = int.Parse(textBox11.Text) * 10000;
                }

                if (textBox10.Text == "")
                {
                    maxMoney = -1;
                }
                else
                {
                    maxMoney = int.Parse(textBox10.Text) * 10000;
                }

                if (textBox9.Text == "")
                {
                    minRate = 0;
                }
                else
                {
                    minRate = float.Parse(textBox9.Text) / 100f;
                }

                if (textBox8.Text == "")
                {
                    maxRate = -1;
                }
                else
                {
                    maxRate = float.Parse(textBox8.Text) / 100f;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (comboBox2.SelectedIndex == 0)
                isDESC = true;
            else
                isDESC = false;

            //异步定时器
           /* HideButton7Del del = new HideButton7Del(HideButton7);
            IAsyncResult ar = del.BeginInvoke(new AsyncCallback(HideButton7CallBack), del);*/
            timer1 = new Timer();
            timer1.Interval = 3000;
            timer1.Tick += new EventHandler(theout);
            timer1.Start();
            button17.Visible = false;
            label42.Visible = true;
            button13.Visible = true;
        }


        //设置定时器
        public Timer timer1;
        //定时器委托函数
        public void theout(object source, EventArgs e)
        {
            html = new GetHTML(GetWebsite.Get(minDays, maxDays, minMoney, maxMoney, minRate, maxRate, isDESC));
            UpdateListView3(listView3);
        }

        private void UpdateListView3(ListView listView)
        {
            //改变ListView的Item
            listView.Items.Clear();
            int productsCount = html.products.Count;

            if(productsCount != 0)
            {
                timer1.Stop();
                button17.Visible = true;
                label42.Visible = false;
                button13.Visible = false;
            }
                

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

                listView.Items.Add(newItem);
            }

           
        }

        private void button13_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button17.Visible = true;
            label42.Visible = false;
            button13.Visible = false;
        }

        private void listView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView3.Items != null)
            {
                string name = listView3.SelectedItems[0].Text;
                DialogResult dr = MessageBox.Show("是否投资项目：" + name, "投资", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    Form2 form2 = new Form2();
                    form2.label_Money.Text = personalSettings.money.ToString();
                    form2.textBox2.Text = personalSettings.date;
                    if (form2.ShowDialog() == DialogResult.OK)
                    {
                        //在excel中添加
                        excel.AddLine(form2.date, "购买", form2.money.ToString(), name);
                        //更新exceList数据
                        excel.listOfExcel = excel.GetTheList();

                        personalSettings.SetMoney(personalSettings.money - form2.money);

                        //更新ListView2
                        string[] a = new string[4];
                        ListViewItem newItem;

                        a[0] = form2.date;
                        a[1] = "购买";
                        a[2] = form2.money.ToString();
                        a[3] = name;

                        newItem = new ListViewItem(a);
                        listView2.Items.Add(newItem);

                        tabControl1.SelectedIndex = 2;

                        //更新可用资金和日期
                        UpdateTotalMoneyAndDate();

                        //更新最后记录时间
                        excel.UpdateTheLastDate();

                    }
                }
            }
        }
    }
}
