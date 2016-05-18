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

    public partial class Form4 : Form
    {
        public string oldDate;

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string date = textBox2.Text;

            //判断是不是合法日期
            if (!IsLegalDate(date))
            {
                MessageBox.Show("输入的日期不合法", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (oldDate != "0")
            {
                if (int.Parse(oldDate) > int.Parse(date))
                {
                    MessageBox.Show("输入的日期与存储投资记录冲突", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //设置对话模式结果为OK
            this.DialogResult = DialogResult.OK;

            this.Close();


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
    }



}
