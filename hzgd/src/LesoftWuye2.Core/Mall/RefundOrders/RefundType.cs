using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.RefundOrders
{
    public enum RefundType
    {
        /// <summary>
        /// 我要退款（无需退货）
        /// </summary>
        OlnyMoney = 1,

        /// <summary>
        /// 我要退货
        /// </summary>
        MoneyAndProduct = 2
    }
}
