using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;


namespace WindowsFormsApplication1.Tests
{
    [TestClass()]
    public class GetHTMLTests
    {

        public class ProductInfo
        {
            public string name = "";
            public float rate;//预计年化率
            public string term = "";//投资期限（月）
            public string waysOfIncome = "";//受益方式
            public double amount;// 投资金额
        }



        string html;
        public List<ProductInfo> products = new List<ProductInfo>();
        [TestMethod()]
        public void GetHTMLTest()
        {
            try
            {
                WebClient myWebClient = new WebClient();
                //获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。
                myWebClient.Credentials = CredentialCache.DefaultCredentials;
                //从指定网站下载数据
                Byte[] pageData = myWebClient.DownloadData("https://list.lu.com/list/p2p");
                string pageHTML = Encoding.UTF8.GetString(pageData);

                html = pageHTML;

                findData();

                writeProductsData();

                StreamWriter sw = new StreamWriter("C:\\Users\\xu\\Desktop\\GetHTMLTest.html");
                sw.Write(pageHTML);
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
            }


        }

        public void findData()
        {
            int index_productInfo = html.IndexOf("product-info");
            while (index_productInfo != -1)
            {
                //查找名称
                ProductInfo product = new ProductInfo();
                int index_title = html.IndexOf("title=", index_productInfo);
                index_title += 7;
                int index_titleEnd = html.IndexOf("\"", index_title);
                product.name = html.Substring(index_title, index_titleEnd - index_title);

                //查找预计年化率
                int index_rate = html.IndexOf("num-style", index_titleEnd);
                index_rate += 11;
                int index_rateEnd = html.IndexOf("%", index_rate);
                product.rate = float.Parse(html.Substring(index_rate, index_rateEnd - index_rate));

                //查找投资期限
                int index_term = html.IndexOf("<p>", index_rateEnd);
                findString(index_term + 4, product, true);

                //查找受益方式
                int index_waysOfIncome = html.IndexOf("collection-method", index_term);
                findString(index_waysOfIncome + 20, product, false);

                //查找投资金额
                int index_amount = html.IndexOf("num-style", index_waysOfIncome);
                index_amount += 11;
                int index_amountEnd = html.IndexOf("<", index_amount);
                product.amount = double.Parse(html.Substring(index_amount, index_amountEnd - index_amount));

                //新的循环
                products.Add(product);
                index_productInfo = html.IndexOf("product-info", index_amountEnd);
            }
        }

        void findString(int index, ProductInfo product, bool isTerm)
        {
            bool isEnd = false;
            for (int i = index; true; i++)
            {
                if (html[i] == ' ')
                {
                    if (isEnd == true)
                        break;
                }
                else
                {
                    if (isTerm)
                        product.term += html[i].ToString();
                    else
                        product.waysOfIncome += html[i].ToString();
                    
                       
                    isEnd = true;
                }

            }

        }

        //写出products数据
        void writeProductsData()
        {
            FileStream fs = new FileStream("D:\\products.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for(int i = 0; i < products.Count; i++)
            {
                sw.WriteLine(products[i].name);
                sw.WriteLine(products[i].rate);
                sw.WriteLine(products[i].term);
                sw.WriteLine(products[i].waysOfIncome);
                sw.WriteLine(products[i].amount);
                sw.WriteLine();
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}