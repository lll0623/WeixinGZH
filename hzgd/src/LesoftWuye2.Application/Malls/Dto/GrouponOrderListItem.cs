using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Core.Mall.Orders;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class GrouponOrderListItem
    {
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public decimal Amount { get; set; }
        public int RequireCount { get; set; }
        public long OrderId { get; set; }

        public long GrouponOrderId { get; set; }

        public GrouponStatus Status { get; set; }

        public string StatusText
        {
            get
            {
                switch (Status)
                {
                    case GrouponStatus.Grouponing:
                        return "拼团中";
                    case GrouponStatus.GrouponSuccess:
                        return "拼团成功";
                    case GrouponStatus.GrouponFail:
                    case GrouponStatus.GrouponFailAndRefunds:
                        return "拼团失败";
                    default:
                        return "未知状态";
                }
            }
        }
    }
}
