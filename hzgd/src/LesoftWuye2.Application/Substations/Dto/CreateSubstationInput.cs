using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Substations;



namespace LesoftWuye2.Application.Substations.Dto
{
    [AutoMap(typeof(Substation))]
    public class CreateSubstationInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.Substations.Substation.MaxNameLength)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "链接")]
        [MaxLength(LesoftWuye2.Core.Substations.Substation.MaxUrlLength)]
        [Required]
        public string Url { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


        [Display(Name = "缩略图")]

        [MaxLength(LesoftWuye2.Core.Substations.Substation.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

    }
}
