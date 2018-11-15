using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Mall.Orders
{
    public enum OrderReceivedType
    {
        /// <summary>
        /// 未收货
        /// </summary>
        None = 0,

        /// <summary>
        /// 用户自己设置收货完成
        /// </summary>
        User = 1,

        /// <summary>
        /// 管理员设置收货完成
        /// </summary>
        Admin=2,

        /// <summary>
        /// 系统设置
        /// </summary>
        System=3

    }
}
