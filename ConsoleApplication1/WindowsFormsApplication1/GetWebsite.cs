using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WindowsFormsApplication1
{
    class GetWebsite
    {

        static public string Get(int minD, int maxD, int minM, int maxM, float minR, float maxR, bool isD)
        {
            string website = "";
            if (isD)
                website += "https://list.lu.com/list/transfer-p2p?minMoney=&maxMoney=&minDays=&maxDays=&minRate=&maxRate=&mode=&tradingMode=&isCx=&currentPage=1&orderCondition=INVEST_RATE_DESC&isShared=&canRealized=&productCategoryEnum=";
            else
                website += "https://list.lu.com/list/transfer-p2p?minMoney=&maxMoney=&minDays=&maxDays=&minRate=&maxRate=&mode=&tradingMode=&isCx=&currentPage=1&orderCondition=INVEST_RATE_ASC&isShared=&canRealized=&productCategoryEnum=";

            if(maxD != -1)
            {
                int index_MinDays = website.IndexOf("minDays");
                index_MinDays += 8;
                website = website.Insert(index_MinDays, minD.ToString());

                int index_MaxDyas = website.IndexOf("maxDays", index_MinDays);
                index_MaxDyas += 8;
                website = website.Insert(index_MaxDyas, maxD.ToString());
            }

            if(maxM != -1)
            {
                int index_MinMoney = website.IndexOf("minMoney");
                index_MinMoney += 9;
                website = website.Insert(index_MinMoney, minM.ToString());

                int index_MaxMoney = website.IndexOf("maxMoney", index_MinMoney);
                index_MaxMoney += 9;
                website = website.Insert(index_MaxMoney, maxM.ToString());


            }

            if (maxR != -1.0)
            {
                int index_MinRate = website.IndexOf("minRate");
                index_MinRate += 8;
                website = website.Insert(index_MinRate, minR.ToString());

                int index_MaxRate = website.IndexOf("maxRate", index_MinRate);
                index_MaxRate += 8;
                website = website.Insert(index_MaxRate, maxR.ToString());
            }
            


            return website;
        }

    }

}
