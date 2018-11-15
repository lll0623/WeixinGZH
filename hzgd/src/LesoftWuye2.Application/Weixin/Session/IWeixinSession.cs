using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Weixin.Session
{
    public interface IWeixinSession
    {
        /// <summary>
        /// app用户id
        /// </summary>
        string OpenId { get; }

        /// <summary>
        /// 小区id
        /// </summary>
        long ProjectId { get; }

        long MemberId { get;}

        bool IsBindRoom { get; }

        string MemberName { get; }
    }
}
