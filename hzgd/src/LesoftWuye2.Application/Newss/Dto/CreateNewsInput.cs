using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Newss;



namespace LesoftWuye2.Application.Newss.Dto
{
    [AutoMap(typeof(News))]
    public class CreateNewsInput
    {

        [Display(Name = "新闻标题")]
        [MaxLength(LesoftWuye2.Core.Newss.News.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "新闻缩略图")]
        [MaxLength(LesoftWuye2.Core.Newss.News.MaxThumbnailLength)]
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
