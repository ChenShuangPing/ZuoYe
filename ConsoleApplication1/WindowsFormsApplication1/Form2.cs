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
    public partial class Form2 : Form
    {
        public int money;
 



        public Form2()
        {
            InitializeComponent();
        }


        //投资按钮
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                return;

            money = int.Parse(textBox1.Text);

            if(money > int.Parse(label_Money.Text))
            {
                MessageBox.Show("资金不足", "警告");
                return;
            }

            //设置对话模式结果为OK
            this.DialogResult = DialogResult.OK;

            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //                               表示退格键
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
                e.Handled = true;//表示已经处理过（就是不处理）
        }
    }
}
