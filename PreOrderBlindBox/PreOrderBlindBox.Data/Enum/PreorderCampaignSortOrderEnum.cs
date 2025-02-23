using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Enum
{
    public enum PreorderCampaignSortOrderEnum
    {
        AZ,              // Sắp xếp theo BlindBox.Name từ A đến Z
        ZA,              // Sắp xếp theo BlindBox.Name từ Z đến A
        Newest,          // Sắp xếp theo CreatedDate giảm dần (Mới nhất)
        BestSelling,     // Sắp xếp theo tổng số lượng bán (Bán chạy)
        PriceAscending,  // Sắp xếp theo giá tăng (dựa trên giá cao nhất của milestone)
        PriceDescending  // Sắp xếp theo giá giảm (dựa trên giá cao nhất của milestone)
    }
}
