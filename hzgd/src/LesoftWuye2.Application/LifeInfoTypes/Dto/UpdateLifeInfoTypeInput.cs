using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.LifeInfoTypes;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.LifeInfoTypes.Dto
{
    [AutoMap(typeof(LifeInfoType))]
    public class UpdateLifeInfoTypeInput : EntityDto<long>
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.LifeInfoTypes.LifeInfoType.MaxNameLength)]
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.LifeInfoTypes.LifeInfoType.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
