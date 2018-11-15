using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.FeeServices;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.FeeServices.Dto
{
    [AutoMapFrom(typeof(FeeService))]
    public class FeeServiceItemDto : EntityDto<long>
    {

        [Display(Name = "公告标题")]
        public string Title { get; set; }

        [Display(Name = "公告缩略图")]
        public string Thumbnail { get; set; }

        public List<long> Projects { get; set; }

        public bool AllProjects { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }
    }
}
