using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.LifeInfos;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.LifeInfos.Dto
{
    [AutoMapFrom(typeof(LifeInfo))]
    public class LifeInfoListDto : EntityDto<long>
    {

        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        public DateTime CreationTime { get; set; }
        public bool AllProjects { get; set; }

        public List<string> ProjectNames { get; set; }

        public string TypeName { get; set; }
        [Display(Name = "排序号")]
        public int Sort { get; set; }

    }
}
