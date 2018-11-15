using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Groupons;
using Abp.Application.Services.Dto;


namespace LesoftWuye2.Application.Groupons.Dto
{
    [AutoMap(typeof(Groupon))]
    public class UpdateGrouponInput : EntityDto<long>
    {
        public long ProductId { get; set; }

        public virtual decimal Price { get; set; }

        [Display(Name = "团购说明")]
        [MaxLength(LesoftWuye2.Core.Groupons.Groupon.MaxSummaryLength)]
        public string Summary { get; set; }

        [Display(Name = "要求人数")]
        public int RequireCount { get; set; }

        //[Display(Name = "是否团购中")]
        //public bool IsSale { get; set; }


        [Display(Name = "团购开始时间")]
        public virtual DateTime StartTime { get; set; }


        [Display(Name = "团购结束时间")]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "团购有效天数")]
        public int ValidDay { get; set; }


    }
}
