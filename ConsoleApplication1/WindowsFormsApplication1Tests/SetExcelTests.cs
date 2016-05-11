using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using System.IO;

namespace WindowsFormsApplication1.Tests
{
    [TestClass()]
    public class SetExcelTests
    {
        [TestMethod()]
        public void CreatExcelTest()
        {
            string file = "D:\\Users\\xu\\Desktop\\test.xlsx";

            ExcelPackage excel = new ExcelPackage(new FileInfo(file));
            ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

            sheet.Cells[1, 1].Value = "日期";
            sheet.Cells[1, 2].Value = "操作";
            sheet.Cells[1, 3].Value = "金额";
            sheet.Cells[1, 4].Value = "项目编号";

            sheet.Cells[2, 1].Value = "张三";
            sheet.Cells[2, 2].Value = 29;
            sheet.Cells[2, 3].Value = "男";

            sheet.Cells[3, 1].Value = "李四";
            sheet.Cells[3, 2].Value = 35;
            sheet.Cells[3, 3].Value = "男";

            sheet.Cells[4, 1].Value = "王五";
            sheet.Cells[4, 2].Value = 20;
            sheet.Cells[4, 3].Value = "女";

            excel.Save();

        }
    }
}