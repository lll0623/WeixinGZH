using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Estateinfos;
using Abp.Application.Services.Dto;
using LesoftWuye2.Core.Estateinfos;


namespace LesoftWuye2.Application.Estateinfos.Dto
{
    [AutoMap(typeof(Estateinfo))]
    public class UpdateEstateinfoInput : EntityDto<long>
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
    }
}
