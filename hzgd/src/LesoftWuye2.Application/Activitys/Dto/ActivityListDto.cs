using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Activitys;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Activitys.Dto
{
    [AutoMapFrom(typeof(Activity))]
    public class ActivityListDto : EntityDto<long>
    {

        [Display(Name = "活动标题")]
        public string Title { get; set; }

        [Display(Name = "活动缩略图")]
        public string Thumbnail { get; set; }

        public DateTime CreationTime { get; set; }

        public bool AllProjects { get; set; }

        public List<string> ProjectNames { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }

        public virtual DateTime Expireday { get; set; }

        public virtual int AllowCount { get; set; }

        public virtual int JoinCount { get; set; }

        public string CreationTimeFormat
        {
            get { return CreationTime.ToString("yyyy-MM-dd"); }
        }
    }
}
