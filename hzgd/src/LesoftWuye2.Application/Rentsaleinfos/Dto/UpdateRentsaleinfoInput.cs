using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Rentsaleinfos;
using Abp.Application.Services.Dto;
using LesoftWuye2.Core.Rentsaleinfos;


namespace LesoftWuye2.Application.Rentsaleinfos.Dto
{
    [AutoMap(typeof(Rentsaleinfo))]
    public class UpdateRentsaleinfoInput : EntityDto<long>
    {

        public virtual long RentsaleinfoTypeId { get; set; }

        [Display(Name = "标题")]
        [Required]
        [MaxLength(LesoftWuye2.Core.Rentsaleinfos.Rentsaleinfo.MaxTitleLength)]
        public virtual string Title { get; set; }

        [Required]
        [Display(Name = "内容")]
        public virtual string Content { get; set; }

        [Display(Name = "联系人")]
        [Required]
        [MaxLength(Rentsaleinfo.MaxContactLength)]
        public virtual string Contact { get; set; }

        [Display(Name = "联系电话")]
        [Required]
        [MaxLength(Rentsaleinfo.MaxMobileLength)]
        public virtual string Mobile { get; set; }

        public string Thumbnail { get; set; }
    }
}
