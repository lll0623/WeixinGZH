using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.Orders
{
    public enum GrouponStatus
    {
        /// <summary>
        /// 未创建
        /// </summary>
        None = 0,

        /// <summary>
        /// 待成团
        /// </summary>
        Grouponing=1,

        /// <summary>
        /// 已成团
        /// </summary>
        GrouponSuccess=2,

        /// <summary>
        /// 拼团失败
        /// </summary>
        GrouponFail=3,

        /// <summary>
        /// 拼团失败,已退款
        /// </summary>
        GrouponFailAndRefunds = 4,
    }
}
