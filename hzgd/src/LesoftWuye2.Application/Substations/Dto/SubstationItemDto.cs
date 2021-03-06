﻿using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Substations;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Substations.Dto
{
    [AutoMapFrom(typeof(Substation))]
    public class SubstationItemDto : EntityDto<long>
    {

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "链接")]
        public string Url { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        [Display(Name = "缩略图")]

        public string Thumbnail { get; set; }
    }
}
