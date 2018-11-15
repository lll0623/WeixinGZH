using Abp.AutoMapper;
using LesoftWuye2.Core.Mall.RefundOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Weixin.Dto
{
    [AutoMapFrom(typeof(RefundOrder))]
    public class RefundViewDto
    {
        public long Id { get; set; }

        public long OrderId { get; set; }

        public string OrderNo { get; set; }

        public virtual decimal Amount { get; set; }

        public long ProductId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Thumbnail { get; set; }

        public virtual int Num { get; set; }

        public RefundType Type { get; set; }

        public RefundReason Reason { get; set; }

        public decimal RefundMoney { get; set; }

        public string Remark { get; set; }

        public RefundStatus Status { get; set; }

        public DateTime CreationTime { get; set; }

        public string TypeDesc {
            get {

                switch (Type)
                {
                    case RefundType.OlnyMoney:
                        return "仅退款";
                    case RefundType.MoneyAndProduct:
                        return "退货";
                    default:
                        return "未知";
                }

            }
        }

        public string StatusDesc {
            get {
                switch (Status)
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
