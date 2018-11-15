using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.Orders
{
    public enum OrderStatus
    {
        /// <summary>
        /// 待支付(订单刚创建)
        /// </summary>
        WaitingPay = 0,

        /// <summary>
        /// 代成团
        /// </summary>
        Grouponing = 1,

        /// <summary>
        /// 已支付/已成团
        /// </summary>
        IsReading = 2,

        /// <summary>
        /// 已发货
        /// </summary>
        HasShip = 3,

        /// <summary>
        /// 已收货
        /// </summary>
        Received = 4,

        /// <summary>
        /// 订单完成(已评价)
        /// </summary>
        Done = 5,

        /// <summary>
        /// 订单被取消
        /// </summary>
        Cancel = 6
    }
}
