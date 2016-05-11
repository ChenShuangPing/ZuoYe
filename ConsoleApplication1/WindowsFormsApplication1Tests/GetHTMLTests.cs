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
            public string term = "";//剩余期限
            public double projectValue;//项目价值(元)
            public string depreciate = "";//降价
            public double interest;//利息(元)
            public string collectionDate = "";//预计下一收款日
            public double transferPrice;//转让价格（元）
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
                Byte[] pageData = myWebClient.DownloadData("https://list.lu.com/list/transfer-p2p");
                string pageHTML = Encoding.UTF8.GetString(pageData);

                html = pageHTML;
                findData();

                //test专属
                writeProductsData();

                StreamWriter sw = new StreamWriter("D:\\Users\\xu\\Desktop\\GetHTMLTest.html");
                sw.Write(pageHTML);
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
            }
        }

        [TestMethod()]
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

                //查找剩余期限
                int index_term = html.IndexOf("<p>", index_rateEnd);
                //findString(index_term + 4, product, true);
                index_term += 3;
                int index_termEnd = html.IndexOf("<", index_term);
                product.term = html.Substring(index_term, index_termEnd - index_term);

                //查找项目价值
                int index_ProjectValue = html.IndexOf("currency", index_termEnd);
                index_ProjectValue += 10;
                int index_ProjectValueEnd = html.IndexOf("元", index_ProjectValue);
                product.projectValue = double.Parse(html.Substring(index_ProjectValue, index_ProjectValueEnd - index_ProjectValue));

                //查找降价
                int index_Depreciate = html.IndexOf("<p>", index_ProjectValueEnd);
                product.depreciate = findString(index_Depreciate + 4);

                //查找利息
                int index_Interest = html.IndexOf("利息", index_Depreciate);
                index_Interest += 2;
                int index_InterestEnd = html.IndexOf("元", index_Interest);
                product.interest = double.Parse(html.Substring(index_Interest, index_InterestEnd - index_Interest));

                //查找预计下一收款日
                int index_CollectionDate = html.IndexOf("<span>", index_InterestEnd);
                index_CollectionDate += 6;
                int index_CollectionDateEnd = html.IndexOf("<", index_CollectionDate);
                product.collectionDate = html.Substring(index_CollectionDate, index_CollectionDateEnd - index_CollectionDate);

                //查找转让价格
                int index_TransferPrice = html.IndexOf("num-style", index_CollectionDateEnd);
                index_TransferPrice += 11;
                int index_TransferPriceEnd = html.IndexOf("<", index_TransferPrice);
                product.transferPrice = double.Parse(html.Substring(index_TransferPrice, index_TransferPriceEnd - index_TransferPrice));

                //新的循环
                products.Add(product);
                index_productInfo = html.IndexOf("product-info", index_TransferPriceEnd);

            }
        }

        string findString(int index)
        {
            string a = "";
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
                    a += html[i].ToString();
                    isEnd = true;
                }

            }
            return a;
        }

        //写出products数据
        void writeProductsData()
        {
            FileStream fs = new FileStream("D:\\Users\\xu\\Desktop\\products.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            for(int i = 0; i < products.Count; i++)
            {
                sw.WriteLine(products[i].name);
                sw.WriteLine(products[i].rate);
                sw.WriteLine(products[i].term);
                sw.WriteLine(products[i].projectValue);
                sw.WriteLine(products[i].depreciate);
                sw.WriteLine(products[i].interest);
                sw.WriteLine(products[i].collectionDate);
                sw.WriteLine(products[i].transferPrice);
                sw.WriteLine();
            }

            sw.Flush();
            sw.Close();
            fs.Close();
        }
    }
}