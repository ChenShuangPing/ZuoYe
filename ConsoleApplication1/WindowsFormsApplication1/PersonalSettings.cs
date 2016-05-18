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

        public void SetMoney(int m)
        {
            string file = "C:\\Users\\Administrator\\Desktop\\PersonalSettings.txt";

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            money = m;
            sw.WriteLine(money);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public PersonalSettings()
        {
            string file = "C:\\Users\\Administrator\\Desktop\\PersonalSettings.txt";

            if(File.Exists(file))
            {
                StreamReader sr = new StreamReader(file, Encoding.Default);
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    money = int.Parse(line);
                }

                sr.Close();
            }
            else
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(100000);
                money = 100000;

                sw.Flush();
                sw.Close();
                fs.Close();
            }

           
        }
    }
}
