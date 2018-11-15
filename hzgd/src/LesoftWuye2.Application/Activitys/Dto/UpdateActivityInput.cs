using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Activitys;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Activitys.Dto
{
    [AutoMap(typeof(Activity))]
    public class UpdateActivityInput : EntityDto<long>
    {

        [Display(Name = "活动标题")]
        [MaxLength(LesoftWuye2.Core.Activitys.Activity.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "活动缩略图")]
        [MaxLength(LesoftWuye2.Core.Activitys.Activity.MaxThumbnailLength)]
        public string Thumbnail { get; set; }

        public List<long> Projects { get; set; }

        public bool AllProjects { get; set; }

        [Display(Name = "内容")]
        [Required]
        public string Content { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }

        public virtual DateTime Expireday { get; set; }

        public virtual int AllowCount { get; set; }
    }
}
