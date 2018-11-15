using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Core.Estateinfos
{
    public enum EstateinfoMode
    {
        /// <summary>
        /// 出租
        /// </summary>
        Rent = 1,

        /// <summary>
        /// 转让
        /// </summary>
        Transfer = 2,

        /// <summary>
        /// 求租
        /// </summary>
        ForRent = 3,

        /// <summary>
        /// 买卖
        /// </summary>
        Purchase = 4,

            None = 5
    }
}
