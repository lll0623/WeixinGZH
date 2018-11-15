using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.FeeServices;



namespace LesoftWuye2.Application.FeeServices.Dto
{
    [AutoMap(typeof(FeeService))]
    public class CreateFeeServiceInput
    {

        [Display(Name = "公告标题")]
        [MaxLength(LesoftWuye2.Core.FeeServices.FeeService.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "公告缩略图")]
        [MaxLength(LesoftWuye2.Core.FeeServices.FeeService.MaxThumbnailLength)]
        public string Thumbnail { get; set; }


        public List<long> Projects { get; set; }

        public bool AllProjects { get; set; }

        [Display(Name = "内容")]
        [Required]
        public string Content { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
