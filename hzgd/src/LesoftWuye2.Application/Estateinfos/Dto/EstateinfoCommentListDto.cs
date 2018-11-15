using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Estateinfos.Dto
{
    [AutoMapFrom(typeof(EstateinfoComment))]
    public class EstateinfoCommentListDto : EntityDto<long>
    {
        public virtual string MemberName { get; set; }

        [Display(Name = "内容")]
        public virtual string Content { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
