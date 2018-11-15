using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesoftWuye2.Application.Frontpages.Session
{
    public interface IFrontSession
    {
        /// <summary>
        /// app用户id
        /// </summary>
        string AppMemberId { get; }

        /// <summary>
        /// 小区id
        /// </summary>
        long ProjectId { get; }

        long MemberId { get;}
    }
}
