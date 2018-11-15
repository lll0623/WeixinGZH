using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Mall.RefundOrders;

namespace LesoftWuye2.Application.Orders.Dto
{
   public class OrderItemDto
    {
       public long Id { get; set; }
       public string MemberName { get; set; }
        public string OrderNo { get; set; }

        public OrderType Type { get; set; }
        public OrderStatus Status { get; set; }

        public GrouponStatus GrouponStatus { get; set; }

        public string Contact { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ProductName { get; set; }
        public int Num { get; set; }
        public string Thumbnail { get; set; }
        public decimal Amount { get; set; }
        public string PayChannel { get; set; }
        public string PayTradeNo { get; set; }

        public virtual DateTime? RefundsPayTime { get; set; }
        public virtual string RefundsTradeNo { get; set; }
        public DateTime CreationTime { get; set; }
        public long GrouponOrderId { get; set; }
        public long GrouponId { get; set; }
        public string ClientRemark { get; set; }
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

        public RefundStatus RefundStatus { get; set; }
        public long RefundOrderId { get; set; }

        public string RefundStatusDesc
        {
            get
            {
                switch (RefundStatus)
                {
                    case RefundStatus.None:
                        return "NONE";
                    case RefundStatus.Processing:
                        return "退款申请中";
                    case RefundStatus.Accept:
                        return "退款申请通过，退款中";
                    case RefundStatus.Reject:
                        return "退款申请未通过";
                    case RefundStatus.Done:
                        return "退款完成";
                    default:
                        return "未知";
                }
            }

        }
    }
}
