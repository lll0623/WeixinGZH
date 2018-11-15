using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Propertys;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Propertys.Dto
{
    [AutoMap(typeof(Property))]
    public class UpdatePropertyInput : EntityDto<long>
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.Propertys.Property.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.Propertys.Property.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        public long PropertyCityId { get; set; }

        public long PropertyTypeId { get; set; }

        [Display(Name = "内容")]
        [Required]
        public string Content { get; set; }
    }
}
