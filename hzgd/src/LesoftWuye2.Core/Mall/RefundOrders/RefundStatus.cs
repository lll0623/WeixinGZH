using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.RefundOrders
{
    public enum RefundStatus
    {
        /// <summary>
        /// 我要退款（无需退货）
        /// </summary>
        None = 0,

        /// <summary>
        /// 审核中 
        /// </summary>
        Processing = 1,

        /// <summary>
        /// 审核通过，退款中
        /// </summary>
        Accept = 2,

        /// <summary>
        /// 审核不通过
        /// </summary>
        Reject=3,

        /// <summary>
        /// 退款成功
        /// </summary>
        Done=4

    }
}
