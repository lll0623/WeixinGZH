using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;

namespace LesoftWuye2.Application.Estateinfos.Dto
{
    [AutoMap(typeof (EstateinfoComment))]
    public class CreateEstateinfoCommentInput
    {

        [Display(Name = "发布人")]
        public virtual string MemberId { get; set; }

        [Display(Name = "发布人")]
        public virtual string MemberName { get; set; }

        [Required]
        [Display(Name = "内容")]
        public virtual string Content { get; set; }

        public long EstateinfoId { get; set; }
    }
}

