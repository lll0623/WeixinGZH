using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Groupons;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Groupons.Dto
{
    [AutoMapFrom(typeof(Groupon))]
    public class GrouponListDto : EntityDto<long>
    {

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "缩略图")]
        public string Thumbnail { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }

        [Display(Name = "要求人数")]
        public int RequireCount { get; set; }

        [Display(Name = "是否团购中")]
        public bool IsSale { get; set; }


        [Display(Name = "团购开始时间")]
        public virtual DateTime StartTime { get; set; }


        [Display(Name = "团购结束时间")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "团购有效天数")]
        public int ValidDay { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual decimal Price { get; set; }

    }
}
