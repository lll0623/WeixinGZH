﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Obs.CodeGenerator;

namespace LesoftWuye2.Core.Products
{
    [Table("ServiceProductSlideImages")]
    [EntityDescription("家政服务轮播图")]
    public class ServiceProductSlideImage : AuditedEntity<long>
    {

        public const int MaxThumbnailLength = 128;

        [Display(Name = "缩略图")]
        [MaxLength(MaxThumbnailLength)]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        [DtoAssign(DtoAssignTargets.All)]
        public virtual int Sort { get; set; }

        public long ProductId { get; set; }

        [ForeignKey("ProductId")]

        public virtual Product Product { get; set; }
    }
}
