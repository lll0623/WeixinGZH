using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.LifeInfos;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.LifeInfos.Dto
{
    [AutoMapFrom(typeof(LifeInfo))]
    public class LifeInfoItemDto : EntityDto<long>
    {

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }
        public string Content { get; set; }
        public List<long> Projects { get; set; }
        public string LifeInfoTypeId { get; set; }

        public string Summary { get; set; }
        public bool AllProjects { get; set; }

        public DateTime CreationTime { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
