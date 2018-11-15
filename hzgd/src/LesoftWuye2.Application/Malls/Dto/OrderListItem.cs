using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Mall.RefundOrders;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class OrderListItem
    {
        public string ProductName { get; set; }
        public string Thumbnail { get; set; }
        public decimal Amount { get; set; }
        public string OrderNo { get; set; }
        public long OrderId { get; set; }

        public virtual OrderType Type { get; set; }

        public GrouponStatus GrouponStatus { get; set; }

        public virtual OrderStatus Status { get; set; }
        public RefundStatus RefundStatus { get; set; }
        public string StatusText
        {
            get
            {
                switch (Type)
                {
                    case OrderType.Normal:
                        switch (Status)
                        {
                            case OrderStatus.WaitingPay:
                                return "待支付";
                            case OrderStatus.IsReading:
                                return "待发货";
                            case OrderStatus.HasShip:
                                return "待收货";
                            case OrderStatus.Received:
                                return "待评价";
                            case OrderStatus.Done:
                                return "已评价";
                            case OrderStatus.Cancel:
                                if (RefundStatus == RefundStatus.Accept)
                                    return "退款申请通过,退款中";
                                if (RefundStatus == RefundStatus.Done)
                                    return "退款申请通过,退款完成";
                                return "已取消";
                            default:
                                return "未知状态";
                        }
                    case OrderType.Groupon:
                        switch (Status)
                        {
                            case OrderStatus.WaitingPay:
                                return "待支付";
                            case OrderStatus.Grouponing:
                                return "拼团中";
                            case OrderStatus.IsReading:
                                return "拼团成功,待发货";
                            case OrderStatus.HasShip:
                                return "拼团成功,待收货";
                            case OrderStatus.Received:
                                return "待评价";
                            case OrderStatus.Done:
                                return "已评价";
                            case OrderStatus.Cancel:

                                if (GrouponStatus == GrouponStatus.GrouponFail)
                                    return "拼团失败,退款中";
                                if (GrouponStatus == GrouponStatus.GrouponFailAndRefunds)
                                    return "拼团失败,退款完成";

                                if (RefundStatus == RefundStatus.Accept)
                                    return "退款申请通过,退款中";
                                if (RefundStatus == RefundStatus.Done)
                                    return "退款申请通过,退款完成";

                                return "已取消";
                            default:
                                return "未知状态";
                        }
                    default:
                        return "未知状态";
                }
            }
        }
    }
}
