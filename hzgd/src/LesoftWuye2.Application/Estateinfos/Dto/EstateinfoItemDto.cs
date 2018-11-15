using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using LesoftWuye2.Core.LifeInfos;
using LesoftWuye2.Core.Utils;
using Newtonsoft.Json;

namespace LesoftWuye2.Application.Estateinfos.Dto
{
    [AutoMapFrom(typeof(Estateinfo))]
    public class EstateinfoItemDto : EntityDto<long>
    {

        [Display(Name = "发布人")]
        [Required]
        public virtual string MemberId { get; set; }

        public virtual EstateinfoMode Mode { get; set; }

        [Display(Name = "标题")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Estateinfos.Estateinfo.MaxTitleLength)]
        public virtual string Title { get; set; }

        [Required]
        [Display(Name = "内容")]
        public virtual string Content { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(Estateinfo.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(Estateinfo.MaxMobileLength)]
        public virtual string Mobile { get; set; }

        [Display(Name = "是否显示")]
        public virtual bool IsShow { get; set; }

        [Display(Name = "是否上架")]
        public virtual bool IsSale { get; set; }

        [JsonConverter(typeof(DateTimeFormat))]
        public DateTime CreationTime { get; set; }

        public List<string> Images { get; set; }

        public List<EstateinfoCommentListDto> Comments { get;set; }
    }
}
