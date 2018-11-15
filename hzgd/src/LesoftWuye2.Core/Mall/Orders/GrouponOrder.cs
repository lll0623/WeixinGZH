using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;
using LesoftWuye2.Core.Categories;
using LesoftWuye2.Core.Groupons;
using LesoftWuye2.Core.Mall.Orders;
using LesoftWuye2.Core.Suppliers;
using LesoftWuye2.Core.Products;
using LesoftWuye2.Core.Wuyebase.Members;

namespace LesoftWuye2.Core.Orders
{
    [Table("GrouponOrders")]
    [EntityDescription("团购订单")]
    public class GrouponOrder : AuditedEntity<long>
    {
        public long HeaderId { get; set; }

        [ForeignKey("HeaderId")]

        public virtual Member Header { get; set; }

        public virtual GrouponStatus GrouponStatus { get; set; }

        public long GrouponId { get; set; }

        [ForeignKey("GrouponId")]

        public virtual Groupon Groupon { get; set; }

        [Display(Name = "要求人数")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int RequireCount { get; set; }

        public virtual int JoinCount { get; set; }

        [Display(Name = "团购结束时间")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual DateTime ExpireTime { get; set; }

        public virtual DateTime? GrouponSuccessTime { get; set; }

    }
}
