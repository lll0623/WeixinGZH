using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.Groupons;
using Abp.Application.Services.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace LesoftWuye2.Application.Groupons.Dto
{
    [AutoMapFrom(typeof(Groupon))]
    public class GrouponItemDto : EntityDto<long>
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
        [JsonConverter(typeof(DateFormat))]
        public virtual DateTime StartTime { get; set; }


        [Display(Name = "团购结束时间")]
        [JsonConverter(typeof(DateFormat))]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "团购有效天数")]
        public int ValidDay { get; set; }

        /// <summary>
        /// 日期格式化，格式化 yyyy-MM-dd
        /// </summary>
        public class DateFormat : IsoDateTimeConverter
        {
            public DateFormat()
            {
                base.DateTimeFormat = "yyyy-MM-dd HH:mm";
            }
        }
    }
}
