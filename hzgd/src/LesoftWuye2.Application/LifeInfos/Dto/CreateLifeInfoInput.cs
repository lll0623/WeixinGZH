using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.LifeInfos;



namespace LesoftWuye2.Application.LifeInfos.Dto
{
    [AutoMap(typeof(LifeInfo))]
    public class CreateLifeInfoInput
    {

        [Display(Name = "标题")]
        [MaxLength(LesoftWuye2.Core.LifeInfos.LifeInfo.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "缩略图")]
        [MaxLength(LesoftWuye2.Core.LifeInfos.LifeInfo.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        [Display(Name = "标题")]
        [MaxLength(LesoftWuye2.Core.LifeInfos.LifeInfo.MaxSummaryLength)]
        public string Summary { get; set; }

        public long LifeInfoTypeId { get; set; }

        public List<long> Projects { get; set; }

        public bool AllProjects { get; set; }

        [Display(Name = "内容")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
