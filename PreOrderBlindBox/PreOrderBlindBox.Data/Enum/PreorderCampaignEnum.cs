using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreOrderBlindBox.Data.Enum
{
    public enum PreorderCampaignStatus
    {
        Pending,   // Đang chờ xử lý
        Active,    // Đang hoạt động
        Completed, // Đã hoàn thành
        Canceled   // Đã hủy
    }

    public enum PreorderCampaignType
    {
        TimedPricing,      // Luồng 1: Giá theo thời gian đặt
        BulkOrder          // Luồng 2: Mua chung/gom đơn
    }

}
