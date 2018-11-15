using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using LesoftWuye2.Core.WeixinSubscribes;



namespace LesoftWuye2.Application.WeixinSubscribes.Dto
{
    [AutoMap(typeof(WeixinSubscribe))]
    public class CreateWeixinSubscribeInput
    {

        [Display(Name = "名称")]
        [MaxLength(LesoftWuye2.Core.WeixinSubscribes.WeixinSubscribe.MaxTitleLength)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "链接")]
        [MaxLength(LesoftWuye2.Core.WeixinSubscribes.WeixinSubscribe.MaxUrlLength)]
        public string Url { get; set; }

        [Display(Name = "排序号")]
        public int Sort { get; set; }


        [Display(Name = "缩略图")]
        [Required]
        [MaxLength(LesoftWuye2.Core.WeixinSubscribes.WeixinSubscribe.MaxThumbnailLength)]
        public string Thumbnail { get; set; }


        [Display(Name = "说明")]
        [Required]
        [MaxLength(LesoftWuye2.Core.WeixinSubscribes.WeixinSubscribe.MaxSummarylLength)]
        public string Summary { get; set; }

    }
}
