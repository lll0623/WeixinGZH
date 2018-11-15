using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Activitys;
using Abp.Application.Services.Dto;
using LesoftWuye2.Core.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace LesoftWuye2.Application.Activitys.Dto
{


[AutoMapFrom(typeof(Activity))]
    public class ActivityItemDto : EntityDto<long>
    {

        [Display(Name = "活动标题")]
        public string Title { get; set; }

        [Display(Name = "活动缩略图")]
        public string Thumbnail { get; set; }

        public List<long> Projects { get; set; }

        public bool AllProjects { get; set; }

        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }

        [JsonConverter(typeof(DateFormat))]
        public virtual DateTime Expireday { get; set; }

        public virtual int AllowCount { get; set; }

        public virtual int JoinCount { get; set; }
    }
}
