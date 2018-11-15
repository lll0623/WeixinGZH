using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.RefundOrders
{
    /// <summary>
    /// 退款原因
    /// </summary>
    public enum RefundReason
    {
        /// <summary>
        /// 不想要了
        /// </summary>
        DotWant = 1,

        /// <summary>
        /// 超时未发货
        /// </summary>
        ShipTimeout = 2,

        /// <summary>
        /// 其他原因
        /// </summary>
        Other = 3,

        /// <summary>
        /// 商家发错货
        /// </summary>
        ShipError=4,

        /// <summary>
        /// 商品质量问题
        /// </summary>
        ProductQuality = 5
    }
}
