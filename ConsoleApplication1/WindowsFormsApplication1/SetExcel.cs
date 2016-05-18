using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using System.IO;

namespace WindowsFormsApplication1
{
    public class SetExcel
    {
        public string lastDate;//找出最后的那天（防止设置的时候把时间往前面设）
        public List<string[]> listOfExcel;

        private ExcelPackage excel;
        private ExcelWorksheet sheet;
        private int line;//可以加入的那一行

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\test.xlsx";

        public SetExcel()
        {
            OpenExcel();

            sheet.Cells[1, 1].Value = "日期";
            sheet.Cells[1, 2].Value = "操作";
            sheet.Cells[1, 3].Value = "金额";
            sheet.Cells[1, 4].Value = "项目编号";

            line = sheet.Dimension.End.Row + 1;

            listOfExcel = GetTheList();//获取excel表格的内容（附带自己添加的内容）

            if (line == 2)
                lastDate = "0";
            else
            {
                UpdateTheLastDate();//更新最后的期限
            }
            excel.Save();
        }

        public SetExcel(string f)
        {
            file = f;
            OpenExcel(f);

            sheet.Cells[1, 1].Value = "日期";
            sheet.Cells[1, 2].Value = "操作";
            sheet.Cells[1, 3].Value = "金额";
            sheet.Cells[1, 4].Value = "项目编号";

            line = sheet.Dimension.End.Row + 1;

            listOfExcel = GetTheList();//获取excel表格的内容（附带自己添加的内容）

            if (line == 2)
                lastDate = "0";
            else
            {
                UpdateTheLastDate();//更新最后的期限
            }
            excel.Save();
        }


        public void UpdateTheLastDate()
        {
            int last = 0;
            for (int i = 0; i < listOfExcel.Count; i++)
            {
                if (listOfExcel[i][1] == "赎回" || listOfExcel[i][5] == "1")
                    continue;
                int value = int.Parse(listOfExcel[i][0]);
                if (last < value)
                    last = value;
            }
            lastDate = last.ToString();
        }

        public void CreatExcel()
        {
            

        }

        public void AddLine(string name, string state, string money, string date)
        {
            OpenExcel();

            sheet.Cells[line, 1].Value = name;
            sheet.Cells[line, 2].Value = state;
            sheet.Cells[line, 3].Value = money;
            sheet.Cells[line, 4].Value = date;
            line++;

            excel.Save();
        }

        private void OpenExcel()
        {

            excel = new ExcelPackage(new FileInfo(file));
            if (excel.Workbook.Worksheets.Count == 0)
                sheet = excel.Workbook.Worksheets.Add("sheet1");
            else
                sheet = excel.Workbook.Worksheets[1];
        }

        private void OpenExcel(string f)
        {

            excel = new ExcelPackage(new FileInfo(f));
            if (excel.Workbook.Worksheets.Count == 0)
                sheet = excel.Workbook.Worksheets.Add("sheet1");
            else
                sheet = excel.Workbook.Worksheets[1];
        }

        public List<string[]> GetTheList()
        {

            List<string[]> listA = new List<string[]>();
            if (line == 2)
                return listA;

            OpenExcel();

            for(int i = 2; i < line; i++)
            {
                string []a = new string[6];
                a[0] = sheet.Cells[i, 1].Value as string;
                a[1] = sheet.Cells[i, 2].Value as string;
                a[2] = sheet.Cells[i, 3].Value as string;
                a[3] = sheet.Cells[i, 4].Value as string;
                a[4] = "";//求收益用的
                a[5] = "0";//代表未被赎回

                if(a[1] == "赎回")
                {
                    for(int j = 0; j < listA.Count; j++)
                    {
                        if(listA[j][3] == a[3])//名字相同
                        {
                            listA[j][5] = "1";//代表已经赎回
                            break;
                        }
                            
                    }
                }

                listA.Add(a);

            }

            return listA;
        }


    }
}
