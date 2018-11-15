using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Wuyebase.Members;
using Obs.CodeGenerator;

namespace LesoftWuye2.Application.Malls.Dto
{
    public class GrouponOrderInfo
    {

        public long Id { get; set; }

        public long GrouponId { get; set; }

        public virtual GrouponStatus GrouponStatus { get; set; }
     
        public virtual int RequireCount { get; set; }

        public virtual int JoinCount { get; set; }

        public virtual DateTime ExpireTime { get; set; }

        public virtual string ProductName { get; set; }
        public virtual string Summary { get; set; }

        public virtual decimal Price { get; set; }

        public string Thumbnail { get; set; }

        public string Unit { get; set; }

        public List<GrouponOrderMemberInfo> JoinMembers { get; set; }

    }

    public class GrouponOrderMemberInfo
    {

        public string Name { get; set; }

        public string Thumbnail { get; set; }
        public virtual bool IsHeader { get; set; }

        public DateTime JoinTime { get; set; }
        public long Id { get; set; }
    }
}
