using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.LifeInfoTypes;



namespace LesoftWuye2.Application.LifeInfoTypes.Dto
{
    [AutoMap(typeof(LifeInfoType))]
    public class CreateLifeInfoTypeInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.LifeInfoTypes.LifeInfoType.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "缩略图")]
        [Required]
        [MaxLength(LesoftWuye2.Core.LifeInfoTypes.LifeInfoType.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


    }
}
