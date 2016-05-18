using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public List<double> totalIncomes = new List<double>();
        public List<double> todayIncomes = new List<double>();
        public List<double> incomeRates = new List<double>();
        public List<string> stringDays = new List<string>();


        public Form3()
        {
            InitializeComponent();

        }

       

        private void Form3_Load(object sender, EventArgs e)
        {
            //总收入
            for (int i = 0; i < 10; i++)
            {
                chart1.Series[0].Points.AddXY(int.Parse(stringDays[i]), (int)totalIncomes[i]);
            }

            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Name = "总收入";

            //今日收入
            for (int i = 0; i < 10; i++)
            {
                chart1.Series[1].Points.AddXY(int.Parse(stringDays[i]), (int)todayIncomes[i]);
            }

            this.chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.Series[1].Name = "今日收入";

            //收益率
            for (int i = 0; i < 10; i++)
            {
                
                chart1.Series[2].Points.AddXY(int.Parse(stringDays[i]), (int)incomeRates[i]);
            }

            this.chart1.Series[2].ChartType = SeriesChartType.Line;
            chart1.Series[2].Name = "收益率%";


            chart1.ChartAreas[0].AxisX.Minimum = int.Parse(stringDays[0]);
            chart1.ChartAreas[0].AxisX.Maximum = int.Parse(stringDays[9]);

            chart1.ChartAreas[0].AxisY.Minimum = (int)todayIncomes[0];
            chart1.ChartAreas[0].AxisY.Maximum = (int)todayIncomes[9];

            chart1.ChartAreas[0].AxisY2.Minimum = 0;
            chart1.ChartAreas[0].AxisY2.Maximum = 100;

            chart1.ChartAreas[0].AxisX.Title = "日期";
            chart1.ChartAreas[0].AxisY.Title = "元";
            chart1.ChartAreas[0].AxisY2.Title = "%收益率";


        }
    }
}
