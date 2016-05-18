using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WindowsFormsApplication1
{
    public class PersonalSettings
    {
        public int money;

        public int defaultMoney;
        public string defaultDate;

        public string date = DateTime.Now.ToString("yyyyMMdd");

        public string file = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\PersonalSettings.txt";

        public void SetMoney(int value)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            money = value;
            sw.WriteLine(money);
            sw.WriteLine(defaultDate);
            sw.WriteLine(defaultMoney);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void SetDefaultMoney(int value)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            defaultMoney = value;
            sw.WriteLine(money);
            sw.WriteLine(defaultDate);
            sw.WriteLine(defaultMoney);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public void SetDefaultDate(string value)
        {
            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            defaultDate = value;
            sw.WriteLine(money);
            sw.WriteLine(defaultDate);
            sw.WriteLine(defaultMoney);

            sw.Flush();
            sw.Close();
            fs.Close();
        }



        public PersonalSettings()
        {

            if(File.Exists(file))
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                String line;
                line = sr.ReadLine();
                money = int.Parse(line);
                line = sr.ReadLine();
                defaultDate = line;
                line = sr.ReadLine();
                defaultMoney = int.Parse(line);

                sr.Close();
            }
            else
            {
                defaultDate = DateTime.Now.ToString("yyyyMMdd");
                money = 100000;
                defaultMoney = 100000;

                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(100000);
                sw.WriteLine(defaultDate);
                sw.WriteLine(100000);

                sw.Flush();
                sw.Close();
                fs.Close();
            }

        }
    }
}
