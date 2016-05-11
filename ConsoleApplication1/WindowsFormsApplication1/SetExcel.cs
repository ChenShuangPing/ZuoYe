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
        private ExcelPackage excel;
        private ExcelWorksheet sheet;
        private int line;//可以加入的那一行
        public SetExcel()
        {
            OpenExcel();

            sheet.Cells[1, 1].Value = "日期";
            sheet.Cells[1, 2].Value = "操作";
            sheet.Cells[1, 3].Value = "金额";
            sheet.Cells[1, 4].Value = "项目编号";

            line = sheet.Dimension.End.Row + 1;

            excel.Save();
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
            string file = "D:\\Users\\xu\\Desktop\\test.xlsx";

            excel = new ExcelPackage(new FileInfo(file));
            if (excel.Workbook.Worksheets.Count == 0)
                sheet = excel.Workbook.Worksheets.Add("sheet1");
            else
                sheet = excel.Workbook.Worksheets[1];
        }

        public List<string[]> GetTheList()
        {
            if (line == 2)
                return null;

            List<string[]> listA = new List<string[]>();

            OpenExcel();

            for(int i = 2; i < line; i++)
            {
                string []a = new string[4];
                a[0] = sheet.Cells[i, 1].Value as string;
                a[1] = sheet.Cells[i, 2].Value as string;
                a[2] = sheet.Cells[i, 3].Value as string;
                a[3] = sheet.Cells[i, 4].Value as string;

                listA.Add(a);

            }

            return listA;
        }


    }
}
